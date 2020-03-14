using Dot.Log;
using Dot.Net.Message;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;

namespace Dot.Net.Client
{
    public enum ClientNetSessionState
    {
        NotUnable = 0,
        Connecting,
        Normal,
        ConnectedFailed,
        Disconnected,
    }

    public delegate void OnMessageReceived(int messageID, byte[] datas);
    public delegate void OnMessageError(MessageErrorCode errorCode);

    public interface IClientNetDataReceiver
    {
        IMessageCrypto Crypto { get; set; }
        IMessageCompressor Compressor { get; set; }
        OnMessageReceived ReceivedCallback { get; set; }
        OnMessageError ErrorCallback { get; set; }

        void OnDataReceived(byte[] datas, int size);
        void Reset();
    }

    public interface IClientNetStateListener
    {
        void OnStateChanged(ClientNetSessionState oldState, ClientNetSessionState newState);
    }

    public class ClientNetSession
    {
        private static readonly string IP_ADDRESS_REGEX = @"^((2(5[0-5]|[0-4]\d))|[0-1]?\d{1,2})(\.((2(5[0-5]|[0-4]\d))|[0-1]?\d{1,2})){3}$";

        private string ip = null;
        private int port = -1;
        public string IP { get => ip; }
        public int Port { get => port; }
        public string SessionAddress { get => $"{ip}:{port}"; }

        private Socket socket = null;
        private SocketAsyncEventArgs generalAsyncEvent = null;
        private SocketAsyncEventArgs receiveAsyncEvent = null;

        private object sessionStateLock = new object();
        private ClientNetSessionState state = ClientNetSessionState.NotUnable;
        private ClientNetSessionState State 
        {
            get
            {
                lock(sessionStateLock)
                {
                    return state;
                }
            }
            set
            {
                lock(sessionStateLock)
                {
                    if(state!=value)
                    {
                        ClientNetSessionState oldState = state;
                        state = value;

                        stateListener?.OnStateChanged(oldState, state);
                    }
                }
            }
         }
        private IClientNetStateListener stateListener = null;

        private object sendingLock = new object();
        private bool isSending = false;
        private List<byte> waitingSendBytes = new List<byte>();

        private object receiverLock = new object();
        private IClientNetDataReceiver dataReceiver = null;


        public ClientNetSession(IClientNetDataReceiver receiver,IClientNetStateListener listener)
        {
            dataReceiver = receiver;
            stateListener = listener;
        }

        public bool IsConnected()
        {
            return State == ClientNetSessionState.Normal;
        }

        public bool Connect(string ipAddress)
        {
            if (string.IsNullOrEmpty(ipAddress))
            {
                LogUtil.LogError(ClientNetConst.LOGGER_NAME, "ClientNetSession::Connect->ipAddress is empty");
                return false;
            }

            string[] splitStrArr = ipAddress.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
            if (splitStrArr == null || splitStrArr.Length != 2)
            {
                LogUtil.LogError(ClientNetConst.LOGGER_NAME, $"ClientNetSession::Connect->The lenght of ip is not correct.ipAddress = {ipAddress}");
                return false;
            }

            if (!int.TryParse(splitStrArr[1], out int port) || port<=0)
            {
                LogUtil.LogError(ClientNetConst.LOGGER_NAME, $"ClientNetSession::Connect->the port is not a int.port = {splitStrArr[1]}");
                return false;
            }

            return Connect(splitStrArr[0], port);
        }

        public bool Connect(string ip,int port)
        {
            if(string.IsNullOrEmpty(ip) || port<=0)
            {
                LogUtil.LogError(ClientNetConst.LOGGER_NAME, $"ClientNetSession::Connect->The ip is empty or the port is not correct.ip = {ip},port={port}");
                return false;
            }

            if(!Regex.IsMatch(ip,IP_ADDRESS_REGEX))
            {
                LogUtil.LogError(ClientNetConst.LOGGER_NAME, $"ClientNetSession::Connect->The format of ip is not correct.ip = {ip}");
                return false;
            }

            if(socket == null)
            {
                try
                {
                    socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                    this.ip = ip;
                    this.port = port;

                    State = ClientNetSessionState.Connecting;

                    IPAddress ipAddress = IPAddress.Parse(ip);
                    generalAsyncEvent = new SocketAsyncEventArgs()
                    {
                        RemoteEndPoint = new IPEndPoint(ipAddress, port),
                        UserToken = socket,
                    };
                    generalAsyncEvent.Completed += OnHandleSocketEvent;

                    socket.ConnectAsync(generalAsyncEvent);
                    return true;
                }catch(Exception e)
                {
                    generalAsyncEvent = null;
                    State = ClientNetSessionState.ConnectedFailed;
                    socket = null;
                    return false;
                }
            }else
            {

            }
            return false;
        }

        public bool Reconnect()
        {
            if(string.IsNullOrEmpty(ip) || port<=0)
            {
                LogUtil.LogError(ClientNetConst.LOGGER_NAME, $"ClientNetSession::Reconnect->the ip is empty,or the port is not correct.");
                return false;
            }

            if(State >= ClientNetSessionState.ConnectedFailed)
            {
                return Connect(ip, port);
            }

            LogUtil.LogError(ClientNetConst.LOGGER_NAME, $"ClientNetSession::Reconnect->the net is connected or the net is connecting.{State}");

            return false;
        }

        public void Disconnect()
        {
            if(generalAsyncEvent!=null)
            {
                generalAsyncEvent.Completed -= OnHandleSocketEvent;
                generalAsyncEvent = null;
            }
            if(receiveAsyncEvent !=null)
            {
                receiveAsyncEvent.Completed -= OnHandleSocketEvent;
                receiveAsyncEvent = null;
            }

            lock(sendingLock)
            {
                waitingSendBytes.Clear();
            }

            if(socket!=null)
            {
                if(socket.Connected)
                {
                    try
                    {
                        socket.Shutdown(SocketShutdown.Both);
                    }catch(Exception e)
                    {

                    }finally
                    {
                        socket.Close();
                        socket = null;
                    }
                }else
                {
                    socket.Close();
                    socket = null;
                }
            }

            State = ClientNetSessionState.Disconnected;
        }

        public void DoLateUpdate()
        {
            if(State != ClientNetSessionState.Normal)
            {
                return;
            }
            lock(sendingLock)
            {
                if(waitingSendBytes.Count>0 && !isSending)
                {
                    try
                    {
                        generalAsyncEvent.SetBuffer(waitingSendBytes.ToArray(), 0, waitingSendBytes.Count);
                        waitingSendBytes.Clear();
                        if(!socket.SendAsync(generalAsyncEvent))
                        {
                            Disconnect();
                        }else
                        {
                            isSending = true;
                        }
                    }catch(Exception e)
                    {
                        LogUtil.LogError(ClientNetConst.LOGGER_NAME, $"ClientNetSession::DoLateUpdate->e = {e.Message}");
                        Disconnect();
                    }
                }
            }
        }

        public void Send(byte[] datas)
        {
            if(State != ClientNetSessionState.Normal)
            {
                return;
            }
            lock(sendingLock)
            {
                waitingSendBytes.AddRange(datas);
            }
        }

       private void Receive()
        {
            try
            {
                if(!socket.ReceiveAsync(receiveAsyncEvent))
                {
                    Disconnect();
                }
            }catch(Exception e)
            {
                Disconnect();
            }
        }

        private void OnHandleSocketEvent(object sender,SocketAsyncEventArgs socketEvent)
        {
            switch (socketEvent.LastOperation)
            {
                case SocketAsyncOperation.Connect:
                    ProcessConnect(socketEvent);
                    break;
                case SocketAsyncOperation.Send:
                    ProcessSend(socketEvent);
                    break;
                case SocketAsyncOperation.Receive:
                    ProcessReceive(socketEvent);
                    break;
                case SocketAsyncOperation.Disconnect:
                    ProcessDisconnect(socketEvent);
                    break;
                default:
                    LogUtil.LogWarning(ClientNetConst.LOGGER_NAME, $"ClientNetSession::OnHandleSocketEvent->received unkown event.opration = {socketEvent.LastOperation}");
                    break;
            }
        }

        private void ProcessConnect(SocketAsyncEventArgs socketEvent)
        {
            if(socketEvent.SocketError == SocketError.Success)
            {
                State = ClientNetSessionState.Normal;

                receiveAsyncEvent = new SocketAsyncEventArgs();
                receiveAsyncEvent.SetBuffer(new byte[4096], 0, 4096);
                receiveAsyncEvent.Completed += OnHandleSocketEvent;

                Receive();
            }else
            {
                Disconnect();
            }
        }

        private void ProcessSend(SocketAsyncEventArgs socketEvent)
        {
            if(socketEvent.SocketError == SocketError.Success)
            {
                lock(sendingLock)
                {
                    isSending = false;
                }
            }else
            {
                Disconnect();
            }
        }

        private void ProcessReceive(SocketAsyncEventArgs socketEvent)
        {
            if(socketEvent.SocketError == SocketError.Success)
            {
                if(socketEvent.BytesTransferred>0)
                {
                    lock(receiverLock)
                    {
                        dataReceiver.OnDataReceived(socketEvent.Buffer, socketEvent.BytesTransferred);
                    }
                    Receive();
                    return;
                }
            }
            Disconnect();
        }

        private void ProcessDisconnect(SocketAsyncEventArgs socketEvent)
        {
            Disconnect();
        }

    }

}

using Dot.Net.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Dot.Net.Client
{
    public enum ClientNetworkState
    {
        NotUnable = 0,
        Connecting,
        Normal,
        ConnectedFailed,
        Disconnected,
    }

    public delegate void ClientNetworkStateChanged(ClientNetworkState oldState, ClientNetworkState newState);

    public class ClientNetworkSession
    {
        private static readonly string IP_ADDRESS_REGEX = @"^((2(5[0-5]|[0-4]\d))|[0-1]?\d{1,2})(\.((2(5[0-5]|[0-4]\d))|[0-1]?\d{1,2})){3}$";

        private string ip = null;
        private int port = -1;
        public string IP { get => ip; }
        public int Port { get => port; }
        public string IPPort { get => $"{ip}:{port}"; }

        private Socket clientSocket = null;
        private SocketAsyncEventArgs generalAsyncArgs = null;
        private SocketAsyncEventArgs receiveAsyncArgs = null;

        private object networkStateLock = new object();
        private ClientNetworkState networkState = ClientNetworkState.NotUnable;
        public ClientNetworkState State { get => networkState; }

        private object isSendingLock = new object();
        private bool isSending = false;

        private object receiveLock = new object();
        private NetMessageReceiver messageReceiver = null;

        public bool IsConnected { get => clientSocket != null && clientSocket.Connected; }

        public ClientNetworkSession(NetMessageReceiver receiver)
        {
            messageReceiver = receiver;
        }

        public bool Connect(string ipPort)
        {
            if(string.IsNullOrEmpty(ipPort))
            {
                return false;
            }

            string[] splitStrArr = ipPort.Split(new char[] { ':' },StringSplitOptions.RemoveEmptyEntries);
            if(splitStrArr == null || splitStrArr.Length != 2)
            {
                return false;
            }

            if(!int.TryParse(splitStrArr[1],out int port))
            {
                return false;
            }

            return Connect(splitStrArr[0], port);
        }

        public bool Connect(string ipAddress, int port)
        {
            if(string.IsNullOrEmpty(ipAddress) || port<=0)
            {
                return false;
            }

            if(!Regex.IsMatch(ipAddress,IP_ADDRESS_REGEX))
            {
                return false;
            }

            if(clientSocket == null)
            {
                try
                {
                    clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp); 
                }catch(Exception e)
                {
                    return false;
                }

                this.ip = ipAddress;
                this.port = port;
                SetNetworkState(ClientNetworkState.Connecting);

                IPAddress ip = IPAddress.Parse(ipAddress);
                generalAsyncArgs = new SocketAsyncEventArgs();
                generalAsyncArgs.RemoteEndPoint = new IPEndPoint(ip, port);
                generalAsyncArgs.UserToken = clientSocket;
                generalAsyncArgs.Completed += OnSocketEventArgsCompleted;

                clientSocket.ConnectAsync(generalAsyncArgs);
                return true;
            }
            return false;
        }

        public bool Reconnect()
        {
            if (networkState > ClientNetworkState.ConnectedFailed)
            {
                Connect(ip, port);
                return true;
            }

            return false;
        }

        public void Disconnect()
        {
            if(generalAsyncArgs != null)
            {
                generalAsyncArgs.Completed -= OnSocketEventArgsCompleted;
            }
            if(receiveAsyncArgs !=null)
            {
                receiveAsyncArgs.Completed -= OnSocketEventArgsCompleted;
            }

            SetNetworkState(ClientNetworkState.Disconnected);

            if(clientSocket!=null)
            {
                if(clientSocket.Connected)
                {
                    try
                    {
                        clientSocket.Shutdown(SocketShutdown.Both);
                    }catch
                    {

                    }finally
                    {
                        clientSocket.Close();
                        clientSocket = null;
                    }
                }else
                {
                    clientSocket.Close();
                    clientSocket = null;
                }
            }
        }

        public void DoUpdate(float deltaTime)
        {

        }

        public void DoLateUpdate()
        {

        }

        public void Send()
        {

        }

        private void Receive()
        {
            try
            {
                if (!clientSocket.ReceiveAsync(receiveAsyncArgs))
                {
                    DisconnectForError();
                }
            }catch
            {
                DisconnectForError();
            }
        }


        private void OnSocketEventArgsCompleted(object sender, SocketAsyncEventArgs e)
        {
            switch(e.LastOperation)
            {
                case SocketAsyncOperation.Connect:
                    ProcessConnect(e);
                    break;
                case SocketAsyncOperation.Send:
                    ProcessSend(e);
                    break;
                case SocketAsyncOperation.Receive:
                    ProcessReceive(e);
                    break;
                case SocketAsyncOperation.Disconnect:
                    ProcessDisconnect(e);
                    break;
                default:

                    break;
            }
        }

        private void ProcessConnect(SocketAsyncEventArgs e)
        {
            if(e.SocketError == SocketError.Success)
            {
                SetNetworkState(ClientNetworkState.Normal);

                receiveAsyncArgs = new SocketAsyncEventArgs();
                receiveAsyncArgs.SetBuffer(new byte[4096], 0, 4096);
                receiveAsyncArgs.Completed += OnSocketEventArgsCompleted;

                Receive();
            }else
            {

            }
        }

        private void ProcessSend(SocketAsyncEventArgs e)
        {
            if(e.SocketError == SocketError.Success)
            {
                lock(isSendingLock)
                {
                    isSending = false;
                }
            }else
            {
                DisconnectForError();
            }
        }

        private void ProcessReceive(SocketAsyncEventArgs e)
        {
            if(e.SocketError == SocketError.Success)
            {
                if(e.BytesTransferred>0)
                {
                    lock(receiveLock)
                    {
                        messageReceiver.OnDataReceived(e.Buffer, e.BytesTransferred);
                    }
                    Receive();
                }else
                {
                    DisconnectForError();
                }
            }else
            {
                DisconnectForError();
            }
        }

        private void ProcessDisconnect(SocketAsyncEventArgs e)
        {
            Disconnect();
        }

        private void DisconnectForError()
        {

        }

        private void SetNetworkState(ClientNetworkState state)
        {
            lock(networkStateLock)
            {
                if(networkState!=state)
                {
                    networkState = state;
                }
            }
        }


    }
}

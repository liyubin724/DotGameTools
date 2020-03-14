using Dot.Net.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dot.Net.Client
{
    public class ClientNet : IClientNetStateListener
    {
        private IMessageWriter messageWriter = null;
        private IClientNetDataReceiver dataReceiver = null;

        private ClientNetSession netSession = null;

        public ClientNet(IMessageWriter writer,IClientNetDataReceiver receiver)
        {
            messageWriter = writer;
            dataReceiver = receiver;

            netSession = new ClientNetSession(receiver, this);
        }

        public void Connect(string ip,int port)
        {
            netSession.Connect(ip, port);
        }

        public void Reconnect()
        {
            netSession.Reconnect();
        }

        public void Disconnect()
        {
            netSession.Disconnect();
        }

        public void OnStateChanged(ClientNetSessionState oldState, ClientNetSessionState newState)
        {
            throw new System.NotImplementedException();
        }
    }
}

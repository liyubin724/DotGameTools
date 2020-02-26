using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dot.Net.Message
{

    public class NetMessageHandler
    {
        public NetMessageHandler()
        {

        }

        public void OnMessageHandle(
            int messageID,
            NetMessageCompressionType compressionType,
            NetMessageCryptoType cryptoType,
            NetMessagePattern pattern,byte[] datas)
        {

        }

        public void OnMessageHandle(int messageID)
        {

        }

        public void OnMessageError(NetMessageError error)
        {

        }
    }
}

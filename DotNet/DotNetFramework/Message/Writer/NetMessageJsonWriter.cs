using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dot.Net.Message.Writer
{
    public class NetMessageJsonWriter : ANetMessageWriter
    {
        public NetMessageJsonWriter(NetMessagePattern pattern, NetMessageCompressor compressor, NetMessageCrypto crypto) : base(pattern, compressor, crypto)
        {
        }

        public NetMessageJsonWriter(NetMessagePattern pattern, NetMessageCompressor compressor, NetMessageCrypto crypto, string secretKey) : base(pattern, compressor, crypto, secretKey)
        {
        }
    }
}

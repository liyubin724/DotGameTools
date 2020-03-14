using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dot.Net.Message
{
    public enum MessageWriterType : byte
    {
        None = 0,
        Json,
        ProtoBuf,
    }

    public interface IMessageWriter
    {
        IMessageCrypto Crypto { get; set; }
        IMessageCompressor Compressor { get; set; }

        byte[] EncodeMessage<T>(int messageID, T message);
        byte[] EncodeMessage<T>(int messageID, T message,bool isCrypto,bool isCompress);
        byte[] EncodeMessage(int messageID);
        byte[] EncodeData(int messageID, byte[] datas);
        byte[] EncodeData(int messageID, byte[] datas, bool isCrypto, bool isCompress);
        byte[] EncodeMessage<T>(T message);

        void Reset();
    }
}

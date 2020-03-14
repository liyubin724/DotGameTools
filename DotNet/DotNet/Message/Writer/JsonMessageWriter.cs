using Newtonsoft.Json;
using System.Text;

namespace Dot.Net.Message.Writer
{
    public class JsonMessageWriter : AMessageWriter
    {
        public JsonMessageWriter() : base(MessageWriterType.Json)
        {
        }

        public JsonMessageWriter(IMessageCompressor compressor, IMessageCrypto crypto) : base(MessageWriterType.Json, compressor, crypto)
        {
        }

        public override byte[] EncodeMessage<T>(T message)
        {
            string json = JsonConvert.SerializeObject(message);
            if(string.IsNullOrEmpty(json))
            {
                return null;
            }else
            {
                return UTF8Encoding.Default.GetBytes(json);
            }
        }
    }
}

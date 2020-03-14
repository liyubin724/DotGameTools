using SnappySharp = Snappy.Sharp.Snappy;

namespace Dot.Net.Message.Compressor
{
    public class SnappyMessageCompressor : IMessageCompressor
    {
        public byte[] Compress(byte[] bytes)
        {
            return SnappySharp.Compress(bytes);
        }

        public byte[] Uncompress(byte[] bytes)
        {
            return SnappySharp.Uncompress(bytes);
        }

        public MessageCompressorType GetCompressorType()
        {
            return MessageCompressorType.Snappy;
        }
    }
}

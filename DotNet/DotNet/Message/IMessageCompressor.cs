namespace Dot.Net.Message
{
    public enum MessageCompressorType : byte
    {
        Uncompressed = 0,
        Snappy,
    }

    public interface IMessageCompressor
    {
        byte[] Compress(byte[] bytes);
        byte[] Uncompress(byte[] bytes);

        MessageCompressorType GetCompressorType();
    }
}

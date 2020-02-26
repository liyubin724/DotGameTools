using System.Collections.Generic;

namespace Dot.Net.Message
{
    public enum NetMessageCompressionType : byte
    {
        Uncompressed = 0,
    }

    public interface INetMessageCompressor
    {
        byte[] Compress(byte[] bytes);
        byte[] Decompress(byte[] bytes);
    }

    public class NetMessageCompressor
    {
        private Dictionary<NetMessageCompressionType, INetMessageCompressor> compressorDic = new Dictionary<NetMessageCompressionType, INetMessageCompressor>();
        public NetMessageCompressor()
        {
        }

        public byte[] Compress(NetMessageCompressionType compressionType, byte[] bytes)
        {
            if(compressionType == NetMessageCompressionType.Uncompressed)
            {
                return bytes;
            }

            INetMessageCompressor compressor = GetCompressor(compressionType);
            if(compressor!=null)
            {
                return compressor.Compress(bytes);
            }
            return null;
        }

        public byte[] Decompress(NetMessageCompressionType compressionType,byte[] bytes)
        {
            if (compressionType == NetMessageCompressionType.Uncompressed)
            {
                return bytes;
            }

            INetMessageCompressor compressor = GetCompressor(compressionType);
            if (compressor != null)
            {
                return compressor.Decompress(bytes);
            }
            return null;
        }

        private INetMessageCompressor GetCompressor(NetMessageCompressionType compressionType)
        {
            if(compressorDic.TryGetValue(compressionType,out INetMessageCompressor compressor))
            {
                return compressor;
            }

            return null;
        }
    }
}

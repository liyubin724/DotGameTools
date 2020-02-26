using System.Collections.Generic;

namespace Dot.Net.Message
{
    public enum NetMessageCryptoType : byte
    {
        NoCrypto = 0,
    }

    public interface INetMessageCrypto
    {
        byte[] Encode(string secretKey, byte[] bytes);
        byte[] Decode(string secretKey, byte[] bytes);
    }

    public class NetMessageCrypto
    {
        private Dictionary<NetMessageCryptoType, INetMessageCrypto> cryptoDic = new Dictionary<NetMessageCryptoType, INetMessageCrypto>();

        public NetMessageCrypto()
        {

        }

        public byte[] Encode(NetMessageCryptoType cryptoType, string secretKey,byte[] bytes)
        {
            if(cryptoType == NetMessageCryptoType.NoCrypto)
            {
                return bytes;
            }
            INetMessageCrypto crypto = GetCrypto(cryptoType);
            if(crypto!=null)
            {
                return crypto.Encode(secretKey, bytes);
            }
            return null;
        }

        public byte[] Decode(NetMessageCryptoType cryptoType, string secretKey, byte[] bytes)
        {
            if (cryptoType == NetMessageCryptoType.NoCrypto)
            {
                return bytes;
            }
            INetMessageCrypto crypto = GetCrypto(cryptoType);
            if (crypto != null)
            {
                return crypto.Decode(secretKey, bytes);
            }
            return null;
        }

        private INetMessageCrypto GetCrypto(NetMessageCryptoType cryptoType)
        {
            if(cryptoDic.TryGetValue(cryptoType,out INetMessageCrypto crypto))
            {
                return crypto;
            }

            return null;
        }
    }
}

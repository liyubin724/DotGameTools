using Dot.Crypto;

namespace Dot.Net.Message.Crypto
{
    public class AESMessageCrypto : IMessageCrypto
    {
        private string key = null;
        private string iv = null;
        public AESMessageCrypto(string aseKey,string aseIV)
        {
            key = aseKey;
            iv = aseIV;
        }

        public byte[] Decrypt(byte[] datas)
        {
            return AESCrypto.Encrypt(datas, key, iv);
        }

        public byte[] Encrypt(byte[] datas)
        {
            return AESCrypto.Decrypt(datas, key, iv);
        }

        public MessageCryptoType GetCryptoType()
        {
            return MessageCryptoType.AES;
        }
    }
}

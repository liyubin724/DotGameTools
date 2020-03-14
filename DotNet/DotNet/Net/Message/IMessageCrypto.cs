namespace Dot.Net.Message
{
    public enum MessageCryptoType : byte
    {
        Nocrypto = 0,
        AES,
    }

    public interface IMessageCrypto
    {
        byte[] Encrypt(byte[] datas);
        byte[] Decrypt(byte[] datas);
        MessageCryptoType GetCryptoType();
    }
}
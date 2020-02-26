using System;
using System.Net;

namespace Dot.Net.Message.Writer
{
    public abstract class ANetMessageWriter
    {
        private NetMessagePattern messPattern = NetMessagePattern.Json;

        private byte serialNumber = 0;
        private MemoryStreamEx bufferStream = new MemoryStreamEx();

        private NetMessageCrypto crypto = null;
        private NetMessageCompressor compressor = null;
        private string secretKey = null;

        protected ANetMessageWriter(
            NetMessagePattern pattern,
            NetMessageCompressor compressor,
            NetMessageCrypto crypto)
        {
            messPattern = pattern;
            this.crypto = crypto;
            this.compressor = compressor;
        }

        protected ANetMessageWriter(
            NetMessagePattern pattern,
            NetMessageCompressor compressor,
            NetMessageCrypto crypto,
            string secretKey):this(pattern,compressor,crypto)
        {
            this.secretKey = secretKey;
        }

        public byte[] EncodeMessage<T>(
            NetMessageCompressionType compressionType,
            NetMessageCryptoType cryptoType,
            int messageID,
            T message
            )
        {
            byte[] messageData = EncodeMessage<T>(message);
            return EncodeData(compressionType, cryptoType, messageID, messageData);
        }

        public byte[] EncodeMessage<T>(int messageID,T message)
        {
            return EncodeMessage<T>(
                NetMessageCompressionType.Uncompressed,
                NetMessageCryptoType.NoCrypto,
                messageID,
                message);
        }

        public byte[] EncodeMessage(int messageID)
        {
            return EncodeData(messageID, null);
        }

        public byte[] EncodeData(
            NetMessageCompressionType compressionType,
            NetMessageCryptoType cryptoType,
            int messageID,
            byte[] messageData)
        {
            bufferStream.Clear();

            ++serialNumber;

            byte[] bytes = messageData;
            if(bytes!=null)
            {
                bytes = crypto.Encode(cryptoType, secretKey, bytes);
                bytes = compressor.Compress(compressionType, bytes);
            }

            int byteSize = NetMessageConst.MessageMinSize + (bytes != null ? bytes.Length + sizeof(byte) : 0);
            int netByteSize = IPAddress.HostToNetworkOrder(byteSize);
            byte[] netSizeBytes = BitConverter.GetBytes(netByteSize);

            bufferStream.Write(netSizeBytes, 0, netSizeBytes.Length);
            bufferStream.WriteByte((byte)serialNumber);
            bufferStream.WriteByte((byte)compressionType);
            bufferStream.WriteByte((byte)cryptoType);

            int netMessageID = IPAddress.HostToNetworkOrder(messageID);
            byte[] netMessageIDBytes = BitConverter.GetBytes(netMessageID);

            bufferStream.Write(netMessageIDBytes, 0, netMessageIDBytes.Length);
            if(bytes!=null)
            {
                bufferStream.WriteByte((byte)messPattern);
                bufferStream.Write(bytes, 0, bytes.Length);
            }
            return bufferStream.ToArray();
        }

        public byte[] EncodeData(int messageID,byte[] messageData)
        {
            return EncodeData(
                NetMessageCompressionType.Uncompressed, 
                NetMessageCryptoType.NoCrypto,
                messageID, 
                messageData);
        }

        protected virtual byte[] EncodeMessage<T>(T message)
        {
            throw new System.NotImplementedException();
        }

        public virtual void Reset()
        {
            serialNumber = 0;
            bufferStream.Clear();
        }
    }
}

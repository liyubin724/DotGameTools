using Dot.Net.Stream;
using System;
using System.Net;

namespace Dot.Net.Message.Writer
{
    public abstract class AMessageWriter : IMessageWriter
    {
        public IMessageCrypto Crypto { get; set; } = null;
        public IMessageCompressor Compressor { get; set; } = null;

        private MessageWriterType writerType = MessageWriterType.None;
        private byte serialNumber = 0;
        private MemoryStreamEx bufferStream = new MemoryStreamEx();

        protected AMessageWriter(MessageWriterType writerType)
        {
            this.writerType = writerType;
        }

        protected AMessageWriter(MessageWriterType writerType,
            IMessageCompressor compressor,
            IMessageCrypto crypto):this(writerType)
        {
            Compressor = compressor;
            Crypto = crypto;
        }

        public byte[] EncodeData(int messageID, byte[] datas)
        {
            return EncodeData(messageID, datas, false, false);
        }

        public byte[] EncodeData(int messageID, byte[] datas, bool isCrypto, bool isCompress)
        {
            bufferStream.Clear();

            byte compressionTypeByte = (byte)MessageCompressorType.Uncompressed;
            byte cryptoTypeByte = (byte)MessageCryptoType.Nocrypto;
            byte[] bytes = datas;
            if (bytes != null)
            {
                if (Crypto != null && isCrypto)
                {
                    cryptoTypeByte = (byte)Crypto.GetCryptoType();
                    bytes = Crypto.Encrypt(bytes);
                }

                if (Compressor != null && isCompress)
                {
                    compressionTypeByte = (byte)Compressor.GetCompressorType();
                    bytes = Compressor.Compress(bytes);
                }
            }

            int byteSize = MessageConst.MessageMinSize + (bytes != null ? bytes.Length + sizeof(byte) : 0);
            int netByteSize = IPAddress.HostToNetworkOrder(byteSize);
            byte[] netSizeBytes = BitConverter.GetBytes(netByteSize);

            bufferStream.Write(netSizeBytes, 0, netSizeBytes.Length);
            bufferStream.WriteByte((byte)serialNumber);
            bufferStream.WriteByte(compressionTypeByte);
            bufferStream.WriteByte(cryptoTypeByte);

            serialNumber++;

            int netMessageID = IPAddress.HostToNetworkOrder(messageID);
            byte[] netMessageIDBytes = BitConverter.GetBytes(netMessageID);

            bufferStream.Write(netMessageIDBytes, 0, netMessageIDBytes.Length);
            if (bytes != null)
            {
                bufferStream.WriteByte((byte)writerType);
                bufferStream.Write(bytes, 0, bytes.Length);
            }
            return bufferStream.ToArray();
        }

        public byte[] EncodeMessage<T>(int messageID, T message)
        {
            return EncodeMessage(messageID, EncodeMessage(message),false,false);
        }

        public byte[] EncodeMessage<T>(int messageID, T message, bool isCrypto, bool isCompress)
        {
            return EncodeData(messageID, EncodeMessage(message),isCrypto,isCompress);
        }

        public byte[] EncodeMessage(int messageID)
        {
            return EncodeData(messageID, null);
        }

        public abstract byte[] EncodeMessage<T>(T message);

        public virtual void Reset()
        {
            serialNumber = 0;
            bufferStream.Clear();
        }
    }
}

using Dot.Net.Message;
using Dot.Net.Stream;
using System.Net;

namespace Dot.Net.Client.Receiver
{
    public class ClientNetDataReceiver : IClientNetDataReceiver
    {
        public IMessageCrypto Crypto { get; set; }
        public IMessageCompressor Compressor { get; set; }

        public OnMessageReceived ReceivedCallback { get; set; }
        public OnMessageError ErrorCallback { get; set; }

        private BufferStream bufferStream = new BufferStream();
        private byte serialNumber = 0;

        public ClientNetDataReceiver()
        {

        }

        public void OnDataReceived(byte[] datas, int size)
        {
            MemoryStreamEx stream = bufferStream.GetActivedStream();
            stream.Write(datas, 0, size);

            int streamLength = (int)stream.Length;
            if (streamLength >= MessageConst.MessageMinSize)
            {
                int startIndex = 0;
                while (true)
                {
                    if (!stream.ReadInt(startIndex, out int totalLength))
                    {
                        break;
                    }
                    totalLength = IPAddress.NetworkToHostOrder(totalLength);
                    if (streamLength < totalLength + sizeof(int))
                    {
                        break;
                    }

                    int offsetIndex = startIndex;

                    offsetIndex += sizeof(int);
                    if (!stream.ReadByte(offsetIndex, out byte serialNum))
                    {
                        ErrorCallback?.Invoke(MessageErrorCode.Receiver_ReadSerialNumberError);
                        break;
                    }
                    if (serialNumber + 1 != serialNum)
                    {
                        ErrorCallback?.Invoke(MessageErrorCode.Receiver_CompareSerialNumberError);
                        break;
                    }

                    offsetIndex += sizeof(byte);
                    if (!stream.ReadByte(offsetIndex, out byte compressionByte))
                    {
                        ErrorCallback?.Invoke(MessageErrorCode.Receiver_ReadCompressorTypeError);
                        break;
                    }
                    MessageCompressorType compressorType = (MessageCompressorType)compressionByte;

                    offsetIndex += sizeof(byte);
                    if (!stream.ReadByte(offsetIndex, out byte cryptoByte))
                    {
                        ErrorCallback?.Invoke(MessageErrorCode.Receiver_ReadCryptoTypeError);
                        break;
                    }
                    MessageCryptoType cryptoType = (MessageCryptoType)cryptoByte;

                    offsetIndex += sizeof(byte);
                    if (!stream.ReadInt(offsetIndex, out int messageID))
                    {
                        ErrorCallback?.Invoke(MessageErrorCode.Receiver_ReadMessageIDError);
                        break;
                    }

                    offsetIndex += sizeof(int);
                    if (offsetIndex < totalLength + startIndex)
                    {
                        if (!stream.ReadByte(offsetIndex, out byte writerTypeByte))
                        {
                            ErrorCallback?.Invoke(MessageErrorCode.Receiver_ReadPatternTypeError);
                            break;
                        }
                        MessageWriterType writerType = (MessageWriterType)writerTypeByte;

                        offsetIndex += sizeof(byte);

                        int messageDataLength = totalLength + startIndex - offsetIndex;
                        byte[] messageDatas = new byte[messageDataLength];
                        if (stream.Read(messageDatas, 0, messageDataLength) != messageDataLength)
                        {
                            ErrorCallback?.Invoke(MessageErrorCode.Receiver_CompareMessageDataLengthError);
                            break;
                        }

                        OnMessage(messageID, compressorType, cryptoType, writerType, messageDatas);
                    }
                    else
                    {
                        ReceivedCallback?.Invoke(messageID, null);
                    }

                    startIndex += totalLength;
                    serialNumber = serialNum;
                }

                bufferStream.MoveStream(startIndex);
            }
        }

        private void OnMessage(int messageID,
            MessageCompressorType compressorType,
            MessageCryptoType cryptoType,
            MessageWriterType writerType,
            byte[] datas)
        {
            byte[] messageBytes = datas;
            if(compressorType!= MessageCompressorType.Uncompressed&&(Compressor == null || Compressor.GetCompressorType() != compressorType))
            {
                ErrorCallback?.Invoke(MessageErrorCode.Receiver_CompareCompressorTypeError);
                return;
            }else
            {
                messageBytes = Compressor.Uncompress(datas);
            }

            if(cryptoType!=MessageCryptoType.Nocrypto && (Crypto == null || Crypto.GetCryptoType() != cryptoType))
            {
                messageBytes = Crypto.Decrypt(messageBytes);
            }

            ReceivedCallback?.Invoke(messageID, messageBytes);
        }

        public void Reset()
        {
            bufferStream.Reset();
            serialNumber = 0;
        }
    }
}

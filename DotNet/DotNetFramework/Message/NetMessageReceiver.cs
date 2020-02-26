using System.Net;

namespace Dot.Net.Message
{
    public class NetMessageReceiver
    {
        private BufferStream bufferStream = new BufferStream();
        private NetMessageHandler handler = null;
        private byte cachedSerialNumber = 0;

        public NetMessageReceiver(NetMessageHandler handler)
        {
            this.handler = handler;
        }

        public void OnDataReceived(byte[] datas,int size)
        {
            MemoryStreamEx stream = bufferStream.GetActivedStream();
            stream.Write(datas, 0, size);

            int streamLength = (int)stream.Length;
            if(streamLength>=NetMessageConst.MessageMinSize)
            {
                int startIndex = 0;
                while(true)
                {
                    if(!stream.ReadInt(startIndex,out int totalLength))
                    {
                        break;
                    }
                    totalLength = IPAddress.NetworkToHostOrder(totalLength);
                    if(streamLength < totalLength+sizeof(int))
                    {
                        break;
                    }

                    int offsetIndex = startIndex;

                    offsetIndex += sizeof(int);
                    if(!stream.ReadByte(offsetIndex, out byte serialNumber))
                    {
                        handler.OnMessageError(NetMessageError.Receiver_ReadSerialNumberError);
                        break;
                    }
                    if(cachedSerialNumber+1 != serialNumber)
                    {
                        handler.OnMessageError(NetMessageError.Receiver_CompareSerialNumberError);
                        break;
                    }

                    offsetIndex += sizeof(byte);
                    if(!stream.ReadByte(offsetIndex, out byte compressionByte))
                    {
                        handler.OnMessageError(NetMessageError.Receiver_ReadCompressionTypeError);
                        break;
                    }
                    NetMessageCompressionType compressionType = (NetMessageCompressionType)compressionByte;

                    offsetIndex += sizeof(byte);
                    if(!stream.ReadByte(offsetIndex, out byte cryptoByte))
                    {
                        handler.OnMessageError(NetMessageError.Receiver_ReadCryptoTypeError);
                        break;
                    }
                    NetMessageCryptoType cryptoType = (NetMessageCryptoType)cryptoByte;

                    offsetIndex += sizeof(byte);
                    if(!stream.ReadInt(offsetIndex, out int messageID))
                    {
                        handler.OnMessageError(NetMessageError.Receiver_ReadMessageIDError);
                        break;
                    }

                    offsetIndex += sizeof(int);
                    if(offsetIndex<totalLength+startIndex)
                    {
                        if(!stream.ReadByte(offsetIndex,out byte patternByte))
                        {
                            handler.OnMessageError(NetMessageError.Receiver_ReadPatternTypeError);
                            break;
                        }
                        NetMessagePattern pattern = (NetMessagePattern)patternByte;

                        offsetIndex += sizeof(byte);

                        int messageDataLength = totalLength + startIndex - offsetIndex;
                        byte[] messageDatas = new byte[messageDataLength];
                        if(stream.Read(messageDatas,0,messageDataLength) != messageDataLength)
                        {
                            handler.OnMessageError(NetMessageError.Receiver_CompareMessageDataLengthError);
                            break;
                        }

                        handler.OnMessageHandle(messageID, compressionType, cryptoType, pattern, messageDatas);
                    }else
                    {
                        handler.OnMessageHandle(messageID);
                    }

                    startIndex += totalLength;
                    cachedSerialNumber = serialNumber;
                }

                bufferStream.MoveStream(startIndex);
            }
        }

        public void Reset()
        {
            bufferStream.Reset();
            cachedSerialNumber = 0;
        }
    }
}

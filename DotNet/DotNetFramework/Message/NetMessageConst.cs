using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dot.Net.Message
{
    public enum NetMessagePattern : byte
    {
        Json = 0,
        ProtoBuf,
    }

    public enum NetMessageError
    {
        Receiver_ReadSerialNumberError = 100,
        Receiver_ReadCompressionTypeError,
        Receiver_ReadCryptoTypeError,
        Receiver_ReadMessageIDError,
        Receiver_ReadPatternTypeError,
        Receiver_CompareMessageDataLengthError,
        Receiver_CompareSerialNumberError,
    }

    public static class NetMessageConst
    {
        public static readonly int MessageMinSize = 0;

        static NetMessageConst()
        {
            MessageMinSize = sizeof(int) //Total Length
                + sizeof(byte) //Serial Number
                + sizeof(byte) //Compression
                + sizeof(byte) //Crypto
                + sizeof(int); //Message ID
        }

    }
}

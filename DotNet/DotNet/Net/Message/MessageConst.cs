using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dot.Net.Message
{
    public enum MessageErrorCode
    {
        Receiver_ReadSerialNumberError = 100,
        Receiver_ReadCompressorTypeError,
        Receiver_ReadCryptoTypeError,
        Receiver_ReadMessageIDError,
        Receiver_ReadPatternTypeError,
        Receiver_CompareMessageDataLengthError,
        Receiver_CompareSerialNumberError,

        Receiver_CompareCompressorTypeError,
        ReceIver_CompareCryptoTypeError,
    }

    public class MessageConst
    {
        public static readonly int MessageMinSize = 0;

        static MessageConst()
        {
            MessageMinSize = sizeof(int) //Total Length
                + sizeof(byte) //Serial Number
                + sizeof(byte) //Compression
                + sizeof(byte) //Crypto
                + sizeof(int); //Message ID
        }
    }
}

using CyoEncode;
using System.Text;

namespace Dot.Crypto
{
    public static class BaseUtil
    {
        #region base64
        private static Base64 _base64 = null;
        private static Base64 base64
        {
            get
            {
                if(_base64 == null)
                {
                    _base64 = new Base64();
                }
                return _base64;
            }
        }

        public static string Base64_Encode(byte[] input)
        {
            return base64.Encode(input);
        }

        public static string Base64_EncodeFromUTF8(string utf8Str)
        {
            byte[] input = UTF8Encoding.Default.GetBytes(utf8Str);
            return Base64_Encode(input);
        }

        public static string Base64_EncodeFromASCII(string asciiStr)
        {
            byte[] input = ASCIIEncoding.Default.GetBytes(asciiStr);
            return Base64_Encode(input);
        }

        public static byte[] Base64_Decode(string input)
        {
            return base64.Decode(input);
        }

        public static string Base64_DecodeToUTF8(string input)
        {
            byte[] bytes = Base64_Decode(input);
            return UTF8Encoding.Default.GetString(bytes);
        }

        public static string Base64_DecodeToASCII(string input)
        {
            byte[] bytes = Base64_Decode(input);
            return ASCIIEncoding.Default.GetString(bytes);
        }
        #endregion

        #region Base32
        private static Base32 _base32 = null;
        private static Base32 base32
        {
            get
            {
                if (_base32 == null)
                {
                    _base32 = new Base32();
                }
                return _base32;
            }
        }

        public static string Base32_Encode(byte[] input)
        {
            return base32.Encode(input);
        }

        public static string Base32_EncodeFromUTF8(string utf8Str)
        {
            byte[] input = UTF8Encoding.Default.GetBytes(utf8Str);
            return Base32_Encode(input);
        }

        public static string Base32_EncodeFromASCII(string asciiStr)
        {
            byte[] input = ASCIIEncoding.Default.GetBytes(asciiStr);
            return Base32_Encode(input);
        }

        public static byte[] Base32_Decode(string input)
        {
            return base32.Decode(input);
        }

        public static string Base32_DecodeToUTF8(string input)
        {
            byte[] bytes = Base32_Decode(input);
            return UTF8Encoding.Default.GetString(bytes);
        }

        public static string Base32_DecodeToASCII(string input)
        {
            byte[] bytes = Base32_Decode(input);
            return ASCIIEncoding.Default.GetString(bytes);
        }

        #endregion

        #region Base16
        private static Base16 _base16 = null;
        private static Base16 base16
        {
            get
            {
                if (_base16 == null)
                {
                    _base16 = new Base16();
                }
                return _base16;
            }
        }

        public static string Base16_Encode(byte[] input)
        {
            return base16.Encode(input);
        }

        public static string Base16_EncodeFromUTF8(string utf8Str)
        {
            byte[] input = UTF8Encoding.Default.GetBytes(utf8Str);
            return Base16_Encode(input);
        }

        public static string Base16_EncodeFromASCII(string asciiStr)
        {
            byte[] input = ASCIIEncoding.Default.GetBytes(asciiStr);
            return Base16_Encode(input);
        }

        public static byte[] Base16_Decode(string input)
        {
            return base16.Decode(input);
        }

        public static string Base16_DecodeToUTF8(string input)
        {
            byte[] bytes = Base16_Decode(input);
            return UTF8Encoding.Default.GetString(bytes);
        }

        public static string Base16_DecodeToASCII(string input)
        {
            byte[] bytes = Base16_Decode(input);
            return ASCIIEncoding.Default.GetString(bytes);
        }

        #endregion

        #region Base85
        private static Base85 _base85 = null;
        private static Base85 base85
        {
            get
            {
                if (_base85 == null)
                {
                    _base85 = new Base85();
                }
                return _base85;
            }
        }

        public static string Base85_Encode(byte[] input)
        {
            return base85.Encode(input);
        }

        public static string Base85_EncodeFromUTF8(string utf8Str)
        {
            byte[] input = UTF8Encoding.Default.GetBytes(utf8Str);
            return Base85_Encode(input);
        }

        public static string Base85_EncodeFromASCII(string asciiStr)
        {
            byte[] input = ASCIIEncoding.Default.GetBytes(asciiStr);
            return Base85_Encode(input);
        }

        public static byte[] Base85_Decode(string input)
        {
            return base85.Decode(input);
        }

        public static string Base85_DecodeToUTF8(string input)
        {
            byte[] bytes = Base85_Decode(input);
            return UTF8Encoding.Default.GetString(bytes);
        }

        public static string Base85_DecodeToASCII(string input)
        {
            byte[] bytes = Base85_Decode(input);
            return ASCIIEncoding.Default.GetString(bytes);
        }

        #endregion
    }
}

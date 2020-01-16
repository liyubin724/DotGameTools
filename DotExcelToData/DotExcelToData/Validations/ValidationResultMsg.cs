using System.Collections.Generic;
using SystemObject = System.Object;

namespace Dot.Tools.ETD.Validations
{
    public enum ValidationResultCode
    {
        Success = 0,
        Pass = 1,

        Failed = -1,
        ArgIsNull = -2,
        ContentIsNull = -3,
        ParseContentFailed = -4,
        NumberRangeError = -5,
        MaxLenError = -6,
        ContentRepeatError = -7,
        ContentDicFormatError = -8,
        ContentDicKeyValueCountError = -9,
        ContentDicKeyRepeatError = -10,
    }

    public static class ValidationMsg
    {
        private static Dictionary<ValidationResultCode, string> resultMsg = new Dictionary<ValidationResultCode, string>();
        static ValidationMsg()
        {
            resultMsg.Add(ValidationResultCode.ArgIsNull, "Argument is null!");
        }

        internal static string GetMsg(ValidationResultCode resultCode,SystemObject[] resultParams)
        {
            if(resultMsg.TryGetValue(resultCode,out string msg))
            {
                if(resultParams!=null && resultParams.Length>0)
                {
                    return string.Format(msg, resultParams);
                }
                else
                {
                    return msg;
                }
            }

            return string.Empty;
        }
    }

    public class ValidationResultMsg
    {
        public ValidationResultCode ResultCode { get; set; } = ValidationResultCode.Success;
        public SystemObject[] ResultParams { get; set; } = null;

        public string ResultMsg
        {
            get
            {
                return ValidationMsg.GetMsg(ResultCode, ResultParams);
            }
        }

        public ValidationResultMsg()
        {

        }

        public ValidationResultMsg(ValidationResultCode resultCode,params object[] values)
        {
            ResultCode = resultCode;
            ResultParams = values;
        }
    }
}

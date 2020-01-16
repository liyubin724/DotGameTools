using System.Collections.Generic;
using SystemObject = System.Object;

namespace Dot.Tools.ETD.Validations
{
    public enum ValidationResultCode
    {
        Success = 0,
        Failed,
        Pass,

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
}

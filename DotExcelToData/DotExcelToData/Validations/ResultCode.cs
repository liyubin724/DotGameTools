using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dot.Tools.ETD.Validations
{
    public enum ResultCode
    {
        Success = 0,
        Pass = 1,

        Failed= -1,
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

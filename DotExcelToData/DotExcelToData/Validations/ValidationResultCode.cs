namespace Dot.Tools.ETD.Validations
{
    public enum ValidationResultCode
    {
        Success = 0,
        Pass =1,

        ValidationFormatError = -1,
        ArgIsNull = -2,
        ContentIsNull = -3,
        ParseContentFailed = -4,
        NumberRangeError = -5,
        MaxLenError = -6,
        ContentRepeatError = -7,
        ContentDicFormatError = -8,
        ContentDicKeyValueCountError = -9,
        ContentDicKeyRepeatError = -10,
        TextNotFoundError = -11,
        TextIDNotFoundError = -12,
    }
}

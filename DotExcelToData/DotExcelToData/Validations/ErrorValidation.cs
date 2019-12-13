namespace Dot.Tools.ETD.Validations
{
    public class ErrorValidation : IValidation
    {
        public bool IsValid => false;

        public string ErrorMsg { get; set; }

        public void SetData(string rule)
        {
            ErrorMsg = "The type of Validation is Error.rule = "+rule;
        }

        public ResultCode Verify(out string msg)
        {
            msg = ErrorMsg;
            return ResultCode.Failed;
        }
    }
}

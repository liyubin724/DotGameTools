namespace Dot.Tools.ETD.Validations
{
    public class ErrorValidation : IValidation
    {
        public bool IsValid => false;

        public void SetData(string rule)
        {
            
        }

        public ResultCode Verify(out string msg)
        {
            msg = "Error Validation";
            return ResultCode.Failed;
        }
    }
}

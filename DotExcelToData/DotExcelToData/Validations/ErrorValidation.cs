using ExtractInject;

namespace Dot.Tools.ETD.Validations
{
    public class ErrorValidation : IValidation
    {

        public void SetRule(string rule)
        {
            throw new System.NotImplementedException();
        }

        public ValidationResultCode Verify(EIContext context)
        {
            throw new System.NotImplementedException();
        }


        //--------------------------------

        public bool IsValid => false;

        public string ErrorMsg { get; set; }

        public void SetData(string rule)
        {
            ErrorMsg = "The type of Validation is Error.rule = "+rule;
        }

        public ValidationResultCode Verify(out string msg)
        {
            msg = ErrorMsg;
            return ValidationResultCode.Failed;
        }
    }
}

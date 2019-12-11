using System;

namespace Dot.Tools.ETD.Validations
{
    public class ErrorValidation : IValidation
    {
        private string rule;
        public string GetRule()
        {
            throw new NotImplementedException();
        }

        public void SetRule(string rule)
        {
            this.rule = rule;
        }

        public ResultCode Valid(out string msg)
        {
            msg = $"Validation Foramt.rule = {rule}";
            return ResultCode.Failed;
        }
    }
}

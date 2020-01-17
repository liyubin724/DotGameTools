using ExtractInject;

namespace Dot.Tools.ETD.Validations
{
    public class ErrorValidation : IValidation
    {
        public string Rule { get => rule; }
        private string rule = string.Empty;
        public void SetRule(string rule)
        {
            this.rule = rule;
        }

        public ValidationResultCode Verify(EIContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}

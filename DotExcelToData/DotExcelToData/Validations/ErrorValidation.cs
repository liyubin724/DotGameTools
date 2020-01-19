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

        public ValidationResultCode Verify()
        {
            throw new System.NotImplementedException();
        }
    }
}

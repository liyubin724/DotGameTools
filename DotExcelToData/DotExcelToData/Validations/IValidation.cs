namespace Dot.Tools.ETD.Validations
{
    public interface IValidation
    {
        void SetRule(string rule);
        string GetRule();
        ResultCode Valid(out string msg);
    }
}

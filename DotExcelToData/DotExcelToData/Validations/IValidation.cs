namespace Dot.Tools.ETD.Validations
{
    public interface IValidation
    {
        bool IsValid { get; }
        void SetData(string rule);
        ResultCode Verify(out string msg);
    }
}

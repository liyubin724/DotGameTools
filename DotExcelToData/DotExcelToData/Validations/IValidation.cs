namespace Dot.Tools.ETD.Validations
{
    public interface IValidation
    {
        string ErrorMsg { get; set; }
        bool IsValid { get; }
        void SetData(string rule);
        ResultCode Verify(out string msg);
    }
}

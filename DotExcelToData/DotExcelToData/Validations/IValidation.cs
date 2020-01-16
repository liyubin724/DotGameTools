using ExtractInject;

namespace Dot.Tools.ETD.Validations
{
    public interface IValidation
    {
        void SetRule(string rule);
        ValidationResultMsg Verify(EIContext context);



        string ErrorMsg { get; set; }
        bool IsValid { get; }
        void SetData(string rule);
        ValidationResultCode Verify(out string msg);
    }
}

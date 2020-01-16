using ExtractInject;

namespace Dot.Tools.ETD.Validations
{
    public interface IValidation
    {
        void SetRule(string rule);
        ValidationResultCode Verify(EIContext context);
    }
}

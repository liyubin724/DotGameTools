using ExtractInject;

namespace Dot.Tools.ETD.Verify
{
    public interface IVerify
    {
        bool Verify(IEIContext context);
    }
}

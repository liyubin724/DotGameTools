using ExtractInject;

namespace ExtractInjectTest
{
    public class InjectObject : ExtractInjectTarget
    {
        [ExtractInjectField(ExtractInjectUsage.In,false)]
        public EIObject eiObj;
    }
}

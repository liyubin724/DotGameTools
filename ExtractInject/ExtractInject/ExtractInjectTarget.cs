namespace ExtractInject
{
    public class ExtractInjectTarget : IExtractInjectTarget
    {
        public void Extract(IExtractInjectContext context)
        {
            ExtractInjectUtil.Extract(context, this);
        }

        public void Inject(IExtractInjectContext context)
        {
            ExtractInjectUtil.Inject(context, this);
        }
    }
}

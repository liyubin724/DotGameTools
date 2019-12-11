namespace ExtractInject
{
    public class EIObject : IEIObject
    {
        public void Extract(IEIContext context)
        {
            EIUtil.Extract(context, this);
        }

        public void Inject(IEIContext context)
        {
            EIUtil.Inject(context, this);
        }
    }
}

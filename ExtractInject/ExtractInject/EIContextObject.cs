namespace ExtractInject
{
    public abstract class EIContextObject : IEIContextObject
    {
        public void AddToContext(IEIContext context)
        {
            if (context != null)
            {
                context.AddObject(GetType(),this);
            }
        }

        public void RemoveFromContext(IEIContext context)
        {
            if (context != null)
            {
                context.DeleteObject(GetType());
            }
        }
    }
}

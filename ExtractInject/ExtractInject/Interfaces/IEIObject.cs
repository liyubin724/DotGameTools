namespace ExtractInject
{
    public interface IEIObject
    {
        void Inject(IEIContext context);
        void Extract(IEIContext context);
    }
}

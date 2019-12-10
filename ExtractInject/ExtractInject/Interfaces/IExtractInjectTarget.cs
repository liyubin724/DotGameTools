namespace ExtractInject
{
    public interface IExtractInjectTarget
    {
        void Inject(IExtractInjectContext context);
        void Extract(IExtractInjectContext context);
    }
}

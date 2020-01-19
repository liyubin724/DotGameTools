namespace Dot.Tools.ETD.IO
{
    public enum ETDWriterFormat
    {
        ALL,
        Json,
        Lua,
    }

    public enum ETDWriterTarget
    {
        Client,
        Server,
    }

    public static class IOConst
    {
        public static readonly string JSON_EXTERSION = ".json";
    }
}

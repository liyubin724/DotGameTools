using Dot.Tools.ETD.Datas;

namespace Dot.Tools.ETD.IO
{
    public enum ETDWriterFormat
    {
        All,
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
        public static readonly string JSON_DIR_NAME = "json";

        public static readonly string LUA_EXTERSION = ".txt";
        public static readonly string LUA_DIR_NAME = "lua";
        public static readonly string LUA_PATH_FORMAT = "Game/Config/{0}_"+SheetConst.TEXT_BOOK_NAME;
    }
}

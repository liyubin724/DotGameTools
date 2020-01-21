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

        public static readonly string LUA_REQUIRE_FORMAT = "require (\"{0}\")";
        public static readonly string LUA_SET_METATABLE_FORMAT = "setmetatable({0},{1})";
        public static readonly string LUA_SET_INDEX_FORMART = "{0}.__index = {1}";
        public static readonly string LUA_LOCAL_DEFINE_FORMAT = "local {0} = {{}}";
        public static readonly string LUA_LOCAL_DEFINE_BEGIN_FORMAT = "local {0} = {{";
        public static readonly string LUA_LOCAL_DEFINE_END = "}";
        public static readonly string LUA_NULL = "nil";

        public static readonly string LUA_STRING_FORMAT = "[===[{0}]===]";

       
        public static readonly string LUA_SUMMARY_SHEET_META_NAME = "ConfigSummarySheet";
        public static readonly string LUA_SUMMARY_SHEET_META = "Dot/Config/"+ LUA_SUMMARY_SHEET_META_NAME;
        public static readonly string LUA_SHEET_LINE_META = "Dot/Config/ConfigSheetLine";
        public static readonly string LUA_SUB_SHEET_META = "Dot/Config/ConfigSubSheet";

        public static readonly string LUA_SUMMARY_DEPEND_NAME = "depends";
        public static readonly string LUA_SUMMARY_TEXT_NAME = "text";
        public static readonly string LUA_SUMMARY_DEFAULT_NAME = "defaultValues";
        public static readonly string LUA_SUMMARY_STRING_NAME = "strValues";
        public static readonly string LUA_SUMMARY_LUA_FIELD_NAME = "luaFieldNames";
        public static readonly string LUA_SUMMARY_STR_FIELD_NAME = "strFieldNames";
        public static readonly string LUA_SUMMARY_TEXT_FIELD_NAME = "textFieldNames";

        public static readonly string LUA_SUMMARY_SUB_NAME = "subSheets";
        public static readonly string LUA_SUMMARY_DATA_NAME = "data";
        public static readonly string LUA_SUBMARY_SUBSHEET_STARTID = "startID";
        public static readonly string LUA_SUBMARY_SUBSHEET_ENDID = "endID";
        public static readonly string LUA_SUBMARY_SUBSHEET_PATH = "path";
        public static readonly string LUA_SUBMARY_SUBSHEET_PATH_FORMAT = "Game/Config/{0}/{1}";

        public static readonly string LUA_FIELD_INDEX_FORMAT = "{0}_Index";

        public static readonly string LUA_TEMP_TABLE_NAME_FORMAT = "t_{0}";
    }
}

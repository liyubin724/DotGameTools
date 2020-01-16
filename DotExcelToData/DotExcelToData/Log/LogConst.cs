using System.Collections.Generic;

namespace Dot.Tools.ETD.Log
{
    public enum LogType
    {
        Verbose = 0,
        Info,
        Warning,
        Error,
    }

    public delegate void OnHandlerLog(LogType type, string msg);

    public static class LogConst
    {
        public static readonly int LOG_FILE_NOT_EXIST = -1;
        public static readonly int LOG_FILE_NOT_EXCEL = -2;

        public static readonly int LOG_WORKBOOK_EMPTY = -100;

        public static readonly int LOG_SHEET_NAME_NULL = -201;
        public static readonly int LOG_SHEET_NAME_NOT_MATCH = -202;
        public static readonly int LOG_SHEET_ROW_LESS = -203;
        public static readonly int LOG_SHEET_COL_LESS = -204;

        public static readonly int LOG_WORKBOOK_START = 100;
        public static readonly int LOG_WORKBOOK_END = 101;

        public static readonly int LOG_SHEET_START = 201;
        public static readonly int LOG_SHEET_END = 202;
        public static readonly int LOG_IGNORE_SHEET = 203;
        public static readonly int LOG_SHEET_FIELD_START = 204;
        public static readonly int LOG_SHEET_FIELD_END = 205;
        public static readonly int LOG_SHEET_LINE_START = 206;
        public static readonly int LOG_SHEET_LINE_END = 207;
        public static readonly int LOG_SHEET_FIELD_IGNORE = 208;
        public static readonly int LOG_SHEET_FIELD_CREATE = 209;
        public static readonly int LOG_SHEET_FIELD_DETAIL = 210;
        public static readonly int LOG_SHEET_LINE_CREATE = 211;
        public static readonly int LOG_SHEET_LINE_DETAIL = 212;

        public static readonly int LOG_ARG_NULL = -1000;

        private static Dictionary<int, string> logFormatDic = new Dictionary<int, string>();
        static LogConst()
        {
            logFormatDic.Add(LOG_FILE_NOT_EXIST, "File is not found.path = {0}");
            logFormatDic.Add(LOG_FILE_NOT_EXCEL, "File is not a excel file.path = {0}");

            logFormatDic.Add(LOG_WORKBOOK_EMPTY, "Workbook is empty. path = {0}");
            logFormatDic.Add(LOG_SHEET_NAME_NULL, "The name of sheet is null.it will be ignored. index = {0}");
            logFormatDic.Add(LOG_SHEET_NAME_NOT_MATCH, "The sheet which named ({0}) will be ingored,because of the name should be like '{1}'");

            logFormatDic.Add(LOG_WORKBOOK_START, "Start to convert excel to workbook.path = {0}");
            logFormatDic.Add(LOG_IGNORE_SHEET, "The sheet which named ({0}) will be ingored,because of the name of sheet is start with '#'");

            logFormatDic.Add(LOG_ARG_NULL, "Argument is null");
        }

        public static string GetLogMsg(int logID,params object[] datas)
        {
            if(logFormatDic.TryGetValue(logID,out string msg))
            {
                if(datas!=null && datas.Length>0)
                {
                    return string.Format(msg, datas);
                }else
                {
                    return msg;
                }
            }
            return string.Empty;
        }
    }
}

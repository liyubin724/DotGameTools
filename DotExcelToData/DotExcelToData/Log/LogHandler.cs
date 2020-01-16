using ExtractInject;

namespace Dot.Tools.ETD.Log
{
    public class LogHandler : EIContextObject
    {
        private OnHandlerLog logCallback = null;

        public LogHandler(OnHandlerLog callback)
        {
            logCallback = callback;
        }

        public void Log(LogType type, int logID, params object[] datas)
        {
            logCallback?.Invoke(type, LogConst.GetLogMsg(logID, datas));
        }
    }
}

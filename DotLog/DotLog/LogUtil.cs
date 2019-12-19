using log4net;

namespace Dot.Log
{
    public static class LogUtil
    {
        public static ILog Logger(string tag)
        {
            return LogManager.GetLogger(tag);
        }
    }
}

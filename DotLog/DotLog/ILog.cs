using System;

namespace Dot.Log
{
    public enum LogLevelType
    {
        Info,
        Warning,
        Error,
        Fatal,
    }

    public interface ILog
    {
        void Log(LogLevelType levelType, string msg);
        void Log(LogLevelType levelType, Type tagType, string msg);
        void Log(LogLevelType levelType, string tagName, string msg);
        void LogFormat(LogLevelType levelType, string msgFormat, params object[] values);
        void LogFormat(LogLevelType levelType, Type tagType, string msgFormat, params object[] values);
        void LogFormat(LogLevelType levelType, string tagName, string msgFormat, params object[] values);

        void LogInfo(string msg);
        void LogInfo(Type tagType, string msg);
        void LogInfo(string tagName, string msg);
        void LogInfoFormat(string msgFormat, params object[] args);
        void LogInfoFormat(Type tagType, string msgFormat, params object[] args);
        void LogInfoFormat(string tagName, string msgFormat, params object[] args);

        void LogWarning(string msg);
        void LogWarning(Type tagType, string msg);
        void LogWarning(string tagName, string msg);
        void LogWarningFormat(string msgFormat, params object[] args);
        void LogWarningFormat(Type tagType, string msgFormat, params object[] args);
        void LogWarningFormat(string tagName, string msgFormat, params object[] args);

        void LogError(string msg);
        void LogError(Type tagType, string msg);
        void LogError(string tagName, string msg);
        void LogErrorFormat(string msgFormat, params object[] args);
        void LogErrorFormat(Type tagType, string msgFormat, params object[] args);
        void LogErrorFormat(string tagName, string msgFormat, params object[] args);

        void LogFatal(string msg);
        void LogFatal(Type tagType, string msg);
        void LogFatal(string tagName, string msg);
        void LogFatalFormat(string msgFormat, params object[] args);
        void LogFatalFormat(Type tagType, string msgFormat, params object[] args);
        void LogFatalFormat(string tagName, string msgFormat, params object[] args);
    }
}

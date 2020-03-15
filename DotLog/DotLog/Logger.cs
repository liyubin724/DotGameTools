using log4net.Config;
using System;
using System.IO;
using System.Text;
using IDotLog = Dot.Log.ILog;
using ILog4NetLog = log4net.ILog;

namespace Dot.Log
{
    public class Logger : IDotLog
    {
        private static string DEFAULT_LOGGER_NAME = "Log";
        private bool isInited = false;

        internal Logger()
        {
        }

        internal bool InitLogger(string config)
        {
            if(string.IsNullOrEmpty(config))
            {
                return false;
            }

            byte[] configBytes = Encoding.UTF8.GetBytes(config);
            using(MemoryStream ms = new MemoryStream(configBytes))
            {
                XmlConfigurator.Configure(ms);
                isInited = true;
                return true;
            }
        }

        public void Log(LogLevelType levelType, string msg)
        {
            Log(levelType, DEFAULT_LOGGER_NAME, msg);
        }

        public void Log(LogLevelType levelType, Type tagType, string msg)
        {
            Log(levelType, tagType?.Name, msg);
        }

        public void Log(LogLevelType levelType, string tagName, string msg)
        {
            if(levelType == LogLevelType.Info)
            {
                LogInfo(tagName, msg);
            }else if(levelType == LogLevelType.Warning)
            {
                LogWarning(tagName, msg);
            }else if(levelType == LogLevelType.Error)
            {
                LogError(tagName, msg);
            }else if(levelType == LogLevelType.Fatal)
            {
                LogFatal(tagName, msg);
            }

            throw new InvalidOperationException($"Log4NetLogger::Log->LogLevelType not found.levelType = {levelType}");
        }

        public void LogError(string msg)
        {
            LogError(DEFAULT_LOGGER_NAME, msg);
        }

        public void LogError(Type tagType, string msg)
        {
            LogError(tagType?.Name, msg);
        }

        public void LogError(string tagName, string msg)
        {
            GetLogger(tagName)?.Error(msg);
        }

        public void LogErrorFormat(string msgFormat, params object[] args)
        {
            LogErrorFormat(DEFAULT_LOGGER_NAME, msgFormat, args);
        }

        public void LogErrorFormat(Type tagType, string msgFormat, params object[] args)
        {
            LogErrorFormat(tagType?.Name, msgFormat, args);
        }

        public void LogErrorFormat(string tagName, string msgFormat, params object[] args)
        {
            GetLogger(tagName)?.ErrorFormat(msgFormat, args);
        }

        public void LogFatal(string msg)
        {
            LogFatal(DEFAULT_LOGGER_NAME, msg);
        }

        public void LogFatal(Type tagType, string msg)
        {
            LogFatal(tagType?.Name, msg);
        }

        public void LogFatal(string tagName, string msg)
        {
            GetLogger(tagName)?.Fatal(msg);
        }

        public void LogFatalFormat(string msgFormat, params object[] args)
        {
            LogFatalFormat(DEFAULT_LOGGER_NAME, msgFormat, args);
        }

        public void LogFatalFormat(Type tagType, string msgFormat, params object[] args)
        {
            LogFatalFormat(tagType?.Name, msgFormat, args);
        }

        public void LogFatalFormat(string tagName, string msgFormat, params object[] args)
        {
            GetLogger(tagName)?.FatalFormat(msgFormat,args);
        }

        public void LogFormat(LogLevelType levelType, string msgFormat, params object[] values)
        {
            LogFormat(levelType, DEFAULT_LOGGER_NAME, msgFormat, values);
        }

        public void LogFormat(LogLevelType levelType, Type tagType, string msgFormat, params object[] values)
        {
            LogFormat(levelType, tagType?.Name, msgFormat, values);
        }

        public void LogFormat(LogLevelType levelType, string tagName, string msgFormat, params object[] values)
        {
            if (levelType == LogLevelType.Info)
            {
                LogInfoFormat(tagName, msgFormat,values);
            }
            else if (levelType == LogLevelType.Warning)
            {
                LogWarningFormat(tagName, msgFormat, values);
            }
            else if (levelType == LogLevelType.Error)
            {
                LogErrorFormat(tagName, msgFormat, values);
            }
            else if (levelType == LogLevelType.Fatal)
            {
                LogFatalFormat(tagName, msgFormat, values);
            }
        }

        public void LogInfo(string msg)
        {
            LogInfo(DEFAULT_LOGGER_NAME, msg);
        }

        public void LogInfo(Type tagType, string msg)
        {
            LogInfo(tagType?.Name, msg);
        }

        public void LogInfo(string tagName, string msg)
        {
            GetLogger(tagName)?.Info(msg);
        }

        public void LogInfoFormat(string msgFormat, params object[] args)
        {
            LogInfoFormat(DEFAULT_LOGGER_NAME, msgFormat, args);
        }

        public void LogInfoFormat(Type tagType, string msgFormat, params object[] args)
        {
            LogInfoFormat(tagType?.Name, msgFormat, args);
        }

        public void LogInfoFormat(string tagName, string msgFormat, params object[] args)
        {
            GetLogger(tagName)?.InfoFormat(msgFormat, args);
        }

        public void LogWarning(string msg)
        {
            LogWarning(DEFAULT_LOGGER_NAME, msg);
        }

        public void LogWarning(Type tagType, string msg)
        {
            LogWarning(tagType?.Name, msg);
        }

        public void LogWarning(string tagName, string msg)
        {
            GetLogger(tagName)?.Warn(msg);
        }

        public void LogWarningFormat(string msgFormat, params object[] args)
        {
            LogWarningFormat(DEFAULT_LOGGER_NAME, msgFormat, args);
        }

        public void LogWarningFormat(Type tagType, string msgFormat, params object[] args)
        {
            LogWarningFormat(tagType?.Name, msgFormat, args);
        }

        public void LogWarningFormat(string tagName, string msgFormat, params object[] args)
        {
            GetLogger(tagName)?.WarnFormat(msgFormat, args);
        }

        private ILog4NetLog GetLogger(string loggerName)
        {
            if(string.IsNullOrEmpty(loggerName))
            {
                throw new ArgumentNullException("Log4NetLogger::GetLogger->loggerName is null or empty.");
            }

            if(!isInited)
            {
                throw new LogNotInitilizedException();
            }

            return log4net.LogManager.GetLogger(loggerName);
        }

    }
}

using System;
using UnityEngine;

namespace Dot.Log
{
    public static class LogUtil
    {
        private static LogLevelType maxLogLevel = LogLevelType.Info;
        private static Logger logger = null;

        public static void Initalize(string xmlConfig,LogLevelType level = LogLevelType.Info)
        {
            maxLogLevel = level;
            logger = new Logger();
            if(!logger.InitLogger(xmlConfig))
            {
                logger = null;
                throw new Exception("LogUtil::Initalize->logger initalized failded");
            }

#if !UNITY_EDITOR
            Application.logMessageReceived += OnMessageReceived;
#endif
        }

        private static void OnMessageReceived(string condition, string stackTrace, LogType type)
        {
            if(type == LogType.Exception)
            {
                LogError("Exception",$"{condition}\r\n{stackTrace}");
            }
        }

        public static void LogError(Type tagType, string msg)
        {
            if(logger!=null && maxLogLevel <= LogLevelType.Error)
            {
                logger.LogError(tagType, msg);
            }
        }

        public static void LogError(string tagName,string msg)
        {
            if(logger!=null && maxLogLevel <= LogLevelType.Error)
            {
                logger.LogError(tagName, msg);
            }
        }

        public static void LogFatal(Type tagType ,string msg)
        {
            if(logger!=null && maxLogLevel <= LogLevelType.Fatal)
            {
                logger.LogFatal(tagType, msg);
            }
        }

        public static void LogFatal(string tagName,string msg)
        {
            if(logger!=null && maxLogLevel <= LogLevelType.Fatal)
            {
                logger.LogFatal(tagName, msg);
            }
        }

        public static void LogInfo(string tagName,string msg)
        {
            if(logger!=null && maxLogLevel<=LogLevelType.Info)
            {
                logger.LogInfo(tagName, msg);
            }
        }

        public static void LogInfo(Type tagType,string msg)
        {
            if(logger!=null && maxLogLevel<=LogLevelType.Info)
            {
                logger.LogInfo(tagType, msg);
            }
        }

        public static void LogWarning(Type tagType,string msg)
        {
            if(logger!=null && maxLogLevel <= LogLevelType.Warning)
            {
                logger.LogWarning(tagType, msg);
            }
        }

        public static void LogWarning(string tagName,string msg)
        {
            if(logger!=null && maxLogLevel<=LogLevelType.Warning)
            {
                logger.LogWarning(tagName, msg);
            }
        }
    }
}

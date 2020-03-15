using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                throw new Exception("LogUtil::Initalize->logger initalized failded");
            }
        }

    }
}

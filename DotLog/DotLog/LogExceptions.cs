using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dot.Log
{
    public class LogUnkownException : Exception
    {
        public LogUnkownException():base("Unknown exception")
        {
        }
    }

    public class LogNotInitilizedException : Exception
    {
        public LogNotInitilizedException() : base("Logger is not initialized.")
        {
        }
    }
}

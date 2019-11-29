using System;

namespace Dot.Core.Config
{
    public static class DebugLogger
    {
        public static void Error(string message)
        {
            Console.WriteLine("Error : "+message);
        }
        public static void Warning(string message)
        {
            Console.WriteLine("Warning : " + message);
        }
        public static void Info(string message)
        {
            Console.WriteLine("Info : " + message);
        }

        public static void Exception(Exception e)
        {
            Console.WriteLine("Exception : " + e.Message);
        }
    }
}

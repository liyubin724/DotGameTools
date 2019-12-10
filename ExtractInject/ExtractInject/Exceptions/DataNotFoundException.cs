using System;

namespace ExtractInject
{
    public class DataNotFoundException : Exception
    {
        public DataNotFoundException(string message):base(message)
        {

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logging.Exceptions
{

    public class LoggingException : Exception
    {
        public LoggingException(string message)
            : base(message)
        {
        }

        public LoggingException(string message, Exception e)
            : base(e.Message, e)
        {
        }

        public LoggingException(Exception e)
            : base(e.Message, e)
        {
        }

    }
}

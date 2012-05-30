using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logging
{
    public class LogPackage
    {

        public string Message { get; set; }

        public string ClassName { get; set; }

        public LogPackage()
        {
        }

        public LogPackage(string message, string className)
        {
            Message = message;
            ClassName = className;
        }
    }
}

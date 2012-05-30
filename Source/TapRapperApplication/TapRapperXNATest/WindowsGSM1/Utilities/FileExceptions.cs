using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilities
{
    public class CSVFileException: Exception
    {

        public CSVFileException(Exception p_InnerException)
            : base(p_InnerException.Message, p_InnerException)
        {
        }

        public CSVFileException(string p_Message)
            : base(p_Message)
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSNamedPipeClient
{
    public class PipeCommunicationException : Exception
    {
        public PipeCommunicationException(Exception l_InnerException)
            : base(l_InnerException.Message, l_InnerException)
        {
        }
    }
}

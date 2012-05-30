using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VideoPanelControl
{
    public class TrainingVideoException : Exception
    {
        public TrainingVideoException(Exception p_InnerException)
            : base(p_InnerException.Message, p_InnerException)
        {
        }

        public TrainingVideoException(string p_Message)
            : base(p_Message)
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace WiimoteData
{
    public class WiimoteCommunicationException : Exception
    {
        public WiimoteCommunicationException(Exception p_InnerException):base(p_InnerException.Message,p_InnerException)
        {
        }
    }

    public class WiimoteConnectionException : Exception
    {
        public WiimoteState State { get; set; }

        public WiimoteConnectionException(Exception p_InnerException,WiimoteState p_State)
            : base(p_InnerException.Message, p_InnerException)
        {
            State = p_State;
        }

        public WiimoteConnectionException(string p_Message, WiimoteState p_State)
            : base(p_Message)
        {
        }
    }

    public class WiimoteRecordingException : Exception
    {

        public WiimoteRecordingException(Exception p_InnerException)
            : base(p_InnerException.Message, p_InnerException)
        {
        }

        public WiimoteRecordingException(string p_Message)
            : base(p_Message)
        {
        }
    }

    public class WiimoteDataStoreException : Exception
    {

        public WiimoteDataStoreException(Exception p_InnerException)
            : base(p_InnerException.Message, p_InnerException)
        {
        }

        public WiimoteDataStoreException(string p_Message)
            : base(p_Message)
        {
        }
    }

}

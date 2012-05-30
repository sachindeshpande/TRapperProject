using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace CSNamedPipeClient
{

    public class PipeDataEventArgs : EventArgs
    {
        public PipeDataEventArgs(string []p_DataList,string pRawRowValue)
        {
            m_DataList = p_DataList;
            mRawRowValue = pRawRowValue;
        }

        private string[] m_DataList;
        private string mRawRowValue;
        public string[] DataList 
        {
            get
            {
                return m_DataList;
            }
        }

        public string RawRowValue
        {
            get
            {
                return mRawRowValue;
            }
        }

    }

    public class DataEventNamedPipe : NamedPipe
    {
        private bool m_DataEventsOn;

        public delegate void OnPipeDataEvent(object sender, PipeDataEventArgs args);

        public event OnPipeDataEvent PipeDataEvent;

        public DataEventNamedPipe()
        {
            strPipeName = WIIMOTE_DATA_PIPE_NAME;
        }

        public void startWiimoteLogging()
        {
            m_DataEventsOn = true;
            Thread t = new Thread(new ThreadStart(listenWiimoteDataCommands));
            t.Start();
        }

        public void stopWiimoteLogging()
        {
            m_DataEventsOn = false;
        }

        public void cleanup()
        {
            stopWiimoteLogging();
        }

        public void listenWiimoteDataCommands()
        {
            // Receive one message from the pipe.
            byte[] bRequest;                        // Client -> Server
            int cbRequestBytes;
            byte[] bReply = new byte[DATA_BUFFER_SIZE];  // Server -> Client
            int cbBytesRead, cbReplyBytes;

            cbReplyBytes = DATA_BUFFER_SIZE;

            while (m_DataEventsOn && pipeClient.IsConnected)
            {
                do
                {
                    if (pipeClient.CanRead)
                    {
                        cbBytesRead = pipeClient.Read(bReply, 0, cbReplyBytes);

                        if (cbBytesRead == 0)
                            break;

                        // Unicode-encode the byte array and trim all the '\0' chars 
                        // at the end.
                        string strMessage = Encoding.Unicode.GetString(bReply).TrimEnd('\0');
/*
                        Console.WriteLine("Receives {0} bytes; Message: \"{1}\"",
                            cbBytesRead, strMessage); */
          
                        //For some reason the pipe is sending extra characters. Code to clean up the extra characters
                        if (strMessage.Length > cbBytesRead / 2)
                            strMessage = strMessage.Substring(0, cbBytesRead / 2 - 1);


                        string []row = strMessage.Split(',');

                        if (PipeDataEvent!= null)
                            PipeDataEvent(this, new PipeDataEventArgs(row, strMessage));
                         
                    }
                }
                while (pipeClient.IsMessageComplete);
            }
        }

    }
}

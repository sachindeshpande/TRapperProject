using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSNamedPipeClient
{
    public class CommandNamedPipe : NamedPipe
    {
        public CommandNamedPipe()
        {
            strPipeName = WIIMOTE_COMMAND_PIPE_NAME;
        }

        public void cleanup()
        {
        }

        public string sendMessage(string strMessage)
        {
            try
            {

                /////////////////////////////////////////////////////////////////
                // Send a message to the pipe server and receive its response.
                //                

                // A byte buffer of BUFFER_SIZE bytes. The buffer should be big 
                // enough for ONE request to the client

                //            string strMessage;
                byte[] bRequest;                        // Client -> Server
                int cbRequestBytes;
                byte[] bReply = new byte[BUFFER_SIZE];  // Server -> Client
                int cbBytesRead, cbReplyBytes;

                // Send one message to the pipe.

                // '\0' is appended in the end because the client may be a native
                // C++ program.
                //            strMessage = "Default request from client\0";
                bRequest = Encoding.Unicode.GetBytes(strMessage);
                cbRequestBytes = bRequest.Length;
                if (pipeClient.CanWrite)
                {
                    pipeClient.Write(bRequest, 0, cbRequestBytes);
                }
                pipeClient.Flush();

                /*
                Console.WriteLine("Sends {0} bytes; Message: \"{1}\"",
                    cbRequestBytes, strMessage.TrimEnd('\0'));
                 */

                // Receive one message from the pipe.

                cbReplyBytes = BUFFER_SIZE;
                do
                {
                    if (pipeClient.CanRead)
                    {
                        cbBytesRead = pipeClient.Read(bReply, 0, cbReplyBytes);

                        // Unicode-encode the byte array and trim all the '\0' chars 
                        // at the end.
                        strMessage = Encoding.Unicode.GetString(bReply).TrimEnd('\0');
                        /*
                        Console.WriteLine("Receives {0} bytes; Message: \"{1}\"",
                            cbBytesRead, strMessage);
                         */
                    }
                }
                while (!pipeClient.IsMessageComplete);

            }
            catch (TimeoutException ex)
            {
                Console.WriteLine("Unable to open named pipe {0}\\{1}",
                   strServerName, strPipeName);
                Console.WriteLine(ex.Message);
                return "-1";
            }
            catch (Exception ex)
            {
                Console.WriteLine("The client throws the error: {0}", ex.Message);
                return "-1";
            }
            finally
            {
            }

            return strMessage;
        }

    }
}

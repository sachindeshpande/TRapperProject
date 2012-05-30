/****************************** Module Header ******************************\
* Module Name:	Program.cs
* Project:		CSNamedPipeClient
* Copyright (c) Microsoft Corporation.
* 
* Named pipe is a mechanism for one-way or bi-directional inter-process 
* communication between the pipe server and one or more pipe clients in the
* local machine or across the computers in the intranet:
* 
* PIPE_ACCESS_INBOUND:
* Client (GENERIC_WRITE) ---> Server (GENERIC_READ)
* 
* PIPE_ACCESS_OUTBOUND:
* Client (GENERIC_READ) <--- Server (GENERIC_WRITE)
* 
* PIPE_ACCESS_DUPLEX:
* Client (GENERIC_READ or GENERIC_WRITE, or both) <--> 
* Server (GENERIC_READ and GENERIC_WRITE)
* 
* .NET supports named pipes in two ways:
* 
* 1. P/Invoke the native APIs.
* 
* By P/Invoke-ing the native APIs from .NET, we can mimic the code logic  
* in CppNamedPipeClient to connect to the pipe server.
* 
* This sample first connects to the named pipe, \\.\pipe\HelloWorld, with 
* the GENERIC_READ and GENERIC_WRITE permissions. The client writes a 
* message to the pipe server and receives its response.
* 
* 2. System.IO.Pipes Namespace
* 
* In .NET Framework 3.5, the namespace System.IO.Pipes and a set of classes 
* (e.g. PipeStream, NamedPipeClientStream) are added to .NET BCL. These 
* classes make the programming of named pipe in .NET much easier and safer  
* than P/Invoke-ing the native APIs directly.
* 
* BCLSystemIOPipeClient first connects a named pipe,\\.\pipe\HelloWorld.Then 
* writes a message to the pipe server and receives its response.
* 
* This source is subject to the Microsoft Public License.
* See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
* All other rights reserved.
* 
* THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
* EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED 
* WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
\***************************************************************************/

#region Using directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Pipes;
using System.Security.Principal;
using System.Threading;

using ProjectCommon;

#endregion

namespace CSNamedPipeClient
{
    public class NamedPipe
    {
        public const int BUFFER_SIZE = 1024;  // 1 KB
        public int DATA_BUFFER_SIZE = 16384;  // 1 KB

        public const string WIIMOTE_COMMAND_PIPE_NAME = "wiimoteCommands";
        public const string WIIMOTE_DATA_PIPE_NAME = "wiimoteData";

        protected NamedPipeClientStream pipeClient = null;

        protected String strServerName = ".";
        protected String strPipeName;

        /// <summary>
        /// Main
        /// </summary>
        static void Main(string[] args)
        {
            //PInvokeNativePipeClient();

            // [-or-]

            new NamedPipe().startPipe();
        }

        public NamedPipe()
        {
        }

        public bool startPipe()
        {
        /////////////////////////////////////////////////////////////////////
        // Try to open a named pipe.
        // 

        // Prepare the pipe name
        strServerName = ".";

        try
        {
            pipeClient = new NamedPipeClientStream(
                strServerName,              // The server name
                strPipeName,                // The unique pipe name
                PipeDirection.InOut,        // The pipe is bi-directional   
                PipeOptions.None,           // No additional parameters

                //The server process cannot obtain identification information about 
                //the client, and it cannot impersonate the client.
                TokenImpersonationLevel.Anonymous);

            pipeClient.Connect(ProjectConstants.WIIMOTE_PIPE_TIMEOUT); // set TimeOut for connection : Original value 60000
            pipeClient.ReadMode = PipeTransmissionMode.Message;

            Console.WriteLine(@"The named pipe, \\{0}\{1}, is connected.",
                strServerName, strPipeName);
/*
            wiimoteDataClient = new NamedPipeClientStream(
                strServerName,              // The server name
                strWiimoteDataPipeName,                // The unique pipe name
                PipeDirection.InOut,        // The pipe is bi-directional   
                PipeOptions.None,           // No additional parameters

                //The server process cannot obtain identification information about 
                //the client, and it cannot impersonate the client.
                TokenImpersonationLevel.Anonymous);

            wiimoteDataClient.Connect(ProjectConstants.WIIMOTE_PIPE_TIMEOUT); // set TimeOut for connection : Original value 60000
            wiimoteDataClient.ReadMode = PipeTransmissionMode.Message;

            Console.WriteLine(@"The named pipe, \\{0}\{1}, is connected.",
                strServerName, strWiimoteDataPipeName);

            Thread t = new Thread(new ThreadStart(listenWiimoteDataCommands));
            t.Start();
 * */
        }
        catch (TimeoutException ex)
        {
            Console.WriteLine("Unable to open named pipe {0}\\{1}",
               strServerName, strPipeName);
            Console.WriteLine(ex.Message);
            throw new PipeCommunicationException(ex);
        }
        catch (Exception ex)
        {
            Console.WriteLine("The client throws the error: {0}", ex.Message);
            throw new PipeCommunicationException(ex);
        }

        return true;
      }


        public bool checkPipeStatus()
        {
            return pipeClient.CanWrite;
        }

        public void closePipe()
        {
            /////////////////////////////////////////////////////////////////
            // Close the pipe.
            // 

            if (pipeClient != null)
                pipeClient.Close();
        }
    }

}
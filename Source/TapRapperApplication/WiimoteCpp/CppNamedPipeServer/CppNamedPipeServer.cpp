/****************************** Module Header ******************************\
* Module Name:	CppNamedPipeServer.cpp
* Project:		CppNamedPipeServer
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
* This sample demonstrates a named pipe server, \\.\pipe\HelloWorld, that  
* supports PIPE_ACCESS_DUPLEX. It first creates such a named pipe, then it 
* listens to the client's connection. When a client is connected, the server 
* attempts to read the client's requests from the pipe and write a response.
* 
* This source is subject to the Microsoft Public License.
* See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
* All other rights reserved.
* 
* THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
* EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED 
* WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
\***************************************************************************/

#include <stdio.h>
#include <stdlib.h>

#include "CppNamedPipeServer.h"
#include "..\FileLogger\FileLogger.h"

#define BUFFER_SIZE		4096 // 4K bytes
#define DATA_BUFFER_SIZE	16384 // 4K bytes


/*
int _tmain(int argc, _TCHAR* argv[])
{
	WiimoteNamedPipeServer * pipeServer = new WiimoteNamedPipeServer();
	pipeServer->StartPipeServer();
}
*/

WiimoteNamedPipeServer::WiimoteNamedPipeServer()
{

}

WiimoteNamedPipeServer::~WiimoteNamedPipeServer()
{
	if(hPipe != NULL)
	{
		DisconnectNamedPipe(hPipe); 
		CloseHandle(hPipe);
	}
}


int WiimoteNamedPipeServer::StartPipeServer()
{
	/////////////////////////////////////////////////////////////////////////
	// Create a named pipe.
	// 

	// Prepare the pipe name
	CString strPipeName;
	strPipeName.Format(_T("\\\\%s\\pipe\\%s"), 
		_T("."),			// Server name
		unformattedPipeName	// Pipe name
		);


	// Prepare the security attributes

	// If lpSecurityAttributes of CreateNamedPipe is NULL, the named pipe 
	// gets a default security descriptor and the handle cannot be inherited. 
	// The ACLs in the default security descriptor for a named pipe grant 
	// full control to the LocalSystem account, administrators, and the 
	// creator owner. They also grant read access to members of the Everyone 
	// group and the anonymous account. In other words, with NULL as the 
	// security attributes, the named pipe cannot be connected with the 
	// WRITE permission across the network, or from a local client running 
	// as a lower integiry level. Here, we fill the security attributes to 
	// grant EVERYONE all access (not just the connect access) to the server 
	// This solves the cross-network and cross-IL issues, but it creates 
	// a security hole right there: the clients have WRITE_OWNER access and 
	// then the server just lose the control of the pipe object.
	SECURITY_ATTRIBUTES sa;
	sa.lpSecurityDescriptor = (PSECURITY_DESCRIPTOR)malloc(
		SECURITY_DESCRIPTOR_MIN_LENGTH);
	InitializeSecurityDescriptor(sa.lpSecurityDescriptor, 
		SECURITY_DESCRIPTOR_REVISION);
	// ACL is set as NULL in order to allow all access to the object.
	SetSecurityDescriptorDacl(sa.lpSecurityDescriptor, TRUE, NULL, FALSE);
	sa.nLength = sizeof(sa);
	sa.bInheritHandle = TRUE;

	// Create the named pipe.
	hPipe = CreateNamedPipe(
		strPipeName,				// The unique pipe name. This string must 
									// have the form of \\.\pipe\pipename
		PIPE_ACCESS_DUPLEX,			// The pipe is bi-directional; both  
									// server and client processes can read 
									// from and write to the pipe
		PIPE_TYPE_MESSAGE |			// Message type pipe 
		PIPE_READMODE_MESSAGE |		// Message-read mode 
		PIPE_WAIT,					// Blocking mode is enabled
		PIPE_UNLIMITED_INSTANCES,	// Max. instances

		// These two buffer sizes have nothing to do with the buffers that 
		// are used to read from or write to the messages. The input and 
		// output buffer sizes are advisory. The actual buffer size reserved 
		// for each end of the named pipe is either the system default, the 
		// system minimum or maximum, or the specified size rounded up to the 
		// next allocation boundary. The buffer size specified should be 
		// small enough that your process will not run out of nonpaged pool, 
		// but large enough to accommodate typical requests.
		BUFFER_SIZE,				// Output buffer size in bytes
		BUFFER_SIZE,				// Input buffer size in bytes

		NMPWAIT_USE_DEFAULT_WAIT,	// Time-out interval
		&sa							// Security attributes
		);

	if (hPipe == INVALID_HANDLE_VALUE)
	{
		_tprintf(_T("Unable to create wiimote Command pipe %s w/err 0x%08lx\n"), 
			strPipeName, GetLastError());
		return 1;
	}
	_tprintf(_T("The named pipe, %s, is created.\n"), strPipeName);
	FileLogger::getFileLogger()->logEntry("Wiimote pipe is created");


	Sleep(5000);
	/////////////////////////////////////////////////////////////////////////
	// Wait for the client to connect.
	// 
	
	_putts(_T("Waiting for the client's connection..."));
	FileLogger::getFileLogger()->logEntry("Waiting for the client's connection...");

	BOOL bPipeConnected = ConnectNamedPipe(hPipe, NULL) ? 
		TRUE : (GetLastError() == ERROR_PIPE_CONNECTED);

	if (!bPipeConnected)
	{
		_tprintf(_T(
			"Error occurred while connecting to the client: 0x%08lx\n"
			), GetLastError()); 
		CloseHandle(hPipe);
		return 1;
	}
	FileLogger::getFileLogger()->logEntry("Wiimote Pipe Connected");

	run();
}



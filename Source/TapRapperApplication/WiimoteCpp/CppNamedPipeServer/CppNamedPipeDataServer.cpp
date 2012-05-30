#include <stdio.h>
#include <stdlib.h>

#include "CppNamedPipeDataServer.h"
#include "..//FileLogger//FileLogger.h"
#include "..//ProjectCommon//ProjectConstants.h"


WiimoteNamedPipeDataServer::WiimoteNamedPipeDataServer()
{
	memset(unformattedPipeName,'\0',MAXIMUM_PIPE_NAME_MAXIMUM_LENGTH);
	wcscpy(unformattedPipeName,WIIMOTE_DATA_PIPE_NAME);
}

WiimoteNamedPipeDataServer::~WiimoteNamedPipeDataServer()
{

}

void WiimoteNamedPipeDataServer::sendWiimoteData(DoubleArrayDataPacket * p_DataPacket)
{
	wiimoteDataQueue.push(p_DataPacket);
}


bool WiimoteNamedPipeDataServer::run() {

	HANDLE			 WiimotePipeDataThread; 
	WiimotePipeDataThread = (HANDLE)_beginthreadex(NULL, 0, WiimoteNamedPipeDataServer::StartSendingData,
												 this, 0, NULL);
	_ASSERT(WiimotePipeDataThread);
	if(!WiimotePipeDataThread)
		return false;
//	SetThreadPriority(WiimotePipeCommandThread, WORKER_THREAD_PRIORITY);

	return true;
}

	unsigned __stdcall WiimoteNamedPipeDataServer::StartSendingData(void * params)
	{
		WiimoteNamedPipeDataServer * server = (WiimoteNamedPipeDataServer *)params;	
		/////////////////////////////////////////////////////////////////////////
		// Send Wiimote Data Packets
		// 
		
		// A char buffer of BUFFER_SIZE chars, aka BUFFER_SIZE * sizeof(TCHAR) 
		// bytes. The buffer should be big enough for ONE data packet.

		TCHAR chRequest[BUFFER_SIZE];	// Client -> Server
		DWORD cbBytesRead, cbRequestBytes;
		TCHAR chReply[BUFFER_SIZE];		// Server -> Client
		DWORD cbBytesWritten, cbReplyBytes;

		BOOL bResult;

		FileLogger::getFileLogger()->logEntry("Starting to send packets");

		while (TRUE)
		{
			/*
			while(server->wiimoteDataQueue.size() == 0)
				Sleep(1);

			EnterCriticalSection(&cs);
			DoubleArrayDataPacket * p_DataPacket = (DoubleArrayDataPacket *)server->wiimoteDataQueue.front();
			server->wiimoteDataQueue.pop();
			LeaveCriticalSection(&cs);
			*/

			DoubleArrayDataPacket * p_DataPacket = (DoubleArrayDataPacket *)server->wiimoteDataQueue.popAndReturn();
			server->sendWiimoteDataToPipe(p_DataPacket);
			delete p_DataPacket;
		}


		/////////////////////////////////////////////////////////////////////////
		// Flush the pipe to allow the client to read the pipe's contents before
		// disconnecting. Then disconnect the pipe, and close the handle to this 
		// pipe instance. 
		// 

		FlushFileBuffers(server->hPipe); 
		DisconnectNamedPipe(server->hPipe); 
		CloseHandle(server->hPipe);

		return 0;
}
	


void WiimoteNamedPipeDataServer::sendWiimoteDataToPipe(DoubleArrayDataPacket * p_DataPacket)
{
		// A char buffer of BUFFER_SIZE chars, aka BUFFER_SIZE * sizeof(TCHAR) 
		// bytes. The buffer should be big enough for ONE request from a client.
		DWORD cbBytesWritten, cbReplyBytes;
		BOOL bResult;

		memset(chReply,'\0',DATA_BUFFER_SIZE);

		p_DataPacket->getCommaSeparatedDataPacket(chReply);

//		FileLogger::getFileLogger()->logEntry("####################### Logging sendWiimoteData Data #######################\n");
//		FileLogger::getFileLogger()->logEntry(chReply);
//		FileLogger::getFileLogger()->logEntry("\n#####################################################################");

		cbReplyBytes = wcslen(chReply);

		p_DataPacket->log(_T("In WiimoteNamedPipeDataServer::sendWiimoteDataToPipe. Before Writing to Pipe"));
	// Write the response to the pipe.

	bResult = WriteFile(		// Write to the pipe.
				hPipe,					// Handle of the pipe
				chReply,				// Buffer to write to 
				cbReplyBytes*2,			// Number of bytes to write 
				&cbBytesWritten,		// Number of bytes written 
				NULL);					// Not overlapped I/O 

	if (!bResult || (cbReplyBytes * 2) != cbBytesWritten) 
	{
		_tprintf(_T("WriteFile failed w/err 0x%08lx\n"), GetLastError());
		return;
	}

	_tprintf(_T("Replies %ld bytes; Message: \"%s\"\n"), 
	cbBytesWritten, chReply);
}
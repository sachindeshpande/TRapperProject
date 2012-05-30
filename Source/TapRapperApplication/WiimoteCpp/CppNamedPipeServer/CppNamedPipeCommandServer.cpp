#include <stdio.h>
#include <stdlib.h>

#include "CppNamedPipeCommandServer.h"
#include "..//FileLogger//FileLogger.h"

#include <process.h>	// for _beginthreadex()
#include <windows.h>


#define MAX_FILE_NAME_SIZE		256 // 4K bytes
#define MAX_INT_VALUE_SIZE		64 // 4K bytes


WiimoteNamedPipeCommandServer::WiimoteNamedPipeCommandServer(IWiimoteData * wiimoteData1,IWiimoteData * wiimoteData2)
{
	m_WiimoteData1 = wiimoteData1;
	m_WiimoteData2 = wiimoteData2;

	logWiimoteData = false;
	wiimotesSwitched = false;
	recordingStatus = false;
	beatsPerMinute = 0;
	beatsPerBar = 0;
	numBars = 0;
	leadIn = 0;
	frequencyInterval = 0;
	wiimoteDataFile = NULL;

	memset(unformattedPipeName,'\0',MAXIMUM_PIPE_NAME_MAXIMUM_LENGTH);
	wcscpy(unformattedPipeName,WIIMOTE_COMMAND_PIPE_NAME);

	m_WiimoteSimulationFileName = new TCHAR[MAX_FILE_NAME_SIZE];

	m_WiimoteSimulationMode = false;
	m_WiimoteSimulationModeSet = false;

	m_WiimoteDataMode = WIIMOTE_MOTIONPLUS_ACC_DATA_MODE;
}

WiimoteNamedPipeCommandServer::~WiimoteNamedPipeCommandServer()
{
	if(wiimoteDataFile != NULL)
		delete wiimoteDataFile;

	if(m_WiimoteSimulationFileName != NULL)
		delete m_WiimoteSimulationFileName;	
}

bool WiimoteNamedPipeCommandServer::run() {

	HANDLE			 WiimotePipeCommandThread; 
	WiimotePipeCommandThread = (HANDLE)_beginthreadex(NULL, 0, WiimoteNamedPipeCommandServer::StartReadingCommands,
												 this, 0, NULL);
	_ASSERT(WiimotePipeCommandThread);
	if(!WiimotePipeCommandThread)
		return false;
//	SetThreadPriority(WiimotePipeCommandThread, WORKER_THREAD_PRIORITY);

}

int WiimoteNamedPipeCommandServer::parseMessage(DWORD cbBytesRead, TCHAR * chRequest,TCHAR *chReply)
{
//	FileLogger::getFileLogger()->logEntry("In parseMessage");

	//Get method name
	TCHAR * methodName = new TCHAR[BUFFER_SIZE];
	int index = 0;
	TCHAR msg[256];
	TCHAR returnValue[MAX_INT_VALUE_SIZE];
	
	getNextMessageBlock(chRequest,&index,methodName);

	if(wcscmp(methodName,STOP_WIIMOTE_PROCESS_METHOD_NAME) == 0)
	{
		_tprintf(_T("Received Stop Wiimote Process Message\n"));
		FileLogger::getFileLogger()->logEntry("Received Stop Wiimote Process Message");
		exit(0);
	}
	else if(wcscmp(methodName,STOP_DATA_SENDING_METHOD_NAME) == 0)
	{
		logWiimoteData = false;
		recordingStatus = false;
		delete wiimoteDataFile;
		wiimoteDataFile = NULL;
		_tprintf(_T("Received Stop Logging Message\n"));
		FileLogger::getFileLogger()->logEntry("Received Stop Logging Message");
	}
	else if(wcscmp(methodName,SET_WIIMOTE_SIMULATION_MODE_METHOD_NAME) == 0)
	{
		_tprintf(_T("Received Set Wiimote Simulation Message\n"));
		FileLogger::getFileLogger()->logEntry("Received Set Wiimote Simulation Message");

		TCHAR *blockName = new TCHAR[BUFFER_SIZE];

		getNextMessageBlock(chRequest,&index,blockName);
		if(wcscmp(blockName,_T("True")) == 0)
		{
			m_WiimoteSimulationMode  = true;
			memset(m_WiimoteSimulationFileName,'\0',MAX_FILE_NAME_SIZE);
			getNextMessageBlock(chRequest,&index,m_WiimoteSimulationFileName);
			m_WiimoteData1->setConnectionState(WIIMOTE_GOOD_DATA_STATE);
			m_WiimoteData2->setConnectionState(WIIMOTE_GOOD_DATA_STATE);
		}
		else
			m_WiimoteSimulationMode  = false;

		m_WiimoteSimulationModeSet = true;
		delete blockName;
	}	
	else if(wcscmp(methodName,SET_WIIMOTE_DATA_MODE_METHOD_NAME) == 0)
	{
		_tprintf(_T("Received Set Wiimote Data Mode Message\n"));
		FileLogger::getFileLogger()->logEntry("Received Set Wiimote Data Mode Message");

		TCHAR *blockName = new TCHAR[BUFFER_SIZE];

		getNextMessageBlock(chRequest,&index,blockName);
		if(wcscmp(blockName,WIIMOTE_MOTIONPLUS_ACC_DATA_MODE_STRING) == 0)
			m_WiimoteDataMode = WIIMOTE_MOTIONPLUS_ACC_DATA_MODE;
		else
			m_WiimoteDataMode = WIIMOTE_MOTIONPLUS_ACC_IR_DATA_MODE;

		delete blockName;
	}	
	else if(wcscmp(methodName,START_DATA_SENDING_METHOD_NAME) == 0)
	{

		logWiimoteData = false;
		/*
		wiimoteDataFile = new TCHAR[MAX_FILE_NAME_SIZE];
		getNextMessageBlock(chRequest,&index,wiimoteDataFile);
		
		//Get method name
		TCHAR *blockName = new TCHAR[BUFFER_SIZE];


		//TODO : Need to correct memeory allocation for temp to avoid leak
		getNextMessageBlock(chRequest,&index,blockName);
		beatsPerMinute = _ttoi(blockName);

		getNextMessageBlock(chRequest,&index,blockName);
		beatsPerBar = _ttoi(blockName);

		getNextMessageBlock(chRequest,&index,blockName);
		numBars = _ttoi(blockName);

		getNextMessageBlock(chRequest,&index,blockName);
		leadIn = _ttoi(blockName);		

		getNextMessageBlock(chRequest,&index,blockName);
		frequencyInterval = _ttoi(blockName);		

		
		TCHAR * WiimoteSwitchedState  = getNextMessageBlock(chRequest,&index,blockName);
		if(wcscmp(WiimoteSwitchedState,_T("true")) == 0)
			wiimotesSwitched = true;
		else
			wiimotesSwitched = false;
			*/

		_tprintf(_T("Received Start Logging Message : Filename = \"%s\"\n"),wiimoteDataFile);
		swprintf(msg,_T("Received Start Logging Message : Filename = %s\tbeatsPerMinute = %d\tbeatsPerBar = %d\tnumBars =%d"),wiimoteDataFile,beatsPerMinute,beatsPerBar,numBars);
		FileLogger::getFileLogger()->logEntry(msg);

//		delete blockName;
	}
	else if(wcscmp(methodName,CHECK_WIIMOTE_STATE_METHOD_NAME) == 0)
	{
//		_tprintf(_T("Received Check Wiimote State Message\n"));
		
//		FileLogger::getFileLogger()->logEntry("Received Check Wiimote State Message");
//		swprintf(msg,_T("WiimoteState = %d"),m_WiimoteData1.m_WiimoteState.m_WiimoteConnectionState);
//		FileLogger::getFileLogger()->logEntry(msg);

		_stprintf(chReply,_T("%d,%d,%d,%d"),m_WiimoteData1->getConnectionState(),m_WiimoteData1->getBatteryLevel(),
			m_WiimoteData2->getConnectionState(),m_WiimoteData2->getBatteryLevel());
		return 0;
	}
	else if(wcscmp(methodName,START_RECORDING_METHOD_NAME) == 0)
	{
		_tprintf(_T("Received Start Recording Message\n"));		
		FileLogger::getFileLogger()->logEntry("Received Start Recording Message");
		recordingStatus = true;
	}
	else if(wcscmp(methodName,SET_CALIBRATION_METHOD_NAME) == 0)
	{
		_tprintf(_T("Received Set Calibration Message\n"));		
		FileLogger::getFileLogger()->logEntry("Received Set Calibration Message");

		TCHAR *blockName = new TCHAR[BUFFER_SIZE];

		getNextMessageBlock(chRequest,&index,blockName);
		double l_Wiimote1AccXCalibration = _wtof(blockName);

		getNextMessageBlock(chRequest,&index,blockName);
		double l_Wiimote1AccYCalibration = _wtof(blockName);

		getNextMessageBlock(chRequest,&index,blockName);
		double l_Wiimote1AccZCalibration = _wtof(blockName);

		getNextMessageBlock(chRequest,&index,blockName);
		double l_Wiimote1YawCalibration = _wtof(blockName);

		getNextMessageBlock(chRequest,&index,blockName);
		double l_Wiimote1PitchCalibration = _wtof(blockName);

		getNextMessageBlock(chRequest,&index,blockName);
		double l_Wiimote1RollCalibration = _wtof(blockName);

		getNextMessageBlock(chRequest,&index,blockName);
		double l_Wiimote2AccXCalibration = _wtof(blockName);

		getNextMessageBlock(chRequest,&index,blockName);
		double l_Wiimote2AccYCalibration = _wtof(blockName);

		getNextMessageBlock(chRequest,&index,blockName);
		double l_Wiimote2AccZCalibration = _wtof(blockName);

		getNextMessageBlock(chRequest,&index,blockName);
		double l_Wiimote2YawCalibration = _wtof(blockName);

		getNextMessageBlock(chRequest,&index,blockName);
		double l_Wiimote2PitchCalibration = _wtof(blockName);

		getNextMessageBlock(chRequest,&index,blockName);
		double l_Wiimote2RollCalibration = _wtof(blockName);

		m_WiimoteData1->setCalibration(l_Wiimote1AccXCalibration,l_Wiimote1AccYCalibration,l_Wiimote1AccZCalibration,
			l_Wiimote1YawCalibration,l_Wiimote1PitchCalibration,l_Wiimote1RollCalibration);
		m_WiimoteData2->setCalibration(l_Wiimote2AccXCalibration,l_Wiimote2AccYCalibration,l_Wiimote2AccZCalibration,
			l_Wiimote2YawCalibration,l_Wiimote2PitchCalibration,l_Wiimote2RollCalibration);

		delete blockName;
	}	


	delete methodName;
	swprintf(chReply,_T("0"));
	return 0;
}

	unsigned __stdcall WiimoteNamedPipeCommandServer::StartReadingCommands(void * params)
	{
		WiimoteNamedPipeCommandServer * server = (WiimoteNamedPipeCommandServer *)params;
		/////////////////////////////////////////////////////////////////////////
		// Read client requests from the pipe and write the response.
		// 
		
		// A char buffer of BUFFER_SIZE chars, aka BUFFER_SIZE * sizeof(TCHAR) 
		// bytes. The buffer should be big enough for ONE request from a client.

		TCHAR chRequest[BUFFER_SIZE];	// Client -> Server
		DWORD cbBytesRead, cbRequestBytes;
		TCHAR chReply[BUFFER_SIZE];		// Server -> Client
		DWORD cbBytesWritten, cbReplyBytes;

		BOOL bResult;

		FileLogger::getFileLogger()->logEntry("Starting to read messages");

		while (TRUE)
		{
			// Receive one message from the pipe.

			memset(chRequest,'\0',BUFFER_SIZE);
			cbRequestBytes = sizeof(TCHAR) * BUFFER_SIZE;

			printf("Receiving bytes ...\n");
			bResult = ReadFile(			// Read from the pipe.
				server->hPipe,					// Handle of the pipe
				chRequest,				// Buffer to receive data
				cbRequestBytes,			// Size of buffer in bytes
				&cbBytesRead,			// Number of bytes read
				NULL);					// Not overlapped I/O

			if (!bResult/*Failed*/ || cbBytesRead == 0/*Finished*/) 
				break;
			
			_tprintf(_T("Receives %ld bytes; Message: \"%s\"\n"), 
				cbBytesRead, chRequest);

			memset(chReply,'\0',BUFFER_SIZE);

			int returnStatus =  server->parseMessage(cbBytesRead, chRequest,chReply);

			// Prepare the response.
/*
			StringCchCopy(
				chReply, BUFFER_SIZE, _T("Default response from server"));
*/

//			_itow(returnStatus,chReply,10);
			cbReplyBytes = wcslen(chReply);

			// Write the response to the pipe.

			bResult = WriteFile(		// Write to the pipe.
				server->hPipe,					// Handle of the pipe
				chReply,				// Buffer to write to 
				cbReplyBytes*2,			// Number of bytes to write 
				&cbBytesWritten,		// Number of bytes written 
				NULL);					// Not overlapped I/O 

			if (!bResult/*Failed*/ || (cbReplyBytes * 2) != cbBytesWritten/*Failed*/) 
			{
				_tprintf(_T("WriteFile failed w/err 0x%08lx\n"), GetLastError());
				break;
			}

			_tprintf(_T("Replies %ld bytes; Message: \"%s\"\n"), 
				cbBytesWritten, chReply);
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



TCHAR * WiimoteNamedPipeCommandServer::getNextMessageBlock(TCHAR * chRequest,int *index,TCHAR *blockName)
{
	//Get method name
//	TCHAR *blockName = new TCHAR[BUFFER_SIZE];	// Client -> Server
	int blockIndex = 0;

	while(TRUE)
	{
		TCHAR currentChar = chRequest[*index];
		if(currentChar != ',' && currentChar != ';')
		{
			blockName[blockIndex++] = currentChar;
			*index = *index + 1;
		}
		else 
		{
			blockName[blockIndex] = '\0';
			*index = *index + 1;
			break;
		}
	}

	return blockName;
	
}


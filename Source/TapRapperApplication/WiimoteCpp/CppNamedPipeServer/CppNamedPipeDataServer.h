#pragma region Includes
#include <stdio.h>
#include <tchar.h>
#include <windows.h>
#include <atlstr.h>
#include <strsafe.h>
#pragma endregion

#include "CppNamedPipeServer.h"
#include "..\ProjectCommon\DoubleArrayDataPacket.h"
#include "..\ProjectCommon\ProjectConstants.h"
#include "..\ProjectCommon\SynchronizedQueue.h"
#include <vector>
#include <queue>


using namespace std;


#ifndef WIIMOTE_NAMEDPIPE_DATA_SERVER_H
#define WIIMOTE_NAMEDPIPE_DATA_SERVER_H

const TCHAR WIIMOTE_DATA_PIPE_NAME[] = _T("wiimoteData");

class WiimoteNamedPipeDataServer : public WiimoteNamedPipeServer
{

public :

	
	WiimoteNamedPipeDataServer();
	~WiimoteNamedPipeDataServer();
	void sendWiimoteData(DoubleArrayDataPacket * p_DataPacket);

	bool run();


private :

	TCHAR chReply[DATA_BUFFER_SIZE];		// Server -> Client

	SynchronizedQueue wiimoteDataQueue;

	void sendWiimoteDataToPipe(DoubleArrayDataPacket * p_DataPacket);

	static unsigned __stdcall WiimoteNamedPipeDataServer::StartSendingData(void * params);
	
};


#endif

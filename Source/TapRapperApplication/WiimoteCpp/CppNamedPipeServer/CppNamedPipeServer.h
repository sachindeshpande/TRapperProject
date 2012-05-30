#pragma region Includes
#include <stdio.h>
#include <tchar.h>
#include <windows.h>
#include <atlstr.h>
#include <strsafe.h>
#pragma endregion

#include "CppNamedPipeServerBase.h"
#include <vector>
#include <queue>


using namespace std;


#ifndef WIIMOTE_NAMEDPIPE_SERVER_H
#define WIIMOTE_NAMEDPIPE_SERVER_H

#define BUFFER_SIZE		4096 // 4K bytes

const int MAXIMUM_PIPE_NAME_MAXIMUM_LENGTH = 20;

//const int WIIMOTE_DATA_PACKET_SIZE = 20;


class WiimoteNamedPipeServer
{

public :

	
	WiimoteNamedPipeServer();
	~WiimoteNamedPipeServer();
	int StartPipeServer();

protected :

	HANDLE hPipe;
	TCHAR unformattedPipeName[MAXIMUM_PIPE_NAME_MAXIMUM_LENGTH];

	virtual bool run() = 0;

	
};


#endif

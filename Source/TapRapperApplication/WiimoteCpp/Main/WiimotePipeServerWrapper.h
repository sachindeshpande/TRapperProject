
#include "..\CppNamedPipeServer\CppNamedPipeCommandServer.h"
#include "..\CppNamedPipeServer\CppNamedPipeDataServer.h"
#include "..\CppNamedPipeServer\CppNamedPipeServer.h"
#include "WiimoteServerWrapper.h"
#include "wiimote.h"
#include "WiimoteData.h"
#include "..\ProjectCommon\DoubleArrayDataPacket.h"
#include <windows.h>
 

#ifndef WIIMOTE_PIPE_SERVER_WRAPPER_H
#define WIIMOTE_PIPE_SERVER_WRAPPER_H

#define WIIMOTE_DATA_SIZE 22
#define WIIMOTE_LOGGING_STOPPED 1

class WiimotePipeServerWrapper : public WiimoteServerWrapper
{
public :
	WiimotePipeServerWrapper();
	~WiimotePipeServerWrapper();

	void startWiimoteServers(WiimoteData *remote1,WiimoteData *remote2);
	void startLogging(WiimoteData *remote1,WiimoteData *remote2);
	void stopLogging();
	bool sendWiimoteData(WiimoteData *wiimoteData1,WiimoteData *wiimoteData2,int sequenceNumber,DWORD current_time);


	void setWiimoteConnectionState(WiimoteData *remote1,WiimoteData *remote2);

	bool isWiimoteLoggingOn();
	TCHAR * getWiimoteLogfileName();
	int getBeatsPerMinute();
	int getBeatsPerBar();
	int getNumBars();
	int getLeadIn();
	int getFrequencyInterval();

	bool isWiimoteSimulationModeOn();
	TCHAR * getWiimoteSimulationFileName();

private :
	WiimoteNamedPipeCommandServer * m_CommandPipeServer;
	WiimoteNamedPipeDataServer * m_DataPipeServer;

	double * wiimoteDataPacket;
	//This variable is required to store the previous state of logging.
	//We cannot depend on only m_CommandPipeServer->isWiimoteLoggingOn()
	//As when you need to stop logging you can check if m_CommandPipeServer->isWiimoteLoggingOn() changes to false, however how would you differentiate if
	//	a) Logging was never ON and 
	//	b) Logging was ON and it changed to false

	bool m_WiimoteLoggingOn;

	bool m_WiimoteCppFileLoggingOn;


	void sendDataPacket(WiimoteData *wiimoteData1,WiimoteData *wiimoteData2,int sequenceNumber,DWORD current_time,bool p_EndStream);

	void openLoggingFile(WiimoteData *remote1,WiimoteData *remote2);
	void closeLoggingFile();
	void logWiimoteFileHeader(WiimoteData *remote1,WiimoteData *remote2,int beatsPerMinute, int beatsPerBar, int numBars,int leadIn);
	void logWiimoteData(WiimoteData *remote1,WiimoteData *remote2,int sequenceNumber,DWORD current_time);

	FILE *fp;

};

#endif

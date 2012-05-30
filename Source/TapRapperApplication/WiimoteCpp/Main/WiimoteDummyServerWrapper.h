#include "WiimoteServerWrapper.h"
#include "wiimote.h"
#include "WiimoteData.h"

#ifndef WIIMOTE_DUMMY_SERVER_WRAPPER_H
#define WIIMOTE_DUMMY_SERVER_WRAPPER_H


class WiimoteDummyServerWrapper : public WiimoteServerWrapper
{
public :
	WiimoteDummyServerWrapper() {}
	~WiimoteDummyServerWrapper() {}

	void startLogging(WiimoteData *remote1,WiimoteData *remote2) {}
	void stopLogging() {}
	void startWiimoteServers(WiimoteData *remote1,WiimoteData *remote2) {}
	bool sendWiimoteData(WiimoteData *wiimoteData1,WiimoteData *wiimoteData2,int sequenceNumber,DWORD current_time) {return false;}


	void setWiimoteConnectionState(WiimoteData *remote1,WiimoteData *remote2) {}

	bool isWiimoteLoggingOn() {return true; }
	bool isWiimoteRecordingOn() {return true; }
	TCHAR * getWiimoteLogfileName() {return _T("DummyFile.csv");}
	int getBeatsPerMinute() {return 0;}
	int getBeatsPerBar() {return 0;}
	int getNumBars() {return 0;}
	int getLeadIn() {return 0;}
	int getFrequencyInterval() {return 0;}

	bool isWiimoteSimulationModeOn() {return false; }
	TCHAR * getWiimoteSimulationFileName() {return NULL; }

};

#endif


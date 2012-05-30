#include "wiimote.h"
#include "WiimoteData.h"

#ifndef WIIMOTE_SERVER_WRAPPER_H
#define WIIMOTE_SERVER_WRAPPER_H

class WiimoteServerWrapper
{
public :
	WiimoteServerWrapper() {}
	~WiimoteServerWrapper() {}

	virtual void startWiimoteServers(WiimoteData *remote1,WiimoteData *remote2) = 0;
	virtual void startLogging(WiimoteData *remote1,WiimoteData *remote2) = 0;
	virtual void stopLogging() = 0;
	virtual bool  sendWiimoteData(WiimoteData *wiimoteData1,WiimoteData *wiimoteData2,int sequenceNumber,DWORD current_time) = 0;

	virtual void setWiimoteConnectionState(WiimoteData *remote1,WiimoteData *remote2) = 0;

	virtual bool isWiimoteLoggingOn() = 0;
	virtual TCHAR * getWiimoteLogfileName() = 0;
	virtual int getBeatsPerMinute() = 0;
	virtual int getBeatsPerBar() = 0;
	virtual int getNumBars() = 0;
	virtual int getLeadIn() = 0;
	virtual int getFrequencyInterval()= 0;

	virtual bool isWiimoteSimulationModeOn() = 0;
	virtual TCHAR * getWiimoteSimulationFileName() = 0;

};

#endif

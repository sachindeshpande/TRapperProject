
#ifndef WIIMOTE_NAMEDPIPE_SERVER_BASE_H
#define WIIMOTE_NAMEDPIPE_SERVER_BASE_H

class WiimoteNamedPipeServerBase
{
public :

	
	WiimoteNamedPipeServerBase() {}
	virtual int StartPipeServer() = 0;
	virtual int StartReadingCommands() = 0;

	virtual void run() = 0;

	virtual void setWiimoteConnectionState(int wiiConnectionState) = 0;

	virtual bool isWiimoteLoggingOn() = 0;
	virtual TCHAR * getWiimoteLogfileName() = 0;
	virtual int getBeatsPerMinute() = 0;
	virtual int getBeatsPerBar() = 0;
	virtual int getNumBars() = 0;
	virtual int getLeadIn() = 0;
	virtual int getFrequencyInterval()= 0;
	virtual void SendWiimoteData(double *wiimoteData,int size) = 0;

};

#endif

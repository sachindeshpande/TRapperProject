#pragma region Includes
#include <stdio.h>
#include <tchar.h>
#include <windows.h>
#include <atlstr.h>
#include <strsafe.h>
#pragma endregion

#include "CppNamedPipeServer.h"
#include "..\Main\WiimoteData.h"
#include <vector>
#include <queue>


using namespace std;


#ifndef WIIMOTE_NAMEDPIPE_COMMAND_SERVER_H
#define WIIMOTE_NAMEDPIPE_COMMAND_SERVER_H


const int WIIMOTE_MOTIONPLUS_ACC_DATA_MODE = 0;
const int WIIMOTE_MOTIONPLUS_ACC_IR_DATA_MODE = 1;

const TCHAR WIIMOTE_MOTIONPLUS_ACC_DATA_MODE_STRING[] = _T("0");
const TCHAR WIIMOTE_MOTIONPLUS_ACC_IR_DATA_STRING[] = _T("1");

const TCHAR WIIMOTE_COMMAND_PIPE_NAME[] = _T("wiimoteCommands");

const TCHAR START_DATA_SENDING_METHOD_NAME[] = _T("startWiimoteDataSending");
const TCHAR STOP_DATA_SENDING_METHOD_NAME[] = _T("stopWiimoteDataSending");
const TCHAR SET_WIIMOTE_SIMULATION_MODE_METHOD_NAME[] = _T("setWiimoteSimulationMode");
const TCHAR SET_WIIMOTE_DATA_MODE_METHOD_NAME[] = _T("setWiimoteDataMode");
const TCHAR CHECK_WIIMOTE_STATE_METHOD_NAME[] = _T("checkWiimoteState");
const TCHAR SET_CALIBRATION_METHOD_NAME[] = _T("setCalibration");
const TCHAR STOP_WIIMOTE_PROCESS_METHOD_NAME[] = _T("stopWiimoteProcess");
const TCHAR WIIMOTE_STATE_PIPE_NAME[] = _T("wiimoteState");
const TCHAR START_RECORDING_METHOD_NAME[] = _T("startRecording");


class WiimoteNamedPipeCommandServer : public WiimoteNamedPipeServer
{
public :

	
	WiimoteNamedPipeCommandServer(IWiimoteData *wiimoteData1, IWiimoteData *wiimoteData2);
	~WiimoteNamedPipeCommandServer();

	bool run();

	bool isWiimoteRecordingOn() {return recordingStatus;}
	bool isWiimoteLoggingOn() {return logWiimoteData;}
//	bool areWiimotesSwitched() {return wiimotesSwitched;}
	bool isWiimoteSimulationModeOn() {return m_WiimoteSimulationMode;}
	bool isWiimoteSimulationModeSet() {return m_WiimoteSimulationModeSet;}
	int getWiimoteDataMode() {return m_WiimoteDataMode;}

	TCHAR * getWiimoteLogfileName() {return wiimoteDataFile;}
	TCHAR * getWiimoteSimulationFileName() {return m_WiimoteSimulationFileName;}

	int getBeatsPerMinute() {return beatsPerMinute;}
	int getBeatsPerBar() {return beatsPerBar;}
	int getNumBars() {return numBars;}
	int getLeadIn() {return leadIn;}
	int getFrequencyInterval() {return frequencyInterval;}

	int parseMessage(DWORD cbBytesRead, TCHAR * chRequest,TCHAR *chReply);
	TCHAR * getNextMessageBlock(TCHAR * chRequest,int *index,TCHAR *blockName);

	IWiimoteData * m_WiimoteData1;
	IWiimoteData * m_WiimoteData2;

private :
	TCHAR * wiimoteDataFile;
	bool logWiimoteData;
	bool wiimotesSwitched;
	int beatsPerMinute;
	int beatsPerBar;
	int numBars;
	int leadIn;
	int frequencyInterval;
	bool recordingStatus;

	TCHAR * m_WiimoteSimulationFileName;
	bool m_WiimoteSimulationMode;
	int m_WiimoteDataMode;
	bool m_WiimoteSimulationModeSet;


	static unsigned __stdcall StartReadingCommands(void * params);

};

#endif

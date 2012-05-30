#include "WiimotePipeServerWrapper.h"


WiimotePipeServerWrapper::WiimotePipeServerWrapper()
{
	wiimoteDataPacket = new double[WIIMOTE_DATA_SIZE];
	fp = NULL;


	m_WiimoteLoggingOn = false;
	m_WiimoteCppFileLoggingOn = false;

	/* Initialize the critical section before entering multi-threaded context. */

}

WiimotePipeServerWrapper::~WiimotePipeServerWrapper()
{
	delete wiimoteDataPacket;

}

void WiimotePipeServerWrapper::startWiimoteServers(WiimoteData *remote1,WiimoteData *remote2)
{
	m_CommandPipeServer = new WiimoteNamedPipeCommandServer((IWiimoteData *)remote1, (IWiimoteData *)remote2);
	m_DataPipeServer = new WiimoteNamedPipeDataServer();

	m_CommandPipeServer->StartPipeServer();
	m_DataPipeServer->StartPipeServer();

	while(!m_CommandPipeServer->isWiimoteSimulationModeSet())
		Sleep(100);
}


void WiimotePipeServerWrapper::startLogging(WiimoteData *remote1,WiimoteData *remote2)
{
	m_WiimoteLoggingOn = true;
	openLoggingFile(remote1,remote2);
}

void WiimotePipeServerWrapper::sendDataPacket(WiimoteData *wiimoteData1,WiimoteData *wiimoteData2,int sequenceNumber,DWORD current_time,bool p_EndStream)
{
	WiimoteDoubleArrayDataPacket * wiimoteDataPacket = new WiimoteDoubleArrayDataPacket();

	if(p_EndStream)
		wiimoteDataPacket->setEndTransmissionTag();
	else
		wiimoteDataPacket->setDataPacket(wiimoteData1,wiimoteData2,sequenceNumber,current_time);

	wiimoteDataPacket->log(_T("In WiimotePipeServerWrapper::sendWiimoteData"));

	m_DataPipeServer->sendWiimoteData(wiimoteDataPacket);
}

void WiimotePipeServerWrapper::stopLogging()
{
	m_WiimoteLoggingOn = false;

	sendDataPacket(NULL,NULL,NULL,NULL,true);

	closeLoggingFile();

}


bool WiimotePipeServerWrapper::sendWiimoteData(WiimoteData *wiimoteData1,WiimoteData *wiimoteData2,int sequenceNumber,DWORD current_time)
{ 
	   if(m_CommandPipeServer->isWiimoteLoggingOn())
		{
			//If is false it indicates that logging was currently off. In that scenario when m_CommandPipeServer->isWiimoteLoggingOn() changes to true
			//it indicates logging request just came in. So need to initialize
			//For this case return right away and do not send packet as this packet is not initialized by the client.
			//The clietnt did not know before sending this message that recording was restarted
			if(!m_WiimoteLoggingOn)
			{
				startLogging(wiimoteData1,wiimoteData2);
				return true;
			}
			else
				sendDataPacket(wiimoteData1,wiimoteData2,sequenceNumber,current_time,false);
		}
		else
		{
			if(m_WiimoteLoggingOn)
			{
				stopLogging();
				return WIIMOTE_LOGGING_STOPPED;
			}
		}

	   logWiimoteData(wiimoteData1,wiimoteData2,sequenceNumber,current_time);

	   return false;
}


void WiimotePipeServerWrapper::setWiimoteConnectionState(WiimoteData *remote1,WiimoteData *remote2)
{
	if(remote1->getConnectionState() == WIIMOTE_DISCONNECTED_STATE || remote1->getConnectionState() == WIIMOTE_BAD_DATA_STATE)
		stopLogging();
}

bool WiimotePipeServerWrapper::isWiimoteLoggingOn()
{
	return m_WiimoteLoggingOn;
}



TCHAR * WiimotePipeServerWrapper::getWiimoteLogfileName()
{
	return m_CommandPipeServer->getWiimoteLogfileName();
}

int WiimotePipeServerWrapper::getBeatsPerMinute()
{
	return m_CommandPipeServer->getBeatsPerMinute();
}

int WiimotePipeServerWrapper::getBeatsPerBar()
{
	return m_CommandPipeServer->getBeatsPerBar();
}

int WiimotePipeServerWrapper::getNumBars()
{
	return m_CommandPipeServer->getNumBars();
}

int WiimotePipeServerWrapper::getLeadIn()
{
	return m_CommandPipeServer->getLeadIn();
}

int WiimotePipeServerWrapper::getFrequencyInterval()
{
	return m_CommandPipeServer->getFrequencyInterval();
}

bool WiimotePipeServerWrapper::isWiimoteSimulationModeOn()
{
	return m_CommandPipeServer->isWiimoteSimulationModeOn();
}

TCHAR * WiimotePipeServerWrapper::getWiimoteSimulationFileName()
{
	return m_CommandPipeServer->getWiimoteSimulationFileName();
}

void WiimotePipeServerWrapper::openLoggingFile(WiimoteData *remote1,WiimoteData *remote2)
{
	if(m_WiimoteCppFileLoggingOn)
	{
		TCHAR * wiimoteLogfileName = getWiimoteLogfileName();
		fp = _wfopen(wiimoteLogfileName,_T("w"));
		logWiimoteFileHeader(remote1,remote2,getBeatsPerMinute(),getBeatsPerBar(),getNumBars(),getLeadIn());
	}

}

void WiimotePipeServerWrapper::closeLoggingFile()
{
	if(fp != NULL && m_WiimoteCppFileLoggingOn)
	{
		fclose(fp);
		fp = NULL;
	}
}

void WiimotePipeServerWrapper::logWiimoteFileHeader(WiimoteData *remote1,WiimoteData *remote2,
													int beatsPerMinute, int beatsPerBar, int numBars,int leadIn)
{
	if(fp != NULL && m_WiimoteCppFileLoggingOn)
	{
		fprintf(fp,"BPM,%d\n",beatsPerMinute);
		fprintf(fp,"BPB,%d\n",beatsPerBar);
		fprintf(fp,"NumBar,%d\n",numBars);
		fprintf(fp,"LeadIn,%d\n",leadIn);

		fprintf(fp,"\nMusicFile,None\n");

		fprintf(fp,"\nCalibration Info Start");

		fprintf(fp,"\nSequence #,Time,Acc Pitch Orientation #WM1, Acc Roll Orientation #WM1,Acc X #WM1,Acc Y #WM1,Acc Z #WM1,\
			Raw Yaw #WM1,Raw Pitch #WM1,Raw Roll #WM1,Speed Yaw #WM1, Speed Pitch #WM1,Speed Roll #WM1,\
			Acc Pitch Orientation #WM2, Acc Roll Orientation #WM2,Acc X #WM2,Acc Y #WM2,Acc Z #WM2,\
			Raw Yaw #WM2,Raw Pitch #WM2,Raw Roll #WM2,Speed Yaw #WM2, Speed Pitch #WM2,Speed Roll #WM2");

		fprintf(fp,"\n0,0,0,0,%f,%f,%f,0,0,0,%lf,%lf,%lf,0,0,%f,%f,%f,0,0,0,%lf,%lf,%lf",remote1->m_WiimoteCalibration.m_WiimoteAccXCalibration,
			remote1->m_WiimoteCalibration.m_WiimoteAccYCalibration,
			remote1->m_WiimoteCalibration.m_WiimoteAccZCalibration,
			remote1->m_WiimoteCalibration.m_WiimoteYawCalibration,
			remote1->m_WiimoteCalibration.m_WiimotePitchCalibration,
			remote1->m_WiimoteCalibration.m_WiimoteRollCalibration,
			remote2->m_WiimoteCalibration.m_WiimoteAccXCalibration,
			remote2->m_WiimoteCalibration.m_WiimoteAccYCalibration,
			remote2->m_WiimoteCalibration.m_WiimoteAccZCalibration,
			remote2->m_WiimoteCalibration.m_WiimoteYawCalibration,
			remote2->m_WiimoteCalibration.m_WiimotePitchCalibration,
			remote2->m_WiimoteCalibration.m_WiimoteRollCalibration,
			remote2->m_WiimoteCalibration.m_WiimoteAccXCalibration);

		fprintf(fp,"\nCalibration Info End\n");

		fprintf(fp,"\nSequence #,Time,Acc Pitch Orientation #WM1, Acc Roll Orientation #WM1,Acc X #WM1,Acc Y #WM1,Acc Z #WM1,\
			Raw Yaw #WM1,Raw Pitch #WM1,Raw Roll #WM1,Speed Yaw #WM1, Speed Pitch #WM1,Speed Roll #WM1,\
			Acc Pitch Orientation #WM2, Acc Roll Orientation #WM2,Acc X #WM2,Acc Y #WM2,Acc Z #WM2,\
			Raw Yaw #WM2,Raw Pitch #WM2,Raw Roll #WM2,Speed Yaw #WM2, Speed Pitch #WM2,Speed Roll #WM2");
	}
	
}

void WiimotePipeServerWrapper::logWiimoteData(WiimoteData *remote1,WiimoteData *remote2,int sequenceNumber,DWORD current_time)
{
		if(fp != NULL && m_WiimoteCppFileLoggingOn)
		{
			wiimote * wiimote1 = &remote1->m_wiimote;
			wiimote * wiimote2 = &remote2->m_wiimote;

			int status = fprintf(fp,"\n%d,%d,%f,%f,%f,%f,%f,%04hx,%04hx,%04hx,%8.3f,%8.3f,%8.3f",
					sequenceNumber,current_time,wiimote1->Acceleration.Orientation.Pitch,wiimote1->Acceleration.Orientation.Roll,wiimote1->Acceleration.X,wiimote1->Acceleration.Y,wiimote1->Acceleration.Z,
					wiimote1->MotionPlus.Raw.Yaw,wiimote1->MotionPlus.Raw.Pitch,wiimote1->MotionPlus.Raw.Roll,
					wiimote1->MotionPlus.Speed.Yaw,wiimote1->MotionPlus.Speed.Pitch,wiimote1->MotionPlus.Speed.Roll);

			status = fprintf(fp,",%f,%f,%f,%f,%f,%04hx,%04hx,%04hx,%8.3f,%8.3f,%8.3f",
					wiimote2->Acceleration.Orientation.Pitch,wiimote2->Acceleration.Orientation.Roll,wiimote2->Acceleration.X,wiimote2->Acceleration.Y,wiimote2->Acceleration.Z,
					wiimote2->MotionPlus.Raw.Yaw,wiimote2->MotionPlus.Raw.Pitch,wiimote2->MotionPlus.Raw.Roll,
					wiimote2->MotionPlus.Speed.Yaw,wiimote2->MotionPlus.Speed.Pitch,wiimote2->MotionPlus.Speed.Roll);
		}
}


/*
bool WiimotePipeServerWrapper::checkWiimoteLoggingStatusChange()
{
	if(!m_WiimoteLoggingOn && m_CommandPipeServer->isWiimoteLoggingOn())
		m_WiimoteLoggingOn = true;
	else if(m_WiimoteLoggingOn && !m_CommandPipeServer->isWiimoteLoggingOn())
		m_WiimoteLoggingOn = false;

	return m_WiimoteLoggingOn;

}
*/
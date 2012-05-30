
#include "stdafx.h"
#include "DoubleArrayDataPacket.h"
#include "..\FileLogger\FileLogger.h"

DoubleArrayDataPacket::DoubleArrayDataPacket()
{
}

void DoubleArrayDataPacket::initialize()
{
	m_SequenceNumber = 0 ;
	m_CurrentTime = 0;

	for(int i = 0 ; i < WIIMOTE_IR_DATA_PACKET_SIZE ; i++)
		m_DataPacket[i] = 0;
}

void DoubleArrayDataPacket::setEndTransmissionTag()
{
	initialize();
	m_SequenceNumber = DATA_TRANSMISSION_END_TAG;
}

/*
This function combines the data in a buffer. It stores the values upto 2 decimal points
*/
void WiimoteDoubleArrayDataPacket::getCommaSeparatedDataPacket(TCHAR * p_DataPacketString)
{
	
	_stprintf(p_DataPacketString,_T("%d%,%d,%4.2f,%4.2f,%4.2f,%4.2f,%4.2f,%4.2f,%4.2f,%4.2f,%4.2f,%4.2f,%4.2f,%4.2f,%4.2f,%4.2f,%4.2f,%4.2f,%4.2f,%4.2f,%4.2f,%4.2f,%4.2f,%4.2f"),
			m_SequenceNumber,m_CurrentTime,
			m_DataPacket[0],m_DataPacket[1],m_DataPacket[2],m_DataPacket[3],m_DataPacket[4],
			m_DataPacket[5],m_DataPacket[6],m_DataPacket[7],m_DataPacket[8],
			m_DataPacket[9],m_DataPacket[10],m_DataPacket[11],m_DataPacket[12],m_DataPacket[13],
			m_DataPacket[14],m_DataPacket[15],m_DataPacket[16],m_DataPacket[17],
			m_DataPacket[18],m_DataPacket[19],m_DataPacket[20],m_DataPacket[21]);			
}

void WiimoteDoubleArrayDataPacket::createWiimoteDataPacket(wiimote *remote1,wiimote *remote2)
{	
	m_DataPacket[0] = remote1->Acceleration.Orientation.Pitch;
	m_DataPacket[1] = remote1->Acceleration.Orientation.Roll;
	m_DataPacket[2] = remote1->Acceleration.X;
	m_DataPacket[3] = remote1->Acceleration.Y;
	m_DataPacket[4] = remote1->Acceleration.Z;
	m_DataPacket[5] = remote1->MotionPlus.Raw.Yaw;
	m_DataPacket[6] = remote1->MotionPlus.Raw.Pitch;
	m_DataPacket[7] = remote1->MotionPlus.Raw.Roll;
	m_DataPacket[8] = remote1->MotionPlus.Speed.Yaw;
	m_DataPacket[9] = remote1->MotionPlus.Speed.Pitch;
	m_DataPacket[10] = remote1->MotionPlus.Speed.Roll;

	m_DataPacket[11] = remote2->Acceleration.Orientation.Pitch;
	m_DataPacket[12] = remote2->Acceleration.Orientation.Roll;
	m_DataPacket[13] = remote2->Acceleration.X;
	m_DataPacket[14] = remote2->Acceleration.Y;
	m_DataPacket[15] = remote2->Acceleration.Z;
	m_DataPacket[16] = remote2->MotionPlus.Raw.Yaw;
	m_DataPacket[17] = remote2->MotionPlus.Raw.Pitch;
	m_DataPacket[18] = remote2->MotionPlus.Raw.Roll;
	m_DataPacket[19] = remote2->MotionPlus.Speed.Yaw;
	m_DataPacket[20] = remote2->MotionPlus.Speed.Pitch;
	m_DataPacket[21] = remote2->MotionPlus.Speed.Roll;

}

void WiimoteDoubleArrayDataPacket::setDataPacket(WiimoteData * p_WiimoteData1,WiimoteData * p_WiimoteData2,int sequenceNumber,DWORD current_time)
{
		m_SequenceNumber = sequenceNumber;
		m_CurrentTime = current_time;

		wiimote * wiimote1 = &p_WiimoteData1->m_wiimote;
		wiimote * wiimote2 = &p_WiimoteData2->m_wiimote;

		createWiimoteDataPacket(wiimote1,wiimote2);
}

void WiimoteDoubleArrayDataPacket::log(TCHAR * locationTag)
{
	
	memset(m_LoggingBuffer,'\0',DATA_BUFFER_SIZE);
//	_stprintf(m_LoggingBuffer,_T("%d%"),m_SequenceNumber);
	
	_stprintf(m_LoggingBuffer,_T("%d%,%d,%4.2f,%4.2f,%4.2f,%4.2f,%4.2f,%4.2f,%4.2f,%4.2f,%4.2f,%4.2f,%4.2f,%4.2f,%4.2f,%4.2f,%4.2f,%4.2f,%4.2f,%4.2f,%4.2f,%4.2f,%4.2f,%4.2f"),
			m_SequenceNumber,m_CurrentTime,
			m_DataPacket[0],m_DataPacket[1],m_DataPacket[2],m_DataPacket[3],m_DataPacket[4],
			m_DataPacket[5],m_DataPacket[6],m_DataPacket[7],m_DataPacket[8],
			m_DataPacket[9],m_DataPacket[10],m_DataPacket[11],m_DataPacket[12],m_DataPacket[13],
			m_DataPacket[14],m_DataPacket[15],m_DataPacket[16],m_DataPacket[17],
			m_DataPacket[18],m_DataPacket[19],m_DataPacket[20],m_DataPacket[21]);
	FileLogger::getFileLogger()->logEntry(m_LoggingBuffer,locationTag);
}

#include "stdafx.h"
#include "DoubleArrayDataPacket.h"
#include "..\FileLogger\FileLogger.h"


/*
This function combines the data in a buffer. It stores the values upto 2 decimal points
*/
void WiimoteDoubleArrayDataPacketWithIR::getCommaSeparatedDataPacket(TCHAR * p_DataPacketString)
{
	
	_stprintf(p_DataPacketString,_T("%d%,%d,%4.2f,%4.2f,%4.2f,%4.2f,%4.2f,%4.2f,%4.2f,%4.2f,%4.2f,%4.2f,%4.2f,%4.2f,%4.2f,%4.2f,%4.2f,%4.2f,%4.2f,%4.2f,%4.2f,%4.2f,%4.2f,%4.2f,%4.2f,%4.2f,%4.2f,%4.2f,%4.2f,%4.2f,%4.2f,%4.2f,%4.2f,%4.2f,%4.2f,%4.2f,%4.2f,%4.2f,%4.2f,%4.2f"),
			m_SequenceNumber,m_CurrentTime,
			m_DataPacket[0],m_DataPacket[1],m_DataPacket[2],m_DataPacket[3],m_DataPacket[4],
			m_DataPacket[5],m_DataPacket[6],m_DataPacket[7],m_DataPacket[8],
			m_DataPacket[9],m_DataPacket[10],m_DataPacket[11],m_DataPacket[12],m_DataPacket[13],
			m_DataPacket[14],m_DataPacket[15],m_DataPacket[16],m_DataPacket[17],
			m_DataPacket[18],m_DataPacket[19],m_DataPacket[20],m_DataPacket[21],
			m_DataPacket[22],m_DataPacket[23],m_DataPacket[24],m_DataPacket[25],
			m_DataPacket[26],m_DataPacket[27],m_DataPacket[28],m_DataPacket[29],
			m_DataPacket[30],m_DataPacket[31],m_DataPacket[32],m_DataPacket[33],
			m_DataPacket[34],m_DataPacket[35],m_DataPacket[36],m_DataPacket[37]);
}

void WiimoteDoubleArrayDataPacketWithIR::createWiimoteDataPacket(wiimote *remote1,wiimote *remote2)
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
	m_DataPacket[11] = remote1->IR.Dot[0].X;
	m_DataPacket[12] = remote1->IR.Dot[0].Y;
	m_DataPacket[13] = remote1->IR.Dot[1].X;
	m_DataPacket[14] = remote1->IR.Dot[1].Y;
	m_DataPacket[15] = remote1->IR.Dot[2].X;
	m_DataPacket[16] = remote1->IR.Dot[2].Y;
	m_DataPacket[17] = remote1->IR.Dot[3].X;
	m_DataPacket[18] = remote1->IR.Dot[3].Y;

	m_DataPacket[19] = remote2->Acceleration.Orientation.Pitch;
	m_DataPacket[20] = remote2->Acceleration.Orientation.Roll;
	m_DataPacket[21] = remote2->Acceleration.X;
	m_DataPacket[22] = remote2->Acceleration.Y;
	m_DataPacket[23] = remote2->Acceleration.Z;
	m_DataPacket[24] = remote2->MotionPlus.Raw.Yaw;
	m_DataPacket[25] = remote2->MotionPlus.Raw.Pitch;
	m_DataPacket[26] = remote2->MotionPlus.Raw.Roll;
	m_DataPacket[27] = remote2->MotionPlus.Speed.Yaw;
	m_DataPacket[28] = remote2->MotionPlus.Speed.Pitch;
	m_DataPacket[29] = remote2->MotionPlus.Speed.Roll;
	m_DataPacket[30] = remote2->IR.Dot[0].X;
	m_DataPacket[31] = remote2->IR.Dot[0].Y;
	m_DataPacket[32] = remote2->IR.Dot[1].X;
	m_DataPacket[33] = remote2->IR.Dot[1].Y;
	m_DataPacket[34] = remote2->IR.Dot[2].X;
	m_DataPacket[35] = remote2->IR.Dot[2].Y;
	m_DataPacket[36] = remote2->IR.Dot[3].X;
	m_DataPacket[37] = remote2->IR.Dot[3].Y;

}

void WiimoteDoubleArrayDataPacketWithIR::log(TCHAR * locationTag)
{
	
	memset(m_LoggingBuffer,'\0',DATA_BUFFER_SIZE);
//	_stprintf(m_LoggingBuffer,_T("%d%"),m_SequenceNumber);
	getCommaSeparatedDataPacket(m_LoggingBuffer);
	
	FileLogger::getFileLogger()->logEntry(m_LoggingBuffer,locationTag);
}
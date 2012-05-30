
#include <stdio.h>
#include <tchar.h>

#include "..\Main\wiimote.h"
#include "..\Main\WiimoteData.h"

#include "ProjectConstants.h"


#ifndef DOUBLEARRAY_DATAPACKET_H
# define DOUBLEARRAY_DATAPACKET_H

class DoubleArrayDataPacket
{
public :
	DoubleArrayDataPacket();
	void setSize(int size) { m_Size = size; }
	int getSize() { return m_Size; }
	void setEndTransmissionTag();
	virtual void getCommaSeparatedDataPacket(TCHAR * p_DataPacketString) = 0;
	virtual void log(TCHAR * locationTag) = 0;

protected :
	double m_DataPacket[WIIMOTE_DATA_PACKET_SIZE];
	int m_SequenceNumber;
	DWORD m_CurrentTime;
	int m_Size;

	void initialize();

};

class WiimoteDoubleArrayDataPacket : public DoubleArrayDataPacket
{
public :

	void setDataPacket(WiimoteData * p_WiimoteData1,WiimoteData * p_WiimoteData2,int sequenceNumber,DWORD current_time);
	virtual void getCommaSeparatedDataPacket(TCHAR * p_DataPacketString);
	virtual void log(TCHAR * locationTag);

protected :

	virtual void createWiimoteDataPacket(wiimote *remote1,wiimote *remote2);
	TCHAR m_LoggingBuffer[DATA_BUFFER_SIZE];

};

class WiimoteDoubleArrayDataPacketWithIR : public WiimoteDoubleArrayDataPacket
{
public :

	virtual void getCommaSeparatedDataPacket(TCHAR * p_DataPacketString);
	virtual void log(TCHAR * locationTag);

private :
	void createWiimoteDataPacket(wiimote *remote1,wiimote *remote2);

};


#endif
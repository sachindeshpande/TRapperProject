#ifndef WIIMOTE_DATA_H
#define WIIMOTE_DATA_H

#include "wiimote.h"

const int WIIMOTE_BAD_DATA_STATE = 1;
const int WIIMOTE_DISCONNECTED_STATE = 2;
const int WIIMOTE_CONNECTED_STATE = 3;
const int WIIMOTE_GOOD_DATA_STATE = 4;

class wiimote;

class IWiimoteData
{
public :
	virtual 	void setCalibration(double l_Wiimote1AccXCalibration,double l_Wiimote1AccYCalibration,double l_Wiimote1AccZCalibration,
			double l_Wiimote1YawCalibration,double l_Wiimote1PitchCalibration,double l_Wiimote1RollCalibration) = 0;

	virtual void setConnectionState(int wiimoteState) = 0;

	virtual int getConnectionState() = 0;

	virtual int getBatteryLevel() = 0;
};

class WiimoteCalibration
{
public :
	double m_WiimoteAccXCalibration;
	double m_WiimoteAccYCalibration;
	double m_WiimoteAccZCalibration;
	double m_WiimoteYawCalibration;
	double m_WiimotePitchCalibration;
	double m_WiimoteRollCalibration;
};

class WiimoteState
{
public :
	int m_WiimoteConnectionState;
};

class WiimoteData : IWiimoteData
{
public :
	WiimoteData();

	WiimoteCalibration m_WiimoteCalibration;
	WiimoteState m_WiimoteState;
	wiimote m_wiimote;

	void setCalibration(double l_Wiimote1AccXCalibration,double l_Wiimote1AccYCalibration,double l_Wiimote1AccZCalibration,
			double l_Wiimote1YawCalibration,double l_Wiimote1PitchCalibration,double l_Wiimote1RollCalibration);

	void setConnectionState(int wiimoteState);

	int getConnectionState() {return m_WiimoteState.m_WiimoteConnectionState;}

	int getBatteryLevel() {return m_wiimote.BatteryPercent; }

};

#endif

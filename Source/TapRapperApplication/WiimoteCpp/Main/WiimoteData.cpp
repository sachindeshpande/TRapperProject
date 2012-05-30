#include "WiimoteData.h"

WiimoteData::WiimoteData()
{
	m_WiimoteState.m_WiimoteConnectionState = WIIMOTE_DISCONNECTED_STATE;

	m_WiimoteCalibration.m_WiimoteAccXCalibration = 0;
	m_WiimoteCalibration.m_WiimoteAccYCalibration = 0;
	m_WiimoteCalibration.m_WiimoteAccZCalibration = 0;
	m_WiimoteCalibration.m_WiimoteYawCalibration = 0;
	m_WiimoteCalibration.m_WiimotePitchCalibration = 0;
	m_WiimoteCalibration.m_WiimoteRollCalibration = 0;

}

void WiimoteData::setCalibration(double l_WiimoteAccXCalibration,double l_WiimoteAccYCalibration,double l_WiimoteAccZCalibration,
			double l_WiimoteYawCalibration,double l_WiimotePitchCalibration,double l_WiimoteRollCalibration)
{
	m_WiimoteCalibration.m_WiimoteAccXCalibration = l_WiimoteAccXCalibration;
	m_WiimoteCalibration.m_WiimoteAccYCalibration = l_WiimoteAccYCalibration;
	m_WiimoteCalibration.m_WiimoteAccZCalibration = l_WiimoteAccZCalibration;
	m_WiimoteCalibration.m_WiimoteYawCalibration = l_WiimoteYawCalibration;
	m_WiimoteCalibration.m_WiimotePitchCalibration = l_WiimotePitchCalibration;
	m_WiimoteCalibration.m_WiimoteRollCalibration = l_WiimoteRollCalibration;
}

void WiimoteData::setConnectionState(int wiimoteState)
{
	m_WiimoteState.m_WiimoteConnectionState = wiimoteState;
}
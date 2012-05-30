using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using ProjectCommon;

namespace SpaceSensor
{
    public class SpaceSensorRecordingDataReceivedEventArgs : EventArgs
    {
        public SpaceSensorRecordingDataReceivedEventArgs(SensorDataRaw sensor1DataPacket, SensorDataRaw sensor2DataPacket)
        {
            _sensor1DataPacket = sensor1DataPacket;
            _sensor2DataPacket = sensor2DataPacket;
        }

        private SensorDataRaw _sensor1DataPacket;
        public SensorDataRaw Sensor1DataPacket
        {
            get { return _sensor1DataPacket; }
            set { _sensor1DataPacket = value; }
        }

        private SensorDataRaw _sensor2DataPacket;
        public SensorDataRaw Sensor2DataPacket
        {
            get { return _sensor2DataPacket; }
            set { _sensor2DataPacket = value; }
        }

    }


    public class SpaceSensorMain
    {

        public delegate void OnSpaceSensorDataEvent(object sender, SpaceSensorRecordingDataReceivedEventArgs args);
        public event OnSpaceSensorDataEvent SpaceSensorDataEvent;

        private bool _spaceRecordingStatus;

        private string COM_PORT = "COM";
        private const byte _sensor1LogicalID = 0x0;
        private const byte _sensor2LogicalID = 0x1;

        private SpaceSensorWirelessDongle _spaceSensorWirelessDongleObj;

        private SpaceSensorDevice _spaceSensor1;
        public SpaceSensorDevice SpaceSensorObject1
        {
            get { return _spaceSensor1; }
            set { _spaceSensor1 = value; }
        }

        private SpaceSensorDevice _spaceSensor2;
        public SpaceSensorDevice SpaceSensorObject2
        {
            get { return _spaceSensor2; }
            set { _spaceSensor2 = value; }
        }


        public SpaceSensorMain()
        {

        }

        public void Connect()
        {
            try
            {
                _spaceSensorWirelessDongleObj = new SpaceSensorWirelessDongle(COM_PORT);

                _spaceSensorWirelessDongleObj.Connect();
                _spaceSensor1 = new SpaceSensorDevice(_sensor1LogicalID, _spaceSensorWirelessDongleObj);
                _spaceSensor2 = new SpaceSensorDevice(_sensor2LogicalID, _spaceSensorWirelessDongleObj);
                _spaceSensor1.SetRange();
                _spaceSensor2.SetRange();
                _spaceSensorWirelessDongleObj.Disconnect();

                _spaceSensorWirelessDongleObj.Connect();
                _spaceSensor1 = new SpaceSensorDevice(_sensor1LogicalID, _spaceSensorWirelessDongleObj);
                _spaceSensor2 = new SpaceSensorDevice(_sensor2LogicalID, _spaceSensorWirelessDongleObj);

            }
            catch (Exception ex)
            {
                
                throw;
            }
        }

        public void StartReading()
        {

            Thread spaceSensorRecording;

            spaceSensorRecording = new Thread(SpaceSensorRecordingThread);

            _spaceRecordingStatus = true;
            spaceSensorRecording.Start();

        }

        public void StopReading()
        {
            _spaceRecordingStatus = false;
        }

        private void SpaceSensorRecordingThread()
        {
            try
            {
                while (_spaceRecordingStatus)
                {

                    SensorDataRaw sensor1DataObj = SpaceSensorObject1.ReadData();
                    SensorDataRaw sensor2DataObj = SpaceSensorObject2.ReadData();

                    SpaceSensorDataEvent(this, new SpaceSensorRecordingDataReceivedEventArgs(sensor1DataObj, sensor2DataObj));

                    Thread.Sleep(1);

                }

            }
            catch (Exception ex)
            {
                
                throw;
            }
        }


        public bool Disconnect()
        {
            _spaceSensorWirelessDongleObj.Disconnect();
            return true;
        }

        public string setSimulationMode(bool p_SimulationMode, string p_SimulationFile)
        {
            return "";
        }

        public string setDataMode(int p_WiimoteDataMode)
        {
            return "";
        }

    }
}

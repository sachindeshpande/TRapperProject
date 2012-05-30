using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.IO.Ports;
using Microsoft.Xna.Framework;
using ProjectCommon;


namespace SpaceSensor
{

    public class SpaceSensorWirelessDongle
    {
        private SerialPort _port;
        public SerialPort Port
        {
            get { return _port; }
            set { _port = value; }
        }

        private string _portName;

        public SpaceSensorWirelessDongle(string portName)
        {
            _portName = portName;
        }

        public bool Connect()
        {
            for (int portNum = 1; portNum < 15; portNum++)
            {
                if (TryConnect(portNum))
                    return true;
            }

            return false;
        }

        public bool TryConnect(int portNum)
        {
            string portName = _portName + portNum;

            //open the COM port using the name currently in the COM port box
            _port = new SerialPort(portName);

            //set this to one second so a write that takes too long will time out
            _port.WriteTimeout = 1000;

            //set this to one second so a read that takes too long will time out
            _port.ReadTimeout = 1000;

            //this is unimportant if communicating through USB, but could be important if the sensor
            //was hooked up through a normal RS232 port(USB and Embedded versions only)
            _port.BaudRate = 115200;
            try
            {

                _port.Open();
                return true;

            }
            catch (Exception ex)
            {
                //reasons for not opening include other programs already having the port open
                //or plugging a sensor in when a program thinks it has it open already
                Console.WriteLine();
            }

            return false;
        }

        public bool Disconnect()
        {
            try
            {
                if (_port != null)
                {
                    _port.Close();
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return false;
        }
    }

    public class SpaceSensorDevice
    {
        SpaceSensorWirelessDongle _wirelessDongle;

        private byte _sensorLogicalID;

        private object _dataReadSync = new object();

        public SpaceSensorDevice(byte sensorLogicalID,SpaceSensorWirelessDongle wirelessDongle)
        {
            _sensorLogicalID = sensorLogicalID;
            _wirelessDongle = wirelessDongle;
        }


        public SensorDataRaw ReadData()
        {
            SensorDataRaw sensorDataObj = new SensorDataRaw();
            ReadRawAccGyroData(sensorDataObj);
            //ReadOrientation(sensorDataObj);
            return sensorDataObj;
        }

        public void SetRange()
        {
            lock (_dataReadSync)
            {
                //check to make sure the port is present
                if (_wirelessDongle.Port != null)
                {

                    byte[] writebuffer = new byte[5];
                    writebuffer[0] = 0xf8;//command start byte for wireless commands
                    writebuffer[1] = _sensorLogicalID;//Logical id for the sensor
                    writebuffer[2] = 0x79;//command number
                    writebuffer[3] = 0x2;
                    writebuffer[4] = (byte)(0x79 + 0x2 + _sensorLogicalID);//checksum(sum of all other packet bytes except start byte and checksum
                    _wirelessDongle.Port.Write(writebuffer, 0, 5);
                }
            }

        }

        private void ReadRawAccGyroData(SensorDataRaw sensorDataObj)
        {
            lock (_dataReadSync)
            {

                int returnDataLength = 39;

                //check to make sure the port is present
                if (_wirelessDongle.Port != null)
                {

                    byte[] writebuffer = new byte[4];
                    writebuffer[0] = 0xf8;//command start byte for wireless commands
                    writebuffer[1] = _sensorLogicalID;//Logical id for the sensor
                    writebuffer[2] = 0x40;//command number
                    writebuffer[3] = (byte)(0x40 + _sensorLogicalID);//checksum(sum of all other packet bytes except start byte and checksum
                    _wirelessDongle.Port.Write(writebuffer, 0, 4);

                    //use this bit of code to read back data until we have the required amount
                    int totalbytes = 0;
                    byte[] readbuffer = new byte[returnDataLength];
                    while (totalbytes != returnDataLength)
                    {
                        int nbytes = _wirelessDongle.Port.Read(readbuffer, totalbytes, returnDataLength - totalbytes);
                        totalbytes += nbytes;
                    }

                    sensorDataObj.AccGyroReadbuffer = readbuffer;
                }
            }

        }


        private void ReadAccGyroData(SensorDataRaw sensorRawDataObj)
        {
            lock (_dataReadSync)
            {

                int returnDataLength = 39;

                //check to make sure the port is present
                if (_wirelessDongle.Port != null)
                {

                    byte[] writebuffer = new byte[4];
                    writebuffer[0] = 0xf8;//command start byte for wireless commands
                    writebuffer[1] = _sensorLogicalID;//Logical id for the sensor
                    writebuffer[2] = 0x20;//command number
                    writebuffer[3] = (byte)(0x20 + _sensorLogicalID);//checksum(sum of all other packet bytes except start byte and checksum
                    _wirelessDongle.Port.Write(writebuffer, 0, 4);

                    //use this bit of code to read back data until we have the required amount
                    int totalbytes = 0;
                    byte[] readbuffer = new byte[returnDataLength];
                    while (totalbytes != returnDataLength)
                    {
                        int nbytes = _wirelessDongle.Port.Read(readbuffer, totalbytes, returnDataLength - totalbytes);
                        totalbytes += nbytes;
                    }

                    sensorRawDataObj.AccGyroReadbuffer = readbuffer;
                }
            }

        }

        private void ReadOrientation(SensorData sensorData)
        {
            lock (_dataReadSync)
            {
                int returnDataLength = 19;
                //check to make sure the port is present
                if (_wirelessDongle.Port != null)
                {

                    byte[] writebuffer = new byte[4];
                    writebuffer[0] = 0xf8;//command start byte for wireless commands
                    writebuffer[1] = _sensorLogicalID;//Logical id for the sensor
                    writebuffer[2] = 0x0;//command number
                    writebuffer[3] = (byte)(0x0 + _sensorLogicalID);//checksum(sum of all other packet bytes except start byte and checksum
                    _wirelessDongle.Port.Write(writebuffer, 0, 4);

                    //use this bit of code to read back data until we have the required amount
                    int totalbytes = 0;
                    byte[] readbuffer = new byte[returnDataLength];
                    while (totalbytes != returnDataLength)
                    {
                        int nbytes = _wirelessDongle.Port.Read(readbuffer, totalbytes, returnDataLength - totalbytes);
                        totalbytes += nbytes;
                    }

                    //reverse the array bytes because this data is big endian
                    readbuffer = readbuffer.Reverse().ToArray();
                    //readbuffer = readbuffer.ToArray();

                    //Populate the data backwards because we reversed the order of the values
                    //as well as their internal bytes

                    Quaternion quatObj = new Quaternion();
                    //quaternion x
                    quatObj.X = BitConverter.ToSingle(readbuffer, 12);

                    //quaternion y
                    quatObj.Y = BitConverter.ToSingle(readbuffer, 8);

                    //quaternion z
                    quatObj.Z = BitConverter.ToSingle(readbuffer, 4);

                    //quaternion w
                    quatObj.W = BitConverter.ToSingle(readbuffer, 0);

                    sensorData.QuaternionObject = quatObj;
                }
            }

        }


        private string ReadID()
        {
            lock (_dataReadSync)
            {

                //check to make sure the port is present
                if (_wirelessDongle.Port != null)
                {
                    byte[] writebuffer = new byte[4];
                    writebuffer[0] = 0xf8;//command start byte for wireless commands
                    writebuffer[1] = _sensorLogicalID;//Logical id for the sensor
                    writebuffer[2] = 0xe6;//command number
                    writebuffer[3] = 0xe6;//checksum(sum of all other packet bytes except start byte and checksum
                    _wirelessDongle.Port.Write(writebuffer, 0, 4);

                    //use this bit of code to read back data until we have the required amount
                    int totalbytes = 0;
                    byte[] readbuffer = new byte[12];
                    while (totalbytes != 12)
                    {
                        int nbytes = _wirelessDongle.Port.Read(readbuffer, totalbytes, 12 - totalbytes);
                        totalbytes += nbytes;
                    }

                    //turn the data into a string manually
                    string s = "";
                    for (int i = 0; i < readbuffer.Length; i++)
                        s += (char)(readbuffer[i]);

                    //print the data normally
                    return s;
                }
            }

            return null;
        }

        private void ReadAxisAngle()
        {
            lock (_dataReadSync)
            {
                //check to make sure the port is present
                if (_wirelessDongle.Port != null)
                {
                    byte[] writebuffer = new byte[4];
                    writebuffer[0] = 0xf8;//command start byte for wireless commands
                    writebuffer[1] = _sensorLogicalID;//Logical id for the sensor
                    writebuffer[2] = 0x3;//command number
                    writebuffer[3] = 0x3;//checksum(sum of all other packet bytes except start byte and checksum
                    _wirelessDongle.Port.Write(writebuffer, 0, 4);

                    //use this bit of code to read back data until we have the required amount
                    int totalbytes = 0;
                    byte[] readbuffer = new byte[16];
                    while (totalbytes != 16)
                    {
                        int nbytes = _wirelessDongle.Port.Read(readbuffer, totalbytes, 16 - totalbytes);
                        totalbytes += nbytes;
                    }

                    //reverse the array bytes because this data is big endian
                    readbuffer = readbuffer.Reverse().ToArray();
                }
            }
        }

    }
}
/**

        private void ReadAccGyroDataTest(SensorData sensorDataObj)
        {
            lock (_dataReadSync)
            {

                int returnDataLength = 15;

                //check to make sure the port is present
                if (_wirelessDongle.Port != null)
                {

                    byte[] writebuffer = new byte[4];
                    writebuffer[0] = 0xf8;//command start byte for wireless commands
                    writebuffer[1] = _sensorLogicalID;//Logical id for the sensor
                    writebuffer[2] = 0x21;//command number
                    writebuffer[3] = (byte)(0x21 + _sensorLogicalID);//checksum(sum of all other packet bytes except start byte and checksum
                    _wirelessDongle.Port.Write(writebuffer, 0, 4);

                    //use this bit of code to read back data until we have the required amount
                    int totalbytes = 0;
                    byte[] readbuffer = new byte[returnDataLength];
                    while (totalbytes != returnDataLength)
                    {
                        int nbytes = _wirelessDongle.Port.Read(readbuffer, totalbytes, returnDataLength - totalbytes);
                        totalbytes += nbytes;
                    }

                    //reverse the array bytes because this data is big endian
                    readbuffer = readbuffer.Reverse().ToArray();

                    //Populate the data backwards because we reversed the order of the values
                    //as well as their internal bytes

                    sensorDataObj.GyroDataObject.Pitch = BitConverter.ToSingle(readbuffer, 8);
                    sensorDataObj.GyroDataObject.Yaw = BitConverter.ToSingle(readbuffer, 4);
                    sensorDataObj.GyroDataObject.Roll = BitConverter.ToSingle(readbuffer, 0);

                    Vector3 accVectorObject = new Vector3();
                    accVectorObject.X = 0;
                    accVectorObject.Y = 0;
                    accVectorObject.Z = 0;
                    sensorDataObj.AccDataObject.DataVector = accVectorObject;


                    Vector3 compassVectorObject = new Vector3();
                    compassVectorObject.X = 0;
                    compassVectorObject.Y = 0;
                    compassVectorObject.Z = 0;
                    sensorDataObj.CompassDataObject.DataVector = compassVectorObject;

                }
            }

        }

         private void ReadRawAccGyroData(SensorDataRaw sensorDataObj)
        {
            lock (_dataReadSync)
            {

                int returnDataLength = 39;

                //check to make sure the port is present
                if (_wirelessDongle.Port != null)
                {

                    byte[] writebuffer = new byte[4];
                    writebuffer[0] = 0xf8;//command start byte for wireless commands
                    writebuffer[1] = _sensorLogicalID;//Logical id for the sensor
                    writebuffer[2] = 0x40;//command number
                    writebuffer[3] = (byte)(0x40 + _sensorLogicalID);//checksum(sum of all other packet bytes except start byte and checksum
                    _wirelessDongle.Port.Write(writebuffer, 0, 4);

                    //use this bit of code to read back data until we have the required amount
                    int totalbytes = 0;
                    byte[] readbuffer = new byte[returnDataLength];
                    while (totalbytes != returnDataLength)
                    {
                        int nbytes = _wirelessDongle.Port.Read(readbuffer, totalbytes, returnDataLength - totalbytes);
                        totalbytes += nbytes;
                    }

                    sensorDataObj.AccGyroReadbuffer = readbuffer;

                    //reverse the array bytes because this data is big endian
                    readbuffer = readbuffer.Reverse().ToArray();

                    //Populate the data backwards because we reversed the order of the values
                    //as well as their internal bytes

                    sensorDataObj.GyroDataObject.Pitch = BitConverter.ToSingle(readbuffer, 32);
                    sensorDataObj.GyroDataObject.Yaw = BitConverter.ToSingle(readbuffer, 28);
                    sensorDataObj.GyroDataObject.Roll = BitConverter.ToSingle(readbuffer, 24);


                    Vector3 accVectorObject = new Vector3();
                    accVectorObject.X = BitConverter.ToSingle(readbuffer, 20);
                    accVectorObject.Y = BitConverter.ToSingle(readbuffer, 16);
                    accVectorObject.Z = BitConverter.ToSingle(readbuffer, 12);
                    sensorDataObj.AccDataObject.DataVector = accVectorObject;


                    Vector3 compassVectorObject = new Vector3();
                    compassVectorObject.X = BitConverter.ToSingle(readbuffer, 8);
                    compassVectorObject.Y = BitConverter.ToSingle(readbuffer, 4);
                    compassVectorObject.Z = BitConverter.ToSingle(readbuffer, 0);
                    sensorDataObj.CompassDataObject.DataVector = compassVectorObject;

                }
            }

        }
**/
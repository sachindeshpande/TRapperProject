using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ProjectCommon
{
    public class GyroData
    {
        public GyroData()
        {
        }

        private float _yaw;
        public float Yaw
        {
            get { return _yaw; }
            set { _yaw = value; }
        }

        private float _pitch;
        public float Pitch
        {
            get { return _pitch; }
            set { _pitch = value; }
        }

        private float _roll;
        public float Roll
        {
            get { return _roll; }
            set { _roll = value; }
        }

    }

    public class AccelerometerData
    {
        public AccelerometerData()
        {
            _dataVector = new Vector3();
        }

        private Vector3 _dataVector;
        public Vector3 DataVector
        {
            get { return _dataVector; }
            set { _dataVector = value; }
        }
    }

    public class CompassData
    {
        public CompassData()
        {
            _dataVector = new Vector3();
        }

        private Vector3 _dataVector;
        public Vector3 DataVector
        {
            get { return _dataVector; }
            set { _dataVector = value; }
        }
    }

    public class SensorDataRaw
    {
        private int _logicalID;
        public int LogicalID
        {
            get { return _logicalID; }
            set { _logicalID = value; }
        }

        private byte[] _accGyroReadbuffer;
        public byte[] AccGyroReadbuffer
        {
            get { return _accGyroReadbuffer; }
            set { _accGyroReadbuffer = value; }
        }

        private byte[] _quatReadbuffer;

        public byte[] QuatReadbuffer
        {
            get { return _quatReadbuffer; }
            set { _quatReadbuffer = value; }
        }

    }


    public class SensorData
    {
        private int _logicalID;
        public int LogicalID
        {
            get { return _logicalID; }
            set { _logicalID = value; }
        }

        private Quaternion _quaternionObject;
        public Quaternion QuaternionObject
        {
            get { return _quaternionObject; }
            set { _quaternionObject = value; }
        }

        private GyroData _gyroDataObject;
        public GyroData GyroDataObject
        {
            get { return _gyroDataObject; }
            set { _gyroDataObject = value; }
        }

        private AccelerometerData _accDataObject;
        public AccelerometerData AccDataObject
        {
            get { return _accDataObject; }
            set { _accDataObject = value; }
        }

        private CompassData _compassDataObject;
        public CompassData CompassDataObject
        {
            get { return _compassDataObject; }
            set { _compassDataObject = value; }
        }

        public SensorData()
        {
            _gyroDataObject = new GyroData();
            _accDataObject = new AccelerometerData();
            _compassDataObject = new CompassData();
        }
    }
}

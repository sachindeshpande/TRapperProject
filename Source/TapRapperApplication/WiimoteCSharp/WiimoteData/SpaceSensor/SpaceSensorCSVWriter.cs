using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities;
using Logging;
using System.Threading;

namespace WiimoteData.SpaceSensor
{
    public class SpaceSensorCSVLineItem : WiimoteCSVLineItem
    {
        public SpaceSensorCSVLineItem(string[] rowHeader,string[] sensor1Data, string[] sensor2Data)
        {
            _rowHeader = rowHeader;
            _sensor1Data = sensor1Data;
            _sensor2Data = sensor2Data;
        }


        private string[] _rowHeader;
        public string[] RowHeader
        {
            get { return _rowHeader; }
            set { _rowHeader = value; }
        }

        private string[] _sensor1Data;
        public string[] Sensor1Data
        {
            get { return _sensor1Data; }
            set { _sensor1Data = value; }
        }
        private string[] _sensor2Data;

        public string[] Sensor2Data
        {
            get { return _sensor2Data; }
            set { _sensor2Data = value; }
        }
    }

    public class SpaceSensorCSVWriter : WiimoteCSVFileWriter
    {
        public const int SPACE_SENSOR_NUMBER_OF_COLUMNS_LOGGING = 22;

        private List<string[]> _fileDataBuffer;

        private Thread _sensorLoggingThread;
        private bool _sensorLoggingThreadStatus;

        string[] fullSensorDataRow = new string[22];


        protected Queue<SpaceSensorCSVLineItem> _spaceSensorCSVQueue = new Queue<SpaceSensorCSVLineItem>();

        public SpaceSensorCSVWriter(string path)
            : base(path)
        {
            mInformationRow = new string[SPACE_SENSOR_NUMBER_OF_COLUMNS_LOGGING];
            _fileDataBuffer = new List<string[]>();

            _sensorLoggingThread = new Thread(SensorLoggingThread);
            _sensorLoggingThreadStatus = true;
            _sensorLoggingThread.Start();
        }

        public override void logHeader(IWiimoteCalibrationRecordInfo p_CalibrationRecord)
        {
            try
            {
                writeLine("Sensor," + "3Space");
                writeLine("BPM," + 0);
                writeLine("BPB," + 0);
                writeLine("NumBar," + 0);
                writeLine("LeadIn," + 0);
                writeLine();

                writeLine("MusicFile,None");
                writeLine();

                writeLine("Calibration Info Start");
                if (p_CalibrationRecord.WiimotesSwitched)
                    writeLine("LRswitched,True");
                else
                    writeLine("LRswitched,False");

                writeLine("Sequence #,Time,Acc X #S1,Acc Y #S1,Acc Z #S1," +
                                      "Pitch #S1,Yaw #S1, Roll #S1," +
                                      "Quat X #S1,Quat Y #S1,Quat Z #S1, Quat W #S1," +
                                      "Acc X #S2,Acc Y #S2,Acc Z #S2," +
                                      "Pitch #S2, Yaw #S2, Roll #S2," +
                                      "Quat X #S2,Quat Y #S2,Quat Z #S2, Quat W #S2,");

                string calibrationMessage = "0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0";
                writeLine(calibrationMessage);


                writeLine("Calibration Info End");
                writeLine();

                writeLine("Sequence #,Time,Acc X #S1,Acc Y #S1,Acc Z #S1," +
                                      "Pitch #S1, Yaw #S1,Roll #S1," +
                                      "Quat X #S1,Quat Y #S1,Quat Z #S1, Quat W #S1," +
                                      "Acc X #S2,Acc Y #S2,Acc Z #S2," +
                                      "Pitch #S2,Yaw #S2,Roll #S2," +
                                      "Quat X #S2,Quat Y #S2,Quat Z #S2, Quat W #S2,");
            }
            catch (CSVFileException e)
            {
                throw e;
            }
        }


        public override void addCSVLineItemToLogFile(WiimoteCSVLineItem csvLineItem)
        {
            _spaceSensorCSVQueue.Enqueue((SpaceSensorCSVLineItem)csvLineItem);            
        }


        private void SensorLoggingThread()
        {
            while (_sensorLoggingThreadStatus)
            {
                Thread.Sleep(10);

                if (_spaceSensorCSVQueue.Count == 0)
                    continue;

                SpaceSensorCSVLineItem csvLineItem =  _spaceSensorCSVQueue.Dequeue();
//                string[] fullSensorDataRow = new string[22];

                csvLineItem.RowHeader.CopyTo(fullSensorDataRow, 0);
                csvLineItem.Sensor1Data.CopyTo(fullSensorDataRow, csvLineItem.RowHeader.Length);
                csvLineItem.Sensor2Data.CopyTo(fullSensorDataRow, csvLineItem.RowHeader.Length + csvLineItem.Sensor1Data.Length);

                base.addInformationToLogFile(fullSensorDataRow);
            }
        }

        public override void close()
        {
            _sensorLoggingThreadStatus = false;
            base.close();
        }


        //public override void writeLine(string[] row, bool wiimotesSwitched)
        //{
        //    _fileDataBuffer.Add(row);
        //}

        //public override void close()
        //{
        //    foreach (string[] rowData in _fileDataBuffer)
        //        base.writeLine(rowData, false);

        //    base.close();
        //}

    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpaceSensor;
using System.Runtime.CompilerServices;
using ProjectCommon;
using Utilities;
using System.Threading;
using Microsoft.Xna.Framework;
using Logging;

namespace WiimoteData.SpaceSensor
{

    public class SpaceSensorDataAdaptor : DataAdaptor
    {

        private SpaceSensorMain _spaceSensorMain;
        private bool _spaceProcessingStatus;
        private long _sequenceNumber = 0;
        private DateTime _startRecordingTime;
        private Queue<SpaceSensorRecordingDataReceivedEventArgs> _spaceSensorDataQueue = new Queue<SpaceSensorRecordingDataReceivedEventArgs>();
        private object _recordingStopSync = new object();

        public SpaceSensorDataAdaptor(Wiimotes pParent)
            : base(pParent)
        {

        }


        public override void initialize(WiimoteCalibrationRecordInfo calibrationRecord)
        {
            _spaceSensorMain = new SpaceSensorMain();
            m_CalibrationRecord = calibrationRecord;

        }

        public override void cleanup()
        {
        }

        #region 3Space Sensor Calls


        private void startWiimotePipe()
        {
            try
            {

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private void stopWiimotePipe()
        {

        }


        public void startWiimoteDataCollection()
        {
            string startWiimoteLoggingMessage = START_DATA_SENDING_METHOD_NAME + ";";
        }

        #endregion


        #region Connection Calls
        public override bool connectWiimote()
        {
            try
            {
                if (ProjectCommon.ProjectConstants.DEFAULT_WIIMOTE_SIMULATION_MODE)
                    return true;

                _spaceSensorMain.Connect();
                return true;
            }
            catch (Exception ex)
            {
                throw new WiimoteCommunicationException(ex);
            }
            return false;
        }


        public override void disconnectWiimote()
        {
            try
            {
                if (ProjectCommon.ProjectConstants.DEFAULT_WIIMOTE_SIMULATION_MODE)
                    return;

                _spaceSensorMain.Disconnect();
            }
            catch (Exception ex)
            {
                throw new WiimoteCommunicationException(ex);
            }
        }


        #endregion

        #region Recording/Logging calls


        public override void setWiimoteSimulationMode(bool p_SimulationMode, string p_SimulationFile)
        {
            _spaceSensorMain.setSimulationMode(p_SimulationMode, p_SimulationFile);
        }

        public override string setWiimoteDataMode(int p_WiimoteDataMode)
        {
            string setWiimoteDataModeMessage = SET_WIIMOTE_DATA_MODE_METHOD_NAME + "," + p_WiimoteDataMode + ";";
            return _spaceSensorMain.setDataMode(p_WiimoteDataMode);
        }

        public override string setCalibration()
        {
            string setCalibrationMessage = SET_CALIBRATION_METHOD_NAME + "," + m_CalibrationRecord.getCalibrationValue(ProjectConstants.WIMMOTE1_DATA_ACCX_COLUMN_INDEX) + "," +
                    m_CalibrationRecord.getCalibrationValue(ProjectConstants.WIMMOTE1_DATA_ACCY_COLUMN_INDEX) + "," +
                    m_CalibrationRecord.getCalibrationValue(ProjectConstants.WIMMOTE1_DATA_ACCZ_COLUMN_INDEX) + "," +
                    m_CalibrationRecord.getCalibrationValue(ProjectConstants.WIMMOTE1_DATA_YAW_COLUMN_INDEX) + "," +
                    m_CalibrationRecord.getCalibrationValue(ProjectConstants.WIMMOTE1_DATA_PITCH_COLUMN_INDEX) + "," +
                    m_CalibrationRecord.getCalibrationValue(ProjectConstants.WIMMOTE1_DATA_ROLL_COLUMN_INDEX) + "," +
                    m_CalibrationRecord.getCalibrationValue(ProjectConstants.WIMMOTE2_DATA_ACCX_COLUMN_INDEX) + "," +
                    m_CalibrationRecord.getCalibrationValue(ProjectConstants.WIMMOTE2_DATA_ACCY_COLUMN_INDEX) + "," +
                    m_CalibrationRecord.getCalibrationValue(ProjectConstants.WIMMOTE2_DATA_ACCZ_COLUMN_INDEX) + "," +
                    m_CalibrationRecord.getCalibrationValue(ProjectConstants.WIMMOTE2_DATA_YAW_COLUMN_INDEX) + "," +
                    m_CalibrationRecord.getCalibrationValue(ProjectConstants.WIMMOTE2_DATA_PITCH_COLUMN_INDEX) + "," +
                    m_CalibrationRecord.getCalibrationValue(ProjectConstants.WIMMOTE2_DATA_ROLL_COLUMN_INDEX) + ";";

            //return m_CommandPipe.sendMessage(setCalibrationMessage);
            return "";

        }

        /**
         * This method
         *  1) Prepares the csv log file for logging Wiimote Data
         *  2) Initializes the Matlab matrix to send data
         */

        public override string startWiimoteLogging(IWiimoteChildRecord wiimoteRecord)
        {
            try
            {
                ITrainingSegmentInfo l_ParentRecord = (ITrainingSegmentInfo)wiimoteRecord.ParentRecord;
                //TODO : Change to member variable
                IWiimoteCalibrationRecordInfo l_Calibration = WiimoteDataStore.getWiimoteDataStore().Calibration;
                setCalibration();
                StartDataCollection(wiimoteRecord.FilePath);

                startRecordingSignalMatlab();
            }
            catch (CSVFileFormatException ex)
            {
                throw new WiimoteCommunicationException(ex);
            }
            catch (Exception ex)
            {
                throw new WiimoteCommunicationException(ex);
            }
            return null;
        }


        public override bool StartDataCollection(string csvFilepath, bool csvFileCreate = true)
        {
            if (csvFileCreate)
            {
                m_CSVFileWriter = new SpaceSensorCSVWriter(csvFilepath);
                m_CSVFileWriter.logHeader(m_CalibrationRecord);
            }
            else
                m_CSVFileWriter = null;



            if (ProjectCommon.ProjectConstants.DEFAULT_WIIMOTE_SIMULATION_MODE)
            {
                Thread spaceSensorRecording;
                spaceSensorRecording = new Thread(SpaceSensorRecordingThreadSimulation);
            }
            else
            {
                _startRecordingTime = DateTime.Now;
                Thread spaceSensorProcessing;
                spaceSensorProcessing = new Thread(SpaceSensorProcessingThread);
                _spaceProcessingStatus = true;
                spaceSensorProcessing.Start();

                _spaceSensorMain.SpaceSensorDataEvent += new SpaceSensorMain.OnSpaceSensorDataEvent(ReceivedSpaceSensorData);
                _spaceSensorMain.StartReading();
            }


            return true;
        }

        public void ReceivedSpaceSensorData(object sender, SpaceSensorRecordingDataReceivedEventArgs args)
        {
            _spaceSensorDataQueue.Enqueue(args);

        }

        private void SpaceSensorProcessingThread()
        {
            while (true)
            {
                while(_spaceSensorDataQueue.Count == 0)
                {
                    if(!_spaceProcessingStatus)
                        return;

                    Thread.Sleep(10);
                }

                string[] rowHeader = new string[2];
                _sequenceNumber++;
                rowHeader[0] = _sequenceNumber.ToString();
                //rowHeader[1] = String.Format("{0:hh:mm:ss.fff}", DateTime.Now);
                TimeSpan recordTime = DateTime.Now - _startRecordingTime;
                rowHeader[1] = recordTime.TotalMilliseconds.ToString();

                    SpaceSensorRecordingDataReceivedEventArgs args =    _spaceSensorDataQueue.Dequeue();
                SensorData sensor1DataObj = new SensorData();
                ReadAccGyroData(args.Sensor1DataPacket,sensor1DataObj);

                SensorData sensor2DataObj = new SensorData();
                ReadAccGyroData(args.Sensor2DataPacket,sensor2DataObj);

                string[] sensor1Data = ConvertSensorDataToCSV(sensor1DataObj);
                string[] sensor2Data = ConvertSensorDataToCSV(sensor2DataObj);

                if(m_CSVFileWriter != null)
                    logSpaceSensorCSVLineItem(new SpaceSensorCSVLineItem(rowHeader, sensor1Data, sensor2Data));


            }
        }

        private void ReadAccGyroData(SensorDataRaw rawData, SensorData sensorDataObj)
        {
            byte []readbuffer = rawData.AccGyroReadbuffer;

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


        private string[] ConvertSensorDataToCSV(SensorData sensorDataObj)
        {
            //Quaternion sensorQuatObj = _spaceSensorMain.SpaceSensorObject2.ReadOrientation();
            string[] dataRow = new string[10];

            if (sensorDataObj.AccDataObject != null)
            {
                dataRow[0] = sensorDataObj.AccDataObject.DataVector.X.ToString();
                dataRow[1] = sensorDataObj.AccDataObject.DataVector.Y.ToString();
                dataRow[2] = sensorDataObj.AccDataObject.DataVector.Z.ToString();
                dataRow[3] = sensorDataObj.GyroDataObject.Pitch.ToString();
                dataRow[4] = sensorDataObj.GyroDataObject.Yaw.ToString();
                dataRow[5] = sensorDataObj.GyroDataObject.Roll.ToString();
            }

            if (sensorDataObj.QuaternionObject != null)
            {
                dataRow[6] = sensorDataObj.QuaternionObject.X.ToString();
                dataRow[7] = sensorDataObj.QuaternionObject.Y.ToString();
                dataRow[8] = sensorDataObj.QuaternionObject.Z.ToString();
                dataRow[9] = sensorDataObj.QuaternionObject.W.ToString();
            }


            return dataRow;
        }


        public void logSpaceSensorCSVLineItem(SpaceSensorCSVLineItem csvLineItem)
        {
            m_CSVFileWriter.addCSVLineItemToLogFile(csvLineItem);
        }


        private void SpaceSensorRecordingThreadSimulation()
        {
            CSVFileParser l_Parser = new CSVFileParser();
            l_Parser.startParsingCSVData(ProjectConstants.SPACE_SENSOR_SIMULATED_DATA_PATH, 0, 14);
            string[] row;


            while ((row = l_Parser.getNextRow(0)) != null)
            {

                SensorData sensor1DataObj = new SensorData();
                ReadSimulationSensorData(sensor1DataObj,row,2);

                SensorData sensor2DataObj = new SensorData();
                ReadSimulationSensorData(sensor2DataObj, row, 12);

                mParent.ReceivedEventRecordingData(sensor2DataObj, sensor2DataObj);
                
                Thread.Sleep(1);

            }

            l_Parser.close();
        }

        private void ReadSimulationSensorData(SensorData sensorDataObj,string []row,int startIndex)
        {
            Vector3 accData = new Vector3();
            accData.X = Convert.ToSingle(row[startIndex]);
            accData.Y = Convert.ToSingle(row[startIndex+1]);
            accData.Z = Convert.ToSingle(row[startIndex+2]);
            sensorDataObj.AccDataObject.DataVector = accData;


            GyroData gyroObj = sensorDataObj.GyroDataObject;
            gyroObj.Pitch = Convert.ToSingle(row[startIndex+3]);
            gyroObj.Yaw = Convert.ToSingle(row[startIndex+4]);
            gyroObj.Roll = Convert.ToSingle(row[startIndex+5]);


            Quaternion quatObj = sensorDataObj.QuaternionObject;
            quatObj.X = Convert.ToSingle(row[startIndex+6]);
            quatObj.Y = Convert.ToSingle(row[startIndex+7]);
            quatObj.Z = Convert.ToSingle(row[startIndex+8]);
            quatObj.W = Convert.ToSingle(row[startIndex+9]);

        }

        public override bool StopDataCollection()
        {
            try
            {
                _spaceSensorMain.StopReading();
                _spaceProcessingStatus = false;

                m_CSVFileWriter.close();
                return true;
            }
            catch (Exception ex)
            {
                throw new WiimoteCommunicationException(ex);
            }

        }

        public override void stopWiimoteLogging()
        {
            try
            {
                //                m_CommandPipe.sendMessage(STOP_DATA_SENDING_METHOD_NAME + ";");
                _spaceProcessingStatus = false;
                _spaceSensorMain.StopReading();
                //endRecordingSignalMatlab();
                m_CSVFileWriter.close();
            }
            catch (Exception ex)
            {
                throw new WiimoteCommunicationException(ex);
            }
        }

        public override bool checkWiimoteLoggingStatus()
        {
            try
            {
                //return mSpaceSensorMain.checkStatus();
                return true;
            }
            catch (Exception ex)
            {
                throw new WiimoteCommunicationException(ex);
            }
        }



        #endregion

        #region Wiimote state Checker

        [MethodImpl(MethodImplOptions.Synchronized)]
        public override void checkWiimoteState(Wiimote wiimote1, Wiimote wiimote2)
        {
            try
            {
                wiimote1.WiimoteCurrentState = WiimoteState.wiimoteGoodStateState;
                wiimote2.WiimoteCurrentState = WiimoteState.wiimoteGoodStateState;
                //Thread.Sleep(Configuration.getConfiguration().WaitTimeCheckConnection);

                //string checkWiimoteStateMessage = CHECK_WIIMOTE_STATE_METHOD_NAME + ";";
                //string returnString = m_CommandPipe.sendMessage(checkWiimoteStateMessage);
                //if (returnString.Length < 4)
                //    return;

                //string[] returnValues = returnString.Split(',');


                //mSpaceSensorMain.CheckState();

                //wiimote1.WiimoteCurrentState = (WiimoteState)Convert.ToInt32(returnValues[0]);
                //wiimote1.BatteryLevel = Convert.ToInt32(returnValues[1]);
                //wiimote2.WiimoteCurrentState = (WiimoteState)Convert.ToInt32(returnValues[2]);
                //wiimote2.BatteryLevel = Convert.ToInt32(returnValues[3]);

            }
            catch (Exception ex)
            {
                throw new WiimoteCommunicationException(ex);
            }
        }

        #endregion

    }
}


/**


 * 
 
         private void SpaceSensorRecordingThread()
        {
            while (_spaceProcessingStatus)
            {
                string[] rowHeader = new string[2];
                _sequenceNumber++;
                rowHeader[0] = _sequenceNumber.ToString();
                //rowHeader[1] = String.Format("{0:hh:mm:ss.fff}", DateTime.Now);
                TimeSpan recordTime = DateTime.Now - _startRecordingTime;
                rowHeader[1] = recordTime.TotalMilliseconds.ToString();

                SensorDataRaw sensor1DataObj = _spaceSensorMain.SpaceSensorObject1.ReadData();
                SensorDataRaw sensor2DataObj = _spaceSensorMain.SpaceSensorObject2.ReadData();
                mParent.ReceivedEventRecordingData(sensor1DataObj, sensor2DataObj);

                string[] sensor1Data = ReadSensorData(sensor1DataObj);
                string[] sensor2Data = ReadSensorData(sensor2DataObj);

                if(m_CSVFileWriter != null)
                    logSpaceSensorCSVLineItem(new SpaceSensorCSVLineItem(rowHeader, sensor1Data, sensor2Data));

                mParent.ReceivedEventRecordingData(sensor2DataObj, sensor2DataObj);

                Thread.Sleep(1);

            }

        }



**/
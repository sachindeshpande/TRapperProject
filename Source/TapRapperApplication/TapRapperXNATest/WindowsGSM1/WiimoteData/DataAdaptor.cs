using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MotionRecognition;
using ProjectCommon;
using MatlabWrapper;

namespace WiimoteData
{
    public abstract class DataAdaptor
    {
        public const string RECORD_NAME_TAG = "RecordName";
        public const string RECORDED_TIME_TAG = "Recorded Time";
        public const string NUM_BEATS_PER_MINUTE_TAG = "NumBeatsPerMinute";
        public const string NUM_BEATS_PER_BAR_TAG = "NumBeatsPerBar";
        public const string NUM_BARS_TAG = "NumBars";
        public const string VIDEO_FILE_PATH_TAG = "VideoFilePath";

        public const string WIIMOTE_MOTIONPLUS_ACC_DATA_MODE_STRING = "0";
        public const string WIIMOTE_MOTIONPLUS_ACC_IR_DATA_STRING = "1";
        public const string START_DATA_SENDING_METHOD_NAME = "startWiimoteDataSending";
        public const string START_RECORDING_METHOD_NAME = "startRecording";
        public const string STOP_DATA_SENDING_METHOD_NAME = "stopWiimoteDataSending";
        public const string CHECK_WIIMOTE_STATE_METHOD_NAME = "checkWiimoteState";
        public const string SET_CALIBRATION_METHOD_NAME = "setCalibration";
        public const string STOP_WIIMOTE_PROCESS_METHOD_NAME = "stopWiimoteProcess";
        public const string SET_WIIMOTE_SIMULATION_MODE_METHOD_NAME = "setWiimoteSimulationMode";
        public const string SET_WIIMOTE_DATA_MODE_METHOD_NAME = "setWiimoteDataMode";

        public const int NUM_ROWS_MATLAB_MATRIX = 100;
        public const int NUM_COLUMNS_MATLAB_MATRIX = 17;

        public const string DATA_TRANSMISSION_END_TAG = "-9999";

        protected Wiimotes mParent;

        protected Queue<string[]> mWiimoteAdditionalInfo = new Queue<string[]>();
        protected WiimoteCSVFileWriter m_CSVFileWriter;
        protected WiimoteCalibrationRecordInfo m_CalibrationRecord;

        //        private List<string[]> mWiimoteDataMatrix = new List<string[]>();
        protected MotionRecognitionMain mMotionRecognition;
        protected int mCurrentMatrixRowIndex = 0;

        public abstract void initialize(WiimoteCalibrationRecordInfo calibrationRecord);
        public abstract void cleanup();

        public abstract bool connectWiimote();
        public abstract void disconnectWiimote();

        public abstract void setWiimoteSimulationMode(bool p_SimulationMode, string SimulationFile);
        public abstract string setWiimoteDataMode(int p_WiimoteDataMode);
        public abstract string setCalibration();
        public abstract string startWiimoteLogging(IWiimoteChildRecord wiimoteRecord);
        public abstract void stopWiimoteLogging();
        public abstract bool checkWiimoteLoggingStatus();

        public abstract bool StartDataCollection(string csvFilepath, bool csvFileCreate = true);
        public abstract bool StopDataCollection();
        
        

        public abstract void checkWiimoteState(Wiimote wiimote1, Wiimote wiimote2);

        public DataAdaptor(Wiimotes pParent)
        {

            mParent = pParent;
            mMotionRecognition = new MotionRecognitionMain();
        }

        protected bool isEndWiimoteData(string[] wiimoteDataStream)
        {

            if (wiimoteDataStream[0].CompareTo(DATA_TRANSMISSION_END_TAG) == 0)
                return true;
            else
                return false;
        }

        /**
         * This method is used by the Video Player
         * **/

        public void addInformationToRecording(string[] pRowData)
        {
            mWiimoteAdditionalInfo.Enqueue(pRowData);
        }

        public virtual void logWiimoteData(string[] rowData)
        {
            while (mWiimoteAdditionalInfo.Count > 0)
            {
                string[] lRowData = mWiimoteAdditionalInfo.Dequeue();
                m_CSVFileWriter.addInformationToLogFile(lRowData);
            }

            m_CSVFileWriter.writeLine(rowData, m_CalibrationRecord.WiimotesSwitched);
        }


        #region Matlab Wrapper calls

        protected void startRecordingSignalMatlab()
        {
        }

        protected void endRecordingSignalMatlab()
        {
        }

        protected void setWiimoteDataMatrix()
        {

        }

        protected void addMatrixRow(string[] pRow)
        {
            double[] lWiimoteRecord = new double[ProjectConstants.WIMMOTE_NUMBER_OF_COLUMNS];
            lWiimoteRecord[0] = Convert.ToDouble(pRow[ProjectConstants.WIMMOTE1_DATA_ACCX_COLUMN_INDEX]);
            lWiimoteRecord[1] = Convert.ToDouble(pRow[ProjectConstants.WIMMOTE1_DATA_ACCY_COLUMN_INDEX]);
            lWiimoteRecord[2] = Convert.ToDouble(pRow[ProjectConstants.WIMMOTE1_DATA_ACCZ_COLUMN_INDEX]);

            lWiimoteRecord[3] = Convert.ToDouble(pRow[ProjectConstants.WIMMOTE1_DATA_YAW_COLUMN_INDEX]);
            lWiimoteRecord[4] = Convert.ToDouble(pRow[ProjectConstants.WIMMOTE1_DATA_PITCH_COLUMN_INDEX]);
            lWiimoteRecord[5] = Convert.ToDouble(pRow[ProjectConstants.WIMMOTE1_DATA_ROLL_COLUMN_INDEX]);


            lWiimoteRecord[6] = Convert.ToDouble(pRow[ProjectConstants.WIMMOTE2_DATA_ACCX_COLUMN_INDEX]);
            lWiimoteRecord[7] = Convert.ToDouble(pRow[ProjectConstants.WIMMOTE2_DATA_ACCY_COLUMN_INDEX]);
            lWiimoteRecord[8] = Convert.ToDouble(pRow[ProjectConstants.WIMMOTE2_DATA_ACCZ_COLUMN_INDEX]);

            lWiimoteRecord[9] = Convert.ToDouble(pRow[ProjectConstants.WIMMOTE2_DATA_YAW_COLUMN_INDEX]);
            lWiimoteRecord[10] = Convert.ToDouble(pRow[ProjectConstants.WIMMOTE2_DATA_PITCH_COLUMN_INDEX]);
            lWiimoteRecord[11] = Convert.ToDouble(pRow[ProjectConstants.WIMMOTE2_DATA_ROLL_COLUMN_INDEX]);

            mMotionRecognition.addMotionDataRecord(lWiimoteRecord);
        }

        public void OnWiimoteActionEvent(object sender, WiimoteActionEventArgs e)
        {
        }

        public string getScore(IWiimoteChildRecord l_SelectedReferenceRow, IWiimoteChildRecord l_SelectedPlayRow,
            out double score, out double stars)
        {
            string lMatlabOptionCode;
            WiimoteReferenceRecord lReferenceRecord = (WiimoteReferenceRecord)l_SelectedReferenceRow.ParentRecord;

            //TODO : This is a temporary hack to check for Pop Overs
            if (lReferenceRecord.RecordName.Contains("Intro"))
                lMatlabOptionCode = ProjectConstants.TRAINING_MATLAB_OPTION_PARTS_OF_THE_FEET;
            else if (lReferenceRecord.RecordName.Contains("Toe"))
                lMatlabOptionCode = ProjectConstants.TRAINING_MATLAB_OPTION_TOE_STANDS;
            else
                lMatlabOptionCode = ProjectConstants.TRAINING_MATLAB_OPTION_POP_OVERS;


            return MatlabWiimoteWrapper.calculateScore(lMatlabOptionCode, l_SelectedPlayRow.FilePath,
                out score, out stars);

        }

        #endregion

    }
}

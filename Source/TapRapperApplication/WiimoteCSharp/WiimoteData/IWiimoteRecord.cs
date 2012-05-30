using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using System.Xml;
using System.Xml.Serialization;

using ProjectCommon;

using Utilities;
using Logging;


namespace WiimoteData
{
    #region base interfaces
    public enum WiimoteState
    {
        wiimoteBadDataState = 1,
        wiimoteDisconnectedState = 2,
        wiimoteConnectedState = 3,
        wiimoteGoodStateState = 4,
    }

    public interface IWiimoteRecordStore
    {
        void save();
        IWiimoteCalibrationRecordInfo Calibration { get; }
    }

    public interface IWiimoteRecord
    {
        int RecordID { get; }

        string RecordName { get; set; }
    }

    #endregion

    #region Derived Interfaces

    public interface IChildRecord : IWiimoteRecord
    {
        IParentRecord ParentRecord { get; set; }
    }

    public interface IWiimoteChildRecord : IChildRecord
    {

        string FilePath { get; }

        DateTime RecordedTime { get; set; }

        bool RecordingDone { get; set; }

        bool isRecordingValid();

        string[] getNextDataRow(int rowSkipStep);
    }

    public interface IParentRecord : IWiimoteRecord
    {
        void deleteChildItem(int index);

        int HighestRecordingItemIndex { get; set; }

        void linkChildToParent();
    }

    public interface IWiimoteParentRecord : IParentRecord
    {
    }


    public interface IWiimoteReferenceRecord : IWiimoteParentRecord, IWiimoteChildRecord
    {
        bool SlowMoOption { get; set; }

        bool ScoringOption { get; set; }

        bool RepeatOption { get; set; }

        string VideoPath { get; set; }

        WiimoteReferenceRecordingItem SelectedRecordingItem { get; set; }
    }

    public interface IWiimoteCalibrationRecord : IWiimoteReferenceRecord
    {
    }

    public interface IWiimotePlayRecord : IWiimoteChildRecord
    {
        double Score { get; set; }

        double AverageDelay { get; set; }

        double Fluctuation { get; set; }

        int NumberOfStars { get; set; }
    }

    public interface ITrainingSegmentInfo : IParentRecord
    {
        IWiimoteReferenceRecord addWiimoteReferenceRecord(string p_ReferenceName,string p_VideoPath);

        int getNumberOfWiimoteReferenceRecords();

        IWiimoteReferenceRecord getWiimoteReferenceRecord(int index);

        void moveWiimoteReferenceRecord(IWiimoteReferenceRecord pReferenceRecord, int pPosition);

        IWiimoteReferenceRecord TrainingReferenceRecord { get; set; }

        IWiimotePlayRecord TrainingPlayRecord { get; set; }

        IWiimotePlayRecord addWiimotePlayRecord(string p_PlayName,
            DateTime p_RecordedTime, int p_Score);

        //This attribute checks if any of the children have SlowMo option
        [XmlIgnore]
        bool SlowMoOption { get; }

        //This attribute checks if any of the children have scoring option
        [XmlIgnore]
        bool ScoringOption { get; }


        int NumBeatsPerMinute { get; set; }

        int NumBeatsPerBar { get; set; }

        int NumBars { get; set; }

        int LeadIn { get; set; }

        int RecordingInterval { get; set; }

        IWiimoteReferenceRecord getReferenceRecord(int lIndex);

        IWiimoteReferenceRecord getMainReferenceRecord();

        IWiimoteReferenceRecord getSlowMoReferenceRecord();

        IWiimoteReferenceRecord getScoringReferenceRecord();

        void deleteAllWiimoteRefernceRecords();

    }

    public interface IWiimoteCalibrationRecordInfo : ITrainingSegmentInfo
    {
        IWiimoteCalibrationRecord getStandStillRecordingItem(DateTime p_RecordedTime);

        IWiimoteCalibrationRecord getLeftRecordingItem(DateTime p_RecordedTime);

        IWiimoteCalibrationRecord getRightRecordingItem(DateTime p_RecordedTime);

        void calculateCalibration();

        void resetCalibration();

        bool WiimotesSwitched { get; }

        double getCalibrationValue(int p_Key);

    }

    #endregion


    #region Abstract Base Classes

    public abstract class WiimoteRecordBase
    {
        public const int TIME_COLUMN_INDEX = 1;
        public const int WIIMOTE1_ACC_PITCH_ANGLE_COLUMN_INDEX = 2;
        public const int WIIMOTE1_ACC_ROLL_ANGLE_COLUMN_INDEX = 3;
        public const int WIIMOTE1_ACC_X_COLUMN_INDEX = 4;
        public const int WIIMOTE1_ACC_Y_COLUMN_INDEX = 5;
        public const int WIIMOTE1_ACC_Z_COLUMN_INDEX = 6;
        public const int WIIMOTE1_SPEED_YAW_COLUMN_INDEX = 10;
        public const int WIIMOTE1_SPEED_PITCH_COLUMN_INDEX = 11;
        public const int WIIMOTE1_SPEED_ROLL_COLUMN_INDEX = 12;
        public const int WIIMOTE2_ACC_PITCH_ANGLE_COLUMN_INDEX = 13;
        public const int WIIMOTE2_ACC_ROLL_ANGLE_COLUMN_INDEX = 14;
        public const int WIIMOTE2_ACC_X_COLUMN_INDEX = 15;
        public const int WIIMOTE2_ACC_Y_COLUMN_INDEX = 16;
        public const int WIIMOTE2_ACC_Z_COLUMN_INDEX = 17;
        public const int WIIMOTE2_SPEED_YAW_COLUMN_INDEX = 21;
        public const int WIIMOTE2_SPEED_PITCH_COLUMN_INDEX = 22;
        public const int WIIMOTE2_SPEED_ROLL_COLUMN_INDEX = 23;

        public const string TIME_COLUMN_HEADER = "Time";
        public const string WIIMOTE1_ACC_PITCH_ANGLE_COLUMN_HEADER = "Pitch Angle";
        public const string WIIMOTE1_ACC_ROLL_ANGLE_COLUMN_HEADER = "Roll Angle";
        public const string WIIMOTE1_ACC_X_COLUMN_HEADER = "Acc X";
        public const string WIIMOTE1_ACC_Y_COLUMN_HEADER = "Acc Y";
        public const string WIIMOTE1_ACC_Z_COLUMN_HEADER = "Acc Z";
        public const string WIIMOTE1_SPEED_YAW_COLUMN_HEADER = "Speed Yaw";
        public const string WIIMOTE1_SPEED_PITCH_COLUMN_HEADER = "Speed Pitch";
        public const string WIIMOTE1_SPEED_ROLL_COLUMN_HEADER = "Speed Roll";
        public const string WIIMOTE2_ACC_PITCH_ANGLE_COLUMN_HEADER = "Pitch Angle";
        public const string WIIMOTE2_ACC_ROLL_ANGLE_COLUMN_HEADER = "Roll Angle";
        public const string WIIMOTE2_ACC_X_COLUMN_HEADER = "Acc X";
        public const string WIIMOTE2_ACC_Y_COLUMN_HEADER = "Acc Y";
        public const string WIIMOTE2_ACC_Z_COLUMN_HEADER = "Acc Z";
        public const string WIIMOTE2_SPEED_YAW_COLUMN_HEADER = "Speed Yaw";
        public const string WIIMOTE2_SPEED_PITCH_COLUMN_HEADER = "Speed Pitch";
        public const string WIIMOTE2_SPEED_ROLL_COLUMN_HEADER = "Speed Roll";

        [XmlAttribute("RecordID")]
        public int RecordID { get; set; }

        [XmlAttribute("RecordName")]
        public string RecordName { get; set; }
    }

    public abstract class WiimoteChildRecordBase : WiimoteRecordBase
    {


        public WiimoteChildRecordBase()
        {
            RecordingDone = false;
            m_CSVDataParsingInProgress = false;
            m_FilepathDirtyFlag = false;
        }

        private const int NUMBER_OF_HEADER_ROWS = 4;
        private const int NUMBER_OF_UNUSEDDATA_ROWS_TO_SKIP = 10;

        [XmlAttribute("RecordedTime")]
        public DateTime RecordedTime { get; set; }

        [XmlAttribute("RecordingDone")]
        public bool RecordingDone { get; set; }

        [XmlIgnore]
        protected string m_RecordName;

        [XmlAttribute("RecordName")]
        public string RecordName
        {
            get
            {
                return m_RecordName;
            }
            set
            {
                m_RecordName = value;
            }
        }

        [XmlIgnore]
        public abstract string FilePath { get; }


        [XmlIgnore]
        public IParentRecord ParentRecord { get; set; }

        /*
        protected abstract WiimoteParentRecordBase getParentRecordByID(int p_ID);

        
        [XmlIgnore]
        public int m_ParentRecordID;

        [XmlAttribute("ParentRecordID")]
        public int ParentRecordID
        {
            get
            {
                return m_ParentRecordID;
            }
            set
            {
                m_ParentRecordID = value;
                ParentRecord = (IWiimoteParentRecord)getParentRecordByID(value);
            }
        }*/

        [XmlIgnore]
        protected bool m_FilepathDirtyFlag;

        [XmlIgnore]
        protected string m_PreviousFilePath;

        [XmlIgnore]
        protected bool m_CSVDataParsingInProgress;

        [XmlIgnore]
        protected CSVFileParser m_CSVFileParser;

        public bool isFilePresent()
        {
            return File.Exists(FilePath);
        }

        public bool isRecordingValid()
        {
            double leftStdDev;
            double rightStdDev;

            if (!isFilePresent())
                throw new WiimoteDataStoreException("StandStill Calibration Data file not present");

            //calculateDataStdDeviation(out leftStdDev, out rightStdDev);

            //if (leftStdDev <= 0 ||
            //    rightStdDev <= 0)
            //    return false;

            return true;

        }


        public string[] getNextDataRow(int rowSkipStep)
        {
            try
            {
                if (m_CSVDataParsingInProgress == false)
                {
                    m_CSVFileParser = new CSVFileParser();
                    m_CSVFileParser.startParsingCSVData(FilePath, NUMBER_OF_HEADER_ROWS, NUMBER_OF_UNUSEDDATA_ROWS_TO_SKIP);
                    m_CSVDataParsingInProgress = true;
                }

                string[] rows = m_CSVFileParser.getNextRow(rowSkipStep);
                if (rows == null)
                {
                    stopCSVParsing();
                }

                return rows;
            }
            catch (CSVFileException e)
            {
                throw e;
            }

        }

        private void stopCSVParsing()
        {
            m_CSVDataParsingInProgress = false;
            m_CSVFileParser.close();
            m_CSVFileParser = null;

        }

        public void calculateAverage(Dictionary<int,double> p_CalibrationValues)
        {
            try
            {
                //TODO : Change when code ready for new senser
                return;

                double[][] calibrationData = new double[ProjectConstants.DEFAULT_NUMBER_CALIBRATION_PARAM][];
                for (int i = 0; i < calibrationData.Length; i++)
                {
                    calibrationData[i] = new double[ProjectConstants.DEFAULT_MAX_CALIBRATION_RECORDS];
                }

                for (int l_RowIndex = 0; l_RowIndex < ProjectConstants.DEFAULT_MAX_CALIBRATION_RECORDS; l_RowIndex++)
                {
                    string[] l_Row = getNextDataRow(0);

                    if (l_Row == null)
                        break;

                    calibrationData[0][l_RowIndex] = Convert.ToDouble(l_Row[ProjectConstants.WIMMOTE1_DATA_ACCX_COLUMN_INDEX]);
                    calibrationData[1][l_RowIndex] = Convert.ToDouble(l_Row[ProjectConstants.WIMMOTE1_DATA_ACCY_COLUMN_INDEX]);
                    calibrationData[2][l_RowIndex] = Convert.ToDouble(l_Row[ProjectConstants.WIMMOTE1_DATA_ACCZ_COLUMN_INDEX]);
                    calibrationData[3][l_RowIndex] = Convert.ToDouble(l_Row[ProjectConstants.WIMMOTE1_DATA_YAW_COLUMN_INDEX]);
                    calibrationData[4][l_RowIndex] = Convert.ToDouble(l_Row[ProjectConstants.WIMMOTE1_DATA_PITCH_COLUMN_INDEX]);
                    calibrationData[5][l_RowIndex] = Convert.ToDouble(l_Row[ProjectConstants.WIMMOTE1_DATA_ROLL_COLUMN_INDEX]);

                    calibrationData[6][l_RowIndex] = Convert.ToDouble(l_Row[ProjectConstants.WIMMOTE2_DATA_ACCX_COLUMN_INDEX]);
                    calibrationData[7][l_RowIndex] = Convert.ToDouble(l_Row[ProjectConstants.WIMMOTE2_DATA_ACCY_COLUMN_INDEX]);
                    calibrationData[8][l_RowIndex] = Convert.ToDouble(l_Row[ProjectConstants.WIMMOTE2_DATA_ACCZ_COLUMN_INDEX]);
                    calibrationData[9][l_RowIndex] = Convert.ToDouble(l_Row[ProjectConstants.WIMMOTE2_DATA_YAW_COLUMN_INDEX]);
                    calibrationData[10][l_RowIndex] = Convert.ToDouble(l_Row[ProjectConstants.WIMMOTE2_DATA_PITCH_COLUMN_INDEX]);
                    calibrationData[11][l_RowIndex] = Convert.ToDouble(l_Row[ProjectConstants.WIMMOTE2_DATA_ROLL_COLUMN_INDEX]);

                }

                if (m_CSVFileParser != null)
                    stopCSVParsing();

                p_CalibrationValues[ProjectConstants.WIMMOTE1_DATA_ACCX_COLUMN_INDEX] = MathUtilities.Average(calibrationData[0]);
                p_CalibrationValues[ProjectConstants.WIMMOTE1_DATA_ACCY_COLUMN_INDEX] = MathUtilities.Average(calibrationData[1]);
                p_CalibrationValues[ProjectConstants.WIMMOTE1_DATA_ACCZ_COLUMN_INDEX] = MathUtilities.Average(calibrationData[2]);
                p_CalibrationValues[ProjectConstants.WIMMOTE1_DATA_YAW_COLUMN_INDEX] = MathUtilities.Average(calibrationData[3]);
                p_CalibrationValues[ProjectConstants.WIMMOTE1_DATA_PITCH_COLUMN_INDEX] = MathUtilities.Average(calibrationData[4]);
                p_CalibrationValues[ProjectConstants.WIMMOTE1_DATA_ROLL_COLUMN_INDEX] = MathUtilities.Average(calibrationData[5]);

                p_CalibrationValues[ProjectConstants.WIMMOTE2_DATA_ACCX_COLUMN_INDEX] = MathUtilities.Average(calibrationData[6]);
                p_CalibrationValues[ProjectConstants.WIMMOTE2_DATA_ACCY_COLUMN_INDEX] = MathUtilities.Average(calibrationData[7]);
                p_CalibrationValues[ProjectConstants.WIMMOTE2_DATA_ACCZ_COLUMN_INDEX] = MathUtilities.Average(calibrationData[8]);
                p_CalibrationValues[ProjectConstants.WIMMOTE2_DATA_YAW_COLUMN_INDEX] = MathUtilities.Average(calibrationData[9]);
                p_CalibrationValues[ProjectConstants.WIMMOTE2_DATA_PITCH_COLUMN_INDEX] = MathUtilities.Average(calibrationData[10]);
                p_CalibrationValues[ProjectConstants.WIMMOTE2_DATA_ROLL_COLUMN_INDEX] = MathUtilities.Average(calibrationData[11]);

            }
            catch (CSVFileException e)
            {
                throw e;
            }
        }

        public void calculateDataStdDeviation(out double leftStdDev, out double rightStdDev)
        {
            try
            {
                //TODO : Change when code ready for new senser
                leftStdDev = 0;
                rightStdDev = 0;
                return;

                double[][] calibrationData = new double[ProjectConstants.DEFAULT_NUMBER_CALIBRATION_PARAM][];
                for (int i = 0; i < calibrationData.Length; i++)
                {
                    calibrationData[i] = new double[ProjectConstants.DEFAULT_MAX_CALIBRATION_RECORDS];
                }

                int l_RowIndex = 0;
                for (l_RowIndex = 0; l_RowIndex < ProjectConstants.DEFAULT_MAX_CALIBRATION_RECORDS; l_RowIndex++)
                {
                    string[] l_Row = getNextDataRow(0);

                    if (l_Row == null)
                        break;

                    calibrationData[0][l_RowIndex] = Convert.ToDouble(l_Row[ProjectConstants.WIMMOTE1_DATA_ACCX_COLUMN_INDEX]);
                    calibrationData[1][l_RowIndex] = Convert.ToDouble(l_Row[ProjectConstants.WIMMOTE1_DATA_ACCY_COLUMN_INDEX]);
                    calibrationData[2][l_RowIndex] = Convert.ToDouble(l_Row[ProjectConstants.WIMMOTE1_DATA_ACCZ_COLUMN_INDEX]);

                    calibrationData[3][l_RowIndex] = Convert.ToDouble(l_Row[ProjectConstants.WIMMOTE2_DATA_ACCX_COLUMN_INDEX]);
                    calibrationData[4][l_RowIndex] = Convert.ToDouble(l_Row[ProjectConstants.WIMMOTE2_DATA_ACCY_COLUMN_INDEX]);
                    calibrationData[5][l_RowIndex] = Convert.ToDouble(l_Row[ProjectConstants.WIMMOTE2_DATA_ACCZ_COLUMN_INDEX]);

                    if (l_RowIndex >= ProjectConstants.DEFAULT_MAX_CALIBRATION_RECORDS)
                        break;

                }

                if (m_CSVFileParser != null)
                    stopCSVParsing();

                leftStdDev = MathUtilities.Deviation(calibrationData[0], l_RowIndex) + MathUtilities.Deviation(calibrationData[1], l_RowIndex) + MathUtilities.Deviation(calibrationData[2], l_RowIndex);
                rightStdDev = MathUtilities.Deviation(calibrationData[3], l_RowIndex) + MathUtilities.Deviation(calibrationData[4], l_RowIndex) + MathUtilities.Deviation(calibrationData[5], l_RowIndex);

            }
            catch (CSVFileException e)
            {
                throw e;
            }
        }

    }

    public abstract class ParentRecordBase : WiimoteRecordBase
    {
        [XmlAttribute("HighestRecordingItemIndex")]
        public int HighestRecordingItemIndex { get; set; }

        public abstract void deleteChildItem(int index);

        public abstract void linkChildToParent();
    }

    public abstract class WiimoteParentRecordBase : ParentRecordBase
    {
        public const string BPM_ROW_KEY = "BPM";
        public const string BPB_ROW_KEY = "BPB";
        public const string NUMBAR_ROW_KEY = "NumBar";
        public const string LEAD_IN_KEY = "LeadIn";

        public const int AUTOMATIC_VIDEO_START_INDEX = 0;
        public const int USER_SELECTED_VIDEO_START_INDEX = 1;
        public const int LAST_VIDEO_START_INDEX = 1;



        public WiimoteParentRecordBase()
        {
            /*
            NextVideoSelectionOptionList = new string[3];
            NextVideoSelectionOptionList[0] = ProjectConstants.TRAINING_AUTOMATIC_NEXT_VIDEO_PLAY;
            NextVideoSelectionOptionList[1] = ProjectConstants.TRAINING_USER_SELECTION_NEXT_VIDEO_PLAY;
            NextVideoSelectionOptionList[2] = ProjectConstants.TRAINING_LAST_VIDEO_PLAY;
            NextVideoPlay = NextVideoSelectionOptionList[AUTOMATIC_VIDEO_START_INDEX];
             * */
        }

        public WiimoteParentRecordBase(bool needCSVParsing)
        {

        }


        /*
        //Not sure if NextVideoPlay should be part of WiimoteParentRecordBase.
        //Probably should be outof reference record and independent
        [XmlIgnore]
        public string[] NextVideoSelectionOptionList { get; set; }

        [XmlAttribute("NextVideoPlay")]
        public string NextVideoPlay { get; set; }
         * */

    }

    #endregion


}


/*
 *      protected string m_RecordName;
        [XmlAttribute("RecordName")]
        public string RecordName 
        {
            get
            {
                return m_RecordName;
            }
            set
            {

                try
                {
                    if (m_RecordName == null)
                    {
                        m_RecordName = value;
                        return;
                    }
                    string oldFilePath = ProjectCommon.ProjectConstants.WIIMOTE_DATA_PATH + "\\" + m_RecordName +
                    ProjectCommon.ProjectConstants.RECORDING_FILE_NAME_EXT;

                    string newFilePath = ProjectCommon.ProjectConstants.WIIMOTE_DATA_PATH + "\\" + value +
                    ProjectCommon.ProjectConstants.RECORDING_FILE_NAME_EXT;

                    if (File.Exists(oldFilePath))
                    {
                        File.Move(oldFilePath, newFilePath);
                    }

                    m_RecordName = value;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

*/


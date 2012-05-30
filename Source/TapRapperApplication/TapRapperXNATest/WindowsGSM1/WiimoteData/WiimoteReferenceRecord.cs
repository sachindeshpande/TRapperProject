using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Windows.Forms;

using ProjectCommon;
using Utilities;
using Logging;

namespace WiimoteData
{

    public enum CalibrationOption
    {
        None = 0,
        Dynamic = 1,
        System = 2,
    }

    # region Class WiimoteDataStore
    [XmlRoot("WiimoteDataStore")]
    public class WiimoteDataStore : IWiimoteRecordStore
    {
        [XmlIgnore]
        private List<TrainingSegmentInfo> trainingSegmentInfoRecordList = new List<TrainingSegmentInfo>();

        [XmlAttribute("HighestTrainingSegmentInfoIndex")]
        public int HighestTrainingSegmentInfoIndex { get; set; }

        private static object _dataStoreSync = new object();
        private static WiimoteDataStore m_WiimoteDataStore;

        [XmlElement("WiimoteCalibrationRecordInfo")]
        public WiimoteCalibrationRecordInfo m_Calibration { get; set; }

        [XmlIgnore]
        public WiimoteReferenceRecord DummyRecord { get; set; }


        public IWiimoteCalibrationRecordInfo Calibration
        { 
            get
            {
                if (m_Calibration == null)
                    m_Calibration = new WiimoteCalibrationRecordInfo();
                return m_Calibration;
            }
        }


        public WiimoteDataStore()
        {
            TrainingSegmentInfo.HighestReferenceIndex = 0;
            TrainingSegmentInfo.HighestPlayIndex = 0;
        }

        public void initialize()
        {
            DummyRecord = new WiimoteReferenceRecord();
            WiimoteReferenceRecordingItem lDummyRecordingItem = m_WiimoteDataStore.DummyRecord.addWiimoteReferenceRecordingItem("Dummy", CalibrationOption.None, DateTime.Now, false);
            DummyRecord.SelectedRecordingItem = lDummyRecordingItem;
        }

        public static WiimoteDataStore getWiimoteDataStore()
        {
            if(m_WiimoteDataStore == null)
                m_WiimoteDataStore = new WiimoteDataStore();
            return m_WiimoteDataStore;
        }

        private void initializeAfterLoad()
        {
            try
            {
                linkChildToParent();
                m_Calibration.calculateCalibration();

            }
            catch (WiimoteDataStoreException e)
            {                
                throw e;
            }
        }

        public TrainingSegmentInfo addTrainingSegmentInfoRecord(string pTrainingSegmentInfoName)
        {
            TrainingSegmentInfo record = new TrainingSegmentInfo(pTrainingSegmentInfoName);
            trainingSegmentInfoRecordList.Add(record);
            return record;
        }

        [XmlElement("TrainingSegmentInfo")]
        public TrainingSegmentInfo[] TrainingSegmentInfoRecords
        {
            get
            {
                TrainingSegmentInfo[] records = new TrainingSegmentInfo[trainingSegmentInfoRecordList.Count];
                trainingSegmentInfoRecordList.CopyTo(records);
                return records;
            }
            set
            {
                if (value == null) return;
                TrainingSegmentInfo[] records = (TrainingSegmentInfo[])value;
                trainingSegmentInfoRecordList.Clear();
                foreach (TrainingSegmentInfo record in records)
                    trainingSegmentInfoRecordList.Add(record);
            }
        }

        public int getNumberOfTrainingSegmentInfoRecords()
        {
            return trainingSegmentInfoRecordList.Count;
        }

        public ITrainingSegmentInfo getTrainingSegmentInfo(int pIndex)
        {
            return (ITrainingSegmentInfo)trainingSegmentInfoRecordList[pIndex];
        }

        public void deleteTrainingSegmentRecord(int index)
        {
            trainingSegmentInfoRecordList.RemoveAt(index);
        }


        public void save()
        {
            WiimoteDataStore.Serialize();
        }

        private void linkChildToParent()
        {
            m_Calibration.linkChildToParent();
            foreach (TrainingSegmentInfo record in trainingSegmentInfoRecordList)
                record.linkChildToParent();

        }

        public static void Serialize()
        {
            lock(_dataStoreSync)
            {
            // Serialization
            XmlSerializer s = new XmlSerializer(typeof(WiimoteDataStore));
            TextWriter w = new StreamWriter(ProjectConstants.WIIMOTE_DATA_PATH + @"\" + ProjectConstants.WIIMOTE_DATA_STORE);
            s.Serialize(w, WiimoteDataStore.m_WiimoteDataStore);
            w.Close();
            }
        }

        public static WiimoteDataStore Deserialize()
        {
            // Deserialization
            try
            {
                lock(_dataStoreSync)
                {
                XmlSerializer s = new XmlSerializer(typeof(WiimoteDataStore));

                if (!File.Exists(ProjectConstants.WIIMOTE_DATA_PATH + @"\" + ProjectConstants.WIIMOTE_DATA_STORE))
                    return WiimoteDataStore.m_WiimoteDataStore;

                TextReader r = new StreamReader(ProjectConstants.WIIMOTE_DATA_PATH + @"\" + ProjectConstants.WIIMOTE_DATA_STORE);
                WiimoteDataStore.m_WiimoteDataStore = (WiimoteDataStore)s.Deserialize(r);
                r.Close();


                m_WiimoteDataStore.initialize();
                m_WiimoteDataStore.initializeAfterLoad();
                }

            }
            catch (WiimoteDataStoreException e)
            {
                MessageBox.Show(e.Message,
                    "Wiimote Data Store Issue", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);                
            }
            return WiimoteDataStore.m_WiimoteDataStore;
        }
    }

    #endregion

    # region Class TrainingSegmentInfo
    public class TrainingSegmentInfo : ParentRecordBase, ITrainingSegmentInfo
    {
        #region Reference Records

        [XmlIgnore]
        private List<WiimoteReferenceRecord> referenceRecordList = new List<WiimoteReferenceRecord>();

        [XmlAttribute("HighestReferenceIndex")]
        public static int HighestReferenceIndex { get; set; }

        [XmlAttribute("NumBeatsPerMinute")]
        public int NumBeatsPerMinute { get; set; }
        [XmlAttribute("NumBeats")]
        public int NumBeatsPerBar { get; set; }
        [XmlAttribute("NumBars")]
        public int NumBars { get; set; }
        [XmlAttribute("LeadIn")]
        public int LeadIn { get; set; }
        [XmlAttribute("RecordingInterval")]
        public int RecordingInterval { get; set; }

        public TrainingSegmentInfo()
        {
        }


        public TrainingSegmentInfo(string pRecordName)
        {
            RecordName = pRecordName;
            RecordID = WiimoteDataStore.getWiimoteDataStore().HighestTrainingSegmentInfoIndex + 1;
            WiimoteDataStore.getWiimoteDataStore().HighestTrainingSegmentInfoIndex++;

            NumBeatsPerMinute = Configuration.getConfiguration().NumBeatsPerMinute;
            NumBeatsPerBar = Configuration.getConfiguration().NumBeatsPerBar;
            NumBars = Configuration.getConfiguration().NumBars;
            LeadIn = Configuration.getConfiguration().LeadIn;
            RecordingInterval = Configuration.getConfiguration().RecordingInterval;
        }


        [XmlElement("WiimoteReferenceRecord")]
        public WiimoteReferenceRecord[] WiimoteReferenceRecords
        {
            get
            {
                WiimoteReferenceRecord[] records = new WiimoteReferenceRecord[referenceRecordList.Count];
                referenceRecordList.CopyTo(records);
                return records;
            }
            set
            {
                if (value == null) return;
                WiimoteReferenceRecord[] records = (WiimoteReferenceRecord[])value;
                referenceRecordList.Clear();
                foreach (WiimoteReferenceRecord record in records)
                {
                    referenceRecordList.Add(record);
                    if (record.ScoringOption)
                        _trainingReferenceRecord = record;
                }
            }
        }

        [XmlIgnore]
        private WiimoteReferenceRecord _trainingReferenceRecord;

        [XmlIgnore]
        public IWiimoteReferenceRecord TrainingReferenceRecord
        {
            get
            {
                return _trainingReferenceRecord;
            }
            set
            {
                _trainingReferenceRecord = (WiimoteReferenceRecord)value;
            }
        }


        public WiimoteReferenceRecord getWiimoteReferenceRecordByID(int p_ID)
        {
            foreach (WiimoteReferenceRecord record in referenceRecordList)
            {
                if (record.RecordID == p_ID)
                    return record;
            }
            return null;
        }

        /*
         * This method is used to create WiimoteReferenceObjects not included in the global wiimote list
         * They do not get saved to the data store when the application exits
         */

        public int getNumberOfWiimoteReferenceRecords()
        {
            return referenceRecordList.Count;
        }


        public IWiimoteReferenceRecord getWiimoteReferenceRecord(int index)
        {
            return (IWiimoteReferenceRecord)referenceRecordList[index];
        }

        public void deleteWiimoteReferenceRecord(int index)
        {
            referenceRecordList.RemoveAt(index);
        }

        public void deleteAllWiimoteRefernceRecords()
        {
            referenceRecordList.Clear();
        }

        public override void deleteChildItem(int index)
        {
            deleteWiimoteReferenceRecord(index);
        }

        public IWiimoteReferenceRecord addWiimoteReferenceRecord(string p_ReferenceName,string p_VideoPath)
        {
            WiimoteReferenceRecord record = new WiimoteReferenceRecord(this, p_ReferenceName,
                p_VideoPath);
            referenceRecordList.Add(record);
            return (IWiimoteReferenceRecord)record;
        }

        public void moveWiimoteReferenceRecord(IWiimoteReferenceRecord pReferenceRecord, int pPosition)
        {
            WiimoteReferenceRecord lReferenceRecord = (WiimoteReferenceRecord)pReferenceRecord;
            int lIndex = referenceRecordList.IndexOf(lReferenceRecord);

            if (pPosition == ProjectConstants.MOVE_ROW_DOWN)
            {
                referenceRecordList.Remove(lReferenceRecord);
                referenceRecordList.Insert(lIndex + 1, lReferenceRecord);
            }
            else
            {
                if (lIndex > 0)
                {
                    referenceRecordList.Remove(lReferenceRecord);
                    referenceRecordList.Insert(lIndex - 1, lReferenceRecord);
                }
            }
        }

        public WiimoteReferenceRecordingItem findWiimoteReferenceRecordingItem(string pReferenceRecordingItemName)
        {
            //TODO : Brute force search. Needs to be improved
            foreach (WiimoteReferenceRecord lReferenceRecord in referenceRecordList)
            {
                foreach (WiimoteReferenceRecordingItem lRecordingItem in lReferenceRecord.ReferenceRecordingItems)
                {
                    if (lRecordingItem.RecordName.CompareTo(pReferenceRecordingItemName) == 0)
                        return lRecordingItem;
                }
            }

            return null;
        }

        [XmlIgnore]
        public bool SlowMoOption 
        {
            get
            {
                foreach (WiimoteReferenceRecord lReferenceRecord in referenceRecordList)
                {
                    if (lReferenceRecord.SlowMoOption)
                        return true;
                }

                return false;
            }
        }

        [XmlIgnore]
        public bool ScoringOption
        {
            get
            {
                if (TrainingReferenceRecord != null)
                    return true;
                else
                    return false;
            }
        }

        public IWiimoteReferenceRecord getReferenceRecord(int pIndex)
        {
            return referenceRecordList[pIndex];
        }

        public IWiimoteReferenceRecord getSlowMoReferenceRecord()
        {
            foreach (WiimoteReferenceRecord lReferenceRecord in referenceRecordList)
            {
                if (lReferenceRecord.SlowMoOption)
                    return (IWiimoteReferenceRecord)lReferenceRecord;
            }

            return null;
        }

        public IWiimoteReferenceRecord getMainReferenceRecord()
        {
            foreach (WiimoteReferenceRecord lReferenceRecord in referenceRecordList)
            {
                if (!lReferenceRecord.SlowMoOption && !lReferenceRecord.ScoringOption)
                    return (IWiimoteReferenceRecord)lReferenceRecord;
            }

            return null;
        }

        public IWiimoteReferenceRecord getScoringReferenceRecord()
        {
            return TrainingReferenceRecord;
        }

        #endregion


        #region PlayRecords

        [XmlIgnore]
        private List<WiimotePlayRecord> playRecordList = new List<WiimotePlayRecord>();

        [XmlAttribute("HighestPlayIndex")]
        public static int HighestPlayIndex { get; set; }

        [XmlElement("WiimotePlayRecord")]
        public WiimotePlayRecord[] WiimotePlayRecords
        {
            get
            {
                WiimotePlayRecord[] records = new WiimotePlayRecord[playRecordList.Count];
                playRecordList.CopyTo(records);
                return records;
            }
            set
            {
                if (value == null) return;
                WiimotePlayRecord[] records = (WiimotePlayRecord[])value;
                playRecordList.Clear();
                foreach (WiimotePlayRecord record in records)
                    playRecordList.Add(record);
            }
        }

        [XmlIgnore]
        private WiimotePlayRecord trainingPlayRecord { get; set; }

        [XmlIgnore]
        public IWiimotePlayRecord TrainingPlayRecord 
        {
            get
            {
                return (IWiimotePlayRecord)trainingPlayRecord;
            }
            set
            {
                trainingPlayRecord = (WiimotePlayRecord)value;
            }
        }

        public WiimotePlayRecord getWiimotePlayRecordByID(int p_ID)
        {
            foreach (WiimotePlayRecord record in playRecordList)
            {
                if (record.RecordID == p_ID)
                    return record;
            }
            return null;
        }


        public void deleteWiimotePlayRecord(int index)
        {
            playRecordList.RemoveAt(index);
        }

        public IWiimotePlayRecord addWiimotePlayRecord(string p_PlayName,
            DateTime p_RecordedTime, int p_Score)
        {
            WiimotePlayRecord record = new WiimotePlayRecord(this, p_PlayName, p_RecordedTime, p_Score);
            playRecordList.Add(record);
            TrainingPlayRecord = record;
            return (IWiimotePlayRecord)record;
        }

        /*
         * This method is used to create WiimoteReferenceObjects not included in the global wiimote list
         * They do not get saved to the data store when the application exits
         */

        public int getNumberOfWiimotePlayRecords()
        {
            return playRecordList.Count;
        }

        #endregion

        public override void linkChildToParent()
        {
            foreach (WiimoteReferenceRecord lReferenceRecord in referenceRecordList)
                lReferenceRecord.linkChildToParent();

        }


    }
    #endregion


    # region Class WiimoteReferenceRecord
    public class WiimoteReferenceRecord : WiimoteParentRecordBase, IWiimoteReferenceRecord, IChildRecord
    {

        [XmlIgnore]
        public double[] RefChartData { get; set; }

        [XmlIgnore]
        public IParentRecord ParentRecord { get; set; }

        [XmlElement("SelectedRecording")]
        public WiimoteReferenceRecordingItem SelectedRecordingItem { get; set; }

        [XmlIgnore]
        public TrainingSegmentInfo ParentTrainingSegmentInfo { get; set; }

        [XmlAttribute("SlowMoOption")]
        public bool SlowMoOption { get; set; }

        [XmlAttribute("ScoringOption")]
        public bool ScoringOption { get; set; }

        [XmlAttribute("RepeatOption")]
        public bool RepeatOption { get; set; }

        // Time after which to start actual wiimote logging.
        [XmlAttribute("WiimoteRecordingDelay")]
        public int WiimoteRecordingDelay { get; set; }
        [XmlAttribute("VideoPath")]
        public string VideoPath { get; set; }

        protected List<WiimoteReferenceRecordingItem> ReferenceRecordingItemList = new List<WiimoteReferenceRecordingItem>();

        [XmlElement("ReferenceRecordingItems")]
        public WiimoteReferenceRecordingItem[] ReferenceRecordingItems
        {
            get
            {
                WiimoteReferenceRecordingItem[] records = new WiimoteReferenceRecordingItem[ReferenceRecordingItemList.Count];
                ReferenceRecordingItemList.CopyTo(records);
                return records;
            }
            set
            {
                if (value == null) return;
                WiimoteReferenceRecordingItem[] records = (WiimoteReferenceRecordingItem[])value;
                ReferenceRecordingItemList.Clear();
                foreach (WiimoteReferenceRecordingItem record in records)
                {
                    record.ParentRecord = this;
                    ReferenceRecordingItemList.Add(record);
                }
            }
        }

        public WiimoteReferenceRecord()
        {
            WiimoteRecordingDelay = 0;
        }


        public WiimoteReferenceRecord(TrainingSegmentInfo pSegmentInfo, string p_ReferenceRecordName, string p_VideoPath)
        {
            ParentTrainingSegmentInfo = pSegmentInfo;
            RecordName = p_ReferenceRecordName;
            VideoPath = p_VideoPath;
            WiimoteRecordingDelay = 0;
            WiimoteDataStore wimoteReferenceDataStore = WiimoteDataStore.getWiimoteDataStore();
            RecordID = TrainingSegmentInfo.HighestReferenceIndex + 1;
            TrainingSegmentInfo.HighestReferenceIndex++;
            ParentRecord = pSegmentInfo;

        }


        public virtual WiimoteReferenceRecordingItem addWiimoteReferenceRecordingItem(string p_RecordingItemName, CalibrationOption p_Calibration,DateTime p_RecordedTime)
        {
            return addWiimoteReferenceRecordingItem(p_RecordingItemName, p_Calibration, p_RecordedTime, true);
        }

        public virtual WiimoteReferenceRecordingItem addWiimoteReferenceRecordingItem(string p_RecordingItemName, CalibrationOption p_Calibration, DateTime p_RecordedTime,bool pPersistence)
        {
            WiimoteReferenceRecordingItem record = new WiimoteReferenceRecordingItem(this, p_RecordingItemName, p_Calibration, p_RecordedTime, pPersistence);
            ReferenceRecordingItemList.Add(record);
            return record;
        }


        public WiimoteReferenceRecordingItem getWiimoteReferenceRecordingItem(int index)
        {
            return ReferenceRecordingItemList[index];
        }

        public int getNumberOfWiimoteReferenceRecordingItems()
        {
            return ReferenceRecordingItemList.Count;
        }

        public void deleleAllWiimoteReferenceRecordingItems()
        {
            ReferenceRecordingItemList.Clear();
        }

        public override void deleteChildItem(int index)
        {
            ReferenceRecordingItemList.RemoveAt(index);
        }

        public override void linkChildToParent()
        {
            foreach (WiimoteReferenceRecordingItem lReferenceRecordingItem in ReferenceRecordingItemList)
                lReferenceRecordingItem.ParentRecord = this;
            if (SelectedRecordingItem != null)
                SelectedRecordingItem.ParentRecord = this;
        }


        public string FilePath
        {
            get{
                if (SelectedRecordingItem != null)
                    return SelectedRecordingItem.FilePath;
                else
                    return null;

            }
        }

        public DateTime RecordedTime 
        {
            get
            {
                if (SelectedRecordingItem != null)
                    return SelectedRecordingItem.RecordedTime;
                else
                    return DateTime.Today;

            }
            set
            {
                if (SelectedRecordingItem != null)
                    SelectedRecordingItem.RecordedTime = value;
            }
        }

        public bool RecordingDone
        {
            get
            {
                if (SelectedRecordingItem != null)
                    return SelectedRecordingItem.RecordingDone;
                else
                    return false;

            }
            set
            {
                if (SelectedRecordingItem != null)
                    SelectedRecordingItem.RecordingDone = value;
            }
        }

        public bool isRecordingValid()
        {
            if (SelectedRecordingItem != null)
                return SelectedRecordingItem.isRecordingValid();
            else
                return false;

        }

        public string[] getNextDataRow(int rowSkipStep)
        {
            if (SelectedRecordingItem != null)
                return SelectedRecordingItem.getNextDataRow(rowSkipStep);
            else
                return null;

        }

    }

    #endregion


    # region Class WiimoteCalibrationRecord

    public class WiimoteCalibrationRecord : WiimoteReferenceRecord, IWiimoteCalibrationRecord
    {

        public WiimoteCalibrationRecord()
        {
        }

        public WiimoteCalibrationRecord(WiimoteCalibrationRecordInfo pWiimoteCalibrationRecordInfo, string pReferenceRecordName, DateTime p_RecordedTime, bool pPersistent)
            : base((TrainingSegmentInfo)pWiimoteCalibrationRecordInfo, pReferenceRecordName,"")
        {
            addWiimoteReferenceRecordingItem(pReferenceRecordName, CalibrationOption.System,p_RecordedTime);
            this.SelectedRecordingItem = ReferenceRecordingItemList[0];
        }


    }

    public class WiimoteCalibrationRecordInfo : TrainingSegmentInfo, IWiimoteCalibrationRecordInfo
    {
        private const string STAND_STILL_CALIBRATION_NAME = "StandStill";
        private const string LEFT_CALIBRATION_NAME = "Left";
        private const string RIGHT_CALIBRATION_NAME = "Right";

        [XmlElement("WiimoteStandStillRecord")]
        public WiimoteCalibrationRecord StandStill { get; set; }

        [XmlElement("WiimoteLeftRecord")]
        public WiimoteCalibrationRecord Left { get; set; }

        [XmlElement("WiimoteRightRecord")]
        public WiimoteCalibrationRecord Right { get; set; }

        Dictionary<int, double> m_CalibrationValues =  new Dictionary<int, double>();

        public bool WiimotesSwitched { get; set;  }

        public WiimoteCalibrationRecordInfo():base("CalibrationRecord")
        {
            this.RecordID = 0;
            WiimotesSwitched = false;

            m_CalibrationValues.Add(ProjectConstants.WIMMOTE1_DATA_ACCX_COLUMN_INDEX, 0);
            m_CalibrationValues.Add(ProjectConstants.WIMMOTE1_DATA_ACCY_COLUMN_INDEX, 0);
            m_CalibrationValues.Add(ProjectConstants.WIMMOTE1_DATA_ACCZ_COLUMN_INDEX, 0);
            m_CalibrationValues.Add(ProjectConstants.WIMMOTE1_DATA_YAW_COLUMN_INDEX, 0);
            m_CalibrationValues.Add(ProjectConstants.WIMMOTE1_DATA_PITCH_COLUMN_INDEX, 0);
            m_CalibrationValues.Add(ProjectConstants.WIMMOTE1_DATA_ROLL_COLUMN_INDEX, 0);

            m_CalibrationValues.Add(ProjectConstants.WIMMOTE2_DATA_ACCX_COLUMN_INDEX, 0);
            m_CalibrationValues.Add(ProjectConstants.WIMMOTE2_DATA_ACCY_COLUMN_INDEX, 0);
            m_CalibrationValues.Add(ProjectConstants.WIMMOTE2_DATA_ACCZ_COLUMN_INDEX, 0);
            m_CalibrationValues.Add(ProjectConstants.WIMMOTE2_DATA_YAW_COLUMN_INDEX, 0);
            m_CalibrationValues.Add(ProjectConstants.WIMMOTE2_DATA_PITCH_COLUMN_INDEX, 0);
            m_CalibrationValues.Add(ProjectConstants.WIMMOTE2_DATA_ROLL_COLUMN_INDEX, 0);
            
            //Initialize calibration objects for cases where they don't exists in the data store
            if (StandStill == null)
                StandStill = (WiimoteCalibrationRecord)getStandStillRecordingItem(DateTime.Now);

            if (Left == null)
                Left = (WiimoteCalibrationRecord)getLeftRecordingItem(DateTime.Now);

            if (Right == null)
                Right = (WiimoteCalibrationRecord)getRightRecordingItem(DateTime.Now);
        }


        public IWiimoteCalibrationRecord getStandStillRecordingItem(DateTime p_RecordedTime)
        {
            if (StandStill == null)
                StandStill = new WiimoteCalibrationRecord(this, STAND_STILL_CALIBRATION_NAME, p_RecordedTime, false);

            return (IWiimoteCalibrationRecord)StandStill;
        }

        public IWiimoteCalibrationRecord getLeftRecordingItem(DateTime p_RecordedTime)
        {
            if (Left == null)
                Left = new WiimoteCalibrationRecord(this, LEFT_CALIBRATION_NAME, p_RecordedTime, false);

            return (IWiimoteCalibrationRecord)Left;
        }

        public IWiimoteCalibrationRecord getRightRecordingItem(DateTime p_RecordedTime)
        {
            if (Right == null)
                Right = new WiimoteCalibrationRecord(this, RIGHT_CALIBRATION_NAME, p_RecordedTime, false);

            return (IWiimoteCalibrationRecord)Right;
        }

        public double getCalibrationValue(int p_Key)
        {
            return m_CalibrationValues[p_Key];
        }

        public void resetCalibration()
        {
            m_CalibrationValues[ProjectConstants.WIMMOTE1_DATA_ACCX_COLUMN_INDEX] = 0;
            m_CalibrationValues[ProjectConstants.WIMMOTE1_DATA_ACCY_COLUMN_INDEX] = 0;
            m_CalibrationValues[ProjectConstants.WIMMOTE1_DATA_ACCZ_COLUMN_INDEX] = 0;
            m_CalibrationValues[ProjectConstants.WIMMOTE1_DATA_YAW_COLUMN_INDEX] = 0;
            m_CalibrationValues[ProjectConstants.WIMMOTE1_DATA_PITCH_COLUMN_INDEX] = 0;
            m_CalibrationValues[ProjectConstants.WIMMOTE1_DATA_ROLL_COLUMN_INDEX] = 0;

            m_CalibrationValues[ProjectConstants.WIMMOTE2_DATA_ACCX_COLUMN_INDEX] = 0;
            m_CalibrationValues[ProjectConstants.WIMMOTE2_DATA_ACCY_COLUMN_INDEX] = 0;
            m_CalibrationValues[ProjectConstants.WIMMOTE2_DATA_ACCZ_COLUMN_INDEX] = 0;
            m_CalibrationValues[ProjectConstants.WIMMOTE2_DATA_YAW_COLUMN_INDEX] = 0;
            m_CalibrationValues[ProjectConstants.WIMMOTE2_DATA_PITCH_COLUMN_INDEX] = 0;
            m_CalibrationValues[ProjectConstants.WIMMOTE2_DATA_ROLL_COLUMN_INDEX] = 0;
        }

        public void calculateCalibration()
        {

            try
            {
                double leftStdDev;
                double rightStdDev;

                if (!StandStill.SelectedRecordingItem.isFilePresent())
                    throw new WiimoteDataStoreException("StandStill Calibration Data file not present");

                StandStill.SelectedRecordingItem.calculateAverage(m_CalibrationValues);

                if (!Left.SelectedRecordingItem.isFilePresent())
                    throw new WiimoteDataStoreException("Left Calibration Data file not present");

                Left.SelectedRecordingItem.calculateDataStdDeviation(out leftStdDev, out rightStdDev);

                if (leftStdDev > rightStdDev)
                    WiimotesSwitched = false;
                else
                    WiimotesSwitched = true;

            }
            catch (CSVFileException e)
            {
                throw new WiimoteDataStoreException(e);
            }
        }

        public override void linkChildToParent()
        {
            StandStill.ParentRecord = this;
            Left.ParentRecord = this;
            Right.ParentRecord = this;
        }
            
    }
    #endregion

    # region Class WiimoteReferenceRecordingItem
    public class WiimoteReferenceRecordingItem : WiimoteChildRecordBase, IWiimoteChildRecord
    {
        [XmlAttribute("Calibration")]
        public CalibrationOption CalibrationOption { get; set; }

        [XmlIgnore]
        public bool m_IsSelectedRecord;

        [XmlAttribute("IsSelectedRecord")]
        public bool IsSelectedRecord
        {
            get
            {
                return m_IsSelectedRecord;
            }
            set
            {
                m_IsSelectedRecord = value;
                if(ParentRecord != null)
                    ((WiimoteReferenceRecord)ParentRecord).SelectedRecordingItem = this;
            }   
        }

        public WiimoteReferenceRecordingItem()
        {
        }

        public WiimoteReferenceRecordingItem(WiimoteReferenceRecord p_ParentRecord,string p_RecordingItemName,
            CalibrationOption p_Calibration, DateTime p_RecordedTime, bool pPersistent)
        {
            RecordName = p_RecordingItemName;
            RecordedTime = p_RecordedTime;
            CalibrationOption = p_Calibration;
            IsSelectedRecord = false;

            ParentRecord = p_ParentRecord;
        }



        public WiimoteReferenceRecordingItem(WiimoteReferenceRecord p_ParentRecord, string p_RecordingItemName,
            DateTime p_RecordedTime, bool p_ForCalibration)
        {
            ParentRecord = p_ParentRecord;
            RecordedTime = p_RecordedTime;
            RecordName = p_RecordingItemName;
        }


        [XmlIgnore]
        public override string FilePath
        {
            get
            {
                return ProjectCommon.ProjectConstants.WIIMOTE_REFERENCE_DATA_PATH + "\\" + RecordName + ".csv";
            }
        }
    }

    #endregion
}

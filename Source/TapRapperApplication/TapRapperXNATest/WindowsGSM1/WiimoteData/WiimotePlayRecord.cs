using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Xml;
using System.Xml.Serialization;

using ProjectCommon;

namespace WiimoteData
{

    public class WiimotePlayRecord : WiimoteChildRecordBase, IWiimotePlayRecord
    {
        public WiimotePlayRecord()
        {
        }

        public WiimotePlayRecord(TrainingSegmentInfo pTrainingSegmentInfo, string p_RecordName,
            DateTime p_RecordedTime, int p_Score)
        {
            ParentRecord = pTrainingSegmentInfo;
//            ParentRecordID = p_ReferenceRecord.RecordID;
            RecordedTime = p_RecordedTime;
            Score = p_Score;
            RecordName = p_RecordName;

            TrainingSegmentInfo.HighestPlayIndex++;
        }



        /*
        protected override WiimoteParentRecordBase getParentRecordByID(int p_ID)
        {
            return (WiimoteParentRecordBase)WiimoteDataStore.getWiimoteDataStore().getWiimoteReferenceRecordByID(p_ID);
        }*/

        [XmlAttribute("Score")]
        public double Score { get; set; }

        [XmlAttribute("AverageDelay")]
        public double AverageDelay { get; set; }

        [XmlAttribute("Fluctuation")]
        public double Fluctuation { get; set; }

        [XmlAttribute("NumberOfStars")]
        public int NumberOfStars { get; set; }


        [XmlIgnore]
        public override string FilePath
        {
            get
            {
                return ProjectCommon.ProjectConstants.WIIMOTE_PLAY_DATA_PATH + "\\" + RecordName + ".csv";
            }
        }

    }
}


/*
    [XmlRoot("WiimotePlayDataStore")]
    public class WiimotePlayDataStore : IWiimoteRecordStore
    {
        private List<WiimotePlayRecord> playRecordList = new List<WiimotePlayRecord>();
        private static WiimotePlayDataStore m_WiimotePlayDataStore;

        [XmlAttribute("HighestIndex")]
        public int HighestIndex { get; set; }

        public WiimotePlayDataStore()
        {
        }

        public static WiimotePlayDataStore getWiimotePlayDataStore()
        {
            if (m_WiimotePlayDataStore == null)
            {
                m_WiimotePlayDataStore = new WiimotePlayDataStore();
                m_WiimotePlayDataStore.HighestIndex = 0;
            }

            return m_WiimotePlayDataStore;
        }

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

        public WiimotePlayRecord getWiimotePlayRecord(int index)
        {
            return playRecordList[index];
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

        public WiimotePlayRecord addWiimotePlayRecord(WiimoteReferenceRecord p_ReferenceRecord, string p_PlayName,string p_FilePath, 
            DateTime p_RecordedTime, int p_Score)
        {
            WiimotePlayRecord record = new WiimotePlayRecord(this,p_ReferenceRecord, p_PlayName, p_FilePath, p_RecordedTime, p_Score);
            playRecordList.Add(record);
            return record;
        }

        public int getNumberOfWiimotePlayRecords()
        {
            return playRecordList.Count;
        }

        public void updateReferenceRecord(WiimoteReferenceRecord p_ReferenceRecord)
        {
            foreach (WiimotePlayRecord playRecord in playRecordList)
            {
                IWiimoteParentRecord checkReferenceRecord = playRecord.ParentRecord;
                if (checkReferenceRecord.RecordID == p_ReferenceRecord.RecordID)
                    playRecord.ParentRecord = p_ReferenceRecord;
            }
        }

        public void save()
        {
            WiimotePlayDataStore.Serialize();
        }

        public static void Serialize()
        {
            // Serialization
            XmlSerializer s = new XmlSerializer(typeof(WiimotePlayDataStore));
            TextWriter w = new StreamWriter(ProjectConstants.WIIMOTE_PLAY_DATA_PATH + @"\" + ProjectConstants.WIIMOTE_PLAY_DATA_STORE);
            s.Serialize(w, WiimotePlayDataStore.m_WiimotePlayDataStore);
            w.Close();
        }

        public static WiimotePlayDataStore Deserialize()
        {
            // Deserialization
            XmlSerializer s = new XmlSerializer(typeof(WiimotePlayDataStore));
            WiimotePlayDataStore wiimoteDataStore;

            if (!File.Exists(ProjectConstants.WIIMOTE_PLAY_DATA_PATH + @"\" + ProjectConstants.WIIMOTE_PLAY_DATA_STORE))
            {
                wiimoteDataStore = WiimotePlayDataStore.getWiimotePlayDataStore();
                return wiimoteDataStore;
            }

            TextReader r = new StreamReader(ProjectConstants.WIIMOTE_PLAY_DATA_PATH + @"\" + ProjectConstants.WIIMOTE_PLAY_DATA_STORE);
            WiimotePlayDataStore.m_WiimotePlayDataStore = (WiimotePlayDataStore)s.Deserialize(r);

            WiimoteReferenceDataStore referenceDataStore = WiimoteReferenceDataStore.getWiimoteReferenceDataStore();

            int length = WiimotePlayDataStore.m_WiimotePlayDataStore.getNumberOfWiimotePlayRecords();
            for (int index = 0; index < length; index++)
            {
                WiimotePlayRecord playRecord = WiimotePlayDataStore.m_WiimotePlayDataStore.getWiimotePlayRecord(index);
                playRecord.DataStore = WiimotePlayDataStore.getWiimotePlayDataStore();
                int referenceRecordID = playRecord.ParentRecordID;
                playRecord.ParentRecord = referenceDataStore.getWiimoteReferenceRecordByID(referenceRecordID);
            }

            r.Close();
            return WiimotePlayDataStore.m_WiimotePlayDataStore;
        }
    }
*/
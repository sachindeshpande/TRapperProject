using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Utilities;
using ProjectCommon;

using System.Threading;
using WiimoteData.SpaceSensor;

namespace WiimoteData
{
    public class WiimoteRecordingEventArgs : EventArgs
    {
        public WiimoteRecordingEventArgs(TimerEventArgs e,IWiimoteChildRecord p_Record,object recordingInvoker)
        {
            m_TimerArgs = e;
            ChildRecord = p_Record;
            RecordingInvoker = recordingInvoker;
        }

        private TimerEventArgs m_TimerArgs;
        public IWiimoteChildRecord ChildRecord { get; set; }
        public object RecordingInvoker { get; set; }

        public int getNumBeatInBar()
        {
            return m_TimerArgs.m_NumBeatInBar;
        }

    }

    public class RecordingDataReceivedEventArgs : EventArgs
    {
        public RecordingDataReceivedEventArgs(SensorData sensor1DataPacket, SensorData sensor2DataPacket)
        {
            _sensor1DataPacket = sensor1DataPacket;
            _sensor2DataPacket = sensor2DataPacket;
        }

        private SensorData _sensor1DataPacket;
        public SensorData Sensor1DataPacket
        {
            get { return _sensor1DataPacket; }
            set { _sensor1DataPacket = value; }
        }

        private SensorData _sensor2DataPacket;
        public SensorData Sensor2DataPacket
        {
            get { return _sensor2DataPacket; }
            set { _sensor2DataPacket = value; }
        }

    }

    public class WiimoteUpdateEventArgs : EventArgs
    {
        public WiimoteUpdateEventArgs(IWiimote wiimote1, IWiimote wiimote2)
        {
            Wiimote1 = wiimote1;
            Wiimote2 = wiimote2;
        }

        public IWiimote Wiimote1 { get; set; }
        public IWiimote Wiimote2 { get; set; }

    }

    public interface IWiimote
    {
        int BatteryLevel { get; }

        WiimoteState WiimoteCurrentState { get; }
    }

    public class Wiimote : IWiimote
    {
        public Wiimote()
        {
            BatteryLevel = 0;
            WiimoteCurrentState = WiimoteState.wiimoteDisconnectedState;
        }

        public int BatteryLevel { get; set;  }

        public WiimoteState WiimoteCurrentState { get; set; }
    }

    public class Wiimotes
    {

        #region Definition
        private DataAdaptor m_WiimoteDataAdaptor;

        private bool m_WiimoteConnected;
        private bool m_ContinueConnectingAttempts;

        private object m_RecordingInvoker;
        private IWiimoteChildRecord m_SelectedRowForRecord;


        public delegate void OnRecordingStartedEvent(object sender, WiimoteRecordingEventArgs args);
        public delegate void OnRecordingBeatEvent(object sender, WiimoteRecordingEventArgs args);
        public delegate void OnRecordingDataReceivedEvent(object sender, RecordingDataReceivedEventArgs args);
        public delegate void OnRecordingCompletedEvent(object sender, WiimoteRecordingEventArgs args);
        public delegate void OnRecordingInterruptedEvent(object sender, WiimoteRecordingEventArgs args);

        public delegate void OnWiimoteUpdateEvent(object sender, WiimoteUpdateEventArgs args);
        public delegate void OnWiimoteDisconnectedEvent(object sender, WiimoteUpdateEventArgs args);


        public event OnRecordingStartedEvent RecordingStartedEvent;
        public event OnRecordingBeatEvent RecordingBeatEvent;
        public event OnRecordingDataReceivedEvent RecordingDataReceivedEvent;
        public event OnRecordingCompletedEvent RecordingCompletedEvent;
        public event OnRecordingInterruptedEvent RecordingInterruptedEvent;
        public event OnWiimoteUpdateEvent WiimoteUpdateEvent;
        public event OnWiimoteDisconnectedEvent WiimoteDisconnectedEvent;

        private Thread m_WiimoteStateThread;
        private bool m_WiimoteStateThreadRunState;

        private static Wiimotes mWiimotesSingleton;

        private Wiimote m_Wiimote1;
        private Wiimote m_Wiimote2;

        #endregion


        #region Initialize

        Wiimotes()
        {
            m_WiimoteConnected = false;
            m_WiimoteDataAdaptor = new SpaceSensorDataAdaptor(this);
            m_SelectedRowForRecord = null;
            m_ContinueConnectingAttempts = true;

            m_Wiimote1 = new Wiimote();
            m_Wiimote2 = new Wiimote();

            Initialize();
        }

        public void Initialize()
        {
            TimerThread.getTimerThread().BeatTimerEvent += new TimerThread.OnBeatTimerEvent(OnBeatTimerEvent);
            TimerThread.getTimerThread().StartingRecordingTimerEvent += new TimerThread.OnStartingRecordingTimerEvent(OnStartingRecordingTimerEvent);
            TimerThread.getTimerThread().TimerCompletedEvent += new TimerThread.OnTimerCompletedEvent(OnTimerCompletedEvent);
            TimerThread.getTimerThread().TimerInterruptedEvent += new TimerThread.OnTimerInterruptedEvent(OnTimerInterruptedEvent);

            MatlabWrapper.MatlabWiimoteWrapper.initialize();

            WiimoteDataStore.Deserialize();

            m_WiimoteDataAdaptor.initialize((WiimoteCalibrationRecordInfo)getCalibrationRecord());

            m_WiimoteStateThreadRunState = false;
        }

        #endregion

        #region Get/Set

        public static Wiimotes getWiimotesObject()
        {
            if (mWiimotesSingleton == null)
                mWiimotesSingleton = new Wiimotes();
            return mWiimotesSingleton;
        } 

        public IWiimoteCalibrationRecordInfo getCalibrationRecord()
        {
            return WiimoteDataStore.getWiimoteDataStore().Calibration;
        }

        public void setWiimoteDataMode()
        {
        }

        #endregion


        #region Wiimote Connection
        public void stopConnectingAttempts()
        {
            m_ContinueConnectingAttempts = false;
        }

        private void handleWiimoteConnected()
        {

            m_WiimoteConnected = true;
            startCheckingWiimoteState();

        }

        private void handleWiimoteConnectError(WiimoteState state)
        {
               disconnectWiimotes();
               stopCheckingWiimoteStatus();
        }

        public void connectWiimotes()
        {
            connectWiimotes(Configuration.getConfiguration().MaxWiimoteConnectionTries);
        }

        public void connectWiimotes(int p_MaxWiimoteConnectionTries)
        {
            m_ContinueConnectingAttempts = true;
            WiimoteState state = WiimoteState.wiimoteDisconnectedState;

            try
            {

                m_WiimoteDataAdaptor.connectWiimote();

                for (int checkCounter = 0; checkCounter < p_MaxWiimoteConnectionTries; checkCounter++)
                {

                    if (!m_ContinueConnectingAttempts)
                        throw new WiimoteConnectionException("Request to discontinue connection attempts", state);


                    m_WiimoteDataAdaptor.checkWiimoteState(m_Wiimote1, m_Wiimote1);
                    Console.WriteLine("In Reference Handler : state = " + state);
                    if (m_Wiimote1.WiimoteCurrentState == WiimoteState.wiimoteGoodStateState)
                    {
                        handleWiimoteConnected();

                        //Perform a dummy recording.
                        //This is temporary code to resolve the bug where the first recording is always incorrect

                        //startRecording(WiimoteDataStore.getWiimoteDataStore().DummyRecord, false, this, "", false);
                        //Thread.Sleep(2000);
                        //stopRecording();
                        return;
                    }
                }

                handleWiimoteConnectError(state);
                throw new WiimoteConnectionException("Wiimote disconnected",state);
            }
            catch (WiimoteCommunicationException ex)
            {
                handleWiimoteConnectError(state);
                throw new WiimoteConnectionException(ex,WiimoteState.wiimoteDisconnectedState);
            }
        }

        public void disconnectWiimotes()
        {
            try
            {
                if (m_Wiimote2.WiimoteCurrentState != WiimoteState.wiimoteDisconnectedState)
                    m_WiimoteDataAdaptor.disconnectWiimote();

                m_WiimoteConnected = false;
            }
            catch (WiimoteCommunicationException ex)
            {
                throw new WiimoteConnectionException(ex, WiimoteState.wiimoteDisconnectedState);
            }

        }

        #endregion


        #region Recording

        public void startRecording(Object recordObject,bool p_MP3Option,object recordingInvoker,string introductoryMessage,bool pUseTImer)
        {
            try
            {

                if (!m_WiimoteConnected)
                {
                    throw new WiimoteConnectionException(ProjectConstants.WIIMOTE_NOT_CONNECTED_MESSAGE, WiimoteState.wiimoteDisconnectedState);
                }

                
                IWiimoteChildRecord childRecord = (IWiimoteChildRecord)recordObject;
                ITrainingSegmentInfo parentRecord = (ITrainingSegmentInfo)childRecord.ParentRecord;

                m_RecordingInvoker = recordingInvoker;

                childRecord.RecordedTime = DateTime.UtcNow;

                //  ApplicationSpeech.speakText(ProjectConstants.RECORDING_STARTED_TEXT);

                m_WiimoteDataAdaptor.startWiimoteLogging(childRecord);

                
                if (pUseTImer)
                    startTimer(parentRecord, p_MP3Option, introductoryMessage);
//                else
//                    m_WiimoteDataAdaptor.startWiimoteRecording(); //If no TImer is used , no need to wait
                // for lead bar to complete to start recording.
                //Can start recording immediately
                

                m_SelectedRowForRecord = childRecord;

            }
            catch (WiimoteCommunicationException ex)
            {
                throw new WiimoteConnectionException(ex, WiimoteState.wiimoteDisconnectedState);
            }

        }

        public void stopRecording()
        {
            m_WiimoteDataAdaptor.stopWiimoteLogging();

            if (m_SelectedRowForRecord != null)
            {
                m_SelectedRowForRecord.RecordingDone = true;
                save();
            }
        }

        public void forceStopRecording()
        {
            TimerThread.getTimerThread().stopRecording();
        }

        public void StartDataCollection(string csvFilepath, bool csvFileCreate = true)
        {
            m_WiimoteDataAdaptor.StartDataCollection(csvFilepath, csvFileCreate);
        }

        public void StopDataCollection()
        {
            m_WiimoteDataAdaptor.StopDataCollection();
        }


        public string comparePlayToReference(IWiimoteReferenceRecord l_ReferenceRecord, IWiimotePlayRecord l_PlayRecord)
        {
            WiimoteReferenceRecordingItem l_ReferenceRecordingItem = l_ReferenceRecord.SelectedRecordingItem;

            if (l_ReferenceRecordingItem == null)
            {
                l_PlayRecord.Score = 1;
                l_PlayRecord.NumberOfStars = (int)1;
                return "";
//                throw new WiimoteRecordingException("No Default Recording is selected from the Reference List");
            }

            double score, stars;

            string message = m_WiimoteDataAdaptor.getScore((IWiimoteChildRecord)l_ReferenceRecordingItem, (IWiimoteChildRecord)l_PlayRecord, out score, out stars);

            l_PlayRecord.Score = score;
            l_PlayRecord.NumberOfStars = (int)stars;

            return message;

        }

        public string comparePlayToReference(IWiimotePlayRecord p_PlayRecord)
        {
            ITrainingSegmentInfo lTrainingSegmentInfo = (ITrainingSegmentInfo)((IWiimoteChildRecord)p_PlayRecord).ParentRecord;

            return comparePlayToReference((IWiimoteReferenceRecord)lTrainingSegmentInfo.TrainingReferenceRecord, p_PlayRecord);
        }


        public void setTimerBeatMP3(string mp3File)
        {
            TimerThread.getTimerThread().setTimerBeatMP3(mp3File);
        }

        public void startTimer(ITrainingSegmentInfo wiimoteRecord, bool p_MP3Option,string introductoryMessage)
        {
            TimerThread.getTimerThread().startTimer(Configuration.getConfiguration().NumBeatsPerMinute, Configuration.getConfiguration().NumBeatsPerBar,
                Configuration.getConfiguration().NumBars, Configuration.getConfiguration().LeadIn, p_MP3Option,introductoryMessage);
//            TimerThread.getTimerThread().startTimer(wiimoteRecord.NumBeatsPerMinute, wiimoteRecord.NumBeatsPerBar,
//                wiimoteRecord.NumBars, wiimoteRecord.LeadIn, p_MP3Option, introductoryMessage);
        }

        public void OnBeatTimerEvent(object sender, TimerEventArgs e)
        {
            RecordingBeatEvent(this, new WiimoteRecordingEventArgs(e,m_SelectedRowForRecord,m_RecordingInvoker));
        }

        public void ReceivedEventRecordingData(SensorData sensor1Data, SensorData sensor2Data)
        {
            if(RecordingDataReceivedEvent != null)
                RecordingDataReceivedEvent(this, new RecordingDataReceivedEventArgs(sensor1Data, sensor2Data));
        }

        /**
         * //TODO : The Timer is not needed anymore. Need to remove
         * **/
        public void OnStartingRecordingTimerEvent(object sender, TimerEventArgs e)
        {
            RecordingStartedEvent(this, new WiimoteRecordingEventArgs(e, m_SelectedRowForRecord, m_RecordingInvoker));
//            m_WiimoteDataAdaptor.startWiimoteRecording();
        }

        private void handleTimerCompletedEvent(object sender, TimerEventArgs e)
        {
            stopRecording();
        }

        public void OnTimerCompletedEvent(object sender, TimerEventArgs e)
        {
            handleTimerCompletedEvent(sender, e);
            if (m_SelectedRowForRecord != null)
            {
                RecordingCompletedEvent(this, new WiimoteRecordingEventArgs(e, m_SelectedRowForRecord, m_RecordingInvoker));
                m_SelectedRowForRecord = null;
            }
        }

        public void OnTimerInterruptedEvent(object sender, TimerEventArgs e)
        {
            handleTimerCompletedEvent(sender, e);
            if (m_SelectedRowForRecord != null)
            {
                RecordingInterruptedEvent(this, new WiimoteRecordingEventArgs(e, m_SelectedRowForRecord, m_RecordingInvoker));
                m_SelectedRowForRecord = null;
            }
        }

        #endregion


        #region Wiimote state Checker


        public void startCheckingWiimoteState()
        {
            m_WiimoteStateThread = new Thread(new ThreadStart(listenWiimoteState));
            m_WiimoteStateThread.Start();
            m_WiimoteStateThreadRunState = true;
        }


        public void listenWiimoteState()
        {
            while (m_WiimoteStateThreadRunState)
            {
                try
                {
                    m_WiimoteDataAdaptor.checkWiimoteState(m_Wiimote1, m_Wiimote2);

                    if (m_Wiimote1.WiimoteCurrentState != WiimoteState.wiimoteGoodStateState
                    || m_Wiimote2.WiimoteCurrentState != WiimoteState.wiimoteGoodStateState)
                    {
                        if (WiimoteDisconnectedEvent != null)
                            WiimoteDisconnectedEvent(this, new WiimoteUpdateEventArgs(m_Wiimote1, m_Wiimote2));
                    }
                        

                    if(WiimoteUpdateEvent != null)
                        WiimoteUpdateEvent(this, new WiimoteUpdateEventArgs(m_Wiimote1, m_Wiimote2));
                }
                catch (WiimoteCommunicationException ex)
                {
                    Console.WriteLine(ex);
                    throw ex;
                }
            }
        }

        public bool areWiimotesConnected()
        {
            if (m_Wiimote1.WiimoteCurrentState == WiimoteState.wiimoteGoodStateState &&
                m_Wiimote1.WiimoteCurrentState == WiimoteState.wiimoteGoodStateState)
                return true;
            else
                return false;
        }

        public void stopCheckingWiimoteStatus()
        {
            m_WiimoteStateThreadRunState = false;
        }

        #endregion



        public void save()
        {
            WiimoteDataStore.getWiimoteDataStore().save();
        }

        public void addInformationToRecording(string[] pRowData)
        {
            m_WiimoteDataAdaptor.addInformationToRecording(pRowData);
        }

        public void cleanup()
        {
            m_WiimoteDataAdaptor.cleanup();
        }

    }



}

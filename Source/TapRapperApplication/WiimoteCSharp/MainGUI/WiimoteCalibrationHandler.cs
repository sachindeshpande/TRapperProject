using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using ProjectCommon;
using WiimoteData;

using System.Windows.Forms;

namespace MainGUI
{
    enum CalibrationStatus
    {
        CalibrationNotStarted = 0,
        CalibrationStarted = 1,
        StandStillCalibrationDone = 2,
        LeftCalibrationDone = 3,
        RightCalibrationDone = 4,
        CalibrationDone = 5,

    }

    internal class WiimoteCalibrationHandler
    {

        private Wiimotes m_Wiimotes;
        private IWiimoteCalibrationRecordInfo m_CalibrationRecord;

        private CalibrationStatus m_Status;

        private object calibrationRecordingSync;

        private Thread mCalibrationThread;
        private bool mCalibrationRunning;

        private Form1 m_parent;

        delegate void setMessageCallback(string text);
        delegate void initializeCalibrationGUIButtonsCallback();
        delegate void calibrationInProgressGUICallback();

        public WiimoteCalibrationHandler(Wiimotes p_Wiimotes,Form1 p_Form)
        {

            m_Status = CalibrationStatus.CalibrationNotStarted;
            m_Wiimotes = p_Wiimotes;
            m_parent = p_Form;
            m_CalibrationRecord = m_Wiimotes.getCalibrationRecord();

            m_Wiimotes.RecordingCompletedEvent += new Wiimotes.OnRecordingCompletedEvent(OnRecordingCompletedEvent);
            m_Wiimotes.RecordingInterruptedEvent += new Wiimotes.OnRecordingInterruptedEvent(OnRecordingInterruptedEvent);

            calibrationRecordingSync = new object();
        }

        public void Initialize()
        {
            initializeCalibrationGUIButtons();
            m_parent.calibrationRecordingStatus.Text = getInitialStatusMessage();
        }

        #region GUI Handlers

        internal void initializeCalibrationGUIButtons()
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (m_parent.calibrationRecordingStatus.InvokeRequired ||
                m_parent.startCalibration.InvokeRequired ||
                m_parent.RecordStopButton.InvokeRequired ||
                m_parent.resetCalibration.InvokeRequired)
            {
                initializeCalibrationGUIButtonsCallback d = new initializeCalibrationGUIButtonsCallback(initializeCalibrationGUIButtons);
                m_parent.calibrationRecordingStatus.Invoke(d, new object[] { });
            }
            else
            {
                m_parent.startCalibration.Enabled = true;
                m_parent.RecordStopButton.Enabled = false;
                m_parent.resetCalibration.Enabled = true;
            }
        }

        internal void calibrationInProgressGUI()
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (m_parent.startCalibration.InvokeRequired ||
                m_parent.RecordStopButton.InvokeRequired ||
                m_parent.resetCalibration.InvokeRequired)
            {
                calibrationInProgressGUICallback d = new calibrationInProgressGUICallback(calibrationInProgressGUI);
                m_parent.startCalibration.Invoke(d, new object[] { });
            }
            else
            {
                m_parent.startCalibration.Enabled = false;
                m_parent.RecordStopButton.Enabled = true;
                m_parent.resetCalibration.Enabled = false;
            }

        }

        internal void setMessage(string pMessage)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (m_parent.calibrationRecordingStatus.InvokeRequired)
            {
                setMessageCallback d = new setMessageCallback(setMessage);
                m_parent.calibrationRecordingStatus.Invoke(d, new object[] { pMessage });
            }
            else
            {
                m_parent.calibrationRecordingStatus.Text = pMessage;
                ApplicationSpeech.speakText(pMessage);
            }
        }

        internal void setCalibrationNumBeatsPerBars(string pNumBeatsPerBarString)
        {
            try
            {
                int lNumBeatsPerBar = int.Parse(pNumBeatsPerBarString);
                m_CalibrationRecord.NumBeatsPerBar = lNumBeatsPerBar;
            }
            catch (Exception)
            {             
                MessageBox.Show("Invalid Number","Calibration NumBars should be a number", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            }
        }

        #endregion

        private void recordCalibrationItem(IWiimoteCalibrationRecord l_Record)
        {
            try
            {
                m_Wiimotes.startRecording(l_Record, false, this, "", true);

                lock (calibrationRecordingSync) Monitor.Wait(calibrationRecordingSync);

            }
            catch (WiimoteCommunicationException ex)
            {
                throw ex;
            }
            catch (WiimoteConnectionException ex)
            {
                throw ex;
            }
        }

        public void startCalibrationThreaded()
        {
            mCalibrationThread = new Thread(new ThreadStart(startCalibration));
            mCalibrationThread.Start();
        }


        public void startCalibration()
        {
            if (mCalibrationRunning)
                return;

            mCalibrationRunning = true;
            calibrationInProgressGUI();
            m_CalibrationRecord.resetCalibration();
            processCalibration();
            initializeCalibrationGUIButtons();
            mCalibrationRunning = false;
        }

        public void processCalibration()
        {
            try
            {
                setMessage(ProjectConstants.CALIBRATION_STARTED_MESSAGE);
                Thread.Sleep(ProjectConstants.CALIBRATION_MESSAGE_LOOK_TIME);
                IWiimoteCalibrationRecord l_StandStill = m_CalibrationRecord.getStandStillRecordingItem(DateTime.UtcNow);
                if (!mCalibrationRunning) return;
                setMessage(ProjectConstants.STAND_STILL_IN_PROGRESS_MESSAGE);
                recordCalibrationItem(l_StandStill);
                if (!mCalibrationRunning) return;
                setMessage(ProjectConstants.STAND_STILL_CALIBRATION_DONE_MESSAGE);
                Thread.Sleep(ProjectConstants.CALIBRATION_MESSAGE_LOOK_TIME);

                IWiimoteCalibrationRecord l_Left = m_CalibrationRecord.getLeftRecordingItem(DateTime.UtcNow);
                if (!mCalibrationRunning) return;
                setMessage(ProjectConstants.LEFT_CALIBRATION_IN_PROGRESS_MESSAGE);
                recordCalibrationItem(l_Left);
                if (!mCalibrationRunning) return;
                setMessage(ProjectConstants.LEFT_CALIBRATION_DONE_MESSAGE);
                Thread.Sleep(ProjectConstants.CALIBRATION_MESSAGE_LOOK_TIME);

                IWiimoteCalibrationRecord l_Right = m_CalibrationRecord.getRightRecordingItem(DateTime.UtcNow);
                if (!mCalibrationRunning) return;
                setMessage(ProjectConstants.RIGHT_CALIBRATION_IN_PROGRESS_MESSAGE);
                recordCalibrationItem(l_Right);
                m_CalibrationRecord.calculateCalibration();
                WiimoteDataStore.getWiimoteDataStore().save();
                if (!mCalibrationRunning) return;
                setMessage(ProjectConstants.CALIBRATION_DONE_MESSAGE);
            }
            catch (WiimoteCommunicationException ex)
            {
                throw ex;
            }
            catch (WiimoteConnectionException ex)
            {
                throw ex;
            }

            return;
        }



        public void OnRecordingCompletedEvent(object sender, WiimoteRecordingEventArgs e)
        {
            if (e.RecordingInvoker.Equals(this))
                lock (calibrationRecordingSync) Monitor.Pulse(calibrationRecordingSync);

        }

        public void OnRecordingInterruptedEvent(object sender, WiimoteRecordingEventArgs e)
        {
            if (e.RecordingInvoker.Equals(this))
                lock (calibrationRecordingSync) Monitor.Pulse(calibrationRecordingSync);

            MessageBox.Show("Recordng Stopped","Recording Successfully Stopped", MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        public void stopCalibration()
        {
            mCalibrationRunning = false;
            initializeCalibrationGUIButtons();
            m_parent.calibrationRecordingStatus.Text = getInitialStatusMessage();
            MessageBox.Show("Calibration has been Stopped", "Calibration Stopped", MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        public void resetCalibration()
        {
            m_CalibrationRecord.resetCalibration();
            initializeCalibrationGUIButtons();
            m_parent.calibrationRecordingStatus.Text = getInitialStatusMessage();
            MessageBox.Show("Calibration has been reset", "Reset Completed", MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        public string getInitialStatusMessage()
        {
            return ProjectConstants.CALIBRATION_NOT_STARTED_MESSAGE;
        }

        public bool isCalibrationRunning()
        {
            return mCalibrationRunning;
        }

    }
}


/*
        public void OnVoiceCommandReceivedEvent(Object sender, VoiceCommandEventArgs args)
        {
            Utilities.ConsoleLogger.logMessage("In OnVoiceCommandReceivedEvent : Command is " + args.VoiceCommandValue);

            try
            {
                if (args.VoiceCommandValue.CompareTo(VoiceCommandEventArgs.CALIBRATE_COMMAND) == 0)
                {
                    if (m_parent.isTrainingVideoRunning())
                        return;

                    if (!m_parent.areWiimotesConnected())
                    {
                        m_parent.focusSetupTab();
                        m_parent.connectWiimotes();
                    }
                    m_parent.focusCalibrationTab();
                    startCalibrationThreaded();
                }
                else if (args.VoiceCommandValue.CompareTo(VoiceCommandEventArgs.STOP_COMMAND) == 0)
                {
                    if (mCalibrationRunning)
                        stopCalibration();
                }
                else if (args.VoiceCommandValue.CompareTo(VoiceCommandEventArgs.TRAINING_COMMAND) == 0)
                {
                    m_parent.focusTrainingTab();
                }

            }
            catch (WiimoteConnectionException ex)
            {

            }
        }

*/
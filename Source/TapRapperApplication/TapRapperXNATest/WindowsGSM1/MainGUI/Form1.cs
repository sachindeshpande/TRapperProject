using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using WiimoteData;
using ProjectCommon;
using VoiceCommandControl;
using Utilities;

using Mogre;
using Server;

//using Microsoft.DirectX.AudioVideoPlayback;

namespace MainGUI

{
    public partial class Form1 : Form
    {

        private System.Windows.Forms.OpenFileDialog openVideoDialog;
        private System.Windows.Forms.OpenFileDialog openWiimoteDataDialog;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.OpenFileDialog openMP3FileDialog;
        private System.Windows.Forms.OpenFileDialog openWiimoteSimulationFileDialog;

       
        private WiimoteRecordingHandler m_WiimoteRecordingHandler;
        private WiimoteCalibrationHandler m_WiimoteCalibrationHandler;
        private TrainingHandler mTrainingHandler;
        private SpeechRecognition mSpeechRecognition;

//        private Video _video = null;

        internal enum WiimoteButtonState
        {
            CONNECTED = 0,
            DISCONNECTED = 1,
        }


        // This delegate enables asynchronous calls for setting
        // the text property on a TextBox control.
        delegate void SetTextBeatLogCallback(string text);
        delegate void SetTextWiimoteStatusCallback(string text);
        delegate void SetWiimoteButtonStateCallback(object buttonState);
        delegate void SetTextRecordingStatusCallback(string text);
        delegate void SetTextBeatCounterCallback(string text);
        delegate void SetTextFeedbackCallback(string text);
        delegate void SetTestTimerButtonStateCallback(bool buttonState);
        delegate void SetReferenceDataBindingSourceCallback(object[] dataStore);
        delegate void SetTrainingSegmentInfoBindingSourceCallback(object[] dataStore);
        delegate void SetReferenceRecordingItemDataBindingSourceCallback(object[] dataStore);
        delegate void SetPlayDataBindingSourceCallback(object[] dataStore);
        delegate void SetChartDataBindingSourceCallback(object[] dataStore);
        delegate void SetRecordingButtonStateCallback(bool buttonState);
        delegate void SetTrainingMessageCallback(string text);
        delegate void SetMatlabReturnCallback(string text);        
        delegate void AppendTextCallback(string text);
        delegate void SetBatteryLevelsCallback(string batteryLevel1,string batteryLevel2);
        
        public Form1()
        {
            cleanup();

            InitializeComponent();

            string workingDirectory = MainGUI.Properties.Settings.Default.WorkingDirectory;
            string wiimoteApplicationPath = MainGUI.Properties.Settings.Default.WiimoteApplicationPath;
            string matlabApplicationPath = MainGUI.Properties.Settings.Default.MatlapApplicationPath;
            string mediaDirectory = MainGUI.Properties.Settings.Default.MediaDirectory;

            ProjectConstants.initialize(workingDirectory, wiimoteApplicationPath, matlabApplicationPath, mediaDirectory);
            Environment.CurrentDirectory = ProjectConstants.PROJECT_PATH;
            this.openVideoDialog = new System.Windows.Forms.OpenFileDialog();
            this.openMP3FileDialog = new System.Windows.Forms.OpenFileDialog();
            this.openWiimoteSimulationFileDialog = new System.Windows.Forms.OpenFileDialog();
            // openVideoDialog
            // 
            this.openVideoDialog.DefaultExt = "avi";
            this.openVideoDialog.FileName = "kangaroo.avi";
            this.openVideoDialog.Filter = "movie files|*.avi||";
            this.openVideoDialog.Title = "Choose a movie to play";
            

            // openMP3Dialog
            // 
            this.openMP3FileDialog.DefaultExt = "mpw";
            this.openMP3FileDialog.Filter = "mp3 files|*.mp3||";
            this.openMP3FileDialog.Title = "Choose a mp3 file to play";

            // openWiimoteSimulationDialog
            // 
            this.openWiimoteSimulationFileDialog.DefaultExt = "csv";
            this.openWiimoteSimulationFileDialog.Filter = "csv files|*.csv||";
            this.openWiimoteSimulationFileDialog.Title = "Choose a wiimote data file for simulation";


            //open Wiimote Dialog
            this.openWiimoteDataDialog = new System.Windows.Forms.OpenFileDialog();
            this.openWiimoteDataDialog.DefaultExt = "csv";
            this.openWiimoteDataDialog.Filter = "csv files|*.csv||";
            this.openWiimoteDataDialog.Title = "Choose Wiimote Data File to upload";


            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.saveFileDialog.DefaultExt = "csv";
            this.saveFileDialog.Filter = "csv files|*.csv||";
            saveFileDialog.InitialDirectory = ProjectConstants.WIIMOTE_REFERENCE_DATA_PATH;

            this.wiimoteDisconnect.Enabled = false;

            Configuration config = Configuration.getConfiguration();
            config.initialize();
            this.configurationBindingSource.DataSource = config;

            WiimoteDataStore.getWiimoteDataStore().initialize();

            Wiimotes l_Wiimotes = Wiimotes.getWiimotesObject();

            m_WiimoteRecordingHandler = new WiimoteRecordingHandler(l_Wiimotes,this);
            m_WiimoteRecordingHandler.Initialize();

            m_WiimoteCalibrationHandler = new WiimoteCalibrationHandler(l_Wiimotes, this);
            m_WiimoteCalibrationHandler.Initialize();

            this.trainingSegmentInfoBindingSource.DataSource = WiimoteDataStore.getWiimoteDataStore().TrainingSegmentInfoRecords;

            InitializeBluetoothSetupTab();

            initializeTraining(l_Wiimotes);

            applicationStartSpeech();

            InitializeMogreComponent();


            mSpeechRecognition = SpeechRecognition.getSpeechRecognition();
            mSpeechRecognition.VoiceCommandReceivedEvent += new SpeechRecognition.OnVoiceCommandReceivedEvent(OnVoiceCommandReceivedEvent);
            setButtonVoiceCommandOff(false);

            this.quartenionViewerControl.SetWiimotes(l_Wiimotes);

            //OSCCommunication oscCommunication = new OSCCommunication();
            //oscCommunication.Initialize(l_Wiimotes);
/*
            try
            {
                m_OgreControl = new OgreControl(this.ogrePanel.Handle);
                m_OgreControl.Init();

            }
            catch (MogreWrapperException e)
            {
                MessageBox.Show(e.Message,
                         "An Ogre exception has occurred!");
            }
 
            Disposed += new EventHandler(Form1_Disposed);
            Resize += new EventHandler(Form1_Resize);
            Paint += new PaintEventHandler(Form1_Paint);
 */
        }
/*
        #region Form Event Handlers

        void Form1_Resize(object sender, EventArgs e)
        {
            m_OgreControl.resize();
        }

        void Form1_Disposed(object sender, EventArgs e)
        {
            m_OgreControl.dispose();
        }

        void Form1_Paint(object sender, EventArgs e)
        {
            m_OgreControl.paint();
        }

        #endregion
        */

        private void cleanup()
        {
            ProcessHelper.cleanupProcesses(ProjectConstants.BLUETOOTH_PROCESS_NAME);
        }

        private void InitializeBluetoothSetupTab()
        {
//            this.BluetoothSetupApp.ExeName = ProjectConstants.BLUETOOTH_SETUP_APP;
//            this.BluetoothSetupApp.Name = "BluetoothSetupApp";
        }

        private void InitializeTrainingVideoLayout()
        {
            this.ControlBox = false;
            this.Text = string.Empty;
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;

            TabControl.TabPageCollection lTabs = this.TabControl.TabPages;


            foreach (TabPage lTab in lTabs)
            {
                if (lTab.Name.CompareTo(ProjectConstants.TRAINING_VIDEO_PANEL_NAME) != 0 &&
                    lTab.Name.CompareTo(ProjectConstants.SETUP_PANEL_NAME) != 0 &&
                    lTab.Name.CompareTo(ProjectConstants.CALIBRATION_PANEL_NAME) != 0)
                    lTabs.Remove(lTab);
            }

            this.RecordingStatus.Visible = false;
            this.scoreFeedbackText.Visible = false;
            this.label30.Visible = false;

            this.startCalibration.Visible = false;
            this.RecordStopButton.Visible = false;
            this.resetCalibration.Visible = false;
        }

        #region GUI Field Setters

        internal void AppendText(string text)
        {
            if (this.BeatLog.InvokeRequired)
            {
                AppendTextCallback d = new AppendTextCallback(this.BeatLog.AppendText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.BeatLog.AppendText(text);
            }
        }



        internal void SetTextRecordingStatus(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.BeatLog.InvokeRequired)
            {
                SetTextRecordingStatusCallback d = new SetTextRecordingStatusCallback(SetTextRecordingStatus);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.RecordingStatus.Text = text;
            }
        }

        internal void SetTextBeatLog(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.BeatLog.InvokeRequired)
            {
                SetTextBeatLogCallback d = new SetTextBeatLogCallback(SetTextBeatLog);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.BeatLog.Text = text;
            }
        }

        internal void SetTextBeatCounter(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.BeatLog.InvokeRequired)
            {
                SetTextBeatCounterCallback d = new SetTextBeatCounterCallback(SetTextBeatCounter);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.beatCount.Text = text;
            }
        }

        internal void SetTextFeedback(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.BeatLog.InvokeRequired)
            {
                SetTextFeedbackCallback d = new SetTextFeedbackCallback(SetTextFeedback);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.scoreFeedbackText.Text = text;
            }
        }

        internal void SetTextWiimoteStatus(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.WiimoteStatus.InvokeRequired)
            {
                SetTextWiimoteStatusCallback d = new SetTextWiimoteStatusCallback(SetTextWiimoteStatus);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.WiimoteStatus.Text = text;
            }
        }

        internal void SetWiimoteButtonState(object p_buttonState)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.WiimoteStatus.InvokeRequired)
            {
                SetWiimoteButtonStateCallback d = new SetWiimoteButtonStateCallback(SetWiimoteButtonState);
                this.Invoke(d, new object[] { p_buttonState });
            }
            else
            {
                WiimoteButtonState buttonState = (WiimoteButtonState)p_buttonState;
                if (buttonState == WiimoteButtonState.CONNECTED)
                {
                    this.wiimoteConnect.Enabled = false;
                    this.wiimoteDisconnect.Enabled = true;
                }
                else
                {
                    this.wiimoteConnect.Enabled = true;
                    this.wiimoteDisconnect.Enabled = false;
                }
            }
        }

        internal void SetTestTimerButtonState(bool buttonState)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.WiimoteStatus.InvokeRequired)
            {
                SetTestTimerButtonStateCallback d = new SetTestTimerButtonStateCallback(SetTestTimerButtonState);
                this.Invoke(d, new object[] { buttonState });
            }
            else
            {
                this.TestTimer.Enabled = buttonState;
            }
        }

        internal void SetTrainingSegmentInfoBindingSource(object[] dataStore)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.WiimoteStatus.InvokeRequired)
            {
                SetTrainingSegmentInfoBindingSourceCallback d = new SetTrainingSegmentInfoBindingSourceCallback(SetTrainingSegmentInfoBindingSource);
                this.Invoke(d, new object[] { dataStore });
            }
            else
            {
                this.trainingSegmentInfoBindingSource.DataSource = (TrainingSegmentInfo[])dataStore;
            }
        }

        internal void SetReferenceDataBindingSource(object[] dataStore)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.WiimoteStatus.InvokeRequired)
            {
                SetReferenceDataBindingSourceCallback d = new SetReferenceDataBindingSourceCallback(SetReferenceDataBindingSource);
                this.Invoke(d, new object[] { dataStore });
            }
            else
            {
                this.wiimoteReferenceDataStoreBindingSource.DataSource = (WiimoteReferenceRecord[])dataStore;
            }
        }

        internal void SetReferenceRecordingItemDataBindingSource(object[] dataStore)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.WiimoteStatus.InvokeRequired)
            {
                SetReferenceRecordingItemDataBindingSourceCallback d = new SetReferenceRecordingItemDataBindingSourceCallback(SetReferenceRecordingItemDataBindingSource);
                this.Invoke(d, new object[] { dataStore });
            }
            else
            {
                this.wiimoteReferenceRecordBindingSource.DataSource = (WiimoteReferenceRecordingItem [])dataStore;
            }
        }


        internal void SetChartDataBindingSource(object referenceRecord)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.WiimoteStatus.InvokeRequired)
            {
                SetChartDataBindingSourceCallback d = new SetChartDataBindingSourceCallback(SetChartDataBindingSource);
                this.Invoke(d, new object[] { referenceRecord });
            }
            else
            {
                this.chartDataBindingSource.DataSource = (WiimoteReferenceRecord)referenceRecord;
            }
        }

        internal void SetRecordingButtonState(bool recordingState)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.WiimoteStatus.InvokeRequired)
            {
                SetRecordingButtonStateCallback d = new SetRecordingButtonStateCallback(SetRecordingButtonState);
                this.Invoke(d, new object[] { recordingState });
            }
            else
            {
                if (recordingState == true)
                {
                    this.RecordReference.Enabled = false;
                    this.RecordPlay.Enabled = false;
                }
                else
                {
                    this.RecordReference.Enabled = true;
                    this.RecordPlay.Enabled = true;
                }
            }
        }

        /*
        internal void SetTrainingMessage(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.trainingMessage.InvokeRequired)
            {
                SetTrainingMessageCallback d = new SetTrainingMessageCallback(SetTrainingMessage);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.trainingMessage.Text = text;
            }
        }
         * */

        internal void SetMatlabReturn(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.matlabReturnValue.InvokeRequired)
            {
                SetMatlabReturnCallback d = new SetMatlabReturnCallback(SetMatlabReturn);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.matlabReturnValue.Text = text;
            }
        }


        internal void SetBatteryLevels(string batteryLevel1, string batteryLevel2)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.wiimote1BatteryLevel.InvokeRequired)
            {
                SetBatteryLevelsCallback d = new SetBatteryLevelsCallback(SetBatteryLevels);
                this.Invoke(d, new object[] { batteryLevel1, batteryLevel2 });
            }
            else
            {
                this.wiimote1BatteryLevel.Text = batteryLevel1 + " %";
                this.wiimote2BatteryLevel.Text = batteryLevel2 + " %";
            }
        }


        #endregion

        #region Wiimote Connect Handlers
        //Button click event for Wiimote Connect
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                connectWiimotes();
            }
            catch (WiimoteConnectionException ex)
            {
                Console.WriteLine(ex);
            }
        }

        public bool connectWiimotes()
        {
            return m_WiimoteRecordingHandler.connectWiimotes();
        }

        public bool areWiimotesConnected()
        {
            return m_WiimoteRecordingHandler.areWiimotesConnected();
        }

        private void wiimoteDisconnect_Click(object sender, EventArgs e)
        {
            m_WiimoteRecordingHandler.disconnectWiimotes();

        }

        private void stopConnecting_Click(object sender, EventArgs e)
        {
            m_WiimoteRecordingHandler.stopConnectingAttempts();
        }

        private void accMotionPlus_CheckedChanged(object sender, EventArgs e)
        {

        }

        #endregion

        #region Reference Handlers

        private void NewReference_Click(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection lTrainingSegmentRows = this.traingSegmentGrid.SelectedRows;

            if (lTrainingSegmentRows.Count == 0)
            {
                MessageBox.Show("No Training Segment Selected. Select a Training Segment Source from the Training SegmentTable");
                return;
            }

            m_WiimoteRecordingHandler.newReferenceSelected(lTrainingSegmentRows[0]);

            this.dataGridView3.Rows[this.dataGridView3.Rows.Count - 1].Selected = true;
        }

        private void newReferenceRecordItem_Click(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection l_ReferenceRows = this.dataGridView3.SelectedRows;


            if (l_ReferenceRows.Count == 0)
            {
                MessageBox.Show("No Reference Data Selected. Select a Reference Source from the Reference Data Table");
                return;
            }

            m_WiimoteRecordingHandler.newReferenceRecordingItemSelected(l_ReferenceRows[0]);

            this.recordingItemGridView4.Rows[this.recordingItemGridView4.Rows.Count - 1].Selected = true;
        }


        private void UploadReference_Click(object sender, EventArgs e)
        {
            openWiimoteDataDialog.InitialDirectory = ProjectConstants.WIIMOTE_REFERENCE_DATA_PATH;
            if (openWiimoteDataDialog.ShowDialog() == DialogResult.OK)
            {
                m_WiimoteRecordingHandler.uploadReferenceSelected(openWiimoteDataDialog.FileName);
            }
        }

        private void MoveUpCommand_Click(object sender, EventArgs e)
        {
            /*
            DataGridViewSelectedRowCollection l_Rows = this.dataGridView3.SelectedRows;
            if (l_Rows.Count <= 0)
                return;

            m_WiimoteRecordingHandler.moveReferenceRecordUp(l_Rows[0].Index);
            int lPreviousIndex = l_Rows[0].Index;

            this.wiimoteReferenceDataStoreBindingSource.DataSource = WiimoteDataStore.getWiimoteDataStore().WiimoteReferenceRecords;
            this.dataGridView3.Rows[lPreviousIndex].Selected = false;
            this.dataGridView3.Rows[lPreviousIndex - 1].Selected = true;
             * */
        }

        private void MoveDownCommand_Click(object sender, EventArgs e)
        {
            /*
            DataGridViewSelectedRowCollection l_Rows = this.dataGridView3.SelectedRows;
            if (l_Rows.Count <= 0)
                return;

            m_WiimoteRecordingHandler.moveReferenceRecordDown(l_Rows[0].Index);
            int lPreviousIndex = l_Rows[0].Index;

            this.wiimoteReferenceDataStoreBindingSource.DataSource = WiimoteDataStore.getWiimoteDataStore().WiimoteReferenceRecords;
            this.dataGridView3.Rows[lPreviousIndex].Selected = false;
            this.dataGridView3.Rows[lPreviousIndex + 1].Selected = true;
             * */
        }

        private void DeleteRecord_Click(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection lTrainingRows = this.traingSegmentGrid.SelectedRows;

            DataGridViewSelectedRowCollection l_Rows = this.dataGridView3.SelectedRows;
            int length = l_Rows.Count;
            for (int index = length - 1; index >= 0; index--)
                m_WiimoteRecordingHandler.deleteReferenceRecordSelected(lTrainingRows[0],l_Rows[index],l_Rows[index].Index);

        }

        private void RecordReference_Click(object sender, EventArgs e)
        {
            try
            {
                DataGridViewSelectedRowCollection l_Rows = this.recordingItemGridView4.SelectedRows;

                if (l_Rows.Count == 0)
                {
                    MessageBox.Show("No Reference Data Selected. Select a Reference Source from the Reference Data Table");
                    return;
                }

                this.m_WiimoteRecordingHandler.recordReferenceRecordingItemSelected(l_Rows[0]);
                this.recordingItemGridView4.Rows[this.recordingItemGridView4.Rows.Count - 1].Selected = true;
//                l_Rows[0].Selected = true;

            }
            catch (WiimoteCommunicationException ex)
            {
                Console.WriteLine(ex);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Wiimote Recording Issue", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveReference_Click(object sender, EventArgs e)
        {
            WiimoteDataStore.Serialize();
        }

        private void dataGridView3_SelectionChanged(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection l_Rows = this.dataGridView3.SelectedRows;

            if (l_Rows.Count == 0)
            {
                return;
            }

            this.m_WiimoteRecordingHandler.referenceRecordRowReferencePageSelected(l_Rows[0]);

        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void chartShowOption_CheckedChanged(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection l_Rows = this.recordingItemGridView4.SelectedRows;

            if (l_Rows.Count == 0)
            {
                return;
            }

            this.m_WiimoteRecordingHandler.referenceRecordingItemRowSelected(l_Rows[0]);
        }

        private void recordingItemGridView4_SelectionChanged(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection l_Rows = this.recordingItemGridView4.SelectedRows;

            if (l_Rows.Count == 0)
            {
                return;
            }

            this.m_WiimoteRecordingHandler.referenceRecordingItemRowSelected(l_Rows[0]);
        }

        private void recordingItemGridView4_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow l_ReferenceRow = this.recordingItemGridView4.Rows[e.RowIndex];
            if (this.m_WiimoteRecordingHandler.editFilenameColumnAttempt(this.recordingItemGridView4, l_ReferenceRow, e.ColumnIndex))
                return;
            else
                this.recordingItemGridView4.CancelEdit();

        }

        private void deleteReferenceRecordingItemButton_Click(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection l_Rows = this.recordingItemGridView4.SelectedRows;
            int length = l_Rows.Count;
            if (length == 0)
            {
                MessageBox.Show("No Reference Recording Item Selected to be deleted");
                return;
            }

            DataGridViewSelectedRowCollection l_ReferenceRows = this.dataGridView3.SelectedRows;

            for (int index = length - 1; index >= 0; index--)
                m_WiimoteRecordingHandler.deleteReferenceRecordingItemSelected(l_ReferenceRows[index], l_Rows[index].Index);
        }

        public String getCurrentSaveDialogFilename()
        {
            return saveFileDialog.FileName;
        }

        #endregion

        #region Play Handlers


        private void NewPlayRecord_Click(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection l_ReferenceRows = this.referenceDataPlayPageView.SelectedRows;

            if (l_ReferenceRows.Count == 0)
            {
                MessageBox.Show("No Reference Data Selected. Select a Reference Source from the Reference Data Table");
                return;
            }

            this.m_WiimoteRecordingHandler.newPlaySelected(l_ReferenceRows[0]);

            this.dataGridView2.Rows[this.dataGridView2.Rows.Count - 1].Selected = true;

        }

        private void RecordPlay_Click(object sender, EventArgs e)
        {
            try
            {
                DataGridViewSelectedRowCollection l_PlayRows = this.dataGridView2.SelectedRows;

                if (l_PlayRows.Count == 0)
                {
                    MessageBox.Show("No Play Data Selected. Select a Play row from the Play Data Table");
                    return;
                }

                m_WiimoteRecordingHandler.recordPlaySelected(l_PlayRows[0]);
                this.dataGridView2.Rows[this.dataGridView2.Rows.Count -1].Selected = true;

            }
            catch (WiimoteCommunicationException ex)
            {
                Console.WriteLine(ex);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Wiimote Recording Issue", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void DeletePlayRecord_Click(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection l_Rows = this.dataGridView2.SelectedRows;
            m_WiimoteRecordingHandler.deletePlayRecord(l_Rows);

        }

        private void UploadPlay_Click(object sender, EventArgs e)
        {
            /*
            if (this.dataGridView1.Rows.Count == 0)
            {
                MessageBox.Show("No Reference Data Available");
                return;
            }

            int index = 0;
            DataGridViewSelectedRowCollection l_Rows = this.dataGridView1.SelectedRows;
            if (l_Rows.Count != 0)
                index = l_Rows[0].Index;

            openWiimoteDataDialog.InitialDirectory = ProjectConstants.WIIMOTE_REFERENCE_DATA_PATH;
            if (openWiimoteDataDialog.ShowDialog() == DialogResult.OK)
            {
                m_WiimoteRecordingHandler.uploadPlaySelected(index, openWiimoteDataDialog.FileName);
            }
             */

        }



        private void referenceDataPlayPageView_SelectionChanged(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection l_Rows = this.referenceDataPlayPageView.SelectedRows;

            if (l_Rows.Count == 0)
            {
                return;
            }

            this.m_WiimoteRecordingHandler.referenceRecordRowPlayPageSelected(l_Rows[0]);
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            /*
            WiimoteReferenceRecord referenceRecord = WiimoteDataStore.getWiimoteDataStore().getWiimoteReferenceRecord(e.RowIndex);
            WiimoteDataStore.getWiimoteDataStore().updateReferenceRecord(referenceRecord);

            this.wiimotePlayDataStoreBindingSource.DataSource = WiimoteDataStore.getWiimoteDataStore().WiimotePlayRecords;
             */
        }


        private void CompareRecords_Click(object sender, EventArgs e)
        {

            DataGridViewSelectedRowCollection l_ReferenceRows = this.referenceDataPlayPageView.SelectedRows;
            DataGridViewRow l_ReferenceRow = null;

            DataGridViewSelectedRowCollection l_PlayRows = this.dataGridView2.SelectedRows;


            if (l_ReferenceRows.Count != 0)
            {
                l_ReferenceRow = l_ReferenceRows[0];
                //                MessageBox.Show("No Reference Data Selected. Select a Reference Source from the Reference Data Table");
                //                return;
            }

            if (l_PlayRows.Count == 0)
            {
                MessageBox.Show("No Play Data Selected. Select a Play Source from the Play Data Table");
                return;
            }

            m_WiimoteRecordingHandler.comparePlayToReference(l_ReferenceRow, l_PlayRows[0]);

        }

        #endregion


        #region VideoLogic

        void ControlLogic()
        {
            /*
            if (_video == null)
            {
                this.PlayButton.Enabled = false;
                this.StopButton.Enabled = false;
                this.PauseButton.Enabled = false;
            }
            else
            {
                this.PlayButton.Enabled = true;
                this.StopButton.Enabled = true;
                this.PauseButton.Enabled = true;
            }
             * */
        }

        /// <summary>
        /// opens a video from an avi file
        /// and plays the first frame inside the panel
        /// </summary>
        void OpenVideo()
        {
            /*
            openVideoDialog.InitialDirectory = Application.StartupPath;
            if (openVideoDialog.ShowDialog() == DialogResult.OK)
            {
                // open the video

                // remember the original dimensions of the panel
                int height = videoPanel.Height;
                int width = videoPanel.Width;

                // dispose of the old video
                if (_video != null)
                {
                    _video.Dispose();
                }

                this.VideoPathValue.Text = openVideoDialog.FileName;

                // open a new video
                _video = new Video(openVideoDialog.FileName);

                // assign the win form control that will contain the video
                _video.Owner = this.videoPanel;

                // resize to fit in the panel
                videoPanel.Width = width;
                videoPanel.Height = height;

                // play the first frame of the video so we can identify it
                _video.Play();
                _video.Pause();
            }

            // enable video buttons
            ControlLogic();
             */
        }



        private void Open_Click(object sender, EventArgs e)
        {
//            OpenVideo();

        }

        private void Play_Click(object sender, EventArgs e)
        {
            /*
            if (_video != null)
            {
                _video.Play();
            }
             */
        }

        private void Pause_Click(object sender, EventArgs e)
        {
            /*
            if (_video != null)
            {
                _video.Pause();
            }
             */
        }

        private void Stop_Click(object sender, EventArgs e)
        {
            /*
            if (_video != null)
            {
                _video.Stop();
            }
             */
        }

        private void ReferenceOpenVideo_Click(object sender, EventArgs e)
        {
//            OpenVideo();
        }

        private void ReferencePlayVideo_Click(object sender, EventArgs e)
        {
            /*
            if (_video != null)
            {
                _video.Play();
            }
             */
        }

        private void ReferenceStopVideo_Click(object sender, EventArgs e)
        {
            /*
            if (_video != null)
            {
                _video.Stop();
            }
             */
        }

        private void ReferencePauseVideo_Click(object sender, EventArgs e)
        {
            /*
            if (_video != null)
            {
                _video.Pause();
            }
             */
        }

        #endregion


        #region Testing Region

        private void applicationStartSpeech()
        {
            ApplicationSpeech.speakText(ProjectConstants.APPLICATION_START_MESSAGE);
        }

        private void Test_Click_1(object sender, EventArgs e)
        {
            m_WiimoteRecordingHandler.testReferenceClicked();
        }

        private void TestPlay_Click_1(object sender, EventArgs e)
        {
/*
            DataGridViewSelectedRowCollection l_Rows = this.dataGridView1.SelectedRows;

            if (l_Rows.Count == 0)
            {
                MessageBox.Show("No Reference Data Selected. Select a Reference Source from the Reference Data Table");
                return;
            }

            m_WiimoteRecordingHandler.testPlayClicked(l_Rows[0]);
 */
        }

        private void TestTimer_Click(object sender, EventArgs e)
        {

            m_WiimoteRecordingHandler.testTimerClicked();
        }

        #endregion

        #region Setup

        private void musicBrowseButton_Click(object sender, EventArgs e)
        {
            this.openMP3FileDialog.InitialDirectory = Application.StartupPath;
            if (openMP3FileDialog.ShowDialog() == DialogResult.OK)
            {
                this.wiimoteBeatMP3Value.Text = openMP3FileDialog.FileName;
                m_WiimoteRecordingHandler.setTimerBeatMP3(openMP3FileDialog.FileName);
            }
        }

        private void wiimoteSumulationBrowse_Click(object sender, EventArgs e)
        {
            this.openWiimoteSimulationFileDialog.InitialDirectory = Application.StartupPath;
            if (openWiimoteSimulationFileDialog.ShowDialog() == DialogResult.OK)
            {
                this.wiimoteSimulationFile.Text = openWiimoteSimulationFileDialog.FileName;
            }
        }



        #endregion

        private void Exit_Click(object sender, EventArgs e)
        {
            Wiimotes.getWiimotesObject().cleanup();
            m_WiimoteRecordingHandler.disconnectWiimotes();
            mTrainingHandler.CleanupMedia();
            Environment.Exit(0);
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            m_WiimoteRecordingHandler.disconnectWiimotes();
            Environment.Exit(0);
        }


        private void _DoNothing(object sender, DataGridViewDataErrorEventArgs e)
        {

        }


        #region Calibration Handlers

        private void startCalibration_Click(object sender, EventArgs e)
        {
            m_WiimoteCalibrationHandler.startCalibrationThreaded();
        }

        private void RecordStepButton_Click(object sender, EventArgs e)
        {
            try
            {
                m_WiimoteCalibrationHandler.stopCalibration();
            }
            catch (WiimoteCommunicationException ex)
            {
                MessageBox.Show(ex.Message,
                    "Wiimote Connection Issue", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            catch (WiimoteConnectionException ex)
            {
                MessageBox.Show(ex.Message,
                    "Wiimote Connection Issue", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
            }
        }

        private void stopCalibration_Click(object sender, EventArgs e)
        {
            m_WiimoteCalibrationHandler.stopCalibration();
        }

        private void resetCalibration_Click(object sender, EventArgs e)
        {
            m_WiimoteCalibrationHandler.resetCalibration();
        }

        private void calibrationNumBeatsPerBarsValue_Validated(object sender, EventArgs e)
        {
            m_WiimoteCalibrationHandler.setCalibrationNumBeatsPerBars(calibrationNumBeatsPerBarsValue.Text);
        }

        #endregion

        #region Mogre Region

        private void loadMogre_Click(object sender, EventArgs e)
        {
//            Go();
        }

        private void mogrePropertyValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            setMogreProperties();
        }

        protected virtual void setMogreProperties()
        {
        }

        #endregion

        #region Training

        private void initializeTraining(Wiimotes pWiimotes)
        {
            try
            {
                if (Configuration.getConfiguration().TrainingMode)
                    InitializeTrainingVideoLayout();

                mTrainingHandler = new TrainingHandler(trainingVideoPanel1);
                mTrainingHandler.Initialize();
                mTrainingHandler.LoadTraining();
                trainingVideoPanel1.setWiimotesObject(pWiimotes);
            }
            catch (VideoPanelControl.TrainingVideoException ex)
            {
                MessageBox.Show(ex.Message, "Training Issue", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void startTapSetup()
        {
            if (mTrainingHandler.isTrainingVideoRunning())
                return;

            if (!areWiimotesConnected())
            {
                focusSetupTab();
                if (!connectWiimotes())
                    return;
            }
            focusCalibrationTab();

            m_WiimoteCalibrationHandler.startCalibrationThreaded();
        }

        private void startTraining()
        {
            focusTrainingTab();
            mTrainingHandler.startTraining();
        }

        private void trainingVideoPanel_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        public bool isTrainingVideoRunning()
        {
            return mTrainingHandler.isTrainingVideoRunning();
        }

        private void trainingPage_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            mTrainingHandler.pause();
        }

        public void focusCalibrationTab()
        {
            this.TabControl.SelectedIndex = ProjectConstants.CALIBRATION_INDEX;
        }

        public void focusSetupTab()
        {
            this.TabControl.SelectedIndex = ProjectConstants.SETUP_PANEL_INDEX;
        }

        public void focusTrainingTab()
        {
            this.TabControl.SelectedIndex = ProjectConstants.TRAINING_VIDEO_PANEL_INDEX;
        }

        private void tapSetupCommand_Click(object sender, EventArgs e)
        {
            startTapSetup();
        }

        private void startTrainingCommand_Click(object sender, EventArgs e)
        {
            startTraining();
        }

        #endregion


        #region Voice Commands

        public void OnVoiceCommandReceivedEvent(Object sender, VoiceCommandEventArgs args)
        {
            Utilities.ConsoleLogger.logMessage("In OnVoiceCommandReceivedEvent : Command is " + args.VoiceCommandValue);

            if (args.VoiceCommandValue.CompareTo(VoiceCommandEventArgs.STOP_COMMAND) == 0)
            {
                if (m_WiimoteCalibrationHandler.isCalibrationRunning())
                    m_WiimoteCalibrationHandler.stopCalibration();
                else if (mTrainingHandler.isTrainingVideoRunning())
                    mTrainingHandler.stop();
            }
            else if (args.VoiceCommandValue.CompareTo(VoiceCommandEventArgs.PAUSE_COMMAND) == 0)
                mTrainingHandler.pause();
            else if (args.VoiceCommandValue.CompareTo(VoiceCommandEventArgs.REPEAT_COMMAND) == 0)
                mTrainingHandler.repeatTraining();
            else if (args.VoiceCommandValue.CompareTo(VoiceCommandEventArgs.BACK_COMMAND) == 0)
                mTrainingHandler.previousFrame();
            else if (args.VoiceCommandValue.CompareTo(VoiceCommandEventArgs.FORWARD_COMMAND) == 0)
                mTrainingHandler.nextFrame();
            else if (args.VoiceCommandValue.CompareTo(VoiceCommandEventArgs.SLOWMO_COMMAND) == 0)
                mTrainingHandler.slowMo();
            else if (args.VoiceCommandValue.CompareTo(VoiceCommandEventArgs.SCORING_COMMAND) == 0)
                mTrainingHandler.scoreTraining();
            else if (args.VoiceCommandValue.CompareTo(VoiceCommandEventArgs.CALIBRATE_COMMAND) == 0)
                startTapSetup();
            else if (args.VoiceCommandValue.CompareTo(VoiceCommandEventArgs.TRAINING_COMMAND) == 0)
                startTraining();

        }

        private void setButtonVoiceCommandOn()
        {
            mSpeechRecognition.startEngine();
            this.voiceCommandsOption.Text = ProjectConstants.VOICE_COMMAND_ON_TEXT;
            this.configureMicCommand.Enabled = true;
            MessageBox.Show("Voice Commands Turned ON. Make sure you configure the Microphone", "Voice Command Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void setButtonVoiceCommandOff(bool pShowMessage)
        {
            mSpeechRecognition.stopEngine();
            this.voiceCommandsOption.Text = ProjectConstants.VOICE_COMMAND_OFF_TEXT;
            this.configureMicCommand.Enabled = false;
            if(pShowMessage)
                MessageBox.Show("Voice Commands Turned OFF", "Voice Command Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void voiceCommandsOption_Click(object sender, EventArgs e)
        {
            if (!mSpeechRecognition.isEngineStarted())
                setButtonVoiceCommandOn();
            else
                setButtonVoiceCommandOff(true);
        }

        private void configureMicCommand_Click(object sender, EventArgs e)
        {
            if(!mSpeechRecognition.configureMicrophone(this.Handle))
                MessageBox.Show("User training wizard not supported !", "ERROR - activating wizard", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        #endregion

        private void newTrainingSegment_Click(object sender, EventArgs e)
        {
            this.m_WiimoteRecordingHandler.newTrainingSegmentSelected();
            this.traingSegmentGrid.Rows[this.traingSegmentGrid.Rows.Count - 1].Selected = true;
        }

        private void traingSegmentGrid_SelectionChanged(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection l_Rows = this.traingSegmentGrid.SelectedRows;

            if (l_Rows.Count == 0)
            {
                return;
            }

            this.m_WiimoteRecordingHandler.trainingSegmentInfoRowReferencePageSelected(l_Rows[0]);
        }

        private void SaveDataStore_Click(object sender, EventArgs e)
        {
            this.m_WiimoteRecordingHandler.save();
        }

        private void deleteTrainingSegment_Click(object sender, EventArgs e)
        {

            DataGridViewSelectedRowCollection l_Rows = this.traingSegmentGrid.SelectedRows;
            int length = l_Rows.Count;
            for (int index = length - 1; index >= 0; index--)
                m_WiimoteRecordingHandler.deleteTrainingSegmentRecordSelected(l_Rows[index], l_Rows[index].Index);
        }

        private void trainingSegmentInfoBindingSource_CurrentChanged(object sender, EventArgs e)
        {

        }

        private void SaveConfiguration_Click(object sender, EventArgs e)
        {
            Configuration.getConfiguration().NumBeatsPerMinute = Convert.ToInt32(this.beatsPerMinute.Text);
            Configuration.getConfiguration().NumBeatsPerBar = Convert.ToInt32(this.beatsPerBar.Text);
            Configuration.getConfiguration().NumBars = Convert.ToInt32(this.numBars.Text);
        }

        private void readDataButton_Click(object sender, EventArgs e)
        {
            this.m_WiimoteRecordingHandler.StartDataCollection();
        }

        private void stopReplayButton_Click(object sender, EventArgs e)
        {
            this.m_WiimoteRecordingHandler.StopDataCollection();
        }

    }

}



/*

        private void dataGridView2_SelectionChanged(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection l_Rows = this.dataGridView2.SelectedRows;

            if (l_Rows.Count == 0)
            {
                return;
            }

            this.m_WiimoteRecordingHandler.playRecordRowSelected(l_Rows[0]);

        }

        private void showPlayRecordCharts_CheckedChanged(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection l_Rows = this.dataGridView2.SelectedRows;

            if (l_Rows.Count == 0)
            {
                return;
            }

            this.m_WiimoteRecordingHandler.playRecordRowSelected(l_Rows[0]);
        }
 
         internal void SetPlayDataBindingSource(object[] dataStore)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.WiimoteStatus.InvokeRequired)
            {
                SetPlayDataBindingSourceCallback d = new SetPlayDataBindingSourceCallback(SetPlayDataBindingSource);
                this.Invoke(d, new object[] { dataStore });
            }
            else
            {
                this.wiimotePlayDataStoreBindingSource.DataSource = (WiimotePlayRecord[] )dataStore;
            }
        }
* 

*/
using System;
using WiimoteData;

namespace MainGUI
{
    public partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series5 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series6 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend3 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series7 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series8 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series9 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea4 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend4 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series10 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series11 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series12 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            this.TUIOLog = new System.Windows.Forms.TextBox();
            this.SessionID = new System.Windows.Forms.TextBox();
            this.WiimoteNumber = new System.Windows.Forms.TextBox();
            this.EventTIme = new System.Windows.Forms.TextBox();
            this.AccX = new System.Windows.Forms.TextBox();
            this.PitchValue = new System.Windows.Forms.TextBox();
            this.RollValue = new System.Windows.Forms.TextBox();
            this.AccY = new System.Windows.Forms.TextBox();
            this.AccZ = new System.Windows.Forms.TextBox();
            this.RawPitch = new System.Windows.Forms.TextBox();
            this.RawRoll = new System.Windows.Forms.TextBox();
            this.RawYaw = new System.Windows.Forms.TextBox();
            this.SpeedRoll = new System.Windows.Forms.TextBox();
            this.SpeedPitch = new System.Windows.Forms.TextBox();
            this.SpeedYaw = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.TabControl = new System.Windows.Forms.TabControl();
            this.trainingPage = new System.Windows.Forms.TabPage();
            this.trainingVideoPanel1 = new VideoPanelControl.TrainingVideoPanel();
            this.setupTab = new System.Windows.Forms.TabPage();
            this.SaveConfiguration = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.calibrationSetup = new System.Windows.Forms.GroupBox();
            this.calibrationNumBeatsPerBarsValue = new System.Windows.Forms.TextBox();
            this.calibrationTimeUnit = new System.Windows.Forms.Label();
            this.wiimoteDataOptions = new System.Windows.Forms.GroupBox();
            this.accMotionplusIR = new System.Windows.Forms.RadioButton();
            this.accMotionPlus = new System.Windows.Forms.RadioButton();
            this.WiimoteDataSendIntervalValue = new System.Windows.Forms.TextBox();
            this.configurationBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.WiimoteDataSendIntervalLabel = new System.Windows.Forms.Label();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.wiimote2BatteryLevel = new System.Windows.Forms.TextBox();
            this.wiimote1BatteryLevel = new System.Windows.Forms.TextBox();
            this.label37 = new System.Windows.Forms.Label();
            this.label36 = new System.Windows.Forms.Label();
            this.label35 = new System.Windows.Forms.Label();
            this.wiimoteSimulationBrowse = new System.Windows.Forms.Button();
            this.wiimoteSimulationFile = new System.Windows.Forms.TextBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.WiimoteStateCheckDelayValue = new System.Windows.Forms.TextBox();
            this.WiimoteStateCheckDelayLabel = new System.Windows.Forms.Label();
            this.WiimoteStateNumChecksValue = new System.Windows.Forms.TextBox();
            this.WiimoteStateNumChecksLabel = new System.Windows.Forms.Label();
            this.WiimoteConnection = new System.Windows.Forms.GroupBox();
            this.stopConnecting = new System.Windows.Forms.Button();
            this.wiimoteConnectionProgress = new System.Windows.Forms.ProgressBar();
            this.WiimoteStatus = new System.Windows.Forms.TextBox();
            this.wiimoteDisconnect = new System.Windows.Forms.Button();
            this.wiimoteConnect = new System.Windows.Forms.Button();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.connectionParameters = new System.Windows.Forms.GroupBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.mediaControls = new System.Windows.Forms.GroupBox();
            this.trainingPreRecordingTime = new System.Windows.Forms.TextBox();
            this.labelPreRecordingTime = new System.Windows.Forms.Label();
            this.browseTrainingVideo = new System.Windows.Forms.Button();
            this.trainingVideoValue = new System.Windows.Forms.TextBox();
            this.trainingVideoLabel = new System.Windows.Forms.Label();
            this.browseTrainingMusic = new System.Windows.Forms.Button();
            this.trainingMusicValue = new System.Windows.Forms.TextBox();
            this.TrainingMusicLabel = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.RecordingIntervalLabel = new System.Windows.Forms.Label();
            this.musicBrowseButton = new System.Windows.Forms.Button();
            this.LeadInLabel = new System.Windows.Forms.Label();
            this.wiimoteBeatMP3Value = new System.Windows.Forms.TextBox();
            this.wiimoteBeatMusicLabel = new System.Windows.Forms.Label();
            this.RecordingInterval = new System.Windows.Forms.TextBox();
            this.mp3MusicOption = new System.Windows.Forms.CheckBox();
            this.LeadIn = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.beatsPerMinute = new System.Windows.Forms.TextBox();
            this.beatsPerBar = new System.Windows.Forms.TextBox();
            this.numBars = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.dynamicCalibrationOption = new System.Windows.Forms.RadioButton();
            this.systemCalibrationOption = new System.Windows.Forms.RadioButton();
            this.noCalibrationOption = new System.Windows.Forms.RadioButton();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.SystemConfiguration = new System.Windows.Forms.GroupBox();
            this.feedbackOnOption = new System.Windows.Forms.CheckBox();
            this.chartOneAxisOnly = new System.Windows.Forms.CheckBox();
            this.chartSkipStepValue = new System.Windows.Forms.TextBox();
            this.label31 = new System.Windows.Forms.Label();
            this.soundOption = new System.Windows.Forms.CheckBox();
            this.CalibrationTab = new System.Windows.Forms.TabPage();
            this.VoiceSetupGroup = new System.Windows.Forms.GroupBox();
            this.configureMicCommand = new System.Windows.Forms.Button();
            this.voiceCommandsOption = new System.Windows.Forms.Button();
            this.startCalibration = new System.Windows.Forms.Button();
            this.resetCalibration = new System.Windows.Forms.Button();
            this.calibrationRecordingStatus = new System.Windows.Forms.TextBox();
            this.RecordStopButton = new System.Windows.Forms.Button();
            this.calibrationGroup = new System.Windows.Forms.GroupBox();
            this.BluetoothSetupTab = new System.Windows.Forms.TabPage();
            this.ReferenceData = new System.Windows.Forms.TabPage();
            this.deleteTrainingSegment = new System.Windows.Forms.Button();
            this.SaveDataStore = new System.Windows.Forms.Button();
            this.newTrainingSegment = new System.Windows.Forms.Button();
            this.traingSegmentGrid = new System.Windows.Forms.DataGridView();
            this.RecordName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.numBeatsPerMinuteDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.numBeatsPerBarDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.numBarsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.leadInDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.recordingIntervalDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.trainingReferenceRecordDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.slowMoOptionDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.scoringOptionDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.trainingPlayRecordDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.highestRecordingItemIndexDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.recordIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.recordNameDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.trainingSegmentInfoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.chartShowOption = new System.Windows.Forms.CheckBox();
            this.wiimote2Label = new System.Windows.Forms.Label();
            this.wiimote1Label = new System.Windows.Forms.Label();
            this.wiimote2Gyro = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.wiimote1Gyro = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.wiimote2AccChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.MoveDownCommand = new System.Windows.Forms.Button();
            this.MoveUpCommand = new System.Windows.Forms.Button();
            this.deleteReferenceRecordingItemButton = new System.Windows.Forms.Button();
            this.newReferenceRecordItem = new System.Windows.Forms.Button();
            this.recordingItemGridView4 = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.RecordingDone = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.recordNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn18 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.referenceRecordingItemsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.wiimoteReferenceRecordBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.NewReference = new System.Windows.Forms.Button();
            this.UploadReference = new System.Windows.Forms.Button();
            this.DeleteRecord = new System.Windows.Forms.Button();
            this.dataGridView3 = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VideoPath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SlowMoOption = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ScoringOption = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.RepeatOption = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.parentRecordDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.parentTrainingSegmentInfoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.slowMoOptionDataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.scoringOptionDataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.repeatOptionDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.wiimoteRecordingDelayDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.videoPathDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.filePathDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.recordedTimeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.recordingDoneDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.highestRecordingItemIndexDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.recordIDDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.recordNameDataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.wiimoteReferenceDataStoreBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.RecordReference = new System.Windows.Forms.Button();
            this.WiimoteSensorData = new System.Windows.Forms.TabPage();
            this.WiimoteLog = new System.Windows.Forms.TabPage();
            this.TimerSettings = new System.Windows.Forms.TabPage();
            this.TestTimer = new System.Windows.Forms.Button();
            this.BeatLog = new System.Windows.Forms.TextBox();
            this.TestPage = new System.Windows.Forms.TabPage();
            this.label32 = new System.Windows.Forms.Label();
            this.matlabReturnValue = new System.Windows.Forms.TextBox();
            this.Test = new System.Windows.Forms.Button();
            this.TestPlay = new System.Windows.Forms.Button();
            this.Training = new System.Windows.Forms.TabPage();
            this.MogrePropertyView = new System.Windows.Forms.DataGridView();
            this.mogreWrapperMainBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.loadMogre = new System.Windows.Forms.Button();
            this.ogrePanel = new System.Windows.Forms.Panel();
            this.xnaWindow = new System.Windows.Forms.TabPage();
            this.stopReplayButton = new System.Windows.Forms.Button();
            this.readDataButton = new System.Windows.Forms.Button();
            this.quartenionViewerControl = new WinFormsGraphicsDevice.QuartenionViewerControl();
            this.RecordingStatus = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label22 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.button8 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.button10 = new System.Windows.Forms.Button();
            this.button11 = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label30 = new System.Windows.Forms.Label();
            this.beatCount = new System.Windows.Forms.Label();
            this.scoreFeedbackText = new System.Windows.Forms.TextBox();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.startTrainingCommand = new System.Windows.Forms.Button();
            this.tapSetupCommand = new System.Windows.Forms.Button();
            this.Exit = new System.Windows.Forms.Button();
            this.chartDataBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.referenceDataPlayPageView = new System.Windows.Forms.DataGridView();
            this.videoPanel = new System.Windows.Forms.Panel();
            this.PlayButton = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.StopButton = new System.Windows.Forms.Button();
            this.PauseButton = new System.Windows.Forms.Button();
            this.label27 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.VideoPathValue = new System.Windows.Forms.Label();
            this.RecordPlay = new System.Windows.Forms.Button();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NumBeatsPerBar = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NumBeatsPerMinute = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Fluctuation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AverageDelay = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DeletePlayRecord = new System.Windows.Forms.Button();
            this.UploadPlay = new System.Windows.Forms.Button();
            this.NewPlayRecord = new System.Windows.Forms.Button();
            this.CompareRecords = new System.Windows.Forms.Button();
            this.TabControl.SuspendLayout();
            this.trainingPage.SuspendLayout();
            this.setupTab.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.calibrationSetup.SuspendLayout();
            this.wiimoteDataOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.configurationBindingSource)).BeginInit();
            this.groupBox7.SuspendLayout();
            this.WiimoteConnection.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.mediaControls.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SystemConfiguration.SuspendLayout();
            this.CalibrationTab.SuspendLayout();
            this.VoiceSetupGroup.SuspendLayout();
            this.ReferenceData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.traingSegmentGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trainingSegmentInfoBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.wiimote2Gyro)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.wiimote1Gyro)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.wiimote2AccChart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.recordingItemGridView4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.referenceRecordingItemsBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.wiimoteReferenceRecordBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.wiimoteReferenceDataStoreBindingSource)).BeginInit();
            this.WiimoteSensorData.SuspendLayout();
            this.WiimoteLog.SuspendLayout();
            this.TimerSettings.SuspendLayout();
            this.TestPage.SuspendLayout();
            this.Training.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MogrePropertyView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mogreWrapperMainBindingSource)).BeginInit();
            this.xnaWindow.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartDataBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.referenceDataPlayPageView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.SuspendLayout();
            // 
            // TUIOLog
            // 
            this.TUIOLog.Location = new System.Drawing.Point(15, 54);
            this.TUIOLog.Multiline = true;
            this.TUIOLog.Name = "TUIOLog";
            this.TUIOLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.TUIOLog.Size = new System.Drawing.Size(840, 208);
            this.TUIOLog.TabIndex = 7;
            // 
            // SessionID
            // 
            this.SessionID.Location = new System.Drawing.Point(121, 28);
            this.SessionID.Name = "SessionID";
            this.SessionID.Size = new System.Drawing.Size(82, 20);
            this.SessionID.TabIndex = 8;
            // 
            // WiimoteNumber
            // 
            this.WiimoteNumber.Location = new System.Drawing.Point(423, 28);
            this.WiimoteNumber.Name = "WiimoteNumber";
            this.WiimoteNumber.Size = new System.Drawing.Size(83, 20);
            this.WiimoteNumber.TabIndex = 9;
            // 
            // EventTIme
            // 
            this.EventTIme.Location = new System.Drawing.Point(121, 70);
            this.EventTIme.Name = "EventTIme";
            this.EventTIme.Size = new System.Drawing.Size(85, 20);
            this.EventTIme.TabIndex = 10;
            // 
            // AccX
            // 
            this.AccX.Location = new System.Drawing.Point(121, 117);
            this.AccX.Name = "AccX";
            this.AccX.Size = new System.Drawing.Size(85, 20);
            this.AccX.TabIndex = 11;
            // 
            // PitchValue
            // 
            this.PitchValue.Location = new System.Drawing.Point(423, 70);
            this.PitchValue.Name = "PitchValue";
            this.PitchValue.Size = new System.Drawing.Size(83, 20);
            this.PitchValue.TabIndex = 12;
            // 
            // RollValue
            // 
            this.RollValue.Location = new System.Drawing.Point(694, 70);
            this.RollValue.Name = "RollValue";
            this.RollValue.Size = new System.Drawing.Size(86, 20);
            this.RollValue.TabIndex = 13;
            // 
            // AccY
            // 
            this.AccY.Location = new System.Drawing.Point(423, 117);
            this.AccY.Name = "AccY";
            this.AccY.Size = new System.Drawing.Size(89, 20);
            this.AccY.TabIndex = 14;
            // 
            // AccZ
            // 
            this.AccZ.Location = new System.Drawing.Point(694, 126);
            this.AccZ.Name = "AccZ";
            this.AccZ.Size = new System.Drawing.Size(86, 20);
            this.AccZ.TabIndex = 15;
            // 
            // RawPitch
            // 
            this.RawPitch.Location = new System.Drawing.Point(423, 167);
            this.RawPitch.Name = "RawPitch";
            this.RawPitch.Size = new System.Drawing.Size(88, 20);
            this.RawPitch.TabIndex = 16;
            // 
            // RawRoll
            // 
            this.RawRoll.Location = new System.Drawing.Point(694, 167);
            this.RawRoll.Name = "RawRoll";
            this.RawRoll.Size = new System.Drawing.Size(85, 20);
            this.RawRoll.TabIndex = 17;
            // 
            // RawYaw
            // 
            this.RawYaw.Location = new System.Drawing.Point(121, 167);
            this.RawYaw.Name = "RawYaw";
            this.RawYaw.Size = new System.Drawing.Size(85, 20);
            this.RawYaw.TabIndex = 18;
            // 
            // SpeedRoll
            // 
            this.SpeedRoll.Location = new System.Drawing.Point(696, 222);
            this.SpeedRoll.Name = "SpeedRoll";
            this.SpeedRoll.Size = new System.Drawing.Size(84, 20);
            this.SpeedRoll.TabIndex = 19;
            // 
            // SpeedPitch
            // 
            this.SpeedPitch.Location = new System.Drawing.Point(423, 222);
            this.SpeedPitch.Name = "SpeedPitch";
            this.SpeedPitch.Size = new System.Drawing.Size(88, 20);
            this.SpeedPitch.TabIndex = 20;
            // 
            // SpeedYaw
            // 
            this.SpeedYaw.Location = new System.Drawing.Point(121, 222);
            this.SpeedYaw.Name = "SpeedYaw";
            this.SpeedYaw.Size = new System.Drawing.Size(81, 20);
            this.SpeedYaw.TabIndex = 21;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(352, 31);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(55, 13);
            this.label4.TabIndex = 22;
            this.label4.Text = "Wiimote #";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(346, 73);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(61, 13);
            this.label5.TabIndex = 23;
            this.label5.Text = "Pitch Angle";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(628, 73);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(55, 13);
            this.label6.TabIndex = 24;
            this.label6.Text = "Roll Angle";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(642, 126);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(36, 13);
            this.label7.TabIndex = 25;
            this.label7.Text = "Acc Z";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(366, 120);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(36, 13);
            this.label8.TabIndex = 26;
            this.label8.Text = "Acc Y";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(628, 170);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(50, 13);
            this.label9.TabIndex = 27;
            this.label9.Text = "Raw Roll";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(346, 170);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(56, 13);
            this.label10.TabIndex = 28;
            this.label10.Text = "Raw Pitch";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(624, 225);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(59, 13);
            this.label11.TabIndex = 29;
            this.label11.Text = "Roll Speed";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(342, 225);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(65, 13);
            this.label12.TabIndex = 30;
            this.label12.Text = "Pitch Speed";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(53, 31);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(58, 13);
            this.label13.TabIndex = 31;
            this.label13.Text = "Session ID";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(35, 70);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(76, 13);
            this.label14.TabIndex = 32;
            this.label14.Text = "WiiEvent Time";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(75, 117);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(36, 13);
            this.label15.TabIndex = 33;
            this.label15.Text = "Acc X";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(58, 170);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(53, 13);
            this.label16.TabIndex = 34;
            this.label16.Text = "Raw Yaw";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(49, 229);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(62, 13);
            this.label17.TabIndex = 35;
            this.label17.Text = "Yaw Speed";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(12, 25);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(66, 13);
            this.label18.TabIndex = 36;
            this.label18.Text = "Wiimote Log";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(349, 28);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(50, 13);
            this.label19.TabIndex = 38;
            this.label19.Text = "Beat Log";
            // 
            // TabControl
            // 
            this.TabControl.Controls.Add(this.trainingPage);
            this.TabControl.Controls.Add(this.setupTab);
            this.TabControl.Controls.Add(this.CalibrationTab);
            this.TabControl.Controls.Add(this.BluetoothSetupTab);
            this.TabControl.Controls.Add(this.ReferenceData);
            this.TabControl.Controls.Add(this.WiimoteSensorData);
            this.TabControl.Controls.Add(this.WiimoteLog);
            this.TabControl.Controls.Add(this.TimerSettings);
            this.TabControl.Controls.Add(this.TestPage);
            this.TabControl.Controls.Add(this.Training);
            this.TabControl.Controls.Add(this.xnaWindow);
            this.TabControl.Location = new System.Drawing.Point(9, 0);
            this.TabControl.Name = "TabControl";
            this.TabControl.SelectedIndex = 0;
            this.TabControl.Size = new System.Drawing.Size(1217, 780);
            this.TabControl.TabIndex = 39;
            // 
            // trainingPage
            // 
            this.trainingPage.Controls.Add(this.trainingVideoPanel1);
            this.trainingPage.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.trainingPage.Location = new System.Drawing.Point(4, 22);
            this.trainingPage.Name = "trainingPage";
            this.trainingPage.Padding = new System.Windows.Forms.Padding(3);
            this.trainingPage.Size = new System.Drawing.Size(1209, 754);
            this.trainingPage.TabIndex = 8;
            this.trainingPage.Text = "Main";
            this.trainingPage.UseVisualStyleBackColor = true;
            this.trainingPage.Click += new System.EventHandler(this.trainingPage_Click);
            // 
            // trainingVideoPanel1
            // 
            this.trainingVideoPanel1.Location = new System.Drawing.Point(3, 0);
            this.trainingVideoPanel1.Name = "trainingVideoPanel1";
            this.trainingVideoPanel1.Size = new System.Drawing.Size(1216, 780);
            this.trainingVideoPanel1.TabIndex = 5;
            // 
            // setupTab
            // 
            this.setupTab.Controls.Add(this.SaveConfiguration);
            this.setupTab.Controls.Add(this.tabControl1);
            this.setupTab.Location = new System.Drawing.Point(4, 22);
            this.setupTab.Name = "setupTab";
            this.setupTab.Padding = new System.Windows.Forms.Padding(3);
            this.setupTab.Size = new System.Drawing.Size(1209, 754);
            this.setupTab.TabIndex = 6;
            this.setupTab.Text = "Setup";
            this.setupTab.UseVisualStyleBackColor = true;
            // 
            // SaveConfiguration
            // 
            this.SaveConfiguration.Location = new System.Drawing.Point(10, 684);
            this.SaveConfiguration.Name = "SaveConfiguration";
            this.SaveConfiguration.Size = new System.Drawing.Size(155, 35);
            this.SaveConfiguration.TabIndex = 70;
            this.SaveConfiguration.Text = "Save";
            this.SaveConfiguration.UseVisualStyleBackColor = true;
            this.SaveConfiguration.Click += new System.EventHandler(this.SaveConfiguration_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(6, 6);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1151, 667);
            this.tabControl1.TabIndex = 69;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.calibrationSetup);
            this.tabPage1.Controls.Add(this.wiimoteDataOptions);
            this.tabPage1.Controls.Add(this.WiimoteDataSendIntervalValue);
            this.tabPage1.Controls.Add(this.WiimoteDataSendIntervalLabel);
            this.tabPage1.Controls.Add(this.groupBox7);
            this.tabPage1.Controls.Add(this.label35);
            this.tabPage1.Controls.Add(this.wiimoteSimulationBrowse);
            this.tabPage1.Controls.Add(this.wiimoteSimulationFile);
            this.tabPage1.Controls.Add(this.checkBox1);
            this.tabPage1.Controls.Add(this.WiimoteStateCheckDelayValue);
            this.tabPage1.Controls.Add(this.WiimoteStateCheckDelayLabel);
            this.tabPage1.Controls.Add(this.WiimoteStateNumChecksValue);
            this.tabPage1.Controls.Add(this.WiimoteStateNumChecksLabel);
            this.tabPage1.Controls.Add(this.WiimoteConnection);
            this.tabPage1.Controls.Add(this.groupBox6);
            this.tabPage1.Controls.Add(this.connectionParameters);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1143, 641);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Wiimote Connection";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // calibrationSetup
            // 
            this.calibrationSetup.Controls.Add(this.calibrationNumBeatsPerBarsValue);
            this.calibrationSetup.Controls.Add(this.calibrationTimeUnit);
            this.calibrationSetup.Location = new System.Drawing.Point(517, 511);
            this.calibrationSetup.Name = "calibrationSetup";
            this.calibrationSetup.Size = new System.Drawing.Size(324, 123);
            this.calibrationSetup.TabIndex = 70;
            this.calibrationSetup.TabStop = false;
            this.calibrationSetup.Text = "Calibration Setup";
            // 
            // calibrationNumBeatsPerBarsValue
            // 
            this.calibrationNumBeatsPerBarsValue.Location = new System.Drawing.Point(142, 40);
            this.calibrationNumBeatsPerBarsValue.Name = "calibrationNumBeatsPerBarsValue";
            this.calibrationNumBeatsPerBarsValue.Size = new System.Drawing.Size(73, 20);
            this.calibrationNumBeatsPerBarsValue.TabIndex = 1;
            this.calibrationNumBeatsPerBarsValue.Text = "8";
            this.calibrationNumBeatsPerBarsValue.Validated += new System.EventHandler(this.calibrationNumBeatsPerBarsValue_Validated);
            // 
            // calibrationTimeUnit
            // 
            this.calibrationTimeUnit.AutoSize = true;
            this.calibrationTimeUnit.Location = new System.Drawing.Point(16, 43);
            this.calibrationTimeUnit.Name = "calibrationTimeUnit";
            this.calibrationTimeUnit.Size = new System.Drawing.Size(97, 13);
            this.calibrationTimeUnit.TabIndex = 0;
            this.calibrationTimeUnit.Text = "Num Beats Per Bar";
            // 
            // wiimoteDataOptions
            // 
            this.wiimoteDataOptions.Controls.Add(this.accMotionplusIR);
            this.wiimoteDataOptions.Controls.Add(this.accMotionPlus);
            this.wiimoteDataOptions.Location = new System.Drawing.Point(516, 333);
            this.wiimoteDataOptions.Name = "wiimoteDataOptions";
            this.wiimoteDataOptions.Size = new System.Drawing.Size(324, 172);
            this.wiimoteDataOptions.TabIndex = 69;
            this.wiimoteDataOptions.TabStop = false;
            this.wiimoteDataOptions.Text = "WiimoteDataOptions";
            // 
            // accMotionplusIR
            // 
            this.accMotionplusIR.AutoSize = true;
            this.accMotionplusIR.Location = new System.Drawing.Point(40, 92);
            this.accMotionplusIR.Name = "accMotionplusIR";
            this.accMotionplusIR.Size = new System.Drawing.Size(131, 17);
            this.accMotionplusIR.TabIndex = 1;
            this.accMotionplusIR.TabStop = true;
            this.accMotionplusIR.Text = "Acc + MotionPlus + IR";
            this.accMotionplusIR.UseVisualStyleBackColor = true;
            // 
            // accMotionPlus
            // 
            this.accMotionPlus.AutoSize = true;
            this.accMotionPlus.Location = new System.Drawing.Point(40, 45);
            this.accMotionPlus.Name = "accMotionPlus";
            this.accMotionPlus.Size = new System.Drawing.Size(113, 17);
            this.accMotionPlus.TabIndex = 0;
            this.accMotionPlus.TabStop = true;
            this.accMotionPlus.Text = "Acc + MotionsPlus";
            this.accMotionPlus.UseVisualStyleBackColor = true;
            this.accMotionPlus.CheckedChanged += new System.EventHandler(this.accMotionPlus_CheckedChanged);
            // 
            // WiimoteDataSendIntervalValue
            // 
            this.WiimoteDataSendIntervalValue.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.configurationBindingSource, "WiimoteDataSendInterval", true));
            this.WiimoteDataSendIntervalValue.DataBindings.Add(new System.Windows.Forms.Binding("Tag", this.configurationBindingSource, "WiimoteDataSendInterval", true));
            this.WiimoteDataSendIntervalValue.Location = new System.Drawing.Point(236, 443);
            this.WiimoteDataSendIntervalValue.Name = "WiimoteDataSendIntervalValue";
            this.WiimoteDataSendIntervalValue.Size = new System.Drawing.Size(82, 20);
            this.WiimoteDataSendIntervalValue.TabIndex = 67;
            // 
            // configurationBindingSource
            // 
            this.configurationBindingSource.DataSource = typeof(ProjectCommon.Configuration);
            // 
            // WiimoteDataSendIntervalLabel
            // 
            this.WiimoteDataSendIntervalLabel.AutoSize = true;
            this.WiimoteDataSendIntervalLabel.Location = new System.Drawing.Point(57, 443);
            this.WiimoteDataSendIntervalLabel.Name = "WiimoteDataSendIntervalLabel";
            this.WiimoteDataSendIntervalLabel.Size = new System.Drawing.Size(137, 13);
            this.WiimoteDataSendIntervalLabel.TabIndex = 66;
            this.WiimoteDataSendIntervalLabel.Text = "Wiimote Data Send Interval";
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.wiimote2BatteryLevel);
            this.groupBox7.Controls.Add(this.wiimote1BatteryLevel);
            this.groupBox7.Controls.Add(this.label37);
            this.groupBox7.Controls.Add(this.label36);
            this.groupBox7.Location = new System.Drawing.Point(516, 50);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(325, 216);
            this.groupBox7.TabIndex = 65;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Wiimote Battery Levels";
            // 
            // wiimote2BatteryLevel
            // 
            this.wiimote2BatteryLevel.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.wiimote2BatteryLevel.ForeColor = System.Drawing.Color.Blue;
            this.wiimote2BatteryLevel.Location = new System.Drawing.Point(134, 101);
            this.wiimote2BatteryLevel.Name = "wiimote2BatteryLevel";
            this.wiimote2BatteryLevel.Size = new System.Drawing.Size(100, 20);
            this.wiimote2BatteryLevel.TabIndex = 3;
            // 
            // wiimote1BatteryLevel
            // 
            this.wiimote1BatteryLevel.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.wiimote1BatteryLevel.ForeColor = System.Drawing.Color.Blue;
            this.wiimote1BatteryLevel.Location = new System.Drawing.Point(134, 46);
            this.wiimote1BatteryLevel.Name = "wiimote1BatteryLevel";
            this.wiimote1BatteryLevel.Size = new System.Drawing.Size(100, 20);
            this.wiimote1BatteryLevel.TabIndex = 2;
            // 
            // label37
            // 
            this.label37.AutoSize = true;
            this.label37.Location = new System.Drawing.Point(38, 108);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(61, 13);
            this.label37.TabIndex = 1;
            this.label37.Text = "Wiimote #2";
            // 
            // label36
            // 
            this.label36.AutoSize = true;
            this.label36.Location = new System.Drawing.Point(38, 49);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(61, 13);
            this.label36.TabIndex = 0;
            this.label36.Text = "Wiimote #1";
            // 
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.Location = new System.Drawing.Point(59, 596);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(115, 13);
            this.label35.TabIndex = 63;
            this.label35.Text = "Wiimote Simulation File";
            // 
            // wiimoteSimulationBrowse
            // 
            this.wiimoteSimulationBrowse.Location = new System.Drawing.Point(387, 586);
            this.wiimoteSimulationBrowse.Name = "wiimoteSimulationBrowse";
            this.wiimoteSimulationBrowse.Size = new System.Drawing.Size(55, 23);
            this.wiimoteSimulationBrowse.TabIndex = 62;
            this.wiimoteSimulationBrowse.Text = "..";
            this.wiimoteSimulationBrowse.UseVisualStyleBackColor = true;
            this.wiimoteSimulationBrowse.Click += new System.EventHandler(this.wiimoteSumulationBrowse_Click);
            // 
            // wiimoteSimulationFile
            // 
            this.wiimoteSimulationFile.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.configurationBindingSource, "WiimoteSimulationFile", true));
            this.wiimoteSimulationFile.Location = new System.Drawing.Point(194, 589);
            this.wiimoteSimulationFile.Name = "wiimoteSimulationFile";
            this.wiimoteSimulationFile.Size = new System.Drawing.Size(173, 20);
            this.wiimoteSimulationFile.TabIndex = 61;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.configurationBindingSource, "WiimoteSimulationMode", true));
            this.checkBox1.DataBindings.Add(new System.Windows.Forms.Binding("CheckState", this.configurationBindingSource, "WiimoteSimulationMode", true));
            this.checkBox1.Location = new System.Drawing.Point(62, 541);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(104, 17);
            this.checkBox1.TabIndex = 60;
            this.checkBox1.Text = "Simulation Mode";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // WiimoteStateCheckDelayValue
            // 
            this.WiimoteStateCheckDelayValue.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.configurationBindingSource, "WaitTimeCheckConnection", true));
            this.WiimoteStateCheckDelayValue.DataBindings.Add(new System.Windows.Forms.Binding("Tag", this.configurationBindingSource, "WaitTimeCheckConnection", true));
            this.WiimoteStateCheckDelayValue.Location = new System.Drawing.Point(236, 403);
            this.WiimoteStateCheckDelayValue.Name = "WiimoteStateCheckDelayValue";
            this.WiimoteStateCheckDelayValue.Size = new System.Drawing.Size(82, 20);
            this.WiimoteStateCheckDelayValue.TabIndex = 59;
            // 
            // WiimoteStateCheckDelayLabel
            // 
            this.WiimoteStateCheckDelayLabel.AutoSize = true;
            this.WiimoteStateCheckDelayLabel.Location = new System.Drawing.Point(68, 403);
            this.WiimoteStateCheckDelayLabel.Name = "WiimoteStateCheckDelayLabel";
            this.WiimoteStateCheckDelayLabel.Size = new System.Drawing.Size(137, 13);
            this.WiimoteStateCheckDelayLabel.TabIndex = 58;
            this.WiimoteStateCheckDelayLabel.Text = "Wiimote State Check Delay";
            // 
            // WiimoteStateNumChecksValue
            // 
            this.WiimoteStateNumChecksValue.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.configurationBindingSource, "MaxWiimoteConnectionTries", true));
            this.WiimoteStateNumChecksValue.DataBindings.Add(new System.Windows.Forms.Binding("Tag", this.configurationBindingSource, "MaxWiimoteConnectionTries", true));
            this.WiimoteStateNumChecksValue.Location = new System.Drawing.Point(236, 356);
            this.WiimoteStateNumChecksValue.Name = "WiimoteStateNumChecksValue";
            this.WiimoteStateNumChecksValue.Size = new System.Drawing.Size(82, 20);
            this.WiimoteStateNumChecksValue.TabIndex = 57;
            // 
            // WiimoteStateNumChecksLabel
            // 
            this.WiimoteStateNumChecksLabel.AutoSize = true;
            this.WiimoteStateNumChecksLabel.Location = new System.Drawing.Point(50, 356);
            this.WiimoteStateNumChecksLabel.Name = "WiimoteStateNumChecksLabel";
            this.WiimoteStateNumChecksLabel.Size = new System.Drawing.Size(154, 13);
            this.WiimoteStateNumChecksLabel.TabIndex = 56;
            this.WiimoteStateNumChecksLabel.Text = "# of Wiimote Connect Attempts";
            // 
            // WiimoteConnection
            // 
            this.WiimoteConnection.Controls.Add(this.stopConnecting);
            this.WiimoteConnection.Controls.Add(this.wiimoteConnectionProgress);
            this.WiimoteConnection.Controls.Add(this.WiimoteStatus);
            this.WiimoteConnection.Controls.Add(this.wiimoteDisconnect);
            this.WiimoteConnection.Controls.Add(this.wiimoteConnect);
            this.WiimoteConnection.Location = new System.Drawing.Point(39, 50);
            this.WiimoteConnection.Name = "WiimoteConnection";
            this.WiimoteConnection.Size = new System.Drawing.Size(423, 273);
            this.WiimoteConnection.TabIndex = 55;
            this.WiimoteConnection.TabStop = false;
            this.WiimoteConnection.Text = "Wiimote Connection";
            // 
            // stopConnecting
            // 
            this.stopConnecting.Location = new System.Drawing.Point(309, 125);
            this.stopConnecting.Name = "stopConnecting";
            this.stopConnecting.Size = new System.Drawing.Size(55, 23);
            this.stopConnecting.TabIndex = 58;
            this.stopConnecting.Text = "Stop";
            this.stopConnecting.UseVisualStyleBackColor = true;
            this.stopConnecting.Click += new System.EventHandler(this.stopConnecting_Click);
            // 
            // wiimoteConnectionProgress
            // 
            this.wiimoteConnectionProgress.Location = new System.Drawing.Point(14, 125);
            this.wiimoteConnectionProgress.Maximum = 80;
            this.wiimoteConnectionProgress.Name = "wiimoteConnectionProgress";
            this.wiimoteConnectionProgress.Size = new System.Drawing.Size(284, 23);
            this.wiimoteConnectionProgress.Step = 1;
            this.wiimoteConnectionProgress.TabIndex = 57;
            // 
            // WiimoteStatus
            // 
            this.WiimoteStatus.BackColor = System.Drawing.SystemColors.Info;
            this.WiimoteStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.WiimoteStatus.ForeColor = System.Drawing.Color.Blue;
            this.WiimoteStatus.Location = new System.Drawing.Point(18, 180);
            this.WiimoteStatus.Name = "WiimoteStatus";
            this.WiimoteStatus.Size = new System.Drawing.Size(346, 26);
            this.WiimoteStatus.TabIndex = 40;
            this.WiimoteStatus.Text = "Wiimotes Not Connected";
            // 
            // wiimoteDisconnect
            // 
            this.wiimoteDisconnect.Location = new System.Drawing.Point(231, 28);
            this.wiimoteDisconnect.Name = "wiimoteDisconnect";
            this.wiimoteDisconnect.Size = new System.Drawing.Size(133, 61);
            this.wiimoteDisconnect.TabIndex = 39;
            this.wiimoteDisconnect.Text = "Disconnect Wiimotes";
            this.wiimoteDisconnect.UseVisualStyleBackColor = true;
            this.wiimoteDisconnect.Click += new System.EventHandler(this.wiimoteDisconnect_Click);
            // 
            // wiimoteConnect
            // 
            this.wiimoteConnect.Location = new System.Drawing.Point(14, 28);
            this.wiimoteConnect.Name = "wiimoteConnect";
            this.wiimoteConnect.Size = new System.Drawing.Size(127, 61);
            this.wiimoteConnect.TabIndex = 38;
            this.wiimoteConnect.Text = "Connect Wiimotes";
            this.wiimoteConnect.UseVisualStyleBackColor = true;
            this.wiimoteConnect.Click += new System.EventHandler(this.button2_Click);
            // 
            // groupBox6
            // 
            this.groupBox6.Location = new System.Drawing.Point(39, 511);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(423, 123);
            this.groupBox6.TabIndex = 64;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Simulation";
            // 
            // connectionParameters
            // 
            this.connectionParameters.Location = new System.Drawing.Point(39, 333);
            this.connectionParameters.Name = "connectionParameters";
            this.connectionParameters.Size = new System.Drawing.Size(423, 172);
            this.connectionParameters.TabIndex = 68;
            this.connectionParameters.TabStop = false;
            this.connectionParameters.Text = "Connection Parameters";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.mediaControls);
            this.tabPage2.Controls.Add(this.groupBox5);
            this.tabPage2.Controls.Add(this.groupBox4);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1143, 641);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Recording";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // mediaControls
            // 
            this.mediaControls.Controls.Add(this.trainingPreRecordingTime);
            this.mediaControls.Controls.Add(this.labelPreRecordingTime);
            this.mediaControls.Controls.Add(this.browseTrainingVideo);
            this.mediaControls.Controls.Add(this.trainingVideoValue);
            this.mediaControls.Controls.Add(this.trainingVideoLabel);
            this.mediaControls.Controls.Add(this.browseTrainingMusic);
            this.mediaControls.Controls.Add(this.trainingMusicValue);
            this.mediaControls.Controls.Add(this.TrainingMusicLabel);
            this.mediaControls.Location = new System.Drawing.Point(469, 413);
            this.mediaControls.Name = "mediaControls";
            this.mediaControls.Size = new System.Drawing.Size(496, 164);
            this.mediaControls.TabIndex = 76;
            this.mediaControls.TabStop = false;
            this.mediaControls.Text = "Training Settings";
            // 
            // trainingPreRecordingTime
            // 
            this.trainingPreRecordingTime.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.configurationBindingSource, "TrainingPreRecordingTime", true));
            this.trainingPreRecordingTime.DataBindings.Add(new System.Windows.Forms.Binding("Tag", this.configurationBindingSource, "TrainingPreRecordingTime", true));
            this.trainingPreRecordingTime.Location = new System.Drawing.Point(178, 121);
            this.trainingPreRecordingTime.Name = "trainingPreRecordingTime";
            this.trainingPreRecordingTime.Size = new System.Drawing.Size(100, 20);
            this.trainingPreRecordingTime.TabIndex = 83;
            // 
            // labelPreRecordingTime
            // 
            this.labelPreRecordingTime.AutoSize = true;
            this.labelPreRecordingTime.Location = new System.Drawing.Point(26, 121);
            this.labelPreRecordingTime.Name = "labelPreRecordingTime";
            this.labelPreRecordingTime.Size = new System.Drawing.Size(101, 13);
            this.labelPreRecordingTime.TabIndex = 82;
            this.labelPreRecordingTime.Text = "Pre Recording Time";
            // 
            // browseTrainingVideo
            // 
            this.browseTrainingVideo.Location = new System.Drawing.Point(332, 76);
            this.browseTrainingVideo.Name = "browseTrainingVideo";
            this.browseTrainingVideo.Size = new System.Drawing.Size(44, 23);
            this.browseTrainingVideo.TabIndex = 81;
            this.browseTrainingVideo.Text = "..";
            this.browseTrainingVideo.UseVisualStyleBackColor = true;
            // 
            // trainingVideoValue
            // 
            this.trainingVideoValue.Location = new System.Drawing.Point(178, 79);
            this.trainingVideoValue.Name = "trainingVideoValue";
            this.trainingVideoValue.Size = new System.Drawing.Size(100, 20);
            this.trainingVideoValue.TabIndex = 80;
            // 
            // trainingVideoLabel
            // 
            this.trainingVideoLabel.AutoSize = true;
            this.trainingVideoLabel.Location = new System.Drawing.Point(24, 77);
            this.trainingVideoLabel.Name = "trainingVideoLabel";
            this.trainingVideoLabel.Size = new System.Drawing.Size(75, 13);
            this.trainingVideoLabel.TabIndex = 79;
            this.trainingVideoLabel.Text = "Training Video";
            // 
            // browseTrainingMusic
            // 
            this.browseTrainingMusic.Location = new System.Drawing.Point(332, 24);
            this.browseTrainingMusic.Name = "browseTrainingMusic";
            this.browseTrainingMusic.Size = new System.Drawing.Size(44, 23);
            this.browseTrainingMusic.TabIndex = 78;
            this.browseTrainingMusic.Text = "..";
            this.browseTrainingMusic.UseVisualStyleBackColor = true;
            // 
            // trainingMusicValue
            // 
            this.trainingMusicValue.Location = new System.Drawing.Point(178, 27);
            this.trainingMusicValue.Name = "trainingMusicValue";
            this.trainingMusicValue.Size = new System.Drawing.Size(100, 20);
            this.trainingMusicValue.TabIndex = 77;
            // 
            // TrainingMusicLabel
            // 
            this.TrainingMusicLabel.AutoSize = true;
            this.TrainingMusicLabel.Location = new System.Drawing.Point(24, 34);
            this.TrainingMusicLabel.Name = "TrainingMusicLabel";
            this.TrainingMusicLabel.Size = new System.Drawing.Size(76, 13);
            this.TrainingMusicLabel.TabIndex = 76;
            this.TrainingMusicLabel.Text = "Training Music";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.RecordingIntervalLabel);
            this.groupBox5.Controls.Add(this.musicBrowseButton);
            this.groupBox5.Controls.Add(this.LeadInLabel);
            this.groupBox5.Controls.Add(this.wiimoteBeatMP3Value);
            this.groupBox5.Controls.Add(this.wiimoteBeatMusicLabel);
            this.groupBox5.Controls.Add(this.RecordingInterval);
            this.groupBox5.Controls.Add(this.mp3MusicOption);
            this.groupBox5.Controls.Add(this.LeadIn);
            this.groupBox5.Controls.Add(this.label1);
            this.groupBox5.Controls.Add(this.beatsPerMinute);
            this.groupBox5.Controls.Add(this.beatsPerBar);
            this.groupBox5.Controls.Add(this.numBars);
            this.groupBox5.Controls.Add(this.label2);
            this.groupBox5.Controls.Add(this.label3);
            this.groupBox5.Location = new System.Drawing.Point(468, 33);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(497, 354);
            this.groupBox5.TabIndex = 69;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "TimerSettings";
            // 
            // RecordingIntervalLabel
            // 
            this.RecordingIntervalLabel.AutoSize = true;
            this.RecordingIntervalLabel.Location = new System.Drawing.Point(37, 225);
            this.RecordingIntervalLabel.Name = "RecordingIntervalLabel";
            this.RecordingIntervalLabel.Size = new System.Drawing.Size(91, 13);
            this.RecordingIntervalLabel.TabIndex = 67;
            this.RecordingIntervalLabel.Text = "RecordingInterval";
            // 
            // musicBrowseButton
            // 
            this.musicBrowseButton.Location = new System.Drawing.Point(333, 265);
            this.musicBrowseButton.Name = "musicBrowseButton";
            this.musicBrowseButton.Size = new System.Drawing.Size(44, 23);
            this.musicBrowseButton.TabIndex = 74;
            this.musicBrowseButton.Text = "..";
            this.musicBrowseButton.UseVisualStyleBackColor = true;
            // 
            // LeadInLabel
            // 
            this.LeadInLabel.AutoSize = true;
            this.LeadInLabel.Location = new System.Drawing.Point(88, 177);
            this.LeadInLabel.Name = "LeadInLabel";
            this.LeadInLabel.Size = new System.Drawing.Size(40, 13);
            this.LeadInLabel.TabIndex = 66;
            this.LeadInLabel.Text = "LeadIn";
            // 
            // wiimoteBeatMP3Value
            // 
            this.wiimoteBeatMP3Value.Location = new System.Drawing.Point(179, 266);
            this.wiimoteBeatMP3Value.Name = "wiimoteBeatMP3Value";
            this.wiimoteBeatMP3Value.Size = new System.Drawing.Size(125, 20);
            this.wiimoteBeatMP3Value.TabIndex = 73;
            // 
            // wiimoteBeatMusicLabel
            // 
            this.wiimoteBeatMusicLabel.AutoSize = true;
            this.wiimoteBeatMusicLabel.Location = new System.Drawing.Point(36, 273);
            this.wiimoteBeatMusicLabel.Name = "wiimoteBeatMusicLabel";
            this.wiimoteBeatMusicLabel.Size = new System.Drawing.Size(92, 13);
            this.wiimoteBeatMusicLabel.TabIndex = 72;
            this.wiimoteBeatMusicLabel.Text = "wiimoteBeatMusic";
            // 
            // RecordingInterval
            // 
            this.RecordingInterval.Location = new System.Drawing.Point(179, 218);
            this.RecordingInterval.Name = "RecordingInterval";
            this.RecordingInterval.Size = new System.Drawing.Size(81, 20);
            this.RecordingInterval.TabIndex = 65;
            this.RecordingInterval.Text = "0";
            // 
            // mp3MusicOption
            // 
            this.mp3MusicOption.AutoSize = true;
            this.mp3MusicOption.Checked = true;
            this.mp3MusicOption.CheckState = System.Windows.Forms.CheckState.Checked;
            this.mp3MusicOption.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.configurationBindingSource, "MP3Option", true));
            this.mp3MusicOption.DataBindings.Add(new System.Windows.Forms.Binding("CheckState", this.configurationBindingSource, "MP3Option", true));
            this.mp3MusicOption.Location = new System.Drawing.Point(40, 319);
            this.mp3MusicOption.Name = "mp3MusicOption";
            this.mp3MusicOption.Size = new System.Drawing.Size(76, 17);
            this.mp3MusicOption.TabIndex = 75;
            this.mp3MusicOption.Text = "MP3Music";
            this.mp3MusicOption.UseVisualStyleBackColor = true;
            // 
            // LeadIn
            // 
            this.LeadIn.Location = new System.Drawing.Point(179, 170);
            this.LeadIn.Name = "LeadIn";
            this.LeadIn.Size = new System.Drawing.Size(80, 20);
            this.LeadIn.TabIndex = 64;
            this.LeadIn.Text = "0";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(40, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 13);
            this.label1.TabIndex = 61;
            this.label1.Text = "Beats Per Minute";
            // 
            // beatsPerMinute
            // 
            this.beatsPerMinute.Location = new System.Drawing.Point(179, 26);
            this.beatsPerMinute.Name = "beatsPerMinute";
            this.beatsPerMinute.Size = new System.Drawing.Size(82, 20);
            this.beatsPerMinute.TabIndex = 58;
            this.beatsPerMinute.Text = "120";
            // 
            // beatsPerBar
            // 
            this.beatsPerBar.Location = new System.Drawing.Point(179, 74);
            this.beatsPerBar.Name = "beatsPerBar";
            this.beatsPerBar.Size = new System.Drawing.Size(81, 20);
            this.beatsPerBar.TabIndex = 59;
            this.beatsPerBar.Text = "4";
            // 
            // numBars
            // 
            this.numBars.Location = new System.Drawing.Point(179, 122);
            this.numBars.Name = "numBars";
            this.numBars.Size = new System.Drawing.Size(81, 20);
            this.numBars.TabIndex = 60;
            this.numBars.Text = "16";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(56, 81);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 13);
            this.label2.TabIndex = 62;
            this.label2.Text = "Beats Per Bar";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(48, 129);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 13);
            this.label3.TabIndex = 63;
            this.label3.Text = "Number of Bars";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.dynamicCalibrationOption);
            this.groupBox4.Controls.Add(this.systemCalibrationOption);
            this.groupBox4.Controls.Add(this.noCalibrationOption);
            this.groupBox4.Location = new System.Drawing.Point(6, 33);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(383, 170);
            this.groupBox4.TabIndex = 56;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Calibration Options";
            // 
            // dynamicCalibrationOption
            // 
            this.dynamicCalibrationOption.AutoSize = true;
            this.dynamicCalibrationOption.Checked = true;
            this.dynamicCalibrationOption.Location = new System.Drawing.Point(39, 130);
            this.dynamicCalibrationOption.Name = "dynamicCalibrationOption";
            this.dynamicCalibrationOption.Size = new System.Drawing.Size(118, 17);
            this.dynamicCalibrationOption.TabIndex = 2;
            this.dynamicCalibrationOption.TabStop = true;
            this.dynamicCalibrationOption.Text = "Dynamic Calibration";
            this.dynamicCalibrationOption.UseVisualStyleBackColor = true;
            // 
            // systemCalibrationOption
            // 
            this.systemCalibrationOption.AutoSize = true;
            this.systemCalibrationOption.Location = new System.Drawing.Point(39, 80);
            this.systemCalibrationOption.Name = "systemCalibrationOption";
            this.systemCalibrationOption.Size = new System.Drawing.Size(111, 17);
            this.systemCalibrationOption.TabIndex = 1;
            this.systemCalibrationOption.Text = "System Calibration";
            this.systemCalibrationOption.UseVisualStyleBackColor = true;
            // 
            // noCalibrationOption
            // 
            this.noCalibrationOption.AutoSize = true;
            this.noCalibrationOption.Location = new System.Drawing.Point(39, 32);
            this.noCalibrationOption.Name = "noCalibrationOption";
            this.noCalibrationOption.Size = new System.Drawing.Size(91, 17);
            this.noCalibrationOption.TabIndex = 0;
            this.noCalibrationOption.Text = "No Calibration";
            this.noCalibrationOption.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.SystemConfiguration);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(1143, 641);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "General";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // SystemConfiguration
            // 
            this.SystemConfiguration.Controls.Add(this.feedbackOnOption);
            this.SystemConfiguration.Controls.Add(this.chartOneAxisOnly);
            this.SystemConfiguration.Controls.Add(this.chartSkipStepValue);
            this.SystemConfiguration.Controls.Add(this.label31);
            this.SystemConfiguration.Controls.Add(this.soundOption);
            this.SystemConfiguration.Location = new System.Drawing.Point(15, 34);
            this.SystemConfiguration.Name = "SystemConfiguration";
            this.SystemConfiguration.Size = new System.Drawing.Size(497, 212);
            this.SystemConfiguration.TabIndex = 57;
            this.SystemConfiguration.TabStop = false;
            this.SystemConfiguration.Text = "Configuration";
            // 
            // feedbackOnOption
            // 
            this.feedbackOnOption.AutoSize = true;
            this.feedbackOnOption.DataBindings.Add(new System.Windows.Forms.Binding("CheckState", this.configurationBindingSource, "FeedbackOn", true));
            this.feedbackOnOption.DataBindings.Add(new System.Windows.Forms.Binding("Tag", this.configurationBindingSource, "FeedbackOn", true));
            this.feedbackOnOption.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.configurationBindingSource, "FeedbackOn", true));
            this.feedbackOnOption.Location = new System.Drawing.Point(32, 174);
            this.feedbackOnOption.Name = "feedbackOnOption";
            this.feedbackOnOption.Size = new System.Drawing.Size(91, 17);
            this.feedbackOnOption.TabIndex = 8;
            this.feedbackOnOption.Text = "Feedback On";
            this.feedbackOnOption.UseVisualStyleBackColor = true;
            // 
            // chartOneAxisOnly
            // 
            this.chartOneAxisOnly.AutoSize = true;
            this.chartOneAxisOnly.DataBindings.Add(new System.Windows.Forms.Binding("CheckState", this.configurationBindingSource, "WiimotesChartOneAxisOnly", true));
            this.chartOneAxisOnly.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.configurationBindingSource, "WiimotesChartOneAxisOnly", true));
            this.chartOneAxisOnly.DataBindings.Add(new System.Windows.Forms.Binding("Tag", this.configurationBindingSource, "WiimotesChartOneAxisOnly", true));
            this.chartOneAxisOnly.Location = new System.Drawing.Point(31, 102);
            this.chartOneAxisOnly.Name = "chartOneAxisOnly";
            this.chartOneAxisOnly.Size = new System.Drawing.Size(157, 17);
            this.chartOneAxisOnly.TabIndex = 7;
            this.chartOneAxisOnly.Text = "Only One Axis Chart Display";
            this.chartOneAxisOnly.UseVisualStyleBackColor = true;
            // 
            // chartSkipStepValue
            // 
            this.chartSkipStepValue.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.configurationBindingSource, "WiimotesChartRowSkipStep", true));
            this.chartSkipStepValue.DataBindings.Add(new System.Windows.Forms.Binding("Tag", this.configurationBindingSource, "WiimotesChartRowSkipStep", true));
            this.chartSkipStepValue.Location = new System.Drawing.Point(247, 35);
            this.chartSkipStepValue.Name = "chartSkipStepValue";
            this.chartSkipStepValue.Size = new System.Drawing.Size(82, 20);
            this.chartSkipStepValue.TabIndex = 6;
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.Location = new System.Drawing.Point(29, 38);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(125, 13);
            this.label31.TabIndex = 5;
            this.label31.Text = "# Points to Skip on Chart";
            // 
            // soundOption
            // 
            this.soundOption.AutoSize = true;
            this.soundOption.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.configurationBindingSource, "SoundOn", true));
            this.soundOption.DataBindings.Add(new System.Windows.Forms.Binding("Tag", this.configurationBindingSource, "SoundOn", true));
            this.soundOption.DataBindings.Add(new System.Windows.Forms.Binding("CheckState", this.configurationBindingSource, "SoundOn", true));
            this.soundOption.Location = new System.Drawing.Point(31, 143);
            this.soundOption.Name = "soundOption";
            this.soundOption.Size = new System.Drawing.Size(74, 17);
            this.soundOption.TabIndex = 4;
            this.soundOption.Text = "Sound On";
            this.soundOption.UseVisualStyleBackColor = true;
            // 
            // CalibrationTab
            // 
            this.CalibrationTab.Controls.Add(this.VoiceSetupGroup);
            this.CalibrationTab.Controls.Add(this.startCalibration);
            this.CalibrationTab.Controls.Add(this.resetCalibration);
            this.CalibrationTab.Controls.Add(this.calibrationRecordingStatus);
            this.CalibrationTab.Controls.Add(this.RecordStopButton);
            this.CalibrationTab.Controls.Add(this.calibrationGroup);
            this.CalibrationTab.Location = new System.Drawing.Point(4, 22);
            this.CalibrationTab.Name = "CalibrationTab";
            this.CalibrationTab.Padding = new System.Windows.Forms.Padding(3);
            this.CalibrationTab.Size = new System.Drawing.Size(1209, 754);
            this.CalibrationTab.TabIndex = 7;
            this.CalibrationTab.Text = "Calibration";
            this.CalibrationTab.UseVisualStyleBackColor = true;
            // 
            // VoiceSetupGroup
            // 
            this.VoiceSetupGroup.Controls.Add(this.configureMicCommand);
            this.VoiceSetupGroup.Controls.Add(this.voiceCommandsOption);
            this.VoiceSetupGroup.Location = new System.Drawing.Point(35, 437);
            this.VoiceSetupGroup.Name = "VoiceSetupGroup";
            this.VoiceSetupGroup.Size = new System.Drawing.Size(1100, 233);
            this.VoiceSetupGroup.TabIndex = 7;
            this.VoiceSetupGroup.TabStop = false;
            this.VoiceSetupGroup.Text = "Voice Setup";
            // 
            // configureMicCommand
            // 
            this.configureMicCommand.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.configureMicCommand.Location = new System.Drawing.Point(411, 80);
            this.configureMicCommand.Name = "configureMicCommand";
            this.configureMicCommand.Size = new System.Drawing.Size(197, 99);
            this.configureMicCommand.TabIndex = 1;
            this.configureMicCommand.Text = "Configure Mic";
            this.configureMicCommand.UseVisualStyleBackColor = true;
            this.configureMicCommand.Click += new System.EventHandler(this.configureMicCommand_Click);
            // 
            // voiceCommandsOption
            // 
            this.voiceCommandsOption.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.voiceCommandsOption.Location = new System.Drawing.Point(75, 78);
            this.voiceCommandsOption.Name = "voiceCommandsOption";
            this.voiceCommandsOption.Size = new System.Drawing.Size(180, 99);
            this.voiceCommandsOption.TabIndex = 0;
            this.voiceCommandsOption.Text = "Voice Commands On";
            this.voiceCommandsOption.UseVisualStyleBackColor = true;
            this.voiceCommandsOption.Click += new System.EventHandler(this.voiceCommandsOption_Click);
            // 
            // startCalibration
            // 
            this.startCalibration.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.startCalibration.Location = new System.Drawing.Point(110, 216);
            this.startCalibration.Name = "startCalibration";
            this.startCalibration.Size = new System.Drawing.Size(180, 89);
            this.startCalibration.TabIndex = 5;
            this.startCalibration.Text = "Start";
            this.startCalibration.UseVisualStyleBackColor = true;
            this.startCalibration.Click += new System.EventHandler(this.startCalibration_Click);
            // 
            // resetCalibration
            // 
            this.resetCalibration.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.resetCalibration.Location = new System.Drawing.Point(824, 216);
            this.resetCalibration.Name = "resetCalibration";
            this.resetCalibration.Size = new System.Drawing.Size(206, 89);
            this.resetCalibration.TabIndex = 3;
            this.resetCalibration.Text = "Reset";
            this.resetCalibration.UseVisualStyleBackColor = true;
            this.resetCalibration.Click += new System.EventHandler(this.resetCalibration_Click);
            // 
            // calibrationRecordingStatus
            // 
            this.calibrationRecordingStatus.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.calibrationRecordingStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.calibrationRecordingStatus.ForeColor = System.Drawing.Color.Blue;
            this.calibrationRecordingStatus.Location = new System.Drawing.Point(110, 42);
            this.calibrationRecordingStatus.Multiline = true;
            this.calibrationRecordingStatus.Name = "calibrationRecordingStatus";
            this.calibrationRecordingStatus.Size = new System.Drawing.Size(920, 151);
            this.calibrationRecordingStatus.TabIndex = 2;
            this.calibrationRecordingStatus.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // RecordStopButton
            // 
            this.RecordStopButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RecordStopButton.Location = new System.Drawing.Point(446, 216);
            this.RecordStopButton.Name = "RecordStopButton";
            this.RecordStopButton.Size = new System.Drawing.Size(197, 89);
            this.RecordStopButton.TabIndex = 1;
            this.RecordStopButton.Text = "Stop";
            this.RecordStopButton.UseVisualStyleBackColor = true;
            this.RecordStopButton.Click += new System.EventHandler(this.RecordStepButton_Click);
            // 
            // calibrationGroup
            // 
            this.calibrationGroup.Location = new System.Drawing.Point(35, 6);
            this.calibrationGroup.Name = "calibrationGroup";
            this.calibrationGroup.Size = new System.Drawing.Size(1100, 328);
            this.calibrationGroup.TabIndex = 6;
            this.calibrationGroup.TabStop = false;
            this.calibrationGroup.Text = "Gadget Calibration";
            // 
            // BluetoothSetupTab
            // 
            this.BluetoothSetupTab.Location = new System.Drawing.Point(4, 22);
            this.BluetoothSetupTab.Name = "BluetoothSetupTab";
            this.BluetoothSetupTab.Size = new System.Drawing.Size(1209, 754);
            this.BluetoothSetupTab.TabIndex = 10;
            this.BluetoothSetupTab.Text = "BluetoothSetup";
            this.BluetoothSetupTab.UseVisualStyleBackColor = true;
            // 
            // ReferenceData
            // 
            this.ReferenceData.Controls.Add(this.deleteTrainingSegment);
            this.ReferenceData.Controls.Add(this.SaveDataStore);
            this.ReferenceData.Controls.Add(this.newTrainingSegment);
            this.ReferenceData.Controls.Add(this.traingSegmentGrid);
            this.ReferenceData.Controls.Add(this.chartShowOption);
            this.ReferenceData.Controls.Add(this.wiimote2Label);
            this.ReferenceData.Controls.Add(this.wiimote1Label);
            this.ReferenceData.Controls.Add(this.wiimote2Gyro);
            this.ReferenceData.Controls.Add(this.wiimote1Gyro);
            this.ReferenceData.Controls.Add(this.wiimote2AccChart);
            this.ReferenceData.Controls.Add(this.chart1);
            this.ReferenceData.Controls.Add(this.groupBox1);
            this.ReferenceData.Location = new System.Drawing.Point(4, 22);
            this.ReferenceData.Name = "ReferenceData";
            this.ReferenceData.Padding = new System.Windows.Forms.Padding(3);
            this.ReferenceData.Size = new System.Drawing.Size(1209, 754);
            this.ReferenceData.TabIndex = 5;
            this.ReferenceData.Text = "ReferenceData";
            this.ReferenceData.UseVisualStyleBackColor = true;
            // 
            // deleteTrainingSegment
            // 
            this.deleteTrainingSegment.Location = new System.Drawing.Point(212, 264);
            this.deleteTrainingSegment.Name = "deleteTrainingSegment";
            this.deleteTrainingSegment.Size = new System.Drawing.Size(127, 46);
            this.deleteTrainingSegment.TabIndex = 69;
            this.deleteTrainingSegment.Text = "Delete";
            this.deleteTrainingSegment.UseVisualStyleBackColor = true;
            this.deleteTrainingSegment.Click += new System.EventHandler(this.deleteTrainingSegment_Click);
            // 
            // SaveDataStore
            // 
            this.SaveDataStore.Location = new System.Drawing.Point(414, 264);
            this.SaveDataStore.Name = "SaveDataStore";
            this.SaveDataStore.Size = new System.Drawing.Size(138, 47);
            this.SaveDataStore.TabIndex = 68;
            this.SaveDataStore.Text = "Save";
            this.SaveDataStore.UseVisualStyleBackColor = true;
            this.SaveDataStore.Click += new System.EventHandler(this.SaveDataStore_Click);
            // 
            // newTrainingSegment
            // 
            this.newTrainingSegment.Location = new System.Drawing.Point(36, 264);
            this.newTrainingSegment.Name = "newTrainingSegment";
            this.newTrainingSegment.Size = new System.Drawing.Size(148, 47);
            this.newTrainingSegment.TabIndex = 67;
            this.newTrainingSegment.Text = "New";
            this.newTrainingSegment.UseVisualStyleBackColor = true;
            this.newTrainingSegment.Click += new System.EventHandler(this.newTrainingSegment_Click);
            // 
            // traingSegmentGrid
            // 
            this.traingSegmentGrid.AutoGenerateColumns = false;
            this.traingSegmentGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.traingSegmentGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.RecordName,
            this.numBeatsPerMinuteDataGridViewTextBoxColumn,
            this.numBeatsPerBarDataGridViewTextBoxColumn,
            this.numBarsDataGridViewTextBoxColumn,
            this.leadInDataGridViewTextBoxColumn,
            this.recordingIntervalDataGridViewTextBoxColumn,
            this.trainingReferenceRecordDataGridViewTextBoxColumn,
            this.slowMoOptionDataGridViewCheckBoxColumn,
            this.scoringOptionDataGridViewCheckBoxColumn,
            this.trainingPlayRecordDataGridViewTextBoxColumn,
            this.highestRecordingItemIndexDataGridViewTextBoxColumn,
            this.recordIDDataGridViewTextBoxColumn,
            this.recordNameDataGridViewTextBoxColumn1});
            this.traingSegmentGrid.DataSource = this.trainingSegmentInfoBindingSource;
            this.traingSegmentGrid.Location = new System.Drawing.Point(36, 51);
            this.traingSegmentGrid.Name = "traingSegmentGrid";
            this.traingSegmentGrid.Size = new System.Drawing.Size(566, 202);
            this.traingSegmentGrid.TabIndex = 66;
            this.traingSegmentGrid.SelectionChanged += new System.EventHandler(this.traingSegmentGrid_SelectionChanged);
            // 
            // RecordName
            // 
            this.RecordName.DataPropertyName = "RecordName";
            this.RecordName.HeaderText = "RecordName";
            this.RecordName.Name = "RecordName";
            this.RecordName.Width = 300;
            // 
            // numBeatsPerMinuteDataGridViewTextBoxColumn
            // 
            this.numBeatsPerMinuteDataGridViewTextBoxColumn.DataPropertyName = "NumBeatsPerMinute";
            this.numBeatsPerMinuteDataGridViewTextBoxColumn.HeaderText = "NumBeatsPerMinute";
            this.numBeatsPerMinuteDataGridViewTextBoxColumn.Name = "numBeatsPerMinuteDataGridViewTextBoxColumn";
            // 
            // numBeatsPerBarDataGridViewTextBoxColumn
            // 
            this.numBeatsPerBarDataGridViewTextBoxColumn.DataPropertyName = "NumBeatsPerBar";
            this.numBeatsPerBarDataGridViewTextBoxColumn.HeaderText = "NumBeatsPerBar";
            this.numBeatsPerBarDataGridViewTextBoxColumn.Name = "numBeatsPerBarDataGridViewTextBoxColumn";
            // 
            // numBarsDataGridViewTextBoxColumn
            // 
            this.numBarsDataGridViewTextBoxColumn.DataPropertyName = "NumBars";
            this.numBarsDataGridViewTextBoxColumn.HeaderText = "NumBars";
            this.numBarsDataGridViewTextBoxColumn.Name = "numBarsDataGridViewTextBoxColumn";
            // 
            // leadInDataGridViewTextBoxColumn
            // 
            this.leadInDataGridViewTextBoxColumn.DataPropertyName = "LeadIn";
            this.leadInDataGridViewTextBoxColumn.HeaderText = "LeadIn";
            this.leadInDataGridViewTextBoxColumn.Name = "leadInDataGridViewTextBoxColumn";
            // 
            // recordingIntervalDataGridViewTextBoxColumn
            // 
            this.recordingIntervalDataGridViewTextBoxColumn.DataPropertyName = "RecordingInterval";
            this.recordingIntervalDataGridViewTextBoxColumn.HeaderText = "RecordingInterval";
            this.recordingIntervalDataGridViewTextBoxColumn.Name = "recordingIntervalDataGridViewTextBoxColumn";
            // 
            // trainingReferenceRecordDataGridViewTextBoxColumn
            // 
            this.trainingReferenceRecordDataGridViewTextBoxColumn.DataPropertyName = "TrainingReferenceRecord";
            this.trainingReferenceRecordDataGridViewTextBoxColumn.HeaderText = "TrainingReferenceRecord";
            this.trainingReferenceRecordDataGridViewTextBoxColumn.Name = "trainingReferenceRecordDataGridViewTextBoxColumn";
            this.trainingReferenceRecordDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // slowMoOptionDataGridViewCheckBoxColumn
            // 
            this.slowMoOptionDataGridViewCheckBoxColumn.DataPropertyName = "SlowMoOption";
            this.slowMoOptionDataGridViewCheckBoxColumn.HeaderText = "SlowMoOption";
            this.slowMoOptionDataGridViewCheckBoxColumn.Name = "slowMoOptionDataGridViewCheckBoxColumn";
            this.slowMoOptionDataGridViewCheckBoxColumn.ReadOnly = true;
            // 
            // scoringOptionDataGridViewCheckBoxColumn
            // 
            this.scoringOptionDataGridViewCheckBoxColumn.DataPropertyName = "ScoringOption";
            this.scoringOptionDataGridViewCheckBoxColumn.HeaderText = "ScoringOption";
            this.scoringOptionDataGridViewCheckBoxColumn.Name = "scoringOptionDataGridViewCheckBoxColumn";
            this.scoringOptionDataGridViewCheckBoxColumn.ReadOnly = true;
            // 
            // trainingPlayRecordDataGridViewTextBoxColumn
            // 
            this.trainingPlayRecordDataGridViewTextBoxColumn.DataPropertyName = "TrainingPlayRecord";
            this.trainingPlayRecordDataGridViewTextBoxColumn.HeaderText = "TrainingPlayRecord";
            this.trainingPlayRecordDataGridViewTextBoxColumn.Name = "trainingPlayRecordDataGridViewTextBoxColumn";
            // 
            // highestRecordingItemIndexDataGridViewTextBoxColumn
            // 
            this.highestRecordingItemIndexDataGridViewTextBoxColumn.DataPropertyName = "HighestRecordingItemIndex";
            this.highestRecordingItemIndexDataGridViewTextBoxColumn.HeaderText = "HighestRecordingItemIndex";
            this.highestRecordingItemIndexDataGridViewTextBoxColumn.Name = "highestRecordingItemIndexDataGridViewTextBoxColumn";
            // 
            // recordIDDataGridViewTextBoxColumn
            // 
            this.recordIDDataGridViewTextBoxColumn.DataPropertyName = "RecordID";
            this.recordIDDataGridViewTextBoxColumn.HeaderText = "RecordID";
            this.recordIDDataGridViewTextBoxColumn.Name = "recordIDDataGridViewTextBoxColumn";
            // 
            // recordNameDataGridViewTextBoxColumn1
            // 
            this.recordNameDataGridViewTextBoxColumn1.DataPropertyName = "RecordName";
            this.recordNameDataGridViewTextBoxColumn1.HeaderText = "RecordName";
            this.recordNameDataGridViewTextBoxColumn1.Name = "recordNameDataGridViewTextBoxColumn1";
            // 
            // trainingSegmentInfoBindingSource
            // 
            this.trainingSegmentInfoBindingSource.DataSource = typeof(WiimoteData.TrainingSegmentInfo);
            this.trainingSegmentInfoBindingSource.CurrentChanged += new System.EventHandler(this.trainingSegmentInfoBindingSource_CurrentChanged);
            // 
            // chartShowOption
            // 
            this.chartShowOption.AutoSize = true;
            this.chartShowOption.Location = new System.Drawing.Point(695, 324);
            this.chartShowOption.Name = "chartShowOption";
            this.chartShowOption.Size = new System.Drawing.Size(86, 17);
            this.chartShowOption.TabIndex = 65;
            this.chartShowOption.Text = "Show Charts";
            this.chartShowOption.UseVisualStyleBackColor = true;
            this.chartShowOption.CheckedChanged += new System.EventHandler(this.chartShowOption_CheckedChanged);
            // 
            // wiimote2Label
            // 
            this.wiimote2Label.AutoSize = true;
            this.wiimote2Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wiimote2Label.Location = new System.Drawing.Point(1037, 7);
            this.wiimote2Label.Name = "wiimote2Label";
            this.wiimote2Label.Size = new System.Drawing.Size(71, 13);
            this.wiimote2Label.TabIndex = 64;
            this.wiimote2Label.Text = "Wiimote #2";
            // 
            // wiimote1Label
            // 
            this.wiimote1Label.AutoSize = true;
            this.wiimote1Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wiimote1Label.Location = new System.Drawing.Point(722, 6);
            this.wiimote1Label.Name = "wiimote1Label";
            this.wiimote1Label.Size = new System.Drawing.Size(71, 13);
            this.wiimote1Label.TabIndex = 63;
            this.wiimote1Label.Text = "Wiimote #1";
            // 
            // wiimote2Gyro
            // 
            chartArea1.AxisX.IsLabelAutoFit = false;
            chartArea1.AxisX.LabelStyle.Font = new System.Drawing.Font("Arial", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            chartArea1.AxisY.IsLabelAutoFit = false;
            chartArea1.AxisY.LabelStyle.Font = new System.Drawing.Font("Arial", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            chartArea1.Name = "ChartArea1";
            this.wiimote2Gyro.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.wiimote2Gyro.Legends.Add(legend1);
            this.wiimote2Gyro.Location = new System.Drawing.Point(929, 185);
            this.wiimote2Gyro.Name = "wiimote2Gyro";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastPoint;
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastPoint;
            series2.Legend = "Legend1";
            series2.Name = "Series2";
            series3.ChartArea = "ChartArea1";
            series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastPoint;
            series3.Legend = "Legend1";
            series3.Name = "Series3";
            this.wiimote2Gyro.Series.Add(series1);
            this.wiimote2Gyro.Series.Add(series2);
            this.wiimote2Gyro.Series.Add(series3);
            this.wiimote2Gyro.Size = new System.Drawing.Size(274, 126);
            this.wiimote2Gyro.TabIndex = 62;
            this.wiimote2Gyro.Text = "wiimote2Gyro";
            // 
            // wiimote1Gyro
            // 
            chartArea2.AxisX.IsLabelAutoFit = false;
            chartArea2.AxisX.LabelStyle.Font = new System.Drawing.Font("Arial", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            chartArea2.AxisY.IsLabelAutoFit = false;
            chartArea2.AxisY.LabelStyle.Font = new System.Drawing.Font("Arial", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            chartArea2.Name = "ChartArea1";
            this.wiimote1Gyro.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            this.wiimote1Gyro.Legends.Add(legend2);
            this.wiimote1Gyro.Location = new System.Drawing.Point(695, 186);
            this.wiimote1Gyro.Name = "wiimote1Gyro";
            series4.ChartArea = "ChartArea1";
            series4.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastPoint;
            series4.Legend = "Legend1";
            series4.Name = "Series1";
            series5.ChartArea = "ChartArea1";
            series5.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastPoint;
            series5.Legend = "Legend1";
            series5.Name = "Series2";
            series6.ChartArea = "ChartArea1";
            series6.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastPoint;
            series6.Legend = "Legend1";
            series6.Name = "Series3";
            this.wiimote1Gyro.Series.Add(series4);
            this.wiimote1Gyro.Series.Add(series5);
            this.wiimote1Gyro.Series.Add(series6);
            this.wiimote1Gyro.Size = new System.Drawing.Size(228, 125);
            this.wiimote1Gyro.TabIndex = 61;
            this.wiimote1Gyro.Text = "wiimote1Gyro";
            // 
            // wiimote2AccChart
            // 
            chartArea3.AxisX.IsLabelAutoFit = false;
            chartArea3.AxisX.LabelStyle.Font = new System.Drawing.Font("Arial", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            chartArea3.AxisX.ScaleView.MinSizeType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Seconds;
            chartArea3.AxisX.ScaleView.SizeType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Milliseconds;
            chartArea3.AxisX.ScaleView.SmallScrollMinSizeType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Seconds;
            chartArea3.AxisY.IsLabelAutoFit = false;
            chartArea3.AxisY.LabelStyle.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            chartArea3.Name = "ChartArea1";
            this.wiimote2AccChart.ChartAreas.Add(chartArea3);
            legend3.Name = "Legend1";
            this.wiimote2AccChart.Legends.Add(legend3);
            this.wiimote2AccChart.Location = new System.Drawing.Point(930, 37);
            this.wiimote2AccChart.Name = "wiimote2AccChart";
            series7.ChartArea = "ChartArea1";
            series7.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastPoint;
            series7.Legend = "Legend1";
            series7.Name = "Series1";
            series8.ChartArea = "ChartArea1";
            series8.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastPoint;
            series8.Legend = "Legend1";
            series8.Name = "Series2";
            series9.ChartArea = "ChartArea1";
            series9.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastPoint;
            series9.Legend = "Legend1";
            series9.Name = "Series3";
            this.wiimote2AccChart.Series.Add(series7);
            this.wiimote2AccChart.Series.Add(series8);
            this.wiimote2AccChart.Series.Add(series9);
            this.wiimote2AccChart.Size = new System.Drawing.Size(274, 123);
            this.wiimote2AccChart.TabIndex = 60;
            this.wiimote2AccChart.Text = "wiimote2AccChart";
            // 
            // chart1
            // 
            chartArea4.AxisX.IsLabelAutoFit = false;
            chartArea4.AxisX.LabelStyle.Font = new System.Drawing.Font("Arial", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            chartArea4.AxisY.IsLabelAutoFit = false;
            chartArea4.AxisY.LabelStyle.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            chartArea4.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea4);
            legend4.Name = "Legend1";
            this.chart1.Legends.Add(legend4);
            this.chart1.Location = new System.Drawing.Point(695, 36);
            this.chart1.Name = "chart1";
            series10.ChartArea = "ChartArea1";
            series10.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastPoint;
            series10.Legend = "Legend1";
            series10.Name = "Series1";
            series11.ChartArea = "ChartArea1";
            series11.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastPoint;
            series11.Legend = "Legend1";
            series11.Name = "Series2";
            series12.ChartArea = "ChartArea1";
            series12.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastPoint;
            series12.Legend = "Legend1";
            series12.Name = "Series3";
            this.chart1.Series.Add(series10);
            this.chart1.Series.Add(series11);
            this.chart1.Series.Add(series12);
            this.chart1.Size = new System.Drawing.Size(229, 124);
            this.chart1.TabIndex = 59;
            this.chart1.Text = "chart1";
            this.chart1.Click += new System.EventHandler(this.chart1_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.MoveDownCommand);
            this.groupBox1.Controls.Add(this.MoveUpCommand);
            this.groupBox1.Controls.Add(this.deleteReferenceRecordingItemButton);
            this.groupBox1.Controls.Add(this.newReferenceRecordItem);
            this.groupBox1.Controls.Add(this.recordingItemGridView4);
            this.groupBox1.Controls.Add(this.NewReference);
            this.groupBox1.Controls.Add(this.UploadReference);
            this.groupBox1.Controls.Add(this.DeleteRecord);
            this.groupBox1.Controls.Add(this.dataGridView3);
            this.groupBox1.Controls.Add(this.RecordReference);
            this.groupBox1.Location = new System.Drawing.Point(36, 347);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1136, 399);
            this.groupBox1.TabIndex = 57;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Reference Tap Recording";
            // 
            // MoveDownCommand
            // 
            this.MoveDownCommand.BackgroundImage = global::MainGUI.Properties.Resources.DownArrow;
            this.MoveDownCommand.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.MoveDownCommand.Location = new System.Drawing.Point(617, 126);
            this.MoveDownCommand.Name = "MoveDownCommand";
            this.MoveDownCommand.Size = new System.Drawing.Size(44, 38);
            this.MoveDownCommand.TabIndex = 59;
            this.MoveDownCommand.UseVisualStyleBackColor = true;
            this.MoveDownCommand.Click += new System.EventHandler(this.MoveDownCommand_Click);
            // 
            // MoveUpCommand
            // 
            this.MoveUpCommand.BackgroundImage = global::MainGUI.Properties.Resources.UpArrow;
            this.MoveUpCommand.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.MoveUpCommand.Location = new System.Drawing.Point(617, 75);
            this.MoveUpCommand.Name = "MoveUpCommand";
            this.MoveUpCommand.Size = new System.Drawing.Size(44, 36);
            this.MoveUpCommand.TabIndex = 58;
            this.MoveUpCommand.UseVisualStyleBackColor = true;
            this.MoveUpCommand.Click += new System.EventHandler(this.MoveUpCommand_Click);
            // 
            // deleteReferenceRecordingItemButton
            // 
            this.deleteReferenceRecordingItemButton.Location = new System.Drawing.Point(1014, 352);
            this.deleteReferenceRecordingItemButton.Name = "deleteReferenceRecordingItemButton";
            this.deleteReferenceRecordingItemButton.Size = new System.Drawing.Size(107, 32);
            this.deleteReferenceRecordingItemButton.TabIndex = 57;
            this.deleteReferenceRecordingItemButton.Text = "Delete Record";
            this.deleteReferenceRecordingItemButton.UseVisualStyleBackColor = true;
            this.deleteReferenceRecordingItemButton.Click += new System.EventHandler(this.deleteReferenceRecordingItemButton_Click);
            // 
            // newReferenceRecordItem
            // 
            this.newReferenceRecordItem.Location = new System.Drawing.Point(689, 352);
            this.newReferenceRecordItem.Name = "newReferenceRecordItem";
            this.newReferenceRecordItem.Size = new System.Drawing.Size(103, 31);
            this.newReferenceRecordItem.TabIndex = 56;
            this.newReferenceRecordItem.Text = "New";
            this.newReferenceRecordItem.UseVisualStyleBackColor = true;
            this.newReferenceRecordItem.Click += new System.EventHandler(this.newReferenceRecordItem_Click);
            // 
            // recordingItemGridView4
            // 
            this.recordingItemGridView4.AutoGenerateColumns = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.recordingItemGridView4.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.recordingItemGridView4.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.recordingItemGridView4.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.RecordingDone,
            this.recordNameDataGridViewTextBoxColumn,
            this.dataGridViewTextBoxColumn18});
            this.recordingItemGridView4.DataSource = this.referenceRecordingItemsBindingSource;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.recordingItemGridView4.DefaultCellStyle = dataGridViewCellStyle2;
            this.recordingItemGridView4.Location = new System.Drawing.Point(689, 29);
            this.recordingItemGridView4.MultiSelect = false;
            this.recordingItemGridView4.Name = "recordingItemGridView4";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.recordingItemGridView4.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.recordingItemGridView4.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.recordingItemGridView4.Size = new System.Drawing.Size(432, 306);
            this.recordingItemGridView4.TabIndex = 55;
            this.recordingItemGridView4.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.recordingItemGridView4_CellEndEdit);
            this.recordingItemGridView4.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this._DoNothing);
            this.recordingItemGridView4.SelectionChanged += new System.EventHandler(this.recordingItemGridView4_SelectionChanged);
            // 
            // Column1
            // 
            this.Column1.DataPropertyName = "IsSelectedRecord";
            this.Column1.HeaderText = "Use";
            this.Column1.Name = "Column1";
            this.Column1.Width = 50;
            // 
            // RecordingDone
            // 
            this.RecordingDone.DataPropertyName = "RecordingDone";
            this.RecordingDone.HeaderText = "Recorded";
            this.RecordingDone.Name = "RecordingDone";
            this.RecordingDone.ReadOnly = true;
            this.RecordingDone.Width = 60;
            // 
            // recordNameDataGridViewTextBoxColumn
            // 
            this.recordNameDataGridViewTextBoxColumn.DataPropertyName = "RecordName";
            this.recordNameDataGridViewTextBoxColumn.HeaderText = "RecordName";
            this.recordNameDataGridViewTextBoxColumn.Name = "recordNameDataGridViewTextBoxColumn";
            this.recordNameDataGridViewTextBoxColumn.Width = 150;
            // 
            // dataGridViewTextBoxColumn18
            // 
            this.dataGridViewTextBoxColumn18.DataPropertyName = "RecordedTime";
            this.dataGridViewTextBoxColumn18.HeaderText = "RecordedTime";
            this.dataGridViewTextBoxColumn18.Name = "dataGridViewTextBoxColumn18";
            this.dataGridViewTextBoxColumn18.Width = 150;
            // 
            // referenceRecordingItemsBindingSource
            // 
            this.referenceRecordingItemsBindingSource.DataMember = "ReferenceRecordingItems";
            this.referenceRecordingItemsBindingSource.DataSource = this.wiimoteReferenceRecordBindingSource;
            // 
            // wiimoteReferenceRecordBindingSource
            // 
            this.wiimoteReferenceRecordBindingSource.DataSource = typeof(WiimoteData.WiimoteReferenceRecord);
            // 
            // NewReference
            // 
            this.NewReference.Location = new System.Drawing.Point(25, 352);
            this.NewReference.Name = "NewReference";
            this.NewReference.Size = new System.Drawing.Size(112, 31);
            this.NewReference.TabIndex = 54;
            this.NewReference.Text = "New";
            this.NewReference.UseVisualStyleBackColor = true;
            this.NewReference.Click += new System.EventHandler(this.NewReference_Click);
            // 
            // UploadReference
            // 
            this.UploadReference.Location = new System.Drawing.Point(913, 352);
            this.UploadReference.Name = "UploadReference";
            this.UploadReference.Size = new System.Drawing.Size(95, 31);
            this.UploadReference.TabIndex = 53;
            this.UploadReference.Text = "Upload";
            this.UploadReference.UseVisualStyleBackColor = true;
            this.UploadReference.Click += new System.EventHandler(this.UploadReference_Click);
            // 
            // DeleteRecord
            // 
            this.DeleteRecord.Location = new System.Drawing.Point(166, 352);
            this.DeleteRecord.Name = "DeleteRecord";
            this.DeleteRecord.Size = new System.Drawing.Size(115, 34);
            this.DeleteRecord.TabIndex = 52;
            this.DeleteRecord.Text = "Delete Record";
            this.DeleteRecord.UseVisualStyleBackColor = true;
            this.DeleteRecord.Click += new System.EventHandler(this.DeleteRecord_Click);
            // 
            // dataGridView3
            // 
            this.dataGridView3.AllowUserToAddRows = false;
            this.dataGridView3.AutoGenerateColumns = false;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView3.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridView3.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView3.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn9,
            this.VideoPath,
            this.SlowMoOption,
            this.ScoringOption,
            this.RepeatOption,
            this.parentRecordDataGridViewTextBoxColumn,
            this.parentTrainingSegmentInfoDataGridViewTextBoxColumn,
            this.slowMoOptionDataGridViewCheckBoxColumn1,
            this.scoringOptionDataGridViewCheckBoxColumn1,
            this.repeatOptionDataGridViewCheckBoxColumn,
            this.wiimoteRecordingDelayDataGridViewTextBoxColumn,
            this.videoPathDataGridViewTextBoxColumn,
            this.filePathDataGridViewTextBoxColumn,
            this.recordedTimeDataGridViewTextBoxColumn,
            this.recordingDoneDataGridViewCheckBoxColumn,
            this.highestRecordingItemIndexDataGridViewTextBoxColumn1,
            this.recordIDDataGridViewTextBoxColumn1,
            this.recordNameDataGridViewTextBoxColumn2});
            this.dataGridView3.DataSource = this.wiimoteReferenceDataStoreBindingSource;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView3.DefaultCellStyle = dataGridViewCellStyle5;
            this.dataGridView3.Location = new System.Drawing.Point(25, 29);
            this.dataGridView3.MultiSelect = false;
            this.dataGridView3.Name = "dataGridView3";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView3.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.dataGridView3.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView3.Size = new System.Drawing.Size(586, 306);
            this.dataGridView3.TabIndex = 51;
            this.dataGridView3.SelectionChanged += new System.EventHandler(this.dataGridView3_SelectionChanged);
            // 
            // dataGridViewTextBoxColumn9
            // 
            this.dataGridViewTextBoxColumn9.DataPropertyName = "RecordName";
            this.dataGridViewTextBoxColumn9.HeaderText = "RecordName";
            this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
            this.dataGridViewTextBoxColumn9.Width = 150;
            // 
            // VideoPath
            // 
            this.VideoPath.DataPropertyName = "VideoPath";
            this.VideoPath.HeaderText = "VideoPath";
            this.VideoPath.Name = "VideoPath";
            this.VideoPath.Width = 150;
            // 
            // SlowMoOption
            // 
            this.SlowMoOption.DataPropertyName = "SlowMoOption";
            this.SlowMoOption.HeaderText = "SlowMo";
            this.SlowMoOption.Name = "SlowMoOption";
            this.SlowMoOption.Width = 75;
            // 
            // ScoringOption
            // 
            this.ScoringOption.DataPropertyName = "ScoringOption";
            this.ScoringOption.HeaderText = "Scoring";
            this.ScoringOption.Name = "ScoringOption";
            this.ScoringOption.Width = 75;
            // 
            // RepeatOption
            // 
            this.RepeatOption.DataPropertyName = "RepeatOption";
            this.RepeatOption.HeaderText = "Repeat";
            this.RepeatOption.Name = "RepeatOption";
            this.RepeatOption.Width = 75;
            // 
            // parentRecordDataGridViewTextBoxColumn
            // 
            this.parentRecordDataGridViewTextBoxColumn.DataPropertyName = "ParentRecord";
            this.parentRecordDataGridViewTextBoxColumn.HeaderText = "ParentRecord";
            this.parentRecordDataGridViewTextBoxColumn.Name = "parentRecordDataGridViewTextBoxColumn";
            // 
            // parentTrainingSegmentInfoDataGridViewTextBoxColumn
            // 
            this.parentTrainingSegmentInfoDataGridViewTextBoxColumn.DataPropertyName = "ParentTrainingSegmentInfo";
            this.parentTrainingSegmentInfoDataGridViewTextBoxColumn.HeaderText = "ParentTrainingSegmentInfo";
            this.parentTrainingSegmentInfoDataGridViewTextBoxColumn.Name = "parentTrainingSegmentInfoDataGridViewTextBoxColumn";
            // 
            // slowMoOptionDataGridViewCheckBoxColumn1
            // 
            this.slowMoOptionDataGridViewCheckBoxColumn1.DataPropertyName = "SlowMoOption";
            this.slowMoOptionDataGridViewCheckBoxColumn1.HeaderText = "SlowMoOption";
            this.slowMoOptionDataGridViewCheckBoxColumn1.Name = "slowMoOptionDataGridViewCheckBoxColumn1";
            // 
            // scoringOptionDataGridViewCheckBoxColumn1
            // 
            this.scoringOptionDataGridViewCheckBoxColumn1.DataPropertyName = "ScoringOption";
            this.scoringOptionDataGridViewCheckBoxColumn1.HeaderText = "ScoringOption";
            this.scoringOptionDataGridViewCheckBoxColumn1.Name = "scoringOptionDataGridViewCheckBoxColumn1";
            // 
            // repeatOptionDataGridViewCheckBoxColumn
            // 
            this.repeatOptionDataGridViewCheckBoxColumn.DataPropertyName = "RepeatOption";
            this.repeatOptionDataGridViewCheckBoxColumn.HeaderText = "RepeatOption";
            this.repeatOptionDataGridViewCheckBoxColumn.Name = "repeatOptionDataGridViewCheckBoxColumn";
            // 
            // wiimoteRecordingDelayDataGridViewTextBoxColumn
            // 
            this.wiimoteRecordingDelayDataGridViewTextBoxColumn.DataPropertyName = "WiimoteRecordingDelay";
            this.wiimoteRecordingDelayDataGridViewTextBoxColumn.HeaderText = "WiimoteRecordingDelay";
            this.wiimoteRecordingDelayDataGridViewTextBoxColumn.Name = "wiimoteRecordingDelayDataGridViewTextBoxColumn";
            // 
            // videoPathDataGridViewTextBoxColumn
            // 
            this.videoPathDataGridViewTextBoxColumn.DataPropertyName = "VideoPath";
            this.videoPathDataGridViewTextBoxColumn.HeaderText = "VideoPath";
            this.videoPathDataGridViewTextBoxColumn.Name = "videoPathDataGridViewTextBoxColumn";
            // 
            // filePathDataGridViewTextBoxColumn
            // 
            this.filePathDataGridViewTextBoxColumn.DataPropertyName = "FilePath";
            this.filePathDataGridViewTextBoxColumn.HeaderText = "FilePath";
            this.filePathDataGridViewTextBoxColumn.Name = "filePathDataGridViewTextBoxColumn";
            this.filePathDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // recordedTimeDataGridViewTextBoxColumn
            // 
            this.recordedTimeDataGridViewTextBoxColumn.DataPropertyName = "RecordedTime";
            this.recordedTimeDataGridViewTextBoxColumn.HeaderText = "RecordedTime";
            this.recordedTimeDataGridViewTextBoxColumn.Name = "recordedTimeDataGridViewTextBoxColumn";
            // 
            // recordingDoneDataGridViewCheckBoxColumn
            // 
            this.recordingDoneDataGridViewCheckBoxColumn.DataPropertyName = "RecordingDone";
            this.recordingDoneDataGridViewCheckBoxColumn.HeaderText = "RecordingDone";
            this.recordingDoneDataGridViewCheckBoxColumn.Name = "recordingDoneDataGridViewCheckBoxColumn";
            // 
            // highestRecordingItemIndexDataGridViewTextBoxColumn1
            // 
            this.highestRecordingItemIndexDataGridViewTextBoxColumn1.DataPropertyName = "HighestRecordingItemIndex";
            this.highestRecordingItemIndexDataGridViewTextBoxColumn1.HeaderText = "HighestRecordingItemIndex";
            this.highestRecordingItemIndexDataGridViewTextBoxColumn1.Name = "highestRecordingItemIndexDataGridViewTextBoxColumn1";
            // 
            // recordIDDataGridViewTextBoxColumn1
            // 
            this.recordIDDataGridViewTextBoxColumn1.DataPropertyName = "RecordID";
            this.recordIDDataGridViewTextBoxColumn1.HeaderText = "RecordID";
            this.recordIDDataGridViewTextBoxColumn1.Name = "recordIDDataGridViewTextBoxColumn1";
            // 
            // recordNameDataGridViewTextBoxColumn2
            // 
            this.recordNameDataGridViewTextBoxColumn2.DataPropertyName = "RecordName";
            this.recordNameDataGridViewTextBoxColumn2.HeaderText = "RecordName";
            this.recordNameDataGridViewTextBoxColumn2.Name = "recordNameDataGridViewTextBoxColumn2";
            // 
            // wiimoteReferenceDataStoreBindingSource
            // 
            this.wiimoteReferenceDataStoreBindingSource.DataMember = "WiimoteReferenceRecords";
            this.wiimoteReferenceDataStoreBindingSource.DataSource = typeof(WiimoteData.TrainingSegmentInfo);
            // 
            // RecordReference
            // 
            this.RecordReference.Location = new System.Drawing.Point(798, 352);
            this.RecordReference.Name = "RecordReference";
            this.RecordReference.Size = new System.Drawing.Size(109, 31);
            this.RecordReference.TabIndex = 41;
            this.RecordReference.Text = "Record Reference";
            this.RecordReference.UseVisualStyleBackColor = true;
            this.RecordReference.Click += new System.EventHandler(this.RecordReference_Click);
            // 
            // WiimoteSensorData
            // 
            this.WiimoteSensorData.Controls.Add(this.SessionID);
            this.WiimoteSensorData.Controls.Add(this.WiimoteNumber);
            this.WiimoteSensorData.Controls.Add(this.EventTIme);
            this.WiimoteSensorData.Controls.Add(this.AccX);
            this.WiimoteSensorData.Controls.Add(this.label17);
            this.WiimoteSensorData.Controls.Add(this.PitchValue);
            this.WiimoteSensorData.Controls.Add(this.label16);
            this.WiimoteSensorData.Controls.Add(this.RollValue);
            this.WiimoteSensorData.Controls.Add(this.label15);
            this.WiimoteSensorData.Controls.Add(this.AccY);
            this.WiimoteSensorData.Controls.Add(this.label14);
            this.WiimoteSensorData.Controls.Add(this.AccZ);
            this.WiimoteSensorData.Controls.Add(this.label13);
            this.WiimoteSensorData.Controls.Add(this.RawPitch);
            this.WiimoteSensorData.Controls.Add(this.label12);
            this.WiimoteSensorData.Controls.Add(this.RawRoll);
            this.WiimoteSensorData.Controls.Add(this.label11);
            this.WiimoteSensorData.Controls.Add(this.RawYaw);
            this.WiimoteSensorData.Controls.Add(this.label10);
            this.WiimoteSensorData.Controls.Add(this.SpeedRoll);
            this.WiimoteSensorData.Controls.Add(this.label9);
            this.WiimoteSensorData.Controls.Add(this.SpeedPitch);
            this.WiimoteSensorData.Controls.Add(this.label8);
            this.WiimoteSensorData.Controls.Add(this.SpeedYaw);
            this.WiimoteSensorData.Controls.Add(this.label7);
            this.WiimoteSensorData.Controls.Add(this.label4);
            this.WiimoteSensorData.Controls.Add(this.label6);
            this.WiimoteSensorData.Controls.Add(this.label5);
            this.WiimoteSensorData.Location = new System.Drawing.Point(4, 22);
            this.WiimoteSensorData.Name = "WiimoteSensorData";
            this.WiimoteSensorData.Padding = new System.Windows.Forms.Padding(3);
            this.WiimoteSensorData.Size = new System.Drawing.Size(1209, 754);
            this.WiimoteSensorData.TabIndex = 1;
            this.WiimoteSensorData.Text = "WiimoteSensorData";
            this.WiimoteSensorData.UseVisualStyleBackColor = true;
            // 
            // WiimoteLog
            // 
            this.WiimoteLog.Controls.Add(this.TUIOLog);
            this.WiimoteLog.Controls.Add(this.label18);
            this.WiimoteLog.Location = new System.Drawing.Point(4, 22);
            this.WiimoteLog.Name = "WiimoteLog";
            this.WiimoteLog.Padding = new System.Windows.Forms.Padding(3);
            this.WiimoteLog.Size = new System.Drawing.Size(1209, 754);
            this.WiimoteLog.TabIndex = 2;
            this.WiimoteLog.Text = "WiimoteLog";
            this.WiimoteLog.UseVisualStyleBackColor = true;
            // 
            // TimerSettings
            // 
            this.TimerSettings.Controls.Add(this.TestTimer);
            this.TimerSettings.Controls.Add(this.BeatLog);
            this.TimerSettings.Location = new System.Drawing.Point(4, 22);
            this.TimerSettings.Name = "TimerSettings";
            this.TimerSettings.Padding = new System.Windows.Forms.Padding(3);
            this.TimerSettings.Size = new System.Drawing.Size(1209, 754);
            this.TimerSettings.TabIndex = 3;
            this.TimerSettings.Text = "TimerSettings";
            this.TimerSettings.UseVisualStyleBackColor = true;
            // 
            // TestTimer
            // 
            this.TestTimer.Location = new System.Drawing.Point(657, 51);
            this.TestTimer.Name = "TestTimer";
            this.TestTimer.Size = new System.Drawing.Size(126, 23);
            this.TestTimer.TabIndex = 39;
            this.TestTimer.Text = "StartTimer";
            this.TestTimer.UseVisualStyleBackColor = true;
            this.TestTimer.Click += new System.EventHandler(this.TestTimer_Click);
            // 
            // BeatLog
            // 
            this.BeatLog.Location = new System.Drawing.Point(319, 53);
            this.BeatLog.Multiline = true;
            this.BeatLog.Name = "BeatLog";
            this.BeatLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.BeatLog.Size = new System.Drawing.Size(303, 98);
            this.BeatLog.TabIndex = 38;
            // 
            // TestPage
            // 
            this.TestPage.Controls.Add(this.label32);
            this.TestPage.Controls.Add(this.matlabReturnValue);
            this.TestPage.Controls.Add(this.Test);
            this.TestPage.Controls.Add(this.TestPlay);
            this.TestPage.Location = new System.Drawing.Point(4, 22);
            this.TestPage.Name = "TestPage";
            this.TestPage.Padding = new System.Windows.Forms.Padding(3);
            this.TestPage.Size = new System.Drawing.Size(1209, 754);
            this.TestPage.TabIndex = 4;
            this.TestPage.Text = "TestPage";
            this.TestPage.UseVisualStyleBackColor = true;
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.Location = new System.Drawing.Point(44, 153);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(74, 13);
            this.label32.TabIndex = 64;
            this.label32.Text = "Matlab Return";
            // 
            // matlabReturnValue
            // 
            this.matlabReturnValue.Location = new System.Drawing.Point(157, 150);
            this.matlabReturnValue.Multiline = true;
            this.matlabReturnValue.Name = "matlabReturnValue";
            this.matlabReturnValue.Size = new System.Drawing.Size(229, 63);
            this.matlabReturnValue.TabIndex = 63;
            // 
            // Test
            // 
            this.Test.Location = new System.Drawing.Point(154, 53);
            this.Test.Name = "Test";
            this.Test.Size = new System.Drawing.Size(87, 34);
            this.Test.TabIndex = 62;
            this.Test.Text = "Test";
            this.Test.UseVisualStyleBackColor = true;
            this.Test.Click += new System.EventHandler(this.Test_Click_1);
            // 
            // TestPlay
            // 
            this.TestPlay.Location = new System.Drawing.Point(37, 55);
            this.TestPlay.Name = "TestPlay";
            this.TestPlay.Size = new System.Drawing.Size(62, 30);
            this.TestPlay.TabIndex = 61;
            this.TestPlay.Text = "TestPlay";
            this.TestPlay.UseVisualStyleBackColor = true;
            this.TestPlay.Click += new System.EventHandler(this.TestPlay_Click_1);
            // 
            // Training
            // 
            this.Training.Controls.Add(this.MogrePropertyView);
            this.Training.Controls.Add(this.loadMogre);
            this.Training.Controls.Add(this.ogrePanel);
            this.Training.Location = new System.Drawing.Point(4, 22);
            this.Training.Name = "Training";
            this.Training.Padding = new System.Windows.Forms.Padding(3);
            this.Training.Size = new System.Drawing.Size(1209, 754);
            this.Training.TabIndex = 9;
            this.Training.Text = "Training";
            this.Training.UseVisualStyleBackColor = true;
            // 
            // MogrePropertyView
            // 
            this.MogrePropertyView.AutoGenerateColumns = false;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.MogrePropertyView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.MogrePropertyView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.MogrePropertyView.DataSource = this.mogreWrapperMainBindingSource;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.MogrePropertyView.DefaultCellStyle = dataGridViewCellStyle8;
            this.MogrePropertyView.Location = new System.Drawing.Point(1014, 92);
            this.MogrePropertyView.Name = "MogrePropertyView";
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.MogrePropertyView.RowHeadersDefaultCellStyle = dataGridViewCellStyle9;
            this.MogrePropertyView.RowHeadersVisible = false;
            this.MogrePropertyView.Size = new System.Drawing.Size(189, 127);
            this.MogrePropertyView.TabIndex = 2;
            this.MogrePropertyView.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.mogrePropertyValueChanged);
            // 
            // mogreWrapperMainBindingSource
            // 
            this.mogreWrapperMainBindingSource.DataMember = "MogrePropertyList";
            // 
            // loadMogre
            // 
            this.loadMogre.Location = new System.Drawing.Point(1043, 18);
            this.loadMogre.Name = "loadMogre";
            this.loadMogre.Size = new System.Drawing.Size(109, 45);
            this.loadMogre.TabIndex = 1;
            this.loadMogre.Text = "Load";
            this.loadMogre.UseVisualStyleBackColor = true;
            this.loadMogre.Click += new System.EventHandler(this.loadMogre_Click);
            // 
            // ogrePanel
            // 
            this.ogrePanel.Location = new System.Drawing.Point(17, 18);
            this.ogrePanel.Name = "ogrePanel";
            this.ogrePanel.Size = new System.Drawing.Size(973, 643);
            this.ogrePanel.TabIndex = 0;
            // 
            // xnaWindow
            // 
            this.xnaWindow.Controls.Add(this.stopReplayButton);
            this.xnaWindow.Controls.Add(this.readDataButton);
            this.xnaWindow.Controls.Add(this.quartenionViewerControl);
            this.xnaWindow.Location = new System.Drawing.Point(4, 22);
            this.xnaWindow.Name = "xnaWindow";
            this.xnaWindow.Padding = new System.Windows.Forms.Padding(3);
            this.xnaWindow.Size = new System.Drawing.Size(1209, 754);
            this.xnaWindow.TabIndex = 11;
            this.xnaWindow.Text = "XNA";
            this.xnaWindow.UseVisualStyleBackColor = true;
            // 
            // stopReplayButton
            // 
            this.stopReplayButton.Location = new System.Drawing.Point(272, 610);
            this.stopReplayButton.Name = "stopReplayButton";
            this.stopReplayButton.Size = new System.Drawing.Size(114, 41);
            this.stopReplayButton.TabIndex = 2;
            this.stopReplayButton.Text = "Stop";
            this.stopReplayButton.UseVisualStyleBackColor = true;
            this.stopReplayButton.Click += new System.EventHandler(this.stopReplayButton_Click);
            // 
            // readDataButton
            // 
            this.readDataButton.Location = new System.Drawing.Point(120, 610);
            this.readDataButton.Name = "readDataButton";
            this.readDataButton.Size = new System.Drawing.Size(109, 41);
            this.readDataButton.TabIndex = 1;
            this.readDataButton.Text = "Show";
            this.readDataButton.UseVisualStyleBackColor = true;
            this.readDataButton.Click += new System.EventHandler(this.readDataButton_Click);
            // 
            // quartenionViewerControl
            // 
            this.quartenionViewerControl.Location = new System.Drawing.Point(120, 73);
            this.quartenionViewerControl.Name = "quartenionViewerControl";
            this.quartenionViewerControl.Size = new System.Drawing.Size(843, 501);
            this.quartenionViewerControl.TabIndex = 0;
            this.quartenionViewerControl.Text = "Quaternion Window";
            // 
            // RecordingStatus
            // 
            this.RecordingStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RecordingStatus.ForeColor = System.Drawing.Color.Red;
            this.RecordingStatus.Location = new System.Drawing.Point(1238, 448);
            this.RecordingStatus.Multiline = true;
            this.RecordingStatus.Name = "RecordingStatus";
            this.RecordingStatus.Size = new System.Drawing.Size(128, 85);
            this.RecordingStatus.TabIndex = 43;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(112, 240);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(0, 13);
            this.label20.TabIndex = 50;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(17, 240);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(65, 13);
            this.label21.TabIndex = 49;
            this.label21.Text = "Video Path :";
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(325, 188);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(91, 31);
            this.button4.TabIndex = 48;
            this.button4.Text = "Pause";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(325, 134);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(91, 28);
            this.button5.TabIndex = 47;
            this.button5.Text = "Stop";
            this.button5.UseVisualStyleBackColor = true;
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(325, 24);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(91, 32);
            this.button6.TabIndex = 46;
            this.button6.Text = "Upload Video";
            this.button6.UseVisualStyleBackColor = true;
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(325, 79);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(91, 28);
            this.button7.TabIndex = 45;
            this.button7.Text = "Play";
            this.button7.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(20, 24);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(276, 197);
            this.panel1.TabIndex = 44;
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(112, 240);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(0, 13);
            this.label22.TabIndex = 50;
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(17, 240);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(65, 13);
            this.label23.TabIndex = 49;
            this.label23.Text = "Video Path :";
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(325, 188);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(91, 31);
            this.button8.TabIndex = 48;
            this.button8.Text = "Pause";
            this.button8.UseVisualStyleBackColor = true;
            // 
            // button9
            // 
            this.button9.Location = new System.Drawing.Point(325, 134);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(91, 28);
            this.button9.TabIndex = 47;
            this.button9.Text = "Stop";
            this.button9.UseVisualStyleBackColor = true;
            // 
            // button10
            // 
            this.button10.Location = new System.Drawing.Point(325, 24);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(91, 32);
            this.button10.TabIndex = 46;
            this.button10.Text = "Upload Video";
            this.button10.UseVisualStyleBackColor = true;
            // 
            // button11
            // 
            this.button11.Location = new System.Drawing.Point(325, 79);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(91, 28);
            this.button11.TabIndex = 45;
            this.button11.Text = "Play";
            this.button11.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Location = new System.Drawing.Point(20, 24);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(276, 197);
            this.panel2.TabIndex = 44;
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Location = new System.Drawing.Point(1239, 432);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(95, 13);
            this.label30.TabIndex = 54;
            this.label30.Text = "Recording Status :";
            // 
            // beatCount
            // 
            this.beatCount.AutoSize = true;
            this.beatCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 72F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.beatCount.ForeColor = System.Drawing.Color.Blue;
            this.beatCount.Location = new System.Drawing.Point(1199, 208);
            this.beatCount.Name = "beatCount";
            this.beatCount.Size = new System.Drawing.Size(0, 108);
            this.beatCount.TabIndex = 56;
            // 
            // scoreFeedbackText
            // 
            this.scoreFeedbackText.BackColor = System.Drawing.Color.Blue;
            this.scoreFeedbackText.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.scoreFeedbackText.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.scoreFeedbackText.ForeColor = System.Drawing.Color.White;
            this.scoreFeedbackText.Location = new System.Drawing.Point(1242, 549);
            this.scoreFeedbackText.Multiline = true;
            this.scoreFeedbackText.Name = "scoreFeedbackText";
            this.scoreFeedbackText.Size = new System.Drawing.Size(128, 208);
            this.scoreFeedbackText.TabIndex = 58;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "CalibrationOption";
            this.dataGridViewTextBoxColumn1.HeaderText = "Calibration";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.Width = 75;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "Tag";
            this.dataGridViewTextBoxColumn2.HeaderText = "Tag";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.DataPropertyName = "Value";
            this.dataGridViewTextBoxColumn8.HeaderText = "Value";
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            this.dataGridViewTextBoxColumn8.Width = 50;
            // 
            // startTrainingCommand
            // 
            this.startTrainingCommand.BackgroundImage = global::MainGUI.Properties.Resources.training_icon;
            this.startTrainingCommand.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.startTrainingCommand.Location = new System.Drawing.Point(1238, 208);
            this.startTrainingCommand.Name = "startTrainingCommand";
            this.startTrainingCommand.Size = new System.Drawing.Size(96, 67);
            this.startTrainingCommand.TabIndex = 60;
            this.startTrainingCommand.UseVisualStyleBackColor = true;
            this.startTrainingCommand.Click += new System.EventHandler(this.startTrainingCommand_Click);
            // 
            // tapSetupCommand
            // 
            this.tapSetupCommand.BackgroundImage = global::MainGUI.Properties.Resources.TieShoes;
            this.tapSetupCommand.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.tapSetupCommand.Location = new System.Drawing.Point(1238, 118);
            this.tapSetupCommand.Name = "tapSetupCommand";
            this.tapSetupCommand.Size = new System.Drawing.Size(96, 64);
            this.tapSetupCommand.TabIndex = 59;
            this.tapSetupCommand.UseVisualStyleBackColor = true;
            this.tapSetupCommand.Click += new System.EventHandler(this.tapSetupCommand_Click);
            // 
            // Exit
            // 
            this.Exit.BackgroundImage = global::MainGUI.Properties.Resources.exit_sign;
            this.Exit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Exit.Location = new System.Drawing.Point(1238, 28);
            this.Exit.Name = "Exit";
            this.Exit.Size = new System.Drawing.Size(96, 65);
            this.Exit.TabIndex = 55;
            this.Exit.UseVisualStyleBackColor = true;
            this.Exit.Click += new System.EventHandler(this.Exit_Click);
            // 
            // referenceDataPlayPageView
            // 
            this.referenceDataPlayPageView.AutoGenerateColumns = false;
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle10.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle10.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle10.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.referenceDataPlayPageView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle10;
            this.referenceDataPlayPageView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.referenceDataPlayPageView.DataSource = this.wiimoteReferenceDataStoreBindingSource;
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle11.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle11.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle11.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle11.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.referenceDataPlayPageView.DefaultCellStyle = dataGridViewCellStyle11;
            this.referenceDataPlayPageView.Location = new System.Drawing.Point(48, 21);
            this.referenceDataPlayPageView.MultiSelect = false;
            this.referenceDataPlayPageView.Name = "referenceDataPlayPageView";
            this.referenceDataPlayPageView.ReadOnly = true;
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle12.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle12.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle12.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle12.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle12.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.referenceDataPlayPageView.RowHeadersDefaultCellStyle = dataGridViewCellStyle12;
            this.referenceDataPlayPageView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.referenceDataPlayPageView.Size = new System.Drawing.Size(954, 90);
            this.referenceDataPlayPageView.TabIndex = 0;
            this.referenceDataPlayPageView.SelectionChanged += new System.EventHandler(this.referenceDataPlayPageView_SelectionChanged);
            // 
            // videoPanel
            // 
            this.videoPanel.Location = new System.Drawing.Point(20, 21);
            this.videoPanel.Name = "videoPanel";
            this.videoPanel.Size = new System.Drawing.Size(263, 197);
            this.videoPanel.TabIndex = 44;
            // 
            // PlayButton
            // 
            this.PlayButton.Location = new System.Drawing.Point(88, 248);
            this.PlayButton.Name = "PlayButton";
            this.PlayButton.Size = new System.Drawing.Size(61, 26);
            this.PlayButton.TabIndex = 45;
            this.PlayButton.Text = "Play";
            this.PlayButton.UseVisualStyleBackColor = true;
            this.PlayButton.Click += new System.EventHandler(this.Play_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(20, 248);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(62, 26);
            this.button3.TabIndex = 46;
            this.button3.Text = "Upload";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.Open_Click);
            // 
            // StopButton
            // 
            this.StopButton.Location = new System.Drawing.Point(155, 248);
            this.StopButton.Name = "StopButton";
            this.StopButton.Size = new System.Drawing.Size(64, 26);
            this.StopButton.TabIndex = 47;
            this.StopButton.Text = "Stop";
            this.StopButton.UseVisualStyleBackColor = true;
            this.StopButton.Click += new System.EventHandler(this.Stop_Click);
            // 
            // PauseButton
            // 
            this.PauseButton.Location = new System.Drawing.Point(225, 248);
            this.PauseButton.Name = "PauseButton";
            this.PauseButton.Size = new System.Drawing.Size(58, 26);
            this.PauseButton.TabIndex = 48;
            this.PauseButton.Text = "Pause";
            this.PauseButton.UseVisualStyleBackColor = true;
            this.PauseButton.Click += new System.EventHandler(this.Pause_Click);
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(17, 228);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(65, 13);
            this.label27.TabIndex = 49;
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(112, 228);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(0, 13);
            this.label26.TabIndex = 50;
            // 
            // VideoPathValue
            // 
            this.VideoPathValue.AutoSize = true;
            this.VideoPathValue.Location = new System.Drawing.Point(93, 228);
            this.VideoPathValue.Name = "VideoPathValue";
            this.VideoPathValue.Size = new System.Drawing.Size(0, 13);
            this.VideoPathValue.TabIndex = 51;
            // 
            // RecordPlay
            // 
            this.RecordPlay.Location = new System.Drawing.Point(927, 182);
            this.RecordPlay.Name = "RecordPlay";
            this.RecordPlay.Size = new System.Drawing.Size(115, 34);
            this.RecordPlay.TabIndex = 42;
            this.RecordPlay.Text = "RecordPlay";
            this.RecordPlay.UseVisualStyleBackColor = true;
            this.RecordPlay.Click += new System.EventHandler(this.RecordPlay_Click);
            // 
            // dataGridView2
            // 
            this.dataGridView2.AutoGenerateColumns = false;
            dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle13.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle13.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle13.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle13.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle13.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle13.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView2.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle13;
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle14.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle14.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle14.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle14.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle14.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle14.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView2.DefaultCellStyle = dataGridViewCellStyle14;
            this.dataGridView2.Location = new System.Drawing.Point(32, 23);
            this.dataGridView2.MultiSelect = false;
            this.dataGridView2.Name = "dataGridView2";
            dataGridViewCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle15.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle15.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle15.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle15.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle15.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle15.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView2.RowHeadersDefaultCellStyle = dataGridViewCellStyle15;
            this.dataGridView2.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView2.Size = new System.Drawing.Size(873, 253);
            this.dataGridView2.TabIndex = 57;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.DataPropertyName = "FileName";
            this.dataGridViewTextBoxColumn5.HeaderText = "FileName";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.DataPropertyName = "RecordingInterval";
            this.dataGridViewTextBoxColumn7.HeaderText = "RecordingInterval";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.DataPropertyName = "LeadIn";
            this.dataGridViewTextBoxColumn6.HeaderText = "LeadIn";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.Width = 75;
            // 
            // NumBeatsPerBar
            // 
            this.NumBeatsPerBar.DataPropertyName = "NumBeatsPerBar";
            this.NumBeatsPerBar.HeaderText = "BPB";
            this.NumBeatsPerBar.Name = "NumBeatsPerBar";
            this.NumBeatsPerBar.Width = 50;
            // 
            // NumBeatsPerMinute
            // 
            this.NumBeatsPerMinute.DataPropertyName = "NumBeatsPerMinute";
            this.NumBeatsPerMinute.HeaderText = "BPM";
            this.NumBeatsPerMinute.Name = "NumBeatsPerMinute";
            this.NumBeatsPerMinute.Width = 50;
            // 
            // Fluctuation
            // 
            this.Fluctuation.DataPropertyName = "Fluctuation";
            this.Fluctuation.HeaderText = "Fluctuation";
            this.Fluctuation.Name = "Fluctuation";
            this.Fluctuation.ReadOnly = true;
            this.Fluctuation.Width = 75;
            // 
            // AverageDelay
            // 
            this.AverageDelay.DataPropertyName = "AverageDelay";
            this.AverageDelay.HeaderText = "AveDelay";
            this.AverageDelay.Name = "AverageDelay";
            this.AverageDelay.ReadOnly = true;
            this.AverageDelay.Width = 75;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.DataPropertyName = "RecordName";
            this.dataGridViewTextBoxColumn4.HeaderText = "RecordName";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "RecordID";
            this.dataGridViewTextBoxColumn3.HeaderText = "ID";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.Width = 50;
            // 
            // DeletePlayRecord
            // 
            this.DeletePlayRecord.Location = new System.Drawing.Point(927, 78);
            this.DeletePlayRecord.Name = "DeletePlayRecord";
            this.DeletePlayRecord.Size = new System.Drawing.Size(115, 33);
            this.DeletePlayRecord.TabIndex = 59;
            this.DeletePlayRecord.Text = "Delete Record";
            this.DeletePlayRecord.UseVisualStyleBackColor = true;
            this.DeletePlayRecord.Click += new System.EventHandler(this.DeletePlayRecord_Click);
            // 
            // UploadPlay
            // 
            this.UploadPlay.Location = new System.Drawing.Point(927, 130);
            this.UploadPlay.Name = "UploadPlay";
            this.UploadPlay.Size = new System.Drawing.Size(115, 33);
            this.UploadPlay.TabIndex = 60;
            this.UploadPlay.Text = "Upload";
            this.UploadPlay.UseVisualStyleBackColor = true;
            this.UploadPlay.Click += new System.EventHandler(this.UploadPlay_Click);
            // 
            // NewPlayRecord
            // 
            this.NewPlayRecord.Location = new System.Drawing.Point(927, 28);
            this.NewPlayRecord.Name = "NewPlayRecord";
            this.NewPlayRecord.Size = new System.Drawing.Size(115, 32);
            this.NewPlayRecord.TabIndex = 61;
            this.NewPlayRecord.Text = "New";
            this.NewPlayRecord.UseVisualStyleBackColor = true;
            this.NewPlayRecord.Click += new System.EventHandler(this.NewPlayRecord_Click);
            // 
            // CompareRecords
            // 
            this.CompareRecords.Location = new System.Drawing.Point(927, 233);
            this.CompareRecords.Name = "CompareRecords";
            this.CompareRecords.Size = new System.Drawing.Size(115, 33);
            this.CompareRecords.TabIndex = 61;
            this.CompareRecords.Text = "Calculate Score";
            this.CompareRecords.UseVisualStyleBackColor = true;
            this.CompareRecords.Click += new System.EventHandler(this.CompareRecords_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(1284, 753);
            this.Controls.Add(this.startTrainingCommand);
            this.Controls.Add(this.tapSetupCommand);
            this.Controls.Add(this.scoreFeedbackText);
            this.Controls.Add(this.beatCount);
            this.Controls.Add(this.Exit);
            this.Controls.Add(this.label30);
            this.Controls.Add(this.TabControl);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.RecordingStatus);
            this.MaximumSize = new System.Drawing.Size(1350, 800);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.TabControl.ResumeLayout(false);
            this.trainingPage.ResumeLayout(false);
            this.setupTab.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.calibrationSetup.ResumeLayout(false);
            this.calibrationSetup.PerformLayout();
            this.wiimoteDataOptions.ResumeLayout(false);
            this.wiimoteDataOptions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.configurationBindingSource)).EndInit();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.WiimoteConnection.ResumeLayout(false);
            this.WiimoteConnection.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.mediaControls.ResumeLayout(false);
            this.mediaControls.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.SystemConfiguration.ResumeLayout(false);
            this.SystemConfiguration.PerformLayout();
            this.CalibrationTab.ResumeLayout(false);
            this.CalibrationTab.PerformLayout();
            this.VoiceSetupGroup.ResumeLayout(false);
            this.ReferenceData.ResumeLayout(false);
            this.ReferenceData.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.traingSegmentGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trainingSegmentInfoBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.wiimote2Gyro)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.wiimote1Gyro)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.wiimote2AccChart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.recordingItemGridView4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.referenceRecordingItemsBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.wiimoteReferenceRecordBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.wiimoteReferenceDataStoreBindingSource)).EndInit();
            this.WiimoteSensorData.ResumeLayout(false);
            this.WiimoteSensorData.PerformLayout();
            this.WiimoteLog.ResumeLayout(false);
            this.WiimoteLog.PerformLayout();
            this.TimerSettings.ResumeLayout(false);
            this.TimerSettings.PerformLayout();
            this.TestPage.ResumeLayout(false);
            this.TestPage.PerformLayout();
            this.Training.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.MogrePropertyView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mogreWrapperMainBindingSource)).EndInit();
            this.xnaWindow.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chartDataBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.referenceDataPlayPageView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.TextBox TUIOLog;
        internal System.Windows.Forms.TextBox SessionID;
        internal System.Windows.Forms.TextBox WiimoteNumber;
        internal System.Windows.Forms.TextBox EventTIme;
        internal System.Windows.Forms.TextBox AccX;
        internal System.Windows.Forms.TextBox PitchValue;
        internal System.Windows.Forms.TextBox RollValue;
        internal System.Windows.Forms.TextBox AccY;
        internal System.Windows.Forms.TextBox AccZ;
        internal System.Windows.Forms.TextBox RawPitch;
        internal System.Windows.Forms.TextBox RawRoll;
        internal System.Windows.Forms.TextBox RawYaw;
        internal System.Windows.Forms.TextBox SpeedRoll;
        internal System.Windows.Forms.TextBox SpeedPitch;
        internal System.Windows.Forms.TextBox SpeedYaw;
        internal System.Windows.Forms.Label label4;
        internal System.Windows.Forms.Label label5;
        internal System.Windows.Forms.Label label6;
        internal System.Windows.Forms.Label label7;
        internal System.Windows.Forms.Label label8;
        internal System.Windows.Forms.Label label9;
        internal System.Windows.Forms.Label label10;
        internal System.Windows.Forms.Label label11;
        internal System.Windows.Forms.Label label12;
        internal System.Windows.Forms.Label label13;
        internal System.Windows.Forms.Label label14;
        internal System.Windows.Forms.Label label15;
        internal System.Windows.Forms.Label label16;
        internal System.Windows.Forms.Label label17;
        internal System.Windows.Forms.Label label18;
        internal System.Windows.Forms.Label label19;
        internal System.Windows.Forms.TabControl TabControl;
        internal System.Windows.Forms.TabPage WiimoteSensorData;
        internal System.Windows.Forms.TabPage WiimoteLog;
        internal System.Windows.Forms.TextBox RecordingStatus;
        internal System.Windows.Forms.TabPage TimerSettings;
        internal System.Windows.Forms.TextBox BeatLog;
        internal System.Windows.Forms.Button TestTimer;
        internal System.Windows.Forms.DataGridViewTextBoxColumn numBeatsDataGridViewTextBoxColumn;
        internal System.Windows.Forms.BindingSource wiimoteReferenceDataStoreBindingSource;
        internal System.Windows.Forms.TabPage TestPage;
        internal System.Windows.Forms.Button Test;
        internal System.Windows.Forms.Button TestPlay;
        private System.Windows.Forms.TabPage ReferenceData;
        internal System.Windows.Forms.GroupBox groupBox1;
        internal System.Windows.Forms.Button UploadReference;
        internal System.Windows.Forms.Button DeleteRecord;
        internal System.Windows.Forms.DataGridView dataGridView3;
        internal System.Windows.Forms.Button RecordReference;
        internal System.Windows.Forms.Label label20;
        internal System.Windows.Forms.Label label21;
        internal System.Windows.Forms.Button button4;
        internal System.Windows.Forms.Button button5;
        internal System.Windows.Forms.Button button6;
        internal System.Windows.Forms.Button button7;
        internal System.Windows.Forms.Panel panel1;
        internal System.Windows.Forms.Label label22;
        internal System.Windows.Forms.Label label23;
        internal System.Windows.Forms.Button button8;
        internal System.Windows.Forms.Button button9;
        internal System.Windows.Forms.Button button10;
        internal System.Windows.Forms.Button button11;
        internal System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button NewReference;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn11;
        internal System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.BindingSource chartDataBindingSource;
        private System.Windows.Forms.BindingSource wiimoteReferenceRecordBindingSource;
        internal System.Windows.Forms.DataVisualization.Charting.Chart wiimote2AccChart;
        internal System.Windows.Forms.DataVisualization.Charting.Chart wiimote2Gyro;
        internal System.Windows.Forms.DataVisualization.Charting.Chart wiimote1Gyro;
        private System.Windows.Forms.Label wiimote1Label;
        private System.Windows.Forms.Label wiimote2Label;
        internal System.Windows.Forms.CheckBox chartShowOption;
        private System.Windows.Forms.TabPage setupTab;
        internal System.Windows.Forms.GroupBox WiimoteConnection;
        internal System.Windows.Forms.TextBox WiimoteStatus;
        internal System.Windows.Forms.Button wiimoteDisconnect;
        internal System.Windows.Forms.Button wiimoteConnect;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.Button Exit;
        private System.Windows.Forms.DataGridView recordingItemGridView4;
        private System.Windows.Forms.BindingSource referenceRecordingItemsBindingSource;
        private System.Windows.Forms.Button newReferenceRecordItem;
        private System.Windows.Forms.Label beatCount;
        private System.Windows.Forms.TextBox scoreFeedbackText;
        internal System.Windows.Forms.GroupBox groupBox4;
        internal System.Windows.Forms.RadioButton dynamicCalibrationOption;
        internal System.Windows.Forms.RadioButton systemCalibrationOption;
        internal System.Windows.Forms.RadioButton noCalibrationOption;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewComboBoxColumn dataGridViewComboBoxColumn1;
        private System.Windows.Forms.DataGridViewComboBoxColumn CalibrationOption;
        internal System.Windows.Forms.ProgressBar wiimoteConnectionProgress;
        private System.Windows.Forms.GroupBox SystemConfiguration;
        private System.Windows.Forms.BindingSource configurationBindingSource;
        private System.Windows.Forms.CheckBox soundOption;
        private System.Windows.Forms.TextBox chartSkipStepValue;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.CheckBox chartOneAxisOnly;
        private System.Windows.Forms.Button stopConnecting;
        private System.Windows.Forms.CheckBox feedbackOnOption;
        private System.Windows.Forms.Button deleteReferenceRecordingItemButton;
        private System.Windows.Forms.TabPage CalibrationTab;
        internal System.Windows.Forms.Button RecordStopButton;
        internal System.Windows.Forms.TextBox calibrationRecordingStatus;
        private System.Windows.Forms.TabPage trainingPage;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TextBox WiimoteStateCheckDelayValue;
        private System.Windows.Forms.Label WiimoteStateCheckDelayLabel;
        private System.Windows.Forms.TextBox WiimoteStateNumChecksValue;
        private System.Windows.Forms.Label WiimoteStateNumChecksLabel;
        internal System.Windows.Forms.CheckBox mp3MusicOption;
        private System.Windows.Forms.GroupBox groupBox5;
        internal System.Windows.Forms.Label RecordingIntervalLabel;
        internal System.Windows.Forms.Label LeadInLabel;
        internal System.Windows.Forms.TextBox RecordingInterval;
        internal System.Windows.Forms.TextBox LeadIn;
        internal System.Windows.Forms.Label label1;
        internal System.Windows.Forms.TextBox beatsPerMinute;
        internal System.Windows.Forms.TextBox beatsPerBar;
        internal System.Windows.Forms.TextBox numBars;
        internal System.Windows.Forms.Label label2;
        internal System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button musicBrowseButton;
        private System.Windows.Forms.TextBox wiimoteBeatMP3Value;
        private System.Windows.Forms.Label wiimoteBeatMusicLabel;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.GroupBox mediaControls;
        private System.Windows.Forms.Button browseTrainingVideo;
        private System.Windows.Forms.TextBox trainingVideoValue;
        private System.Windows.Forms.Label trainingVideoLabel;
        private System.Windows.Forms.Button browseTrainingMusic;
        private System.Windows.Forms.TextBox trainingMusicValue;
        private System.Windows.Forms.Label TrainingMusicLabel;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.TextBox matlabReturnValue;
        private System.Windows.Forms.Label label35;
        private System.Windows.Forms.Button wiimoteSimulationBrowse;
        private System.Windows.Forms.TextBox wiimoteSimulationFile;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.GroupBox groupBox6;
        internal System.Windows.Forms.Button resetCalibration;
        internal System.Windows.Forms.Button startCalibration;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.Label label37;
        private System.Windows.Forms.Label label36;
        internal System.Windows.Forms.TextBox wiimote2BatteryLevel;
        internal System.Windows.Forms.TextBox wiimote1BatteryLevel;
        private System.Windows.Forms.TabPage Training;
        protected System.Windows.Forms.Panel ogrePanel;
        private System.Windows.Forms.Button loadMogre;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn valueDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn10;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn19;
        internal System.Windows.Forms.DataGridView MogrePropertyView;
        internal System.Windows.Forms.BindingSource mogreWrapperMainBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn20;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn21;
        private VideoPanelControl.TrainingVideoPanel trainingVideoPanel1;
        private System.Windows.Forms.TextBox trainingPreRecordingTime;
        private System.Windows.Forms.Label labelPreRecordingTime;
        private System.Windows.Forms.Button MoveDownCommand;
        private System.Windows.Forms.Button MoveUpCommand;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn RecordingDone;
        private System.Windows.Forms.DataGridViewTextBoxColumn recordNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn18;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private System.Windows.Forms.Button tapSetupCommand;
        private System.Windows.Forms.Button startTrainingCommand;
        private System.Windows.Forms.TextBox WiimoteDataSendIntervalValue;
        private System.Windows.Forms.Label WiimoteDataSendIntervalLabel;
        private System.Windows.Forms.GroupBox VoiceSetupGroup;
        private System.Windows.Forms.Button configureMicCommand;
        private System.Windows.Forms.Button voiceCommandsOption;
        private System.Windows.Forms.GroupBox calibrationGroup;
        private System.Windows.Forms.TabPage BluetoothSetupTab;
        private System.Windows.Forms.GroupBox wiimoteDataOptions;
        internal System.Windows.Forms.RadioButton accMotionPlus;
        private System.Windows.Forms.GroupBox connectionParameters;
        internal System.Windows.Forms.RadioButton accMotionplusIR;
        private System.Windows.Forms.GroupBox calibrationSetup;
        private System.Windows.Forms.TextBox calibrationNumBeatsPerBarsValue;
        private System.Windows.Forms.Label calibrationTimeUnit;
        private System.Windows.Forms.DataGridView traingSegmentGrid;
        private System.Windows.Forms.DataGridViewTextBoxColumn nextVideoPlayDataGridViewTextBoxColumn;
        internal System.Windows.Forms.BindingSource trainingSegmentInfoBindingSource;
        private System.Windows.Forms.Button newTrainingSegment;
        private System.Windows.Forms.DataGridViewTextBoxColumn referenceNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn numBeatsPerMinuteDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn numBeatsPerBarDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn numBarsDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn leadInDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn recordingIntervalDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn videoPathDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn scoreDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn averageDelayDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn fluctuationDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn numberOfStarsDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn numBeatsPerMinuteDataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn numBeatsPerBarDataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn numBarsDataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn leadInDataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn recordingIntervalDataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn wiimoteRecordingDelayDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn videoPathDataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn nextVideoPlayDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn repeatOptionDataGridViewCheckBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn recordIDDataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn recordNameDataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn selectedRecordingDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn parentTrainingSegmentInfoDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn highestRecordingItemIndexDataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn recordIDDataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn recordNameDataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridView referenceDataPlayPageView;
        internal System.Windows.Forms.Panel videoPanel;
        internal System.Windows.Forms.Button PlayButton;
        internal System.Windows.Forms.Button button3;
        internal System.Windows.Forms.Button StopButton;
        internal System.Windows.Forms.Button PauseButton;
        internal System.Windows.Forms.Label label27;
        internal System.Windows.Forms.Label label26;
        internal System.Windows.Forms.Label VideoPathValue;
        internal System.Windows.Forms.Button RecordPlay;
        internal System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn NumBeatsPerBar;
        private System.Windows.Forms.DataGridViewTextBoxColumn NumBeatsPerMinute;
        private System.Windows.Forms.DataGridViewTextBoxColumn Fluctuation;
        private System.Windows.Forms.DataGridViewTextBoxColumn AverageDelay;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        internal System.Windows.Forms.Button DeletePlayRecord;
        private System.Windows.Forms.Button UploadPlay;
        private System.Windows.Forms.Button NewPlayRecord;
        private System.Windows.Forms.Button CompareRecords;
        private System.Windows.Forms.Button SaveDataStore;
        private System.Windows.Forms.Button deleteTrainingSegment;
        private System.Windows.Forms.Button SaveConfiguration;
        private System.Windows.Forms.DataGridViewTextBoxColumn RecordName;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
        private System.Windows.Forms.DataGridViewTextBoxColumn VideoPath;
        private System.Windows.Forms.DataGridViewCheckBoxColumn SlowMoOption;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ScoringOption;
        private System.Windows.Forms.DataGridViewCheckBoxColumn RepeatOption;
        private System.Windows.Forms.DataGridViewTextBoxColumn numBeatsPerMinuteDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn numBeatsPerBarDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn numBarsDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn leadInDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn recordingIntervalDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn trainingReferenceRecordDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn slowMoOptionDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn scoringOptionDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn trainingPlayRecordDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn highestRecordingItemIndexDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn recordIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn recordNameDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn parentRecordDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn selectedRecordingDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn parentTrainingSegmentInfoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn slowMoOptionDataGridViewCheckBoxColumn1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn scoringOptionDataGridViewCheckBoxColumn1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn repeatOptionDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn wiimoteRecordingDelayDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn videoPathDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn filePathDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn recordedTimeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn recordingDoneDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn highestRecordingItemIndexDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn recordIDDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn recordNameDataGridViewTextBoxColumn2;
        private System.Windows.Forms.TabPage xnaWindow;
        private WinFormsGraphicsDevice.QuartenionViewerControl quartenionViewerControl;
        private System.Windows.Forms.Button readDataButton;
        private System.Windows.Forms.Button stopReplayButton;
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data;

using WiimoteData;
using ProjectCommon;
using Utilities;

using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

using System.Data;
using System.Data.OleDb;

using System.IO;
using Logging;



namespace MainGUI
{
    internal class WiimoteRecordingHandler
    {
        public const int WIIMOTES_CONNECTED_CODE = 0;
        public const string WIIMOTES_CONNECTED_STRING = "Wiimotes now connected !!";
        public const string WIIMOTES_DISCONNECTED_STRING = "Wiimotes not connected";
        public const string WIIMOTES_CONNECTING_STRING = "Wiimotes connecting ............";

        public const string REFERENCE_RECORDING_IN_PROGRESS_STRING = "Reference Recording in Progress";
        public const string REFERENCE_RECORDING_COMPLETED = "Reference Recording Completed";

        public const string PLAY_RECORDING_IN_PROGRESS_STRING = "Play Recording in Progress";
        public const string PLAY_RECORDING_COMPLETED = "Play Recording Completed";

        private static WiimoteRecordingHandler m_WiimoteRecordingHandler;

        private Form1 m_parent;

        private bool m_RecordReference;
        private bool m_RecordPlay;

        private DateTime m_RecordStartTime;

        private Wiimotes m_Wiimotes;

        public WiimoteRecordingHandler(Wiimotes p_Wiimotes, Form1 form)
        {
            m_parent = form;

            m_RecordReference = false;
            m_RecordPlay = false;

            m_Wiimotes = p_Wiimotes;

            this.m_parent.wiimoteConnectionProgress.Maximum = Configuration.getConfiguration().MaxWiimoteConnectionTries;

            m_Wiimotes.RecordingBeatEvent += new Wiimotes.OnRecordingBeatEvent(OnRecordingBeatEvent);
            m_Wiimotes.RecordingStartedEvent += new Wiimotes.OnRecordingStartedEvent(OnRecordingStartedEvent);
            m_Wiimotes.RecordingCompletedEvent += new Wiimotes.OnRecordingCompletedEvent(OnRecordingCompletedEvent);
            m_Wiimotes.WiimoteUpdateEvent += new Wiimotes.OnWiimoteUpdateEvent(OnWiimoteUpdateEvent);

        }

        public static WiimoteRecordingHandler getWiimoteRecordingHandler(Wiimotes p_Wiimotes,Form1 form)
        {
            if (m_WiimoteRecordingHandler == null)
                m_WiimoteRecordingHandler = new WiimoteRecordingHandler(p_Wiimotes,form);
            return m_WiimoteRecordingHandler;
        }

        public void Initialize()
        {
        }

        #region Wiimote Connect Handler Calls

        private void handleWiimoteConnected()
        {
            this.m_parent.SetTextWiimoteStatus(WIIMOTES_CONNECTED_STRING);
            this.m_parent.SetWiimoteButtonState((object)Form1.WiimoteButtonState.CONNECTED);

            this.m_parent.wiimoteConnectionProgress.Value = 0;
        }

        private void handleWiimoteConnectError(WiimoteState state)
        {
            this.m_parent.SetTextWiimoteStatus(WIIMOTES_DISCONNECTED_STRING);
            this.m_parent.SetWiimoteButtonState((object)Form1.WiimoteButtonState.DISCONNECTED);

            string msg;
            if (state == WiimoteState.wiimoteDisconnectedState)
                msg = ProjectConstants.WIIMOTE_DISCONNECTED_MESSAGE;
            else if (state == WiimoteState.wiimoteBadDataState)
                msg = ProjectConstants.WIIMOTE_INVALID_DATA_MESSAGE;
            else
                msg = ProjectConstants.GENERAL_WIIMOTE_CONNECTION_ISSUE_MESSAGE;

            MessageBox.Show(msg,
                    "Wiimote Connection Issue", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

        }

        public void onWiimoteStateIssue(WiimoteState state)
        {
            handleWiimoteConnectError(state);
        }


        public bool connectWiimotes()
        {
            try
            {
                this.m_parent.SetWiimoteButtonState((object)Form1.WiimoteButtonState.CONNECTED);
                this.m_parent.SetTextWiimoteStatus(WIIMOTES_CONNECTING_STRING);
                this.m_parent.Update();


                this.m_parent.wiimoteConnectionProgress.Maximum = Configuration.getConfiguration().MaxWiimoteConnectionTries;

                m_Wiimotes.connectWiimotes(Configuration.getConfiguration().MaxWiimoteConnectionTries);
                handleWiimoteConnected();
                return true;
            }
            catch (WiimoteConnectionException ex)
            {
                handleWiimoteConnectError(ex.State);
                Console.WriteLine(ex);
                return false;
            }
        }

        public bool areWiimotesConnected()
        {
            return m_Wiimotes.areWiimotesConnected();
        }

        public void connectionTryAttempt()
        {
            this.m_parent.wiimoteConnectionProgress.Value += 1;
        }

        public void disconnectWiimotes()
        {
            try
            {

                m_Wiimotes.disconnectWiimotes();

                this.m_parent.SetTextWiimoteStatus(WIIMOTES_DISCONNECTED_STRING);
                this.m_parent.SetWiimoteButtonState((object)Form1.WiimoteButtonState.DISCONNECTED);
                this.m_parent.wiimoteConnectionProgress.Value = 0;
            }
            catch (WiimoteCommunicationException ex)
            {                
                Console.WriteLine(ex);
            }
        }

        public void stopConnectingAttempts()
        {
            m_Wiimotes.stopConnectingAttempts(); 
        }

        public void OnWiimoteUpdateEvent(object sender, WiimoteUpdateEventArgs e)
        {
            if (e.Wiimote1.WiimoteCurrentState != WiimoteState.wiimoteGoodStateState)
                handleWiimoteConnectError(e.Wiimote1.WiimoteCurrentState);

            if (e.Wiimote2.WiimoteCurrentState != WiimoteState.wiimoteGoodStateState)
                handleWiimoteConnectError(e.Wiimote2.WiimoteCurrentState);

            this.m_parent.SetBatteryLevels(e.Wiimote1.BatteryLevel.ToString(), e.Wiimote2.BatteryLevel.ToString());
        }

        public void wiimoteDataModeChanged(object sender, EventArgs e)
        {
            if (this.m_parent.accMotionPlus.Checked)
                Configuration.getConfiguration().WiimoteDataMode = ProjectConstants.WIIMOTE_MOTIONPLUS_ACC_DATA_MODE;
            else
                Configuration.getConfiguration().WiimoteDataMode = ProjectConstants.WIIMOTE_MOTIONPLUS_ACC_IR_DATA_MODE;
        }

#endregion



        #region Common Reference/Play Handlers

        public void save()
        {
            m_Wiimotes.save();
        }

        private CalibrationOption getCalibrationSelection()
        {
            if (this.m_parent.noCalibrationOption.Checked)
                return CalibrationOption.None;
            else if (this.m_parent.systemCalibrationOption.Checked)
                return CalibrationOption.System;
            else
                return CalibrationOption.Dynamic;
        }

        public void recordSelected(DataGridViewRow l_TableRow)
        {
            try
            {
                this.m_parent.TestTimer.Enabled = false;
                m_Wiimotes.startRecording(l_TableRow.DataBoundItem, Configuration.getConfiguration().MP3Option,this,
                    "Start Tapping,100,1,200,2,200,3,200,4,200",true);

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
        }

        public bool editFilenameColumnAttempt(DataGridView l_View, DataGridViewRow l_TableReferenceRow, int columnIndex)
        {
            IWiimoteChildRecord childRecord = (IWiimoteChildRecord)l_TableReferenceRow.DataBoundItem;

            if (l_View.Columns[columnIndex].DataPropertyName.CompareTo(ProjectConstants.FILE_NAME_PROPERTY_NAME) == 0 &&
                childRecord.RecordingDone)
                return false;
            else
                return true;
        }

        #endregion

        #region Plots

        private void updateChart(IWiimoteChildRecord wiimoteRecord, Chart wiimote1AccChart, Chart wiimote1Gyro,
            Chart wiimote2AccChart, Chart wiimote2Gyro,bool chartShowOption)
        {
            try
            {
                clearChart(wiimote1AccChart, wiimote1Gyro, wiimote2AccChart, wiimote2Gyro);

                if (chartShowOption)
                    plotChart(wiimoteRecord, wiimote1AccChart, wiimote1Gyro, wiimote2AccChart, wiimote2Gyro);
            }
            catch (CSVFileException e)
            {
                throw e;
            }
        }

        private void clearChart(Chart wiimote1AccChart, Chart wiimote1Gyro,
            Chart wiimote2AccChart, Chart wiimote2Gyro)
        {
            if (wiimote1AccChart.Series[0].Points == null)
                return;

            //Series 1 :  Acc X
            wiimote1AccChart.Series[0].Points.Clear();

            //Series 2 :  Acc Y
            wiimote1AccChart.Series[1].Points.Clear();

            //Series 3 :  Acc Z
            wiimote1AccChart.Series[2].Points.Clear();

            //Series 4 :  Speed Yaw
            wiimote1Gyro.Series[0].Points.Clear();

            //Series 5 :  Speed Pitch
            wiimote1Gyro.Series[1].Points.Clear();

            //Series 6 :  Speed Roll
            wiimote1Gyro.Series[2].Points.Clear();

            //Series 7 :  Acc X
            wiimote2AccChart.Series[0].Points.Clear();

            //Series 8 :  Acc Y
            wiimote2AccChart.Series[1].Points.Clear();

            //Series 9 :  Acc Z
            wiimote2AccChart.Series[2].Points.Clear();

            //Series 10 :  Speed Yaw
            wiimote2Gyro.Series[0].Points.Clear();

            //Series 11 :  Speed Pitch
            wiimote2Gyro.Series[1].Points.Clear();

            //Series 12 :  Speed Roll
            wiimote2Gyro.Series[2].Points.Clear();
        }

        private void plotChart(IWiimoteChildRecord wiimoteRecord, Chart wiimote1AccChart, Chart wiimote1Gyro,
            Chart wiimote2AccChart, Chart wiimote2Gyro)
        {
            try
            {
                if (!wiimoteRecord.RecordingDone)
                    return;

                while (true)
                {
                    string[] row = wiimoteRecord.getNextDataRow(Configuration.getConfiguration().WiimotesChartRowSkipStep);

                    if (row == null)
                        return;

                    //Series 1 :  Acc X
                    wiimote1AccChart.Series[0].Points.AddXY(Convert.ToDouble(row[WiimoteRecordBase.TIME_COLUMN_INDEX]),
                        Convert.ToDouble(row[WiimoteRecordBase.WIIMOTE1_ACC_X_COLUMN_INDEX]));
                    wiimote1AccChart.Series[0].Name = WiimoteRecordBase.WIIMOTE1_ACC_X_COLUMN_HEADER;

                    if (!Configuration.getConfiguration().WiimotesChartOneAxisOnly)
                    {
                        //Series 2 :  Acc Y
                        wiimote1AccChart.Series[1].Points.AddXY(Convert.ToDouble(row[WiimoteRecordBase.TIME_COLUMN_INDEX]),
                            Convert.ToDouble(row[WiimoteRecordBase.WIIMOTE1_ACC_Y_COLUMN_INDEX]));
                        wiimote1AccChart.Series[1].Name = WiimoteRecordBase.WIIMOTE1_ACC_Y_COLUMN_HEADER;

                        //Series 3 :  Acc Z
                        wiimote1AccChart.Series[2].Points.AddXY(Convert.ToDouble(row[WiimoteRecordBase.TIME_COLUMN_INDEX]),
                            Convert.ToDouble(row[WiimoteRecordBase.WIIMOTE1_ACC_Z_COLUMN_INDEX]));
                        wiimote1AccChart.Series[2].Name = WiimoteRecordBase.WIIMOTE1_ACC_Z_COLUMN_HEADER;
                    }

                    //Series 4 :  Speed Yaw
                    wiimote1Gyro.Series[0].Points.AddXY(Convert.ToDouble(row[WiimoteRecordBase.TIME_COLUMN_INDEX]),
                        Convert.ToDouble(row[WiimoteRecordBase.WIIMOTE1_SPEED_YAW_COLUMN_INDEX]));
                    wiimote1Gyro.Series[0].Name = WiimoteRecordBase.WIIMOTE1_SPEED_YAW_COLUMN_HEADER;

                    if (!Configuration.getConfiguration().WiimotesChartOneAxisOnly)
                    {
                        //Series 5 :  Speed Pitch
                        wiimote1Gyro.Series[1].Points.AddXY(Convert.ToDouble(row[WiimoteRecordBase.TIME_COLUMN_INDEX]),
                            Convert.ToDouble(row[WiimoteRecordBase.WIIMOTE1_ACC_PITCH_ANGLE_COLUMN_INDEX]));
                        wiimote1Gyro.Series[1].Name = WiimoteRecordBase.WIIMOTE1_ACC_PITCH_ANGLE_COLUMN_HEADER;

                        //Series 6 :  Speed Roll
                        wiimote1Gyro.Series[2].Points.AddXY(Convert.ToDouble(row[WiimoteRecordBase.TIME_COLUMN_INDEX]),
                            Convert.ToDouble(row[WiimoteRecordBase.WIIMOTE1_SPEED_ROLL_COLUMN_INDEX]));
                        wiimote1Gyro.Series[2].Name = WiimoteRecordBase.WIIMOTE1_SPEED_ROLL_COLUMN_HEADER;
                    }

                    //Series 7 :  Acc X
                    wiimote2AccChart.Series[0].Points.AddXY(Convert.ToDouble(row[WiimoteRecordBase.TIME_COLUMN_INDEX]),
                        Convert.ToDouble(row[WiimoteRecordBase.WIIMOTE2_ACC_X_COLUMN_INDEX]));
                    wiimote2AccChart.Series[0].Name = WiimoteRecordBase.WIIMOTE2_ACC_X_COLUMN_HEADER;

                    if (!Configuration.getConfiguration().WiimotesChartOneAxisOnly)
                    {
                        //Series 8 :  Acc Y
                        wiimote2AccChart.Series[1].Points.AddXY(Convert.ToDouble(row[WiimoteRecordBase.TIME_COLUMN_INDEX]),
                            Convert.ToDouble(row[WiimoteRecordBase.WIIMOTE2_ACC_Y_COLUMN_INDEX]));
                        wiimote2AccChart.Series[1].Name = WiimoteRecordBase.WIIMOTE2_ACC_Y_COLUMN_HEADER;

                        //Series 9 :  Acc Z
                        wiimote2AccChart.Series[2].Points.AddXY(Convert.ToDouble(row[WiimoteRecordBase.TIME_COLUMN_INDEX]),
                            Convert.ToDouble(row[WiimoteRecordBase.WIIMOTE2_ACC_Z_COLUMN_INDEX]));
                        wiimote2AccChart.Series[2].Name = WiimoteRecordBase.WIIMOTE2_ACC_Z_COLUMN_HEADER;
                    }

                    //Series 10 :  Speed Yaw
                    wiimote2Gyro.Series[0].Points.AddXY(Convert.ToDouble(row[WiimoteRecordBase.TIME_COLUMN_INDEX]),
                        Convert.ToDouble(row[WiimoteRecordBase.WIIMOTE2_SPEED_YAW_COLUMN_INDEX]));
                    wiimote2Gyro.Series[0].Name = WiimoteRecordBase.WIIMOTE2_SPEED_YAW_COLUMN_HEADER;

                    if (!Configuration.getConfiguration().WiimotesChartOneAxisOnly)
                    {
                        //Series 11 :  Speed Pitch
                        wiimote2Gyro.Series[1].Points.AddXY(Convert.ToDouble(row[WiimoteRecordBase.TIME_COLUMN_INDEX]),
                            Convert.ToDouble(row[WiimoteRecordBase.WIIMOTE2_ACC_PITCH_ANGLE_COLUMN_INDEX]));
                        wiimote2Gyro.Series[1].Name = WiimoteRecordBase.WIIMOTE2_ACC_PITCH_ANGLE_COLUMN_HEADER;

                        //Series 12 :  Speed Roll
                        wiimote2Gyro.Series[2].Points.AddXY(Convert.ToDouble(row[WiimoteRecordBase.TIME_COLUMN_INDEX]),
                            Convert.ToDouble(row[WiimoteRecordBase.WIIMOTE2_SPEED_ROLL_COLUMN_INDEX]));
                        wiimote2Gyro.Series[2].Name = WiimoteRecordBase.WIIMOTE2_SPEED_ROLL_COLUMN_HEADER;
                    }
                }

            }
            catch (CSVFileException e)
            {
                throw e;
            }
        }

        #endregion

        #region Reference Record Handlers


        public void newTrainingSegmentSelected()
        {
            string trainingSegmentName = ProjectConstants.UNTITLED_REFERENCE_NAME +
                (WiimoteDataStore.getWiimoteDataStore().HighestTrainingSegmentInfoIndex + 1);

            TrainingSegmentInfo lTrainingSegmentInfo = WiimoteDataStore.getWiimoteDataStore().addTrainingSegmentInfoRecord(trainingSegmentName);

            this.m_parent.SetTrainingSegmentInfoBindingSource(WiimoteDataStore.getWiimoteDataStore().TrainingSegmentInfoRecords);
        }

        public void trainingSegmentInfoRowReferencePageSelected(DataGridViewRow lTableReferenceRow)
        {
            TrainingSegmentInfo trainingSegmentInfo = (TrainingSegmentInfo)lTableReferenceRow.DataBoundItem;

            this.m_parent.SetReferenceDataBindingSource(trainingSegmentInfo.WiimoteReferenceRecords);
        }

        public void deleteTrainingSegmentRecordSelected(DataGridViewRow lTrainingSegmentRow, int index)
        {
            TrainingSegmentInfo trainingSegmentInfo = (TrainingSegmentInfo)lTrainingSegmentRow.DataBoundItem;

            DialogResult result;
            if (trainingSegmentInfo.getNumberOfWiimoteReferenceRecords() > 0)
            {
                result = MessageBox.Show("There are Multiple Reference Records for this Training Segment. Do you want to delete all the References ?",
                "Need User Input", MessageBoxButtons.YesNo);
            }
            else
            {
                WiimoteDataStore.getWiimoteDataStore().deleteTrainingSegmentRecord(index);
                return;
            }

            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                trainingSegmentInfo.deleteAllWiimoteRefernceRecords();
                WiimoteDataStore.getWiimoteDataStore().deleteTrainingSegmentRecord(index);
            }

            this.m_parent.SetTrainingSegmentInfoBindingSource(WiimoteDataStore.getWiimoteDataStore().TrainingSegmentInfoRecords);

        }


        public void newReferenceSelected(DataGridViewRow lTableReferenceRow)
        {
            TrainingSegmentInfo trainingSegmentInfo = (TrainingSegmentInfo)lTableReferenceRow.DataBoundItem;

            //add newly created Wiimote csv file to ReferenceDataStore
            IWiimoteReferenceRecord wiimoteRecord = trainingSegmentInfo.addWiimoteReferenceRecord(ProjectConstants.UNTITLED_REFERENCE_NAME + (trainingSegmentInfo.HighestRecordingItemIndex + 1), 
               this.m_parent.VideoPathValue.Text);

            this.m_parent.SetReferenceDataBindingSource(trainingSegmentInfo.WiimoteReferenceRecords);
        }

        public void newReferenceRecordingItemSelected(DataGridViewRow l_TableReferenceRow)
        {
            WiimoteReferenceRecord referenceRecord = (WiimoteReferenceRecord)l_TableReferenceRow.DataBoundItem;

            string referenceRecordingItemName = referenceRecord.RecordName + "_" +
                ProjectConstants.UNTITLED_REFERENCE_RECORDING_PREFIX + "_" + 
                (referenceRecord.HighestRecordingItemIndex + 1);

            IWiimoteChildRecord wiimoteRecord = (IWiimoteChildRecord)referenceRecord.addWiimoteReferenceRecordingItem(referenceRecordingItemName,
                getCalibrationSelection(),m_RecordStartTime);

            this.m_parent.SetReferenceRecordingItemDataBindingSource(referenceRecord.ReferenceRecordingItems);
        }

        public void recordReferenceRecordingItemSelected(DataGridViewRow l_TableReferenceRow)
        {
            try
            {
                this.m_parent.RecordingStatus.Text = REFERENCE_RECORDING_IN_PROGRESS_STRING;
                recordSelected(l_TableReferenceRow);

                m_RecordReference = true;

//                this.m_parent.SetReferenceDataBindingSource(WiimoteDataStore.getWiimoteDataStore().WiimoteReferenceRecords);
            }
            catch (WiimoteCommunicationException ex)
            {
                MessageBox.Show(ex.Message, "Record Selection Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            catch (WiimoteConnectionException ex)
            {
                MessageBox.Show(ex.Message, "Record Selection Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }


        }

        public void uploadReferenceSelected(string fileName)
        {
/*
            IWiimoteRecord wiimoteRecord = WiimoteReferenceDataStore.getWiimoteReferenceDataStore().addWiimoteReferenceRecord(ProjectConstants.UNTITLED_REFERENCE_NAME,
                fileName,
                DateTime.UtcNow, 0,
                0, 0,
                0, 0,
                "",
                true);
 
            this.m_parent.SetReferenceDataBindingSource(WiimoteReferenceDataStore.getWiimoteReferenceDataStore().WiimoteReferenceRecords);
 */
        }


        public void referenceRecordRowReferencePageSelected(DataGridViewRow l_TableReferenceRow)
        {
            WiimoteReferenceRecord referenceRecord = (WiimoteReferenceRecord)l_TableReferenceRow.DataBoundItem;

            this.m_parent.SetReferenceRecordingItemDataBindingSource(referenceRecord.ReferenceRecordingItems);
        }


        public void referenceRecordingItemRowSelected(DataGridViewRow l_TableReferenceRow)
        {
            try
            {
                IWiimoteChildRecord wiimoteRecord = (IWiimoteChildRecord)l_TableReferenceRow.DataBoundItem;
                updateChart(wiimoteRecord, this.m_parent.chart1, this.m_parent.wiimote1Gyro, this.m_parent.wiimote2AccChart,
                    this.m_parent.wiimote2Gyro, this.m_parent.chartShowOption.Checked);
            }
            catch (CSVFileException e)
            {
                MessageBox.Show(e.Message, "CSV File Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            
        }

        public void deleteReferenceRecordSelected(DataGridViewRow lTrainingSegmentRow,DataGridViewRow lReferenceRow,int index)
        {
            TrainingSegmentInfo trainingSegmentInfo = (TrainingSegmentInfo)lTrainingSegmentRow.DataBoundItem;
            WiimoteReferenceRecord referenceRecord= (WiimoteReferenceRecord)lReferenceRow.DataBoundItem;

            DialogResult result;
            if (referenceRecord.getNumberOfWiimoteReferenceRecordingItems() > 0)
            {
                result = MessageBox.Show("There are Multiple Recording Items for this Reference. Do you want to delete all the Recording Items ?",
                "Need User Input", MessageBoxButtons.YesNo);
            }
            else
            {
                trainingSegmentInfo.deleteWiimoteReferenceRecord(index);
                return;
            }
            
            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                referenceRecord.deleleAllWiimoteReferenceRecordingItems();
                trainingSegmentInfo.deleteChildItem(index);
            }

            this.m_parent.SetTrainingSegmentInfoBindingSource(WiimoteDataStore.getWiimoteDataStore().TrainingSegmentInfoRecords);

        }

        public void deleteReferenceRecordingItemSelected(DataGridViewRow l_TableReferenceRow,
            int index)
        {
            WiimoteReferenceRecord referenceRecord = (WiimoteReferenceRecord)l_TableReferenceRow.DataBoundItem;

            referenceRecord.deleteChildItem(index);
            this.m_parent.SetReferenceRecordingItemDataBindingSource(referenceRecord.ReferenceRecordingItems);
        }

        public void moveReferenceRecordUp(int index)
        {/*
            WiimoteReferenceRecord lReferenceRecord = WiimoteDataStore.getWiimoteDataStore().getWiimoteReferenceRecord(index);
            WiimoteDataStore.getWiimoteDataStore().moveWiimoteReferenceRecord(lReferenceRecord, ProjectConstants.MOVE_ROW_UP);
          */
        }

        public void moveReferenceRecordDown(int index)
        {
            /*
            WiimoteReferenceRecord lReferenceRecord = WiimoteDataStore.getWiimoteDataStore().getWiimoteReferenceRecord(index);
            WiimoteDataStore.getWiimoteDataStore().moveWiimoteReferenceRecord(lReferenceRecord, ProjectConstants.MOVE_ROW_DOWN);
             * */
        }


#endregion

        #region Play Record Handlers

        public void InitializePlayPageReferenceList()
        {
        }



        public void referenceRecordRowPlayPageSelected(DataGridViewRow l_TableReferenceRow)
        {

        }

        public void newPlaySelected(DataGridViewRow l_TableReferenceRow)
        {
/*
            WiimoteReferenceRecord referenceRecord = (WiimoteReferenceRecord)l_TableReferenceRow.DataBoundItem;

            WiimoteDataStore dataStore = WiimoteDataStore.getWiimoteDataStore();

            string playItemName = referenceRecord.RecordName + "_" +
                ProjectConstants.UNTITLED_PLAY_PREFIX + "_" +
                (referenceRecord.HighestRecordingItemIndex + 1);

            IWiimoteChildRecord wiimoteRecord = dataStore.addWiimotePlayRecord(referenceRecord,
                playItemName, m_RecordStartTime, 0);

            this.m_parent.SetPlayDataBindingSource(WiimoteDataStore.getWiimoteDataStore().WiimotePlayRecords);
 * */
        }

        public void recordPlaySelected(DataGridViewRow l_TablePlayRow)
        {
            /*
            try
            {
                this.m_parent.RecordingStatus.Text = PLAY_RECORDING_IN_PROGRESS_STRING;
                recordSelected(l_TablePlayRow);

                m_RecordPlay = true;

                this.m_parent.SetPlayDataBindingSource(WiimoteDataStore.getWiimoteDataStore().WiimotePlayRecords);
            }
            catch (WiimoteCommunicationException ex)
            {
                MessageBox.Show(ex.Message, "Play Record Selection Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            catch (WiimoteConnectionException ex)
            {
                MessageBox.Show(ex.Message, "Play Record Selection Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
             */
        }

        public void uploadPlaySelected(int referenceIndex, string fileName)
        {
/*
            WiimoteReferenceRecord referenceRecordForPlay = WiimoteDataStore.getWiimoteDataStore().getWiimoteReferenceRecord(referenceIndex);

            List<string[]> parsedData = CSVFileParser.parseCSV(fileName);

            if (parsedData.Count != 4)
            {
                MessageBox.Show("There was an issue with parsing the Wiimote data file",
                    "Wiimote Data Parsing Issue", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            int beatsPerMinute = Convert.ToInt32(parsedData[0][1]);
            int beatsPerBar = Convert.ToInt32(parsedData[1][1]);
            int numBars = Convert.ToInt32(parsedData[2][1]);
            int leadIn = Convert.ToInt32(parsedData[3][1]);

            IWiimoteRecord wiimoteRecord = WiimotePlayDataStore.getWiimotePlayDataStore().addWiimotePlayRecord(referenceRecordForPlay,
                ProjectConstants.UNTITLED_PLAY_NAME,
                fileName,
                DateTime.UtcNow,0,true);

            this.m_parent.SetPlayDataBindingSource(WiimotePlayDataStore.getWiimotePlayDataStore().WiimotePlayRecords);
 */
        }

        public void deletePlayRecord(DataGridViewSelectedRowCollection p_Rows)
        {
            /*
            int length = p_Rows.Count;
            for (int index = length - 1; index >= 0; index--)
                WiimoteDataStore.getWiimoteDataStore().deleteWiimotePlayRecord(p_Rows[index].Index);

            this.m_parent.wiimotePlayDataStoreBindingSource.DataSource = WiimoteDataStore.getWiimoteDataStore().WiimotePlayRecords;
             */

        }


        private void showScoreFeedback(double score)
        {
            string scoreText;

            if (score <= 5)
                scoreText = ProjectConstants.SUPER_SCORE_TEXT;
            else if (score <= 10)
                scoreText = ProjectConstants.EXCELLENT_SCORE_TEXT;
            else if (score <= 30)
                scoreText = ProjectConstants.GOOD_SCORE_TEXT;
            else if (score <= 50)
                scoreText = ProjectConstants.FAIR_SCORE_TEXT;
            else
                scoreText = ProjectConstants.POOR_SCORE_TEXT;

            this.m_parent.SetTextFeedback(scoreText);

            ApplicationSpeech.speakText(scoreText);

        }

        public void comparePlayToReference(DataGridViewRow l_ReferenceRow, DataGridViewRow l_PlayRow)
        {
            /*
            try
            {

                string message = null;

                if (l_ReferenceRow == null)
                {
                    message = m_Wiimotes.comparePlayToReference((WiimotePlayRecord)l_PlayRow.DataBoundItem);
                }
                else
                {
                    message = m_Wiimotes.comparePlayToReference((WiimoteReferenceRecord)l_ReferenceRow.DataBoundItem, 
                        (WiimotePlayRecord)l_PlayRow.DataBoundItem);
                }


                //            showScoreFeedback(score);
                m_parent.SetMatlabReturn(message);

                this.m_parent.SetPlayDataBindingSource(WiimoteDataStore.getWiimoteDataStore().WiimotePlayRecords);
            }
            catch (WiimoteRecordingException ex)
            {
                MessageBox.Show(ex.Message, "Record Comparison Error",MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }
             */

        }

        #endregion


        #region Timer

        public void testTimerClicked()
        {            
            
            this.m_parent.TestTimer.Enabled = false;
            TimerThread.getTimerThread().startTimer(Convert.ToInt32(this.m_parent.beatsPerMinute.Text),
                                   Convert.ToInt32(this.m_parent.beatsPerBar.Text),
                                   Convert.ToInt32(this.m_parent.numBars.Text),
                                   Convert.ToInt32(this.m_parent.LeadIn.Text),
                                   Configuration.getConfiguration().MP3Option,
                                   "Timer Testing,0");
             
            
        }

        public void setTimerBeatMP3(string mp3File)
        {
            m_Wiimotes.setTimerBeatMP3(mp3File);
        }

        public void OnRecordingStartedEvent(object sender, WiimoteRecordingEventArgs e)
        {
        }

        public void OnRecordingBeatEvent(object sender, WiimoteRecordingEventArgs e)
        {
            /*
            string logMessage = "\r\n# of beats so far = " + p_numBeats + " : # of Bars so far = " + p_numBars + " : Current Beat # In Bar = " + p_numBeatInBar;
            //            System.Console.WriteLine(logMessage);


            this.m_parent.AppendText(logMessage);

            int length = this.m_parent.BeatLog.Text.Length;
            if (length >= 2000)
            {
                char[] newText = new char[1000];
                this.m_parent.BeatLog.Text.CopyTo(1000, newText, 0, 1000);

                string newTextString = new String(newText);
                this.m_parent.SetTextBeatLog(newTextString);
            }
             */
            if (e.RecordingInvoker.Equals(this))
                this.m_parent.SetTextBeatCounter(e.getNumBeatInBar().ToString());
        }

        
        public void OnRecordingCompletedEvent(object sender, WiimoteRecordingEventArgs e)
        {
            if (!e.RecordingInvoker.Equals(this))
                return;

            this.m_parent.SetTextBeatCounter(e.getNumBeatInBar().ToString());

            if (m_RecordReference)
            {
                m_RecordReference = false;
                this.m_parent.SetTextRecordingStatus(REFERENCE_RECORDING_COMPLETED);
            }

            if (m_RecordPlay)
            {
                m_RecordPlay = false;
                this.m_parent.SetTextRecordingStatus(PLAY_RECORDING_COMPLETED);
            }

            this.m_parent.SetTestTimerButtonState(true);
            this.m_parent.SetRecordingButtonState(false);

            this.m_parent.SetTextBeatCounter("");

            ApplicationSpeech.speakText(ProjectConstants.RECORDING_COMPLETED_TEXT);

            this.m_parent.TestTimer.Enabled = true;

            this.m_parent.SetReferenceRecordingItemDataBindingSource(((WiimoteReferenceRecord)e.ChildRecord.ParentRecord).ReferenceRecordingItems);
        }


        #endregion

        #region Replay

        public void StartDataCollection()
        {
            m_Wiimotes.StartDataCollection("",false);
        }

        public void StopDataCollection()
        {
            m_Wiimotes.StopDataCollection();
        }



        
        #endregion

        #region TestCode
        public void testPlayClicked(DataGridViewRow l_oRow)
        {
/*
            WiimoteReferenceRecord l_SelectedReferenceRowForPlay = (WiimoteReferenceRecord)l_oRow.DataBoundItem;

            IWiimoteRecord wiimoteRecord = WiimotePlayDataStore.getWiimotePlayDataStore().addWiimotePlayRecord(l_SelectedReferenceRowForPlay,
                ProjectConstants.UNTITLED_PLAY_NAME + "1", "File1", DateTime.UtcNow, 0,false);

            wiimoteRecord = WiimotePlayDataStore.getWiimotePlayDataStore().addWiimotePlayRecord(l_SelectedReferenceRowForPlay,
                ProjectConstants.UNTITLED_PLAY_NAME + "2", "File2", DateTime.UtcNow, 0, false);

            this.m_parent.SetPlayDataBindingSource(WiimotePlayDataStore.getWiimotePlayDataStore().WiimotePlayRecords);
 */
        }

        public void testReferenceClicked()
        {
            /*
            WiimoteReferenceRecord record = null;

            record = WiimoteDataStore.getWiimoteDataStore().addWiimoteReferenceRecord(ProjectConstants.UNTITLED_REFERENCE_NAME + "1",
                10, 10, 10, 1, 0, "Video Path1");
            //            this.wiimoteReferenceDataStoreBindingSource.Add(record);

            record.addWiimoteReferenceRecordingItem(ProjectConstants.UNTITLED_REFERENCE_RECORDING_PREFIX + "Recording1",
                CalibrationOption.Dynamic, DateTime.UtcNow);

            record.addWiimoteReferenceRecordingItem(ProjectConstants.UNTITLED_REFERENCE_RECORDING_PREFIX + "Recording2",
                CalibrationOption.Dynamic, DateTime.UtcNow);

            record = WiimoteDataStore.getWiimoteDataStore().addWiimoteReferenceRecord(ProjectConstants.UNTITLED_REFERENCE_NAME + "2",
                10, 10, 10, 1, 0, "Video Path2");

            record.addWiimoteReferenceRecordingItem(ProjectConstants.UNTITLED_REFERENCE_RECORDING_PREFIX + "Recording1",
                CalibrationOption.Dynamic, DateTime.UtcNow);

            record.addWiimoteReferenceRecordingItem(ProjectConstants.UNTITLED_REFERENCE_RECORDING_PREFIX + "Recording2",
                CalibrationOption.Dynamic, DateTime.UtcNow);

            this.m_parent.wiimoteReferenceDataStoreBindingSource.DataSource = WiimoteDataStore.getWiimoteDataStore().WiimoteReferenceRecords;
         */
   
        }

        #endregion

    }
}


/*

        public void playRecordRowSelected(DataGridViewRow l_TableReferenceRow)
        {
            try
            {
                WiimotePlayRecord wiimoteRecord = (WiimotePlayRecord)l_TableReferenceRow.DataBoundItem;
                updateChart(wiimoteRecord, this.m_parent.wiimote1PlayAccChart, this.m_parent.wiimote1PlayGyroChart,
                        this.m_parent.wiimote2PlayAccChart, this.m_parent.wiimote2PlayGyroChart,
                        this.m_parent.showPlayRecordCharts.Checked);

                if (Configuration.getConfiguration().FeedbackOn)
                    showScoreFeedback(wiimoteRecord.Score);
            }
            catch (CSVFileException e)
            {
                MessageBox.Show(e.Message, "Play Record Selection Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

*/
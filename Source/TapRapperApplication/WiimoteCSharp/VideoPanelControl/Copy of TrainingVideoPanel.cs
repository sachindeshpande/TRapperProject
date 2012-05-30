using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using ProjectCommon;
using VideoWrapper;
using Utilities;
using WiimoteData;
using MatlabWrapper;
using SpeechTest;

namespace VideoPanelControl
{
    public partial class TrainingVideoPanel : UserControl
    {        
        private IVideoPlayer mVideoPlayer;
        private List<TrainingSegmentInfo> mVideoInfoList;
        private string m_VideoPath;
        Graphics g;

        //The flag is set to true when training is started
        private bool mTrainingStarted;
  
        //This flag is set to true when wiimote recording is started
        //It is immediately set to false when wiimote recording is done
        private bool mRecordingStarted;

        private int mCurrentVideoIndex;
        private TrainingSegmentInfo mCurrentTrainingSegment;

        private Wiimotes mWiimotes;

        //For Feedback
        private Image mImage;
        private FontStyle mFontStyle;
        private int mFSize;
        private Font mFontFeedback;

        private SpeechRecognition mSpeechRecognition;

        delegate void SetPanelDimensionsCallback(int pVideoWidth, int pVideoHeight);
        delegate void ClickContinueButtonCallback();

        public delegate void OnTrainingSelectionEvent(object sender, TrainingSelectionEventArgs args);
        public event OnTrainingSelectionEvent TrainingSelectionEvent;

        delegate void SetUserControlStatusCallback(bool pStatus);
        delegate void SetCurentPlayNameCallback();
        delegate void SetScoreFeedbackCallback(string pFeedback,int pNumStars);

        public TrainingVideoPanel()
        {
            InitializeComponent();
            mTrainingStarted = false;
            mRecordingStarted = false;
            mCurrentVideoIndex = 0;

            this.RepeatCommand.Enabled = false;

            selectVideoFile(Configuration.getConfiguration().TrainingVideoFile);
            this.currentPlayName.Text = ProjectConstants.TRAINING_STARTING_MESSAGE_CURRENT_PLAY_LABEL;

        }

        public void setWiimotesObject(Wiimotes pWiimotes)
        {
            mWiimotes = pWiimotes;
            mWiimotes.WiimoteDisconnectedEvent += new Wiimotes.OnWiimoteDisconnectedEvent(OnWiimoteDisconnectedEvent);

            mSpeechRecognition = SpeechRecognition.getSpeechRecognition();
            mSpeechRecognition.VoiceCommandReceivedEvent += new SpeechRecognition.OnVoiceCommandReceivedEvent(OnVoiceCommandReceivedEvent);
            mSpeechRecognition.startEngine();
        }

        public Panel getTrainingVideoPlayerPanel()
        {
            return this.TrainingVideoPlayPanel;
        }

        public Panel getStepPanel()
        {
            return null;
        }

        #region GUI Delegates

        internal void SetPanelDimensions(int pVideoWidth,int pVideoHeight)
        {
            Panel videoPanel = getTrainingVideoPlayerPanel();

            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (videoPanel.InvokeRequired)
            {
                SetPanelDimensionsCallback d = new SetPanelDimensionsCallback(SetPanelDimensions);
                this.Invoke(d, new object[] { pVideoWidth,pVideoHeight });
            }
            else
            {
                videoPanel.Width = pVideoWidth;
                videoPanel.Height = pVideoHeight;
            }
        }

        internal void SetCurentPlayName()
        {
            if (mVideoInfoList.Count == 0)
            {
                MessageBox.Show("Training Data not Loaded",
                "Tap Rapper Says", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
                return;
            }
            
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.currentPlayName.InvokeRequired)
            {
                SetCurentPlayNameCallback d = new SetCurentPlayNameCallback(SetCurentPlayName);
                this.Invoke(d, new object[] { });
            }
            else
            {
                TrainingSegmentInfo lNextTrainingSegment = mVideoInfoList[mCurrentVideoIndex];
                this.currentPlayName.Text = ProjectConstants.TRAINING_CURRENT_PLAY_HEADING + "\r\n" + lNextTrainingSegment.TrainingReferenceRecord.RecordName;
            }
        }

        internal void SetScoreFeedback(string pFeedback, int pNumStars)
        {
            
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.scoreFeedback.InvokeRequired)
            {
                SetScoreFeedbackCallback d = new SetScoreFeedbackCallback(SetScoreFeedback);
                this.Invoke(d, new object[] { pFeedback, pNumStars });
            }
            else
            {
//                this.scoreFeedback.Text = pFeedback;
                Graphics g = this.scoreFeedback.CreateGraphics();

                for(int i= 0; i < pNumStars; i++)
                    g.DrawImage(mImage, 50 + i * 75, 25, 75, 75);


                g.DrawString(pFeedback, mFontFeedback, System.Drawing.Brushes.Yellow, 50, 150);

//                this.scoreFeedback.Text = "\r\n\r\n\r\n\r\n" + pFeedback;
                g.Dispose();
            }
        }

        internal void ClickContinueButton()
        {

            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.ContinueCommand.InvokeRequired)
            {
                ClickContinueButtonCallback d = new ClickContinueButtonCallback(ClickContinueButton);
                this.Invoke(d, new object[] {  });
            }
            else
            {
                this.ContinueCommand.PerformClick();
            }
        }

        #endregion

        public void initializeTraining()
        {

            try
            {
//                mWiimotes.connectWiimotes(Configuration.getConfiguration().MaxWiimoteConnectionTries);
                initializeFeedback();
                loadTrainingVideoInfoData();

                if (mVideoInfoList.Count == 0)
                    return; // throw exception here

                mTrainingStarted = true;
                mCurrentTrainingSegment = mVideoInfoList[0];
                this.RepeatCommand.Enabled = true;

            }
            catch (WiimoteConnectionException ex)
            {                
                Console.WriteLine(ex);
                throw ex;
            }
        }

        private void initializeFeedback()
        {
            mImage = Image.FromFile(ProjectConstants.STEP_ICONS_MEDIA_PATH + @"\" + ProjectConstants.TRAINING_RATING_IMAGE_FILENAME);


            mFSize = (int)(25);
            mFontStyle = new FontStyle();
            mFontStyle = mFontStyle | FontStyle.Bold;
            mFontFeedback = new Font("Impact", mFSize, mFontStyle,
                System.Drawing.GraphicsUnit.Point);
        }


        private void selectVideoFile(string videoPath)
        {
            m_VideoPath = videoPath;
        }

        private void connectWiimotes()
        {
            mWiimotes.connectWiimotes();
        }

        private void OpenTrainingVideo(string pVideoInfo, VideoUserOptions pOptions)
        {
            Panel videoPanel = getTrainingVideoPlayerPanel();

            // open the video
            // remember the original dimensions of the panel
            int height = videoPanel.Height;
            int width = videoPanel.Width;

            mVideoPlayer = VideoPlayerFactory.getVideoPlayer(ProjectConstants.DIRECT_SHOW_VIDEO_PLAYER_NAME, pVideoInfo, videoPanel, getStepPanel(), pOptions);


            SetPanelDimensions(width * 3 / 2, height);

            // play the first frame of the video so we can identify it
            mVideoPlayer.load();

        }


        public void loadTrainingVideoInfoData()
        {
            WiimoteDataStore dataStore = WiimoteDataStore.getWiimoteDataStore();
            int lNumRecords = dataStore.getNumberOfWiimoteReferenceRecords();

            mVideoInfoList = new List<TrainingSegmentInfo>();

            for (int lIndex = 0; lIndex < lNumRecords; lIndex++)
            {
                WiimoteReferenceRecord lReferenceRecord = dataStore.getWiimoteReferenceRecord(lIndex);

                if (lReferenceRecord.VideoPath != null && lReferenceRecord.VideoPath.CompareTo("") != 0)
                    mVideoInfoList.Add(new TrainingSegmentInfo(lReferenceRecord));
            }

        }

        private void playTrainingVideo(VideoUserOptions pOptions)
        {
            try
            {
                OpenTrainingVideo(ProjectConstants.PROJECT_MEDIA_PATH + @"\" + mCurrentTrainingSegment.TrainingReferenceRecord.VideoPath, pOptions);
                mVideoPlayer.VideoPlayCompleteEvent += new EventHandler(videoCompletedEvent);
                mVideoPlayer.FrameMilestoneEvent += new EventHandler(frameMilestoneEvent);

                //Check to see if there is an event (Recording event) in the frame info.
                //If there is a recording  event it indicates that recording needs to be started at that time
                //and not right in the beginning
                System.Collections.ObjectModel.ReadOnlyCollection<string> lEventList = mVideoPlayer.getFrameEventList();
                bool lWiimoteEvent = false;
                foreach (string lEventName in lEventList)
                {
                    if(lEventName.CompareTo(ProjectConstants.TRAINING_START_RECORDING_EVENT_STRING) == 0)
                        lWiimoteEvent = true;
                }


                //If there is no recording event , the recording can be started right away
                //Also start Wiimote recording a little earlier than the video play so there is some buffer recording
                //before the actual movement starts
                if(!lWiimoteEvent)
                    startRecording(Configuration.getConfiguration().TrainingPreRecordingTime);
                SpeechRecognition.getSpeechRecognition().startEngine();

                mVideoPlayer.play();
            }
            catch (WiimoteConnectionException ex)
            {
                throw;
            }
        }

        private void startRecording(int pPrerecordingTime)
        {
            //setup Wiimote recording
            if (mCurrentTrainingSegment.TrainingReferenceRecord.SelectedRecording != null)
            {
                WiimoteDataStore dataStore = WiimoteDataStore.getWiimoteDataStore();
                string lDateString = DateTime.Now.Month.ToString() + "_" + DateTime.Now.Day.ToString() + "_" + DateTime.Now.Year.ToString() +
                    "_" + DateTime.Now.Hour.ToString() + "_" + DateTime.Now.Minute.ToString() + "_" + DateTime.Now.Second.ToString();

                string lPlayname = "Play" + mCurrentTrainingSegment.TrainingReferenceRecord.RecordName + "_" + lDateString;
                mCurrentTrainingSegment.TrainingPlayRecord =
                    dataStore.addWiimotePlayRecord(mCurrentTrainingSegment.TrainingReferenceRecord, lPlayname, DateTime.Today, 0);
                mWiimotes.startRecording(mCurrentTrainingSegment.TrainingPlayRecord, false, this, "", false);
                mRecordingStarted = true;
                System.Threading.Thread.Sleep(pPrerecordingTime);
            }
        }

        private void continueTraining(VideoUserOptions pOptions)
        {
            try
            {
                if (!mTrainingStarted)
                    initializeTraining();

                if (mCurrentVideoIndex == mVideoInfoList.Count)
                    return;

                mCurrentTrainingSegment = mVideoInfoList[mCurrentVideoIndex];
                playTrainingVideo(pOptions);

            }
            catch (WiimoteConnectionException ex)
            {
                throw ex;
            }
        }

        private void stopTrainingVideo()
        {
        }

        #region GUI EventHandlers
        private void ContinueCommand_Click(object sender, EventArgs e)
        {
            try
            {
                VideoUserOptions lOptions = new VideoUserOptions();
                continueTraining(lOptions);
            }
            catch (WiimoteConnectionException ex)
            {
                MessageBox.Show("Tap Rapper : Yo .. Connect your wiimotes",
                "Tap Rapper Says", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            }

        }

        private void RepeatCommand_Click(object sender, EventArgs e)
        {
            mCurrentVideoIndex = mCurrentVideoIndex - 1;
            VideoUserOptions lOptions = new VideoUserOptions();

            mCurrentTrainingSegment = mVideoInfoList[mCurrentVideoIndex];
            playTrainingVideo(lOptions);
        }

        private void PreviousFrameCommand_Click(object sender, EventArgs e)
        {
            mCurrentVideoIndex = mCurrentVideoIndex - 1;
            SetCurentPlayName();
        }

        private void nextVideoCommand_Click(object sender, EventArgs e)
        {
            mCurrentVideoIndex = mCurrentVideoIndex + 1;
            SetCurentPlayName();
        }

        private void SlowMoCommand_Click(object sender, EventArgs e)
        {
            try
            {
                VideoUserOptions lOptions = new VideoUserOptions();
                lOptions.SlowMo = true;
                continueTraining(lOptions);
            }
            catch (WiimoteConnectionException ex)
            {
                MessageBox.Show("Tap Rapper : Yo .. Connect your wiimotes",
                "Tap Rapper Says", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            }

        }

        #endregion


        #region Async Event Handlers
        public void frameMilestoneEvent(object sender, EventArgs args)
        {
            try
            {
                if (!mRecordingStarted)
                {
                    ConsoleLogger.logMessage("TrainingVideoPanel : In frameMilestoneEvent");
                    startRecording(0); //No need to have pre recording time here. 
                    //This is the case where recording starts in the middle of the frame
                }
            }
            catch (WiimoteConnectionException ex)
            {
                MessageBox.Show("Tap Rapper : Yo .. Connect your wiimotes",
                "Tap Rapper Says", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
                throw ex;
            }
        }

        public void videoCompletedEvent(object sender, EventArgs args)
        {
            ConsoleLogger.logMessage("In videoCompletedEvent");

            mVideoPlayer.dispose();
            if (mRecordingStarted)
            {
                mWiimotes.stopRecording();
                mRecordingStarted = false;

                mWiimotes.comparePlayToReference(mCurrentTrainingSegment.TrainingPlayRecord);

                int lNumStars = mCurrentTrainingSegment.TrainingPlayRecord.NumberOfStars;
                if (lNumStars == 1)
                    lNumStars = 2;

                StringBuilder lFeedbackMessage = new StringBuilder();

//                for (int lIndex = 0; lIndex < lNumStars; lIndex++)
//                    lFeedbackMessage.Append("*  ");
//                lFeedbackMessage.Append("   :   ");

                switch (lNumStars)
                {
                    case 1: lFeedbackMessage.Append(ProjectConstants.TRAINING_FEEDBACK_FOR_ONE_STAR);
                        break;
                    case 2: lFeedbackMessage.Append(ProjectConstants.TRAINING_FEEDBACK_FOR_TWO_STARS);
                        break;
                    case 3: lFeedbackMessage.Append(ProjectConstants.TRAINING_FEEDBACK_FOR_THREE_STARS);
                        break;
                    case 4: lFeedbackMessage.Append(ProjectConstants.TRAINING_FEEDBACK_FOR_FOUR_STARS);
                        break;
                    case 5: lFeedbackMessage.Append(ProjectConstants.TRAINING_FEEDBACK_FOR_FIVE_STARS);
                        break;
                    default:
                        break;
                }

                mVideoPlayer.addInformation(mCurrentTrainingSegment.TrainingReferenceRecord.RecordName,lNumStars);
                SetScoreFeedback(lFeedbackMessage.ToString(), lNumStars);
            }

            if (mCurrentVideoIndex == mVideoInfoList.Count - 1)
            {
                //Last Video done
                return;
            }

            mCurrentVideoIndex++;

            if (mCurrentTrainingSegment.TrainingReferenceRecord.NextVideoPlay.CompareTo(ProjectConstants.TRAINING_AUTOMATIC_NEXT_VIDEO_PLAY) == 0)
                ClickContinueButton();
            else if (mCurrentVideoIndex == mVideoInfoList.Count - 1 || mCurrentTrainingSegment.TrainingReferenceRecord.NextVideoPlay.CompareTo(ProjectConstants.TRAINING_LAST_VIDEO_PLAY) == 0)
            {
                //Code for end
            }

            SetCurentPlayName();

        }

        public void OnWiimoteDisconnectedEvent(object sender, WiimoteUpdateEventArgs e)
        {
            if (mRecordingStarted)
            {

            }
        }

        public void OnVoiceCommandReceivedEvent(Object sender, VoiceCommandEventArgs args)
        {
            ConsoleLogger.logMessage("In OnVoiceCommandReceivedEvent : Command is " + args.VoiceCommandValue);
            if (mVideoPlayer == null)
                return;

            if (args.VoiceCommandValue.CompareTo(VoiceCommandEventArgs.STOP_COMMAND) == 0 &&
                mVideoPlayer.getVideoState() == VideoState.Running)
            {
                ConsoleLogger.logMessage("Executing Stop");
                mVideoPlayer.dispose();
                if (mRecordingStarted)
                {
                    mWiimotes.stopRecording();
                    mRecordingStarted = false;
                }
                mTrainingStarted = false;
            }
        }

        protected override void WndProc(ref Message m)
        {
            
            int WM_LBUTTONDOWN = 513; // 0x0201
            int WM_LBUTTONUP = 514; // 0x0202
            int WM_LBUTTONDBLCLK = 515; // 0x0203
            int WM_RBUTTONDOWN = 516; // 0x0204
            int WM_PARENTNOTIFY = 528; //0x210

            ConsoleLogger.logMessage("In WndProc : " + m.ToString() + " : Msg = " + m.Msg);
            if (m.Msg == WM_PARENTNOTIFY)
            {
                if ((int)m.WParam == WM_RBUTTONDOWN)
                {
                    ConsoleLogger.logMessage("In WndProc Right Button : " + (int)m.WParam);
                    pause();
                }
            }

            base.WndProc(ref m);
        }

        #endregion

        public void pause()
        {
            if (mVideoPlayer != null)
                mVideoPlayer.pause();
        }

        public void stop()
        {
            if (mVideoPlayer != null)
                mVideoPlayer.stop();
        }
    }     

}

/*
        public void loadTrainingVideoInfoData()
        {
            CSVFileParser l_Parser = new CSVFileParser();
            l_Parser.startParsingCSVData(ProjectConstants.TRAINING_VIDEO_INFO_PATH, 0, 0);

            string[] row;

            mVideoInfoList = new List<TrainingSegmentInfo>();

            while ((row = l_Parser.getNextRow(0)) != null)
            {
                mVideoInfoList.Add(new TrainingSegmentInfo(ProjectConstants.PROJECT_MEDIA_PATH + @"\" + row[0],
                    row[1]));
            }

        }
 * 
         internal void SetUserControlStatus(bool pShowStatus)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.RepeatSession.InvokeRequired)
            {
                SetUserControlStatusCallback d = new SetUserControlStatusCallback(SetUserControlStatus);
                this.Invoke(d, new object[] { pShowStatus });
            }
            else
            {
                this.RepeatSession.Visible = pShowStatus;
            }
        }
        

        public void showUserControls(bool pShowStatus)
        {
            SetUserControlStatus(pShowStatus);
        }
 *
        private void continueTraining(VideoUserOptions pOptions)
        {
            try
            {
                if (!mTrainingStarted)
                {
                    startTraining(pOptions);
                }
                else
                {
                    if (mCurrentVideoIndex == mVideoInfoList.Count)
                        return;

                    mCurrentVideoIndex++;
                    mCurrentTrainingSegment = mVideoInfoList[mCurrentVideoIndex];
                    playTrainingVideo(pOptions);
                }
            }
            catch (WiimoteConnectionException ex)
            {
                throw ex;
            }
        }
 * 
        public void startTraining(VideoUserOptions pOptions)
        {

            try
            {
//                mWiimotes.connectWiimotes(Configuration.getConfiguration().MaxWiimoteConnectionTries);

                loadTrainingVideoInfoData();

                if (mVideoInfoList.Count == 0)
                    return; // throw exception here

                mTrainingStarted = true;
                mCurrentVideoIndex++;
                mCurrentTrainingSegment = mVideoInfoList[0];
                playTrainingVideo(pOptions);
                this.RepeatCommand.Enabled = true;
            }
            catch (WiimoteConnectionException ex)
            {                
                Console.WriteLine(ex);
                throw ex;
            }
        }
 * 
         public void videoCompletedEvent(object sender, EventArgs args)
        {
            ConsoleLogger.logMessage("In videoCompletedEvent");
            mVideoPlayer.dispose();
            if (mRecordingStarted)
            {
                mWiimotes.stopRecording();
                mRecordingStarted = false;

                //TODO : This is a temporary hack to check for Pop Overs
                if (!mCurrentTrainingSegment.TrainingReferenceRecord.RecordName.Contains("Pop"))
                {

                    int lScore = 0;
                    if (lScore < ProjectConstants.TRAINING_THRESHOLD_SCORE)
                        SetScoreFeedback(ProjectConstants.TRAINING_FEEDBACK_FOR_REPEAT);
                    else
                        SetScoreFeedback(ProjectConstants.TRAINING_FEEDBACK_FOR_CONTINUE);
                }
                else
                {
                    int lNumStars = 3;
                    StringBuilder lFeedbackMessage = new StringBuilder();
                    for (int lIndex = 0; lIndex < lNumStars; lIndex++)
                        lFeedbackMessage.Append("*  ");
                    lFeedbackMessage.Append("   :   ");

                    switch (lNumStars)
                    {
                        case 1: lFeedbackMessage.Append(ProjectConstants.TRAINING_FEEDBACK_FOR_ONE_STAR);
                            break;
                        case 2: lFeedbackMessage.Append(ProjectConstants.TRAINING_FEEDBACK_FOR_TWO_STARS);
                            break;
                        case 3: lFeedbackMessage.Append(ProjectConstants.TRAINING_FEEDBACK_FOR_THREE_STARS);
                            break;
                        case 4: lFeedbackMessage.Append(ProjectConstants.TRAINING_FEEDBACK_FOR_FOUR_STARS);
                            break;
                        case 5: lFeedbackMessage.Append(ProjectConstants.TRAINING_FEEDBACK_FOR_FIVE_STARS);
                            break;
                        default :
                            break;
                    }

                    SetScoreFeedback(lFeedbackMessage.ToString());
                }
            }

            if (mCurrentVideoIndex == mVideoInfoList.Count - 1)
            {
                //Last Video done
                return;
            }


            if (mCurrentTrainingSegment.TrainingReferenceRecord.NextVideoPlay.CompareTo(ProjectConstants.TRAINING_AUTOMATIC_NEXT_VIDEO_PLAY) == 0)
                ClickContinueButton();
            else if (mCurrentVideoIndex == mVideoInfoList.Count - 1 || mCurrentTrainingSegment.TrainingReferenceRecord.NextVideoPlay.CompareTo(ProjectConstants.TRAINING_LAST_VIDEO_PLAY) == 0)
            {
                //Code for end
            }

            mCurrentVideoIndex++;
            SetCurentPlayName();

        }
        
        protected override void OnPaint(PaintEventArgs e)
        {
//            Image lImage = Image.FromFile(ProjectConstants.STEP_ICONS_MEDIA_PATH + @"\" + ProjectConstants.TRAINING_RATING_IMAGE_FILENAME);
//            e.Graphics.DrawImage(lImage, 50, 50, 100, 100);
            if (g != null)
            {
                g.DrawRectangle(Pens.White, new Rectangle(10, 10, 100, 100));
                g.DrawRectangle(Pens.White, new Rectangle(10, 10, 99, 99));
                g.DrawRectangle(Pens.White, new Rectangle(10, 10, 98, 98));
                g.DrawRectangle(Pens.White, new Rectangle(10, 10, 97, 97));
            }

        }

 * 
*/

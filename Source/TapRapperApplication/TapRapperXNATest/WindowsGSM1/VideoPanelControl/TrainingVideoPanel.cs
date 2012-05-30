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

namespace VideoPanelControl
{
    public partial class TrainingVideoPanel : UserControl
    {
        private TrainingVideoRunner mTrainingVideoRunner;
        private Graphics g;

        //For Feedback
        private Image mImage;
        private FontStyle mFontStyle;
        private int mFSize;
        private Font mFontFeedback;

        private bool blnSubMenu;

        delegate void SetPanelDimensionsCallback(int pVideoWidth, int pVideoHeight);
        delegate void ClickContinueButtonCallback();

        public delegate void OnTrainingSelectionEvent(object sender, TrainingSelectionEventArgs args);
        public event OnTrainingSelectionEvent TrainingSelectionEvent;

        delegate void SetUserControlStatusCallback(bool pStatus);


        delegate void SetPreviousPlayNameCallback(string pPlayName);
        delegate void SetCurentPlayNameCallback(string pPlayName);
        delegate void SetNextPlayNameCallback(string pPlayName);

        delegate void SetCommandButtonEnabledCallback(System.Windows.Forms.Button pCommandButton, bool pEnabledStatus);
        delegate void SetScoreFeedbackCallback(string pFeedback,int pNumStars);

        public TrainingVideoPanel()
        {
            InitializeComponent();

            this.RepeatCommand.Enabled = false;

            this.currentPlayName.Text = ProjectConstants.TRAINING_STARTING_MESSAGE_CURRENT_PLAY_LABEL;

            blnSubMenu = false;

        }

        public void setTrainingVideoRunner(TrainingVideoRunner pTrainingVideoRunner)
        {
            mTrainingVideoRunner = pTrainingVideoRunner;
        }

        public void setWiimotesObject(Wiimotes pWiimotes)
        {
            mTrainingVideoRunner.setWiimotesObject(pWiimotes);
        }

        public Panel getTrainingVideoPlayerPanel()
        {
            return this.TrainingVideoPlayPanel;
        }

        public Panel getStepPanel()
        {
            return null;
        }

        public bool isTrainingVideoRunning()
        {
            return mTrainingVideoRunner.isTrainingVideoRunning();
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


        internal void SetPreviousPlayName(string pPlayName)
        {
           
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.previousPlayName.InvokeRequired)
            {
                SetPreviousPlayNameCallback d = new SetPreviousPlayNameCallback(SetPreviousPlayName);
                this.Invoke(d, new object[] { pPlayName });
            }
            else
            {
                this.previousPlayName.Text = pPlayName;
            }
        }


        internal void SetCurentPlayName(string pPlayName)
        {
           
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.currentPlayName.InvokeRequired)
            {
                SetCurentPlayNameCallback d = new SetCurentPlayNameCallback(SetCurentPlayName);
                this.Invoke(d, new object[] { pPlayName });
            }
            else
            {
                this.currentPlayName.Text = pPlayName;
            }
        }

        internal void SetNextPlayName(string pPlayName)
        {

            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.nextPlayName.InvokeRequired)
            {
                SetNextPlayNameCallback d = new SetNextPlayNameCallback(SetNextPlayName);
                this.Invoke(d, new object[] { pPlayName });
            }
            else
            {
                this.nextPlayName.Text = pPlayName;
            }
        }

        internal void SetCommandButtonEnabled(System.Windows.Forms.Button pCommandButton,bool pEnabledStatus)
        {
           
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (pCommandButton.InvokeRequired)
            {
                SetCommandButtonEnabledCallback d = new SetCommandButtonEnabledCallback(SetCommandButtonEnabled);
                this.Invoke(d, new object[] { pCommandButton,pEnabledStatus });
            }
            else
            {
                pCommandButton.Enabled = pEnabledStatus;
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

        internal void SetCurrentSubFrame(string pPlayName)
        {
            this.SubFrame.Text = pPlayName;
        }


        #endregion

        public void initializeTraining()
        {

            try
            {
//                mWiimotes.connectWiimotes(Configuration.getConfiguration().MaxWiimoteConnectionTries);
                initializeFeedback();
                this.RepeatCommand.Enabled = true;

                mTrainingVideoRunner.initializeTraining();

            }
            catch (WiimoteConnectionException ex)
            {                
                Console.WriteLine(ex);
                throw ex;
            }
        }

        public void gotoFirstSegment()
        {
            mTrainingVideoRunner.gotoFirstSegment();
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

        public void setScoringButtonEnabled(bool pButtonEnabledOption)
        {
            SetCommandButtonEnabled(this.scoringCommand, pButtonEnabledOption);
        }

        public void setSlowMoButtonEnabled(bool pButtonEnabledOption)
        {
            SetCommandButtonEnabled(this.SlowMoCommand, pButtonEnabledOption);
        }

        public void setRepeatButtonEnabled(bool pButtonEnabledOption)
        {
            SetCommandButtonEnabled(this.RepeatCommand, pButtonEnabledOption);
        }

        #region GUI EventHandlers
        private void ContinueCommand_Click(object sender, EventArgs e)
        {
            try
            {
//                mTrainingVideoRunner.continueTraining();
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
            mTrainingVideoRunner.playTraining();
        }

        private void PreviousFrameCommand_Click(object sender, EventArgs e)
        {
            mTrainingVideoRunner.previousFrame();
        }

        private void nextVideoCommand_Click(object sender, EventArgs e)
        {
            mTrainingVideoRunner.nextFrame();
        }

        private void SlowMoCommand_Click(object sender, EventArgs e)
        {
            try
            {
                if (!mTrainingVideoRunner.slowMoTraining())
                {
                    
                    MessageBox.Show("Tap Rapper : SlowMo Option Not Available here",
                    "Tap Rapper Says", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                }
            }
            catch (WiimoteConnectionException ex)
            {
                MessageBox.Show("Tap Rapper : Yo .. Connect your wiimotes",
                "Tap Rapper Says", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            }

        }

        private void scoringCommand_Click(object sender, EventArgs e)
        {
            try
            {
                if (!mTrainingVideoRunner.scoreTraining())
                    MessageBox.Show("Tap Rapper : Scoring Option Not Available here\r\nSelect 1)Repeat , 2) SlowMo or 3)Continue",
                    "Tap Rapper Says", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            catch (WiimoteConnectionException ex)
            {
                MessageBox.Show("Tap Rapper : Yo .. Connect your wiimotes",
                "Tap Rapper Says", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            }
        }

        #endregion

        protected override bool ProcessCmdKey(ref Message m, Keys keyData)
        {
            bool blnProcess = false;

            if (keyData == Keys.Right)
            {
                // Process the keystroke
                blnProcess = true;
                if (blnSubMenu)
                    mTrainingVideoRunner.nextSubFrame();
                else
                    mTrainingVideoRunner.nextFrame();
            }
            else if (keyData == Keys.Left)
            {
                // Process the keystroke
                blnProcess = true;
                if (blnSubMenu)
                    mTrainingVideoRunner.previousSubFrame();
                else
                    mTrainingVideoRunner.previousFrame();
            }
            else if (keyData == Keys.Up)
            {
                // Process the keystroke
                blnProcess = true;
                blnSubMenu = false;
            }
            else if (keyData == Keys.Down)
            {
                // Process the keystroke
                blnProcess = true;
                blnSubMenu = true;
            }


            if (blnProcess == true)
                return true;
            else
                return base.ProcessCmdKey(ref m, keyData);
        }



    }     

}

/*
 * 
         protected override void WndProc(ref Message m)
        {
            
            int WM_LBUTTONDOWN = 513; // 0x0201
            int WM_LBUTTONUP = 514; // 0x0202
            int WM_LBUTTONDBLCLK = 515; // 0x0203
            int WM_RBUTTONDOWN = 516; // 0x0204
            int WM_PARENTNOTIFY = 528; //0x210

            base.WndProc(ref m);
        }

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

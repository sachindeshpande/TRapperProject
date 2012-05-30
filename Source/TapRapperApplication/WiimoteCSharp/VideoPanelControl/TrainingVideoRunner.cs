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
    public partial class TrainingVideoRunner
    {
        private TrainingVideoPanel mOutputParent;
        private IVideoPlayer mVideoPlayer;
//        private List<TrainingSegmentInfo> mVideoInfoList;
        private string m_VideoPath;

        //The flag is set to true when training is started
        private bool mTrainingStarted;
  
        //This flag is set to true when wiimote recording is started
        //It is immediately set to false when wiimote recording is done
        private bool mRecordingStarted;

        private int mCurrentVideoIndex;
        private int mCurrentSubVideoIndex;
        private ITrainingSegmentInfo mCurrentTrainingSegment;

        private Wiimotes mWiimotes;
        private string[] START_VIDEO_CODE = { "-1" };
        private string[] END_VIDEO_CODE = { "-9" };


        public TrainingVideoRunner(TrainingVideoPanel pOuputParent)
        {
            mOutputParent = pOuputParent;
            mTrainingStarted = false;
            mRecordingStarted = false;
            mCurrentVideoIndex = 0;
            mCurrentSubVideoIndex = 0;

            selectVideoFile(Configuration.getConfiguration().TrainingVideoFile);

        }

        public void initializeTraining()
        {

            try
            {

                loadTrainingVideoInfoData();

                if (WiimoteDataStore.getWiimoteDataStore().getNumberOfTrainingSegmentInfoRecords() == 0)
                    return; // throw exception here

                mTrainingStarted = true;
                mCurrentTrainingSegment = WiimoteDataStore.getWiimoteDataStore().getTrainingSegmentInfo(0);

            }
            catch (WiimoteConnectionException ex)
            {
                Console.WriteLine(ex);
                throw ex;
            }
        }

        #region Getters/Setters

        public void setWiimotesObject(Wiimotes pWiimotes)
        {
            mWiimotes = pWiimotes;
            mWiimotes.WiimoteDisconnectedEvent += new Wiimotes.OnWiimoteDisconnectedEvent(OnWiimoteDisconnectedEvent);
//            mWiimotes.WiimoteActionEvent += new Wiimotes.OnWiimoteActionEvent(OnWiimoteActionEvent);
        }


        private string getFormattedPlayName(ITrainingSegmentInfo lSegmentInfo)
        {
            StringBuilder lPlayName = new StringBuilder();

            lPlayName.Append(lSegmentInfo.RecordName);

            return lPlayName.ToString();
        }


        public bool isTrainingVideoRunning()
        {
            if (mVideoPlayer != null && mVideoPlayer.getVideoState() == VideoState.Running)
                return true;
            return false;
        }

        public bool isTrainingVideoPaused()
        {
            if (mVideoPlayer != null && mVideoPlayer.getVideoState() == VideoState.Paused)
                return true;
            return false;
        }

        private void selectVideoFile(string videoPath)
        {
            m_VideoPath = videoPath;
        }

        #endregion

        #region Private Helpers

        private void connectWiimotes()
        {
            mWiimotes.connectWiimotes();
        }

        private void OpenTrainingVideo(string pVideoInfo, VideoUserOptions pOptions)
        {
            Panel videoPanel = mOutputParent.getTrainingVideoPlayerPanel();

            // open the video
            // remember the original dimensions of the panel
            int height = videoPanel.Height;
            int width = videoPanel.Width;

            mVideoPlayer = VideoPlayerFactory.getVideoPlayer(ProjectConstants.DIRECT_SHOW_VIDEO_PLAYER_NAME, pVideoInfo, videoPanel, null, pOptions);
            //mVideoPlayer = VideoPlayerFactory.getVideoPlayer(ProjectConstants.DUMMY_VIDEO_PLAYER_NAME, pVideoInfo, videoPanel, null, pOptions);


            mOutputParent.SetPanelDimensions(width * 3 / 2, height);

            // play the first frame of the video so we can identify it
            mVideoPlayer.load();

        }


        private void loadTrainingVideoInfoData()
        {
            /*
            WiimoteDataStore dataStore = WiimoteDataStore.getWiimoteDataStore();
            int lNumRecords = dataStore.getNumberOfTrainingSegmentInfoRecords();

            mVideoInfoList = new List<TrainingSegmentInfo>();

            for (int lIndex = 0; lIndex < lNumRecords; lIndex++)
            {
                TrainingSegmentInfo lReferenceRecord = dataStore.getTrainingSegmentInfo(lIndex);

                if (lReferenceRecord.VideoPath != null && lReferenceRecord.VideoPath.CompareTo("") != 0)
                    mVideoInfoList.Add(new TrainingSegmentInfo(lReferenceRecord));
            }
          * */

        }

        private void playTrainingVideo(IWiimoteReferenceRecord pReferenceRecord, VideoUserOptions pOptions)
        {
            try
            {
                OpenTrainingVideo(ProjectConstants.PROJECT_MEDIA_PATH + @"\" + pReferenceRecord.VideoPath, pOptions);
                mVideoPlayer.VideoPlayCompleteEvent += new EventHandler(videoCompletedEvent);
                mVideoPlayer.FrameMilestoneEvent += new EventHandler(frameMilestoneEvent);

                if (pReferenceRecord.ScoringOption)
                {


                    //Check to see if there is an event (Recording event) in the frame info.
                    //If there is a recording  event it indicates that recording needs to be started at that time
                    //and not right in the beginning
                    System.Collections.ObjectModel.ReadOnlyCollection<string> lEventList = mVideoPlayer.getFrameEventList();
                    bool lWiimoteEvent = false;
                    if (lEventList != null)
                    {
                        foreach (string lEventName in lEventList)
                        {
                            if (lEventName.CompareTo(ProjectConstants.TRAINING_START_RECORDING_EVENT_STRING) == 0)
                                lWiimoteEvent = true;
                        }
                    }


                    //If there is no recording event , the recording can be started right away
                    //Also start Wiimote recording a little earlier than the video play so there is some buffer recording
                    //before the actual movement starts
                    if (!lWiimoteEvent)
                        startRecording(pReferenceRecord, Configuration.getConfiguration().TrainingPreRecordingTime);
                }

                mVideoPlayer.play();

                if (pReferenceRecord.ScoringOption)
                    mWiimotes.addInformationToRecording(START_VIDEO_CODE);
            }
            catch (WiimoteConnectionException ex)
            {
                throw;
            }
        }

        private void startRecording(IWiimoteReferenceRecord pReferenceRecord,int pPrerecordingTime)
        {
            //setup Wiimote recording

            WiimoteDataStore dataStore = WiimoteDataStore.getWiimoteDataStore();
            string lDateString = DateTime.Now.Month.ToString() + "_" + DateTime.Now.Day.ToString() + "_" + DateTime.Now.Year.ToString() +
                "_" + DateTime.Now.Hour.ToString() + "_" + DateTime.Now.Minute.ToString() + "_" + DateTime.Now.Second.ToString();

            string lPlayname = "Play" + pReferenceRecord.RecordName + "_" + lDateString;
            IWiimotePlayRecord lPlayRecord = mCurrentTrainingSegment.addWiimotePlayRecord(lPlayname, DateTime.Today, 0);
            mWiimotes.startRecording(lPlayRecord, false, this, "", false);
            mRecordingStarted = true;
            System.Threading.Thread.Sleep(pPrerecordingTime);
        }

        private void playTraining(IWiimoteReferenceRecord pReferenceRecord, VideoUserOptions pOptions)
        {
            try
            {
                if (!mTrainingStarted)
                    initializeTraining();

                if (isTrainingVideoRunning())
                    return;

                if (isTrainingVideoPaused())
                {
                    mVideoPlayer.resume();
                    return;
                }

                if (mCurrentVideoIndex == WiimoteDataStore.getWiimoteDataStore().getNumberOfTrainingSegmentInfoRecords())
                    return;

                playTrainingVideo(pReferenceRecord,pOptions);

            }
            catch (WiimoteConnectionException ex)
            {
                throw ex;
            }
        }

        private ITrainingSegmentInfo getPreviousSegment()
        {
            if (mCurrentVideoIndex == 0)
                return null;
            else
                return WiimoteDataStore.getWiimoteDataStore().getTrainingSegmentInfo(mCurrentVideoIndex - 1);
        }

        private ITrainingSegmentInfo getNextSegment()
        {
            if (mCurrentVideoIndex == WiimoteDataStore.getWiimoteDataStore().getNumberOfTrainingSegmentInfoRecords() -1)
                return WiimoteDataStore.getWiimoteDataStore().getTrainingSegmentInfo(0);
            else
                return WiimoteDataStore.getWiimoteDataStore().getTrainingSegmentInfo(mCurrentVideoIndex+1);
        }

        private void setCommandOptions()
        {
            if (mCurrentTrainingSegment.ScoringOption)
                mOutputParent.setScoringButtonEnabled(true);
            else
                mOutputParent.setScoringButtonEnabled(false);

            if (mCurrentTrainingSegment.SlowMoOption)
                mOutputParent.setSlowMoButtonEnabled(true);
            else
                mOutputParent.setSlowMoButtonEnabled(false);

            mOutputParent.setRepeatButtonEnabled(true);
        }

        #endregion

        #region Commands

        public bool playTraining()
        {
            VideoUserOptions lVideoUserOptions = new VideoUserOptions();
            IWiimoteReferenceRecord lRecord = mCurrentTrainingSegment.getReferenceRecord(mCurrentSubVideoIndex);

            if (lRecord.SlowMoOption)
                lVideoUserOptions.SlowMo = true;

            playTraining(lRecord, lVideoUserOptions);
            return true;
        }

        public bool slowMoTraining()
        {
            if (!mCurrentTrainingSegment.SlowMoOption)
                return false;

            VideoUserOptions lOptions = new VideoUserOptions();
            lOptions.SlowMo = true;

            IWiimoteReferenceRecord lSlowMoReferenceRecord = mCurrentTrainingSegment.getSlowMoReferenceRecord();
            if (lSlowMoReferenceRecord == null)
                return false;

            playTraining(lSlowMoReferenceRecord,lOptions);
            return true;
        }

        public bool scoreTraining()
        {
            if (!mCurrentTrainingSegment.ScoringOption)
                return false;

            IWiimoteReferenceRecord lScoringReferenceRecord = mCurrentTrainingSegment.getScoringReferenceRecord();
            if (lScoringReferenceRecord == null)
                return false;

            VideoUserOptions lOptions = new VideoUserOptions();

            playTraining(lScoringReferenceRecord, lOptions);
            return true;
        }

        private void setCurrentPlay()
        {
            ITrainingSegmentInfo lSegmentInfo = getPreviousSegment();
            if (lSegmentInfo != null)
                mOutputParent.SetPreviousPlayName(getFormattedPlayName(lSegmentInfo));
            else
                mOutputParent.SetPreviousPlayName("");

            if (mCurrentTrainingSegment != null && WiimoteDataStore.getWiimoteDataStore().getNumberOfTrainingSegmentInfoRecords() > 0)
                mOutputParent.SetCurentPlayName(getFormattedPlayName(mCurrentTrainingSegment));
            else
                mOutputParent.SetCurentPlayName("");

            lSegmentInfo = getNextSegment();
            if (lSegmentInfo != null)
                mOutputParent.SetNextPlayName(getFormattedPlayName(lSegmentInfo));
            else
                mOutputParent.SetNextPlayName("");

            setCurrentSubFrame();
        }


        public bool gotoFirstSegment()
        {
            if (mVideoPlayer != null &&
                (mVideoPlayer.getVideoState() == VideoState.Running ||
                mVideoPlayer.getVideoState() == VideoState.Paused))
                return false;

            if (WiimoteDataStore.getWiimoteDataStore().getNumberOfTrainingSegmentInfoRecords() == 0)
                throw new TrainingVideoException("No Training Data Loaded");

            mCurrentVideoIndex = 0;
            mCurrentSubVideoIndex = 0;
            mCurrentTrainingSegment = WiimoteDataStore.getWiimoteDataStore().getTrainingSegmentInfo(mCurrentVideoIndex);
            setCurrentPlay();
            setCommandOptions();
            return true;
        }

        public bool previousFrame()
        {
            if (mVideoPlayer != null &&
                (mVideoPlayer.getVideoState() == VideoState.Running ||
                mVideoPlayer.getVideoState() == VideoState.Paused))
                return false;

            if (mCurrentVideoIndex > 0)
                mCurrentVideoIndex = mCurrentVideoIndex - 1;
            mCurrentTrainingSegment = WiimoteDataStore.getWiimoteDataStore().getTrainingSegmentInfo(mCurrentVideoIndex);
            mCurrentSubVideoIndex = 0;

            setCurrentPlay();
            setCommandOptions();
            return true;
        }

        public bool nextFrame()
        {
            if (mVideoPlayer != null &&
                (mVideoPlayer.getVideoState() == VideoState.Running ||
                mVideoPlayer.getVideoState() == VideoState.Paused))
                return false;

            mCurrentVideoIndex = (mCurrentVideoIndex + 1) % WiimoteDataStore.getWiimoteDataStore().getNumberOfTrainingSegmentInfoRecords();
            mCurrentTrainingSegment = WiimoteDataStore.getWiimoteDataStore().getTrainingSegmentInfo(mCurrentVideoIndex);
            mCurrentSubVideoIndex = 0;

            setCurrentPlay();
            setCommandOptions();
            return true;
        }

        public bool pause()
        {
            if (mVideoPlayer != null)
                mVideoPlayer.pause();

            return true;
        }

        public bool stop()
        {
            if (mVideoPlayer == null)
                return false;

            if (mVideoPlayer.getVideoState() != VideoState.Running)
                return false;

            mVideoPlayer.stop();

            ConsoleLogger.logMessage("Executing Stop");
            mVideoPlayer.dispose();
            if (mRecordingStarted)
            {
                mWiimotes.stopRecording();
                mRecordingStarted = false;
            }

            mTrainingStarted = false;

            return true;
        }

        #endregion

        #region SubFrame Commands

        private void setCurrentSubFrame()
        {
            IWiimoteReferenceRecord lRecord = mCurrentTrainingSegment.getReferenceRecord(mCurrentSubVideoIndex);
//            mCurrentTrainingSegment.TrainingReferenceRecord = lRecord;
            mOutputParent.SetCurrentSubFrame(mCurrentSubVideoIndex.ToString() + " : " + lRecord.RecordName);
        }

        public void nextSubFrame()
        {
            mCurrentSubVideoIndex = (mCurrentSubVideoIndex + 1) % mCurrentTrainingSegment.getNumberOfWiimoteReferenceRecords();
            setCurrentSubFrame();
        }

        public void previousSubFrame()
        {
            mCurrentSubVideoIndex = mCurrentSubVideoIndex - 1;
            if(mCurrentSubVideoIndex < 0)
                mCurrentSubVideoIndex = mCurrentTrainingSegment.getNumberOfWiimoteReferenceRecords() -1;
            setCurrentSubFrame();
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
                    startRecording(mCurrentTrainingSegment.getScoringReferenceRecord(), 0); //No need to have pre recording time here. 
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
            if (mRecordingStarted) // Need to send this command early as dispose takes some time
                mWiimotes.addInformationToRecording(END_VIDEO_CODE);

            mVideoPlayer.dispose();

            if (mRecordingStarted)
            {
                mWiimotes.stopRecording();
                mRecordingStarted = false;

                IWiimoteReferenceRecord lScoringReferenceRecord = mCurrentTrainingSegment.getScoringReferenceRecord();
                
                if (!mCurrentTrainingSegment.TrainingPlayRecord.isRecordingValid())
                {
                    MessageBox.Show("Tap Rapper : There is an issue in recording\r\nPlease check your Wiimote connection",
                    "Tap Rapper Says", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                    mOutputParent.SetScoreFeedback("Error in Wiimote Recording",0);
                    return;
                }

                mWiimotes.comparePlayToReference(mCurrentTrainingSegment.TrainingPlayRecord);

                int lNumStars = mCurrentTrainingSegment.TrainingPlayRecord.NumberOfStars;
                if (lNumStars == 1)
                    lNumStars = 2;

                StringBuilder lFeedbackMessage = new StringBuilder();

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

                mVideoPlayer.addInformation(lScoringReferenceRecord.RecordName, lNumStars);
                mOutputParent.SetScoreFeedback(lFeedbackMessage.ToString(), lNumStars);
            }

            setCurrentPlay();

        }

        public void OnWiimoteDisconnectedEvent(object sender, WiimoteUpdateEventArgs e)
        {
            if (mRecordingStarted)
            {
                stop();
            }

            MessageBox.Show("Tap Rapper : Wiimotes got Disconnected. Please connect again",
            "Tap Rapper Says", MessageBoxButtons.OK,
            MessageBoxIcon.Error);
        }

        /*
        public void OnWiimoteActionEvent(object sender, WiimoteActionEventArgs e)
        {
            WiimoteAction lAction = e.WiimoteActionObject;
            if (lAction.ActionType.CompareTo(ProjectConstants.TRAINING_NEXT_SELECTION) == 0)
                nextFrame();
            else if (lAction.ActionType.CompareTo(ProjectConstants.TRAINING_PREVIOUS_SELECTION) == 0)
                previousFrame();
        }
        */

        #endregion

        #region Cleanup
        public void Cleanup()
        {
            if (mVideoPlayer != null)
                mVideoPlayer.cleanup();
        }

        #endregion


    }

    public class TrainingSelectionEventArgs : EventArgs
    {
        public TrainingSelection TrainingSelectionValue { get; set; }

        public TrainingSelectionEventArgs(TrainingSelection trainingSelection)
        {
            TrainingSelectionValue = trainingSelection;
        }
    }

    public enum TrainingSelection
    {
        CONTINUE = 1,
        REPEAT = 2,
    }

    /*
    public class TrainingSegmentInfo
    {

        //Data after training
        public WiimotePlayRecord TrainingPlayRecord { get; set; }
        public WiimoteReferenceRecord TrainingReferenceRecord { get; set; }

        public TrainingSegmentInfo(WiimoteReferenceRecord pTrainingReferenceRecord)
        {
            TrainingReferenceRecord = pTrainingReferenceRecord;
        }
    }
     * 
        public List<string> getCurrentSegmentOptions()
        {
            List<string> lOptionsList = new List<string>();
            lOptionsList.Add("Repeat");
            if (mCurrentTrainingSegment.TrainingReferenceRecord.SlowMoOption)
                lOptionsList.Add("SlowMo");
            lOptionsList.Add("Continue");
            if (mCurrentTrainingSegment.TrainingReferenceRecord.ScoringOption)
                lOptionsList.Add("Scoring");
            return lOptionsList;

        }
     * 
        private string getCurrentPlayName()
        {
            if (mVideoInfoList.Count == 0)
                return null;
            return getFormattedPlayName(mCurrentTrainingSegment);
        }
        public bool continueTraining()
        {
            nextFrame();
            playTraining();
            return true;
        }
     * 
     * 
     * 
     * */



}
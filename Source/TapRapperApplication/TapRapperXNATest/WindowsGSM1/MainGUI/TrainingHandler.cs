using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using Utilities;
using ProjectCommon;

using System.Windows.Forms;

using VideoPanelControl;

namespace MainGUI
{
    enum TrainingStatus
    {
        TrainingNotStarted = 0,
        FollowMeStarted = 1,
        FollowMeDone = 2,
        TapWithMeDone = 3,
        TrainingDone = 4,
    }

    internal class TrainingHandler
    {
        private System.Windows.Forms.OpenFileDialog openTrainingVideoDialog;
        private System.Windows.Forms.OpenFileDialog openTrainingMusicDialog;

        private TrainingVideoPanel m_parent;
        private TrainingVideoRunner mTrainingVideoRunner;

//        private Video trainingVideo = null;

        private MP3PlayerWrapper m_MP3PLayer;

        private string m_MusicPath;

        private Thread m_TrainingThread;


        public TrainingHandler(TrainingVideoPanel videoPanelControl)
        {
            mTrainingVideoRunner = new TrainingVideoRunner(videoPanelControl);
            m_parent = videoPanelControl;
            m_parent.setTrainingVideoRunner(mTrainingVideoRunner);
        }

        public void Initialize()
        {
            // openTrainingVideoDialog
            // 
            this.openTrainingVideoDialog = new System.Windows.Forms.OpenFileDialog();
            this.openTrainingVideoDialog.DefaultExt = "avi";
            this.openTrainingVideoDialog.Filter = "movie files|*.avi||";
            this.openTrainingVideoDialog.Title = "Choose a movie to play";

            // openTrainingMusicDialog
            // 
            this.openTrainingMusicDialog = new System.Windows.Forms.OpenFileDialog();
            this.openTrainingMusicDialog.DefaultExt = "mp3";
            this.openTrainingMusicDialog.Filter = "mpw files|*.mp3||";
            this.openTrainingMusicDialog.Title = "Choose a music to play";

            selectMusicFile(Configuration.getConfiguration().TrainingMusicFile);

            ApplicationSpeech.setVolume(100);

            m_parent.TrainingSelectionEvent += new TrainingVideoPanel.OnTrainingSelectionEvent(OnTrainingSelectionEvent);
        }

        public void LoadTraining()
        {
            m_parent.initializeTraining();
            m_parent.gotoFirstSegment();
        }

        public bool isTrainingVideoRunning()
        {
            return m_parent.isTrainingVideoRunning();
        }

        public void startTraining()
        {
            mTrainingVideoRunner.playTraining();
        }

        public void repeatTraining()
        {
            mTrainingVideoRunner.playTraining();
        }

        public void slowMo()
        {
            mTrainingVideoRunner.slowMoTraining();
        }


        public void scoreTraining()
        {
            mTrainingVideoRunner.scoreTraining();
        }

        public void pause()
        {
            mTrainingVideoRunner.pause();
        }

        public void stop()
        {
            mTrainingVideoRunner.stop();
        }

        public void previousFrame()
        {
            mTrainingVideoRunner.previousFrame();
        }

        public void nextFrame()
        {
            mTrainingVideoRunner.nextFrame();
        }


        public void startTrainingThread()
        {

//            trainingVideo.Ending += new System.EventHandler(this.ClipEnding);

//            StopTraining();            
        }

        public void videoCompletedEvent(object sender,EventArgs args)
        {
            ConsoleLogger.logMessage("In videoCompletedEvent");
        }


        #region Video


        public void OnTrainingSelectionEvent(object sender, TrainingSelectionEventArgs e)
        {
            Console.Out.WriteLine("Event Received : " + e.TrainingSelectionValue);
        }

        #endregion

        #region Music

        void selectMusicFile(string musicPath)
        {
            m_MusicPath = musicPath;
        }

        void OpenTrainingMusic()
        {
            if (m_MP3PLayer == null)
            {
                m_MP3PLayer = new MP3PlayerWrapper();
            }
            m_MP3PLayer.MP3Filename = m_MusicPath;
        }

        public void OpenTrainingMusicForBrowseSelected()
        {

            openTrainingMusicDialog.InitialDirectory = Application.StartupPath;
            if (openTrainingMusicDialog.ShowDialog() == DialogResult.OK)
            {
                selectMusicFile(openTrainingMusicDialog.FileName);
                OpenTrainingMusic();
            }
        }
        #endregion


        public void mouseDoubleClickEventHandler()
        {
        }

        public void CleanupMedia()
        {
            mTrainingVideoRunner.Cleanup();
        }

        private void playTrainingSegment(string p_Message, int p_WaitTime)
        {
            System.Threading.Thread.Sleep(p_WaitTime);
        }

    }
}
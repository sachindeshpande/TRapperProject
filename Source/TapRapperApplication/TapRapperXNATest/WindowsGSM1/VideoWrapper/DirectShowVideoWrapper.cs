using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Drawing;

using DirectShowPlayer;
using DirectShowLib;
using ProjectCommon;

using System.Windows.Forms;

namespace VideoWrapper
{
    public class DirectShowVideoWrapper : IVideoPlayer
    {
        private Capture cam = null;
        private int m_Width;
        private int m_Height;
        private Graphics m_Graphics;
        private System.Drawing.Bitmap m_StepPanelBitmap;
        private Panel m_StepsOwnerPanel;
        private int m_RenderingSpeed = 10;
        private int m_RefreshRate = 10;
        private int m_UpdateCount = 0;

        private Thread mStreamCheckThread;
        private const int STREAM_CHECK_WAIT_TIME = 1000;

        private DirectShowLib.IMediaEventEx mediaEvent;

//        public delegate void OnVideoPlayCompleteEvent(object sender, EventArgs args);
//        public event OnVideoPlayCompleteEvent VideoPlayCompleteEvent;
        public event EventHandler VideoPlayCompleteEvent;
        public event EventHandler FrameMilestoneEvent;

        public DirectShowVideoWrapper(string videoPath, Panel videoOwnerPanel, Panel stepsOwnerPanel, VideoUserOptions pVideoUserOptions)
        {
            cam = new Capture(videoPath, "Test String", videoOwnerPanel, true, pVideoUserOptions);
            m_StepsOwnerPanel = stepsOwnerPanel;
            cam.FrameMilestoneEvent += new Capture.OnFrameMilestoneEvent(OnFrameMilestoneEvent);
        }


        public void load()
        {
//            Shape.initializeShapes(m_Width, m_Height);
        }

        //This method is called in a thread. Its purpose is to animate the Left/Right Shoesteps
        public void updatePanel()
        {
            while (true)
            {
                Graphics l_StepPanelGraphics;

                m_StepPanelBitmap = new Bitmap(m_Width, m_Height,
                    System.Drawing.Imaging.PixelFormat.Format24bppRgb);

                l_StepPanelGraphics = Graphics.FromImage(m_StepPanelBitmap);

                //Calls the Shape updateGraphics method to update the current position of all the shoes at this instant
                Shape.updateGraphics(l_StepPanelGraphics, m_Width, m_Height);

                Thread.Sleep(m_RenderingSpeed);
                m_Graphics.DrawImage(m_StepPanelBitmap, 0, 0, m_Width, m_Height);

                l_StepPanelGraphics.Dispose();
            }
        }

        public void play()
        {
            cam.fullScreenMode(true);
            cam.Start();
            mStreamCheckThread = new Thread(new ThreadStart(startStreamCheckingThread));
            mStreamCheckThread.Start();

//            Thread t = new Thread(new ThreadStart(updatePanel));
//            t.Start();
        }

        public void resume()
        {
            cam.fullScreenMode(true);
            cam.Resume();
        }

        public void stop()
        {
            cam.fullScreenMode(false);
            cam.Stop();
        }

        public void pause()
        {
            cam.fullScreenMode(false);
            cam.Pause();
        }

        public void dispose()
        {
            if (cam != null)
                cam.Dispose();
        }

        public void setDisplayString(string displayString)
        {
//            cam.setDisplayString(displayString);
        }


        public void setOwner(System.Windows.Forms.Panel parentPanel)
        {
        }

        public void addInformation(string pKey, int lNumStars)
        {
            cam.addInformation(pKey, lNumStars);
        }

        public System.Collections.ObjectModel.ReadOnlyCollection<string> getFrameEventList()
        {
            return cam.getFrameEventList();
        }

        public void startStreamCheckingThread()
        {
            EventCode lEventCode;
            IntPtr lPtr1, lPtr2;
            bool lCheckingThreadContinue = true;
//            EventHandler lHandler = VideoPlayCompleteEvent;

            while (lCheckingThreadContinue)
            {
                Thread.Sleep(STREAM_CHECK_WAIT_TIME);

                if (cam == null || cam.MediaEventEx == null)
                    return;

                cam.MediaEventEx.GetEvent(out lEventCode, out lPtr1, out lPtr2, 0);

                Utilities.ConsoleLogger.logMessage("In startStreamCheckingThread : lEventCode = " + lEventCode);
                if ((lEventCode == EventCode.Complete || 
                    lEventCode == EventCode.UserAbort ||
                    lEventCode == EventCode.StreamControlStopped) 
                    && VideoPlayCompleteEvent != null)
                {
                    lCheckingThreadContinue = false;
                    VideoPlayCompleteEvent(this, new EventArgs());
                }

                if (lEventCode == EventCode.FullScreenLost)
                {
                    Console.WriteLine("");
                }

            }
        }

        public void OnFrameMilestoneEvent(object sender, FrameMilestoneEventArgs args)
        {
            FrameMilestoneEvent(this, args);
        }

        public VideoState getVideoState()
        {
            if (cam == null)
                return VideoState.Stopped;
            GraphState lGraphState = cam.getGraphState();

            switch (lGraphState)
            {
                case GraphState.Running:
                    return VideoState.Running;
                case GraphState.Exiting:
                    return VideoState.Exiting;
                case GraphState.Paused:
                    return VideoState.Paused;
                case GraphState.Stopped:
                    return VideoState.Stopped;
                default:
                    return VideoState.Stopped;
            }
        }

        public void cleanup()
        {
            if (cam != null)
            cam.cleanup();
        }
    }
}

/*
        event EventHandler IVideoPlayer.VideoPlayCompleteEvent
        {
            add
            {
                    VideoPlayCompleteEvent += value;
            }
            remove
            {
                    VideoPlayCompleteEvent -= value;
            }
        }

        public void startStreamCheckingThread()
        {
            EventCode lEventCode;
            IntPtr lPtr1, lPtr2;
            EventHandler lHandler = VideoPlayCompleteEvent;

            while (true)
            {
                Thread.Sleep(STREAM_CHECK_WAIT_TIME);

                cam.MediaEventEx.GetEvent(out lEventCode, out lPtr1, out lPtr2, 0);

                if (lEventCode == EventCode.Complete)
                    lHandler(this, new EventArgs());
            }
        }
 * 
 * 
        public void OnCaptureShutdownEvent(object sender,EventArgs evt)
        {
            if(VideoPlayCompleteEvent != null)
                VideoPlayCompleteEvent(this, new EventArgs());
        }
*/
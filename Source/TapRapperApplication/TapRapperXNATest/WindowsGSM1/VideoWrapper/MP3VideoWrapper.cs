using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.DirectX.AudioVideoPlayback;

using System.Windows.Forms;
using Utilities;
using System.Threading;

namespace VideoWrapper
{
    public class MP3VideoWrapper : IVideoPlayer
    {
        private MP3PlayerWrapper m_Mp3Player = null;
        private double m_Lastmusicposition = -1;
        private int mp3PlayTime = 15;

        public event EventHandler VideoPlayCompleteEvent;
        public event EventHandler FrameMilestoneEvent;

        public MP3VideoWrapper(string videoPath, Panel ownerPanel)
        {
            m_Mp3Player = MP3PlayerWrapper.getMP3PlayerWrapper();
            m_Mp3Player.MP3Filename = videoPath;
        }


        public void load()
        {

        }

        public void play()
        {
            Thread playerCheckThread = new Thread(CheckPlayerStatus);
            playerCheckThread.Start();
            m_Mp3Player.play();
        }

        public void stop()
        {
            m_Mp3Player.stop();
        }

        public void pause()
        {

        }

        public void dispose()
        {

        }

        public void resume()
        {
        }

        public void CheckPlayerStatus()
        {
            TimeSpan startTime = new TimeSpan(DateTime.Now.Ticks);
            while (true)
            {
                TimeSpan currentTime = new TimeSpan(DateTime.Now.Ticks);
                Thread.Sleep(1000);
                if ((currentTime.TotalSeconds - startTime.TotalSeconds) > mp3PlayTime)
                {
                    VideoPlayCompleteEvent(this, new EventArgs());
                    return;
                }
            }

        }

        public void setDisplayString(string displayString)
        {
        }


        public void setOwner(System.Windows.Forms.Panel parentPanel)
        {
        }

        public System.Collections.ObjectModel.ReadOnlyCollection<string> getFrameEventList()
        {
            return null;
        }

        public void addInformation(string pKey, int lNumStars)
        {
        }

        public VideoState getVideoState()
        {
            return VideoState.Stopped;
        }

        public void cleanup()
        {
        }

    }
}


/**
        public void CheckPlayerStatus()
        {
            while (true)
            {
                TimeSpan startTime = TimeSpan(DateTime.Now);

                //if (m_Lastmusicposition == m_Mp3Player.CurrentPosition)
                //{
                //    VideoPlayCompleteEvent(this, new EventArgs());
                //    return;
                //}

                //m_Lastmusicposition = m_Mp3Player.CurrentPosition;
                //Thread.Sleep(1000);
            }
        }
**/
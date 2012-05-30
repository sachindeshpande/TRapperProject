using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.DirectX.AudioVideoPlayback;

using System.Windows.Forms;

namespace VideoWrapper
{
    public class DirectXVideoWrapper : IVideoPlayer
    {
        private Video m_Video = null;

        event EventHandler VideoPlayCompleteEvent;
        public event EventHandler FrameMilestoneEvent;

        public DirectXVideoWrapper(string videoPath, Panel ownerPanel)
        {
            m_Video = new Video(videoPath);
            m_Video.Owner = ownerPanel;
        }


        public void load()
        {
            play();
            pause();
        }

        public void play()
        {
            m_Video.Play();
        }

        public void stop()
        {
            m_Video.Stop();
        }

        public void pause()
        {
            m_Video.Pause();
        }

        public void dispose()
        {
            m_Video.Dispose();
        }

        public void resume()
        {
        }

        public void setDisplayString(string displayString)
        {
        }


        public void setOwner(System.Windows.Forms.Panel parentPanel)
        {
            // assign the win form control that will contain the video
            m_Video.Owner = parentPanel;
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

        event EventHandler IVideoPlayer.VideoPlayCompleteEvent
        {
            add
            {
                lock (VideoPlayCompleteEvent)
                {
                    VideoPlayCompleteEvent += value;
                }
            }
            remove
            {
                lock (VideoPlayCompleteEvent)
                {
                    VideoPlayCompleteEvent -= value;
                }
            }
        }

        public void cleanup()
        {
        }

    }
}

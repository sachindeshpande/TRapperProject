using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.DirectX.AudioVideoPlayback;

using System.Windows.Forms;
using System.Threading;

namespace VideoWrapper
{
    public class DummyVideoWrapper : IVideoPlayer
    {
        public event EventHandler VideoPlayCompleteEvent;
        public event EventHandler FrameMilestoneEvent;
        private bool _checkingThreadContinue;

        private Thread mStreamCheckThread;
        private const int STREAM_CHECK_WAIT_TIME = 1000;


        public DummyVideoWrapper()
        {

        }


        public void load()
        {
            play();
            pause();
        }

        public void play()
        {
            mStreamCheckThread = new Thread(new ThreadStart(startStreamCheckingThread));
            mStreamCheckThread.Start();            
        }

        public void stop()
        {
            _checkingThreadContinue = false;
        }

        public void pause()
        {

        }

        public void dispose()
        {
            _checkingThreadContinue = false;        
        }

        public void resume()
        {
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

        public void startStreamCheckingThread()
        {
            _checkingThreadContinue = true;

            for (int index = 0; index < 10; index++)
            {
                Thread.Sleep(STREAM_CHECK_WAIT_TIME);
                if (!_checkingThreadContinue)
                    break;
            }

            _checkingThreadContinue = false;
            VideoPlayCompleteEvent(this, new EventArgs());
        }

    }
}

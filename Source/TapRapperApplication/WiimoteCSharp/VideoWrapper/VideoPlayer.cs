using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ProjectCommon;

using System.Windows.Forms;

namespace VideoWrapper
{

    public enum VideoState
    {
        Stopped,
        Paused,
        Running,
        Exiting
    }

    public interface IVideoPlayer
    {
        event EventHandler VideoPlayCompleteEvent;
        event EventHandler FrameMilestoneEvent;

        void load();

        void play();

        void stop();

        void pause();

        void resume();

        void dispose();

        void setDisplayString(string displayString);

        void setOwner(System.Windows.Forms.Panel parentPanel);

        void addInformation(string pKey, int lNumStars);

        VideoState getVideoState();

        System.Collections.ObjectModel.ReadOnlyCollection<string> getFrameEventList();

        void cleanup();
    }

    public class VideoPlayerFactory
    {
        public static IVideoPlayer m_DirectXPlayer;
//        public static IVideoPlayer m_DirectShowPlayer;
        public static IVideoPlayer m_Mp3Player;

        public static IVideoPlayer getVideoPlayer(string videoPlayerName, string videoFile, Panel videoOwner, Panel stepsOwner, VideoUserOptions pVideoUserOptions)
        {
            if (videoFile.Contains(".mp3"))
            {
                if (m_Mp3Player == null)
                    m_Mp3Player = new MP3VideoWrapper(videoFile, videoOwner);
                return m_Mp3Player;
            }

            if (videoPlayerName.CompareTo(ProjectConstants.DIRECTX_VIDEO_PLAYER_NAME) == 0)
            {
                if (m_DirectXPlayer == null)
                    m_DirectXPlayer = new DirectXVideoWrapper(videoFile, videoOwner);
                return m_DirectXPlayer;
            }
            else if (videoPlayerName.CompareTo(ProjectConstants.DIRECT_SHOW_VIDEO_PLAYER_NAME) == 0)
            {
                return new DirectShowVideoWrapper(videoFile, videoOwner, stepsOwner, pVideoUserOptions);
            }
            else if (videoPlayerName.CompareTo(ProjectConstants.DUMMY_VIDEO_PLAYER_NAME) == 0)
            {
                return new DummyVideoWrapper();
            }

            throw new VideoPlayerNotFoundException();
        }
    }
}

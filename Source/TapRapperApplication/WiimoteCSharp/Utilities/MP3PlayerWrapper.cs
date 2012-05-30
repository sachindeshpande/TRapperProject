using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Media;

using Microsoft.DirectX.AudioVideoPlayback;
using ProjectCommon;

//using System.Runtime.InteropServices;

namespace Utilities
{
    public enum MP3PlayerVolumeLevels
    {
        LOUD = 0,
        MEDIUM = -500,
        LOW = -1000,
        SILENT = -10000,
    }

    public class MP3PlayerWrapper
    {

        public int SND_SYNC = 0x0000;
        public int SND_ASYNC = 0x0001;
        public int SND_NODEFAULT = 0x0002;
        public int SND_MEMORY = 0x0004;
        public int SND_LOOP = 0x0008;
        public int SND_NOSTOP = 0x0010;
        public int SND_NOWAIT = 0x00002000;
        public int SND_ALIAS = 0x00010000;
        public int SND_ALIAS_ID = 0x00110000;
        public int SND_FILENAME = 0x00020000;
        public int SND_RESOURCE = 0x00040004;
        public int SND_PURGE = 0x0040;
        public int SND_APPLICATION = 0x0080;


        public MP3PlayerWrapper()
        {

        }

        private static MP3PlayerWrapper m_MP3PlayerWrapperInstance;

        private SoundPlayer m_SimpleSound;

        //[DllImport("winmm.dll")]
        //private static extern bool PlaySound(string filename, int module, int flags);

        private string m_MP3Filename;
        public string MP3Filename
        {
            get
            {
                return m_MP3Filename;
            }
            set
            {
                m_MP3Filename = value;
                m_Backmusic = new WMPLib.WindowsMediaPlayer();
                m_Backmusic.URL = m_MP3Filename;
                //m_Backmusic = new Microsoft.DirectX.AudioVideoPlayback.Audio(m_MP3Filename);
                //m_Backmusic.Play();
                //m_Backmusic.Pause();

            }
        }

        private WMPLib.WindowsMediaPlayer m_Backmusic;
//        private Audio m_Backmusic;
        private double m_Lastmusicposition;

        public double CurrentPosition
        {
            get
            {
                if (m_Backmusic != null)
                    return m_Backmusic.controls.currentPosition;
                else
                    return -1;
            }

        }

        public static MP3PlayerWrapper getMP3PlayerWrapper()
        {
            if(m_MP3PlayerWrapperInstance == null)
                m_MP3PlayerWrapperInstance = new MP3PlayerWrapper();
            return m_MP3PlayerWrapperInstance;
        }

        public void load()
        {

        }

        public void play()
        {
            if (Configuration.getConfiguration().SoundOn)
            {
                m_Backmusic.controls.play();
            }
        }

        public void stop()
        {
            if (Configuration.getConfiguration().SoundOn)
                m_Backmusic.controls.stop();
        }


    }
}


/**
 * 
         public void setVolume(MP3PlayerVolumeLevels volumeLevel)
        {
            m_Backmusic.Volume = (int)volumeLevel;
        }
 
        public void restartMP3IfEnded()
        {
            
            if (Configuration.getConfiguration().SoundOn)
            {
                if (m_Backmusic.CurrentPosition == m_Lastmusicposition)
                {
                    m_Backmusic.SeekCurrentPosition(0, SeekPositionFlags.AbsolutePositioning);
                }
                m_Lastmusicposition = m_Backmusic.CurrentPosition;
            }
        }


**/
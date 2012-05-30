using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

using ProjectCommon;   


namespace Utilities
{
    public class TimerEventArgs : EventArgs
    {
        public TimerEventArgs(int numBeats, int numBars, int numBeatInBar)
        {
            m_NumBeats = numBeats;
            m_NumBars = numBars;
            m_NumBeatInBar = numBeatInBar;
        }

        public int m_NumBeats;
        public int m_NumBars;
        public int m_NumBeatInBar;
    }


    public class TimerThread
    {
        private System.Timers.Timer aTimer;
        private System.Timers.Timer aLeadInTimer;
        private int m_beatsPerMinute;
        private int m_beatsPerBar;
        private int m_numBars;
        private int m_LeadIn;
        private int leadinBeatCounter;
        private int recordingBeatCounter;

        private bool m_LeadInCompleted;
        private bool m_UseMP3Music;
        private bool m_SoundOption;

        private string m_IntroductoryMessage;

        private MP3PlayerWrapper m_MP3PLayer;


        private static TimerThread m_TimerThreadInstance;

        public delegate void OnStartingRecordingTimerEvent(object sender, TimerEventArgs args);
        public delegate void OnBeatTimerEvent(object sender, TimerEventArgs args);
        public delegate void OnTimerCompletedEvent(object sender, TimerEventArgs args);
        public delegate void OnTimerInterruptedEvent(object sender, TimerEventArgs args);        

        public event OnStartingRecordingTimerEvent StartingRecordingTimerEvent;
        public event OnBeatTimerEvent BeatTimerEvent;
        public event OnTimerCompletedEvent TimerCompletedEvent;
        public event OnTimerInterruptedEvent TimerInterruptedEvent;

        // Define the duration of a note in units of milliseconds.
        protected enum Duration
        {
            WHOLE = 1600,
            HALF = WHOLE / 2,
            QUARTER = HALF / 2,
            EIGHTH = QUARTER / 2,
            SIXTEENTH = EIGHTH / 2,
        }

        // Define the frequencies of notes in an octave, as well as 
        // silence (rest).
        protected enum Tone
        {
            REST = 0,
            GbelowC = 196,
            A = 220,
            Asharp = 233,
            B = 247,
            C = 262,
            Csharp = 277,
            D = 294,
            Dsharp = 311,
            E = 330,
            F = 349,
            Fsharp = 370,
            G = 392,
            Gsharp = 415,
        }

        public TimerThread()
        {
            //m_MP3PLayer = new MP3PlayerWrapper();
            //m_MP3PLayer.MP3Filename = ProjectConstants.DEFAULT_BEATMP3_FILEPATH;
        }

        public static TimerThread getTimerThread()
        {
            if (m_TimerThreadInstance == null)
                m_TimerThreadInstance = new TimerThread();
            return m_TimerThreadInstance;
        }

        public void setTimerBeatMP3(string mp3Filename)
        {
            m_MP3PLayer.MP3Filename = mp3Filename;
        }

        public void startTimer(int p_beatsPerMinute,int p_beatsPerBar,int p_numBars,int p_LeadIn,
            bool p_UseMP3Music,string introductoryMessage)
        {
            m_beatsPerMinute = p_beatsPerMinute;
            m_beatsPerBar = p_beatsPerBar;
            m_numBars = p_numBars;
            m_LeadIn = p_LeadIn;
            m_UseMP3Music = p_UseMP3Music;
            leadinBeatCounter = 0;
            recordingBeatCounter = 0;
            m_IntroductoryMessage = introductoryMessage;

            if (m_beatsPerMinute == 0) m_beatsPerMinute = 100;

            if (m_beatsPerBar == 0) m_beatsPerBar = 4;

            if (m_numBars == 0) m_numBars = 10;

            Console.Out.WriteLine("beatsPerMinute = " + m_beatsPerMinute + " : beatsPerBar = " + m_beatsPerBar + " : numBars = " + m_numBars);

            m_LeadInCompleted = false;

            if (m_LeadIn == 0)
                startRecording();
            else
                startLeadinTimer();

        }

        private void startTimer(Timer p_Timer)
        {
            // Set the Interval to 2 seconds (2000 milliseconds).
            p_Timer.Interval = (60 * 1000) / m_beatsPerMinute;
            p_Timer.Enabled = true;
        }

        public void stopRecording()
        {

            lock (this)
            {
                if (!m_LeadInCompleted)
                    closeLeadInTimer();
                closeRecordingTimer();
                TimerInterruptedEvent(this, new TimerEventArgs(0, 0, 0));
            }
        }


        #region LeadIn
        private void startLeadinTimer()
        {
            aLeadInTimer = new System.Timers.Timer(m_beatsPerBar * m_LeadIn);
            // Hook up the Elapsed event for the timer.
            aLeadInTimer.Elapsed += new ElapsedEventHandler(OnLeadinTimedEvent);
            startTimer(aLeadInTimer);
        }

        private void OnLeadinTimedEvent(object source, ElapsedEventArgs e)
        {
            if (((leadinBeatCounter % m_beatsPerBar) == 0 || leadinBeatCounter == 0) && !m_LeadInCompleted)
            {
                ApplicationSpeech.systemBeep((int)Tone.A, (int)Duration.SIXTEENTH);
            }
            else if (!m_LeadInCompleted)
            {
                ApplicationSpeech.systemBeep((int)Tone.B, (int)Duration.SIXTEENTH);
            }


            if (leadinBeatCounter <= m_LeadIn * m_beatsPerBar - 1)
            {
                BeatTimerEvent(this, new TimerEventArgs(leadinBeatCounter, (leadinBeatCounter / m_beatsPerBar), (leadinBeatCounter % m_beatsPerBar) + 1));
            }

            if ((leadinBeatCounter == m_LeadIn * m_beatsPerBar - 1) && !m_LeadInCompleted)
            {
                closeLeadInTimer();
                startRecording();
                return;
            }

            leadinBeatCounter++;
        }


        private void closeLeadInTimer()
        {
            m_LeadInCompleted = true;
            aLeadInTimer.Enabled = false;
            aLeadInTimer.Close();
        }

        #endregion

        #region Recording Timer

        private void startRecording()
        {

            StartingRecordingTimerEvent(this, new TimerEventArgs(recordingBeatCounter, (recordingBeatCounter / m_beatsPerBar), (recordingBeatCounter % m_beatsPerBar) + 1));

            string[] rows = m_IntroductoryMessage.Split(',');

            if (rows.Length > 1)
            {
                int length = rows.Length;

                for (int i = 0; i < length; i = i + 2)
                {
                    ApplicationSpeech.speakText(rows[i]);
                    System.Threading.Thread.Sleep(Convert.ToInt32(rows[i + 1]));
                }
            }
            /*
            ApplicationSpeech.speakText("Start Tapping");
            System.Threading.Thread.Sleep(100);
            ApplicationSpeech.speakText("1");
            System.Threading.Thread.Sleep(200);
            ApplicationSpeech.speakText("2");
            System.Threading.Thread.Sleep(200);
            ApplicationSpeech.speakText("3");
            System.Threading.Thread.Sleep(200);
            ApplicationSpeech.speakText("4");
            System.Threading.Thread.Sleep(200);
            */
            if (m_UseMP3Music)
            {
                m_MP3PLayer.play();
            }

            //                ApplicationSpeech.speakText("Start Tapping");

            startRecordingTimer();
        }


        private void startRecordingTimer()
        {

            aTimer = new System.Timers.Timer(m_beatsPerBar * m_numBars);
            // Hook up the Elapsed event for the timer.
            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            startTimer(aTimer);
        }

        private void closeRecordingTimer()
        {
            aTimer.Enabled = false;
            aTimer.Close();

            if (m_UseMP3Music)
                m_MP3PLayer.stop();
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            if (!m_UseMP3Music)
            {
                if ((recordingBeatCounter % m_beatsPerBar) == 0 || recordingBeatCounter == 0)
                {
                    ApplicationSpeech.systemBeep((int)Tone.A, (int)Duration.SIXTEENTH);
                }
                else
                {
                    ApplicationSpeech.systemBeep((int)Tone.B, (int)Duration.SIXTEENTH);
                }
            }

            if (recordingBeatCounter == m_numBars * m_beatsPerBar - 1)
            {
                closeRecordingTimer();
                TimerCompletedEvent(this, new TimerEventArgs(recordingBeatCounter, (recordingBeatCounter / m_beatsPerBar), (recordingBeatCounter % m_beatsPerBar) + 1));
            }
            else if(recordingBeatCounter < m_numBars * m_beatsPerBar - 1)
            {
                BeatTimerEvent(this, new TimerEventArgs(recordingBeatCounter, (recordingBeatCounter / m_beatsPerBar), (recordingBeatCounter % m_beatsPerBar) + 1));
            }


            //if (m_UseMP3Music)
            //    m_MP3PLayer.restartMP3IfEnded();

            recordingBeatCounter++;
        }

        #endregion

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SpeechLib;

namespace ProjectCommon
{
    public class ApplicationSpeech
    {
        private static SpVoice m_voice = new SpVoice();

        public static void setVolume(int volumeLevel)
        {
            m_voice.Volume = volumeLevel;
        }

        public static void speakText(string speechText)
        {
            if (Configuration.getConfiguration().SoundOn)
            {
                m_voice.Speak(speechText, SpeechVoiceSpeakFlags.SVSFDefault);
            }
        }

        public static void speakTextThreaded(string speechText)
        {
        }

        public static void systemBeep(int p_Tone,int p_Duration)
        {
            if (Configuration.getConfiguration().SoundOn)
            {
                Console.Beep(p_Tone, p_Duration);
            }
        }
    }
}

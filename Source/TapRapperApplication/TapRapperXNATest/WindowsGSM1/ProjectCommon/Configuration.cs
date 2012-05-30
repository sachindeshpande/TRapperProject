using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectCommon
{

    public class Configuration
    {
        private static Configuration m_Configuration;
        public int MaxWiimoteConnectionTries { get; set; }
        public int WaitTimeCheckConnection { get; set; }
        public int WiimotesChartRowSkipStep { get; set; }
        public bool WiimotesChartOneAxisOnly { get; set; }
        public bool SoundOn { get; set; }
        public bool FeedbackOn { get; set; }
        public bool MP3Option { get; set; }
        public string TrainingVideoFile { get; set; }
        public string TrainingMusicFile { get; set; }
        public int TrainingPreRecordingTime { get; set; }
        public int WiimoteDataSendInterval { get; set; }

        public bool WiimoteSimulationMode { get; set; }
        public string WiimoteSimulationFile { get; set; }
        public int WiimoteDataMode { get; set; }

        public bool TrainingMode { get; set; }

        public int NumBeatsPerMinute { get; set; }
        public int NumBeatsPerBar { get; set; }
        public int NumBars { get; set; }
        public int LeadIn { get; set; }
        public int RecordingInterval { get; set; }
        
        public void initialize()
        {
            MaxWiimoteConnectionTries = ProjectConstants.DEFAULT_MAX_WIIMOTE_CONNECTION_TRIES;
            WaitTimeCheckConnection = ProjectConstants.DEFAULT_WAIT_TIME_CHECK_WIIMOTE_CONNECTION;
            WiimotesChartRowSkipStep = ProjectConstants.DEFAULT_WIIMOTES_CHART_SKIP_STEP;
            WiimotesChartOneAxisOnly = ProjectConstants.DEFAULT_ONE_AXIS_ONLY_OPTION;
            SoundOn = ProjectConstants.DEFAULT_SOUND_ON_OPTION;
            FeedbackOn = ProjectConstants.DEFAULT_FEEDBACK_ON_OPTION;
            MP3Option = ProjectConstants.DEFAULT_MP3_ON_OPTION;
            TrainingVideoFile = ProjectConstants.DEFAULT_TRAINING_VIDEO_FILEPATH;
            TrainingMusicFile = ProjectConstants.DEFAULT_BEATMP3_FILEPATH;
            TrainingPreRecordingTime = ProjectConstants.DEFAULT_TRAINING_PRE_RECORDING_TIME;

            WiimoteSimulationMode = ProjectConstants.DEFAULT_WIIMOTE_SIMULATION_MODE;
            WiimoteSimulationFile = ProjectConstants.DEFAULT_WIIMOTE_SIMULATION_FILE;
            WiimoteDataMode = ProjectConstants.DEFAULT_WIIMOTE_DATA_MODE;

            TrainingMode = ProjectConstants.TRAINING_MODE;

            WiimoteDataSendInterval = ProjectConstants.DEFAULT_DATA_SEND_INTERVAL;

            NumBeatsPerMinute = ProjectConstants.DEFAULT_NUM_BEATS_PER_MINUTE;
            NumBeatsPerBar = ProjectConstants.DEFAULT_NUM_BEATS_PER_BAR;
            NumBars = ProjectConstants.DEFAULT_NUM_BARS;
            LeadIn = ProjectConstants.DEFAULT_LEAD_IN;
            RecordingInterval = ProjectConstants.DEFAULT_RECORDING_INTERVAL;
        }

        public static Configuration getConfiguration()
        {
            if (m_Configuration == null)
                m_Configuration = new Configuration();
            return m_Configuration;
        }

    }

}


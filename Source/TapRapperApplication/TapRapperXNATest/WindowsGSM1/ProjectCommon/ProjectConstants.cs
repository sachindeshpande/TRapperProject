using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace ProjectCommon
{
    public class ProjectConstants
    {


        public const int WIIMOTE_MOTIONPLUS_ACC_DATA_MODE = 0;
        public const int WIIMOTE_MOTIONPLUS_ACC_IR_DATA_MODE = 1;

        public const bool TRAINING_MODE = false;
        public const bool DEFAULT_WIIMOTE_SIMULATION_MODE = true;
        public const int DEFAULT_WIIMOTE_DATA_MODE = WIIMOTE_MOTIONPLUS_ACC_DATA_MODE;

        public const string FILE_NAME_PROPERTY_NAME = "FileName";
        public const int NUMBER_OPEN_FILE_ATTEMPTS = 10;
        public static double SMALL_DOUBLE_VALUE = Math.Pow(10, -3);
        public static double REAL_SMALL_DOUBLE_VALUE = Math.Pow(10, -12);

        public const string TRAINING_VIDEO_PANEL_NAME = "trainingPage";
        public const string SETUP_PANEL_NAME = "setupTab";
        public const string CALIBRATION_PANEL_NAME = "CalibrationTab";

        public const int TRAINING_VIDEO_PANEL_INDEX = 0;
        public const int SETUP_PANEL_INDEX = 1;
        public const int CALIBRATION_INDEX = 2;

        public const string UNTITLED_REFERENCE_NAME ="Reference";
        public const string UNTITLED_REFERENCE_RECORDING_PREFIX = "Try";
        public const string RECORDING_FILE_NAME_EXT = ".csv";
        public const string UNTITLED_PLAY_PREFIX = "Play";
        public const string WIIMOTE_DATA_STORE = "WiimoteDataStore.xml";

        public const int MOVE_ROW_UP = 1;
        public const int MOVE_ROW_DOWN = 2;

        public const string MATLAB_SCORE_FILE_NAME = "result.txt";
        public const int MATLAB_SCORE_FILE_NUMBER_OF_HEADERS = 2;

        public const string RECORDING_STARTED_TEXT = "Now Starting Recording";
        public const string RECORDING_IN_PROGRESS_TEXT = "Now Recording";
        public const string RECORDING_COMPLETED_TEXT = "Recording Completed. Thank you";

        public const string SUPER_SCORE_TEXT = "Super... You are ready to start your own show !!!";
        public const string EXCELLENT_SCORE_TEXT = "Excellent.. You are now ready to learn from Ann Marie!!";
        public const string GOOD_SCORE_TEXT = "Good Job.. You are almost there !!";
        public const string FAIR_SCORE_TEXT = "Nice Try .. Try a little more harder !!";
        public const string POOR_SCORE_TEXT = "YOU SUCK !!";

        public const string WIIMOTE_DISCONNECTED_MESSAGE = "Wiimotes got disconnected. Please try connecting again";
        public const string WIIMOTE_INVALID_DATA_MESSAGE = "Wiimote data is invalid. Please try connecting again";
        public const string GENERAL_WIIMOTE_CONNECTION_ISSUE_MESSAGE = "There was an issue with the Wiimote Connection. Please try again";
        public const string WIIMOTE_NOT_CONNECTED_MESSAGE = "Wiimotes are not connected. Please connect wiimotes before recording reference";

        public const double DEFAULT_VALID_RECORDING_STDDEV = 0.1;

        public const string APPLICATION_START_MESSAGE = "Welcome to Tap Rapper!";

        public const string NO_CALIBRATION_OPTION_TEXT = "None";
        public const string SYSTEM_CALIBRATION_OPTION_TEXT = "System";
        public const string DYNAMIC_CALIBRATION_OPTION_TEXT = "Dynamic";

        public const string MATLAB_APP_NAME = "playvsref";

        //Bluetooth Setup
//        public const string BLUETOOTH_SETUP_APP = @"D:\Program Files\IVT Corporation\BlueSoleil\BlueSoleil.exe";
        public const string BLUETOOTH_SETUP_APP = @"notepad.exe";
        public const string BLUETOOTH_PROCESS_NAME = @"BlueSoleil";

        //Calibration
        public const int DEFAULT_MAX_CALIBRATION_RECORDS = 1000;
        //TODO : Change default to smaller #
        public const int DEFAULT_NUMBER_CALIBRATION_PARAM = 100;

        public const int WIMMOTE_NUMBER_OF_COLUMNS = 24;

        public const int WIMMOTE1_DATA_ACCX_COLUMN_INDEX = 4;
        public const int WIMMOTE1_DATA_ACCY_COLUMN_INDEX = 5;
        public const int WIMMOTE1_DATA_ACCZ_COLUMN_INDEX = 6;

        public const int WIMMOTE1_DATA_RAW_YAW_COLUMN_INDEX = 7;
        public const int WIMMOTE1_DATA_RAW_PITCH_COLUMN_INDEX = 8;
        public const int WIMMOTE1_DATA_RAW_ROLL_COLUMN_INDEX = 9;

        public const int WIMMOTE1_DATA_YAW_COLUMN_INDEX = 10;
        public const int WIMMOTE1_DATA_PITCH_COLUMN_INDEX = 11;
        public const int WIMMOTE1_DATA_ROLL_COLUMN_INDEX = 12;

        public const int WIMMOTE2_DATA_ACCX_COLUMN_INDEX = 15;
        public const int WIMMOTE2_DATA_ACCY_COLUMN_INDEX = 16;
        public const int WIMMOTE2_DATA_ACCZ_COLUMN_INDEX = 17;

        public const int WIMMOTE2_DATA_RAW_YAW_COLUMN_INDEX = 18;
        public const int WIMMOTE2_DATA_RAW_PITCH_COLUMN_INDEX = 19;
        public const int WIMMOTE2_DATA_RAW_ROLL_COLUMN_INDEX = 20;

        public const int WIMMOTE2_DATA_YAW_COLUMN_INDEX = 21;
        public const int WIMMOTE2_DATA_PITCH_COLUMN_INDEX = 22;
        public const int WIMMOTE2_DATA_ROLL_COLUMN_INDEX = 23;

        public const int CALIBRATION_MESSAGE_LOOK_TIME = 5000;
        public const string CALIBRATION_NOT_STARTED_MESSAGE = "Please attach the Wiimotes to your feet\r\nThen click Start to start the Calibration Process.";
        public const string CALIBRATION_STARTED_MESSAGE = "OK Lets Start. Now Stand Still for a few seconds";
        public const string STAND_STILL_IN_PROGRESS_MESSAGE = "Continue Standing Still";
        public const string STAND_STILL_CALIBRATION_DONE_MESSAGE = "Okay. Now Start Tapping your LEFT foot";
        public const string LEFT_CALIBRATION_IN_PROGRESS_MESSAGE = "Keep Tapping your LEFT Foot";
        public const string LEFT_CALIBRATION_DONE_MESSAGE = "Good !! Now Start Tapping your RIGHT foot";
        public const string RIGHT_CALIBRATION_IN_PROGRESS_MESSAGE = "Keep Tapping your RIGHT Foot";
        public const string CALIBRATION_DONE_MESSAGE = "Hooray !! Calibration is completed. Thanks !!";

        #region Training General

        public const string TRAINING_IMAGE_CAPTION_STRING = "Image";
        public const string TRAINING_TEXT_CAPTION_STRING = "Text";
        public const string TRAINING_OBJECT_CAPTION_STRING = "Object";
        public const string TRAINING_EVENT_STRING = "Event";
        public const string TRAINING_PAUSE_CAPTION_STRING = "Pause";
        public const string TRAINING_START_RECORDING_EVENT_STRING = "StartRecording";

        #endregion

        #region Training Control

        public const string TRAINING_AUTOMATIC_NEXT_VIDEO_PLAY = "Automatic";
        public const string TRAINING_USER_SELECTION_NEXT_VIDEO_PLAY = "UserSelection";
        public const string TRAINING_LAST_VIDEO_PLAY = "Last";

        public const string TRAINING_NEXT_SELECTION = "SelectNext";
        public const string TRAINING_PREVIOUS_SELECTION = "SelectPrevious";
        public const string TRAINING_PLAY_SELECTION = "PlaySelection";
        public const string TRAINING_STOP_PLAY = "StopPlay";
        public const string TRAINING_NO_ACTION = "NoAction";        

        #endregion

        #region Training Feedback
        public const int DEFAULT_TRAINING_BAR_TIME = 8000;
        public const int DEFAULT_TRAINING_PRE_RECORDING_TIME = 2000;
        public static string GREAT_JOB_MESSAGE = "Great Job !!";
        public static string GOOD_JOB_MESSAGE = "Good Job !!";
        public static string NICE_WORK_MESSAGE = "Nice Work !!!";

        public static string TRAINING_STARTING_MESSAGE;
        public static string TRAINING_FOLLOW_ME_MESSAGE;
        public static string TRAINING_TAP_WITH_ME_MESSAGE;
        public static string TRAINING_TAP_WITH_ME_AGAIN_MESSAGE;
        public static string TRAINING_DONE_MESSAGE;


        public const int TRAINING_IMAGE_CAPTION_OPTION = 1;
        public const int TRAINING_TEXT_CAPTION_OPTION = 2;
        public const int TRAINING_PAUSE_CAPTION_OPTION = 2;

        public const string TRAINING_CURRENT_PLAY_HEADING = "Current Training Segment";
        public const string TRAINING_STARTING_MESSAGE_CURRENT_PLAY_LABEL = "Press Continue to Start";
        public const int TRAINING_THRESHOLD_SCORE = 100;

        public const string TRAINING_FEEDBACK_FOR_REPEAT = "Oops !! Please try again";
        public const string TRAINING_FEEDBACK_FOR_CONTINUE = "Great job ! You got it !! ";

        public const string TRAINING_FEEDBACK_FOR_ONE_STAR = "Oops ";
        public const string TRAINING_FEEDBACK_FOR_TWO_STARS = "We've got work to do ! ";
        public const string TRAINING_FEEDBACK_FOR_THREE_STARS = "Pretty Good, Rookie ! ";
        public const string TRAINING_FEEDBACK_FOR_FOUR_STARS = "That's the stuff ! ";
        public const string TRAINING_FEEDBACK_FOR_FIVE_STARS = "Congratulations !! ";

        public const string TRAINING_MATLAB_OPTION_PARTS_OF_THE_FEET = "FEET";
        public const string TRAINING_MATLAB_OPTION_TOE_STANDS = "TOESTAND";
        public const string TRAINING_MATLAB_OPTION_POP_OVERS = "POPOVERS";

        public const string TRAINING_RATING_IMAGE_FILENAME = "SmileyStar.jpg";
        public const string TRAINING_BLANK_IMAGE_FILENAME = "Blank.png";
        public const string TRAINING_BLANK_SCORING_IMAGE_FILENAME = "BlankScoring.png";


        #endregion


        public static string DEFAULT_WIIMOTE_SIMULATION_FILE;

        public static int DEFAULT_DATA_SEND_INTERVAL = 0;

        public const string DIRECTX_VIDEO_PLAYER_NAME = "DirectXVideoPlayer";
        public const string DIRECT_SHOW_VIDEO_PLAYER_NAME = "DirectShowVideoPlayer";
        public const string DUMMY_VIDEO_PLAYER_NAME = "DummyVideoPlayer";

        public const string TAP_SHOE_IMAGE_FILE_NAME = "TapShoe.JPG";
        public static string TAP_SHOE_IMAGE_PATH;
        public const string REPEAT_OPTION_IMAGE_FILE_NAME = "Repeat.png";
        public static string REPEAT_OPTION_IMAGE_FILE_PATH;
        public const string FORWARD_OPTION_IMAGE_FILE_NAME = "Forward.png";
        public static string FORWARD_OPTION_IMAGE_FILE_PATH;
        public const string SLOWMO_OPTION_IMAGE_FILE_NAME = "SlowMotion.png";
        public static string SLOWMO_OPTION_IMAGE_FILE_PATH;

        public const int DEFAULT_MAX_WIIMOTE_CONNECTION_TRIES = 40;
        public const int DEFAULT_WAIT_TIME_CHECK_WIIMOTE_CONNECTION = 1000;
        public const int DEFAULT_WIIMOTES_CHART_SKIP_STEP = 0;
        public const bool DEFAULT_ONE_AXIS_ONLY_OPTION = false;

        public const int DEFAULT_NUM_BEATS_PER_MINUTE = 240;
        public const int DEFAULT_NUM_BEATS_PER_BAR = 4;
        public const int DEFAULT_NUM_BARS = 4;
        public const int DEFAULT_LEAD_IN = 0;
        public const int DEFAULT_RECORDING_INTERVAL = 0;

        public const bool DEFAULT_SOUND_ON_OPTION = false;
        public const bool DEFAULT_MP3_ON_OPTION = true;
        public const bool DEFAULT_FEEDBACK_ON_OPTION = false;

        public const int WIIMOTE_PIPE_TIMEOUT = 60000;
        public const int WAIT_TIME_CPP_APP = 5000;
        public const int WAIT_TIME_MATLAB_APP = 30000;

        public const string MATLAB_SCORE_FILE_SCORE_HEADER_NAME = "score";
        public const string MATLAB_SCORE_FILE_AVEDELAY_HEADER_NAME = "avedelay";

        public static string PROJECT_PATH;
        public static string WIIMOTE_DATA_PATH;
        public static string WIIMOTE_CPP_APP;
        public static string PROJECT_MEDIA_PATH;
        public static string STEP_ICONS_MEDIA_PATH;
        public static string PROJECT_TEMP_PATH;
        public static string SPACE_SENSOR_SIMULATED_DATA_PATH;

        public static string WIIMOTE_REFERENCE_DATA_PATH;
        public static string WIIMOTE_PLAY_DATA_PATH;

        public static string MATLAB_DIRECTORY;

        public static string DEFAULT_BEATMP3_FILEPATH;
        public static string DEFAULT_TRAINING_VIDEO_FILEPATH;
        public static string STOMP_IMAGE_PATH;
        public static string TRAINING_VIDEO_INFO_PATH;


        //Training Shapes
        public static int SHAPE_TYPE_INDEX = 0;
        public static int SHAPE_FOOT_INDEX = 1;
        public static int SHAPE_STEP_NAME_INDEX = 2;
        public static int SHAPE_TIME_INDEX = 3;

        public static string SHAPE_LEFT_FOOT = "Left";
        public static string SHAPE_RIGHT_FOOT = "Right";

        public const string SHAPE_LIST_FILE_NAME = "ShapeList.csv";
        public static string SHAPE_LIST_PATH;

        public static float TOTAL_FRAME_TIME = 3.0F;

        public const string VOICE_COMMAND_ON_TEXT = "Voice Commands ON";
        public const string VOICE_COMMAND_OFF_TEXT = "Voice Commands OFF";
        
        //End Training Shape

        public static void initialize(string workingDirectory, string wiimoteApplicationPath, string matlabApplicationDirectory, string mediaDirectory)
        {
            //PROJECT_PATH = ConfigurationSettings.AppSettings[WORKING_PATH_KEY].ToString();            
            PROJECT_PATH = workingDirectory;
            WIIMOTE_DATA_PATH = PROJECT_PATH + @"\Data";
            WIIMOTE_CPP_APP = wiimoteApplicationPath;
            PROJECT_MEDIA_PATH = mediaDirectory;
            STEP_ICONS_MEDIA_PATH = PROJECT_MEDIA_PATH  + @"\uiStepIcons";
            PROJECT_TEMP_PATH = PROJECT_PATH + @"\temp\CSharp";

            WIIMOTE_REFERENCE_DATA_PATH = WIIMOTE_DATA_PATH + @"\WiimoteReferenceData";

            WIIMOTE_PLAY_DATA_PATH = WIIMOTE_DATA_PATH + @"\WiimotePlayData";

            DEFAULT_WIIMOTE_SIMULATION_FILE =  WIIMOTE_REFERENCE_DATA_PATH + @"\WiimoteSimulation.csv";

            MATLAB_DIRECTORY = PROJECT_PATH + matlabApplicationDirectory;

            SPACE_SENSOR_SIMULATED_DATA_PATH = PROJECT_PATH + @"\Data\WiimoteReferenceData\SensorSimulation.csv"; 

            DEFAULT_BEATMP3_FILEPATH = PROJECT_MEDIA_PATH + @"\Kalimba.wav";
//            DEFAULT_TRAINING_VIDEO_FILEPATH = PROJECT_MEDIA_PATH + @"\TapDance_4Times.avi";
            DEFAULT_TRAINING_VIDEO_FILEPATH = PROJECT_MEDIA_PATH + @"\IntroAndGettingToKnowYourFeet.csv";

            STOMP_IMAGE_PATH = PROJECT_MEDIA_PATH + @"\Stomp.jpg";
            TRAINING_VIDEO_INFO_PATH = PROJECT_MEDIA_PATH + @"\TrainingVideoInfoList.csv";
            
            TAP_SHOE_IMAGE_PATH = WIIMOTE_DATA_PATH + @"\" + TAP_SHOE_IMAGE_FILE_NAME;

            REPEAT_OPTION_IMAGE_FILE_PATH = STEP_ICONS_MEDIA_PATH + @"\" + REPEAT_OPTION_IMAGE_FILE_NAME;
            FORWARD_OPTION_IMAGE_FILE_PATH = STEP_ICONS_MEDIA_PATH + @"\" + FORWARD_OPTION_IMAGE_FILE_NAME;
            SLOWMO_OPTION_IMAGE_FILE_PATH = STEP_ICONS_MEDIA_PATH + @"\" + SLOWMO_OPTION_IMAGE_FILE_NAME;

            SHAPE_LIST_PATH = WIIMOTE_DATA_PATH + @"\" + SHAPE_LIST_FILE_NAME;

            TRAINING_STARTING_MESSAGE = ProjectCommon.Properties.Settings.Default.TrainingStartingMessage.Replace("nextLine","\r\n");
            TRAINING_FOLLOW_ME_MESSAGE = ProjectCommon.Properties.Settings.Default.FollowMeMessage.Replace("nextLine", "\r\n");
            TRAINING_TAP_WITH_ME_MESSAGE = ProjectCommon.Properties.Settings.Default.TapWithMeMessage.Replace("nextLine", "\r\n");
            TRAINING_TAP_WITH_ME_AGAIN_MESSAGE = ProjectCommon.Properties.Settings.Default.TapWithMeAgainMessage.Replace("nextLine", "\r\n");
            TRAINING_DONE_MESSAGE = ProjectCommon.Properties.Settings.Default.TrainingDoneMessage.Replace("nextLine", "\r\n");

        }



    }
}

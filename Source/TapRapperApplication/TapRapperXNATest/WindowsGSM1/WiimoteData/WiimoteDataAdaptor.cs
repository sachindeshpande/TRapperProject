using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

using ProjectCommon;
using Utilities;
using MatlabWrapper;
using CSNamedPipeClient;

using System.Runtime.CompilerServices;

using WiimoteData;
using MotionRecognition;
using Logging;

namespace WiimoteData
{
    public class WiimoteDataAdaptor : DataAdaptor
    {

        private static Process m_wiimoteProcess;

        private CommandNamedPipe m_CommandPipe;
        private DataEventNamedPipe m_DataPipe;


        public WiimoteDataAdaptor(Wiimotes pParent)
            : base(pParent)
        {

        }


        public override void initialize(WiimoteCalibrationRecordInfo calibrationRecord)
        {

            ProcessHelper.cleanupProcesses(Path.GetFileNameWithoutExtension(ProjectConstants.WIIMOTE_CPP_APP));
            

            m_CommandPipe = new CommandNamedPipe();
            m_DataPipe = new DataEventNamedPipe();

            m_DataPipe.PipeDataEvent += new DataEventNamedPipe.OnPipeDataEvent(collectWiimoteData);
            mMotionRecognition.WiimoteActionEvent += new MotionRecognitionMain.OnWiimoteActionEvent(OnWiimoteActionEvent);

            m_CalibrationRecord = calibrationRecord;

        }

        public override void cleanup()
        {
            m_CommandPipe.cleanup();
            m_DataPipe.cleanup();
        }

        #region Pipe Calls
            

        private void startWiimotePipe()
        {
            try
            {
                m_CommandPipe.startPipe();
                m_DataPipe.startPipe();
                startWiimoteDataCollection();
            }
            catch (PipeCommunicationException ex)
            {
                throw ex;
            }

        }

        private void stopWiimotePipe()
        {
            m_CommandPipe.closePipe();
            m_DataPipe.closePipe();
        }

        public bool checkPipeStatus()
        {
            return m_CommandPipe.checkPipeStatus();
        }

        public void startWiimoteDataCollection()
        {
//            string lCppFilePath = wiimoteRecord.FilePath.Insert(wiimoteRecord.FilePath.Length - 4, "Cpp");
//            string startWiimoteLoggingMessage = START_DATA_SENDING_METHOD_NAME + "," + lCppFilePath + "," + ";";
            string startWiimoteLoggingMessage = START_DATA_SENDING_METHOD_NAME + ";";

            m_DataPipe.startWiimoteLogging();

            m_CommandPipe.sendMessage(startWiimoteLoggingMessage);
        }

        #endregion


        #region Connection Calls
        public override bool connectWiimote()
        {
            try
            {
                ProcessHelper.cleanupProcesses(Path.GetFileNameWithoutExtension(ProjectConstants.WIIMOTE_CPP_APP));

                m_wiimoteProcess = new Process();
                m_wiimoteProcess.StartInfo.UseShellExecute = false;
                m_wiimoteProcess.StartInfo.FileName = ProjectConstants.WIIMOTE_CPP_APP;
                m_wiimoteProcess.StartInfo.CreateNoWindow = true;
//                m_wiimoteProcess.Start();

                startWiimotePipe();
                setWiimoteSimulationMode(Configuration.getConfiguration().WiimoteSimulationMode,
                    Configuration.getConfiguration().WiimoteSimulationFile);
                setWiimoteDataMode(Configuration.getConfiguration().WiimoteDataMode);

                return true;
            }
            catch (PipeCommunicationException ex)
            {                
                throw new WiimoteCommunicationException(ex);
            }
            return false;
        }


        public override void disconnectWiimote()
        {
            try
            {
                if (m_wiimoteProcess == null)
                    return;

                stopWiimotePipe();
                //stopWiimoteProcess();

                ProcessHelper.killProcess(m_wiimoteProcess);
                m_wiimoteProcess = null;
            }
            catch (Exception ex)
            {
                throw new WiimoteCommunicationException(ex);
            }
        }


        public bool stopWiimoteProcess()
        {
            try
            {
                string stopWiimoteLoggingMessage = STOP_WIIMOTE_PROCESS_METHOD_NAME + ";";
                m_CommandPipe.sendMessage(stopWiimoteLoggingMessage);
                return true;
            }
            catch (PipeCommunicationException ex)
            {
                throw new WiimoteCommunicationException(ex);
            }
            return false;
        }

        #endregion

        #region Recording/Logging calls

        public void collectWiimoteData(object sender, PipeDataEventArgs e)
        {
            ConsoleLogger.logMessage("In collectWiimoteData");
            try
            {
                logWiimoteData(e.DataList);
                addMatrixRow(e.DataList);

            }
            catch (CSVFileFormatException ex)
            {
                MessageBox.Show(ex.Message, "CSV File Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        public override void setWiimoteSimulationMode(bool p_SimulationMode, string SimulationFile)
        {
            string setWiimoteSimulationModeMessage = SET_WIIMOTE_SIMULATION_MODE_METHOD_NAME + "," + p_SimulationMode + "," +
                SimulationFile + ";";
            m_CommandPipe.sendMessage(setWiimoteSimulationModeMessage);
        }

        public override string setWiimoteDataMode(int p_WiimoteDataMode)
        {
            string setWiimoteDataModeMessage = SET_WIIMOTE_DATA_MODE_METHOD_NAME + "," + p_WiimoteDataMode + ";";
            return m_CommandPipe.sendMessage(setWiimoteDataModeMessage);
        }

        public override string setCalibration()
        {
            string setCalibrationMessage = SET_CALIBRATION_METHOD_NAME + "," + m_CalibrationRecord.getCalibrationValue(ProjectConstants.WIMMOTE1_DATA_ACCX_COLUMN_INDEX) + "," +
                    m_CalibrationRecord.getCalibrationValue(ProjectConstants.WIMMOTE1_DATA_ACCY_COLUMN_INDEX) + "," +
                    m_CalibrationRecord.getCalibrationValue(ProjectConstants.WIMMOTE1_DATA_ACCZ_COLUMN_INDEX) + "," +
                    m_CalibrationRecord.getCalibrationValue(ProjectConstants.WIMMOTE1_DATA_YAW_COLUMN_INDEX) + "," +
                    m_CalibrationRecord.getCalibrationValue(ProjectConstants.WIMMOTE1_DATA_PITCH_COLUMN_INDEX) + "," +
                    m_CalibrationRecord.getCalibrationValue(ProjectConstants.WIMMOTE1_DATA_ROLL_COLUMN_INDEX) + "," +
                    m_CalibrationRecord.getCalibrationValue(ProjectConstants.WIMMOTE2_DATA_ACCX_COLUMN_INDEX) + "," +
                    m_CalibrationRecord.getCalibrationValue(ProjectConstants.WIMMOTE2_DATA_ACCY_COLUMN_INDEX) + "," +
                    m_CalibrationRecord.getCalibrationValue(ProjectConstants.WIMMOTE2_DATA_ACCZ_COLUMN_INDEX) + "," +
                    m_CalibrationRecord.getCalibrationValue(ProjectConstants.WIMMOTE2_DATA_YAW_COLUMN_INDEX) + "," +
                    m_CalibrationRecord.getCalibrationValue(ProjectConstants.WIMMOTE2_DATA_PITCH_COLUMN_INDEX) + "," +
                    m_CalibrationRecord.getCalibrationValue(ProjectConstants.WIMMOTE2_DATA_ROLL_COLUMN_INDEX) + ";";

            return m_CommandPipe.sendMessage(setCalibrationMessage);

        }

        public override bool StartDataCollection(string csvFilepath, bool csvFileCreate = true)
        {
            return true;
        }

        public override bool StopDataCollection()
        {
            return true;
        }

        /**
         * This method
         *  1) Prepares the csv log file for logging Wiimote Data
         *  2) Initializes the Matlab matrix to send data
         */

        public override string startWiimoteLogging(IWiimoteChildRecord wiimoteRecord)
        {
            try
            {
                ITrainingSegmentInfo l_ParentRecord = (ITrainingSegmentInfo)wiimoteRecord.ParentRecord;
                //TODO : Change to member variable
                IWiimoteCalibrationRecordInfo l_Calibration = WiimoteDataStore.getWiimoteDataStore().Calibration;
                setCalibration();

                m_CSVFileWriter = new WiimoteCSVFileWriter(wiimoteRecord.FilePath.Insert(wiimoteRecord.FilePath.Length - 4, "CSharp"));
                m_CSVFileWriter = new WiimoteCSVFileWriter(wiimoteRecord.FilePath);
                m_CSVFileWriter.logHeader(m_CalibrationRecord);

                startRecordingSignalMatlab();
            }
            catch (PipeCommunicationException ex)
            {
                throw new WiimoteCommunicationException(ex);
            }
            catch (CSVFileFormatException ex)
            {
                throw new WiimoteCommunicationException(ex);
            }

            return null;
        }



        public override void stopWiimoteLogging()
        {
            try
            {
//                m_CommandPipe.sendMessage(STOP_DATA_SENDING_METHOD_NAME + ";");
                endRecordingSignalMatlab();
                m_CSVFileWriter.close();
            }
            catch (PipeCommunicationException ex)
            {
                throw new WiimoteCommunicationException(ex);
            }
        }

        public override bool checkWiimoteLoggingStatus()
        {
            try
            {
                return m_CommandPipe.checkPipeStatus();
            }
            catch (PipeCommunicationException ex)
            {
                throw new WiimoteCommunicationException(ex);
            }
        }



        #endregion

        #region Wiimote state Checker

        [MethodImpl(MethodImplOptions.Synchronized)]
        public override void checkWiimoteState(Wiimote wiimote1, Wiimote wiimote2)
        {
            try
            {
                Thread.Sleep(Configuration.getConfiguration().WaitTimeCheckConnection);

                string checkWiimoteStateMessage = CHECK_WIIMOTE_STATE_METHOD_NAME + ";";
                string returnString = m_CommandPipe.sendMessage(checkWiimoteStateMessage);
                if (returnString.Length < 4)
                    return;

                string[] returnValues = returnString.Split(',');

                wiimote1.WiimoteCurrentState = (WiimoteState)Convert.ToInt32(returnValues[0]);
                wiimote1.BatteryLevel = Convert.ToInt32(returnValues[1]);
                wiimote2.WiimoteCurrentState = (WiimoteState)Convert.ToInt32(returnValues[2]);
                wiimote2.BatteryLevel = Convert.ToInt32(returnValues[3]);

            }
            catch (PipeCommunicationException ex)
            {
                throw new WiimoteCommunicationException(ex);
            }
        }

       #endregion

    }
}

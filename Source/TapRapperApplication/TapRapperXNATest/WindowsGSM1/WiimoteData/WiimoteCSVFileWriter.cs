using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Utilities;

using ProjectCommon;
using Logging;

namespace WiimoteData
{
    public class WiimoteCSVLineItem
    {

    }

    public class WiimoteCSVFileWriter : CSVFileWriter
    {
        protected string[] mInformationRow;

        public const int WIMMOTE_NUMBER_OF_COLUMNS_LOGGING = 24;

        public WiimoteCSVFileWriter(string path)
            : base(path)
        {
            mInformationRow = new string[WIMMOTE_NUMBER_OF_COLUMNS_LOGGING];
        }


        public virtual void logHeader(IWiimoteCalibrationRecordInfo p_CalibrationRecord)
        {
            try
            {
                writeLine("BPM," + 0);
                writeLine("BPB," + 0);
                writeLine("NumBar," + 0);
                writeLine("LeadIn," + 0);
                writeLine();

                writeLine("MusicFile,None");
                writeLine();

                writeLine("Calibration Info Start");
                if (p_CalibrationRecord.WiimotesSwitched)
                    writeLine("LRswitched,True");
                else
                    writeLine("LRswitched,False");

                writeLine("Sequence #,Time,Acc Pitch Orientation #WM1, Acc Roll Orientation #WM1,Acc X #WM1,Acc Y #WM1,Acc Z #WM1," +
                                      "Raw Yaw #WM1,Raw Pitch #WM1,Raw Roll #WM1,Speed Yaw #WM1, Speed Pitch #WM1,Speed Roll #WM1," +
                                      "Acc Pitch Orientation #WM2, Acc Roll Orientation #WM2,Acc X #WM2,Acc Y #WM2,Acc Z #WM2," +
                                      "Raw Yaw #WM2,Raw Pitch #WM2,Raw Roll #WM2,Speed Yaw #WM2, Speed Pitch #WM2,Speed Roll #WM2");

                string calibrationMessage = "0,0,0,0," +
                p_CalibrationRecord.getCalibrationValue(ProjectConstants.WIMMOTE1_DATA_ACCX_COLUMN_INDEX) + "," +
                p_CalibrationRecord.getCalibrationValue(ProjectConstants.WIMMOTE1_DATA_ACCY_COLUMN_INDEX) + "," +
                p_CalibrationRecord.getCalibrationValue(ProjectConstants.WIMMOTE1_DATA_ACCZ_COLUMN_INDEX) + ",0,0,0," +
                p_CalibrationRecord.getCalibrationValue(ProjectConstants.WIMMOTE1_DATA_YAW_COLUMN_INDEX) + "," +
                p_CalibrationRecord.getCalibrationValue(ProjectConstants.WIMMOTE1_DATA_PITCH_COLUMN_INDEX) + "," +
                p_CalibrationRecord.getCalibrationValue(ProjectConstants.WIMMOTE1_DATA_ROLL_COLUMN_INDEX) + ",0,0," +
                p_CalibrationRecord.getCalibrationValue(ProjectConstants.WIMMOTE2_DATA_ACCX_COLUMN_INDEX) + "," +
                p_CalibrationRecord.getCalibrationValue(ProjectConstants.WIMMOTE2_DATA_ACCY_COLUMN_INDEX) + "," +
                p_CalibrationRecord.getCalibrationValue(ProjectConstants.WIMMOTE2_DATA_ACCZ_COLUMN_INDEX) + ",0,0,0," +
                p_CalibrationRecord.getCalibrationValue(ProjectConstants.WIMMOTE2_DATA_YAW_COLUMN_INDEX) + "," +
                p_CalibrationRecord.getCalibrationValue(ProjectConstants.WIMMOTE2_DATA_PITCH_COLUMN_INDEX) + "," +
                p_CalibrationRecord.getCalibrationValue(ProjectConstants.WIMMOTE2_DATA_ROLL_COLUMN_INDEX);
                writeLine(calibrationMessage);


                writeLine("Calibration Info End");
                writeLine();

                writeLine("Sequence #,Time,Acc Pitch Orientation #WM1, Acc Roll Orientation #WM1,Acc X #WM1,Acc Y #WM1,Acc Z #WM1," +
                                      "Raw Yaw #WM1,Raw Pitch #WM1,Raw Roll #WM1,Speed Yaw #WM1, Speed Pitch #WM1,Speed Roll #WM1," +
                                      "Acc Pitch Orientation #WM2, Acc Roll Orientation #WM2,Acc X #WM2,Acc Y #WM2,Acc Z #WM2," +
                                      "Raw Yaw #WM2,Raw Pitch #WM2,Raw Roll #WM2,Speed Yaw #WM2, Speed Pitch #WM2,Speed Roll #WM2");
            }
            catch (CSVFileException e)
            {
                throw e;
            }
        }


        public virtual void writeLine(string[] row, bool wiimotesSwitched)
        {
            try
            {
                if (row == null || row.Length == 0)
                    throw new CSVFileFormatException();

                StringBuilder line = new StringBuilder();

                if (row.Length == 1) //This is for the video Start/End line
                {
                    line.Append(row[0]);
                    writeLine(line.ToString());
                    return;
                }

                line.Append(row[0] + ",");
                line.Append(row[1]);

                if (wiimotesSwitched)
                {
                    int startIndex = (row.Length - 2) / 2 + 2;
                    for (int i = startIndex; i < row.Length; i++)
                        line.Append("," + row[i]);
                    for (int i = 2; i < startIndex; i++)
                        line.Append("," + row[i]);
                }
                else
                {
                    for (int i = 2; i < row.Length; i++)
                        line.Append("," + row[i]);
                }

                writeLine(line.ToString());

            }
            catch (CSVFileException e)
            {
                throw e;
            }
        }


        public void LoggingExecute()
        {

        }

        public virtual void addCSVLineItemToLogFile(WiimoteCSVLineItem csvLineItem)
        {

        }

        public void addInformationToLogFile(string[] pRowData)
        {

            int lTotalNumberOfColumns = mInformationRow.Length;
            for (int lIndex = 0; lIndex < lTotalNumberOfColumns; lIndex++)
                mInformationRow[lIndex] = "0";

            int lCount = pRowData.Length;
            for (int lIndex = 0; lIndex < lCount; lIndex++)
                mInformationRow[lIndex] = pRowData[lIndex];

            writeLine(mInformationRow, false);
        }

        public virtual void close()
        {
            base.close();
        }

    }
}

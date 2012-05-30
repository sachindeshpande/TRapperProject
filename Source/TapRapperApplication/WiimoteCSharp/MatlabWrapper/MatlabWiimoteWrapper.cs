using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MatlabWrapper
{
    using MLApp;

    public class MatlabWiimoteWrapper
    {
        private static MLAppClass m_matlabComObject;

        private const string SCORE_COMMAND = "score";
        private const string AVERAGE_DELAY_COMMAND = "avedelay";
        private const string FLUCTUATION_COMMAND = "fluctuation";
        private const string STARS_COMMAND = "stars";

        static void Main(string[] args)
        {
            string workingDirectory = MatlabWrapper.Properties.Settings.Default.WorkingDirectory;
            string wiimoteApplicationPath = MatlabWrapper.Properties.Settings.Default.WiimoteApplicationPath;
            string matlabApplicationPath = MatlabWrapper.Properties.Settings.Default.MatlapApplicationPath;

            ProjectCommon.ProjectConstants.initialize(workingDirectory, wiimoteApplicationPath, matlabApplicationPath, workingDirectory);

            MatlabWiimoteWrapper.initialize();
            string command = @"playvsref J:\Projects\WiimoteProjects\WiiMotionPlus\Version15-WithTUIO\wiicompare\Play1.csv J:\Projects\WiimoteProjects\WiiMotionPlus\Version15-WithTUIO\wiicompare\Play2.csv";
            MatlabWiimoteWrapper.matlabCommand(command);

        }

        public static void initialize()
        {           
            //m_matlabComObject = new MLAppClass();
            //string dirCommand = "cd " + ProjectCommon.ProjectConstants.MATLAB_DIRECTORY;
            //m_matlabComObject.Execute(dirCommand);
             
        }

        public static string matlabCommand(string command)
        {
            string returnString = "";
 //           string returnString = m_matlabComObject.Execute(command);
//            return returnString.Replace("\u000a".ToString(), Environment.NewLine);
            returnString = returnString.Replace("\r", "");
            returnString = returnString.Replace("\n", "");
            returnString = returnString.Replace("=", "");
            returnString = returnString.Replace("'", "");
            returnString = returnString.Replace(command, "");
            return returnString;
        }

        public static void setWiimoteData(List<string[]> pWiimoteDataMatrix)
        {
            int lNumRows = pWiimoteDataMatrix.Count;
            if (lNumRows == 0)
                return;

            string[] lRowValue = pWiimoteDataMatrix[0];
            int lNumColumns = lRowValue.Length;

            StringBuilder setMatrixCommand = new StringBuilder();

            setMatrixCommand.Append("wiimoteData = ");
            setMatrixCommand.Append("[");

            for (int lRowIndex = 0; lRowIndex < lNumRows; lRowIndex++)
            {
                lRowValue = pWiimoteDataMatrix[lRowIndex];

                for (int lColumnIndex = 0; lColumnIndex < lNumColumns; lColumnIndex++)
                {
                    if (isRawYawColumn(lColumnIndex))
                        lColumnIndex = lColumnIndex + 2;
                    else
                        setMatrixCommand.Append(lRowValue[lColumnIndex] + ",");
                }
                setMatrixCommand.Replace(',', ';', setMatrixCommand.Length-1, 1);
            }

            setMatrixCommand.Remove(setMatrixCommand.Length - 1, 1);
            setMatrixCommand.Append("]");

            matlabCommand(setMatrixCommand.ToString());
            matlabCommand("wiidataset");//wytry
        }

        public static string checkWiimoteEvent()
        {
            return null;
        }

        private static bool isRawYawColumn(int pColumnNumber)
        {
            if (pColumnNumber == ProjectCommon.ProjectConstants.WIMMOTE1_DATA_RAW_YAW_COLUMN_INDEX ||
                pColumnNumber == ProjectCommon.ProjectConstants.WIMMOTE2_DATA_RAW_YAW_COLUMN_INDEX)
                return true;
            return false;
        }

        public static string calculateScore(string referenceCode,string playFilepath,
            out double score, out double stars)
        {
            string returnMessage = null;

            try
            {
                string globalVariableCommand = "global stars score";
                returnMessage = matlabCommand(globalVariableCommand);

                string command = ProjectCommon.ProjectConstants.MATLAB_APP_NAME + " " + referenceCode +
                    " " + playFilepath;
                returnMessage = matlabCommand(command);

                stars = Convert.ToDouble(matlabCommand(STARS_COMMAND));
                score = Convert.ToDouble(matlabCommand(SCORE_COMMAND));
            }
            catch (Exception e) //TODO : This is a temporary solution for the Matlab parsing issue
            {
                stars = 2;
                score = 1.5;
            }


            return returnMessage;
            
        }
    }
}

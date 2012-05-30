using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjectCommon;

namespace MotionRecognition
{
    class MotionRecognitionData
    {
        public const int MAX_NUMBER_OF_ROWS_MOTION_DATA = 100;
        public const int NUMBER_OF_OVERLAPPING_ROWS = 10;

        public double[,] MotionDataA { get; set; }
        public double[,] MotionDataB { get; set; }

        public double[,] CurrentMotionData;
        public int mCurrentRowIndex;

        private MotionRecognitionMain mParent;

        private enum UsedArrayType
        {
            A,
            B
        }

        private UsedArrayType UsedArray;

        public MotionRecognitionData(MotionRecognitionMain pParent)
        {
            MotionDataA = new double[ProjectConstants.WIMMOTE_NUMBER_OF_COLUMNS, MAX_NUMBER_OF_ROWS_MOTION_DATA];
            MotionDataB = new double[ProjectConstants.WIMMOTE_NUMBER_OF_COLUMNS, MAX_NUMBER_OF_ROWS_MOTION_DATA];

            CurrentMotionData = MotionDataA;
            UsedArray = UsedArrayType.A;
            mCurrentRowIndex = 0;

            mParent = pParent;
        }

        public void addMotionDataRecord(double[] pMotionDataRecord)
        {
            for (int lColumnIndex = 0; lColumnIndex < ProjectConstants.WIMMOTE_NUMBER_OF_COLUMNS; lColumnIndex++)
                CurrentMotionData[mCurrentRowIndex,lColumnIndex] = pMotionDataRecord[lColumnIndex];

            mCurrentRowIndex++;

            if (mCurrentRowIndex > MAX_NUMBER_OF_ROWS_MOTION_DATA - 1)
            {
                checkMotionData();
                transferToOtherArray();
            }
        }

        private void transferToOtherArray()
        {

            if (UsedArray == UsedArrayType.A)
            {
                clearArray(MotionDataB);
                copyArray(MotionDataA, MotionDataB, MAX_NUMBER_OF_ROWS_MOTION_DATA - NUMBER_OF_OVERLAPPING_ROWS - 1);
                UsedArray = UsedArrayType.B;
                CurrentMotionData = MotionDataB;
            }
            else
            {
                clearArray(MotionDataA);
                copyArray(MotionDataB, MotionDataA, MAX_NUMBER_OF_ROWS_MOTION_DATA - NUMBER_OF_OVERLAPPING_ROWS - 1);
                UsedArray = UsedArrayType.A;
                CurrentMotionData = MotionDataA;
            }

            mCurrentRowIndex = NUMBER_OF_OVERLAPPING_ROWS - 1;
        }

        private void copyArray(double[,] pSource, double[,] pDestination, int pStartSourceIndex)
        {
            for (int lSourceRowIndex = pStartSourceIndex; lSourceRowIndex < MAX_NUMBER_OF_ROWS_MOTION_DATA - 1; lSourceRowIndex++)
            {
                for (int lColumnIndex = 0; lColumnIndex < ProjectConstants.WIMMOTE_NUMBER_OF_COLUMNS - 1; lColumnIndex++)
                    pDestination[lSourceRowIndex - pStartSourceIndex, lColumnIndex] = pSource[lSourceRowIndex, lColumnIndex];
            }
        }

        private void clearArray(double[,] pArrayData)
        {
            for (int lRowIndex = 0; lRowIndex < MAX_NUMBER_OF_ROWS_MOTION_DATA - 1; lRowIndex++)
            {
                for (int lColumnIndex = 0; lColumnIndex < ProjectConstants.WIMMOTE_NUMBER_OF_COLUMNS - 1; lColumnIndex++)
                    pArrayData[lRowIndex, lColumnIndex] = 0;
            }
        }

        /**
         * This is the function which will analyze the motion data in the CurrentMotionData matrix
         * */

        public void checkMotionData()
        {
            WiimoteAction lAction = new WiimoteAction(ProjectCommon.ProjectConstants.TRAINING_NEXT_SELECTION);
            mParent.sendWiimoteActionEvent(lAction);
            
        }
    }
}

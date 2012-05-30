using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjectCommon;

namespace MotionRecognition
{


    public class WiimoteAction
    {
        public WiimoteAction(string pActionType)
        {
            ActionType = pActionType;            
        }

        public string ActionType { get; set; }
    }

    public class WiimoteActionEventArgs : EventArgs
    {
        public WiimoteActionEventArgs(WiimoteAction pWiimoteActionObject)
        {
            WiimoteActionObject = pWiimoteActionObject;
        }

        public WiimoteAction WiimoteActionObject { get; set; }
    }


    public class MotionRecognitionMain
    {
        private MotionRecognitionData MotionRecognitionDataObject;

        public delegate void OnWiimoteActionEvent(object sender, WiimoteActionEventArgs args);

        public event OnWiimoteActionEvent WiimoteActionEvent;

        public MotionRecognitionMain()
        {
            MotionRecognitionDataObject = new MotionRecognitionData(this);
        }

        public void addMotionDataRecord(double [] pMotionDataRecord)
        {
            MotionRecognitionDataObject.addMotionDataRecord(pMotionDataRecord);
        }

        public void sendWiimoteActionEvent(WiimoteAction pWiimoteAction)
        {
            WiimoteActionEvent(this, new WiimoteActionEventArgs(pWiimoteAction));
        }

    }
}

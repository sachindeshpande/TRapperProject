using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Data;
using SpeechLib;

namespace SpeechTest
{
    public class VoiceCommandEventArgs : EventArgs
    {
        public const string STOP_COMMAND = "Stop";
        public const string CONTINUE_COMMAND = "Continue";
        public const string NO_COMMAND = "None";

        public string VoiceCommandValue { get; set; }

        public VoiceCommandEventArgs(string pVoiceCommandValue)
        {
            VoiceCommandValue = pVoiceCommandValue;
        }
    }



    public class SpeechRecognition
    {
        private static SpeechRecognition mSpeechRecognition;
        private SpeechLib.SpSharedRecoContext objRecoContext = null;
        private SpeechLib.ISpeechRecoGrammar grammar = null;
        private SpeechLib.ISpeechGrammarRule menuRule = null;

        public delegate void OnVoiceCommandReceivedEvent(object sender, VoiceCommandEventArgs args);
        public event OnVoiceCommandReceivedEvent VoiceCommandReceivedEvent;

        SpeechRecognition()
        {
        }

        public static SpeechRecognition getSpeechRecognition()
        {
            if (mSpeechRecognition == null)
                mSpeechRecognition = new SpeechRecognition();
            return mSpeechRecognition;
        }

        public void startEngine()
        {
            // Get an insance of RecoContext. I am using the shared RecoContext.
            objRecoContext = new SpeechLib.SpSharedRecoContext();
            // Assign a eventhandler for the Hypothesis Event.
            objRecoContext.Hypothesis += new _ISpeechRecoContextEvents_HypothesisEventHandler(Hypo_Event);
            // Assign a eventhandler for the Recognition Event.
            objRecoContext.Recognition += new
                _ISpeechRecoContextEvents_RecognitionEventHandler(Reco_Event);
            //Creating an instance of the grammer object.
            grammar = objRecoContext.CreateGrammar(0);

            //Activate the Menu Commands.			
            menuRule = grammar.Rules.Add("MenuCommands", SpeechRuleAttributes.SRATopLevel | SpeechRuleAttributes.SRADynamic, 1);
            object PropValue = "";

            menuRule.InitialState.AddWordTransition(null, "Nothing", " ", SpeechGrammarWordType.SGLexical, "Nothing", 1, ref PropValue, 1.0F);
            menuRule.InitialState.AddWordTransition(null, "Continue", " ", SpeechGrammarWordType.SGLexical, "Continue", 2, ref PropValue, 1.0F);
            menuRule.InitialState.AddWordTransition(null, "Stop", " ", SpeechGrammarWordType.SGLexical, "Stop", 3, ref PropValue, 1.0F);
            grammar.Rules.Commit();
            grammar.CmdSetRuleState("MenuCommands", SpeechRuleState.SGDSActive);
        }

        public void startMicTrainingWizard()
        {
            menuRule = null;
            grammar = null;
            objRecoContext = null;
        }

        public void stopEngine()
        {
            menuRule = null;
            grammar = null;
            objRecoContext = null;
        }

        private void Reco_Event(int StreamNumber, object StreamPosition, SpeechRecognitionType RecognitionType, ISpeechRecoResult Result)
        {
            string lWord = Result.PhraseInfo.GetText(0, -1, true);
            if (VoiceCommandReceivedEvent != null)
                VoiceCommandReceivedEvent(this, new VoiceCommandEventArgs(lWord));            
        }

        private void Hypo_Event(int StreamNumber, object StreamPosition, ISpeechRecoResult Result)
        {
            string lWord = Result.PhraseInfo.GetText(0, -1, true);

        }
    }
}

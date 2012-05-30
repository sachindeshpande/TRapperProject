using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.ComponentModel;
using System.Data;
using SpeechLib;

namespace VoiceCommandControl
{
    public class VoiceCommandEventArgs : EventArgs
    {
        public const string EXIT_COMMAND = "Tap Exit";
        public const string TRAINING_COMMAND = "Tap Training";
        public const string STOP_COMMAND = "Tap Stop";
        public const string PAUSE_COMMAND = "Tap Pause";
        public const string REPEAT_COMMAND = "Tap Repeat";
        public const string BACK_COMMAND = "Tap Back";
        public const string FORWARD_COMMAND = "Tap Forward";
        public const string CONTINUE_COMMAND = "Tap Continue";
        public const string SLOWMO_COMMAND = "Tap SlowMo";
        public const string SCORING_COMMAND = "Tap Scoring";
        public const string NO_COMMAND = "None";
        public const string CALIBRATE_COMMAND = "Tap Setup";
        public const string CONNECT_WIIMOTES_COMMAND = "Tap Connect Wiimotes";

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

        public bool mEngineStarted;

        SpeechRecognition()
        {
            mEngineStarted = false;
        }

        public static SpeechRecognition getSpeechRecognition()
        {
            if (mSpeechRecognition == null)
                mSpeechRecognition = new SpeechRecognition();
            return mSpeechRecognition;
        }

        public bool isEngineStarted()
        {
            return mEngineStarted;
        }

        public void startEngine()
        {
            if (mEngineStarted)
                return;

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

            menuRule.InitialState.AddWordTransition(null, VoiceCommandEventArgs.NO_COMMAND, " ", SpeechGrammarWordType.SGLexical, VoiceCommandEventArgs.NO_COMMAND, 1, ref PropValue, 1.0F);
            menuRule.InitialState.AddWordTransition(null, VoiceCommandEventArgs.REPEAT_COMMAND, " ", SpeechGrammarWordType.SGLexical, VoiceCommandEventArgs.REPEAT_COMMAND, 2, ref PropValue, 1.0F);
            menuRule.InitialState.AddWordTransition(null, VoiceCommandEventArgs.STOP_COMMAND, " ", SpeechGrammarWordType.SGLexical, VoiceCommandEventArgs.STOP_COMMAND, 3, ref PropValue, 1.0F);
            menuRule.InitialState.AddWordTransition(null, VoiceCommandEventArgs.PAUSE_COMMAND, " ", SpeechGrammarWordType.SGLexical, VoiceCommandEventArgs.PAUSE_COMMAND, 3, ref PropValue, 1.0F);
            menuRule.InitialState.AddWordTransition(null, VoiceCommandEventArgs.BACK_COMMAND, " ", SpeechGrammarWordType.SGLexical, VoiceCommandEventArgs.BACK_COMMAND, 3, ref PropValue, 1.0F);
            menuRule.InitialState.AddWordTransition(null, VoiceCommandEventArgs.FORWARD_COMMAND, " ", SpeechGrammarWordType.SGLexical, VoiceCommandEventArgs.FORWARD_COMMAND, 3, ref PropValue, 1.0F);
            menuRule.InitialState.AddWordTransition(null, VoiceCommandEventArgs.CALIBRATE_COMMAND, " ", SpeechGrammarWordType.SGLexical, VoiceCommandEventArgs.CALIBRATE_COMMAND, 3, ref PropValue, 1.0F);
            menuRule.InitialState.AddWordTransition(null, VoiceCommandEventArgs.SLOWMO_COMMAND, " ", SpeechGrammarWordType.SGLexical, VoiceCommandEventArgs.SLOWMO_COMMAND, 3, ref PropValue, 1.0F);
            menuRule.InitialState.AddWordTransition(null, VoiceCommandEventArgs.TRAINING_COMMAND, " ", SpeechGrammarWordType.SGLexical, VoiceCommandEventArgs.TRAINING_COMMAND, 3, ref PropValue, 1.0F);
            menuRule.InitialState.AddWordTransition(null, VoiceCommandEventArgs.CONTINUE_COMMAND, " ", SpeechGrammarWordType.SGLexical, VoiceCommandEventArgs.CONTINUE_COMMAND, 3, ref PropValue, 1.0F);
            menuRule.InitialState.AddWordTransition(null, VoiceCommandEventArgs.SCORING_COMMAND, " ", SpeechGrammarWordType.SGLexical, VoiceCommandEventArgs.SCORING_COMMAND, 3, ref PropValue, 1.0F);
            menuRule.InitialState.AddWordTransition(null, "Current", " ", SpeechGrammarWordType.SGLexical, "Current", 3, ref PropValue, 1.0F);
            menuRule.InitialState.AddWordTransition(null, "Video", " ", SpeechGrammarWordType.SGLexical, "Video", 3, ref PropValue, 1.0F);
            grammar.Rules.Commit();
            grammar.CmdSetRuleState("MenuCommands", SpeechRuleState.SGDSActive);

            mEngineStarted = true;
        }

        public void stopEngine()
        {
            menuRule = null;
            grammar = null;
            objRecoContext = null;
            mEngineStarted = false;
        }

        public bool configureMicrophone(IntPtr pParentHandle)
        {
            object str1 = "";

            if (objRecoContext.Recognizer.IsUISupported("UserTraining", ref str1) == true)
            {
                objRecoContext.Recognizer.DisplayUI((int)pParentHandle, "SR", "UserTraining", ref str1);
                return true;
            }
            else
                return false;
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

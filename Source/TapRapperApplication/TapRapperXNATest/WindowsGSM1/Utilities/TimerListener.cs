using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilities
{
    public interface TimerListener
    {
        void OnStartingRecording(int numBeats, int numBars, int numBeatInBar);
        void OnTimedEvent(int numBeats, int numBars, int numBeatInBar);
        void OnRecordingInCompletedEvent(int numBeats, int numBars, int numBeatInBar);
    }
}



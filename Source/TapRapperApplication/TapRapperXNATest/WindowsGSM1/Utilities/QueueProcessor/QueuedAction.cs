using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilities.QueueProcessor
{
    /// <summary>
    /// Any client that needs to use the QueuedProcessor needs to implement this interface
    /// </summary>
    public abstract class QueuedAction
    {
        //Check stating if this is the last action in the queue
        protected bool _isLastAction;

        public QueuedAction(bool isLastAction)
        {
        }

        //This is the method that is called by the QueuedProcessor for executing the code
        public abstract void Perform();

        //This is a check to verify if it is the last action. After this action the queue is stopped
        public bool IsLastAction()
        {
            return _isLastAction;
        }

    }
}

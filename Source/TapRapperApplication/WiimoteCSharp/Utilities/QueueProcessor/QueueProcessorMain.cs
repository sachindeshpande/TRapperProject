using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Utilities.QueueProcessor
{
    /// <summary>
    /// This is allows you to perform a set of actions that are added to a queue in a threaded manner.
    /// The client can add any action to the queue as long as it implements the QueuedAction Interface
    /// The code defined in the Perform class is then executed
    /// </summary>
    public class QueueProcessorMain
    {
        private SynchronizedQueue<QueuedAction> _queuedActionList = new SynchronizedQueue<QueuedAction>();

        private bool _queueRunningStatus;

        public QueueProcessorMain()
        {
        }

        /// <summary>
        /// Starts the Queued processing Thread
        /// </summary>
        public void StartQueueProcessing()
        {
            _queueRunningStatus = true;
            //Start Queue Thread
//            Thread updateThread = new Thread(new ThreadStart(ProcessQueueMessages));
            Thread updateThread = ThreadFactory.CreateNewThread(new ThreadStart(ProcessQueueMessages));
            updateThread.Start();
        }

        /// <summary>
        /// Stops the Queued processing Thread
        /// </summary>
        public void StopQueueProcessing()
        {
            _queueRunningStatus = false;
        }

        /// <summary>
        /// Add a new action to the Queue for processing
        /// </summary>
        /// <param name="queuedActionObject"></param>
        public void AddQueuedAction(QueuedAction queuedActionObject)
        {
            if (_queueRunningStatus)
                _queuedActionList.AddItem(queuedActionObject);
        }

        /// <summary>
        /// Method that executes all actions in the Queue
        /// </summary>
        public void ProcessQueueMessages()
        {
            while (_queueRunningStatus)
            {
                QueuedAction queueActionObject = (QueuedAction)_queuedActionList.GetItemWithTimeout(100);

                if (queueActionObject != null)
                {
                    queueActionObject.Perform();

                    if (queueActionObject.IsLastAction())
                    {
                        _queueRunningStatus = false;
                        return;
                    }
                }
            }
        }
    }
}

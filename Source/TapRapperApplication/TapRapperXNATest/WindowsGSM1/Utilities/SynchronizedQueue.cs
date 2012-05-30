using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Utilities.QueueProcessor
{
    /// <summary>
    /// This queue has multithreaded support. It manages consumers and producers accessing it simultaneously 
    /// from multiple threads
    /// </summary>
    public class SynchronizedQueue<T> : Queue<object>
    {
        private AutoResetEvent _messageReceived;
        private const int DEFAULT_MESSAGE_SLEEP_TIME = 10;
        private int _messageWaitTime;

        public SynchronizedQueue()
        {
            Initialize(DEFAULT_MESSAGE_SLEEP_TIME);
        }

        public SynchronizedQueue(int messageWaitTime)
        {
            Initialize(messageWaitTime);
        }

        private void Initialize(int messageWaitTime)
        {
            _messageReceived = new AutoResetEvent(false);
            _messageWaitTime = messageWaitTime;

        }

        /// <summary>
        /// Adds item to the Queue synchronously
        /// </summary>
        /// <param name="item"></param>
        public void AddItem(T t)
        {
            lock (this)
            {
                Enqueue(t);
                _messageReceived.Set();
            }
        }

        /// <summary>
        /// Gets item from the queue synchronously using Infinite timeout
        /// </summary>
        /// <returns></returns>
        public object GetItem()
        {
            lock (this)
            {
                if (Count > 0)
                    return Dequeue();
                else
                    return null;
            }

        }

        /// <summary>
        /// Gets item from the queue synchronously
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public object GetItemWithTimeout(int timeout)
        {
            int timeoutCounter = 0;
            object queueItem = null;

            while ((queueItem = GetItem()) == null)
            {
                Thread.Sleep(_messageWaitTime);

                if (timeout != Timeout.Infinite)
                {
                    timeoutCounter += DEFAULT_MESSAGE_SLEEP_TIME;
                    if (timeoutCounter > timeout)
                        return null;
                }
            }

            return queueItem;

        }
    }
}

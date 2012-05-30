using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Utilities.QueueProcessor
{
    public class ThreadFactory
    {
        private static List<Thread> _threadList = new List<Thread>();

        public static Thread CreateNewThread(ThreadStart threadMethod)
        {
            Thread threadObject = new Thread(threadMethod);
            _threadList.Add(threadObject);
            return threadObject;
        }

        public static void StopAllThreads()
        {
            foreach (Thread threadObject in _threadList)
            {
                threadObject.Abort();
            }
        }

    }
}

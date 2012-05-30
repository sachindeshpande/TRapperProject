using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Utilities.QueueProcessor;

namespace Logging.LogFile
{
    /// <summary>
    /// Queued action flushes the buffer to the log file
    /// </summary>
    public class LoggerFileFlushAction : QueuedAction
    {
        private StreamWriter _dataWriter;

        public LoggerFileFlushAction(StreamWriter dataWriter)
            : base(false)
        {
            _dataWriter = dataWriter;
        }

        /// <summary>
        /// Flush buffer
        /// </summary>
        public override void Perform()
        {
            try
            {
                if (_dataWriter != null)
                    _dataWriter.Flush();

            }
            catch (Exception ex)
            {

            }
        }
    }
}

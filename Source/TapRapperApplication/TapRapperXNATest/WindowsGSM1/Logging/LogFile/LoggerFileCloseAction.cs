using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Utilities.QueueProcessor;


namespace Logging.LogFile
{
    /// <summary>
    /// Queued Action closes the log file
    /// </summary>
    public class LoggerFileCloseAction :QueuedAction
    {
        private StreamWriter _dataWriter;

        public LoggerFileCloseAction(StreamWriter dataWriter)
            : base(true)
        {
            _dataWriter = dataWriter;
            _isLastAction = true;
        }

        /// <summary>
        /// Close log file
        /// </summary>
        public override void Perform()
        {
            try
            {
                _dataWriter.Flush();
                _dataWriter.Close();
            }
            catch (Exception e)
            {
                //Ignore Exception. Do not need to hang the application for logging error
            }
        }
    }
}

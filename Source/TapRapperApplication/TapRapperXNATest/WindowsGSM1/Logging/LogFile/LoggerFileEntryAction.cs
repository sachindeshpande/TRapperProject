using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Utilities.QueueProcessor;


namespace Logging.LogFile
{
    /// <summary>
    /// Queued action logs the string to the log file
    /// </summary>
    public class LoggerFileEntryAction : QueuedAction
    {
        private StreamWriter _dataWriter;
        private string _lineContent;
 //       private DateTime _logEntryTimestamp;
		private string _timeStamp;

        public LoggerFileEntryAction(StreamWriter dataWriter, string lineContent, TimeSpan logEntryTimestamp)
            : base(false)
        {
            _dataWriter = dataWriter;
            _lineContent = lineContent;
        }

        /// <summary>
        /// Log the string to the log file
        /// </summary>
        public override void Perform()
        {

            try
            {
                _dataWriter.Write(_lineContent);
            }
            catch (Exception e)
            {
            }

        }
    }
}

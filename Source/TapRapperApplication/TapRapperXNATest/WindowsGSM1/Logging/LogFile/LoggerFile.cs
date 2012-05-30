using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using Utilities.QueueProcessor;


namespace Logging.LogFile
{
	/// <summary>
	/// Logger file provides mechanism to log entries in a queued manner.
	/// It adds file entry requests to a queue , which are then processed in a separate thread
	/// This file is synchronized and supports multiple threads
	/// </summary>
	public class LoggerFile
	{
		/// <summary>
		/// Constant indicating when to flush (after how many log entries)
		/// </summary>
		private const int FLUSH_CYCLE = 20;

		/// <summary>
		/// Stream Writer
		/// </summary>
		private StreamWriter _dataWriter;

		/// <summary>
		/// Counter indicating number of lines logged before the last flush
		/// </summary>
		private int _flushCounter;

		/// <summary>
		/// Link to the QueedProcessor. All logging  events are added to the queue , which are then processed
		/// in a separate thread
		/// </summary>
		private QueueProcessorMain _queuedProcessor = new QueueProcessorMain();

		/// <summary>
		/// Flag indicating if logging is on
		/// </summary>
		private bool _dataLogging;

        /// <summary>
        /// Lets the Logger file factory know if this file should be moved after
        /// </summary>
        private bool _doNotMove;
        public bool DoNotMove
        {
            get { return _doNotMove; }
            set { _doNotMove = value; }
        }

		/// <summary>
		/// File path for the Logger
		/// </summary>
		private string _filePath;
		public string Filepath
		{
			get { return _filePath; }
		}

		internal LoggerFile(string filePath, bool dataLogging)
		{
			Initialize(filePath, dataLogging);
		}

		/// <summary>
		/// Initializes the Stream writer, Queued Processor and FlushCounter
		/// </summary>
		/// <param name="filePath"></param>
		private void Initialize(string filePath, bool dataLogging)
		{
			_filePath = filePath;
			_dataLogging = dataLogging;

			if (!_dataLogging)
				return;

			_flushCounter = 0;


			FileStream dataFile = new FileStream(_filePath, FileMode.Create);
			_dataWriter = new StreamWriter(dataFile);

			_queuedProcessor.StartQueueProcessing();

		}

        public void WriteLine(string lineContent)
        {
            Write(lineContent + "\r\n");
        }

        public void WriteLine(string[] row)
        {
            try
            {
                StringBuilder line = new StringBuilder();
                line.Append(row[0]);
                for (int i = 1; i < row.Length; i++)
                    line.Append("," + row[i]);
                WriteLine(line.ToString());
            }
            catch (Exception e)
            {
                throw e;
            }

        }


		/// <summary>
		/// Adds the log entry to the Queued Processor
		/// </summary>
		/// <param name="lineContent"></param>
		public void Write(string lineContent)
		{
			if (!_dataLogging)
				return;


			_queuedProcessor.AddQueuedAction(new LoggerFileEntryAction(_dataWriter, lineContent,  TimeSpan.FromTicks(DateTime.Now.Ticks)));

			//Increases the flush counter
			_flushCounter++;

			//Check if the flush counter is reached FLUSH_CYCLE. If yes add flush to the queue
			if (_flushCounter == FLUSH_CYCLE)
			{
				_queuedProcessor.AddQueuedAction(new LoggerFileFlushAction(_dataWriter));
				_flushCounter = 0;
			}


		}

		/// <summary>
		/// Forces logger to flush the queue
		/// </summary>
		public void FlushNow()
		{
			_queuedProcessor.AddQueuedAction(new LoggerFileFlushAction(_dataWriter));
			_flushCounter = 0;
		}

		/// <summary>
		/// Add request for closing the file to the queue
		/// </summary>
		public void Close()
		{
			if (!_dataLogging)
				return;

				_queuedProcessor.AddQueuedAction(new LoggerFileCloseAction(_dataWriter));
		}

		public void CloseImmediate()
		{
			if (!_dataLogging)
				return;

			_queuedProcessor.StopQueueProcessing();
			_dataWriter.Close();
		}

			
	}
}

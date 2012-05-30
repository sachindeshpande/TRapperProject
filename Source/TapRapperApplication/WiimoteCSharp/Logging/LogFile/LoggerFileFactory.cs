using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Logging.Exceptions;

namespace Logging.LogFile
{
    /// <summary>
    /// Factory for creating and maintaining LoggerFile objects
    /// </summary>
    public class LoggerFileFactory
    {
        private static List<LoggerFile> _loggerFileList = new List<LoggerFile>();
        /// <summary>
        /// Create a new LoggerFile object
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static LoggerFile GetLoggerFile(string filePath, bool dataLogging)
        {
            try
            {
                LoggerFile loggerFileObject = new LoggerFile(filePath, dataLogging);
                _loggerFileList.Add(loggerFileObject);
                return loggerFileObject;
            }
            catch (Exception e)
            {
                throw new LoggingException(e);
            }
        }



        /// <summary>
        /// Removes log file object from list. Does not delete the file
        /// </summary>
        /// <param name="logFile"></param>
        /// <returns></returns>
        public static bool ReturnLoggerFileObjectToPool(LoggerFile logFile)
        {
            if (!_loggerFileList.Contains(logFile))
                return false;

            _loggerFileList.Remove(logFile);
            return true;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

namespace Utilities
{

    public class FileLogger
    {
        public const string FILE_NAME_REFERENCE = "WiimoteReferenceData.csv";
        public const string FILE_NAME_PLAY = "WiimoteRecordData.csv";

        protected StreamWriter sw;

        public FileLogger()
        {
        }

        public FileLogger(string filename)
        {
            sw = File.CreateText(filename);
        }

        public void logMessage(string message)
        {
            sw.Write(message);
        }

        public void logEmptyLine()
        {
            sw.WriteLine("");
        }

        public void close()
        {
            sw.Close();
        }

        ~FileLogger()
        {
            if (sw!= null)
                sw.Close();
        }

    }
}

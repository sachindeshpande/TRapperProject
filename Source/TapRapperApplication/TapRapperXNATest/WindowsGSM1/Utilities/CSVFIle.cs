using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;


namespace Utilities
{
    public class CSVFileFormatException : Exception
    {
    }

    public class CSVFileHeader
    {

        Dictionary<string, string> m_HeaderData = new Dictionary<string, string>();

        public void parseHeader(StreamReader readFile, int numberOfHeaderLines)
        {
            try
            {
                string line;
                string[] row;

                int lineCounter = 0;

                while ((line = readFile.ReadLine()) != null)
                {
                    row = line.Split(',');
                    m_HeaderData.Add(row[0], row[1]);

                    lineCounter++;
                    if (lineCounter >= numberOfHeaderLines)
                        break;
                }
            }
            catch (Exception e)
            {                
                throw new CSVFileException(e);
            }
        }

        public string getValue(string key)
        {
            if (!m_HeaderData.ContainsKey(key))
                throw new CSVFileException("Key Value " + key + " not present in csv header");
            return m_HeaderData[key];
        }

    }

    public class CSVFileData
    {
        List<string[]> m_ParsedData = new List<string[]>();

        public void parseData(StreamReader readFile)
        {
            try
            {
                string line;
                string[] row;

                //skip 2 lines
                readFile.ReadLine();
                readFile.ReadLine();

                while ((line = readFile.ReadLine()) != null)
                {
                    row = line.Split(',');
                    m_ParsedData.Add(row);
                }
            }
            catch (Exception e)
            {
                throw new CSVFileException(e);
            }
        }

        public int numberOfLines()
        {
            return m_ParsedData.Count;
        }

        public string[] getRowData(int rowIndex)
        {
            if (rowIndex >= m_ParsedData.Count)
                throw new CSVFileException("Index " + rowIndex + " greater than row length in CSV FIle");
            return m_ParsedData[rowIndex];
        }

    }

    public class CSVFileParser
    {
        StreamReader m_ReadFile;
        public CSVFileHeader csvFileHeader { get; set; }
        public CSVFileData csvFileData { get; set;  }

        public void parseCSV(string path,bool headerOnly,int numberOfHeaderLines)
        {
            try
            {
                using (StreamReader readFile = new StreamReader(path))
                {
                    csvFileHeader = new CSVFileHeader();
                    csvFileHeader.parseHeader(readFile, numberOfHeaderLines);

                    if (!headerOnly)
                    {
                        csvFileData = new CSVFileData();
                        csvFileData.parseData(readFile);
                    }

                }
            }
            catch (CSVFileException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new CSVFileException(e);
                //                    MessageBox.Show(e.Message);
            }
        }

        public void startParsingCSVData(string path, int numberOfHeaderLines, int numberOfLinesToSkip)
        {
            int lNumberOfOpenAttempts = 0;
            while (true)
            {
                try
                {
                    m_ReadFile = new StreamReader(path);
                    csvFileHeader = new CSVFileHeader();
                    if (numberOfHeaderLines > 0)
                        csvFileHeader.parseHeader(m_ReadFile, numberOfHeaderLines);

                    for (int i = 0; i < numberOfLinesToSkip; i++)
                        m_ReadFile.ReadLine();

                    ConsoleLogger.logMessage("lNumberOfOpenAttempts = " + lNumberOfOpenAttempts);
                    break;

                }
                catch (CSVFileException e)
                {
                    lNumberOfOpenAttempts++;
                    System.Threading.Thread.Sleep(1000);
                }
                catch (Exception e)
                {
                    lNumberOfOpenAttempts++;
                    System.Threading.Thread.Sleep(1000);
                    //                    MessageBox.Show(e.Message);
                }
                finally
                {
                    if (lNumberOfOpenAttempts > ProjectCommon.ProjectConstants.NUMBER_OPEN_FILE_ATTEMPTS)
                        throw new CSVFileException("Could not open file " + path);
                }
            }
        }
        public string[] getNextRow(int rowSkipStep)
        {

            try
            {
                string line = this.m_ReadFile.ReadLine();
                for (int i = 0; i < rowSkipStep; i++)
                    this.m_ReadFile.ReadLine();

                if (line == null)
                    return null;

                return line.Split(',');
            }
            catch (Exception e)
            {
                throw new CSVFileException(e);
            }
        }

        public void close()
        {
            if (m_ReadFile != null)
            {
                m_ReadFile.Close();
                m_ReadFile.Dispose();
            }
        }

    }

    public class CSVFileWriter
    {
        protected StreamWriter m_WriteFile;

        public CSVFileWriter()
        {
        }

        public CSVFileWriter(string path)
        {
            m_WriteFile = new StreamWriter(path);
        }

        public void writeLine()
        {
            try
            {
                writeLine("");
            }
            catch (CSVFileException e)
            {
                throw e;
            }
        }

        public void writeLine(string line)
        {
            try
            {
                if (m_WriteFile != null)
                {
                    lock (this)
                    {
                        m_WriteFile.WriteLine(line);
                    }
                }
            }
            catch (Exception e)
            {
                throw new CSVFileException(e);                
            }
        }

        public void writeLine(string []row)
        {
            if (row == null || row.Length == 0)
                throw new CSVFileFormatException();

            try
            {
                StringBuilder line = new StringBuilder();
                line.Append(row[0]);
                for (int i = 1; i < row.Length; i++)
                    line.Append("," + row[i]);
                writeLine(line.ToString());
            }
            catch (CSVFileException e)
            {
                throw e;
            }
        }

        public void close()
        {
            if (m_WriteFile != null)
            {
                m_WriteFile.Close();
                m_WriteFile.Dispose();
                m_WriteFile = null;
            }
        }
    }


}

/*

        public void startParsingCSVData(string path, int numberOfHeaderLines,int numberOfLinesToSkip)
        {
            int lNumberOfOpenAttempts = 0;
            while (true)
            {
                try
                {
                    m_ReadFile = new StreamReader(path);
                    csvFileHeader = new CSVFileHeader();
                    if (numberOfHeaderLines > 0)
                        csvFileHeader.parseHeader(m_ReadFile, numberOfHeaderLines);

                    for (int i = 0; i < numberOfLinesToSkip; i++)
                        m_ReadFile.ReadLine();

                    ConsoleLogger.logMessage("lNumberOfOpenAttempts = " + lNumberOfOpenAttempts);
                    break;

                }
                catch (CSVFileException e)
                {
                    lNumberOfOpenAttempts++;
                    System.Threading.Thread.Sleep(1000);
                }
                catch (Exception e)
                {
                    lNumberOfOpenAttempts++;
                    System.Threading.Thread.Sleep(1000);
                    //                    MessageBox.Show(e.Message);
                }
                finally
                {
                    if (lNumberOfOpenAttempts > ProjectCommon.ProjectConstants.NUMBER_OPEN_FILE_ATTEMPTS)
                        throw new CSVFileException("Could not open file " + path);
                }
            }
        }


         public void startParsingCSVData(string path, int numberOfHeaderLines, int numberOfLinesToSkip)
        {

            try
            {
                m_ReadFile = new StreamReader(path);
                csvFileHeader = new CSVFileHeader();
                if (numberOfHeaderLines > 0)
                    csvFileHeader.parseHeader(m_ReadFile, numberOfHeaderLines);

                for (int i = 0; i < numberOfLinesToSkip; i++)
                    m_ReadFile.ReadLine();

            }
            catch (CSVFileException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new CSVFileException(e);
                //                    MessageBox.Show(e.Message);
            }
        }
*/
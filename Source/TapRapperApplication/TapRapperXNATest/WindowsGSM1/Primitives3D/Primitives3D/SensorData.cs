using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Primitives3D
{
    class SensorData
    {
        public float[,] quarternian;
        public int currentIndex;
        public int numData;
        String InputCSVFile = @"D:\temp\Circle.csv";
        //String InputCSVFile = @"C:\Weidong\My Dropbox\TapRapperShare\3SpaceSensor\EightMovement.csv";
        //String InputCSVFile = @"C:\Weidong\My Dropbox\TapRapperShare\3SpaceSensor\Rotation.csv";
        //String InputCSVFile = @"C:\Weidong\My Dropbox\TapRapperShare\3SpaceSensor\StandstillTestData.csv";
        //String InputCSVFile = @"C:\Weidong\My Dropbox\TapRapperShare\3SpaceSensor\UpDownMovement.csv";

        public SensorData()
        {
            currentIndex = 0;
            numData = 0;
        }

        public void loadCSV()
        {
            string fname = InputCSVFile;

            try
            {
                using (StreamReader sr = new StreamReader(fname))
                {
                    String line;
                    numData = -1;
                    while ((line = sr.ReadLine()) != null)
                    {
                        numData++;
                    }
                    sr.Close();
                }

                quarternian = new float[numData, 4];

                using (StreamReader sr = new StreamReader(fname))
                {
                    String line;
                    int idx = -1;
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (idx > -1)
                        {
                            string[] fields = line.Split(',');
                            for (int i1 = 0; i1 < 4; i1++)
                                quarternian[idx, i1] = float.Parse(fields[i1 + 1]);

                        }
                        idx++;
                    }
                    sr.Close();
                }


            }
            catch (Exception e)
            {
            }
        }
    }
}

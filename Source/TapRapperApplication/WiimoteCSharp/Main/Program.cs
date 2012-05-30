using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using MainGUI;
using SpaceSensor;
using ProjectCommon;
using System.Threading;
using Utilities;
using System.Text;
using Microsoft.Xna.Framework;
using WiimoteData.SpaceSensor;
using WiimoteData;


namespace Main
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            //            try
            {
                bool l_Mogre = false;

                if (!l_Mogre)
                {

                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new Form1());
                }
                else
                {

//                    OgreControl win = new WiimoteOgreControl();
//                    win.Go();
                    //                    win.Initialize();
                    //                    Application.Run(new Form1());
                }
            }
            /*
                        catch (System.Runtime.InteropServices.SEHException)
                        {
                            if (OgreException.IsThrown)
                                MessageBox.Show(OgreException.LastException.FullDescription, "An Ogre exception has occurred!");
                            else
                                throw;
                        }
            */
        }


        //public static void Test3SpaceSensorAccGyro()
        //{
        //    SpaceSensorMain spaceSensorMain = new SpaceSensorMain();
        //    spaceSensorMain.SpaceSensorObject1.Connect();

        //    CSVFileWriter csvFileWriter = new CSVFileWriter(@"D:\temp\TestData.csv");

        //    for (int i = 0; i < 50; i++)
        //    {
        //        SensorData sensorDataObj = spaceSensorMain.SpaceSensorObject1.ReadAccGyroData();
        //        //Quaternion sensor2QuatObj = _spaceSensorMain.SpaceSensorObject2.ReadOrientation();
        //        StringBuilder dataRow = new StringBuilder();

        //        dataRow.Append(String.Format("{0:hh:mm:ss.fff}", DateTime.Now));
        //        dataRow.Append("," + sensorDataObj.AccDataObject.DataVector.X);
        //        dataRow.Append("," + sensorDataObj.AccDataObject.DataVector.Y);
        //        dataRow.Append("," + sensorDataObj.AccDataObject.DataVector.Z);
        //        dataRow.Append("," + sensorDataObj.GyroDataObject.Pitch);
        //        dataRow.Append("," + sensorDataObj.GyroDataObject.Roll);
        //        dataRow.Append("," + sensorDataObj.GyroDataObject.Yaw);

        //        csvFileWriter.writeLine(dataRow.ToString());
        //        Thread.Sleep(100);

        //    }

        //    csvFileWriter.close();
        //}

        public static void Test3SpaceSensor()
        {
            Wiimotes wiimotesObj = Wiimotes.getWiimotesObject();
            wiimotesObj.connectWiimotes(1);
            wiimotesObj.StartDataCollection(@"D:\temp\TestData.csv",true);
            Thread.Sleep(30000);
            wiimotesObj.StopDataCollection();
            Thread.Sleep(3000);
            wiimotesObj.disconnectWiimotes();
            Application.Exit();
        }

        //public static void Test3SpaceSensor()
        //{
        //    SpaceSensorMain spaceSensorMain = new SpaceSensorMain();
        //    spaceSensorMain.Connect();

        //    CSVFileWriter csvFileWriter = new CSVFileWriter(@"D:\temp\TestData.csv");

        //    for(int i = 0 ; i < 50 ; i++)
        //    {
        //        SensorData sensorDataObj = spaceSensorMain.SpaceSensorObject1.ReadData();
        //        //Quaternion sensor2QuatObj = _spaceSensorMain.SpaceSensorObject2.ReadOrientation();
        //        StringBuilder dataRow = new StringBuilder();

        //        dataRow.Append(String.Format("{0:hh:mm:ss.fff}", DateTime.Now));
        //        dataRow.Append("," + sensorDataObj.AccDataObject.DataVector.X);
        //        dataRow.Append("," + sensorDataObj.AccDataObject.DataVector.Y);
        //        dataRow.Append("," + sensorDataObj.AccDataObject.DataVector.Z);
        //        dataRow.Append("," + sensorDataObj.GyroDataObject.Pitch);
        //        dataRow.Append("," + sensorDataObj.GyroDataObject.Roll);
        //        dataRow.Append("," + sensorDataObj.GyroDataObject.Yaw);

        //        dataRow.Append("," + sensorDataObj.QuaternionObject.X);
        //        dataRow.Append("," + sensorDataObj.QuaternionObject.Y);
        //        dataRow.Append("," + sensorDataObj.QuaternionObject.Z);
        //        dataRow.Append("," + sensorDataObj.QuaternionObject.W);

        //        csvFileWriter.writeLine(dataRow.ToString());
        //        Thread.Sleep(10);

        //    }

        //    csvFileWriter.close();
        //}
    }
}

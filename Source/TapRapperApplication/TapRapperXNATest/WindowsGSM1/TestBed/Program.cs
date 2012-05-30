using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace TestBed
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            test();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());

        }

        static void test()
        {
            string filePath = @"D:\Projects\WiimoteProjects\WiiMotionPlus\Version22-WithTUIO\Data\WiimoteReferenceData\Reference6_Try_1.csv";
            string newFilePath = filePath.Insert(filePath.Length - 4, "CSharp");
            return;
        }
    }
}

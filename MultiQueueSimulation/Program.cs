using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using MultiQueueTesting;
using MultiQueueModels;
using static MultiQueueModels.Enums;

namespace MultiQueueSimulation
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            SimulationSystem system = new SimulationSystem();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            string path=null;
            Application.Run(new Form1(path));
            path = Form1.getPath();
           
            
            if (path == null) return;
            if (path == "") return;
            system.Read_file(path);

            if (system.SelectionMethod.ToString() == "HighestPriority")
            {
                system.Fun_P();
            }
            else if (system.SelectionMethod.ToString() == "Random")
            {
                system.RandomMethod();
            }
            else if (system.SelectionMethod.ToString() == "LeastUtilization")
            {
             
            }

            Form1.setSystem(system);
            Application.Run(new Form1(path));
            List<SimulationCase> outs = system.return_data_of_server(2);
            string result = TestingManager.Test(system, Constants.FileNames.TestCase1);

            MessageBox.Show(result);
            
            Console.WriteLine(system.NumberOfServers);


        }



    }
}
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


            string file = @"E:\الترم الاول\Template_Students\MultiQueueSimulation\MultiQueueSimulation\TestCases\TestCase1.txt";
            file = @"D:\Material of Faculity level 4\Semester 1\Modeling & Simulation\Sec pdf\Lab 2_Task1\Template_Students\Template_Students\MultiQueueSimulation\MultiQueueSimulation\TestCases\TestCase1.txt";
            system.Read_file(file);

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
                system.Least_utlization();
            }
            
            List<SimulationCase> outs = system.return_data_of_server(2);
            string result = TestingManager.Test(system, Constants.FileNames.TestCase1);

            MessageBox.Show(result);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1(system.SimulationTable));
            Console.WriteLine(system.NumberOfServers);


        }



    }
}
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using MultiQueueTesting;
using MultiQueueModels;
using static MultiQueueModels.Enums;
using System.Security.Cryptography.X509Certificates;

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
            //string path = @"D:\Material of Faculity level 4\Semester 1\Modeling & Simulation\Sec pdf\Lab 2_Task1\Template_Students\Template_Students\MultiQueueSimulation\MultiQueueSimulation\TestCases\TestCase3.txt";

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
                system.Least_utlization();
            }

            Console.WriteLine("system.ttc");
            Console.WriteLine(system.ttc);
            try
            {
                int summi = 0;
                for(int i = 0;i< system.SimulationTable.Count();i++)
                {
                    summi += system.SimulationTable[i].TimeInQueue;
                }
                system.PerformanceMeasures.AverageWaitingTime = ((decimal)summi / (decimal)system.SimulationTable.Count());
            system.PerformanceMeasures.WaitingProbability = ((decimal)system.ctr / (decimal)system.SimulationTable.Count());
            }
            catch (Exception ex)
            {
                // handle exception here
                Console.Write("Could not divide any number by 0");
            }
            //performance for each Server
            Console.WriteLine("Total simulation Time = " + system.totalSimulationTime);
            for (int b = 0; b< system.NumberOfServers;b++)
            {
                if (system.Servers[b].customersPerServer != 0)
                {
                    system.Servers[b].AverageServiceTime = ((decimal)system.Servers[b].twt / (decimal)system.Servers[b].customersPerServer);
                }
                else
                    system.Servers[b].AverageServiceTime = 0;
                int maxi = system.SimulationTable.Max((SimulationCase e) => e.EndTime);
                system.Servers[b].idle = maxi - system.Servers[b].twt;
                system.Servers[b].IdleProbability =((decimal) system.Servers[b].idle / (decimal)maxi);
                system.Servers[b].Utilization = ((decimal)system.Servers[b].twt / (decimal)maxi);
                Console.WriteLine("Total idle Time of server "+b+" is "+ system.Servers[b].idle);
                //Console.WriteLine("      ");
                //Console.WriteLine("Averag
                //eServiceTime od server "+b+" is " + system.Servers[b].AverageServiceTime);
            }
             system.max_q();
            //system.idelprobab();
            Form1.setSystem(system);
            Application.Run(new Form1(path));
           
            string result = TestingManager.Test(system, Constants.FileNames.TestCase1);

            MessageBox.Show(result);
            
            Console.WriteLine(system.NumberOfServers);





            

        }
        
       

    }
}
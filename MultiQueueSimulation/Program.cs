using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using MultiQueueTesting;
using MultiQueueModels;
using System.IO;

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
            system = readInputFromFile("E:\\الترم الاول\\Template_Students\\MultiQueueSimulation\\MultiQueueSimulation\\TestCases\\TestCase1.txt",system);


            string result = TestingManager.Test(system, Constants.FileNames.TestCase1);
            MessageBox.Show(result);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
            Console.WriteLine(system.NumberOfServers);


        }
        static SimulationSystem readInputFromFile(String path, SimulationSystem system)
        {
            String filePath = path;
            String[] inputs = File.ReadAllLines(filePath);
            Dictionary<string, string> variables = new Dictionary<string, string>();
            for (int i = 0; i < inputs.Length; i++)
            {
                if (inputs[i] == "NumberOfServers")
                    system.NumberOfServers = int.Parse(inputs[i + 1]);
                if (inputs[i] == "StoppingNumber")
                    system.StoppingNumber = int.Parse(inputs[i + 1]);
                if (inputs[i] == "StoppingCriteria")
                {
                    if (int.Parse(inputs[i + 1]) == 1)
                        system.StoppingCriteria = Enums.StoppingCriteria.NumberOfCustomers;
                    else
                        system.StoppingCriteria = Enums.StoppingCriteria.SimulationEndTime;
                }
                if (inputs[i] == "SelectionMethod")
                {
                    if (int.Parse(inputs[i + 1]) == 1)
                        system.SelectionMethod = Enums.SelectionMethod.HighestPriority;
                    else if (int.Parse(inputs[i + 1]) == 2)
                        system.SelectionMethod = Enums.SelectionMethod.Random;
                    else
                        system.SelectionMethod = Enums.SelectionMethod.LeastUtilization;
                }
                if (inputs[i] == "InterarrivalDistribution")
                {


                    for (int j = i + 1; j < inputs.Length; j++)
                    {
                        if (inputs[j] != "")
                        {
                            string[] values = inputs[j].Split(',');
                            TimeDistribution interarrivalDistribution = new TimeDistribution();

                            // Assign values to local variables
                            int time;
                            decimal probability;
                            int.TryParse(values[0].Trim(), out time);
                            decimal.TryParse(values[1].Trim(), out probability);

                            // Set the property values from the local variables
                            interarrivalDistribution.Time = time;
                            interarrivalDistribution.Probability = probability;

                            system.InterarrivalDistribution.Add(interarrivalDistribution);
                        }
                        else
                            break;
                    }

                }
                if (inputs[i].Contains("ServiceDistribution_Server"))
                {


                    for (int j = i + 1; j < inputs.Length; j++)
                    {
                        if (inputs[j] != "")
                        {
                            string[] values = inputs[j].Split(',');

                            TimeDistribution serverDistribution = new TimeDistribution();

                            // Assign values to local variables
                            int time;
                            decimal probability;
                            int.TryParse(values[0].Trim(), out time);
                            decimal.TryParse(values[1].Trim(), out probability);

                            // Set the property values from the local variables
                            serverDistribution.Time = time;
                            serverDistribution.Probability = probability;
                            Server server = new Server();
                            server.TimeDistribution.Add(serverDistribution);
                            system.Servers.Add(server);
                        }
                        else
                            break;
                    }

                }

            }
            return system;
        }
    }
}

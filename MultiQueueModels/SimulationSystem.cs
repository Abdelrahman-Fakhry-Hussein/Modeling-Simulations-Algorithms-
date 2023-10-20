using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MultiQueueModels.Enums;

namespace MultiQueueModels
{
    public class SimulationSystem
    {
        public SimulationSystem()
        {
            this.Servers = new List<Server>();
            this.InterarrivalDistribution = new List<TimeDistribution>();
            this.PerformanceMeasures = new PerformanceMeasures();
            this.SimulationTable = new List<SimulationCase>();


        }

        ///////////// INPUTS ///////////// 
        
        public int NumberOfServers { get; set; }
        public int StoppingNumber { get; set; }
        public List<Server> Servers { get; set; }
        public List<TimeDistribution> InterarrivalDistribution { get; set; }
        public Enums.StoppingCriteria StoppingCriteria { get; set; }
        public Enums.SelectionMethod SelectionMethod { get; set; }

        ///////////// OUTPUTS /////////////
        public List<SimulationCase> SimulationTable { get; set; }
        public PerformanceMeasures PerformanceMeasures { get; set; }

        public void Read_file(string FP)
        {


            int ia = -1;
           
            decimal val33 = 0;
            int down_int=0,upper_int = 0;

            List<int> arr = new List<int>();

            List<TimeDistribution> TDS = new List<TimeDistribution>();

            try
            {
                using (StreamReader reader = new StreamReader(FP))
                {
                    string line;
                    string currentKey = null;

                    while ((line = reader.ReadLine()) != null)
                    {
                        if (string.IsNullOrWhiteSpace(line))
                            continue;

                        if (line == "NumberOfServers")
                        {
                            currentKey = "NumberOfServers";
                        }
                        else if (line == "StoppingNumber")
                        {
                            currentKey = "StoppingNumber";
                        }
                        else if (line == "StoppingCriteria")
                        {
                            currentKey = "StoppingCriteria";
                        }
                        else if (line == "SelectionMethod")
                        {
                            currentKey = "SelectionMethod";
                        }
                        else if (line == "InterarrivalDistribution")
                        {
                            currentKey = "InterarrivalDistribution";
                        }
                        else if (line.StartsWith("ServiceDistribution"))
                        {
                            currentKey = "ServiceDistribution";

                             Servers.Add(new Server());
                            
                            ia++;
                            val33 = 0;
                            down_int = 0;
                            upper_int = 0;
                        }
                        else
                        {
                            int value;
                            if (currentKey == "NumberOfServers" && int.TryParse(line, out value))
                            {
                                NumberOfServers = value;
                            }
                            else if (currentKey == "StoppingNumber" && int.TryParse(line, out value))
                            {
                                StoppingNumber = value;
                            }
                            else if (currentKey == "StoppingCriteria" && int.TryParse(line, out value))
                            {

                                if (value == 1)
                                {
                                    StoppingCriteria = Enums.StoppingCriteria.NumberOfCustomers;

                                }
                                else
                                {
                                    StoppingCriteria = Enums.StoppingCriteria.SimulationEndTime;
                                }

                            }
                            else if (currentKey == "SelectionMethod" && int.TryParse(line, out value))
                            {

                                if (value == 1)
                                {
                                    SelectionMethod = Enums.SelectionMethod.HighestPriority;

                                }
                                else if (value == 2)
                                {
                                    SelectionMethod = Enums.SelectionMethod.Random;
                                }
                                else if (value == 3)
                                {
                                    SelectionMethod = Enums.SelectionMethod.LeastUtilization;
                                }
                            }
                            else if (currentKey == "InterarrivalDistribution")
                            {


                                string[] parts = line.Split(',');

                                if (parts.Length == 2 && int.TryParse(parts[0], out int va111) && decimal.TryParse(parts[1], out decimal va122))
                                {

                                    val33 += va122;
                                    down_int = (upper_int + 1);
                                    upper_int = (int)(val33 * 100);


                                    InterarrivalDistribution.Add(new TimeDistribution { Time = va111, Probability = va122, CummProbability = val33, MinRange = down_int, MaxRange = upper_int });
                                }


                            }
                            else if (currentKey == "ServiceDistribution")
                            {


                                string[] parts = line.Split(',');

                                if (parts.Length == 2 && int.TryParse(parts[0], out int va111) && decimal.TryParse(parts[1], out decimal va122))
                                {

                                    val33 += va122;
                                    down_int = (upper_int + 1);
                                    upper_int = (int)(val33 * 100);
                                    Servers[ia].TimeDistribution.Add(new TimeDistribution { Time = va111, Probability = va122, CummProbability = val33, MinRange = down_int, MaxRange = upper_int });
                                    

                                    //Servers.Add( TimeDistribution.Add(new TimeDistribution { Time = va111, Probability = va122, CummProbability = val33, MinRange = down_int, MaxRange = upper_int }));
                                }


                            }
                        }






                    }
                }

                
            }
            catch { }

        }


        }
}

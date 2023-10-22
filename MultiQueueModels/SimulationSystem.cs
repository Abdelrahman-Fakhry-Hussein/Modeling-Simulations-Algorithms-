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
            int down_int = 0, upper_int = 0;

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
                                    Servers[ia].FinishTime = 0;
                                    Servers[ia].ID = (ia + 1);
                                    //Servers.Add( TimeDistribution.Add(new TimeDistribution { Time = va111, Probability = va122, CummProbability = val33, MinRange = down_int, MaxRange = upper_int }));
                                }


                            }
                        }
                    }
                }

            }
            catch
            {

            }
        }

        public List<SimulationCase> return_data_of_server(int server_number)
        {
            List<SimulationCase> SimulationTables = new List<SimulationCase>();
            foreach (var SimulationCase in SimulationTable)
            {
                if (SimulationCase.AssignedServer.ID == server_number)
                {
                    SimulationTables.Add(SimulationCase);
                }
            }
            return SimulationTables;
        }

        public void Fun_P()
        {
            Random randoms = new Random();

            // Generate a random number within the specified range


            if (StoppingCriteria.ToString() == "NumberOfCustomers")
            {
                Console.WriteLine(StoppingCriteria);
                for (int i = 1; i <= StoppingNumber; i++)
                {
                    int RandomServices = randoms.Next(1, 101);

                    int Randomfor_Intrearival = randoms.Next(1, 101);
                    Console.WriteLine(Randomfor_Intrearival);

                   
                    if (i == 1)
                    {
                        TimeDistribution matchingDistribution = null;
                        foreach (var timeDistribution in Servers[0].TimeDistribution)
                        {
                            if (RandomServices >= timeDistribution.MinRange && RandomServices <= timeDistribution.MaxRange)
                            {
                                matchingDistribution = timeDistribution;
                                break; // Exit the loop when a match is found
                            }
                        }
                        SimulationTable.Add(new SimulationCase
                        {
                            AssignedServer = Servers[0],
                            CustomerNumber = i,
                            RandomInterArrival = Randomfor_Intrearival,
                            ArrivalTime = 0,
                            InterArrival = 0,
                            RandomService = RandomServices,
                            StartTime = 0,
                            ServiceTime = matchingDistribution.Time,
                            EndTime = matchingDistribution.Time,
                            TimeInQueue = 0
                        });
                        Servers[0].FinishTime = matchingDistribution.Time;
                    }
                    else
                    {
                        TimeDistribution matchingDistribution_for_arrival = null;
                        foreach (var timeDistribution in InterarrivalDistribution)
                        {
                            if (Randomfor_Intrearival >= timeDistribution.MinRange && Randomfor_Intrearival <= timeDistribution.MaxRange)
                            {
                                matchingDistribution_for_arrival = timeDistribution;
                                break; // Exit the loop when a match is found
                            }
                        }
                        bool temp = false;
                        int ser = 0, ArrivalTimes = (matchingDistribution_for_arrival.Time + SimulationTable[i - 2].ArrivalTime), min_wait = int.MaxValue, server_if_not = -1;
                        foreach (var se in Servers)
                        {
                            if (ArrivalTimes < se.FinishTime)
                            {
                                if (se.FinishTime - ArrivalTimes < min_wait)
                                {

                                    min_wait = se.FinishTime - ArrivalTimes;
                                    server_if_not = ser;
                                }
                                temp = false;
                                ser++;

                            }
                            else
                            {
                                temp = true;
                                break; // Exit the loop when a match is found        
                            }
                        }


                        if (temp == true)
                        {
                            TimeDistribution matchingDistribution = null;
                            foreach (var timeDistribution in Servers[ser].TimeDistribution)
                            {
                                if (RandomServices >= timeDistribution.MinRange && RandomServices <= timeDistribution.MaxRange)
                                {
                                    matchingDistribution = timeDistribution;
                                    break; // Exit the loop when a match is found
                                }
                            }

                            SimulationTable.Add(new SimulationCase
                            {
                                AssignedServer = Servers[ser],
                                CustomerNumber = i,
                                RandomInterArrival = Randomfor_Intrearival,
                                ArrivalTime = ArrivalTimes,
                                InterArrival = matchingDistribution_for_arrival.Time,
                                RandomService = RandomServices,
                                StartTime = ArrivalTimes,
                                ServiceTime = matchingDistribution.Time,
                                EndTime = ArrivalTimes + matchingDistribution.Time,
                                TimeInQueue = 0
                            });
                            Servers[ser].FinishTime = ArrivalTimes + matchingDistribution.Time;



                        }
                        else
                        {
                            TimeDistribution matchingDistribution = null;
                            foreach (var timeDistribution in Servers[server_if_not].TimeDistribution)
                            {
                                if (RandomServices >= timeDistribution.MinRange && RandomServices <= timeDistribution.MaxRange)
                                {
                                    matchingDistribution = timeDistribution;
                                    break; // Exit the loop when a match is found
                                }
                            }
                            SimulationTable.Add(new SimulationCase
                            {
                                AssignedServer = Servers[server_if_not],
                                CustomerNumber = i,
                                RandomInterArrival = Randomfor_Intrearival,
                                ArrivalTime = ArrivalTimes,
                                InterArrival = matchingDistribution_for_arrival.Time,
                                RandomService = RandomServices,
                                StartTime = Servers[server_if_not].FinishTime,
                                ServiceTime = matchingDistribution.Time,
                                EndTime = Servers[server_if_not].FinishTime + matchingDistribution.Time,
                                TimeInQueue = Servers[server_if_not].FinishTime - ArrivalTimes
                            });
                            Servers[server_if_not].FinishTime += matchingDistribution.Time;
                        }



                    }
                }

            }
            else
            {
                int i = 1;
                while (true)
                {
                    int RandomServices = randoms.Next(1, 101);
                    int Randomfor_Intrearival = randoms.Next(1, 101);

                   
                    if (i == 1)
                    {
                        TimeDistribution matchingDistribution = null;
                        foreach (var timeDistribution in Servers[0].TimeDistribution)
                        {
                            if (RandomServices >= timeDistribution.MinRange && RandomServices <= timeDistribution.MaxRange)
                            {
                                matchingDistribution = timeDistribution;
                                break; // Exit the loop when a match is found
                            }
                        }
                        SimulationTable.Add(new SimulationCase
                        {
                            AssignedServer = Servers[0],
                            CustomerNumber = i,
                            RandomInterArrival = Randomfor_Intrearival,
                            ArrivalTime = 0,
                            InterArrival = 0,
                            RandomService = RandomServices,
                            StartTime = 0,
                            ServiceTime = matchingDistribution.Time,
                            EndTime = matchingDistribution.Time,
                            TimeInQueue = 0
                        });
                        Servers[0].FinishTime = matchingDistribution.Time;
                    }
                    else
                    {
                        TimeDistribution matchingDistribution_for_arrival = null;
                        foreach (var timeDistribution in InterarrivalDistribution)
                        {
                            if (Randomfor_Intrearival >= timeDistribution.MinRange && Randomfor_Intrearival <= timeDistribution.MaxRange)
                            {
                                matchingDistribution_for_arrival = timeDistribution;
                                break; // Exit the loop when a match is found
                            }
                        }
                        Console.WriteLine(matchingDistribution_for_arrival.Time);
                        bool temp = false;
                        int ser = 0, ArrivalTimes = (matchingDistribution_for_arrival.Time + SimulationTable[i - 2].ArrivalTime), min_wait = int.MaxValue, server_if_not = -1;
                        foreach (var se in Servers)
                        {
                            if (ArrivalTimes < se.FinishTime)
                            {
                                if (se.FinishTime - ArrivalTimes < min_wait)
                                {

                                    min_wait = se.FinishTime - ArrivalTimes;
                                    server_if_not = ser;
                                }
                                temp = false;
                                ser++;

                            }
                            else
                            {
                                temp = true;
                                break; // Exit the loop when a match is found        
                            }
                        }


                        if (temp == true)
                        {
                            TimeDistribution matchingDistribution = null;
                            foreach (var timeDistribution in Servers[ser].TimeDistribution)
                            {
                                if (RandomServices >= timeDistribution.MinRange && RandomServices <= timeDistribution.MaxRange)
                                {
                                    matchingDistribution = timeDistribution;
                                    break; // Exit the loop when a match is found
                                }
                            }


                            SimulationTable.Add(new SimulationCase
                            {
                                AssignedServer = Servers[ser],
                                CustomerNumber = i,
                                RandomInterArrival = Randomfor_Intrearival,
                                ArrivalTime = ArrivalTimes,
                                InterArrival = matchingDistribution_for_arrival.Time,
                                RandomService = RandomServices,
                                StartTime = ArrivalTimes,
                                ServiceTime = matchingDistribution.Time,
                                EndTime = ArrivalTimes + matchingDistribution.Time,
                                TimeInQueue = 0
                            });
                            Servers[ser].FinishTime = ArrivalTimes + matchingDistribution.Time;



                        }
                        else
                        {
                            TimeDistribution matchingDistribution = null;
                            foreach (var timeDistribution in Servers[server_if_not].TimeDistribution)
                            {
                                if (RandomServices >= timeDistribution.MinRange && RandomServices <= timeDistribution.MaxRange)
                                {
                                    matchingDistribution = timeDistribution;
                                    break; // Exit the loop when a match is found
                                }
                            }

                            SimulationTable.Add(new SimulationCase
                            {
                                AssignedServer = Servers[server_if_not],
                                CustomerNumber = i,
                                RandomInterArrival = Randomfor_Intrearival,
                                ArrivalTime = ArrivalTimes,
                                InterArrival = matchingDistribution_for_arrival.Time,
                                RandomService = RandomServices,
                                StartTime = Servers[server_if_not].FinishTime,
                                ServiceTime = matchingDistribution.Time,
                                EndTime = Servers[server_if_not].FinishTime + matchingDistribution.Time,
                                TimeInQueue = Servers[server_if_not].FinishTime - ArrivalTimes
                            });
                            Servers[server_if_not].FinishTime += matchingDistribution.Time;
                        }



                    }
                    if (SimulationTable[SimulationTable.Count - 1].EndTime >= StoppingNumber)
                    {
                        break;
                    }
                    i++;

                }

            }


        }

        public void RandomMethod()
        {


            if (StoppingCriteria.ToString() == "NumberOfCustomers")
            {
                for (int i = 1; i <= StoppingNumber; i++)
                {

                    fillSimulationTable(i);
                }
            }
            else
            {
                int i = 1;
                while (true)
                {
                    fillSimulationTable(i);
                    if (SimulationTable[SimulationTable.Count - 1].EndTime >= StoppingNumber)
                    {
                        break;
                    }
                    i++;
                }
            }
        }
        public void fillSimulationTable(int i)
        {
            Random randoms = new Random();
           
                SimulationCase simulationcase = new SimulationCase();
                int RandomServices = randoms.Next(1, 101);

                int Randomfor_Intrearival = randoms.Next(1, 101);

                if (i == 1)
                {
                    TimeDistribution Servicetime = null;
                    foreach (var timeDistribution in Servers[0].TimeDistribution)
                    {
                        if (RandomServices >= timeDistribution.MinRange && RandomServices <= timeDistribution.MaxRange)
                        {
                            Servicetime = timeDistribution;
                            break; // Exit the loop when a match is found
                        }
                    }

                    simulationcase.RandomInterArrival = Randomfor_Intrearival;//random interarrival
                    simulationcase.RandomService = RandomServices;//random service
                    simulationcase.ArrivalTime = 0;
                    simulationcase.CustomerNumber = i;
                    simulationcase.TimeInQueue = 0;
                    simulationcase.StartTime = 0;
                    simulationcase.ServiceTime = Servicetime.Time;
                    simulationcase.EndTime = Servicetime.Time;
                    simulationcase.AssignedServer = Servers[0];
                    simulationcase.InterArrival = 0;
                    SimulationTable.Add(simulationcase);
                    Servers[0].FinishTime = Servicetime.Time;

                }
                else
                {

                    TimeDistribution matchingDistribution_for_arrival = null;
                    foreach (var timeDistribution in InterarrivalDistribution)
                    {
                        if (Randomfor_Intrearival >= timeDistribution.MinRange && Randomfor_Intrearival <= timeDistribution.MaxRange)
                        {
                            matchingDistribution_for_arrival = timeDistribution;

                            break; // Exit the loop when a match is found
                        }

                    }
                    //Console.WriteLine(SimulationTable[i - 2].ArrivalTime);
                    // bool IsCustomerWaiting = false;

                    int ClockArrivalTimes = (matchingDistribution_for_arrival.Time + SimulationTable[i - 2].ArrivalTime);
                    int min_wait = int.MaxValue;
                    int server_if_not = -1;
                  for (int server = 0; server < Servers.Count; server++)
                    {
                        TimeDistribution Servicetime = null;

                        if (ClockArrivalTimes < Servers[server].FinishTime)
                        {
                            if (Servers[server].FinishTime - ClockArrivalTimes < min_wait)
                            {

                                min_wait = Servers[server].FinishTime - ClockArrivalTimes;
                                server_if_not = server;
                            }
                          
                            foreach (var timeDistribution in Servers[server_if_not].TimeDistribution)
                            {
                                if (RandomServices >= timeDistribution.MinRange && RandomServices <= timeDistribution.MaxRange)
                                {
                                    Servicetime = timeDistribution;
                                    break; // Exit the loop when a match is found
                                }
                            }
                            simulationcase.RandomInterArrival = Randomfor_Intrearival;//random interarrival
                            simulationcase.RandomService = RandomServices;//random service
                            simulationcase.ArrivalTime = ClockArrivalTimes;
                            simulationcase.CustomerNumber = i;
                            simulationcase.TimeInQueue = Servers[server_if_not].FinishTime - ClockArrivalTimes;
                            simulationcase.StartTime = Servers[server_if_not].FinishTime;
                            simulationcase.ServiceTime = Servicetime.Time;
                            simulationcase.EndTime = Servers[server_if_not].FinishTime + Servicetime.Time;
                            simulationcase.AssignedServer = Servers[server_if_not];
                            simulationcase.InterArrival = matchingDistribution_for_arrival.Time;

                            SimulationTable.Add(simulationcase);
                            Servers[server_if_not].FinishTime += Servicetime.Time;
                        }
                        else
                        {
                            int randomServerIndex = randoms.Next(0, Servers.Count);
                            var selectedServer = Servers[randomServerIndex];
                            foreach (var timeDistribution in selectedServer.TimeDistribution)
                            {
                                if (RandomServices >= timeDistribution.MinRange && RandomServices <= timeDistribution.MaxRange)
                                {
                                    Servicetime = timeDistribution;
                                    break; // Exit the loop when a match is found
                                }
                            }
                            simulationcase.RandomInterArrival = Randomfor_Intrearival;//random interarrival
                            simulationcase.RandomService = RandomServices;//random service
                            simulationcase.ArrivalTime = ClockArrivalTimes;
                            simulationcase.CustomerNumber = i;
                            simulationcase.TimeInQueue = 0;
                            simulationcase.StartTime = ClockArrivalTimes;
                            simulationcase.ServiceTime = Servicetime.Time;
                            simulationcase.EndTime = ClockArrivalTimes + Servicetime.Time;
                            simulationcase.AssignedServer = selectedServer;
                            simulationcase.InterArrival = matchingDistribution_for_arrival.Time;
                            SimulationTable.Add(simulationcase);
                            Servers[randomServerIndex].FinishTime = ClockArrivalTimes + Servicetime.Time;

                            break; // Exit the loop when a match is found        
                        }
                    }

                }

            }
        }
       

        
    }
            
        
                   

    
                

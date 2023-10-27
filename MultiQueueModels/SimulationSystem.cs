using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MultiQueueModels.Enums;
using System.Runtime.ConstrainedExecution;

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
        public decimal totalSimulationTime = 0;
        public decimal waitingSum = 0; // total time customer waited
        public decimal ctr = 0; //number of customer who waited
        public decimal ttc = 0; //total customer 
        public decimal t = 0; //once waiting time temp
        public int servingTime = 0; // work time for each server

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

        public void idelprobab()
        {
            int maxi = SimulationTable.Max((SimulationCase e) => e.EndTime);

            Console.WriteLine("Maxiiiiiiiiiii");
            Console.WriteLine(maxi);
            for (int i =0;i<NumberOfServers;i++)
            {
                List<SimulationCase> SimulationTableas = return_data_of_server(i);
                decimal sums = 0;
                for(int j = 1;j<SimulationTableas.Count();j++)
                {
                    sums += SimulationTableas[j].ServiceTime;
                }
                Servers[i].IdleProbability = ((decimal)(maxi - sums) / (decimal)maxi);
                Console.WriteLine(Servers[i].IdleProbability);

            }
           
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
                        if (i == StoppingNumber)
                        {
                            totalSimulationTime = matchingDistribution.Time;
                        }
                        Servers[0].FinishTime = matchingDistribution.Time;
                        ttc++;
                        servingTime = matchingDistribution.Time;
                        Servers[0].twt += servingTime;
                        Servers[0].customersPerServer++;
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
                            if (i == StoppingNumber)
                            {
                                totalSimulationTime = ArrivalTimes + matchingDistribution.Time;
                            }
                            Servers[ser].FinishTime = ArrivalTimes + matchingDistribution.Time;
                            ttc++;
                            servingTime = matchingDistribution.Time;
                            Servers[ser].twt += servingTime;
                            Servers[ser].customersPerServer++;
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
                            if (i == StoppingNumber)
                            {
                                totalSimulationTime = Servers[server_if_not].FinishTime + matchingDistribution.Time;
                            }
                            t = Servers[server_if_not].FinishTime - (ArrivalTimes);
                            Servers[server_if_not].FinishTime += matchingDistribution.Time;
                            servingTime = matchingDistribution.Time;
                            Servers[server_if_not].twt += servingTime;
                            Servers[server_if_not].customersPerServer++;
                            // Console.WriteLine("waiting time of customer "+ i+"is "+ t);
                            waitingSum += t;
                            t = 0;
                            ctr++;
                            ttc++;

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
                        if (i == StoppingNumber)
                        {
                            totalSimulationTime = matchingDistribution.Time;
                        }
                        Servers[0].FinishTime = matchingDistribution.Time;
                        ttc++;
                        servingTime = matchingDistribution.Time;
                        Servers[0].twt += servingTime;
                        Servers[0].customersPerServer++;
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
                            if (i == StoppingNumber)
                            {
                                totalSimulationTime = ArrivalTimes + matchingDistribution.Time;
                            }
                            Servers[ser].FinishTime = ArrivalTimes + matchingDistribution.Time;

                            ttc++;
                            servingTime = matchingDistribution.Time;
                            Servers[0].twt += servingTime;
                            Servers[0].customersPerServer++;
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
                            if (i == StoppingNumber)
                            {
                                totalSimulationTime = Servers[server_if_not].FinishTime + matchingDistribution.Time;
                            }
                            t = Servers[server_if_not].FinishTime - (ArrivalTimes);
                            Servers[server_if_not].FinishTime += matchingDistribution.Time;
                            Console.WriteLine("waiting time of customer " + i + "is " + t);
                            servingTime = matchingDistribution.Time;
                            Servers[0].twt += servingTime;
                            Servers[0].customersPerServer++;
                            waitingSum += t;
                            t = 0;
                            ctr++;
                            ttc++;

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




        public void Least_utlization()
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
                        if (i == StoppingNumber)
                        {
                            totalSimulationTime = matchingDistribution.Time;
                        }
                        Servers[0].FinishTime = matchingDistribution.Time;
                        Servers[0].TotalWorkingTime += matchingDistribution.Time;
                        servingTime = matchingDistribution.Time;
                        Servers[0].twt += servingTime;
                        Servers[0].customersPerServer++;

                        ttc++;
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
                        int minutili = int.MaxValue, serv_if_least = -1;
                        foreach (var se in Servers)
                        {

                            if (ArrivalTimes < se.FinishTime)
                            {
                                if (se.FinishTime - ArrivalTimes < min_wait)
                                {

                                    min_wait = se.FinishTime - ArrivalTimes;
                                    server_if_not = ser;
                                }
                                // temp = false;


                            }
                            else
                            {
                                if (minutili > se.TotalWorkingTime)
                                {
                                    minutili = se.TotalWorkingTime;
                                    serv_if_least = ser;

                                }
                                temp = true;

                            }
                            ser++;
                        }


                        if (temp == true)
                        {
                            TimeDistribution matchingDistribution = null;
                            foreach (var timeDistribution in Servers[serv_if_least].TimeDistribution)
                            {
                                if (RandomServices >= timeDistribution.MinRange && RandomServices <= timeDistribution.MaxRange)
                                {
                                    matchingDistribution = timeDistribution;
                                    break; // Exit the loop when a match is found
                                }
                            }

                            SimulationTable.Add(new SimulationCase
                            {
                                AssignedServer = Servers[serv_if_least],
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
                            if (i == StoppingNumber)
                            {
                                totalSimulationTime = matchingDistribution.Time;
                            }
                            Servers[serv_if_least].FinishTime = ArrivalTimes + matchingDistribution.Time;
                            Servers[serv_if_least].TotalWorkingTime += matchingDistribution.Time;
                            servingTime = matchingDistribution.Time;
                            Servers[server_if_not].twt += servingTime;
                            Servers[server_if_not].customersPerServer++;
                            ttc++;

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
                            if (i == StoppingNumber)
                            {
                                totalSimulationTime = matchingDistribution.Time;
                            }
                            Servers[server_if_not].FinishTime += matchingDistribution.Time;
                            Servers[server_if_not].TotalWorkingTime += matchingDistribution.Time;
                            servingTime = matchingDistribution.Time;
                            Servers[server_if_not].twt += servingTime;
                            Servers[server_if_not].customersPerServer++;
                            t = Servers[server_if_not].FinishTime - (ArrivalTimes); //time in queue

                            // Console.WriteLine("waiting time of customer "+ i+"is "+ t);
                            waitingSum += t;
                            t = 0;
                            ctr++;
                            ttc++;
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
                    Console.WriteLine(Randomfor_Intrearival);


                    if (i == 1)
                    {
                        ttc++;
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
                        if (i == StoppingNumber)
                        {
                            totalSimulationTime = matchingDistribution.Time;
                        }
                        Servers[0].FinishTime = matchingDistribution.Time;
                        Servers[0].TotalWorkingTime += matchingDistribution.Time;
                        servingTime = matchingDistribution.Time;
                        Servers[0].twt += servingTime;
                        Servers[0].customersPerServer++;
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
                        int minutili = int.MaxValue, serv_if_least = -1;
                        foreach (var se in Servers)
                        {

                            if (ArrivalTimes < se.FinishTime)
                            {
                                if (se.FinishTime - ArrivalTimes < min_wait)
                                {

                                    min_wait = se.FinishTime - ArrivalTimes;
                                    server_if_not = ser;
                                }
                                // temp = false;


                            }
                            else
                            {
                                if (minutili > se.TotalWorkingTime)
                                {
                                    minutili = se.TotalWorkingTime;
                                    serv_if_least = ser;

                                }
                                temp = true;

                            }
                            ser++;
                        }


                        if (temp == true)
                        {
                            TimeDistribution matchingDistribution = null;
                            foreach (var timeDistribution in Servers[serv_if_least].TimeDistribution)
                            {
                                if (RandomServices >= timeDistribution.MinRange && RandomServices <= timeDistribution.MaxRange)
                                {
                                    matchingDistribution = timeDistribution;
                                    break; // Exit the loop when a match is found
                                }
                            }

                            SimulationTable.Add(new SimulationCase
                            {
                                AssignedServer = Servers[serv_if_least],
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
                            if (i == StoppingNumber)
                            {
                                totalSimulationTime = matchingDistribution.Time;
                            }
                            Servers[serv_if_least].FinishTime = ArrivalTimes + matchingDistribution.Time;
                            Servers[serv_if_least].TotalWorkingTime += matchingDistribution.Time;
                            servingTime = matchingDistribution.Time;
                            Servers[serv_if_least].twt += servingTime;
                            Servers[serv_if_least].customersPerServer++;
                            ttc++;

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
                            if (i == StoppingNumber)
                            {
                                totalSimulationTime = matchingDistribution.Time;
                            }
                            Servers[server_if_not].FinishTime += matchingDistribution.Time;
                            Servers[server_if_not].TotalWorkingTime += matchingDistribution.Time;
                            servingTime = matchingDistribution.Time;
                            Servers[server_if_not].twt += servingTime;
                            Servers[server_if_not].customersPerServer++;
                            t = Servers[server_if_not].FinishTime - (ArrivalTimes); // time in queue

                            // Console.WriteLine("waiting time of customer "+ i+"is "+ t);
                            waitingSum += t;
                            t = 0;
                            ctr++;
                            ttc++;
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
            Random randoms = new Random();

            if (StoppingCriteria.ToString() == "NumberOfCustomers")
            {

                for (int i = 1; i <= StoppingNumber; i++)
                {










                    int RandomServices = randoms.Next(1, 101);
                    int Randomfor_Intrearival = randoms.Next(1, 101);


                    fillSimulationTableForRandomMethod(i, RandomServices, Randomfor_Intrearival, randoms);
                }
            }
            else
            {
                int i = 1;
                while (true)
                {

                    int RandomServices = randoms.Next(1, 101);
                    int Randomfor_Intrearival = randoms.Next(1, 101);


                    fillSimulationTableForRandomMethod(i, RandomServices, Randomfor_Intrearival, randoms);

                    if (SimulationTable[SimulationTable.Count - 1].EndTime >= StoppingNumber)
                    {
                        break;
                    }
                    i++;
                }
            }
        }


        public void fillSimulationTableForRandomMethod(int i, int RandomServices, int Randomfor_Intrearival, Random randoms)
        {
            if (i == 1)
            {
                int serv = randoms.Next(0, NumberOfServers);
                TimeDistribution matchingDistribution = null;
                foreach (var timeDistribution in Servers[serv].TimeDistribution)
                {
                    if (RandomServices >= timeDistribution.MinRange && RandomServices <= timeDistribution.MaxRange)
                    {
                        matchingDistribution = timeDistribution;
                        break; // Exit the loop when a match is found
                    }
                }
                SimulationTable.Add(new SimulationCase
                {
                    AssignedServer = Servers[serv],
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
                if (i == StoppingNumber)
                {
                    totalSimulationTime = matchingDistribution.Time;
                }
                Servers[serv].FinishTime = matchingDistribution.Time;
                servingTime = matchingDistribution.Time;
                Servers[serv].twt += servingTime;
                Servers[serv].customersPerServer++;
                ttc++;
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


                List<int> numbers = new List<int>();
                bool IsServerAvailable = false;
                for (int server = 0; server < Servers.Count; server++)
                {



                    if (ClockArrivalTimes < Servers[server].FinishTime)
                    {
                        if (Servers[server].FinishTime - ClockArrivalTimes < min_wait)
                        {

                            min_wait = Servers[server].FinishTime - ClockArrivalTimes;
                            server_if_not = server;
                        }
                    }
                    else
                    {
                        numbers.Add(server);
                        IsServerAvailable = true;
                    }
                }

                if (IsServerAvailable)
                {
                    int numsss = randoms.Next(0, numbers.Count);
                    int randomServerIndex = numbers[numsss];
                    var selectedServer = Servers[randomServerIndex];
                    TimeDistribution matchingDistribution = null;
                    foreach (var timeDistribution in selectedServer.TimeDistribution)
                    {
                        if (RandomServices >= timeDistribution.MinRange && RandomServices <= timeDistribution.MaxRange)
                        {
                            matchingDistribution = timeDistribution;
                            break; // Exit the loop when a match is found
                        }
                    }





                    SimulationTable.Add(new SimulationCase
                    {
                        AssignedServer = Servers[randomServerIndex],
                        CustomerNumber = i,
                        RandomInterArrival = Randomfor_Intrearival,
                        ArrivalTime = ClockArrivalTimes,
                        InterArrival = matchingDistribution_for_arrival.Time,
                        RandomService = RandomServices,
                        StartTime = ClockArrivalTimes,
                        ServiceTime = matchingDistribution.Time,
                        EndTime = ClockArrivalTimes + matchingDistribution.Time,
                        TimeInQueue = 0
                    });
                    if (i == StoppingNumber)
                    {
                        totalSimulationTime = ClockArrivalTimes + matchingDistribution.Time;
                    }
                    Servers[randomServerIndex].FinishTime = ClockArrivalTimes + matchingDistribution.Time;
                    ttc++;
                    servingTime = matchingDistribution.Time;
                    Servers[randomServerIndex].twt += servingTime;
                    Servers[randomServerIndex].customersPerServer++;

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
                        ArrivalTime = ClockArrivalTimes,
                        InterArrival = matchingDistribution_for_arrival.Time,
                        RandomService = RandomServices,
                        StartTime = Servers[server_if_not].FinishTime,
                        ServiceTime = matchingDistribution.Time,
                        EndTime = Servers[server_if_not].FinishTime + matchingDistribution.Time,
                        TimeInQueue = Servers[server_if_not].FinishTime - ClockArrivalTimes
                    });
                    if (i == StoppingNumber)
                    {
                        totalSimulationTime = Servers[server_if_not].FinishTime + matchingDistribution.Time;
                    }
                    t = Servers[server_if_not].FinishTime - (ClockArrivalTimes);
                    Servers[server_if_not].FinishTime += matchingDistribution.Time;
                    servingTime = matchingDistribution.Time;
                    Servers[server_if_not].twt += servingTime;
                    Servers[server_if_not].customersPerServer++;
                    Console.WriteLine("waiting time of customer " + i + " is " + t);
                    waitingSum += t;
                    t = 0;
                    ctr++;
                    ttc++;


                }

            }
            //  List<SimulationCase> simulationCases = new List<SimulationCase>();
            // Populate the simulationCases list with your simulation data
            Console.WriteLine("VECTOR DATA");



        }

        public int max_q()
        {

            Dictionary<int, int> em = new Dictionary<int, int>(); //vector


            for (int i = 0; i < SimulationTable.Count(); i++)
            {
                for (int j = SimulationTable[i].ArrivalTime; j < SimulationTable[i].StartTime; j++)
                {
                    if (em.ContainsKey(j))
                    {
                        em[j] += 1;
                    }
                    else
                    {
                        em[j] = 1;
                    }

                }


            }
            Console.WriteLine("Dict");
            if (em.Count > 0)
            {
                PerformanceMeasures.MaxQueueLength = em.Values.Max();
            }
            else
            {
                PerformanceMeasures.MaxQueueLength = 0;
            }


            return 0;
        }

    }
}






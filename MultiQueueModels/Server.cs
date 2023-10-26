using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiQueueModels
{
    public class Server
    {
        public Server()
        {
            this.TimeDistribution = new List<TimeDistribution>();
        }

        public int ID { get; set; }
        public decimal IdleProbability { get; set; }
        public decimal AverageServiceTime { get; set; }
        public decimal Utilization { get; set; }

        public List<TimeDistribution> TimeDistribution;
        public decimal twt = 0; //total working time
        public decimal customersPerServer = 0; //total customer of server
        public decimal idle = 0;
        //optional if needed use them
        public int FinishTime { get; set; }
        public int TotalWorkingTime { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiderNets.Service.SystemMonitor
{
    public class NetworkCounter
    {
        PerformanceCounter performanceCounterSent;
        PerformanceCounter performanceCounterReceived;

        string instance;
        public NetworkCounter(string instance)
        {
            this.instance = instance;
            performanceCounterSent = new PerformanceCounter("Network Interface", "Bytes Sent/sec", instance);
            performanceCounterReceived = new PerformanceCounter("Network Interface", "Bytes Received/sec", instance);
        }
        public string Instance { get { return instance; } }
        public float GetSent()
        {
            return performanceCounterSent.NextValue();
        }
        public float GetReceived()
        {
            return performanceCounterReceived.NextValue();
        }
    }
}

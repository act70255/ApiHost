using SpiderNets.Utility.Extension;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SpiderNets.Service.SystemMonitor
{
    public sealed class SystemMonitorService
    {
        private static readonly Lazy<SystemMonitorService> lazy = new Lazy<SystemMonitorService>(() => new SystemMonitorService());

        public static SystemMonitorService Instance { get { return lazy.Value; } }

        List<NetworkCounter> networkCounters = new List<NetworkCounter>();

        private SystemMonitorService()
        {
        }

        public List<CounterMonitor> GetCounterMonitors()
        {
            List<CounterMonitor> monitors = new List<CounterMonitor>();

            monitors.AddRange(GetMonitor(CounterType.CPU));
            monitors.AddRange(GetMonitor(CounterType.RAMused));
            monitors.AddRange(GetMonitor(CounterType.RAMspace));
            monitors.AddRange(GetMonitor(CounterType.DISK));
            monitors.AddRange(GetMonitor(CounterType.NET));

            return monitors;
        }

        IEnumerable<CounterMonitor> GetMonitor(CounterType counterType)
        {
            if (counterType == CounterType.NET)
            {
                PerformanceCounterCategory performanceCounterCategory = new PerformanceCounterCategory("Network Interface");
                networkCounters = performanceCounterCategory.GetInstanceNames().Select(instance => new NetworkCounter(instance)).ToList();
                return networkCounters.Select(counter => new CounterMonitor(counter, CounterType.NET));
            }
            if (string.IsNullOrEmpty(counterType.GetInstanceName()))
            {
                return new List<CounterMonitor> { new CounterMonitor(new PerformanceCounter(counterType.GetCategoryName(), counterType.GetCounterName()), counterType) };
            }
            else
            {
                return new List<CounterMonitor> { new CounterMonitor(new PerformanceCounter(counterType.GetCategoryName(), counterType.GetCounterName(), counterType.GetInstanceName()), counterType) };
            }
        }
    }
}

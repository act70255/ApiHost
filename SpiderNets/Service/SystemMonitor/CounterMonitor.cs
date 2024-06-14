using SpiderNets.Utility.Extension;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SpiderNets.Service.SystemMonitor
{
    public enum CounterType
    {
        [Description("CPU usage")]
        [CategoryName("Processor")]
        [CounterName("% Processor Time")]
        [InstanceName("_Total")]
        [Postfix("%")]
        CPU,
        [Description("RAM usage")]
        [CategoryName("Process")]
        [CounterName("Working Set")]
        [InstanceName("_Total")]
        [Postfix("B")]
        RAMused,
        [Description("RAM useable")]
        [CategoryName("Memory")]
        [CounterName("Available Bytes")]
        [Postfix("B")]
        RAMspace,
        [Description("Disk busy")]
        [CategoryName("PhysicalDisk")]
        [CounterName("% Disk Time")]
        [InstanceName("_Total")]
        [Postfix("%")]
        DISK,
        [Description("Network")]
        [CategoryName("Processor")]
        [CounterName("% Processor Time")]
        [InstanceName("_Total")]
        [Postfix("B")]
        NET
    }
    public class CounterMonitor
    {
        public event EventHandler<DataEventArgs> ValueUpdated;
        PerformanceCounter _performanceCounter;
        NetworkCounter _networkCounter;
        public string DisplayValue { get; set; }
        public CounterType CounterType { get; set; }
        public int RefreshRate { get => _refreshRate; set { _refreshRate = value; Debug.WriteLine($"[{CounterType}]_refreshRate <= {value}"); } }
        int _refreshRate;
        public CounterMonitor()
        {
        }
        public CounterMonitor(PerformanceCounter counter, CounterType counterType)
        {
            _performanceCounter = counter;
            CounterType = counterType;
        }

        public CounterMonitor(NetworkCounter counter, CounterType counterType)
        {
            _networkCounter = counter;
            CounterType = counterType;
        }

        protected virtual void OnValueUpdated(DataEventArgs e)
        {
            ValueUpdated?.Invoke(this, e);
        }

        public void StartNext(int refreshRate)
        {
            RefreshRate = refreshRate;
            while (true)
            {
                DisplayValue = NextValue();
                OnValueUpdated(new DataEventArgs(DisplayValue));
                Thread.Sleep(RefreshRate);
            }
        }
        public string NextValue()
        {
            if (_performanceCounter != null)
            {
                return $"{ParseValueString(_performanceCounter.NextValue(), CounterType.GetPostfix())}";
            }
            else if (_networkCounter != null)
            {
                return $"↑{ParseValueString(_networkCounter.GetSent())} ↓{ParseValueString(_networkCounter.GetReceived())}";
            }
            return "No counter...";
        }
        public string ParseValueString(float value, string postfix = "B")
        {
            string infix = " ";
            var kb = 1024;
            var mb = Math.Pow(kb, 2);
            var gb = Math.Pow(kb, 3);
            if (value > gb)
            {
                return $"{(value / gb).ToString("0.00")}{infix}{(postfix == "B" ? "GB" : postfix)}";
            }
            if (value > mb)
            {
                return $"{(value / mb).ToString("0.00")}{infix}{(postfix == "B" ? "MB" : postfix)}";
            }
            else if (value > kb)
            {
                return $"{(value / kb).ToString("0.00")}{infix}{(postfix == "B" ? "KB" : postfix)}";
            }
            else
            {
                return $"{value.ToString("0.00")}{infix}{(postfix == "B" ? "B" : postfix)}";
            }
        }
    }

    public class DataEventArgs : EventArgs
    {
        public readonly string _data;
        public DataEventArgs(string data)
        {
            _data = data;
        }

        public string Data
        {
            get
            {
                return _data;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ApiHost.CLI.Model
{
    public enum CounterType
    {
        CPU,
        RAMused,
        RAMspace,
        DISK,
        NET
    }
    public class MonitorModel
    {
        public string DisplayValue { get; set; }
        public CounterType CounterType { get; set; }
        public MonitorModel()
        {
        }
    }
}

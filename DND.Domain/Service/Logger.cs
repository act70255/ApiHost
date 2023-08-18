using DND.Domain.Service.Interface;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DND.Domain.Service
{
    public class Logger : ILogger
    {
        public void Write(string message, params object[] args)
        {
            Console.WriteLine($"[{DateTime.Now.ToString("MM/dd HH:mm")} LOG][{message}] {JsonConvert.SerializeObject(args)}");
        }
    }
}

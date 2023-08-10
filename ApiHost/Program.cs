using ApiHost.Service;
using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ApiHost
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string hostAddress = "http://localhost:9000/";
            HostProvider apiServer = null;
            var command = Console.ReadLine();
            while(command != "exit")
            {
                switch (command)
                {
                    case "start":
                        {
                            if (apiServer == null)
                            {
                                apiServer = new HostProvider(hostAddress);
                                apiServer.HostStatusChanged += (s, e) =>
                                {
                                    Debug.WriteLine($"HostStatusChanged {e} {DateTime.Now}");
                                };
                            }
                            apiServer.Start();
                            break;
                        }
                    case "stop":
                        {
                            if(apiServer == null)
                                return;
                            apiServer.Stop();
                            GC.Collect();
                            break;
                        }
                    default:
                        {
                            Console.WriteLine("UnHandeled command");
                            break;
                        }
                }
                command = Console.ReadLine();
            }
        }
    }
}

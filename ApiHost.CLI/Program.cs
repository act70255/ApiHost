using ApiHost.Host;
using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ApiHost.CLI
{
    internal class Program
    {
        static HostProvider apiServer = null;
        static void Main(string[] args)
        {
            string HostAddress = "http://localhost:9000/";
            StartServer();
            var command = Console.ReadLine();
            while (command != "exit")
            {
                switch (command)
                {
                    case "start":
                        {
                            StartServer();
                            break;
                        }
                    case "stop":
                        {
                            StopServer();
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

            void StartServer()
            {
                if (apiServer == null)
                {
                    apiServer = new HostProvider(HostAddress);
                    apiServer.HostStatusChanged += (s, e) =>
                    {
                        Debug.WriteLine($"HostStatusChanged {e} {DateTime.Now}");
                    };
                }
                apiServer.Start();
            }
            void StopServer()
            {
                if (apiServer == null)
                    return;
                apiServer.Stop();
                GC.Collect();
            }
        }
    }
}

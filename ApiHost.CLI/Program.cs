using ApiHost.Host;
using Microsoft.Owin.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ApiHost.CLI
{
    internal class Program
    {
        static ApiServer apiServer = null;
        static void Main(string[] args)
        {
            string Port()
            {
                return ConfigurationSettings.AppSettings["HostPort"];
            }
            string IPAddress()
            {
                return ConfigurationSettings.AppSettings["HostIP"];
            }

            string HostAddress = $"{IPAddress()}:{Port()}/";
            HostMessaging.Instance.DataReceived+= (s, e) =>
            {
                Console.WriteLine($"DataReceived [{e.Item1}/{e.Item2}] {JsonConvert.SerializeObject(e.Item3)}");
            };

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
                    apiServer = new ApiServer(HostAddress);
                    apiServer.DataReceived += (s, e) =>
                        {
                            Console.WriteLine($"DataReceived [{e.Item1}/{e.Item2}] {JsonConvert.SerializeObject(e.Item3)}");
                        };
                }
                apiServer.Start();
            }
            void StopServer()
            {
                if (apiServer == null)
                    return;
                apiServer.Stop();
                apiServer = null;
                GC.Collect();
            }
        }
    }
}

using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ApiHost.DNDHost
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string Port() => ConfigurationManager.AppSettings["HostPort"];
            string IPAddress() => ConfigurationManager.AppSettings["HostIP"];
            string HostAddress = $"{IPAddress()}:{Port()}/";
            using (var apiServer = new ApiHost.Host.ApiServer(HostAddress))
            {
                apiServer.Start<Startup>();
                Console.ReadLine();
            }

            if (Debugger.IsAttached)
            {
            }
        }
    }
}

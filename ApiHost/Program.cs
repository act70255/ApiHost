using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
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
            string baseAddress = "http://localhost:9000/";

            // Start OWIN host 
            using (WebApp.Start<StartUp>(url: baseAddress))
            {
                #region Testing...
                HttpClient client = new HttpClient();

                var response = client.GetAsync(baseAddress + "api/Breath/Get").Result;

                Console.WriteLine(response);
                Console.WriteLine(response.Content.ReadAsStringAsync().Result);
                Console.ReadLine(); 
                #endregion
            }
        }
    }
}

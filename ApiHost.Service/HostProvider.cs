using Microsoft.Owin.Hosting;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ApiHost.Host
{
    public class HostProvider:IDisposable
    {
        public event EventHandler<HostStatus> HostStatusChanged;
        private IDisposable _server;
        string HostAddress = "http://localhost:9000/";

        public HostProvider(string hostAddress)
        {
            HostAddress = hostAddress;
        }

        public void Start()
        {
            try
            {
                // Start OWIN host 
                _server = WebApp.Start<StartUp>(HostAddress);
                
                HttpClient client = new HttpClient();
                var response = client.PostAsJsonAsync<object>($"{HostAddress}api/Host/Data", new { Instance = "StartTest", Time = DateTime.Now }).GetAwaiter().GetResult();

                Console.WriteLine(response);
                HostStatusChanged?.Invoke(this, HostStatus.HostStarted);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public void Stop()
        {
            try
            {
                this.Dispose();
                HostStatusChanged?.Invoke(this, HostStatus.HostStopped);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        public void Dispose()
        {
            _server?.Dispose();
            _server = null;
        }
    }

    public enum HostStatus
    {
        HostStarted,
        HostStopped,
        HostError,
    }
}

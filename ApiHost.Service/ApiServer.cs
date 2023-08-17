using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiHost.Host
{
    public class ApiServer:IDisposable
    {
        static HostProvider apiHost = null;

        public string HostAddress { get; set; }
        public HostStatus HostStatus { get => _hostStatus; set { _hostStatus = value; HostStatusChanged?.Invoke(this,value); } }
        HostStatus _hostStatus;
        public event EventHandler<HostStatus> HostStatusChanged;
        public event EventHandler<Tuple<string, string, object>> DataReceived;

        public ApiServer(string url)
        {
            HostAddress = url;
            HostMessaging.Instance.DataReceived+= (s, e) =>
            {
                OnDataReceived(e.Item1, e.Item2, e.Item3);
            };
        }
        public void OnDataReceived(string controller, string action,object data)
        {
            DataReceived?.Invoke(null, new Tuple<string, string, object>(controller, action, data));
        }

        public void Start<TStartUp>()
        {
            if (apiHost == null)
            {
                Console.WriteLine($"Starting server at {HostAddress}");
                apiHost = new HostProvider(HostAddress);
                apiHost.HostStatusChanged += (s, e) =>
                {
                    HostStatus = e;
                    Debug.WriteLine($"HostStatusChanged {e} {DateTime.Now}");
                };
            }
            apiHost.Start<TStartUp>();
        }
        public void Stop()
        {
            if (apiHost == null)
                return;
            apiHost.Stop();
            apiHost = null;
            HostStatus = HostStatus.HostStopped;
            GC.Collect();
        }

        public void Dispose()
        {
            Stop();
        }
    }
}

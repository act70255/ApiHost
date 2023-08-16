using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiHost.Host
{
    public sealed class HostMessaging
    {
        private static readonly Lazy<HostMessaging> lazy =
            new Lazy<HostMessaging>(() => new HostMessaging());

        public static HostMessaging Instance { get { return lazy.Value; } }

        public event EventHandler<Tuple<string, string, object>> DataReceived;
        public void OnDataReceived(string controller, string action, object data)
        {
            DataReceived?.Invoke(null, new Tuple<string, string, object>(controller, action, data));
        }

        private HostMessaging()
        {
        }
    }
}

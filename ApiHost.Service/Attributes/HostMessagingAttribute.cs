using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace ApiHost.Host.Attributes
{
    /// <summary>
    /// 轉換處理接收者
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class HostMessagingAttribute : Attribute, IAuthorizationFilter
    {
        public bool AllowMultiple => true;
        public object[] IgnoreParams = new object[] { };

        public HostMessagingAttribute()
        { }

        public HostMessagingAttribute(object[] ignoreParams)
        {
            IgnoreParams = ignoreParams;
        }

        public Task<HttpResponseMessage> ExecuteAuthorizationFilterAsync(HttpActionContext actionContext, CancellationToken cancellationToken, Func<Task<HttpResponseMessage>> continuation)
        {
            try
            {
                var data = actionContext.Request.Content.ReadAsAsync<object>().GetAwaiter().GetResult();
                if (!IgnoreParams.Contains(data))
                    HostMessaging.Instance.OnDataReceived(actionContext.ActionDescriptor.ControllerDescriptor.ControllerName, actionContext.ActionDescriptor.ActionName, data);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            return continuation();
        }
    }
}

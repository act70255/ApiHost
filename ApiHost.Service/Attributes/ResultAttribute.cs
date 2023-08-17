using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Filters;

namespace ApiHost.Host.Attributes
{
    public class ResultAttribute : ActionFilterAttribute
    {
        override public void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            base.OnActionExecuting(actionContext);
        }
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.Exception != null)
            {
                return;
            }

            var data = actionExecutedContext.Response.Content.ReadAsAsync<object>().Result;
            Console.WriteLine(JsonConvert.SerializeObject(data));
            var result = new
            {
                Success = true,
                data = data
            };

            actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(result);
        }
    }
}

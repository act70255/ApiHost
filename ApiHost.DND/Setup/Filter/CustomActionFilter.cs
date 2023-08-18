using Autofac.Integration.WebApi;
using DND.Domain.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace ApiHost.DNDHost.Controller.Filter
{
    public class ResultActionFilter : IAutofacActionFilter
    {
        private readonly ILogger _logger;

        public ResultActionFilter(ILogger logger)
        {
            _logger = logger;
        }

        public Task OnActionExecutingAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            //_logger.Write("Inside the 'OnActionExecutingAsync' method of the custom action filter.");
            return Task.FromResult(0);
        }

        public Task OnActionExecutedAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        {
            //_logger.Write("Inside the 'OnActionExecutedAsync' method of the custom action filter.");
            if (actionExecutedContext.Exception != null)
            {
                return Task.FromResult(0);
            }

            var data = actionExecutedContext.Response.Content?.ReadAsAsync<object>().GetAwaiter().GetResult();
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(data));
            var result = new
            {
                Success = true,
                data = data
            };

            actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(result);
            return Task.FromResult(0);
        }
    }
}

using DND.Domain.Service.Interface;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace ApiHost.DNDHost
{
    public class DNDMiddleware : OwinMiddleware
    {
        private readonly ILogger _logger;

        public DNDMiddleware(OwinMiddleware next, ILogger logger) : base(next)
        {
            _logger = logger;
        }

        public override async Task Invoke(IOwinContext context)
        {
            _logger.Write("DNDMiddleware", context.Request.Path.Value);
            await Next.Invoke(context);
        }
    }
}

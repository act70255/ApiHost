using Autofac;
using Autofac.Integration.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace ApiHost.DND.Setup.Module
{
    public class ControllerModule: Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DNDMiddleware>().InstancePerRequest();

            builder.RegisterType<Controller.Filter.ResultActionFilter>().AsWebApiActionFilterFor<Controller.DNDController>().InstancePerRequest();

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
        }
    }
}

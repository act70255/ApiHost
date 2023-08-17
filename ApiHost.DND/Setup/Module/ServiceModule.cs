using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiHost.DND.Setup.Module
{
    public class ServiceModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new Service.Logger()).As<Service.Interface.ILogger>().InstancePerRequest();

            builder.RegisterType<Service.TerriarService>().As<Service.Interface.ITerriarService>().InstancePerRequest();

            
        }
    }
}

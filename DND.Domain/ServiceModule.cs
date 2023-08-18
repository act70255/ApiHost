using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DND.Domain
{
    public class ServiceModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new Service.Logger()).As<Service.Interface.ILogger>().InstancePerRequest();

            builder.RegisterType<Service.TerrariaService>().As<Service.Interface.ITerrariaService>().InstancePerRequest();
            builder.RegisterType<Service.CreatureService>().As<Service.Interface.ICreatureService>().InstancePerRequest();
        }
    }
}

using Autofac;
using Autofac.Integration.WebApi;
using AutoMapper.Configuration;
using Microsoft.Owin.Logging;
using Owin;
using System.Configuration;
using System.Reflection;
using System.Web.Http;

namespace ApiHost.DND
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var builder = new ContainerBuilder();
            var config = new HttpConfiguration();
            
            config.Routes.MapHttpRoute(
                "DefaultApi",
                "api/{controller}/{action}",
                new { id = RouteParameter.Optional });
            builder.RegisterWebApiFilterProvider(config);

            // Register Controllers.
            builder.RegisterModule(new Setup.Module.ControllerModule());

            //Register service
            builder.RegisterModule(new Setup.Module.ServiceModule());

            //Register AutoMapper
            builder.RegisterModule(new Setup.Module.AutoMapperModule());

            //Register configuration
            //builder.RegisterType<Configuration>().As<IConfiguration>().InstancePerLifetimeScope();

            // Create and assign a dependency resolver for Web API to use.
            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            app.UseAutofacMiddleware(container);

            // Again, the alternative to "UseAutofacMiddleware" is something like this:
            // app.UseAutofacLifetimeScopeInjector(container);
            // app.UseMiddlewareFromContainer<FirstMiddleware>();
            // app.UseMiddlewareFromContainer<SecondMiddleware>();

            // Make sure the Autofac lifetime scope is passed to Web API.
            app.UseAutofacWebApi(config);
            app.UseWebApi(config);
        }
    }
}

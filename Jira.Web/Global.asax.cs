using System.IO;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using Jira.Business;
using Jira.Business.Clients;
using Jira.Models.Config;
using RestSharp;

namespace Jira.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            RegisterDependencies();
        }

        private void RegisterDependencies()
        {
            var builder = new ContainerBuilder();

            // Register your MVC controllers.
            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            
            var configPath = Server.MapPath("~/config.json");

            builder.Register(x => new ConfigProvider<Config>(configPath)).As<IConfigProvider<Config>>().SingleInstance();

            builder.RegisterType<RestClient>().As<IRestClient>();
            builder.RegisterType<JiraClient>().As<IJiraClient>();
            builder.RegisterType<WorklogClient>().As<IWorklogClient>();

            // Set the dependency resolver to be Autofac.
            var container = builder.Build();
            
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}

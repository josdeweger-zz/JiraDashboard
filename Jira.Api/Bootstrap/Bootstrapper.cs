using Autofac;
using Jira.Business;
using Jira.Business.Clients;
using Jira.Business.Stores;
using Jira.Models.Config;
using Nancy.Bootstrappers.Autofac;
using Nancy.Diagnostics;
using RestSharp;

namespace Jira.Api.Bootstrap
{
    public class Bootstrapper : AutofacNancyBootstrapper
    {
        protected override void ConfigureApplicationContainer(ILifetimeScope container)
        {
            //Configuration
            container.Update(
                        builder => builder.Register(c => new ConfigProvider<Config>(@"config.json")).As<IConfigProvider<Config>>()
                        .SingleInstance());
            container.Update(
                builder =>
                    builder.Register(c => c.Resolve<IConfigProvider<Config>>().Get()).As<IConfig>().SingleInstance());

            //Stores
            container.Update(
                builder =>
                    builder.RegisterType<CookieStore>().As<ICookieStore>().SingleInstance());

            //Rest Clients
            container.Update(builder => builder.RegisterType<RestClient>().As<IRestClient>());
            container.Update(builder => builder.RegisterType<JiraClient>().As<IJiraClient>());

            container.Update(builder => builder.RegisterType<CustomerStatusProvider>().As<ICustomerStatusProvider>());
        }

        protected override DiagnosticsConfiguration DiagnosticsConfiguration => new DiagnosticsConfiguration { Password = @"b" };
    }
}

using Autofac;
using Jira.Api.Business;
using Jira.Api.Business.Clients;
using Jira.Api.Business.Stores;
using Jira.Api.Models.Config;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Bootstrappers.Autofac;
using Nancy.Configuration;
using Nancy.Conventions;
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
                        builder => builder.Register(c => new Config()).As<IConfig>()
                        .SingleInstance());

            //Stores
            container.Update(
                builder =>
                    builder.RegisterType<CookieStore>().As<ICookieStore>().SingleInstance());

            //Rest Clients
            container.Update(builder => builder.RegisterType<RestClient>().As<IRestClient>());
            container.Update(builder => builder.RegisterType<RestClientWrapper>().As<IRestClientWrapper>());

            container.Update(builder => builder.RegisterType<CustomerStatusProvider>().As<ICustomerStatusProvider>());
            container.Update(builder => builder.RegisterType<AuthenticationProvider>().As<IAuthenticationProvider>());
        }

        public override void Configure(INancyEnvironment environment)
        {
            var config = ApplicationContainer.Resolve<IConfig>();

            environment.Diagnostics(
                enabled: config.DiagnosticsEnabled,
                password: config.DiagnosticsPassword,
                path: "/_Nancy",
                cookieName: "jira.api",
                slidingTimeout: 30);

            environment.Tracing(
                enabled: true,
                displayErrorTraces: true);
        }
    }
}

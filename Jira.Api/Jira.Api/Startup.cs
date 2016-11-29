using Jira.Api.Bootstrap;
using Microsoft.AspNetCore.Builder;
using Nancy.Owin;

namespace Jira.Api
{
    public class Startup
    {
        public void Configure(IApplicationBuilder app)
        {
            app.UseOwin(x => x.UseNancy(options =>
            {
                options.Bootstrapper = new Bootstrapper();
            }));
        }
    }
}

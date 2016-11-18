using System;
using Microsoft.Owin.Hosting;
using Topshelf;

namespace Jira.Api
{
    internal class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(x =>
            {
                x.Service<JiraApiService>(s =>
                {
                    s.ConstructUsing(name => new JiraApiService());
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());
                });

                x.RunAsLocalSystem();

                x.SetDescription("Redhotminute - Jira Api Service");
                x.SetDisplayName("JiraApiService");
                x.SetServiceName("JiraApiService");
            });
        }
    }
}
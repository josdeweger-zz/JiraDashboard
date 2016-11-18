using System;
using Nancy.Hosting.Self;

namespace Jira.Api
{
    public class JiraApiService
    {
        private NancyHost _host;
        private const string Url = "http://localhost:8080";
        
        public void Start()
        {
            _host = new NancyHost(new Uri(Url));
            _host.Start();
        }

        public void Stop()
        {
            _host.Stop();
        }
    }
}

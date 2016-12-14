using System;

namespace Jira.Api.Models.Config
{
    public class Config : IConfig
    {
        public string RestApi => Environment.GetEnvironmentVariable("ASPNETCORE_JIRA_REST_API");
        public string Username => Environment.GetEnvironmentVariable("ASPNETCORE_JIRA_USERNAME");
        public string Password => Environment.GetEnvironmentVariable("ASPNETCORE_JIRA_PASSWORD");
        public string AuthenticationResource => Environment.GetEnvironmentVariable("ASPNETCORE_JIRA_AUTHENTICATION_RESOURCE");
        public string WorklogResource => Environment.GetEnvironmentVariable("ASPNETCORE_JIRA_WORKLOG_RESOURCE");
        public int FaultMarginPercentage => int.Parse(Environment.GetEnvironmentVariable("ASPNETCORE_FAULTMARGIN_PERCENTAGE"));
    }
}
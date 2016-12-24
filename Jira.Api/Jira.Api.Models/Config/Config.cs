using System;

namespace Jira.Api.Models.Config
{
    public class Config : IConfig
    {
        public bool DiagnosticsEnabled => Get<bool>("ASPNETCORE_JIRA_DIAGNOSTICS_ENABLED");
        public string DiagnosticsPassword => Get<string>("ASPNETCORE_JIRA_DIAGNOSTICS_PASSWORD");
        public string RestApi => Get<string>("ASPNETCORE_JIRA_REST_API");
        public string Username => Get<string>("ASPNETCORE_JIRA_USERNAME");
        public string Password => Get<string>("ASPNETCORE_JIRA_PASSWORD");
        public string AuthenticationResource => Get<string>("ASPNETCORE_JIRA_AUTHENTICATION_RESOURCE");
        public string WorklogResource => Get<string>("ASPNETCORE_JIRA_WORKLOG_RESOURCE");
        public int FaultMarginPercentage => Get<int>("ASPNETCORE_FAULTMARGIN_PERCENTAGE");

        private static T Get<T>(string environmentVariable)
        {
            return (T)Convert.ChangeType(Environment.GetEnvironmentVariable(environmentVariable), typeof(T));
        }
    }
}
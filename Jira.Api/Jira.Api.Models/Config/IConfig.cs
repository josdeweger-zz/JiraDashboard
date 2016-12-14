namespace Jira.Api.Models.Config
{
    public interface IConfig
    {
        string RestApi { get; }
        string Username { get; }
        string Password { get; }
        string AuthenticationResource { get; }
        string WorklogResource { get; }
        int FaultMarginPercentage { get; }
    }
}

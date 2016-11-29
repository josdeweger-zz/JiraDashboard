namespace Jira.Api.Models.Config
{
    public interface IConfig
    {
        string RestApi { get; set; }
        string Username { get; set; }
        string Password { get; set; }
        string AuthenticationResource { get; set; }
        string WorklogResource { get; set; }
        int FaultMarginPercentage { get; set; }
    }
}

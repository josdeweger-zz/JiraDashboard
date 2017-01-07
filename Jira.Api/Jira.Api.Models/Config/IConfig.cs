namespace Jira.Api.Models.Config
{
    public interface IConfig
    {
        bool DiagnosticsEnabled { get; }
        string DiagnosticsPassword { get; }
        string RestApi { get; }
        string AuthenticationResource { get; }
        string WorklogResource { get; }
        int FaultMarginPercentage { get; }
    }
}

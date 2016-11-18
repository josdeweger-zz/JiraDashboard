namespace Jira.Models.Config
{
    public class Config : IConfig
    {
        public string RestApi { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string AuthenticationResource { get; set; }
        public string WorklogResource { get; set; }
        public int FaultMarginPercentage { get; set; }
    }
}
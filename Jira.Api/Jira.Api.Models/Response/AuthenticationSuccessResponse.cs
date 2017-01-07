using Jira.Api.Models.Jira;

namespace Jira.Api.Models.Response
{
    public class AuthenticationSuccessResponse
    {
        public LoginInfo LoginInfo { get; set; }
        public Session Session { get; set; }
    }
}

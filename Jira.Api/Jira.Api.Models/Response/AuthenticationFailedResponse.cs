using System.Collections.Generic;
using Jira.Api.Models.Jira;

namespace Jira.Api.Models.Response
{
    public class AuthenticationFailedResponse
    {
        public string Message { get; set; }
        public List<string> ErrorMessages { get; set; }
    }
}

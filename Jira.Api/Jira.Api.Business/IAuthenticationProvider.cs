using Jira.Api.Models.Request;
using RestSharp;

namespace Jira.Api.Business
{
    public interface IAuthenticationProvider
    {
        IRestResponse Authenticate(AuthenticationRequest authenticationRequest);
    }
}

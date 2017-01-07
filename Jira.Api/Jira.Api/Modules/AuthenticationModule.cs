using Jira.Api.Business;
using Jira.Api.Business.Extensions;
using Jira.Api.Models.Request;
using Jira.Api.Models.Response;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Validation;
using Newtonsoft.Json;

namespace Jira.Api.Modules
{
    public sealed class AuthenticationModule : NancyModule
    {
        public AuthenticationModule(IAuthenticationProvider authenticationProvider)
        {
            Post("authentication", (ctx) =>
            {
                var authenticationRequest = this.Bind<AuthenticationRequest>();

                var validationResult = this.Validate(authenticationRequest);

                if (!validationResult.IsValid)
                    return Negotiate.WithModel(validationResult).WithStatusCode(HttpStatusCode.BadRequest);

                var response = authenticationProvider.Authenticate(authenticationRequest);

                if (!response.IsSuccessful())
                {
                    var failedResponse = JsonConvert.DeserializeObject<AuthenticationFailedResponse>(response.Content);
                    return Negotiate.WithModel(failedResponse).WithStatusCode((HttpStatusCode) response.StatusCode);
                }

                var successResponse = JsonConvert.DeserializeObject<AuthenticationSuccessResponse>(response.Content);
                return Negotiate.WithModel(successResponse);
            });
        }
    }
}

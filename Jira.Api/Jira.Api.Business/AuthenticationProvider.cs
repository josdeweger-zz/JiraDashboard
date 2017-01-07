using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Jira.Api.Business.Clients;
using Jira.Api.Models.Config;
using Jira.Api.Models.Jira;
using Jira.Api.Models.Request;
using Jira.Api.Models.Response;
using Newtonsoft.Json;
using RestSharp;
using Jira.Api.Business.Extensions;
using Nancy;
using Nancy.Responses.Negotiation;

namespace Jira.Api.Business
{
    public class AuthenticationProvider : IAuthenticationProvider
    {
        private readonly IConfig _config;
        private readonly IRestClientWrapper _jiraClient;

        public AuthenticationProvider(IConfig config, IRestClientWrapper jiraClient)
        {
            _config = config;
            _jiraClient = jiraClient;
        }

        public IRestResponse Authenticate(AuthenticationRequest authenticationRequest)
        {
            return _jiraClient
                .WithBaseUrl(_config.RestApi)
                .ForResource(_config.AuthenticationResource, Method.POST)
                .WithBody(new { username = authenticationRequest.Username, password = authenticationRequest.Password })
                .ExecuteAsync()
                .Result;
        }
    }
}

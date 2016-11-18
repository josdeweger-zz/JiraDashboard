using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Jira.Business.Stores;
using Jira.Models;
using Jira.Models.Config;
using Jira.Models.Jira;
using RestSharp;

namespace Jira.Business.Clients
{
    public class JiraClient : IJiraClient
    {
        private readonly IConfig _config;
        private readonly ICookieStore _authenticationCookieStore;
        private readonly IRestClient _client;

        private const string AuthenticationCookieName = "JSESSIONID";
        private bool _authenticate = false;
        private Credentials _credentials;
        private string _baseUrl;
        private string _resource;
        private Method _method;
        private IDictionary<string, string> _queryParams = new Dictionary<string, string>();
        private object _jsonBody;

        public JiraClient(IConfig config, IRestClient client, ICookieStore authenticationCookieStore)
        {
            _config = config;
            _client = client;
            _authenticationCookieStore = authenticationCookieStore;
        }

        public IJiraClient WithBaseUrl(string baseUrl)
        {
            _baseUrl = baseUrl;
            _client.BaseUrl = new Uri(baseUrl);

            return this;
        }

        public IJiraClient ForResource(string resource, Method method = Method.GET)
        {
            _resource = resource;
            _method = method;
            
            return this;
        }

        public IJiraClient WithQueryParams(IDictionary<string, string> queryParams)
        {
            _queryParams = queryParams;

            return this;
        }

        public IJiraClient WithQueryParam(string key, string value)
        {
            _queryParams.Add(key, value);

            return this;
        }

        public IJiraClient WithBody(object body)
        {
            _jsonBody = body;

            return this;
        }

        public IJiraClient UsingCredentials(Credentials credentials)
        {
            _credentials = credentials;
            _authenticate = true;

            return this;
        }

        public T Execute<T>() where T : new()
        {
            var restRequest = CreateRestRequest();

            ClearRequestProperties();

            var response = _client.Execute<T>(restRequest);

            if (response.ErrorException != null)
                throw new ApplicationException("Error retrieving response.  Check inner details for more info.",
                    response.ErrorException);

            return response.Data;
        }

        public async Task<T> ExecuteAsync<T>() where T : new()
        {
            var restRequest = CreateRestRequest();

            ClearRequestProperties();

            var response = await _client.ExecuteTaskAsync<T>(restRequest);

            if (response.ErrorException != null)
                throw new ApplicationException("Error retrieving response.  Check inner details for more info.",
                    response.ErrorException);

            return response.Data;
        }

        private RestRequest CreateRestRequest()
        {
            var restRequest = new RestRequest
            {
                RequestFormat = DataFormat.Json,
                Resource = _resource,
                Method = _method
            };

            restRequest.AddHeader("Content-Type", "application/json");

            foreach (var param in _queryParams)
                restRequest.AddQueryParameter(param.Key, param.Value);

            restRequest.AddJsonBody(_jsonBody);

            if (_authenticate)
            {
                var cookie = SetAuthenticationCookie();
                restRequest.AddCookie(cookie.Key, cookie.Value);
            }

            return restRequest;
        }

        private Cookie SetAuthenticationCookie()
        {
            var authenticationCookie = _authenticationCookieStore.Get(AuthenticationCookieName);
            if (authenticationCookie == null)
            {
                var cookie =
                    new JiraClient(_config, _client, _authenticationCookieStore)
                        .WithBaseUrl(_baseUrl)
                        .ForResource(_config.AuthenticationResource, Method.POST)
                        .WithBody(new {username = _credentials.UserName, password = _credentials.Password})
                        .Execute<CookieAuthentication>();

                _authenticationCookieStore.Store(AuthenticationCookieName, cookie.Session.Name, cookie.Session.Value,
                    DateTime.Now.AddHours(1));

                authenticationCookie = new Cookie(AuthenticationCookieName, cookie.Session.Name, cookie.Session.Value,
                    DateTime.Now.AddHours(1));
            }

            return authenticationCookie;
        }

        private void ClearRequestProperties()
        {
            _resource = default(string);
            _method = default(Method);
            _queryParams.Clear();
            _jsonBody = default(string);
        }
    }
}

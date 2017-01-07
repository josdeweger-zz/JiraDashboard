using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Jira.Api.Models.Exceptions;
using RestSharp;

namespace Jira.Api.Business.Clients
{
    public class RestClientWrapper : IRestClientWrapper
    {
        private readonly IRestClient _client;
        
        private string _resource;
        private Method _method;
        private readonly IDictionary<string, string> _cookies = new Dictionary<string, string>();
        private IDictionary<string, string> _queryParams = new Dictionary<string, string>();
        private object _jsonBody;

        public RestClientWrapper(IRestClient client)
        {
            _client = client;
        }

        public IRestClientWrapper WithBaseUrl(string baseUrl)
        {
            _client.BaseUrl = new Uri(baseUrl);

            return this;
        }

        public IRestClientWrapper ForResource(string resource, Method method = Method.GET)
        {
            _resource = resource;
            _method = method;
            
            return this;
        }

        public IRestClientWrapper WithSessionId(string sessionName, string sessionValue)
        {
            _cookies.Add(sessionName, sessionValue);

            return this;
        }

        public IRestClientWrapper WithQueryParams(IDictionary<string, string> queryParams)
        {
            _queryParams = queryParams;

            return this;
        }

        public IRestClientWrapper WithQueryParam(string key, string value)
        {
            if(!_queryParams.ContainsKey(key))
                _queryParams.Add(key, value);

            return this;
        }

        public IRestClientWrapper WithBody(object body)
        {
            _jsonBody = body;

            return this;
        }

        public Task<IRestResponse> ExecuteAsync()
        {
            var restRequest = CreateRestRequest();

            ClearRequestProperties();

            var taskCompletionSource = new TaskCompletionSource<IRestResponse> ();
            _client.ExecuteAsync(restRequest, response =>
            {
                if (response.ErrorException != null)
                    taskCompletionSource.TrySetException(response.ErrorException);
                else if (response.StatusCode == HttpStatusCode.RequestTimeout)
                    taskCompletionSource.TrySetException(new RequestTimeoutException(response.Content));
                else 
                    taskCompletionSource.SetResult(response);
            });

            return taskCompletionSource.Task;
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

            foreach (var cookie in _cookies)
                restRequest.AddCookie(cookie.Key, cookie.Value);

            restRequest.AddJsonBody(_jsonBody);

            return restRequest;
        }

        private void ClearRequestProperties()
        {
            _resource = default(string);
            _method = default(Method);
            _queryParams.Clear();
            _cookies.Clear();
            _jsonBody = default(string);
        }
    }
}

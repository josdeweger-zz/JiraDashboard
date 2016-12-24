using System.Collections.Generic;
using System.Threading.Tasks;
using Jira.Api.Models.Jira;
using RestSharp;

namespace Jira.Api.Business.Clients
{
    public interface IRestClientWrapper
    {
        IRestClientWrapper WithBaseUrl(string baseUrl);
        IRestClientWrapper ForResource(string resource, Method method = Method.GET);
        IRestClientWrapper WithQueryParams(IDictionary<string, string> queryParams);
        IRestClientWrapper WithQueryParam(string key, string value);
        IRestClientWrapper WithBody(object body);
        IRestClientWrapper UsingCredentials(Credentials credentials);
        T Execute<T>() where T : new();
        Task<T> ExecuteAsync<T>() where T : new();
    }
}
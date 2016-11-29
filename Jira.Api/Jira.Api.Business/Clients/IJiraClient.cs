using System.Collections.Generic;
using System.Threading.Tasks;
using Jira.Api.Models.Jira;
using RestSharp;

namespace Jira.Api.Business.Clients
{
    public interface IJiraClient
    {
        IJiraClient WithBaseUrl(string baseUrl);
        IJiraClient ForResource(string resource, Method method = Method.GET);
        IJiraClient WithQueryParams(IDictionary<string, string> queryParams);
        IJiraClient WithQueryParam(string key, string value);
        IJiraClient WithBody(object body);
        IJiraClient UsingCredentials(Credentials credentials);
        T Execute<T>() where T : new();
        Task<T> ExecuteAsync<T>() where T : new();
    }
}
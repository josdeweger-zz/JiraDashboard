using System.Collections.Generic;
using System.Threading.Tasks;
using RestSharp;

namespace Jira.Api.Business.Clients
{
    public interface IRestClientWrapper
    {
        IRestClientWrapper WithBaseUrl(string baseUrl);
        IRestClientWrapper ForResource(string resource, Method method = Method.GET);
        IRestClientWrapper WithSessionId(string sessionName, string sessionValue);
        IRestClientWrapper WithQueryParams(IDictionary<string, string> queryParams);
        IRestClientWrapper WithQueryParam(string key, string value);
        IRestClientWrapper WithBody(object body);
        Task<IRestResponse> ExecuteAsync();
    }
}
using System.Collections.Generic;
using System.Linq;
using RestSharp.Extensions.MonoHttp;

namespace Jira.Business.Extensions
{
    public static class DictionaryExtensions
    {
        public static string ToQueryString(this IDictionary<string, string> dic)
        {
            var queryParamArray =
                dic.Select(kv => $"{HttpUtility.UrlEncode(kv.Key)}={HttpUtility.UrlEncode(kv.Value)}").ToArray();

            return "?" + string.Join("&", queryParamArray);
        }
    }
}

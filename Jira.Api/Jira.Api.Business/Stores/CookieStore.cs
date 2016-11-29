using System;
using System.Collections.Concurrent;
using Jira.Api.Models;

namespace Jira.Api.Business.Stores
{
    public class CookieStore : ICookieStore
    {
        private readonly ConcurrentDictionary<string, Cookie> _cookies;

        public CookieStore()
        {
            _cookies = new ConcurrentDictionary<string, Cookie>();
        }

        public void Store(string name, string key, string value, DateTime expirationDate)
        {
            var cookie = new Cookie(name, key, value, expirationDate);
            _cookies.GetOrAdd(name, cookie);
        }

        public Cookie Get(string name)
        {
            Cookie cookie;

            if (_cookies.TryGetValue(name, out cookie))
            {
                if (cookie.ExpirationDate > DateTime.Now)
                    return cookie;
            }

            return null;
        }
    }
}

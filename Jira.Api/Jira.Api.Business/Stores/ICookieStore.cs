using System;
using Jira.Api.Models;

namespace Jira.Api.Business.Stores
{
    public interface ICookieStore
    {
        void Store(string name, string key, string value, DateTime expirationDate);
        Cookie Get(string name);
    }
}

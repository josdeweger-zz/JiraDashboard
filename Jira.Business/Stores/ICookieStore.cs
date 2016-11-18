using System;
using Jira.Models;

namespace Jira.Business.Stores
{
    public interface ICookieStore
    {
        void Store(string name, string key, string value, DateTime expirationDate);
        Cookie Get(string name);
    }
}

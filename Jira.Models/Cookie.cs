using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jira.Models
{
    public class Cookie
    {
        public string Name { get; }
        public string Key { get; }
        public string Value { get; }
        public DateTime ExpirationDate { get; }

        public Cookie(string name, string key, string value, DateTime expirationDate)
        {
            Key = key;
            Value = value;
            Name = name;
            ExpirationDate = expirationDate;
        }
    }
}

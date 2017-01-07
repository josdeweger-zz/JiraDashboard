using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jira.Api.Models.Response
{
    public class Error
    {
        public string Key { get; set; }
        public ErrorValue[] Value { get; set; }
    }
}

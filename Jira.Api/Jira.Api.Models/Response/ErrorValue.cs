using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jira.Api.Models.Response
{
    public class ErrorValue
    {
        public string[] MemberNames { get; set; }
        public string ErrorMessage { get; set; }
    }
}

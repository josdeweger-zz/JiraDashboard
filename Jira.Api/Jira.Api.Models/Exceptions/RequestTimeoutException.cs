using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jira.Api.Models.Exceptions
{
    public class RequestTimeoutException : Exception
    {
        public RequestTimeoutException()
        {
        }

        public RequestTimeoutException(string message) : base(message)
        {
        }

        public RequestTimeoutException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}

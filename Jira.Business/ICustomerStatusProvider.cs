using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Jira.Models.Request;
using Jira.Models.Response;

namespace Jira.Business
{
    public interface ICustomerStatusProvider
    {
        Task<CustomerStatusResponse> GetCustomerStatus(List<string> projectKeys, DateTime date, Sprint sprint,
            decimal hoursReserved);
    }
}

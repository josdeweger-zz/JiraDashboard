using System;
using System.Collections.Generic;
using Jira.Api.Models.Jira;
using Jira.Api.Models.Request;
using Jira.Api.Models.Response;
using RestSharp;
using Sprint = Jira.Api.Models.Config.Sprint;

namespace Jira.Api.Business
{
    public interface ICustomerStatusProvider
    {
        IRestResponse GetWorklogsForProject(
            CustomerStatusRequest customerStatusRequest, 
            string projectKey, 
            Sprint sprint);

        CustomerStatusResponse CalculateCustomerStatusResponse(
            Sprint sprint, 
            DateTime date, 
            List<Worklog> worklogs,
            decimal hoursReserved);
    }
}

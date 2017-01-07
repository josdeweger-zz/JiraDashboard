using System;
using System.Collections.Generic;
using System.Linq;
using Jira.Api.Business.Clients;
using Jira.Api.Models.Config;
using Jira.Api.Models.Jira;
using Jira.Api.Models.Request;
using Jira.Api.Models.Response;
using RestSharp;
using Sprint = Jira.Api.Models.Config.Sprint;

namespace Jira.Api.Business
{
    public class CustomerStatusProvider : ICustomerStatusProvider
    {
        private readonly IConfig _config;
        private readonly IRestClientWrapper _jiraClient;

        public CustomerStatusProvider(IConfig config, IRestClientWrapper jiraClient)
        {
            _config = config;
            _jiraClient = jiraClient;
        }

        public IRestResponse GetWorklogsForProject(CustomerStatusRequest customerStatusRequest, string projectKey, Sprint sprint)
        {
            return _jiraClient
                .WithBaseUrl(_config.RestApi)
                .ForResource(_config.WorklogResource)
                .WithSessionId(customerStatusRequest.Session.Name, customerStatusRequest.Session.Value)
                .WithQueryParam("projectKey", projectKey)
                .WithQueryParam("teamId", customerStatusRequest.TeamId.ToString())
                .WithQueryParam("dateFrom", sprint.Start.ToString("yyyy-MM-dd"))
                .WithQueryParam("dateTo", customerStatusRequest.Date.Value.ToString("yyyy-MM-dd"))
                .ExecuteAsync()
                .Result;
        }

        public CustomerStatusResponse CalculateCustomerStatusResponse(
            Sprint sprint, 
            DateTime date, 
            List<Worklog> worklogs, 
            decimal hoursReserved)
        {
            decimal hoursLogged = worklogs.Any() ? worklogs.Sum(y => y.TimeSpentSeconds)/3600m : 0;
            decimal hoursExpected = LoggedHoursValueCalculator.CalculateHoursExpected(sprint, date, hoursReserved);

            var customerStatus = new CustomerStatusResponse()
            {
                HoursReserved = hoursReserved,
                TotalHours = hoursLogged,
                HoursExpected = Math.Round(hoursExpected),
                LoggedHoursValue = LoggedHoursValueCalculator.CalculateLoggedHours(
                                    sprint,
                                    date,
                                    hoursReserved,
                                    hoursLogged,
                                    _config.FaultMarginPercentage)
            };

            customerStatus.Percentage = customerStatus.HoursReserved > 0
                                            ? Math.Round(customerStatus.TotalHours/customerStatus.HoursReserved*100)
                                            : 0;

            return customerStatus;
        }
    }
}

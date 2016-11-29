using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Jira.Api.Business.Clients;
using Jira.Api.Models.Config;
using Jira.Api.Models.Jira;
using Jira.Api.Models.Request;
using Jira.Api.Models.Response;
using Sprint = Jira.Api.Models.Config.Sprint;

namespace Jira.Api.Business
{
    public class CustomerStatusProvider : ICustomerStatusProvider
    {
        private readonly IConfig _config;
        private readonly IJiraClient _jiraClient;
        private readonly Credentials _credentials;

        public CustomerStatusProvider(IConfig config, IJiraClient jiraClient)
        {
            _config = config;
            _jiraClient = jiraClient;

            _credentials = new Credentials(_config.Username, _config.Password);
        }

        public CustomerStatusResponse GetCustomerStatus(CustomerStatusRequest customerStatusRequest)
        {
            var sprint = new Sprint(customerStatusRequest.Sprint.Start, customerStatusRequest.Sprint.End);
            var worklogs = new List<Worklog>();

            //per project
            foreach (var projectKey in customerStatusRequest.ProjectKeys)
            {
                //get all worklogs for this project between sprint start and end
                worklogs.AddRange(
                    _jiraClient
                        .WithBaseUrl(_config.RestApi)
                        .ForResource(_config.WorklogResource)
                        .UsingCredentials(_credentials)
                        .WithQueryParam("projectKey", projectKey)
                        .WithQueryParam("teamId", customerStatusRequest.TeamId.ToString())
                        .WithQueryParam("dateFrom", sprint.Start.ToString("yyyy-MM-dd"))
                        .WithQueryParam("dateTo", customerStatusRequest.Date.Value.ToString("yyyy-MM-dd"))
                        .ExecuteAsync<List<Worklog>>()
                        .Result);
            }

            //calculate hour status
            return CustomerStatusFromWorklogs(sprint, customerStatusRequest.Date.Value, worklogs,
                customerStatusRequest.HoursReserved.Value);
        }

        private CustomerStatusResponse CustomerStatusFromWorklogs(Sprint sprint, DateTime date, List<Worklog> worklogs, decimal hoursReserved)
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Jira.Business.Clients;
using Jira.Models.Config;
using Jira.Models.Jira;
using Jira.Models.Response;

namespace Jira.Business
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

        public async Task<CustomerStatusResponse> GetCustomerStatus(List<string> projectKeys, DateTime date,
            Models.Request.Sprint sprintRequest, decimal hoursReserved)
        {
            var sprint = new Sprint(sprintRequest.Start, sprintRequest.End);
            var worklogs = new List<Worklog>();

            //per project
            foreach (var projectKey in projectKeys)
            {
                //get all worklogs for this project between sprint start and end
                worklogs.AddRange(
                    await _jiraClient
                        .WithBaseUrl(_config.RestApi)
                        .ForResource(_config.WorklogResource)
                        .UsingCredentials(_credentials)
                        .WithQueryParam("projectKey", projectKey)
                        .WithQueryParam("dateFrom", sprint.Start.ToString("yyyy-MM-dd"))
                        .WithQueryParam("dateTo", date.ToString("yyyy-MM-dd"))
                        .ExecuteAsync<List<Worklog>>());
            }

            //calculate hour status
            return CustomerStatusFromWorklogs(sprint, date, worklogs, hoursReserved);
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

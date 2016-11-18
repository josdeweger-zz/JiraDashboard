using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Jira.Business;
using Jira.Business.Clients;
using Jira.Models.Config;
using Jira.Models.DTO;
using Jira.Models.Jira;

namespace Jira.Web.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IWorklogClient _worklogClient;
        private readonly Config _config;
        private readonly Credentials _credentials;
        private readonly DateTime _date = new DateTime(2016, 10, 11);
        private readonly Sprint _currentSprint = new Sprint(new DateTime(2016, 9, 29), new DateTime(2016, 10, 12));
        private readonly int _faultMargin = 50;

        public DashboardController(IWorklogClient worklogClient, IConfigProvider<Config> configProvider)
        {
            _worklogClient = worklogClient;
            _config = configProvider.Get();
            _credentials = new Credentials(_config.JiraApi.Username, _config.JiraApi.Password);
        }

        public ActionResult Index()
        {
            var worklogs =
                _worklogClient
                    .WithBaseUrl(_config.JiraApi.RestApi)
                    .WithCredentials(_credentials)
                    .FilterByTeamId(_config.Team.Id)
                    .StartingFrom(_currentSprint.Start)
                    .Ending(_currentSprint.End)
                    .Get();

            var hoursPerCustomerList =
                _config.Customers
                    .Select(customer => CustomerViewModelFromWorklogs(worklogs, customer))
                    .OrderBy(c => c.CustomerName)
                    .ToList();

            var dashboardViewModel = new CustomerStatusDto()
            {
                Sprints = _config.Sprints,
                HoursPerCustomer = hoursPerCustomerList
            };

            return View(dashboardViewModel);
        }

        private HoursPerCustomerDto CustomerViewModelFromWorklogs(List<Worklog> worklogs, Customer customer)
        {
            var worklogsForCustomer =
                worklogs.Where(w => customer.Projects.Select(p => p.Id).Contains(w.Issue.ProjectId));

            decimal hoursLogged = worklogsForCustomer.Any() ? worklogsForCustomer.Sum(y => y.TimeSpentSeconds)/3600m : 0;
            
            var sprint = _config.Sprints.First(s => _date >= s.Start && _date <= s.End);
            
            var hoursPerCustomerVm = new HoursPerCustomerDto()
            {
                CustomerName = customer.Name,
                ProjectIds = customer.Projects.Select(p => p.Id).ToArray(),
                ProjectKeys = customer.Projects.Select(p => p.Key).ToArray(),
                HoursReserved = customer.HoursReserved,
                TotalHours = hoursLogged,
                LoggedHoursValue = LoggedHoursValueCalculator.CalculateLoggedHours(
                                    _currentSprint.IsDateInSprint(_date),
                                    customer.HoursReserved,
                                    sprint.NrOfDayInSprint(_date),
                                    sprint.TotalNrOfDaysInSprint,
                                    hoursLogged,
                                    _faultMargin)
            };

            hoursPerCustomerVm.Percentage =
                Math.Round(hoursPerCustomerVm.TotalHours/hoursPerCustomerVm.HoursReserved*100);

            return hoursPerCustomerVm;
        }
    }
}
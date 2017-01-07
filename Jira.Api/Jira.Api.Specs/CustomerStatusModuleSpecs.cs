using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Jira.Api.Business;
using Jira.Api.Models;
using Jira.Api.Models.Jira;
using Jira.Api.Models.Request;
using Jira.Api.Models.Response;
using Moq;
using Nancy.Testing;
using Xunit;
using Jira.Api.Modules;
using Newtonsoft.Json;
using RestSharp;
using HttpStatusCode = Nancy.HttpStatusCode;

namespace Jira.Api.Specs
{
    public class CustomerStatusModuleSpecs
    {
        [Fact]
        public async void GettingCustomerStatusShouldReturnStatusCodeOk()
        {
            var worklogResponse = new RestResponse<List<Worklog>>()
            {
                Content = JsonConvert.SerializeObject(new List<Worklog>()),
                StatusCode = System.Net.HttpStatusCode.OK,
                ResponseStatus = ResponseStatus.Completed
            };

            var customerStatusResponse = new CustomerStatusResponse()
            {
                HoursExpected = 15,
                HoursReserved = 30,
                LoggedHoursValue = LoggedHoursValue.Positive,
                Percentage = 50,
                TotalHours = 15
            };

            var browser = SetupRequest(worklogResponse, customerStatusResponse);

            var browserResponse = await browser.Post("/customer/status", with =>
            {
                var customerStatusRequest = new CustomerStatusRequest()
                {
                    TeamId = 24,
                    Date = new DateTime(2016, 1, 15),
                    Sprint =
                        new Sprint {Start = new DateTime(2016, 1, 12), End = new DateTime(2016, 1, 26)},
                    ProjectKeys = new List<string>() {"KEY"},
                    HoursReserved = 20
                };

                with.JsonBody(customerStatusRequest);
                with.Header("Accept", "application/json");
                with.HttpRequest();
            });
            
            browserResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async void GettingCustomerStatusShouldReturnCustomerStatusRequestObject()
        {
            var worklogResponse = new RestResponse<List<Worklog>>()
            {
                Content = JsonConvert.SerializeObject(new List<Worklog>()),
                StatusCode = System.Net.HttpStatusCode.OK,
                ResponseStatus = ResponseStatus.Completed
            };

            var customerStatusResponse = new CustomerStatusResponse()
            {
                HoursExpected = 15,
                HoursReserved = 30,
                LoggedHoursValue = LoggedHoursValue.Positive,
                Percentage = 50,
                TotalHours = 15
            };

            var browser = SetupRequest(worklogResponse, customerStatusResponse);

            var browserResponse = await browser.Post("/customer/status", with =>
            {
                var customerStatusRequest = new CustomerStatusRequest()
                {
                    TeamId = 24,
                    Date = new DateTime(2016, 1, 19),
                    Sprint =
                        new Sprint { Start = new DateTime(2016, 1, 12), End = new DateTime(2016, 1, 26) },
                    ProjectKeys = new List<string>() { "KEY" },
                    HoursReserved = 30
                };

                with.JsonBody(customerStatusRequest);
                with.Header("Accept", "application/json");
                with.HttpRequest();
            });
            
            browserResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var body = browserResponse.Body.DeserializeJson<CustomerStatusResponse>();
            body.HoursReserved.Should().Be(30);
            body.HoursExpected.Should().Be(15);
            body.LoggedHoursValue.Should().Be(LoggedHoursValue.Positive);
            body.Percentage.Should().Be(50);
            body.TotalHours.Should().Be(15);
        }

        [Fact]
        public async void GettingCustomerStatusWithEmptyTeamIdShouldReturnBadRequest()
        {
            var browser = SetupRequest(new RestResponse<List<Worklog>>(), null);

            var browserResponse = await browser.Post("/customer/status", with =>
            {
                var customerStatusRequest = new CustomerStatusRequest()
                {
                    Sprint =
                        new Sprint { Start = new DateTime(2016, 1, 12), End = new DateTime(2016, 1, 26) },
                    ProjectKeys = new List<string>() { "KEY" },
                    HoursReserved = 20
                };

                with.JsonBody(customerStatusRequest);
                with.Header("Accept", "application/json");
                with.HttpRequest();
            });

            browserResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var body = browserResponse.Body.DeserializeJson<ValidationFailedResponse>();
            body.Errors.First().Key.Should().Be(nameof(CustomerStatusRequest.TeamId));
        }

        [Fact]
        public async void GettingCustomerStatusWithEmptyProjectKeysShouldReturnBadRequest()
        {
            var browser = SetupRequest(new RestResponse<List<Worklog>>(), null);

            var browserResponse = await browser.Post("/customer/status", with =>
            {
                var customerStatusRequest = new CustomerStatusRequest()
                {
                    TeamId = 24,
                    Date = new DateTime(2016, 1, 15),
                    Sprint =
                        new Sprint { Start = new DateTime(2016, 1, 12), End = new DateTime(2016, 1, 26) },
                    ProjectKeys = new List<string>(),
                    HoursReserved = 20
                };

                with.JsonBody(customerStatusRequest);
                with.Header("Accept", "application/json");
                with.HttpRequest();
            });

            browserResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var body = browserResponse.Body.DeserializeJson<ValidationFailedResponse>();
            body.Errors.First().Key.Should().Be("ProjectKeys");
        }

        [Fact]
        public async void GettingCustomerStatusWithEmptyDateShouldReturnBadRequest()
        {
            var browser = SetupRequest(new RestResponse<List<Worklog>>(), null);

            var browserResponse = await browser.Post("/customer/status", with =>
            {
                var customerStatusRequest = new CustomerStatusRequest()
                {
                    TeamId = 24,
                    Sprint =
                        new Sprint { Start = new DateTime(2016, 1, 12), End = new DateTime(2016, 1, 26) },
                    ProjectKeys = new List<string>() { "KEY" },
                    HoursReserved = 20
                };

                with.JsonBody(customerStatusRequest);
                with.Header("Accept", "application/json");
                with.HttpRequest();
            });

            browserResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var body = browserResponse.Body.DeserializeJson<ValidationFailedResponse>();
            body.Errors.First().Key.Should().Be("Date");
        }

        [Fact]
        public async void GettingCustomerStatusWithSprintStartGreaterThanSprintEndShouldReturnBadRequest()
        {
            var browser = SetupRequest(new RestResponse<List<Worklog>>(), null);

            var browserResponse = await browser.Post("/customer/status", with =>
            {
                var customerStatusRequest = new CustomerStatusRequest()
                {
                    TeamId = 24,
                    Date = new DateTime(2016, 1, 16),
                    Sprint =
                        new Sprint { Start = new DateTime(2016, 1, 16), End = new DateTime(2016, 1, 11) },
                    ProjectKeys = new List<string>() { "KEY" },
                    HoursReserved = 20
                };

                with.JsonBody(customerStatusRequest);
                with.Header("Accept", "application/json");
                with.HttpRequest();
            });

            browserResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var body = browserResponse.Body.DeserializeJson<ValidationFailedResponse>();
            body.Errors.Should().Contain(e => e.Key == "Sprint.Start");
        }

        [Fact]
        public async void GettingCustomerStatusWithSelectedDateNotInSprintShouldReturnBadRequest()
        {
            var browser = SetupRequest(new RestResponse<List<Worklog>>(), null);

            var browserResponse = await browser.Post("/customer/status", with =>
            {
                var customerStatusRequest = new CustomerStatusRequest()
                {
                    TeamId = 24,
                    Date = new DateTime(2016, 1, 15),
                    Sprint =
                        new Sprint { Start = new DateTime(2016, 1, 16), End = new DateTime(2016, 1, 28) },
                    ProjectKeys = new List<string>() { "KEY" },
                    HoursReserved = 20
                };

                with.JsonBody(customerStatusRequest);
                with.Header("Accept", "application/json");
                with.HttpRequest();
            });

            browserResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var body = browserResponse.Body.DeserializeJson<ValidationFailedResponse>();
            body.Errors.First().Key.Should().Be("Date");
        }

        private Browser SetupRequest<T>(RestResponse<T> worklogResponse, CustomerStatusResponse customerStatusResponse)
        {
            var customerStatusProviderMock = new Mock<ICustomerStatusProvider>();
            customerStatusProviderMock.Setup(
                    c => c.GetWorklogsForProject(
                        It.IsAny<CustomerStatusRequest>(),
                        It.IsAny<string>(),
                        It.IsAny<Models.Config.Sprint>()))
                .Returns(worklogResponse);

            customerStatusProviderMock.Setup(
                    c => c.CalculateCustomerStatusResponse(
                        It.IsAny<Models.Config.Sprint>(),
                        It.IsAny<DateTime>(),
                        It.IsAny<List<Worklog>>(),
                        It.IsAny<decimal>()))
                .Returns(customerStatusResponse);

            var bootstrapper = new ConfigurableBootstrapper(with =>
            {
                with.Dependency(customerStatusProviderMock.Object);
                with.Module<CustomerStatusModule>();
            });

            return new Browser(bootstrapper);
        }
    }
}

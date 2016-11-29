using System;
using System.Collections.Generic;
using FluentAssertions;
using Jira.Api.Business;
using Jira.Api.Models;
using Jira.Api.Models.Request;
using Jira.Api.Models.Response;
using Moq;
using Nancy;
using Nancy.Testing;
using Xunit;
using Jira.Api.Modules;

namespace Jira.Api.Specs
{
    public class CustomerStatusModuleSpecs
    {
        private readonly Mock<ICustomerStatusProvider> _customerStatusProviderMock;
        private readonly Browser _browser;

        public CustomerStatusModuleSpecs()
        {
            _customerStatusProviderMock = new Mock<ICustomerStatusProvider>();
            _customerStatusProviderMock.Setup(
                c =>
                    c.GetCustomerStatus(It.IsAny<CustomerStatusRequest>()))
                .Returns(new CustomerStatusResponse()
                {
                    HoursExpected = 15,
                    HoursReserved = 30,
                    LoggedHoursValue = LoggedHoursValue.Positive,
                    Percentage = 50,
                    TotalHours = 15
                });
            
            var bootstrapper = new ConfigurableBootstrapper(with =>
            {
                with.Dependency(_customerStatusProviderMock.Object);
                with.Module<CustomerStatusModule>();
            });

            _browser = new Browser(bootstrapper);
        }

        [Fact]
        public async void GettingCustomerStatusShouldReturnStatusCodeOk()
        {
            var browserResponse = await _browser.Post("/customer/status", with =>
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
            var browserResponse = await _browser.Post("/customer/status", with =>
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
            var browserResponse = await _browser.Post("/customer/status", with =>
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
        }

        [Fact]
        public async void GettingCustomerStatusWithEmptyDateShouldReturnBadRequest()
        {
            var browserResponse = await _browser.Post("/customer/status", with =>
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
        }

        [Fact]
        public async void GettingCustomerStatusWithEmptyProjectKeysShouldReturnBadRequest()
        {
            var browserResponse = await _browser.Post("/customer/status", with =>
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
        }
    }
}

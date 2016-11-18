using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Jira.Api.Modules;
using Jira.Business;
using Jira.Models;
using Jira.Models.Request;
using Jira.Models.Response;
using Moq;
using Nancy;
using Nancy.Testing;
using Xunit;

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
                    c.GetCustomerStatus(It.IsAny<List<string>>(), It.IsAny<DateTime>(), It.IsAny<Sprint>(),
                        It.IsAny<decimal>()))
                .Returns(Task.FromResult(new CustomerStatusResponse()
                {
                    HoursExpected = 15,
                    HoursReserved = 30,
                    LoggedHoursValue = LoggedHoursValue.Positive,
                    Percentage = 50,
                    TotalHours = 15
                }));

            _browser = new Browser(with =>
            {
                with.Dependency(_customerStatusProviderMock.Object);
                with.Module<CustomerStatusModule>();
            });
        }

        [Fact]
        public void GettingCustomerStatusShouldReturnStatusCodeOk()
        {
            var result = _browser.Post("/customer/status", with =>
            {
                var customerStatusRequest = new CustomerStatusRequest()
                {
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

            result.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public void GettingCustomerStatusShouldReturnCustomerStatusRequestObject()
        {
            var result = _browser.Post("/customer/status", with =>
            {
                var customerStatusRequest = new CustomerStatusRequest()
                {
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

            var response = result.Body.DeserializeJson<CustomerStatusResponse>();
            response.HoursReserved.Should().Be(30);
            response.HoursExpected.Should().Be(15);
            response.LoggedHoursValue.Should().Be(LoggedHoursValue.Positive);
            response.Percentage.Should().Be(50);
            response.TotalHours.Should().Be(15);
        }

        [Fact]
        public void GettingCustomerStatusWithEmptyDateShouldReturnBadRequest()
        {
            var result = _browser.Post("/customer/status", with =>
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

            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public void GettingCustomerStatusWithEmptyProjectKeysShouldReturnBadRequest()
        {
            var result = _browser.Post("/customer/status", with =>
            {
                var customerStatusRequest = new CustomerStatusRequest()
                {
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

            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}

using System;
using System.Collections.Generic;
using FluentAssertions;
using Jira.Api.Business;
using Jira.Api.Models;
using Jira.Api.Models.Jira;
using Jira.Api.Models.Request;
using Jira.Api.Models.Response;
using Jira.Api.Modules;
using Moq;
using Nancy;
using Nancy.Testing;
using Newtonsoft.Json;
using RestSharp;
using Xunit;

namespace Jira.Api.Specs
{
    public class AuthenticationModuleSpecs
    {
        private const string JSessionName = "JSessionName";
        private const string JSessionValue = "12345";

        [Fact]
        public async void AuthenticatingSuccessfullyShouldReturnSessionObject()
        {
            var response = new RestResponse<AuthenticationSuccessResponse>()
            {
                Content = JsonConvert.SerializeObject(new AuthenticationSuccessResponse()
                {
                    Session = new Session() {Name = JSessionName, Value = JSessionValue}
                }),
                StatusCode = System.Net.HttpStatusCode.OK,
                ResponseStatus = ResponseStatus.Completed
            };

            var browser = SetupRequest(response);

            var browserResponse = await browser.Post("/authentication", with =>
            {
                var authenticationRequest = new AuthenticationRequest()
                {
                    Username = "SomeUsername",
                    Password = "SomePassword"
                };

                with.JsonBody(authenticationRequest);
                with.Header("Accept", "application/json");
                with.HttpRequest();
            });

            browserResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var body = browserResponse.Body.DeserializeJson<AuthenticationSuccessResponse>();
            body.Session.Name.Should().Be(JSessionName);
            body.Session.Value.Should().Be(JSessionValue);
        }

        [Fact]
        public async void AuthenticatingWithWrongUsernameShouldReturnUnauthorized()
        {
            var loginFailedMessage = "Login failed";
            var response = new RestResponse<AuthenticationFailedResponse>()
            {
                Content = JsonConvert.SerializeObject(new AuthenticationFailedResponse()
                {
                    Message = loginFailedMessage
                }),
                StatusCode = System.Net.HttpStatusCode.Unauthorized,
                ResponseStatus = ResponseStatus.Completed
            };

            var browser = SetupRequest(response);

            var browserResponse = await browser.Post("/authentication", with =>
            {
                var authenticationRequest = new AuthenticationRequest()
                {
                    Username = "SomeUsername",
                    Password = "SomePassword"
                };

                with.JsonBody(authenticationRequest);
                with.Header("Accept", "application/json");
                with.HttpRequest();
            });

            browserResponse.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

            var body = browserResponse.Body.DeserializeJson<AuthenticationFailedResponse>();
            body.Message.Should().Be(loginFailedMessage);
        }

        private Browser SetupRequest<T>(RestResponse<T> response)
        {
            var authenticationProviderMock = new Mock<IAuthenticationProvider>();
            authenticationProviderMock.Setup(c => c.Authenticate(It.IsAny<AuthenticationRequest>())).Returns(response);

            var bootstrapper = new ConfigurableBootstrapper(with =>
            {
                with.Dependency(authenticationProviderMock.Object);
                with.Module<AuthenticationModule>();
            });

            return new Browser(bootstrapper);
        }
    }
}
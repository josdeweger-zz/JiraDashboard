using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Jira.Api.Business.Clients;
using Jira.Api.Business.Stores;
using Jira.Api.Models.Config;
using Jira.Api.Models.Jira;
using Moq;
using RestSharp;
using Xunit;

namespace Jira.Api.Business.Specs
{
    public class RestClientWrapperSpecs
    {
        private readonly Mock<IConfig> _config;
        private readonly Mock<IRestClient> _restSharpClient;
        private KeyValuePair<string, string> _authCookie;
        private readonly Mock<ICookieStore> _cookieStore;

        public RestClientWrapperSpecs()
        {
            _config = new Mock<IConfig>();
            _config.Setup(c => c.AuthenticationResource).Returns(string.Empty);

            _authCookie = new KeyValuePair<string, string>("SessionName", "SessionValue");

            _restSharpClient = new Mock<IRestClient>();
            _restSharpClient.Setup(r => r.Execute<CookieAuthentication>(It.IsAny<IRestRequest>()))
                .Returns(new RestResponse<CookieAuthentication>()
                {
                    StatusCode = HttpStatusCode.OK,
                    ResponseStatus = ResponseStatus.Completed,
                    Data = new CookieAuthentication()
                    {
                        Session = new Session() { Name = _authCookie.Key, Value = _authCookie.Value }
                    }
                });

            _cookieStore = new Mock<ICookieStore>();
        }

        [Fact]
        public void WhenUsingTheClientWithoutUsingCredentialsTheCookieShouldNotBeStored()
        {
            _restSharpClient.Setup(r => r.Execute<object>(It.IsAny<IRestRequest>())).Returns(new RestResponse<object>());

            new RestClientWrapper(_config.Object, _restSharpClient.Object, _cookieStore.Object)
                .WithBaseUrl("http://localhost")
                .ForResource("some/resource")
                .ExecuteAsync<object>();

            _cookieStore.Verify(
                x => x.Store(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>()),
                Times.Never);
        }

        [Fact]
        public void WhenUsingCredentialsTheAuthenticationCookieShouldBeStoredInTheCookieStore()
        {
            _restSharpClient.Setup(r => r.Execute<object>(It.IsAny<IRestRequest>())).Returns(new RestResponse<object>());

            new RestClientWrapper(_config.Object, _restSharpClient.Object, _cookieStore.Object)
                .WithBaseUrl("http://localhost")
                .ForResource("some/resource")
                .UsingCredentials(new Credentials("username", "password"))
                .ExecuteAsync<CookieAuthentication>();

            _cookieStore.Verify(
                x => x.Store(It.IsAny<string>(), _authCookie.Key, _authCookie.Value, It.IsAny<DateTime>()), Times.Once);
        }

        [Fact]
        public void WhenTheClientTimesOutItShouldThrow()
        {
            var timeoutMessage = "The request timed out";

            _restSharpClient.Setup(c => c.ExecuteAsync<object>(
                Moq.It.IsAny<IRestRequest>(),
                Moq.It.IsAny<Action<IRestResponse<object>, RestRequestAsyncHandle>>()))
            .Callback<IRestRequest, Action<IRestResponse<object>, RestRequestAsyncHandle>>((request, callback) =>
            {
                var responseMock = new Mock<IRestResponse<object>>();
                responseMock.Setup(r => r.StatusCode).Returns(HttpStatusCode.RequestTimeout);
                responseMock.Setup(r => r.Content).Returns(timeoutMessage);
                callback(responseMock.Object, null);
            });

            Action result = () => new RestClientWrapper(_config.Object, _restSharpClient.Object, _cookieStore.Object)
                .WithBaseUrl("http://localhost")
                .ForResource("some/resource")
                .ExecuteAsync<object>();

            result.ShouldThrow<Exception>().WithMessage(timeoutMessage);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Net;
using Jira.Business.Clients;
using Jira.Business.Stores;
using Jira.Models.Config;
using Jira.Models.Jira;
using Moq;
using RestSharp;
using Xunit;

namespace Jira.Business.Specs
{
    public class JiraClientSpecs
    {
        private readonly Mock<IConfig> _config;
        private readonly Mock<IRestClient> _restSharpClient;
        private KeyValuePair<string, string> _authCookie;
        private readonly Mock<ICookieStore> _cookieStore;

        public JiraClientSpecs()
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
            _restSharpClient.Setup(r => r.Execute<object>(It.IsAny<IRestRequest>())).Returns(new RestResponse<object>());

            _cookieStore = new Mock<ICookieStore>();
        }

        [Fact]
        public void WhenUsingTheClientWithoutUsingCredentialsTheCookieShouldNotBeStored()
        {
            new JiraClient(_config.Object, _restSharpClient.Object, _cookieStore.Object)
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
            new JiraClient(_config.Object, _restSharpClient.Object, _cookieStore.Object)
                .WithBaseUrl("http://localhost")
                .ForResource("some/resource")
                .UsingCredentials(new Credentials("username", "password"))
                .ExecuteAsync<CookieAuthentication>();

            _cookieStore.Verify(
                x => x.Store(It.IsAny<string>(), _authCookie.Key, _authCookie.Value, It.IsAny<DateTime>()), Times.Once);
        }
    }
}

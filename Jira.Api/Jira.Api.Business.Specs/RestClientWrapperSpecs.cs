using System;
using System.Threading.Tasks;
using FluentAssertions;
using Jira.Api.Business.Clients;
using Jira.Api.Models.Exceptions;
using Moq;
using Nancy;
using RestSharp;
using Xunit;
using HttpStatusCode = System.Net.HttpStatusCode;

namespace Jira.Api.Business.Specs
{
    public class RestClientWrapperSpecs
    {
        private readonly Mock<IRestClient> _restSharpClient;

        public RestClientWrapperSpecs()
        {
            _restSharpClient = new Mock<IRestClient>();
        }

        [Fact]
        public void WhenTheClientTimesOutItShouldThrow()
        {
            var timeoutMessage = "The request timed out";
            _restSharpClient.Setup(c => c.ExecuteAsync(
                It.IsAny<IRestRequest>(),
                It.IsAny<Action<IRestResponse, RestRequestAsyncHandle>>()))
            .Callback<IRestRequest, Action<IRestResponse, RestRequestAsyncHandle>>((request, callback) =>
            {
                var responseMock = new Mock<IRestResponse>();
                responseMock.Setup(r => r.StatusCode).Returns(HttpStatusCode.RequestTimeout);
                responseMock.Setup(r => r.Content).Returns(timeoutMessage);
                callback(responseMock.Object, null);
            });

            Func<Task<IRestResponse>> result = 
                () => new RestClientWrapper(_restSharpClient.Object)
                    .WithBaseUrl("http://localhost")
                    .ForResource("some/resource")
                    .ExecuteAsync();
            
            result.ShouldThrow<AggregateException>()
                .WithInnerException<RequestTimeoutException>()
                .WithInnerMessage(timeoutMessage);
        }
    }
}

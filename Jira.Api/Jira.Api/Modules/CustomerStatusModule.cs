using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Jira.Api.Business;
using Jira.Api.Business.Extensions;
using Jira.Api.Models.Jira;
using Jira.Api.Models.Request;
using Jira.Api.Models.Response;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Validation;
using Newtonsoft.Json;
using Sprint = Jira.Api.Models.Config.Sprint;

namespace Jira.Api.Modules
{
    public sealed class CustomerStatusModule : NancyModule
    {
        public CustomerStatusModule(ICustomerStatusProvider customerStatusProvider) 
            : base("/customer")
        {
            Post("status", (ctx) =>
            {
                var customerStatusRequest = this.Bind<CustomerStatusRequest>();

                var validationResult = this.Validate(customerStatusRequest);

                if (!validationResult.IsValid)
                    return Negotiate.WithModel(validationResult).WithStatusCode(HttpStatusCode.BadRequest);

                var sprint = new Sprint(customerStatusRequest.Sprint.Start, customerStatusRequest.Sprint.End);
                var worklogs = new List<Worklog>();

                foreach (var projectKey in customerStatusRequest.ProjectKeys)
                {
                    var response = customerStatusProvider.GetWorklogsForProject(
                        customerStatusRequest,
                        projectKey,
                        sprint);

                    if (!response.IsSuccessful())
                    {
                        var failedResponse =
                            JsonConvert.DeserializeObject<CustomerStatusFailedResponse>(response.Content);

                        return Negotiate.WithModel(failedResponse).WithStatusCode((HttpStatusCode) response.StatusCode);
                    }

                    var successFullResponse = JsonConvert.DeserializeObject<List<Worklog>>(response.Content);
                    worklogs.AddRange(successFullResponse);
                }

                var customerStatusResponse = customerStatusProvider.CalculateCustomerStatusResponse(
                    sprint,
                    customerStatusRequest.Date.Value,
                    worklogs,
                    customerStatusRequest.HoursReserved.Value);

                return customerStatusResponse;
            });
        }
    }
}

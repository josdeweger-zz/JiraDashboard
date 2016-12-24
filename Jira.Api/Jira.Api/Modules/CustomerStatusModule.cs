using System;
using System.Threading.Tasks;
using Jira.Api.Business;
using Jira.Api.Models.Request;
using Jira.Api.Models.Response;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Validation;

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
                {
                    return Negotiate.WithModel(validationResult).WithStatusCode(HttpStatusCode.BadRequest);
                }

                var customerStatusResponse = customerStatusProvider.GetCustomerStatus(customerStatusRequest);

                return customerStatusResponse;
            });
        }
    }
}

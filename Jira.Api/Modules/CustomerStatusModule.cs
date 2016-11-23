using System;
using Jira.Business;
using Jira.Models.Request;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Validation;

namespace Jira.Api.Modules
{
    public class CustomerStatusModule : NancyModule
    {
        public CustomerStatusModule(ICustomerStatusProvider customerStatusProvider) 
            : base("/customer")
        {
            Post["/status", true] = async (x, ct) =>
            {
                var customerStatusRequest = this.Bind<CustomerStatusRequest>();

                var validationResult = this.Validate(customerStatusRequest);

                if (!validationResult.IsValid)
                {
                    return Negotiate.WithModel(validationResult).WithStatusCode(HttpStatusCode.BadRequest);
                }

                var customerStatusResponse = await customerStatusProvider.GetCustomerStatus(customerStatusRequest);

                return customerStatusResponse;
            };
        }
    }
}

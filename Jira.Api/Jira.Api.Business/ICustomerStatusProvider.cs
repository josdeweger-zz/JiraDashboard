using System.Threading.Tasks;
using Jira.Api.Models.Request;
using Jira.Api.Models.Response;

namespace Jira.Api.Business
{
    public interface ICustomerStatusProvider
    {
        CustomerStatusResponse GetCustomerStatus(CustomerStatusRequest customerStatusRequest);
    }
}

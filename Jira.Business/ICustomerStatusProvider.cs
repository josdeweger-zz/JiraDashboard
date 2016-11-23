using System.Threading.Tasks;
using Jira.Models.Request;
using Jira.Models.Response;

namespace Jira.Business
{
    public interface ICustomerStatusProvider
    {
        Task<CustomerStatusResponse> GetCustomerStatus(CustomerStatusRequest customerStatusRequest);
    }
}

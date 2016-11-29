using Jira.Api.Models.Config;

namespace Jira.Api.Business
{
    public interface IConfigProvider<T> where T : IConfig
    {
        T Get();
    }
}

using Jira.Models.Config;

namespace Jira.Business
{
    public interface IConfigProvider<T> where T : IConfig
    {
        T Get();
    }
}

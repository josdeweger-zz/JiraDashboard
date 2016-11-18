using System.IO;
using Jira.Models.Config;
using Newtonsoft.Json;

namespace Jira.Business
{
    public class ConfigProvider<T> : IConfigProvider<T> 
        where T : IConfig
    {
        private readonly T _config;

        public ConfigProvider(string configFilePath)
        {
            _config = JsonConvert.DeserializeObject<T>(File.ReadAllText(configFilePath));
        }

        public T Get()
        {
            return _config;
        }
    }
}

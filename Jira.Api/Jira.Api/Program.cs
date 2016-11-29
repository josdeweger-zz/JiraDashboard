using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace Jira.Api
{
    public class Program
    {
        private const string Url = "http://*";
        private const int Port = 3002;

        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseKestrel()
                .UseUrls($"{Url}:{Port}")
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}

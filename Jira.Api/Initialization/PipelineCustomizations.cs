using System;
using System.Text;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Extensions;
using Newtonsoft.Json;

namespace Jira.Api.Initialization
{
    public class PipelineCustomizations : IApplicationStartup
    {
        private readonly object _lockObject = new object();

        public void Initialize(IPipelines pipelines)
        {
            StaticConfiguration.DisableErrorTraces = false;

            pipelines.BeforeRequest.AddItemToStartOfPipeline(ctx =>
            {
                lock (_lockObject)
                {
                    WriteRequest(ctx);

                    if (ctx.Request.Method.Equals("POST", StringComparison.InvariantCultureIgnoreCase))
                        WriteBody(ctx);

                    Console.WriteLine();
                }

                return null;
            });

            pipelines.OnError.AddItemToEndOfPipeline((ctx, ex) =>
            {
                WriteError(ex);

                return null;
            });

            pipelines.AfterRequest.AddItemToEndOfPipeline(AddAccessControlHeaders);
        }

        private static void AddAccessControlHeaders(NancyContext ctx)
        {
            ctx.Response
                .WithHeader("Access-Control-Allow-Origin", "*")
                .WithHeader("Access-Control-Allow-Methods", "POST,GET")
                .WithHeader("Access-Control-Allow-Headers", "Origin, X-Requested-With, Content-Type, Accept");
        }

        private void WriteBody(NancyContext ctx)
        {
            Console.Write("With body:");
            Console.WriteLine();
            dynamic parsedJson = JsonConvert.DeserializeObject(ctx.Request.Body.AsString());
            Console.WriteLine(JsonConvert.SerializeObject(parsedJson, Formatting.Indented));
        }

        private void WriteRequest(NancyContext ctx)
        {
            Console.WriteLine($"{ctx.Request.Method} request to {ctx.Request.Url}");
        }

        private static void WriteError(Exception ex)
        {
            Console.WriteLine("An error occured.");
            Console.WriteLine($"Message: {ex.Message}");
            Console.WriteLine($"Inner Exception: {ex.InnerException}");
        }
    }
}
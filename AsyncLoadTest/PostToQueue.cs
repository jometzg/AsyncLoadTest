using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace AsyncLoadTest
{
    public static class PostToQueue
    {
        [FunctionName("PostToQueue")]
        [return: ServiceBus("%QueueName%", Connection = "ServiceBusConnection")]
        public static string Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] dynamic input,
            ILogger log)
        {
            log.LogInformation($"C# HTTP trigger with {input}");

            return input.ToString();
        }
    }
}

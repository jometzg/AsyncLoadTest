using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.Management.ServiceBus;
using Microsoft.Azure.ServiceBus.Management;
using Microsoft.ApplicationInsights;

namespace AsyncLoadTest
{
    public static class PollQueueLength
    {
        [FunctionName("PollQueueLength")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            
            ManagementClient managementClient = new ManagementClient(Environment.GetEnvironmentVariable("ServiceBusConnection"));
            var queue = await managementClient.GetQueueRuntimeInfoAsync(Environment.GetEnvironmentVariable("QueueName"));
            var messageCount = queue.MessageCount;

            return new OkObjectResult(messageCount);
        }
    }
}

using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Linq.Expressions;

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

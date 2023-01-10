using System;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus.Management;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace AsyncLoadTest
{
    public class ScheduledSetCustomMetric
    {
        private static string queueName = Environment.GetEnvironmentVariable("QueueName");
        private static string sbConnection = Environment.GetEnvironmentVariable("ServiceBusConnection");
        private static string metricName = Environment.GetEnvironmentVariable("MetricName");

        [FunctionName("ScheduledSetCustomMetric")]
        public async Task Run([TimerTrigger("%metricUpdatePattern%")]TimerInfo myTimer, ILogger log) // see https://arminreiter.com/2017/02/azure-functions-time-trigger-cron-cheat-sheet/
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            ManagementClient managementClient = new ManagementClient(sbConnection);
            var queue = await managementClient.GetQueueRuntimeInfoAsync(queueName);
            var messageCount = queue.MessageCount;
            log.LogMetric(metricName, messageCount);
        }
    }
}

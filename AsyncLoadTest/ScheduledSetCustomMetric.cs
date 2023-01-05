using System;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using Microsoft.Azure.ServiceBus.Management;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace AsyncLoadTest
{
    public class ScheduledSetCustomMetric
    {
        private static TelemetryClient _telemetryClient = new TelemetryClient();

        [FunctionName("ScheduledSetCustomMetric")]
        public async Task Run([TimerTrigger("* * * * * *")]TimerInfo myTimer, ILogger log) // every second. May be better every minute
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            ManagementClient managementClient = new ManagementClient(Environment.GetEnvironmentVariable("ServiceBusConnection"));
            var queue = await managementClient.GetQueueRuntimeInfoAsync(Environment.GetEnvironmentVariable("QueueName"));
            var messageCount = queue.MessageCount;
            _telemetryClient.TrackMetric(Environment.GetEnvironmentVariable("MetricName"), messageCount);
        }
    }
}

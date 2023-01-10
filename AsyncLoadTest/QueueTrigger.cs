using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;


namespace AsyncLoadTest
{
    public class QueueTrigger
    {
        [FunctionName("QueueTrigger")]
        [StorageAccount("TargetStorageAccount")]
        public async Task Run([ServiceBusTrigger("%QueueName%", Connection = "ServiceBusConnection")]string myQueueItem,
            [Blob("%outboundContainer%/{messageId}", FileAccess.Write)] Stream message,
            Int32 deliveryCount,
            DateTime enqueuedTimeUtc,
            string messageId,
            ILogger log
            )
        {
            // do some logging
            log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");
            log.LogInformation($"EnqueuedTimeUtc={enqueuedTimeUtc}");
            log.LogInformation($"DeliveryCount={deliveryCount}");
            log.LogInformation($"MessageId={messageId}");

            // copy to blob storage using messageId as name
            await GenerateStreamFromString(myQueueItem).CopyToAsync(message);

            // burn some cycles to simulate work
            string messageCopy = PoorStringCopy(myQueueItem);
            log.LogInformation($"Completed MessageId={messageId}");
        }

        // generate a stream version of the body of the message
        Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        // poor performing string copy :-)
        string PoorStringCopy(string source)
        {
            string target = "";
            for (int i = 0; i < source.Length; i++)
            {
                target = target + source[i];
                Thread.Sleep(int.Parse(Environment.GetEnvironmentVariable("WaitMs")));
            }
            return target;
        }
    }
}

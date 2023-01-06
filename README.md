# Async Load Test

This repository is an Azure Load Test that tests an asynchronous service. When testing asynchronous or background processes, it is hard to know when the background processing has completed.

In order to address this, this test:
1. Sends a fixed number of POST requests to an API
2. Uses a second phase of the load test to then poll another API to see how many items remain to be processed. Only when there are none, does the test complete.
3. Uses a custom metric in application insights to represent this queue lenrth.

In this way, the load test executes for as long as it takes to process all of the data and the load test can then pull in the custom metric to graph the items remaining to be processed into the load test results. Thus giving the tester a more complete picture of how long an asynchronous process takes to complete a given number of requests.

## Load Test Structure

![alt text](AsyncLoadTest/Images/load-test-in-jmeter.png "Load test structure.")

## Demonstration System Under Test

![alt text](AsyncLoadTest/Images/load-test-and-system-under-test.png "System under test.")

## Custom metric

![alt text](AsyncLoadTest/Images/backlogcount-metric.png "Custom metric.")

## Load Test Sample Run

![alt text](AsyncLoadTest/Images/load-test-results.png "Test results.")

## Function App Configuration Items needed

1. ServiceBusConnection - the connection string to Azure service bus instance
2. QueueName - service bus queue name
3. MetricName - the name of the application insights (configured for the function app) metric
4. outboundContainer - the name of the blob storage container in which the messages get put after pulling from the service bus queue
5. TargetStorageAccount" - the connection string for the above storage account
6. WaitMs - how many mS to wait in each letter of string copy (to simulate busyness)
7. metricUpdatePattern - CRON pattern for the timed function that sets the metric value from the queue length ("*/15 * * * * *" is every 15 seconds)

It may also be useful (for test purposes) to limit the scale out of the function as service bus is used by the function runtime to scale out to multiple instances - thus getting more work done in parallel. This is what is needed for a real workload, but for this demonstration, limiting the function to a few instances (in the scale out section) is useful to be more representative of a background processing activity.

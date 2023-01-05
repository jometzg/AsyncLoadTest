# Async Load Test

This repository is an Azure Load Test that tests an asynchronous service. When testing asynchronous or background processes, it is hard to know when the background processing has completed.

In order to address this, this test:
1. Sends a fixed number of POST requests to an API
2. Uses a second phase of the load test to then poll another API to see how many items remain to be processed. Only when there are none, does the test complete.
3. Uses a custom metric in application insights to represent this queue lenrth.

In this way, the load test executes for as long as it takes to process all of the data and the load test can then pull in the custom metric to graph the items remaining to be processed into the load test results. Thus giving the tester a more complete picture of how long an asynchronous process takes to complete a given number of requests.

## Load Test Structure

![alt text]("AsyncLoadTest/images/load-test-in-jmeter.png" "Load test structure.")

## Demonstration System Under Test

![alt text]("AsyncLoadTest/images/load test and system under test.png" "System under test.")

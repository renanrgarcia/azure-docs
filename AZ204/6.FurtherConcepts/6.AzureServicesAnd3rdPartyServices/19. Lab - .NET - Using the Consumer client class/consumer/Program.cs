using System.Text;
using Azure.Messaging.EventHubs.Consumer;

string eventHubConnectionString="";
string eventHubName="orders-events";

string consumerGroup = EventHubConsumerClient.DefaultConsumerGroupName;

await using var consumer = new EventHubConsumerClient(consumerGroup, eventHubConnectionString, eventHubName);
using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(45));
await foreach (PartitionEvent receivedEvent in consumer.ReadEventsAsync(startReadingAtEarliestEvent: true, cancellationToken: cts.Token))

{
    if (receivedEvent.Data is null) continue;

    Console.WriteLine($"Received event: {receivedEvent.Data.EventBody}");
}
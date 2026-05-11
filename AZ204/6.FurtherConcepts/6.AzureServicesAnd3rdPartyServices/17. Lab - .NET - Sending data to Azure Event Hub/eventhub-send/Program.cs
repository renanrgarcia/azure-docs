using System.Diagnostics.Tracing;
using System.Text;
using System.Text.Json;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;

string eventHubConnectionString="";
string eventHubName="orders-events";

await using var producer=new EventHubProducerClient(eventHubConnectionString,eventHubName);
using EventDataBatch batch = await producer.CreateBatchAsync();

for(int i=1;i<=10;i++)
{
var evt = new
{
    eventType = "OrderCreated",
    orderId=$"ORD-{i:0000}",
    customerId=$"CUST-{(i % 3) + 1:000}",
    total=49.99 + i,
    createdUtc = DateTime.UtcNow
};

string json=JsonSerializer.Serialize(evt);
EventData eventData=new EventData(Encoding.UTF8.GetBytes(json));
batch.TryAdd(eventData);
}

await producer.SendAsync(batch);
Console.WriteLine("Sent 10 events to Event Hubs.");

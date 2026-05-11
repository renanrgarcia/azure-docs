// Use the below code as as reference to create your own custom topic. You can change the name of the class and the topic name as needed.
// Add the package - dotnet add package Azure.Messaging.EventGrid
using Azure;
using Azure.Messaging;
using Azure.Messaging.EventGrid;

// 1) From your custom topic in Azure Portal
string topicEndpoint = "";
string topicKey = ""; // demo only - store in config/Key Vault in real apps

var credential = new AzureKeyCredential(topicKey);
var client = new EventGridPublisherClient(new Uri(topicEndpoint), credential);

var events = new List<EventGridEvent>();

for (int i = 1; i <= 3; i++)
{
    var payload = new
    {
        orderId = $"ORD-{i:0000}",
        customer = $"Customer {i:000}",
        course = "AZ-204: Azure Developer Associate",
        amount = 49.00,
        createdUtc = DateTimeOffset.UtcNow
    };

    var evt = new EventGridEvent(
        subject: $"/orders/ORD-{i:0000}",
        eventType: "CloudXeus.Orders.OrderCreated",
        dataVersion: "1.0",
        data: BinaryData.FromObjectAsJson(payload));

    events.Add(evt);
}

Console.WriteLine("Sending events to Event Grid custom topic...");
await client.SendEventsAsync(events);
Console.WriteLine("Done.");
using System.Text.Json;
using Azure.Messaging.ServiceBus;

string connectionString="";
string topicName="course-orders-topic";

var orders = new List<CourseOrder>
{
    new(12547, "Customer 001", "customer001@cloudxeus.com", "AZ-204: Azure Developer Associate", 49.00m, DateTimeOffset.Parse("2026-02-26T09:15:00Z")),
    new(12548, "Customer 002", "customer002@cloudxeus.com", "AZ-104: Azure Administrator",       39.00m, DateTimeOffset.Parse("2026-02-26T09:16:00Z")),
    new(12549, "Customer 003", "customer003@cloudxeus.com", "DP-600: Fabric Analytics Engineer", 59.00m, DateTimeOffset.Parse("2026-02-26T09:17:00Z"))
};

await using var client=new ServiceBusClient(connectionString);
ServiceBusSender sender = client.CreateSender(topicName);
foreach(var order in orders)
{
    string json = JsonSerializer.Serialize(order);
    var message=new ServiceBusMessage(json)
    {
         ContentType = "application/json",
         MessageId=order.OrderId.ToString()
    };
   
    await sender.SendMessageAsync(message);
    Console.WriteLine($"Sent order {order.OrderId} to the queue.");

}

public record CourseOrder(
    int OrderId,
    string CustomerName,
    string CustomerEmail,
    string CourseName,
    decimal Price,
    DateTimeOffset OrderedUtc
);

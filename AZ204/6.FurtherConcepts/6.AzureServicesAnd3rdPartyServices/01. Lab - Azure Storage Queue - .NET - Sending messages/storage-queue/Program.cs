using Azure.Storage.Queues;
using System.Text.Json;

string connectionString="";
string queueName="course-orders";

var queueClient=new QueueClient(connectionString,queueName);

var orders = new List<CourseOrder>
{
    new(12547, "Customer 001", "customer001@cloudxeus.com", "AZ-204: Azure Developer Associate", 49.00m, DateTimeOffset.Parse("2026-02-26T09:15:00Z")),
    new(12548, "Customer 002", "customer002@cloudxeus.com", "AZ-104: Azure Administrator",       39.00m, DateTimeOffset.Parse("2026-02-26T09:16:00Z")),
    new(12549, "Customer 003", "customer003@cloudxeus.com", "DP-600: Fabric Analytics Engineer", 59.00m, DateTimeOffset.Parse("2026-02-26T09:17:00Z"))
};

static async Task SendOrders(QueueClient queueClient,IEnumerable<CourseOrder> orders)
{
    var jsonOptions = new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false
    };
    foreach(var order in orders)
    {
        string json = JsonSerializer.Serialize(order, jsonOptions);
        await queueClient.SendMessageAsync(json);
        Console.WriteLine($"Sent orderId={order.OrderId}");
    }
}

await SendOrders(queueClient, orders);

public sealed record CourseOrder(
    int OrderId,
    string CustomerName,
    string CustomerEmail,
    string CourseName,
    decimal Amount,
    DateTimeOffset OrderDateUtc
);


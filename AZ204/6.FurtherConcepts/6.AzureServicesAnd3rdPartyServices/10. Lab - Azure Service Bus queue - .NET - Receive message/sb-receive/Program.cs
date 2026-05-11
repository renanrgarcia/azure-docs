using Azure.Messaging.ServiceBus;
using System.Text.Json;

string connectionString="";
string queueName="course-orders";


static async Task PeekMessages(string connectionString,string queueName,int maxMessages)
{
   await using var client = new ServiceBusClient(connectionString);
    ServiceBusReceiver receiver = client.CreateReceiver(queueName);
    IReadOnlyList<ServiceBusReceivedMessage> messages = await receiver.PeekMessagesAsync(maxMessages: maxMessages);

    foreach(var msg in messages)
    {
        string body=msg.Body.ToString();
        Console.WriteLine($"Message: {body}");
    }
}
//await PeekMessages(connectionString,queueName,maxMessages: 10);

static async Task ReceiveMessages(string connectionString,string queueName,int maxMessages)
{
    await using var client = new ServiceBusClient(connectionString);
    ServiceBusReceiver receiver = client.CreateReceiver(queueName,
        new ServiceBusReceiverOptions {ReceiveMode = ServiceBusReceiveMode.PeekLock}
    );

    IReadOnlyList<ServiceBusReceivedMessage> messages =
        await receiver.ReceiveMessagesAsync(maxMessages: maxMessages, maxWaitTime: TimeSpan.FromSeconds(5));

    foreach(var msg in messages)
    {
        string body = msg.Body.ToString();
        CourseOrder? order = JsonSerializer.Deserialize<CourseOrder>(body);
        Console.WriteLine($"Received order {order.OrderId} for course {order.CourseName} from {order.CustomerName}");
        await receiver.CompleteMessageAsync(msg);
    }

}

await ReceiveMessages(connectionString, queueName, maxMessages: 10);

public record CourseOrder(
    int OrderId,
    string CustomerName,
    string CustomerEmail,
    string CourseName,
    decimal Price,
    DateTimeOffset OrderedUtc
);
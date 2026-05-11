using Azure.Messaging.ServiceBus;

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
await PeekMessages(connectionString,queueName,maxMessages: 10);
using System.Text.Json;
using Azure.Messaging.ServiceBus;

const string connectionString = "";
const string queueName = "course-orders";

await using var client = new ServiceBusClient(connectionString);
ServiceBusProcessor processor = client.CreateProcessor(queueName,new ServiceBusProcessorOptions
{
    AutoCompleteMessages = false,
    MaxConcurrentCalls = 1
});

processor.ProcessMessageAsync += async args =>
{
    string json = args.Message.Body.ToString();
    CourseOrder? order = JsonSerializer.Deserialize<CourseOrder>(json);
    Console.WriteLine($"Processing OrderId={order?.OrderId}, Course={order?.CourseName}, Price={order?.Price}");
    await args.CompleteMessageAsync(args.Message);
};

processor.ProcessErrorAsync += args =>
    {
        Console.WriteLine($"Error Source: {args.ErrorSource}");
        Console.WriteLine($"Exception: {args.Exception.Message}");
        return Task.CompletedTask;
    };

await processor.StartProcessingAsync();
 Console.WriteLine("Processor started. Press ENTER to stop...");
    Console.ReadLine();

await processor.StopProcessingAsync();
    await processor.DisposeAsync();
public record CourseOrder(
    int OrderId,
    string CustomerName,
    string CustomerEmail,
    string CourseName,
    decimal Price,
    DateTimeOffset OrderedUtc
);
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Company.Function;

public class blob_trigger
{
    private readonly ILogger<blob_trigger> _logger;

    public blob_trigger(ILogger<blob_trigger> logger)
    {
        _logger = logger;
    }

    [Function(nameof(blob_trigger))]
    [CosmosDBOutput(
        databaseName: "appdb",
        containerName: "orders",
        Connection = "CosmosDbConnection")]
    public async Task<OrderDocument?> Run([BlobTrigger("incoming-orders/{name}", Connection = "stdeveus10_STORAGE")] Stream stream, string name)
    {
        using var reader = new StreamReader(stream);
        string blobContent = await reader.ReadToEndAsync();

         OrderFile? order;
        order = JsonSerializer.Deserialize<OrderFile>(
                blobContent,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

        var document = new OrderDocument
        {
            id = order.OrderId,
            orderId = order.OrderId,
            customerId = order.CustomerId,
            customerName = order.CustomerName,
            courseId = order.CourseId,
            courseName = order.CourseName,
            amount = order.Amount,
            currency = order.Currency,
            status = order.Status,
            sourceBlobName = name,
            processedOnUtc = DateTime.UtcNow
        };
        return document;

    }
}

public class OrderFile
{
    public string OrderId { get; set; } = default!;
    public string CustomerId { get; set; } = default!;
    public string CustomerName { get; set; } = default!;
    public string CourseId { get; set; } = default!;
    public string CourseName { get; set; } = default!;
    public decimal Amount { get; set; }
    public string Currency { get; set; } = default!;
    public string Status { get; set; } = default!;
}

public class OrderDocument
{
    public string id { get; set; } = default!;
    public string orderId { get; set; } = default!;
    public string customerId { get; set; } = default!;
    public string customerName { get; set; } = default!;
    public string courseId { get; set; } = default!;
    public string courseName { get; set; } = default!;
    public decimal amount { get; set; }
    public string currency { get; set; } = default!;
    public string status { get; set; } = default!;
    public string sourceBlobName { get; set; } = default!;
    public DateTime processedOnUtc { get; set; }
}
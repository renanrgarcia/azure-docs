using System;
using System.Text.Json;
using Azure.Storage.Queues.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Company.Function;

public class stqueue
{
    private readonly ILogger<stqueue> _logger;

    public stqueue(ILogger<stqueue> logger)
    {
        _logger = logger;
    }

    [Function(nameof(stqueue))]
    public void Run([QueueTrigger("course-orders", Connection = "stdeveus10_STORAGE")] QueueMessage message)
    {
        CourseOrder? order;
         order = JsonSerializer.Deserialize<CourseOrder>(
                message.MessageText,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                 _logger.LogInformation(
                    order.OrderId,
            order.CustomerEmail,
            order.CourseName,
            order.Amount,
            order.OrderDateUtc
                 );
    }

     private sealed record CourseOrder(
        int OrderId,
        string CustomerName,
        string CustomerEmail,
        string CourseName,
        decimal Amount,
        DateTimeOffset OrderDateUtc
    );
}
using System;
using System.Text.Json;
using Azure.Data.Tables;
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
    [TableOutput("orders",Connection = "stdeveus10_STORAGE")]
    public TableEntity Run([QueueTrigger("course-orders", Connection = "stdeveus10_STORAGE")] CourseOrder order)
    {
        var entity=new TableEntity(
            partitionKey:order.CourseName,
            rowKey:order.OrderId.ToString()
        )
        {
            ["CustomerName"] = order.CustomerName,
            ["CustomerEmail"] = order.CustomerEmail,
            ["Amount"] = order.Amount,
            ["OrderDateUtc"] = order.OrderDateUtc.UtcDateTime
        };

        return entity;
    }

     public sealed record CourseOrder(
        int OrderId,
        string CustomerName,
        string CustomerEmail,
        string CourseName,
        decimal Amount,
        DateTimeOffset OrderDateUtc
    );
}
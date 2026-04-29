using System;
using System.Collections.Generic;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Company.Function;

public class changefeed
{
    private readonly ILogger<changefeed> _logger;

    public changefeed(ILogger<changefeed> logger)
    {
        _logger = logger;
    }

    [Function("changefeed")]
    public void Run([CosmosDBTrigger(
        databaseName: "orders-db",
        containerName: "orders",
        Connection = "appcosmosdb01_DOCUMENTDB",
        LeaseContainerName = "leases",
        CreateLeaseContainerIfNotExists = true)] IReadOnlyList<Order> input)
    {
        if (input != null && input.Count > 0)
        {
            _logger.LogInformation("Documents modified: " + input.Count);
            _logger.LogInformation("First document Id: " + input[0].id);
        }
    }
}

public class Order
{
    public string id { get; set; } = default!;
    public string customerId { get; set; } = default!;
    public string type { get; set; } = default!;
    public double total { get; set; }
}
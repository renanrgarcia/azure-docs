// Default URL for triggering event grid function in the local environment.
// http://localhost:7071/runtime/webhooks/EventGrid?functionName={functionname}

using System;
using Azure.Messaging;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Company.Function;

public class evt_trigger
{
    private readonly ILogger<evt_trigger> _logger;

    public evt_trigger(ILogger<evt_trigger> logger)
    {
        _logger = logger;
    }

    [Function(nameof(evt_trigger))]
    public void Run([EventGridTrigger] CloudEvent cloudEvent)
    {
        // CloudEvents envelope (schema properties)
        _logger.LogInformation("Id: {id}", cloudEvent.Id);
        _logger.LogInformation("Type: {type}", cloudEvent.Type);
        _logger.LogInformation("Source: {source}", cloudEvent.Source);
        _logger.LogInformation("Subject: {subject}", cloudEvent.Subject);
        _logger.LogInformation("Time: {time}", cloudEvent.Time);
        _logger.LogInformation("DataContentType: {ct}", cloudEvent.DataContentType);
        _logger.LogInformation("DataSchema: {schema}", cloudEvent.DataSchema);

        // Event payload (service-specific details)
        _logger.LogInformation("Data: {data}", cloudEvent.Data?.ToString());
    }
}
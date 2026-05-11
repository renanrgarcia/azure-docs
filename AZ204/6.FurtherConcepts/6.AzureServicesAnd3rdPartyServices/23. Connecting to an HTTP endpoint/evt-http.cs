using Azure.Messaging.EventGrid;
using Azure.Messaging.EventGrid.SystemEvents;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Company.Function;

public class evt_http
{
    private readonly ILogger<evt_http> _logger;

    public evt_http(ILogger<evt_http> logger)
    {
        _logger = logger;
    }

    [Function("evt_http")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
    {
         {
        // OPTIONS preflight (some portal validations do this)
        if (HttpMethods.IsOptions(req.Method))
            return new OkResult();

        // Read body as BinaryData (recommended by doc)
        BinaryData events = await BinaryData.FromStreamAsync(req.Body);

        // Parse one-or-many events from Event Grid schema
        // (Matches doc: ParseMany)
        EventGridEvent[] eventGridEvents = EventGridEvent.ParseMany(events); 
        foreach (EventGridEvent e in eventGridEvents)
        {
            // System events helper: gives you strongly-typed data objects
            if (e.TryGetSystemEventData(out object eventData))
            {
                // 1) Subscription validation handshake
                if (eventData is SubscriptionValidationEventData validation)
                {
                    _logger.LogInformation("ValidationCode: {code}", validation.ValidationCode);

                    // Must return validationResponse
                    return new OkObjectResult(new { validationResponse = validation.ValidationCode }); 
                }

                // 2) Blob created
                if (eventData is StorageBlobCreatedEventData blobCreated)
                {
                    _logger.LogInformation("BlobCreated URL: {url}", blobCreated.Url);
                }
            }
        }

        return new OkResult();
    }
    }
}
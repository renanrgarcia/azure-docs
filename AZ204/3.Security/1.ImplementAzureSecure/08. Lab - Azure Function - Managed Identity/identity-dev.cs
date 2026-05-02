using System.Text;
using Azure.Identity;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Company.Function;

public class identity_dev
{
    private readonly ILogger<identity_dev> _logger;

    public identity_dev(ILogger<identity_dev> logger)
    {
        _logger = logger;
    }

    [Function("identity_dev")]
     public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
    {
        string accountName = "stdeveus1000";
        string containerName = "data";
        string managedIdentityClientId = "cdd06f78-c15d-4f0f-88e3-f225f841c3a7";

         var serviceUri = new Uri($"https://{accountName}.blob.core.windows.net");

        var credential = new DefaultAzureCredential(new DefaultAzureCredentialOptions
        {
            ManagedIdentityClientId = managedIdentityClientId
        });

        var serviceClient = new BlobServiceClient(serviceUri, credential);
        var containerClient = serviceClient.GetBlobContainerClient(containerName);

        var output = new StringBuilder();
        output.AppendLine($"Listing blobs in container: {containerName}");

        await foreach (BlobItem blobItem in containerClient.GetBlobsAsync())
        {
            output.AppendLine(blobItem.Name);
        }

        return new OkObjectResult(output.ToString());

    }
}
using System.Text;
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

string connectionString = "DefaultEndpointsProtocol=https;AccountName=stdeveus10;AccountKey=/A1yiSJt28wfR8okTFQd9TVBhFUk9sFqtc+v5TVh2thZVKKJnGxkj+4DAMK+5DnolVZzUmsP8AxQ+AStTxf6Rw==;EndpointSuffix=core.windows.net";

string containerName = "scripts";
string blobName = "init.sql";

var serviceClient = new BlobServiceClient(connectionString);
var containerClient = serviceClient.GetBlobContainerClient(containerName);
var blobClient = containerClient.GetBlobClient(blobName);

try
{
    BlobProperties properties = await blobClient.GetPropertiesAsync();
    ETag originalETag = properties.ETag;

    Console.WriteLine($"Original ETag: {originalETag}");

    Console.WriteLine("You now have 45 seconds to modify the blob from somewhere else...");
            await Task.Delay(TimeSpan.FromSeconds(45));

     string updatedContent = $"Updated by app at {DateTime.UtcNow:u}";
     using var stream = new MemoryStream(Encoding.UTF8.GetBytes(updatedContent));
      var options = new BlobUploadOptions
            {
                Conditions = new BlobRequestConditions
                {
                    IfMatch = originalETag
                }
            };
      await blobClient.UploadAsync(stream, options);
      Console.WriteLine("Blob updated successfully.");

}
catch (RequestFailedException ex) when (ex.Status == 412)
        {
            Console.WriteLine("Update failed because the blob was modified by another process.");
            Console.WriteLine("Reload the blob, get the new ETag, and retry the update.");
        }
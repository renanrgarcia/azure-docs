using System.Text;
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;

string connectionString = "DefaultEndpointsProtocol=https;AccountName=stdeveus10;AccountKey=/A1yiSJt28wfR8okTFQd9TVBhFUk9sFqtc+v5TVh2thZVKKJnGxkj+4DAMK+5DnolVZzUmsP8AxQ+AStTxf6Rw==;EndpointSuffix=core.windows.net";

string containerName = "scripts";
string blobName = "init.sql";

var serviceClient = new BlobServiceClient(connectionString);
var containerClient = serviceClient.GetBlobContainerClient(containerName);
var blobClient = containerClient.GetBlobClient(blobName);
BlobLeaseClient leaseClient = blobClient.GetBlobLeaseClient();
try
{
    BlobLease lease = await leaseClient.AcquireAsync(TimeSpan.FromSeconds(60));
    Console.WriteLine($"Lease acquired. Lease ID: {lease.LeaseId}");

    string updatedContent = $"Updated under lease at {DateTime.UtcNow:u}";
    using var stream = new MemoryStream(Encoding.UTF8.GetBytes(updatedContent));

    // Include the lease ID in the upload request
    var options = new BlobUploadOptions
    {
        Conditions = new BlobRequestConditions
        {
            LeaseId = lease.LeaseId
        }
    };

    await blobClient.UploadAsync(stream, options);
    Console.WriteLine("Blob updated successfully while lease was active.");

    await leaseClient.ReleaseAsync();
    Console.WriteLine("Lease released.");
}
catch (RequestFailedException ex)
{
    Console.WriteLine($"Azure Storage error: {ex.Status} - {ex.Message}");
}
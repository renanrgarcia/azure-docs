using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Identity;

string tenantId="4f067c2d-2766-4c5f-a81a-ec27f14f8492";
string clientId="0399e0c8-c1e2-4e66-83d7-2e96f3968af1";
string clientSecret="Zxz8Q~mURAyJkrwHuINlGXMG6rUK1uF~moHIaacB";

string accountName="stdeveus1000";
string containerName = "data";

var credential = new ClientSecretCredential(tenantId,clientId,clientSecret);

var serviceClient = new BlobServiceClient(new Uri($"https://{accountName}.blob.core.windows.net"),credential);

var containerClient = serviceClient.GetBlobContainerClient(containerName);

static async Task ListBlobsAsync(BlobContainerClient containerClient)
{
    Console.WriteLine($"Listing blobs in container: {containerClient.Name}");
     await foreach (BlobItem blobItem in containerClient.GetBlobsAsync())
     {        
        Console.WriteLine($" {blobItem.Name}");
     }
}

await ListBlobsAsync(containerClient);
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

string connectionstring="DefaultEndpointsProtocol=https;AccountName=stdeveus100;AccountKey=RdpKElx3SQNR3nLOsKQlBMLCcMBRJHR/8Vs4yYKlVqvow1y9fsqUcOFZI/BC4jfALZbgQNtpNTp5+ASt1EeAbA==;EndpointSuffix=core.windows.net";

string containerName="data";

var serviceClient= new BlobServiceClient(connectionstring);

var containerClient=serviceClient.GetBlobContainerClient(containerName);

static async Task CreateContainerAsync(BlobContainerClient containerClient)
{
await containerClient.CreateIfNotExistsAsync();
Console.WriteLine("Container created");   

}

static async Task UploadBlob(BlobContainerClient containerClient)
{
    string localFilePath="C:\\dev\\code\\01.py";
    string blobName="01.py";

    var blobClient=containerClient.GetBlobClient(blobName);
    await blobClient.UploadAsync(localFilePath,overwrite: true);
    Console.WriteLine("Blob uploaded to storage account");

}

//await UploadBlob(containerClient);

static async Task ListBlobs(BlobContainerClient containerClient)
{
    await foreach(BlobItem blobItem in containerClient.GetBlobsAsync())
    {
        Console.WriteLine(blobItem.Name);
    }
}

//await ListBlobs(containerClient);

static async Task DownloadBlob(BlobContainerClient containerClient)
{
    string localFilePath="C:\\dev\\code\\01.py";
    string blobName="01.py";

    var blobClient= containerClient.GetBlobClient(blobName);
    await blobClient.DownloadToAsync(localFilePath);
    Console.WriteLine("Download operation complete");
}

await DownloadBlob(containerClient);
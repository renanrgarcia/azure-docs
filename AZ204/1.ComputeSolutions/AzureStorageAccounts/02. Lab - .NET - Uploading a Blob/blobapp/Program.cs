using Azure.Storage.Blobs;

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

await UploadBlob(containerClient);
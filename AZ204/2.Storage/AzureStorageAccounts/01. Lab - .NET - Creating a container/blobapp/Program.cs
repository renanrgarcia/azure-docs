using Azure.Storage.Blobs;

string connectionstring = "DefaultEndpointsProtocol=https;AccountName=stdeveus100;AccountKey=RdpKElx3SQNR3nLOsKQlBMLCcMBRJHR/8Vs4yYKlVqvow1y9fsqUcOFZI/BC4jfALZbgQNtpNTp5+ASt1EeAbA==;EndpointSuffix=core.windows.net";

string containerName = "data";

// Create a BlobServiceClient object which will be used to create a container client
var serviceClient = new BlobServiceClient(connectionstring);

// See if the container exists and create it if it does not
var containerClient = serviceClient.GetBlobContainerClient(containerName);

static async Task CreateContainerAsync(BlobContainerClient containerClient)
{
    await containerClient.CreateIfNotExistsAsync();
    Console.WriteLine("Container created");

}
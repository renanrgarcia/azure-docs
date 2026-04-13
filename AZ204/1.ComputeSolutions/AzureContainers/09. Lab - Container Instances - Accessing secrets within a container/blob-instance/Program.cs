using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System.Text;

string? connectionString = Environment.GetEnvironmentVariable("AZURE_STORAGE_CONNECTION_STRING");

string containerName = "data";
var serviceClient = new BlobServiceClient(connectionString);
var containerClient = serviceClient.GetBlobContainerClient(containerName);

string blobName = "data.txt";

// In containers, use a Linux-friendly temp path
string downloadFilePath = "/tmp/downloaded_data.txt";


static async Task DownloadBlobAsync(BlobContainerClient containerClient, string blobName, string downloadFilePath)
{
    var blobClient = containerClient.GetBlobClient(blobName);
    await blobClient.DownloadToAsync(downloadFilePath);
    Console.WriteLine("Download complete");

    // Output to container logs
    string contents = await File.ReadAllTextAsync(downloadFilePath, Encoding.UTF8);
    Console.WriteLine("---- Blob contents start ----");
    Console.WriteLine(contents);
    Console.WriteLine("---- Blob contents end ----");
}

await DownloadBlobAsync(containerClient, blobName, downloadFilePath);
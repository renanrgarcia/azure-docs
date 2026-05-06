using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

string connectionString = 
"DefaultEndpointsProtocol=https;AccountName=appstore554554;AccountKey=Z/jI1QzplUeAXU8yWxSuu2knZfh8O0sfA7HG1R7dGTppufkG37lzTjq0X1fFpYfDrqOdmyM2q8pY+AStLsg5RA==;EndpointSuffix=core.windows.net";

BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);

string containerName="scripts";
string fileName="fileA.txt";


// Getting a handle to the container
BlobContainerClient blobContainerClient=blobServiceClient.GetBlobContainerClient(containerName);

BlobClient blobClient=blobContainerClient.GetBlobClient(fileName);
Response<BlobDownloadResult> response= await blobClient.DownloadContentAsync();
string blobData= response.Value.Content.ToString();
Console.WriteLine(blobData);

// Get the Etag of the Blob
ETag eTag=response.Value.Details.ETag;
Console.WriteLine(eTag);

// Let's sleep for some time
System.Threading.Thread.Sleep(10000);
// Let's make a change to the blob and upload it again
 BlobUploadOptions blobUploadOptions = new()
        {
            Conditions = new BlobRequestConditions()
            {
                IfMatch = eTag
            }
        };
try
{
    string newBlobData=$"We want to update the blob {blobData}";
    await blobClient.UploadAsync(BinaryData.FromString(newBlobData), blobUploadOptions);
    Console.WriteLine("Changes made");
}
catch(Exception e)
{
    Console.WriteLine(e.Message);
}


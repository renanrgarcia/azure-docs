using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;



namespace Common.Functions
{
    public class blobtrigger
    {
        private readonly ILogger<blobtrigger> _logger;

        public blobtrigger(ILogger<blobtrigger> logger)
        {
            _logger = logger;
        }

        [Function(nameof(blobtrigger))]
        [CosmosDBOutput("appdb", "blobinfo", Connection = "CosmosDBConnection", CreateIfNotExists = true)]
        public Object Run([BlobTrigger("scripts/{name}", Connection = "164161_STORAGE")] BlobClient blobClient,string name)
        {
             
            BlobProperties blobProperties = blobClient.GetProperties();
            _logger.LogInformation($"The size of the blob : {blobProperties.ContentLength}");
            _logger.LogInformation($"The container of the blob : {blobClient.BlobContainerName}");
            _logger.LogInformation($"The name of the blob : {blobClient.Name}");
            return new {id=Guid.NewGuid().ToString(),blobcontainername=blobClient.BlobContainerName,name=blobClient.Name,length=blobProperties.ContentLength};
           

        }
    }
}

using Azure.Messaging.EventHubs;
using Azure.Storage.Blobs;

string eventHubConnectionString="";
string eventHubName="orders-events";

string blobConnectionString="";
string containerName="eh-checkpoints";

string consumerGroup="cg-analytics";

var containerClient = new BlobContainerClient(blobConnectionString,containerName);

var processor = new EventProcessorClient(
containerClient,consumerGroup,eventHubConnectionString,eventHubName
);

processor.ProcessEventAsync += async args =>
{
    string body = args.Data.EventBody.ToString();
    Console.WriteLine($"[{consumerGroup}] Partition={args.Partition.PartitionId} Seq={args.Data.SequenceNumber} Body={body}");
    await args.UpdateCheckpointAsync();

};

processor.ProcessErrorAsync += args =>
{
    Console.WriteLine($"ERROR in {consumerGroup}: {args.Exception.Message}");
    return Task.CompletedTask;
};

await processor.StartProcessingAsync();
Console.WriteLine($"Listening as {consumerGroup}. Press Enter to stop...");
Console.ReadLine();
await processor.StopProcessingAsync();
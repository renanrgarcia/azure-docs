using Microsoft.Azure.Cosmos;
using Newtonsoft.Json;

string endpoint = "https://app-cosmos-db-01.documents.azure.com:443/";
string key = "f1S7UMVmF3DGAtobK5NCiBMXvrEhj6sloG5uTZyHUogZ9Mh2feqMAuIcERMC3DQOo6ITxtfhX0AYACDb2eKZxg==";

string databaseId = "orders-db";
const string monitoredContainerId = "orders";
const string leaseContainerId = "leases";
const string processorName = "orders-cfp";
string instanceName = Environment.MachineName;

var client = new CosmosClient(endpoint, key);
Container monitored = client.GetContainer(databaseId, monitoredContainerId);
Container leases = client.GetContainer(databaseId, leaseContainerId);

static Task HandleChangesAsync(
    IReadOnlyCollection<Order> changes,
    CancellationToken cancellationToken)
{
    foreach (var doc in changes)
    {
        Console.WriteLine($"Change: id={doc.id}, customerId={doc.customerId}, type={doc.type}, total={doc.total}");
    }
    return Task.CompletedTask;
}

ChangeFeedProcessor processor =monitored.GetChangeFeedProcessorBuilder<Order>(
processorName: processorName,
onChangesDelegate: HandleChangesAsync)
.WithInstanceName(instanceName)
.WithLeaseContainer(leases)
.WithPollInterval(TimeSpan.FromSeconds(2))
.Build();

Console.WriteLine("Starting Change Feed Processor...");
await processor.StartAsync();

Console.WriteLine("Running. Insert/update docs in the monitored container. Press ENTER to stop.");
Console.ReadLine();

Console.WriteLine("Stopping...");
await processor.StopAsync();
Console.WriteLine("Stopped.");

public class Order
{
    public string id { get; set; } = default!;
    public string customerId { get; set; } = default!;
    public string type { get; set; } = default!;
    public double total { get; set; }
}
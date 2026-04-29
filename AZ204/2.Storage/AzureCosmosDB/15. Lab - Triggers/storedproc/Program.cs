using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Scripts;
using Newtonsoft.Json;

string Endpoint="https://app-cosmos-db-01.documents.azure.com:443/";
string Key="f1S7UMVmF3DGAtobK5NCiBMXvrEhj6sloG5uTZyHUogZ9Mh2feqMAuIcERMC3DQOo6ITxtfhX0AYACDb2eKZxg==";
string DatabaseId = "orders-db";
string ContainerId = "orders";
string PartitionKeyPath = "/customerId";

var client = new CosmosClient(Endpoint, Key);

Container container = client.GetContainer(DatabaseId, ContainerId);

var order = new { id = "ord-1001", customerId = "C004", total = 199.0 };

var options = new ItemRequestOptions
{
    PreTriggers = new List<string> { "setOrderDefaults" }
};

var resp=await container.CreateItemAsync(order,new PartitionKey("C004"),options);

Console.WriteLine(resp.Resource);
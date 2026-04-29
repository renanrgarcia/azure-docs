using System.Net;
using Microsoft.Azure.Cosmos;
using Newtonsoft.Json;

string Endpoint="https://app-cosmos-db-01.documents.azure.com:443/";
string Key="f1S7UMVmF3DGAtobK5NCiBMXvrEhj6sloG5uTZyHUogZ9Mh2feqMAuIcERMC3DQOo6ITxtfhX0AYACDb2eKZxg==";

var cosmosClient = new CosmosClient(Endpoint,Key);

static async Task CreateDBContainer(CosmosClient client)
{
    string DatabaseId="orders-db";
    string ContainerId="orders";
    string PartitionKey="/customerId";
    
    Database database=client.GetDatabase(DatabaseId);
    await client.CreateDatabaseIfNotExistsAsync(DatabaseId);
    Console.WriteLine("Database created");

    Container container=database.GetContainer(ContainerId);
    await database.CreateContainerIfNotExistsAsync(ContainerId,PartitionKey);
    Console.WriteLine("Container created");

}

await CreateDBContainer(cosmosClient);
using System.Net;
using Microsoft.Azure.Cosmos;
using Newtonsoft.Json;

string Endpoint="https://app-cosmos-db-01.documents.azure.com:443/";
string Key="f1S7UMVmF3DGAtobK5NCiBMXvrEhj6sloG5uTZyHUogZ9Mh2feqMAuIcERMC3DQOo6ITxtfhX0AYACDb2eKZxg==";

var cosmosClient = new CosmosClient(Endpoint,Key);

var orders = new Order[]
{
    new Order
    {
        Id = "ord-1001",
        CustomerId = "C001",
        CustomerName = "Mark",
        CourseId = "AZ-204",
        CourseName = "AZ-204 Azure Developer",
        OrderDateUtc = DateTime.Parse("2026-02-11T10:15:00Z").ToUniversalTime(),
        Amount = 49.99m,
        Currency = "USD",
        Status = "Paid"
    },
    new Order
    {
        Id = "ord-1002",
        CustomerId = "C001",
        CustomerName = "Mark",
        CourseId = "AZ-204",
        CourseName = "AZ-204 Azure Developer",
        OrderDateUtc = DateTime.UtcNow.AddMinutes(-20),
        Amount = 49.99m,
        Currency = "USD",
        Status = "Paid"
    },
    new Order
    {
        Id = "ord-2001",
        CustomerId = "C002",
        CustomerName = "Sara",
        CourseId = "AZ-204",
        CourseName = "AZ-204 Azure Developer",
        OrderDateUtc = DateTime.UtcNow.AddHours(-3),
        Amount = 39.99m,
        Currency = "USD",
        Status = "Pending"
    }
};

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

//await CreateDBContainer(cosmosClient);

static async Task CreateItems(CosmosClient client,Order[] orders)
{
    string DatabaseId="orders-db";
    string ContainerId="orders";
    Database database=client.GetDatabase(DatabaseId);
    Container container=database.GetContainer(ContainerId);
    foreach (var order in orders)
    {
        await container.CreateItemAsync(order,new PartitionKey(order.CustomerId));
        Console.WriteLine($"Inserted item: {order.Id}");
    }
    
}

await CreateItems(cosmosClient,orders);
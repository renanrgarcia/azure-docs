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
        Id = "ord-3001",
        CustomerId = "C004",
        CustomerName = "Sara",
        OrderDateUtc = DateTime.Parse("2026-02-11T15:30:00Z").ToUniversalTime(),
        Status = "Paid",
        Course = new CourseInfo
        {
            CourseId = "DP-600",
            CourseName = "DP-600 Fabric Analytics Engineer",
            Category = "Data"
        },
        Payment = new PaymentInfo
        {
            Method = "Card",
            Provider = "Visa",
            TransactionId = "tx-9001",
            PaidAmount = 59.99m,
            Currency = "USD"
        }
    },
    new Order
    {
        Id = "ord-3002",
        CustomerId = "C002",
        CustomerName = "James",
        OrderDateUtc = DateTime.Parse("2026-02-11T16:05:00Z").ToUniversalTime(),
        Status = "Failed",
        Course = new CourseInfo
        {
            CourseId = "AZ-204",
            CourseName = "AZ-204 Azure Developer",
            Category = "Azure"
        },
        Payment = new PaymentInfo
        {
            Method = "Card",
            Provider = "Mastercard",
            TransactionId = "tx-9002",
            PaidAmount = 49.99m,
            Currency = "USD",
            FailureReason = "InsufficientFunds"
        }
    }
};


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

static async Task ReadItem(CosmosClient client)
{
    string DatabaseId="orders-db";
    string ContainerId="orders";
    Database database=client.GetDatabase(DatabaseId);
    Container container=database.GetContainer(ContainerId);

    string id="ord-1001";
    string partitionKey="C001";

    ItemResponse<Order>response=await container.ReadItemAsync<Order>(
        id: id,
            partitionKey: new PartitionKey(partitionKey)
    );

    Order order=response.Resource;
    Console.WriteLine($"Read item: {order.Id} | {order.CustomerName} | {order.Status}");
}

//await ReadItem(cosmosClient);



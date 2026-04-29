using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Scripts;
using Newtonsoft.Json;

string Endpoint="https://app-cosmos-db-01.documents.azure.com:443/";
string Key="f1S7UMVmF3DGAtobK5NCiBMXvrEhj6sloG5uTZyHUogZ9Mh2feqMAuIcERMC3DQOo6ITxtfhX0AYACDb2eKZxg==";
string DatabaseId = "orders-db";
string ContainerId = "orders";
string PartitionKeyPath = "/customerId";
string sprocId = "placeOrder";

var client = new CosmosClient(Endpoint, Key);

Container container = client.GetContainer(DatabaseId, ContainerId);

var customerId = "C004"; 
var orderId = $"ord-001";
var auditId = "audit-001";   

var order = new
{
    id = orderId,
    customerId = customerId,
    type = "order",
    total = 199.00
};

var audit = new
{
    id = auditId,
    customerId = customerId,
    type = "audit",
    message = $"Order {orderId} placed"
};

StoredProcedureExecuteResponse<dynamic> resp=await container.Scripts.ExecuteStoredProcedureAsync<dynamic>(
    storedProcedureId: sprocId,
    partitionKey: new PartitionKey(customerId),
     parameters: new dynamic[] {order,audit}
);

Console.WriteLine($"Status: {resp.StatusCode}");
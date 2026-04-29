
using System;
using System.Threading.Tasks;
using Azure;
using Azure.Data.Tables;

string Endpoint="https://app-cosmos-db-02.table.cosmos.azure.com:443/";
string accountName="app-cosmos-db-02";
string accountKey="CMGhfIoCm2SP02cPJz5nYcVM6sPwtT36G83N9k9PDkXTqu1yD1SOGKikGb985y8Adu3rpatnkf5XACDbVG4V4w==";
string tableName="Orders";

var serviceClient = new 
TableServiceClient(new Uri(Endpoint),new TableSharedKeyCredential(accountName,accountKey));

TableClient tableClient=serviceClient.GetTableClient(tableName);

var orders = new[]
    {
        new TableEntity(partitionKey: "C004", rowKey: "ord-3001")
        {
            ["CustomerName"] = "Sara",
            ["OrderDateUtc"] = DateTime.Parse("2026-02-11T15:30:00Z").ToUniversalTime(),
            ["Status"]       = "Paid",
            ["CourseId"]     = "DP-600",
            ["CourseName"]   = "DP-600 Fabric Analytics Engineer",
            ["Category"]     = "Data",
            ["Method"]       = "Card",
            ["Provider"]     = "Visa",
            ["TransactionId"]= "tx-9001",
            ["PaidAmount"]   = 59.99,
            ["Currency"]     = "USD"
        },

        new TableEntity(partitionKey: "C002", rowKey: "ord-3002")
        {
            ["CustomerName"] = "James",
            ["OrderDateUtc"] = DateTime.Parse("2026-02-11T16:05:00Z").ToUniversalTime(),
            ["Status"]       = "Failed",
            ["CourseId"]     = "AZ-204",
            ["CourseName"]   = "AZ-204 Azure Developer",
            ["Category"]     = "Azure",
            ["Method"]       = "Card",
            ["Provider"]     = "Mastercard",
            ["TransactionId"]= "tx-9002",
            ["PaidAmount"]   = 49.99,
            ["Currency"]     = "USD",
            ["FailureReason"]= "InsufficientFunds"
        },

        new TableEntity(partitionKey: "C004", rowKey: "ord-3003")
        {
            ["CustomerName"] = "Sara",
            ["OrderDateUtc"] = DateTime.UtcNow,
            ["Status"]       = "Paid",
            ["CourseId"]     = "AZ-204",
            ["CourseName"]   = "AZ-204 Azure Developer",
            ["Category"]     = "Azure",
            ["Method"]       = "Card",
            ["Provider"]     = "Visa",
            ["TransactionId"]= "tx-9003",
            ["PaidAmount"]   = 49.99,
            ["Currency"]     = "USD"
        }
    };

static async Task AddEntities(TableClient tableClient, TableEntity[] orders)
{
    foreach(var order in orders)
    {
        await tableClient.AddEntityAsync(order);
        Console.WriteLine($"Inserted: PK={order.PartitionKey}, RK={order.RowKey}");
    }
}

await AddEntities(tableClient,orders);

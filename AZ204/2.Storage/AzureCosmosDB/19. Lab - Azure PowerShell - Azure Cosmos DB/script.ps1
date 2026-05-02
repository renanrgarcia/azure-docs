# The following can be used to create a Cosmos DB account using Azure PowerShell. 
# Make sure to replace the $resourceGroupName variable with the name of your resource group.

$resourceGroupName = "rg-az204-dev-eus"

# Set the location for the Cosmos DB account. You can specify multiple locations for geo-redundancy if needed.
$locations = @(
    New-AzCosmosDBLocationObject -LocationName eastus -FailoverPriority 0 -IsZoneRedundant 0
    # Failover priority 0 means this is the primary region. You can add more regions with failover priority 1, 2, etc. if needed.
)

# Create the Cosmos DB account with the specified settings. The API kind is set to "Sql" for SQL API, and the default consistency level is set to "Session".
New-AzCosmosDBAccount `
    -ResourceGroupName $resourceGroupName `
    -Name "nosqlaccount5000" `
    -ApiKind "Sql" `
    -LocationObject $locations `
    -DefaultConsistencyLevel "Session" 

# Then create a database and a container in the Cosmos DB account.

New-AzCosmosDBSqlDatabase `
    -ResourceGroupName $resourceGroupName `
    -AccountName "nosqlaccount5000" `
    -Name appdb

New-AzCosmosDBSqlContainer `
    -ResourceGroupName $resourceGroupName `
    -AccountName "nosqlaccount5000" `
    -DatabaseName "appdb" `
    -Name "orders" `
    -PartitionKeyKind "Hash" `
    -PartitionKeyPath "/customerId" `
    -Throughput 400
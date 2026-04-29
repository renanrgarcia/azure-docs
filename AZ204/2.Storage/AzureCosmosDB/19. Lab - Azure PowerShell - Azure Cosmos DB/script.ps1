# The following can be used to create a Cosmos DB account using Azure PowerShell. 
# Make sure to replace the $resourceGroupName variable with the name of your resource group.

$resourceGroupName="rg-az204-dev-eus"

$locations = @(
    New-AzCosmosDBLocationObject -LocationName eastus -FailoverPriority 0 -IsZoneRedundant 0
)

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
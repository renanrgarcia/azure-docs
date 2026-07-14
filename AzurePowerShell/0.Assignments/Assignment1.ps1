# Assignment 1 - Creating multiple containers in a storage account

# Create a new resource group
$ResourceGroupName = "MyResourceGroup"
$Location = "East US"
New-AzResourceGroup -Name $ResourceGroupName -Location $Location

# Create a new storage account
$StorageAccountName = "mystorageaccount$(Get-Random)"
New-AzStorageAccount -ResourceGroupName $ResourceGroupName -Name $StorageAccountName

# Create multiple containers in the storage account
$ContainerNames = @
(
    "container1",
    "container2",
    "container3"
)

$StorageAccount = Get-AzStorageAccount -ResourceGroupName $ResourceGroupName -Name $StorageAccountName

foreach ($ContainerName in $ContainerNames) {
    New-AzStorageContainer -Name $ContainerName -Context $StorageAccount.Context
    Write-Output "Created container: $ContainerName"
}


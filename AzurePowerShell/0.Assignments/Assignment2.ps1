# Assignment 2 - Create a Azure Data Lake Gen2 storage account

# Create the storage account with Data Lake Gen2 capabilities
$StorageAccountName = "mydatalakestorage$(Get-Random)"
$AccountKind="StorageV2"
$AccountSKU="Standard_LRS"
$ResourceGroupName="powershell-grp"
$Location = "North Europe"

New-AzStorageAccount -ResourceGroupName $ResourceGroupName -Name $StorageAccountName -Location $Location -SkuName $AccountSKU -Kind $AccountKind -EnableHierarchicalNamespace $true

# Create a new container in the Data Lake Gen2 storage account
$ContainerName = "mydatalakecontainer"
$StorageAccount = Get-AzStorageAccount -ResourceGroupName $ResourceGroupName -Name $StorageAccountName

New-AzStorageContainer -Name $ContainerName -Context $StorageAccount.Context

# Create a new directory in the container
$DirectoryName = "mydatalakedirectory"
New-AzDataLakeGen2Item -FileSystem $ContainerName -Path $DirectoryName

# Uploading a file to the directory in the Data Lake Gen2 storage account
$FileName = "myfile.txt"
$CompleteStoragePath = "$DirectoryName/$FileName"

New-AzDataLakeGen2Item -Context $StorageAccount.Context `
    -FileSystem $ContainerName `
    -Path $CompleteStoragePath `
    -Source "C:\path\to\local\file.txt" `
    -Force
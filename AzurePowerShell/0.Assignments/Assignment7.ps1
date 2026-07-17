$ResourceGroupName = "YourResourceGroupName"
$VMName = "YourVMName"
$StorageAccountName = "YourStorageAccountName"
$AccountKind = "StorageV2"
$AccountSKU = "Standard_LRS"
$Location = "North Europe"

# Get the VM object
$VM = Get-AzVM -ResourceGroupName $ResourceGroupName -Name $VMName

# Create the storage account (must be in the same region as the VM)
$StorageAccount = New-AzStorageAccount -ResourceGroupName $ResourceGroupName `
-Name $StorageAccountName `
-Location $Location `
-Kind $AccountKind `
-SkuName $AccountSKU

# Enable boot diagnostics on the VM, pointing at the storage account
Set-AzVMBootDiagnostic -VM $VM -Enable -ResourceGroupName $ResourceGroupName `
-StorageAccountName $StorageAccount.StorageAccountName

# Update the VM to apply the boot diagnostics configuration
Update-AzVM -ResourceGroupName $ResourceGroupName -VM $VM

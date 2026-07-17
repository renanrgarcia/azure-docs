$ResourceGroupName = "YourResourceGroupName"
$VMName = "YourVMName"
$Location = "YourLocation"
$VirtualNetworkName = "YourVNetName"
$NewNICName = "NewNIC"

# First stop the VM
Set-AzVM -ResourceGroupName $ResourceGroupName -Name $VMName -Status Stopped

# Resolve the subnet dynamically from the virtual network (first subnet)
$VirtualNetwork = Get-AzVirtualNetwork -ResourceGroupName $ResourceGroupName -Name $VirtualNetworkName
$SubnetId = $VirtualNetwork.Subnets[0].Id

# Create a new network interface (secondary NIC - no public IP needed)
$NewNIC = New-AzNetworkInterface -ResourceGroupName $ResourceGroupName -Name $NewNICName -Location $Location -SubnetId $SubnetId

# Get the VM object
$VM = Get-AzVM -ResourceGroupName $ResourceGroupName -Name $VMName

# Set the NetworkInterface[0] primary property to $true , that is for the first network interface for the VM
$VM.NetworkProfile.NetworkInterfaces[0].Primary = $true

# Add the new network interface to the VM
Add-AzVMNetworkInterface -VM $VM -Id $NewNIC.Id

# Update the VM
Update-AzVM -ResourceGroupName $ResourceGroupName -VM $VM

# Start the VM
Start-AzVM -ResourceGroupName $ResourceGroupName -Name $VMName

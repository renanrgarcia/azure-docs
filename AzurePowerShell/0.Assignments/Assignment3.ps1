$ResourceGroupName ="powershell-grp"
$VirtualNetworkName="app-network"
$SubnetName="SubnetB"
$SubnetAddressSpace="10.0.1.0/24"

# Create a new virtual network
New-AzVirtualNetwork -Name $VirtualNetworkName -ResourceGroupName $ResourceGroupName

# Create a new subnet in the virtual network
$VirtualNetwork = Get-AzVirtualNetwork -Name $VirtualNetworkName -ResourceGroupName $ResourceGroupName

Add-AzVirtualNetworkSubnetConfig -VirtualNetwork $VirtualNetwork -Name $SubnetName -AddressPrefix $SubnetAddressSpace

# Update the virtual network with the new subnet configuration
Set-AzVirtualNetwork -VirtualNetwork $VirtualNetwork
# Or
$VirtualNetwork | Set-AzVirtualNetwork

# After, we can remove the subnet from the virtual network if needed
Remove-AzVirtualNetworkSubnetConfig -VirtualNetwork $VirtualNetwork -Name $SubnetName

$VirtualNetwork | Set-AzVirtualNetwork
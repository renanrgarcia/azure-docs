$ResourceGroupName = "MyResourceGroup"
$VirtualNetworkName = "MyVNet"

# Find the Network Security Group (NSG) associated with the virtual network
$VirtualNetwork = Get-AzVirtualNetwork -Name $VirtualNetworkName -ResourceGroupName $ResourceGroupName

$NetworkSecurityGroup = $VirtualNetwork.Subnets[0].NetworkSecurityGroup

$NetworkSecurityGroupId = $NetworkSecurityGroup.Id
$Length = $NetworkSecurityGroupId.Length
$NetworkSecurityGroupName = $NetworkSecurityGroupId.Substring($NetworkSecurityGroupId.LastIndexOf('/') + 1, $Length - $NetworkSecurityGroupId.LastIndexOf('/') - 1)

# Remove the association of the Network Security Group (NSG) from the subnet
$VirtualNetwork.Subnets[0].NetworkSecurityGroup = $null

$VirtualNetwork | Set-AzVirtualNetwork

# Delete the Network Security Group (NSG) associated with the virtual network

Remove-AzNetworkSecurityGroup -Name $NetworkSecurityGroupName -ResourceGroupName $ResourceGroupName -Force

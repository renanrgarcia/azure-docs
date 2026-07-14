$ResourceGroupName ="powershell-grp"
$VirtualNetworkName="app-network"

# Get the virtual network
$VirtualNetwork = Get-AzVirtualNetwork -Name $VirtualNetworkName -ResourceGroupName $ResourceGroupName

# List all subnets and Address prefixes in the virtual network
$VirtualNetwork.Subnets | ForEach-Object {
    Write-Output "Subnet Name: $($_.Name)"
    Write-Output "Address Prefix: $($_.AddressPrefix)"
    Write-Output "-----------------------------"
}


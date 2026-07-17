# Ensure that you have an Azure virtual machine in place that does NOT have a Network Security Group.

# Then create a new Network Security Group and attach it to the Network Interface of the virtual machine.

$ResourceGroupName = "YourResourceGroupName"
$VMName = "YourVMName"
$Location = "North Europe"
$NSGName = "YourNSGName"

# Get the VM object
$VM = Get-AzVM -ResourceGroupName $ResourceGroupName -Name $VMName

# Resolve the Network Interface attached to the VM (first NIC)
$NICId = $VM.NetworkProfile.NetworkInterfaces[0].Id
$NICName = $NICId.Split('/')[-1]
$NIC = Get-AzNetworkInterface -ResourceGroupName $ResourceGroupName -Name $NICName

# Create an inbound rule (example: allow RDP on port 3389)
$RDPRule = New-AzNetworkSecurityRuleConfig -Name "Allow-RDP" `
-Description "Allow inbound RDP" `
-Access Allow `
-Protocol Tcp `
-Direction Inbound `
-Priority 1000 `
-SourceAddressPrefix Internet `
-SourcePortRange * `
-DestinationAddressPrefix * `
-DestinationPortRange 3389

# Create the new Network Security Group with the rule
$NSG = New-AzNetworkSecurityGroup -ResourceGroupName $ResourceGroupName `
-Name $NSGName `
-Location $Location `
-SecurityRules $RDPRule

# Attach the Network Security Group to the Network Interface
$NIC.NetworkSecurityGroup = $NSG

# Apply the change to the Network Interface
$NIC | Set-AzNetworkInterface

It seems to me that the piano is more free and maybe inconsequential in jazz, I think that it's no closed box that the piano tried to stay in. The composers try to extract using different kinds of colors through the most unexpected dissonant chords. 
# For the existing Azure Firewall, add an Application rule that will allow requests
# from the Azure virtual machine to www.google.com


$FirewallPolicyName = "firewall-policy"
$ResourceGroupName = "powershell-grp"

$CollectionGroup = New-AzFirewallPolicyRuleCollectionGroup -Name "ApplicationCollectionGroup" -Priority 300 `
  -ResourceGroupName $ResourceGroupName -FirewallPolicyName $FirewallPolicyName

$VmName = "appvm"
$AppRuleName = "Allow-Google-$VmName"

# We need the private IP address assigned to appvm so the rule only applies to that VM
$VMNetworkProfile = (Get-AzVm -Name $VmName).NetworkProfile
$NetworkInterface = Get-AzNetworkInterface -ResourceId $VMNetworkProfile.NetworkInterfaces[0].Id
$VMPrivateIPAddress = $NetworkInterface.IpConfigurations[0].PrivateIpAddress

$Rule1 = New-AzFirewallPolicyApplicationRule -Name $AppRuleName -SourceAddress "$VMPrivateIPAddress/32" `
  -Protocol "Https" -TargetFqdn "www.google.com"

$Collection = New-AzFirewallPolicyFilterRuleCollection -Name "ApplicationCollectionA" -Priority 1000 -Rule $Rule1 `
  -ActionType "Allow"

$CollectionGroup = Get-AzFirewallPolicyRuleCollectionGroup -Name "ApplicationCollectionGroup" `
  -ResourceGroupName $ResourceGroupName -AzureFirewallPolicyName $FirewallPolicyName

$CollectionGroup.Properties.RuleCollection.Add($Collection)

# We then update the Firewall Policy accordingly
$FirewallPolicy = Get-AzFirewallPolicy -Name $FirewallPolicyName -ResourceGroupName $ResourceGroupName

Set-AzFirewallPolicyRuleCollectionGroup -Name "ApplicationCollectionGroup" -Priority 300 `
  -FirewallPolicyObject $FirewallPolicy -RuleCollection $CollectionGroup.Properties.RuleCollection

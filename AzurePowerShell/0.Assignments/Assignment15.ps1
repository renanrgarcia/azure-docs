# Ensure that you have more than one Azure virtual machine for which you want to
# create the metric alert rule, then use Add-AzMetricAlertRuleV2 to create the rule

<#
Command Reference

1. Get-AzResource
https://docs.microsoft.com/en-us/powershell/module/az.resources/get-azresource?view=azps-7.3.2

2. New-TimeSpan
https://docs.microsoft.com/en-us/powershell/module/microsoft.powershell.utility/new-timespan?view=powershell-7.2

3. New-AzMetricAlertRuleV2Criteria
https://docs.microsoft.com/en-us/powershell/module/az.monitor/new-azmetricalertrulev2criteria?view=azps-7.3.2

4. Add-AzMetricAlertRuleV2
https://docs.microsoft.com/en-us/powershell/module/az.monitor/add-azmetricalertrulev2?view=azps-7.3.2

#>

function Get-ResourceId
{
    param([String] $ResourceName)

    $Resource=Get-AzResource -Name $ResourceName
    return $Resource.Id
}

function Get-ResourceType
{
    param([String] $ResourceName)

    $Resource=Get-AzResource -Name $ResourceName
    return $Resource.ResourceType
}

# appvm1 and appvm2 are the virtual machines created in the Load Balancer lab
$VMNames="appvm1","appvm2"

$ResourceGroupName="powershell-grp"

$TargetResourceScope=@()
foreach($VMName in $VMNames)
{
    $TargetResourceScope+=Get-ResourceId $VMName
}

$TargetResourceType=Get-ResourceType $VMNames[0]
$TargetResourceRegion="northeurope"

# The alert rule will check the CPU Usage utilization across both virtual machines
# If the CPU Utilization goes beyond 70% in the last 5 minutes, then the alert will be raised

$AlertName="MultiVMCPUAlert"
$Threshold=70
$MetricName="Percentage CPU"
$Description="Alert when CPU percentage goes beyond 70% on any of the target virtual machines"
$WindowSize=New-TimeSpan -Minutes 5
$Frequency=New-TimeSpan -Minutes 5

$Condition=New-AzMetricAlertRuleV2Criteria -MetricName $MetricName `
-TimeAggregation Average -Operator GreaterThanOrEqual -Threshold $Threshold

Add-AzMetricAlertRuleV2 -Name $AlertName -ResourceGroupName $ResourceGroupName `
-Severity 3 -TargetResourceScope $TargetResourceScope `
-TargetResourceType $TargetResourceType -TargetResourceRegion $TargetResourceRegion `
-Description $Description -Condition $Condition `
-WindowSize $WindowSize -Frequency $Frequency

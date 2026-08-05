# Get the resources that have a tag name of Department and a tag value of Logistics
# Then display the resource name and resource type for the resources that fit this criteria


$TagName = "Department"
$TagValue = "Logistics"

$TaggedResources = Get-AzResource -TagName $TagName -TagValue $TagValue

$TaggedResources | Select-Object Name, ResourceType

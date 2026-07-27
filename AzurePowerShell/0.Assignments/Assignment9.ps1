# Ensure that you have an Azure SQL Web App in place.

# Then configure Access Restrictions so that traffic is denied from all locations
# except from your machine. Use 300 and 400 as weights.

$ResourceGroupName = "YourResourceGroupName"
$WebAppName = "YourWebAppName"

# Get the public IP address assigned to our machine
$IPAddress = Invoke-WebRequest -Uri "https://ifconfig.me/ip" | Select-Object -ExpandProperty Content
$IPAddress = $IPAddress.Trim()

# Allow traffic only from our machine (weight 300)
Add-AzWebAppAccessRestrictionRule -ResourceGroupName $ResourceGroupName `
  -WebAppName $WebAppName -Name "Allow-MyMachine" -Priority 300 `
  -Action Allow -IpAddress "$IPAddress/32"

# Deny traffic from all other locations (weight 400)
Add-AzWebAppAccessRestrictionRule -ResourceGroupName $ResourceGroupName `
  -WebAppName $WebAppName -Name "Deny-All" -Priority 400 `
  -Action Deny -IpAddress "0.0.0.0/0"

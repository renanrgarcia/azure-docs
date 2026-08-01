# Ensure that you have an Azure Traffic Manager Profile in place that has web endpoints configured.

# Then create a script that continuously checks whether the Traffic Manager profile's
# endpoints are reachable, and notifies whenever an endpoint goes down.

$ResourceGroupName = "YourResourceGroupName"
$ProfileName = "YourTrafficManagerProfileName"
$CheckIntervalSeconds = 30

# Keep track of each endpoint's last known status so we only notify on a change
$LastKnownStatus = @{}

while ($true) {
  $TrafficManagerProfile = Get-AzTrafficManagerProfile -ResourceGroupName $ResourceGroupName -Name $ProfileName

  # Check if the profile has any endpoints
  if ($TrafficManagerProfile.Endpoints.Count -eq 0) {
    Write-Warning "No endpoints found in Traffic Manager profile '$ProfileName'."
    Start-Sleep -Seconds $CheckIntervalSeconds
    continue
  }

  foreach ($Endpoint in $TrafficManagerProfile.Endpoints) {
    $CurrentStatus = $Endpoint.EndpointMonitorStatus
    $PreviousStatus = $LastKnownStatus[$Endpoint.Name]

    if ($CurrentStatus -ne "Online" -and $PreviousStatus -ne $CurrentStatus) {
      # Endpoint just went down (or came up in a non-Online state) - notify
      Write-Warning "ALERT: Endpoint '$($Endpoint.Name)' is '$CurrentStatus' as of $(Get-Date)."

      # Optionally send an email notification instead of (or in addition to) the console warning
      # Send-MailMessage -To "you@example.com" -From "alerts@example.com" `
      #   -Subject "Traffic Manager Alert: $($Endpoint.Name) is $CurrentStatus" `
      #   -Body "Endpoint '$($Endpoint.Name)' in profile '$ProfileName' is now '$CurrentStatus'." `
      #   -SmtpServer "smtp.example.com"
    }
    elseif ($CurrentStatus -eq "Online" -and $PreviousStatus -ne "Online" -and $PreviousStatus) {
      Write-Host "RECOVERED: Endpoint '$($Endpoint.Name)' is back Online as of $(Get-Date)." -ForegroundColor Green
    }
    else {
      Write-Host "Endpoint '$($Endpoint.Name)' is $CurrentStatus."
    }

    $LastKnownStatus[$Endpoint.Name] = $CurrentStatus
  }

  Start-Sleep -Seconds $CheckIntervalSeconds
}

# Delete the user in Azure AD which was created in an earlier chapter
# (114. Lab - Creating a user in Azure AD)

<#
Command Reference

1. Remove-AzADUser
https://docs.microsoft.com/en-us/powershell/module/az.resources/remove-azaduser?view=azps-7.3.2

#>

$UserPrincipalName="UserA@techsup4000gmail.onmicrosoft.com"

Remove-AzADUser -UserPrincipalName $UserPrincipalName

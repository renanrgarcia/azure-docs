# Delete the Custom RBAC role that was created in the earlier chapter
# (113. Lab - Creating a custom role)

# We first need to remove any role assignments that use the role, since the role
# definition cannot be deleted while assignments still reference it

<#
Command Reference

1. Get-AzRoleAssignment
https://docs.microsoft.com/en-us/powershell/module/az.resources/get-azroleassignment?view=azps-7.3.2

2. Remove-AzRoleAssignment
https://docs.microsoft.com/en-us/powershell/module/az.resources/remove-azroleassignment?view=azps-7.3.2

3. Get-AzRoleDefinition
https://docs.microsoft.com/en-us/powershell/module/az.resources/get-azroledefinition?view=azps-7.3.2

4. Remove-AzRoleDefinition
https://docs.microsoft.com/en-us/powershell/module/az.resources/remove-azroledefinition?view=azps-7.3.2

#>

$CustomRoleDefinition="Storage And Virtual Machine Contributor"

# Remove every role assignment that uses this custom role
Get-AzRoleAssignment -RoleDefinitionName $CustomRoleDefinition | Remove-AzRoleAssignment

# Now that no assignments reference the role, the custom role definition can be deleted
Get-AzRoleDefinition -Name $CustomRoleDefinition | Remove-AzRoleDefinition -Force

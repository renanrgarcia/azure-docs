# Ensure that you have an Azure SQL database in place as per a prior deployment script.

# Then change the DTU model of the database from Standard to Basic.

$ResourceGroupName = "powershell-grp"
$ServerName = "dbserver422bdc"
$DatabaseName = "appdb"

# Get what is the current service level objective

$SQLDatabase = Get-AzSqlDatabase -DatabaseName $DatabaseName -ResourceGroupName $ResourceGroupName `
-ServerName $ServerName

'Current Service Level Objective is ' + $SQLDatabase.RequestedServiceObjectiveName

$NewServiceLevelObjective = "Basic"

Set-AzSqlDatabase -DatabaseName $DatabaseName -ResourceGroupName $ResourceGroupName `
-ServerName $ServerName -RequestedServiceObjectiveName $NewServiceLevelObjective

# Get the new service level objective

$SQLDatabase = Get-AzSqlDatabase -DatabaseName $DatabaseName -ResourceGroupName $ResourceGroupName `
-ServerName $ServerName

'Current Service Level Objective is ' + $SQLDatabase.RequestedServiceObjectiveName

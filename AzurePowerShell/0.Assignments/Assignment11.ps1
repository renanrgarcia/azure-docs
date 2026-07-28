# Ensure that you have an Azure SQL Database and database server in place.

# Then deploy another database on the database server.

$ResourceGroupName = "powershell-grp"
$ServerName = "dbserver422bdc"
$DatabaseName = "appdb2"

# Deploy a new database on the existing database server

New-AzSqlDatabase -ResourceGroupName $ResourceGroupName -ServerName $ServerName `
-DatabaseName $DatabaseName -RequestedServiceObjectiveName "S0"

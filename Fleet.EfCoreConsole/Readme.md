# Fleet Manager

## EF Core

### Migrations

Add-Migration <MigrationName> -context FleetContext -project 'Fleet.Library'
Update-Database -context FleetContext

#### Reverting
Update-Database <MigrationName> -context FleetContext
Remove-Migration -context FleetContext -project 'Fleet.Library'


# Accounts

## EF Core

### Migrations

Add-Migration <MigrationName> -context AccountsContext -project 'Fleet.Identity'
Update-Database -context AccountsContext

#### Reverting
Update-Database <MigrationName> -context AccountsContext
Remove-Migration -context AccountsContext -project 'Fleet.Identity'



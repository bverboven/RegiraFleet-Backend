# Fleet Manager

## EF Core

Supported [Type]:
- PostgreSQL
- MySQL

### Migrations

Add-Migration [MigrationName] -context Fleet[Type]Context -project 'Fleet.Data.[Type]'
Update-Database -context Fleet[Type]Context

#### Reverting
Update-Database [MigrationName] -context Fleet[Type]Context
Remove-Migration -context Fleet[Type]Context -project 'Fleet.Data.[Type]'


# Accounts

## EF Core

### Migrations

Add-Migration [MigrationName] -context Accounts[Type]Context -project 'Fleet.Identity.Data.[Type]'
Update-Database -context Accounts[Type]Context

#### Reverting
Update-Database [MigrationName] -context Accounts[Type]Context
Remove-Migration -context Accounts[Type]Context -project 'Fleet.Identity.Data.[Type]'



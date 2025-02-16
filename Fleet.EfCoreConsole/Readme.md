# Fleet Manager

## EF Core

Supported Databases:
- PostgreSQL
- MySQL
- SqlServer

### Migrations

Add-Migration [MigrationName] -context Fleet[Database]Context -project 'Fleet.Data.[Database]'
Update-Database -context Fleet[Database]Context

```
Add-Migration [MigrationName] -context FleetMySqlContext -project 'Fleet.Data.MySQL'
Update-Database -context FleetMySqlContext
Add-Migration [MigrationName] -context FleetPostgresContext -project 'Fleet.Data.PostgreSQL'
Update-Database -context FleetPostgresContext
Add-Migration [MigrationName] -context FleetSqlServerContext -project 'Fleet.Data.SqlServer'
Update-Database -context FleetSqlServerContext
```

#### Statics views

Add new (empty) migration. Insert following code.
```cs
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        foreach (var sql in StatisticsViews.All)
        {
            migrationBuilder.Sql(sql);
        }
    }
```


#### Reverting
```
Update-Database [MigrationName] -context Fleet[Database]Context
Remove-Migration -context Fleet[Database]Context -project 'Fleet.Data.[Database]'
```

# Accounts

## EF Core

### Migrations

Add-Migration [MigrationName] -context Accounts[Database]Context -project 'Fleet.Identity.Data.[Database]'
Update-Database -context Accounts[Database]Context

```
Add-Migration [MigrationName] -context AccountsMySqlContext -project 'Fleet.Identity.Data.MySQL'
Add-Migration [MigrationName] -context AccountsPostgresContext -project 'Fleet.Identity.Data.PostgreSQL'
Add-Migration [MigrationName] -context AccountsSqlServerContext -project 'Fleet.Identity.Data.SqlServer'
```

#### Reverting
```
Update-Database [MigrationName] -context Accounts[Database]Context
Remove-Migration -context Accounts[Database]Context -project 'Fleet.Identity.Data.[Database]'
```

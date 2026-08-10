# Fleet Backend

A sample **Fleet Management** API built on top of the [Regira Entities](https://regira.github.io/Regira-Packages/src/Common.Entities/) framework.

The project tracks **vehicles**, the **interventions** (maintenance, repairs, inspections) performed on them, the **operators** (garages, suppliers) who carry them out, and the resulting **invoices**. It is multi-tenant from the ground up and shows how Regira Entities handles filtering, sorting, includes, DTO mapping, normalization, attachments, and DI wiring in a realistic, non-trivial domain.

The headline feature of this sample is **database portability**: the exact same domain model and service layer runs unchanged on **SQL Server**, **PostgreSQL**, or **MySQL**. The active provider is chosen by a single configuration value — no code changes required.

| Site | URL |
|---|---|
| 🏢 Regira | [regira.com](https://www.regira.com) |
| 🚗 Live demo | [fleet-demo.regira.com](https://fleet-demo.regira.com/) |
| 📚 Regira Entities | [Regira Entities framework](https://regira.github.io/Regira-Packages/src/Common.Entities/) |
| 📦 Package sources | [Regira/Regira-Packages](https://github.com/Regira/Regira-Packages) |
| 🍳 Sibling demo (PIM) | [Regira/Regira-PIM-Backend](https://github.com/Regira/Regira-PIM-Backend) |

> ⚠️ **A valid license for Regira Entities is required to run this example.** You can request one — including a free trial — at [regira.com/licensing?product=regira.entities](https://regira.com/licensing?product=regira.entities#request). See [License](#license) for details.

---

## Stack

| Layer | Technology |
|---|---|
| Framework | .NET 10 / ASP.NET Core |
| ORM | [Entity Framework Core](https://www.nuget.org/packages/microsoft.entityframeworkcore/) |
| Databases | SQL Server · PostgreSQL · MySQL (interchangeable, see below) |
| DTO mapping | [Mapster](https://www.nuget.org/packages/Mapster/) via `Regira.Entities.Mapping.Mapster` |
| Authentication | JWT Bearer + ASP.NET Identity via `Regira.Security.Authentication.Web` |
| Email | [MailGun](https://www.mailgun.com/) via `Regira.Office.Mail.MailGun` |
| Excel export | [NPOI](https://www.nuget.org/packages/Regira.Office.Excel.NpoiMapper) / [EPPlus](https://www.nuget.org/packages/Regira.Office.Excel.EPPlus) via `Regira.Office.Excel` |
| API docs | OpenAPI + [Swagger UI](https://www.nuget.org/packages/Swashbuckle.AspNetCore.SwaggerUI) |
| Logging | [Serilog](https://www.nuget.org/packages/Serilog) |

---

## Project layout

The solution is organised into folders that mirror the runtime boundaries. The data layer is deliberately split per database provider.

```
APIs/
  Fleet.Manager.API/          ← the main runnable API (tenant-facing)
  Fleet.Admin.API/            ← back-office API (tenants, users, cultures)

Manager/                      ← the fleet domain
  Fleet.Models/               ← domain entities, DTOs, search objects
  Fleet.Data.Core/            ← FleetContextBase (the shared EF model)
  Fleet.Data.SqlServer/       ← SQL Server context + migrations
  Fleet.Data.PostgreSQL/      ← PostgreSQL context + migrations
  Fleet.Data.MySQL/           ← MySQL context
  Fleet.DependencyInjection/  ← AddFleet(), per-entity .For<>() wiring
  Fleet.Library/              ← shared helpers

Identity/                     ← authentication & multi-tenancy
  Fleet.Identity.Models/      ← FleetUser, Tenant, claims
  Fleet.Identity.Data.Core/   ← AccountsContextBase (ASP.NET Identity)
  Fleet.Identity.Data.SqlServer / .PostgreSQL / .MySQL
  Fleet.Identity / .Web / .DependencyInjection

Tools/
  DemoData.Console/           ← drops, migrates & seeds a demo database
  Fleet.EfCoreConsole/        ← design-time host for `dotnet ef` migrations
  UserManager.Console/        ← creates super-user / admin accounts

Fleet.Core/                   ← constants, abstractions, global query filters
```

There are two databases: a **fleet** database (vehicles, interventions, …) and a separate **accounts** database (ASP.NET Identity users, tenants, claims). Each can independently target any of the three supported providers.

---

## Multi-database support

This is what the sample is really about. The pattern is the same for both the fleet data and the identity data.

### 1. One shared model, one context per provider

`FleetContextBase` (in `Fleet.Data.Core`) defines the entire EF model once — `DbSet`s, indexes, relationships, decimal precision, tenant indexes:

```csharp
public abstract class FleetContextBase(DbContextOptions options) : DbContext(options), IFleetDbContext
{
    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<Intervention> Interventions { get; set; }
    // …

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // shared indexes, keys, relationships for every provider
        modelBuilder.SetDecimalPrecisionConvention(9, 2);
    }
}
```

Each provider then derives a thin context that only carries what is *provider-specific* — table naming, `snake_case` conventions, and the occasional index that one engine accepts and another rejects:

```csharp
// Fleet.Data.PostgreSQL
public class FleetPostgresContext(DbContextOptions<FleetPostgresContext> options) : FleetContextBase(options)
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseSnakeCaseNamingConvention();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<OperatorAddress>(e =>
        {
            e.ToTable("intervention_operator_addresses");
            e.HasIndex(cd => cd.NormalizedContent); // causes an error on MySQL
        });
        // …
    }
}
```

| Provider | Context | EF Core package |
|---|---|---|
| SQL Server | `FleetSqlServerContext` | `Microsoft.EntityFrameworkCore.SqlServer` |
| PostgreSQL | `FleetPostgresContext` | `Npgsql.EntityFrameworkCore.PostgreSQL` |
| MySQL | `FleetMySqlContext` | `Pomelo.EntityFrameworkCore.MySql` |

### 2. The provider is selected from configuration

`AddFleet()` reads `Database:Fleet:Type` and wires up the matching context — that's the only switch:

```csharp
// Fleet.DependencyInjection/ServiceCollectionExtensions.cs
public static FleetServiceBuilder AddDbContext(this FleetServiceBuilder builder)
    => builder.Options.DatabaseType switch
    {
        DataBaseTypes.PostgreSQL => builder.AddPgContext(builder.Options.ConnectionString),
        DataBaseTypes.MySQL      => builder.AddMySqlContext(builder.Options.ConnectionString),
        DataBaseTypes.SqlServer  => builder.AddSqlServerContext(builder.Options.ConnectionString),
        _ => throw new NotSupportedException($"Type {builder.Options.DatabaseType} not supported"),
    };
```

Every context registration goes through the same Regira Entities interceptor pipeline, so timestamps, normalization, and auto-truncation behave identically regardless of provider:

```csharp
services.AddDbContext<TContext>((sp, db) =>
{
    configureDb(db);
    db.AddPrimerInterceptors(sp);       // Created / LastModified, tenant stamping
    db.AddNormalizerInterceptors(sp);   // fills Normalized* fields for search
    db.AddAutoTruncateInterceptors();   // trims values over [MaxLength]
});
```

### 3. Provider-aware querying

The one place where SQL dialects leak through is text search. PostgreSQL gets case-insensitive `ILIKE` filters; SQL Server and MySQL use the default `LIKE`-based builders. The right implementation is swapped in at registration time:

```csharp
// Fleet.DependencyInjection/Entities/VehicleServiceCollectionExtensions.cs
_ = dbType == DataBaseTypes.PostgreSQL
    ? e.AddFilter<VehiclePostgresLikeQueryFilter>()   // EF.Functions.ILike(...)
    : e.AddFilter<VehicleLikeQueryFilter>();          // EF.Functions.Like(...)
```

The same conditional picks a Postgres-specific global "has normalized content" filter in `FleetServiceBuilder`.

### 4. Migrations live with each provider

Each provider keeps its own migrations assembly (`MigrationsAssembly(typeof(FleetPostgresContext).Assembly)`), so the generated SQL is correct for that engine:

```
Fleet.Data.SqlServer/Migrations/   →  20260304171833_InitialMigration
Fleet.Data.PostgreSQL/Migrations/  →  20260304212843_InitialMigration
```

`Fleet.EfCoreConsole` is the design-time host that `dotnet ef` uses to resolve the contexts. To add a migration, point it at the target provider's context and assembly, e.g.:

```bash
dotnet ef migrations add <Name> \
  --project Fleet.Data.PostgreSQL \
  --startup-project Fleet.EfCoreConsole \
  --context FleetPostgresContext
```

> **Note:** SQL Server and PostgreSQL ship with committed initial migrations. The MySQL context is fully wired but its migrations are generated locally as needed.

---

## Domain model

Everything in the fleet database is tenant-scoped and revolves around three clusters.

### Vehicles

A `Vehicle` belongs to a `Brand` and a `VehicleType`, carries labels and attachments, and is searchable by code, model, brand, and type.

```csharp
public class Vehicle : IFleetEntity, IEntityWithSerial, IHasCode, IArchivable,
    IHasDescription, IHasNormalizedTitle, IHasNormalizedContent,
    IHasLabels<VehicleLabel>, IHasAttachments<VehicleAttachment>
{
    public int Id { get; set; }
    public string TenantId { get; set; } = null!;
    public int? BrandId { get; set; }
    public int? VehicleTypeId { get; set; }
    public string? Code { get; set; }
    public string? Model { get; set; }
    public string? IdentificationNumber { get; set; }

    public virtual Brand? Brand { get; set; }
    public virtual VehicleType? VehicleType { get; set; }
    public ICollection<VehicleInterventionType>? InterventionTypes { get; set; }
    public ICollection<VehicleLabel>? Labels { get; set; }
    public ICollection<VehicleAttachment>? Attachments { get; set; }
}
```

### Interventions

An `Intervention` records work done on a vehicle by an operator on a given date: it has an `InterventionType`, a set of `InterventionAction`s, an optional `Invoice`, labels, and attachments.

### Operators

An `Operator` is a garage or supplier with `OperatorAddress`es and `OperatorContactData`. Operators are linked to the intervention types they can perform.

Reference data — `Country`, translatable `VehicleType` / `InterventionType` titles, and `EntityLabel`s — rounds out the model.

---

## How Regira Entities is used

> **Note:** what follows is a customised, multi-domain, multi-tenant setup. The framework default is considerably simpler — a single `UseEntities()` call with a few `.For<>()` blocks is enough to get a fully functional CRUD API. If you are working with an AI coding assistant, the Regira MCP server exposes a `get_example` tool that returns ready-to-use minimal examples for any supported package.

### Service registration

`UseEntities<FleetContextBase>()` sets up the Mapster mapping layer, default normalizing, the multi-tenant global filter, and the primers — then each domain adds its own services:

```csharp
// Fleet.DependencyInjection/FleetServiceBuilder.cs
Services.UseEntities<FleetContextBase>(c =>
{
    c.UseMapsterMapping();
    c.UseDefaults(ed => ed.ConfigureNormalizing(o => o.Transform = TextTransform.ToUpperCase));

    // every query is automatically scoped to the current tenant
    c.AddGlobalFilterQueryBuilder<FilterHasTenantQueryBuilder>();
    c.AddPrimer<HasTenantPrimer>();
    c.AddPrimer<ArchivablePrimer>();

    // Postgres ILIKE vs default LIKE
    _ = options.DatabaseType == DataBaseTypes.PostgreSQL
        ? c.AddGlobalFilterQueryBuilder<PgFilterHasNormalizedContentQueryBuilder>()
        : c.AddGlobalFilterQueryBuilder<FilterHasNormalizedContentQueryBuilder>();
})
.WithAttachments(options.FileServiceFactory!);

Entities
    .AddCountries()
    .AddInterventions(options.DatabaseType)
    .AddVehicles(options.DatabaseType)
    .AddOperators(options.DatabaseType);
```

Inside each domain extension the `.For<>()` builder wires up filters, sorting, includes, related collections, normalizers, and attachments — all in one place:

```csharp
// Fleet.DependencyInjection/Entities/VehicleServiceCollectionExtensions.cs
services.For<Vehicle, VehicleSearchObject, EntitySortBy, VehicleIncludes>(e =>
{
    e.AddFilter<VehicleFilteredQueryBuilder>();
    _ = dbType == DataBaseTypes.PostgreSQL
        ? e.AddFilter<VehiclePostgresLikeQueryFilter>()
        : e.AddFilter<VehicleLikeQueryFilter>();
    e.SortBy((query, _) => query.OrderBy(x => x.Code));
    e.AddIncludes<VehicleIncludingQueryBuilder>();
    e.AddNormalizer<VehicleNormalizer>();
    e.Related(item => item.Labels, item => item.Labels?.Prepare());
    e.Related(item => item.InterventionTypes, item => item.InterventionTypes?.Prepare());
    e.HasAttachments(item => item.Attachments);
});
```

### Filtering via SearchObject

Each entity has a typed `SearchObject` that maps cleanly to query-builder logic:

```csharp
public record VehicleSearchObject : FleetSearchObject
{
    public string? Code { get; set; }
    public string? Model { get; set; }
    public ICollection<int>? BrandId { get; set; }
    public ICollection<int>? VehicleTypeId { get; set; }
    public string? Brand { get; set; }
    public string? VehicleType { get; set; }
    public string? Title { get; set; }
    public bool? HasIntervention { get; set; }
}
```

A `GET /vehicles?brand=volvo&hasIntervention=true` is all it takes.

### Controllers

Controllers inherit `EntityControllerBase` and get CRUD + search for free; attachment endpoints come from `EntityAttachmentControllerBase`:

```csharp
[ApiController, Route("vehicles")]
public class VehicleController
    : EntityControllerBase<Vehicle, VehicleSearchObject, EntitySortBy,
                           VehicleIncludes, VehicleDto, VehicleInputDto>;

[ApiController, Route("vehicles")]
public class VehicleAttachmentController
    : EntityAttachmentControllerBase<VehicleAttachment, EntityAttachmentDto, EntityAttachmentInputDto>;
```

### Multi-tenancy

Every fleet entity implements `IHasTenantId`. A global query-builder filters all reads to the current tenant, and a primer stamps the tenant id on writes — so controllers and services never have to think about it:

```csharp
public class FilterHasTenantQueryBuilder(IFleetAppContext appContext)
    : GlobalFilteredQueryBuilderBase<IHasTenantId, int>
{
    public override IQueryable<IHasTenantId> Build(IQueryable<IHasTenantId> query, ISearchObject<int>? _)
        => query.Where(x => x.TenantId == appContext.Tenant.TenantId);
}
```

The tenant (and culture) are resolved per request from the JWT by the `AppContextLoader` middleware.

---

## Tooling

| Console app | What it does |
|---|---|
| `DemoData.Console` | Drops, migrates, and seeds a **demo** database with tenants, accounts, and fleet data. Refuses to run unless the database name contains `demo`. |
| `Fleet.EfCoreConsole` | Design-time host that exposes the contexts to `dotnet ef` for adding/applying migrations. |
| `UserManager.Console` | Creates the `super_user` role and the admin accounts listed under `Identity:AdminUsers`. |

---

## Running the API

1. Pick a database provider and create two empty databases (one for fleet data, one for accounts).
2. Configure the connection in `appsettings.json` or user secrets:

```json
{
  "Database": {
    "Fleet":    { "Type": "PostgreSQL", "ConnectionString": "Host=…;Database=…;Username=…;Password=…" },
    "Accounts": { "Type": "PostgreSQL", "ConnectionString": "Host=…;Database=…;Username=…;Password=…" }
  }
}
```

`Type` is one of `SqlServer`, `PostgreSQL`, or `MySQL`. The fleet and accounts databases can use different providers.

3. Apply migrations (or run `DemoData.Console` against a `*demo*` database to get a fully seeded instance).
4. Run the API and open Swagger UI:

```bash
dotnet run --project Fleet.Manager.API
# docs at  https://localhost:<port>/swagger
```

> ⚠️ **A license for Regira Entities is required to run this example.** See [License](#license) below.

---

## Authorization

Authentication is JWT Bearer backed by ASP.NET Identity (the `accounts` database). Access is gated by claims-based policies, enforced globally through `CanRead` / `CanWrite` authorization filters:

| Policy | Purpose |
|---|---|
| `can_read` | Read access to tenant data |
| `can_write` | Write access to tenant data |
| `admin` | Tenant administration |
| `super_user` | Cross-tenant / platform administration |

Obtain a token via the identity endpoints (`/account/login`). Claims are scoped per tenant through `TenantUserClaim`.

---

## Related packages

| Package | Role in this project |
|---|---|
| [`Regira.Entities`](https://www.nuget.org/packages/Regira.Entities) | Entity abstractions, service interfaces |
| [`Regira.Entities.DependencyInjection`](https://www.nuget.org/packages/Regira.Entities.DependencyInjection) | `UseEntities()` / `.For<>()` builder |
| [`Regira.Entities.Mapping.Mapster`](https://www.nuget.org/packages/Regira.Entities.Mapping.Mapster) | Mapster DTO pipeline integration |
| [`Regira.Entities.Web`](https://www.nuget.org/packages/Regira.Entities.Web) | `EntityControllerBase`, attachment controllers |
| [`Regira.DAL.EFcore`](https://www.nuget.org/packages/Regira.DAL.EFcore) | EF Core conventions, interceptors, helpers |
| [`Regira.Security.Authentication.Web`](https://www.nuget.org/packages/Regira.Security.Authentication.Web) | JWT bearer setup |
| [`Regira.Office.Mail.MailGun`](https://www.nuget.org/packages/Regira.Office.Mail.MailGun) | Transactional email |
| [`Regira.Office.Excel.EPPlus`](https://www.nuget.org/packages/Regira.Office.Excel.EPPlus) / [`Regira.Office.Excel.NpoiMapper`](https://www.nuget.org/packages/Regira.Office.Excel.NpoiMapper) | Excel export |
| [`Npgsql.EntityFrameworkCore.PostgreSQL`](https://www.nuget.org/packages/Npgsql.EntityFrameworkCore.PostgreSQL) · [`Pomelo.EntityFrameworkCore.MySql`](https://www.nuget.org/packages/Pomelo.EntityFrameworkCore.MySql) · [`Microsoft.EntityFrameworkCore.SqlServer`](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.SqlServer) | The three EF Core database providers |

---

## License

The sample code in this repository is licensed under the **MIT License** — see [LICENSE](LICENSE). The referenced Regira NuGet packages keep their own licenses (most are Apache-2.0; the Entities registration packages are commercially licensed with a free tier).

This example registers more entities than the free tier covers (5 simple + 2 complex), so the **Regira Entities** framework requires a valid license to run it. Without a license key the API will not start (unless validation is explicitly skipped during local development).

You can request a license — including a **free trial** — here:

➡️ **[regira.com/licensing?product=regira.entities](https://regira.com/licensing?product=regira.entities#request)**

Once you have a key, provide it via either:

- `Regira:LicenseKey` in `appsettings.json`, or
- the `Regira__LicenseKey` environment variable.

For local development only, set `REGIRA_LICENSE_SKIP_VALIDATION=true` to bypass validation.

# Add a new Entity (specialized instructions for an AI coding agent)

You are making changes in this repository to add a new domain Entity. Follow existing patterns in this solution; do not invent new architecture.

## 0. First, identify the closest existing entity
- Search for an existing entity that matches the desired behavior (tenant-scoped, archivable, normalized content, labels/lookups, attachments).
- Use that entity as a template for file placement, naming, EF Core configuration style, and DI registration.

## 1. Choose the entity complexity level (follow existing patterns)
Pick the lowest complexity level that satisfies requirements; do not over-engineer.

### 1.1 Basic entity
- Key: `int` id.
- No `SearchObject`.
- No custom sorting.
- No custom includes.

### 1.2 Simple entity
- Key: `int` id.
- Has a `SearchObject`.
- No custom sorting.
- No custom includes.

### 1.3 Complex entity
- Key: `int` id.
- Has a `SearchObject`.
- Has custom entity sorting.
- Has custom entity includes.

### 1.4 Full custom entity
- Key: `Guid` id.
- Has a `SearchObject`.
- Has custom sorting.
- Has custom includes.

Notes
- The exact base classes/interfaces and where `SearchObject` lives are solution-specific; always copy the closest existing entity implementation.
- When implementing sorting/includes, prefer reusing existing helper abstractions (e.g., query builders, specification-style objects, include helpers) already present in this solution.

## 2. Domain model
- Add the entity class to the same project and folder structure as similar entities.
- Default to `class` for EF Core entities.
- Apply least-exposure: only make members `public` when required.
- Keep nullable annotations correct; avoid null-forgiving (`!`) unless unavoidable.
- Ensure the chosen complexity level (basic/simple/complex/full-custom) is reflected consistently:
  - Key type (`int` vs `Guid`).
  - Presence/absence of `SearchObject`.
  - Presence/absence of custom sorting and includes.

## 3. EF Core persistence
- Add configuration in the correct data project(s):
  - `Fleet.Data.SqlServer`
  - `Fleet.Data.PostgreSQL`
  - `Fleet.Data.MySQL`
- Ensure the entity is included in the EF model.
- If the entity must be tenant-scoped or archivable, ensure the existing global filters/primers apply, or add the minimum required primer/filter following current patterns.
- If the entity uses a `Guid` key, ensure the provider configuration matches the existing conventions for `Guid` keys (generation, column types, indexes).

## 4. Dependency injection wiring
- Add/extend an entity-specific extension under `Fleet.DependencyInjection/Entities` named `XxxServiceCollectionExtensions`.
- Wire it into `Fleet.DependencyInjection/FleetServiceBuilder.cs` in the fluent `Entities.AddXxx(...)` chain, matching existing ordering and style.
- If the chosen complexity level requires custom sorting/includes/search:
  - Register the relevant services used by existing entities (sort builders, include builders, search providers, query builders).
  - Do not introduce new generic infrastructure when an existing pattern exists.

## 5. Mapping
- If mapping is required, use Mapster and place configuration under `Fleet.DependencyInjection/Mapping`.
- Do not introduce AutoMapper.

## 6. Normalizing / labels / global filters (only if relevant)
- If the entity participates in normalized searching/filtering, add the normalizer and ensure the correct global filter query builder is registered.

## 7. Attachments (only if the entity supports file attachments)
If the new entity needs attachments, follow the closest existing "entity with attachments" pattern.

### 7.1 Domain model
- Add an attachments/navigation model following existing conventions (e.g., link/join entity vs owned collection).
- Ensure delete behavior is consistent with similar entities (restrict/cascade/soft-delete).
- Keep attachment-related members non-public unless required by EF.

### 7.2 EF Core persistence
- Configure the relationship(s) in each provider project.
- Ensure indexes/constraints match the solution’s conventions for attachments (foreign keys, uniqueness, ordering if applicable).
- Ensure global filters (tenant/archivable) apply consistently to attachment records when required.

### 7.3 DI wiring
- Register any attachment-related services needed by the entity (e.g., attachment managers, validators, query helpers) by copying the existing pattern.

### 7.4 API / application behavior (only if applicable in this solution)
- If the solution exposes attachment endpoints/commands/queries per entity, replicate the closest existing entity’s approach.
- Keep permissions/scoping consistent with tenant rules and existing authorization patterns.

## 8. Quality gates
- Do not modify generated files (`obj/`, `*.g.cs`).
- Keep changes minimal and consistent with repository conventions.
- Ensure the solution builds (`dotnet build`).

## Output expectation
A change set that includes the new entity model, persistence configuration, DI wiring, and mapping/normalization/attachments only when required.

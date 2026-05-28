# AGENTS.md

## What this repo is

A **template scaffold** for .NET 10 APIs backed by DynamoDB. The string `{API_NAME}` is a literal placeholder in `src/Api/Configurations/Extensions/ScalarConfiguration.cs` and must be replaced when instantiating from this template. Most business logic is `throw new NotImplementedException()` — handlers, controllers, and test projects are empty placeholders.

## Build & run

```bash
# Restore (requires internal IATec NuGet feed — see NuGet setup below)
dotnet restore

dotnet build IATec.Standard.Net.Api.sln

dotnet run --project src/Api/Api.csproj
```

Solution file is `IATec.Standard.Net.Api.sln` (not `*.DynamoDB.sln` — ignore the `.DotSettings.user` name).

## NuGet: internal feed required

There is no `NuGet.config` in the repo. The `IATec.Shared.*` packages (`IATec.Shared.Api`, `IATec.Shared.Application`, `IATec.Shared.Domain`, etc.) are hosted on a private IATec feed. `dotnet restore` will fail on a new machine without configuring this feed.

## Local DynamoDB (LocalStack)

```bash
docker run --rm -it -p 4566:4566 localstack/localstack
```

Default `appsettings.json` has `AWS:UseLocalStack: true` and `ServiceUrl: http://localhost:4566`. Migrations (table creation) run **only** when `UseLocalStack: true` — in production, tables must be provisioned externally (CloudFormation, Terraform, etc.).

Starting the API without LocalStack running will crash during startup at the migration step.

## Known startup crash

`appsettings.json` is missing a `LogServiceOption` section. The API throws `ArgumentNullException` on startup because `AddLoggingService()` requires `LogServiceOption.Url` to be non-null. Add this to `appsettings.json` before starting:

```json
"LogServiceOption": {
  "Url": "http://your-log-service-url"
}
```

## Environment

Launch profile sets `ASPNETCORE_ENVIRONMENT=Local` (not `Development`). `IsDevelopment()` returns `false`. The codebase guards Scalar/OpenAPI with `!IsProduction()`, so `Local` works. Do not use `IsDevelopment()` in new code — use `!IsProduction()` to match existing convention.

- Scalar UI: `http://localhost:5015/documentation`
- OpenAPI JSON: `http://localhost:5015/openapi/v1.json`

## Project structure

```
src/
├── Api/              — Entrypoint (ASP.NET Core, no controllers yet)
├── Application/      — MediatR CQRS handlers (all NotImplementedException)
├── CrossCutting/     — MediatR behaviors wiring
├── Domain/           — Person aggregate + value objects
├── Persistence/      — DynamoDB (AWS SDK v2), migrations, converters
├── AntiCorruption/   — Typed HttpClient to IATec Log Service
├── MessageQueue/     — Empty scaffolding
├── Domain.Tests/     — Empty (no test framework installed)
└── Application.Tests/— Empty (no test framework installed)
```

All projects target `net10.0`. `Nullable=enable`, `ImplicitUsings=enable` everywhere.

## DynamoDB patterns

- **PK/SK pattern**: `"PERSON#{personId}"` / `"PERFIL#{externalId}"` in `src/Persistence/Models/PersonPersistence.cs`.
- **Migrations**: implement `IMigration` in the `Persistence` assembly — auto-discovered via reflection. No manual registration needed.
- **Table name mapping**: `AWSConfigsDynamoDB.Context.AddMapping(...)` is global, process-wide state. Adding a second mapping for the same type will conflict.
- **`PersonPersistence` parameterless constructor**: required for DynamoDB high-level API deserialization. Do not remove it even though all properties have `private set`.
- **Custom converters**: value objects in `src/Persistence/Converters/People/` implement `IPropertyConverter`. Register via `[DynamoDBProperty(typeof(XConverter))]`.

## MediatR pipeline order

Registered in `src/Application/Configurations/Extensions/MediatorConfig.cs`:
1. `ValidatorPipelineBehavior<,>` — FluentValidation runs first
2. `ExceptionPipelineBehavior<,>` — exception wrapping

## Tests

Test projects have **no test framework packages** installed. `dotnet test` succeeds silently. Before adding tests, add per project:

```bash
dotnet add src/Domain.Tests package xunit
dotnet add src/Domain.Tests package Microsoft.NET.Test.Sdk
dotnet add src/Domain.Tests package xunit.runner.visualstudio
```

## Dockerfiles

`docker/Dockerfile` and `docker/Local.Dockerfile` are **0 bytes** — intentional placeholders. Do not attempt `docker build`.

## Kubernetes secrets

`secrets/secrets.yml` is a template with `#{deployment_name}#` and `#{namespace}#` placeholders and an empty `stringData:`. Fill in before deploying.

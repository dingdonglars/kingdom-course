# Module 3.4 — OpenAPI/Swagger & Logging

> **Hook:** today the API gets two adult features. **OpenAPI** auto-generates documentation — visit `/swagger` and a UI lets anyone (or any AI) explore your endpoints, send test requests, see responses. **Logging** writes structured records of every request — for debugging, performance, and the time something goes wrong in production.

> **Words to watch**
> - **OpenAPI** — a standard description format for HTTP APIs (formerly Swagger; now both names are used)
> - **Swagger UI** — the interactive HTML page generated from an OpenAPI spec
> - **structured logging** — log entries with named fields, not just text strings
> - **`ILogger<T>`** — ASP.NET Core's logger interface, dependency-injected per consumer

---

## Why OpenAPI

**Without OpenAPI:** the only way to know your API is to read the source code or read your README. Clients re-invent assumptions. The first wrong call is in production.

**With OpenAPI:** there's a machine-readable spec at `/openapi/v1.json`. The Swagger UI at `/swagger` shows every endpoint, its parameters, request/response shapes, status codes. **Anyone can call your API in 10 seconds without reading a line of C#.**

In .NET 9+, OpenAPI is a single line: `builder.Services.AddOpenApi();` + `app.MapOpenApi();`.

## Why structured logging

`Console.WriteLine($"User {userId} did action {action}")` is *string* logging. The log entry is unstructured text — to find "all actions by user 42," you grep.

**Structured** logging:

```csharp
_log.LogInformation("User {UserId} did action {Action}", userId, action);
```

The log entry is JSON-ish (depending on sink): `{ "msg": "User did action", "UserId": 42, "Action": "tick" }`. To find all actions by user 42, you query: `WHERE UserId = 42`. **The cost difference at 10k events: hours vs seconds.**

`ILogger<T>` is the standard. Inject it; call `LogInformation`, `LogWarning`, `LogError`. The framework + your sink (Console / file / Application Insights / Seq) handle the rest.

## Delta starter

- **MODIFIED:** `Kingdom.Api/Kingdom.Api.csproj` — adds `Microsoft.AspNetCore.OpenApi` package
- **MODIFIED:** `Kingdom.Api/Program.cs` — adds OpenAPI registration + endpoint, plus `ILogger` injection in two handlers
- The `Console` and `Debug` log providers are already wired by default in ASP.NET Core templates.

## Step 0 — install OpenAPI

```powershell
cd Kingdom.Api
dotnet add package Microsoft.AspNetCore.OpenApi
```

## Step 1 — wire OpenAPI

In `Program.cs`:

```csharp
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();              // <-- add OpenAPI services

var app = builder.Build();

app.MapOpenApi();                           // <-- expose /openapi/v1.json
```

Run: visit `http://localhost:5xxx/openapi/v1.json`. JSON of your entire API.

For the interactive UI, add **Scalar** (the modern lightweight alternative to Swashbuckle's Swagger UI):

```powershell
dotnet add package Scalar.AspNetCore
```

```csharp
if (app.Environment.IsDevelopment())
    app.MapScalarApiReference();            // serves /scalar/v1
```

Visit `http://localhost:5xxx/scalar/v1` — beautiful interactive docs. **Click any endpoint, fill in the parameters, hit Try It.**

(The older `Swashbuckle.AspNetCore` package serves the classic Swagger UI at `/swagger`. Both work; pick whichever feels nicer.)

## Step 2 — add `ILogger`

In a minimal-API handler, inject `ILogger<Program>` (or any class type) just by adding it as a parameter — the DI container provides it:

```csharp
group.MapPost("/", (CreateKingdomRequest req, ILogger<Program> log) =>
{
    if (string.IsNullOrWhiteSpace(req.Name))
    {
        log.LogWarning("CreateKingdom called with empty name from {RemoteIP}",
            "(unknown)");   // M3.6 will inject the real IP
        return Results.BadRequest(new { error = "Name is required." });
    }

    var k = new Kingdom.Engine.Kingdom(req.Name.Trim(), rng, clock);
    var id = store.Save(k);
    log.LogInformation("Created kingdom {KingdomId} '{KingdomName}'", id, k.Name);
    return Results.Created($"/kingdoms/{id}", new KingdomCreated(id, k.Name));
});
```

Two log calls — one warning, one information. Run + create a kingdom + watch the console:

```
warn: Program[0]
      CreateKingdom called with empty name from (unknown)
info: Program[0]
      Created kingdom 1 'Eldoria'
```

The `{KingdomId}` and `{KingdomName}` are *placeholders* — Serilog/Application Insights / Seq capture them as named fields, not just substituted into text. **In production this is the difference between grep-and-pray and SQL-on-logs.**

Levels in order of severity:

- `LogTrace` (very chatty)
- `LogDebug` (development only)
- `LogInformation` (normal events)
- `LogWarning` (something off, app continues)
- `LogError` (something failed, user-visible)
- `LogCritical` (everything-on-fire)

## Step 3 — config

`appsettings.json` (already in the project):

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

`Microsoft.AspNetCore` is set to `Warning` so the framework's chatter doesn't drown out your messages. Tune to taste.

## Tinker

- Visit `/scalar/v1`, click `POST /kingdoms`, hit "Try It," fill in `{ "name": "Test" }`, see the 201 + Location header. **Anyone can do this**, including a future Athos in 6 months who forgot the API shape.
- Add `app.UseSerilogRequestLogging();` after installing `Serilog.AspNetCore` — every request gets a single log line with method, path, status, duration. **Free observability.**
- Try `LogLevel.Debug` for one handler — observe the increased verbosity. Then notice: when `LogLevel.Default = Information`, your `LogDebug` calls don't appear. Filtering happens *before* the message is rendered.
- Add `[ProducesResponseType(StatusCodes.Status201Created)]` style annotations (or the minimal-API equivalent `Produces<T>(...)`) so the OpenAPI spec lists the possible status codes for each endpoint.

## Name it

- **OpenAPI** — the spec format (JSON or YAML) describing an API.
- **Swagger UI / Scalar** — interactive HTML docs generated from an OpenAPI spec.
- **`AddOpenApi()` + `MapOpenApi()`** — .NET 9+'s built-in OpenAPI generator (no Swashbuckle needed).
- **`ILogger<T>`** — DI'd logger; T is the calling class for log categorisation.
- **Structured logging** — log entries with named fields, queryable.
- **Log level** — `Information` / `Warning` / `Error` etc. Filtering happens by level.

## The rule of the through-line

> **Make your API legible and observable from day one.** OpenAPI documents the *shape*; structured logging captures the *behaviour*. Both are cheap to add early and *very* hard to add when production is on fire.

## Quiz / challenge

Open `quiz.md`.

## Connect

Module 3.5 introduces **OAuth via Google** — letting users sign in. Production auth is a full topic; we do the smallest correct version (Google Sign-In + cookie auth). After 3.5, every request can know *which user* it's for.
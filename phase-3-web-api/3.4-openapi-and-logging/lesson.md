# Module 3.4 — OpenAPI and Structured Logging

The API works, but right now the only way for someone else to know what it does is to read your source code. Today the API gets two grown-up features. **OpenAPI** auto-generates documentation — visit `/swagger` (or `/scalar/v1`) and a UI lets anyone explore your endpoints, send test requests, and see real responses. **Logging** writes structured records of every request — for debugging now, and for the moment six months from now when something goes wrong in production and you have to figure out what.

Both features are cheap to add today and very expensive to add when production is on fire. We add them now while the API is small enough to grasp end-to-end.

> **Words to watch**
>
> - **OpenAPI** — a standard description format for HTTP APIs (formerly called Swagger; both names still appear)
> - **Swagger UI / Scalar** — interactive HTML pages generated from an OpenAPI spec
> - **structured logging** — log entries with named fields, not just text strings
> - **`ILogger<T>`** — ASP.NET Core's logger interface, dependency-injected per consumer

---

## Why OpenAPI

Without OpenAPI, the only way to know your API is to read the source code or read your README. Clients re-invent assumptions. The first wrong call lands in production.

With OpenAPI, there's a machine-readable spec at `/openapi/v1.json`. The Swagger UI at `/swagger` shows every endpoint, its parameters, request/response shapes, and possible status codes. Anyone can call your API in ten seconds without reading a line of C#.

In .NET 9 and later, OpenAPI is one line each side: `builder.Services.AddOpenApi();` plus `app.MapOpenApi();`.

## Why structured logging

`Console.WriteLine($"User {userId} did action {action}")` is *string* logging. The log entry is unstructured text — to find "all actions by user 42," you grep through the file.

**Structured** logging:

```csharp
_log.LogInformation("User {UserId} did action {Action}", userId, action);
```

The log entry is JSON-ish (depending on the destination): `{ "msg": "User did action", "UserId": 42, "Action": "tick" }`. To find every action by user 42, you query: `WHERE UserId = 42`. The cost difference at ten thousand log entries is hours versus seconds.

`ILogger<T>` is the standard. Inject it; call `LogInformation`, `LogWarning`, `LogError`. The framework plus your destination of choice (console, file, Application Insights, Seq) handle the rest.

## What ships in the starter

- **MODIFIED:** `Kingdom.Api/Kingdom.Api.csproj` — adds the `Microsoft.AspNetCore.OpenApi` package
- **MODIFIED:** `Kingdom.Api/Program.cs` — adds OpenAPI registration plus the endpoint, and `ILogger` injection in two handlers
- The console and debug log destinations are already wired by default in ASP.NET Core templates.

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

Run, then visit `http://localhost:5xxx/openapi/v1.json`. JSON of your entire API.

For an interactive UI, add **Scalar** — a modern lightweight alternative to the older Swashbuckle Swagger UI:

```powershell
dotnet add package Scalar.AspNetCore
```

```csharp
if (app.Environment.IsDevelopment())
    app.MapScalarApiReference();            // serves /scalar/v1
```

Visit `http://localhost:5xxx/scalar/v1`. You get interactive docs you can click around. Pick any endpoint, fill in the parameters, hit Try It.

The older `Swashbuckle.AspNetCore` package serves the classic Swagger UI at `/swagger`. Both work; pick whichever feels nicer.

## Step 2 — add `ILogger`

In a minimal-API handler, you can inject `ILogger<Program>` (or any class type) just by adding it as a parameter — the dependency-injection container provides it for free:

```csharp
group.MapPost("/", (CreateKingdomRequest req, ILogger<Program> log) =>
{
    if (string.IsNullOrWhiteSpace(req.Name))
    {
        log.LogWarning("CreateKingdom called with empty name from {RemoteIP}",
            "(unknown)");   // Module 3.6 will inject the real IP
        return Results.BadRequest(new { error = "Name is required." });
    }

    var k = new Kingdom.Engine.Kingdom(req.Name.Trim(), rng, clock);
    var id = store.Save(k);
    log.LogInformation("Created kingdom {KingdomId} '{KingdomName}'", id, k.Name);
    return Results.Created($"/kingdoms/{id}", new KingdomCreated(id, k.Name));
});
```

Two log calls — one warning, one information. Run, create a kingdom, watch the console:

```
warn: Program[0]
      CreateKingdom called with empty name from (unknown)
info: Program[0]
      Created kingdom 1 'Eldoria'
```

The `{KingdomId}` and `{KingdomName}` are *placeholders* — Serilog, Application Insights, or Seq capture them as named fields rather than just substituting them into text. In production, that's the difference between grep-and-pray and SQL-on-logs.

Levels in order of severity:

- `LogTrace` — very chatty
- `LogDebug` — development only
- `LogInformation` — normal events
- `LogWarning` — something off, app continues
- `LogError` — something failed, user-visible
- `LogCritical` — everything-on-fire

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

`Microsoft.AspNetCore` is set to `Warning` so the framework's built-in chatter doesn't drown out your messages. Tune to taste.

## Tinker

Visit `/scalar/v1`, click `POST /kingdoms`, hit Try It, fill in `{ "name": "Test" }`, and watch the 201 plus the `Location` header come back. Anyone can do this — including future-you in six months who forgot the API's exact layout.

Add `app.UseSerilogRequestLogging();` after installing `Serilog.AspNetCore`. Every request now gets a single log line with method, path, status, and duration. Free observability.

Try `LogLevel.Debug` for one handler. Notice the increased verbosity. Then notice: when `LogLevel.Default = Information`, your `LogDebug` calls don't appear. Filtering happens *before* the message is rendered.

Add the minimal-API equivalent of `[ProducesResponseType(StatusCodes.Status201Created)]` — `Produces<T>(...)` — so the OpenAPI spec lists the possible status codes for each endpoint.

## The through-line

Make your API legible and observable from day one. OpenAPI documents the *form* of each endpoint; structured logging captures the *behaviour*. Both are cheap to add early and very hard to add when production is on fire.

## What you just did

You turned your API from *a working endpoint set* into *a documented, observable service*. Three new lines added the OpenAPI spec, the Scalar UI, and structured logs to your handlers. Anyone can now hit `/scalar/v1` and explore your API without reading a line of C#. Every request you handle leaves a structured record with named fields that real log tools can query. Two skills that took years to standardise across the industry, available from the start of your API's life.

**Key concepts you can now name:**

- **OpenAPI** — the spec format describing an API in JSON or YAML
- **Swagger UI / Scalar** — interactive HTML docs generated from that spec
- **`AddOpenApi` / `MapOpenApi`** — .NET's built-in OpenAPI generator, no third-party package needed
- **`ILogger<T>`** — DI-supplied logger; the type parameter is the calling class for filtering
- **structured logging** — log entries with named fields, queryable later

## Wrap up

1. **Quiz** — open `quiz.md`, jot your answers in `journal/quiz-notes.md`.
2. **Progress** — one line in `journal/progress.md`: `Module 3.4 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit and push** — stage both files, commit message `Module 3.4 done`, Sync.
4. **Post in `#wins`** — one line about today, plus the URL of the commit.

Module 0.1 covers the why and the panel/CLI steps if you need a refresher. Bring quiz answers you're least sure about to the next weekly sync.

## Next

Module 3.5 introduces **OAuth via Google** — letting users sign in. Production auth is a full topic; we do the smallest correct version (Google Sign-In plus cookie auth). After 3.5, every request can know *which user* it's for.

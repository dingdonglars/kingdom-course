# Module 3.4 — OpenAPI and Structured Logging

The API works, but right now the only way for someone else to learn what it does is to read your source code. Today the API gets two features that real services have. **OpenAPI** writes the documentation for you — visit `/swagger` (or `/scalar/v1`) and a web page lets anyone explore your endpoints, send test requests, and see real responses. **Logging** writes a tidy record of every request — useful for fixing bugs now, and useful six months from now when something goes wrong in production and you have to work out what happened.

Both features are cheap to add today and very expensive to add later, once the API is live and something is broken. We add them now while the API is still small enough to understand from end to end.

> **Words to watch**
>
> - **OpenAPI** — a standard description format for HTTP APIs (formerly called Swagger; both names still appear)
> - **Swagger UI / Scalar** — interactive HTML pages generated from an OpenAPI spec
> - **structured logging** — log entries with named fields, not just text strings
> - **`ILogger<T>`** — ASP.NET Core's logger interface; you ask for it as a parameter and the framework provides it

---

## Why OpenAPI

Without OpenAPI, the only way to learn your API is to read the source code or your README. Each client team has to guess how it works. They guess wrong, and the wrong call ends up in production.

With OpenAPI, there's a description a machine can read at `/openapi/v1.json`. The Swagger UI at `/swagger` shows every endpoint: its parameters, the layout of the request and response, and the status codes it can return. Anyone can call your API in ten seconds without reading a line of C#.

In .NET 9 and later, OpenAPI is one line each side: `builder.Services.AddOpenApi();` plus `app.MapOpenApi();`.

## Why structured logging

`Console.WriteLine($"User {userId} did action {action}")` is *string* logging. The log entry is just text with no structure — to find "all actions by user 42," you have to search through the file by hand.

**Structured** logging:

```csharp
_log.LogInformation("User {UserId} did action {Action}", userId, action);
```

The log entry now has named fields (the exact form depends on where the logs go): `{ "msg": "User did action", "UserId": 42, "Action": "tick" }`. To find every action by user 42, you run a query: `WHERE UserId = 42`. With ten thousand log entries, that's the difference between hours of searching and a few seconds.

`ILogger<T>` is the standard way to do this. Ask for it as a parameter, then call `LogInformation`, `LogWarning`, `LogError`. The framework and the place you send the logs to (console, a file, Application Insights, Seq) handle the rest.

## What ships in the starter

- **MODIFIED:** `Kingdom.Api/Kingdom.Api.csproj` — adds the `Microsoft.AspNetCore.OpenApi` package
- **MODIFIED:** `Kingdom.Api/Program.cs` — adds OpenAPI registration plus the endpoint, and `ILogger` injection in two handlers
- The console and debug log outputs are already set up by default in ASP.NET Core templates.

## Step 0 — install OpenAPI

```powershell
cd Kingdom.Api
dotnet add package Microsoft.AspNetCore.OpenApi
```

## Step 1 — set up OpenAPI

In `Program.cs`:

```csharp
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();              // <-- add OpenAPI services

var app = builder.Build();

app.MapOpenApi();                           // <-- expose /openapi/v1.json
```

Run, then visit `http://localhost:5xxx/openapi/v1.json`. That's your whole API, described as JSON.

For a page you can click through, add **Scalar** — a newer, lightweight alternative to the older Swashbuckle Swagger UI:

```powershell
dotnet add package Scalar.AspNetCore
```

```csharp
if (app.Environment.IsDevelopment())
    app.MapScalarApiReference();            // serves /scalar/v1
```

Visit `http://localhost:5xxx/scalar/v1`. You get docs you can click around in. Pick any endpoint, fill in the parameters, and press Try It.

The older `Swashbuckle.AspNetCore` package shows the classic Swagger UI at `/swagger`. Both work; pick whichever one you prefer.

## Step 2 — add `ILogger`

In a minimal-API handler, you can get an `ILogger<Program>` (or any class type) just by adding it as a parameter — the dependency-injection container provides it for free:

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

The `{KingdomId}` and `{KingdomName}` are *placeholders*. Tools like Serilog, Application Insights, or Seq save them as named fields, instead of only dropping them into a line of text. In production, that's the difference between searching log files by hand and running a query.

Levels, from least to most serious:

- `LogTrace` — very chatty, tiny details
- `LogDebug` — for development only
- `LogInformation` — normal events
- `LogWarning` — something's off, but the app keeps going
- `LogError` — something failed, and the user can see it
- `LogCritical` — everything is broken

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

`Microsoft.AspNetCore` is set to `Warning` so the framework's own messages don't bury yours. Adjust it however you like.

## Tinker

Visit `/scalar/v1`, click `POST /kingdoms`, press Try It, fill in `{ "name": "Test" }`, and watch the 201 and the `Location` header come back. Anyone can do this — including you in six months, once you've forgotten the API's exact layout.

Add `app.UseSerilogRequestLogging();` after installing `Serilog.AspNetCore`. Every request now gets a single log line with the method, path, status, and how long it took. That's a lot of visibility for one line.

Set `LogLevel.Debug` for one handler. Notice how much more it prints. Then notice this: when `LogLevel.Default = Information`, your `LogDebug` calls don't show up at all. The level filter runs *before* the message is built.

Add the minimal-API version of `[ProducesResponseType(StatusCodes.Status201Created)]` — `Produces<T>(...)` — so the OpenAPI description lists the status codes each endpoint can return.

## The main point

Make your API easy to read and easy to watch from day one. OpenAPI describes the *layout* of each endpoint. Structured logging records what actually *happens*. Both are cheap to add early and very hard to add later, once the API is live and something is broken.

## What you just did

You turned your API from *a set of working endpoints* into *a service that's documented and easy to watch*. Three new lines added the OpenAPI description, the Scalar UI, and structured logs to your handlers. Anyone can now open `/scalar/v1` and explore your API without reading a line of C#. Every request you handle leaves a record with named fields that real log tools can query. These are two skills the industry took years to settle on, and you have them from the very start of your API's life.

**Key concepts you can now name:**

- **OpenAPI** — the spec format describing an API in JSON or YAML
- **Swagger UI / Scalar** — interactive HTML docs generated from that spec
- **`AddOpenApi` / `MapOpenApi`** — .NET's built-in OpenAPI generator, no third-party package needed
- **`ILogger<T>`** — a logger the framework provides; the type `T` names the calling class, used for filtering
- **structured logging** — log entries with named fields, queryable later

## On your own

Time to put the book away. Don't scroll back up to the steps — write a structured log line from your own head. No one marks this one — it's just for you. It's the easiest way to spot what *hasn't* stuck yet, while it's still simple to fix. Getting stuck here is completely fine — that's exactly what it's for.

Imagine you have a logger called `log` and two values: an `int id` and a `string name`. Without looking:

1. Write one `LogInformation` call that records "deleted a kingdom" *and* keeps the id and name as named fields (not text glued into the sentence).
2. Then say, in a sentence, why the named-field form beats `Console.WriteLine`.

<details><summary>Stuck? Open this to check yourself.</summary>

```csharp
log.LogInformation("Deleted kingdom {KingdomId} '{KingdomName}'", id, name);
```

The `{KingdomId}` and `{KingdomName}` are placeholders, and the values come after as arguments. A log tool saves them as named fields, so later you can run a query like `WHERE KingdomId = 42` instead of searching a text file by hand. With ten thousand log lines, that's the difference between seconds and hours.

</details>

## Wrap up

1. **Quiz** — open `quiz.md`, jot your answers in `journal/quiz-notes.md`.
2. **Progress** — one line in `journal/progress.md`: `Module 3.4 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit and push** — stage both files, commit message `Module 3.4 done`, Sync.
4. **Post in `#wins`** — one line about today, plus the URL of the commit.

Module 0.1 covers the why and the panel/CLI steps if you need a refresher. Bring quiz answers you're least sure about to the next weekly sync.

## Next

Module 3.5 introduces **OAuth via Google** — letting users sign in. Real-world auth is a big topic, so we do the smallest version that's still correct (Google Sign-In plus cookie auth). After 3.5, every request can know *which user* it's for.

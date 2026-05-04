# Module 3.1 — HTTP & Your First Endpoint

> **Hook:** the kingdom now lives at `http://localhost:5000/kingdom`. Open the URL in a browser; the kingdom is JSON. **The browser is just one of infinitely many clients now.** A friend's curl, a JavaScript fetch, your phone — all the same shell talking to the same engine.

> **Words to watch**
> - **HTTP** — Hypertext Transfer Protocol — the language clients and servers speak
> - **request** / **response** — one client → server message and the reply
> - **verb (method)** — `GET`, `POST`, `PUT`, `DELETE` — what the client wants to do
> - **status code** — the 3-digit number describing how the request went (200 OK, 404 Not Found, 500 Server Error)
> - **minimal API** — ASP.NET Core's lightweight syntax for building HTTP endpoints (`app.MapGet(...)`)

---

## HTTP, in one screen

A client (browser, curl, mobile app) sends a **request**:

```
GET /kingdom HTTP/1.1
Host: localhost:5000
Accept: application/json
```

A server replies with a **response**:

```
HTTP/1.1 200 OK
Content-Type: application/json

{ "name": "Eldoria", "day": 11 }
```

Three parts to know: **verb + path** (`GET /kingdom`), **status code** (200, 404, 500), **body** (the JSON). Everything else is detail.

The five verbs you'll meet today and tomorrow:

| Verb | Use | Idempotent? |
|---|---|---|
| `GET` | read data | yes |
| `POST` | create / take an action | no |
| `PUT` | replace an entire resource | yes |
| `PATCH` | partial update | depends |
| `DELETE` | remove | yes |

*Idempotent* = doing it twice has the same effect as doing it once. Important when networks blip and clients retry.

## Common status codes

| Code | Meaning |
|---|---|
| 200 OK | success |
| 201 Created | success, a new thing was made |
| 204 No Content | success, no body to return |
| 400 Bad Request | client sent bad data |
| 401 Unauthorized | not signed in |
| 403 Forbidden | signed in but not allowed |
| 404 Not Found | resource doesn't exist |
| 500 Internal Server Error | the server crashed |

You'll memorise them by using them. The split between 4xx (client's fault) and 5xx (server's fault) is the most important distinction.

## Delta starter

A new project: `Kingdom.Api`.

- **NEW project:** `Kingdom.Api/` (ASP.NET Core minimal API)
- **NEW:** `Kingdom.Api/Program.cs` — sets up the host + first endpoint
- **MODIFIED:** `Kingdom.slnx` (add the new project)
- **NEW:** `tests/Kingdom.Api.Tests/` (placeholder; integration tests come in M3.7)

For this module, the API exposes one endpoint: `GET /kingdom` returns a `KingdomSummary`.

## Step 0 — create the project

```powershell
dotnet new web -n Kingdom.Api
dotnet add Kingdom.Api reference Kingdom.Engine
dotnet add Kingdom.Api reference Kingdom.Persistence
dotnet sln Kingdom.slnx add Kingdom.Api
```

`dotnet new web` creates a minimal API project. The default `Program.cs` already has a `MapGet("/", () => "Hello World!")` — you can run it now and visit <http://localhost:5000>.

## Step 1 — first kingdom endpoint

Replace `Kingdom.Api/Program.cs`:

```csharp
using Kingdom.Engine;
using Kingdom.Engine.Buildings;
using Kingdom.Engine.Citizens;
using Kingdom.Engine.Infrastructure;
using Kingdom.Persistence;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Build a one-off in-memory kingdom for the demo.
// (M3.5 will switch this to a per-request, persisted kingdom.)
IRandom rng = new SystemRandom(seed: 42);
var kingdom = new Kingdom.Engine.Kingdom("Eldoria", rng, new SystemClock());
kingdom.AddBuilding(new Farm("Main Farm"));
kingdom.AddBuilding(new Lumberyard("Eastern Lumberyard"));
kingdom.AddCitizen(new Citizen("Lyra"));
for (int i = 0; i < 10; i++) kingdom.AdvanceDay();

// GET /kingdom — returns the summary as JSON
app.MapGet("/kingdom", () => KingdomJsonStore.ToSummary(kingdom));

app.Run();
```

Three things to read:

- **`WebApplication.CreateBuilder(args)`** — the entry point. Configures hosting, logging, configuration sources. Returns a *builder*; you call `Build()` to get the actual `app`.
- **`app.MapGet("/kingdom", () => ...)`** — registers a route. When a `GET /kingdom` arrives, run that lambda. The return value is auto-serialised to JSON.
- **`app.Run()`** — start the server. Blocks until shut down.

That's *the entire setup*. No XML config, no servlet container. Run it:

```powershell
dotnet run --project Kingdom.Api
```

You'll see `Now listening on: http://localhost:5xxx` in the console. Open the URL in a browser. The kingdom appears as JSON.

## Step 2 — try a real client

Open another terminal:

```powershell
curl http://localhost:5xxx/kingdom
```

Same JSON. **The browser doesn't matter — `curl` doesn't matter — every client speaks HTTP.**

Try a path that doesn't exist:

```powershell
curl -i http://localhost:5xxx/nonsense
```

`-i` includes the response headers. You'll see `HTTP/1.1 404 Not Found`. That's ASP.NET handling the route-not-matched case for you.

## Step 3 — test the endpoint

`tests/Kingdom.Api.Tests/Kingdom.Api.Tests.csproj` already exists (placeholder). Add a smoke test using the `WebApplicationFactory<>`-light pattern (deeper integration testing in Module 3.7). For now, just verify the project builds and the endpoint compiles.

```csharp
namespace Kingdom.Api.Tests;

public class SmokeTests
{
    [Fact]
    public void Api_Project_Compiles()
    {
        // The fact that this test class compiles proves the API project is referenced cleanly.
        Assert.True(true);
    }
}
```

(Module 3.7 brings real HTTP integration tests with `WebApplicationFactory<Program>`.)

## Tinker

- Hit your URL from a Discord message. **A friend can hit it too** if you tunnel with `ngrok http 5000`. (Don't deploy yet — there's no auth.)
- Add a second endpoint: `app.MapGet("/", () => "Welcome, traveller.");`. Visit `/` in the browser.
- Add `app.MapGet("/buildings", () => kingdom.Buildings.Select(b => new { b.Name, Kind = b.GetType().Name, b.Level }));`. The `new { ... }` is an *anonymous object* — perfect for one-off API shapes.
- Stop the server with Ctrl+C and try the URL again. Browser shows "Connection refused." That's how you know the server actually was the thing serving the page.

## Name it

- **HTTP** — the protocol. Verb + path + headers + (optional) body in the request; status + headers + body in the response.
- **`MapGet` / `MapPost` / `MapPut` / `MapDelete`** — minimal-API methods to register routes.
- **`WebApplication.CreateBuilder(args)`** — entry point. Returns a builder, configure, then `Build()` to get `app`.
- **`app.Run()`** — start the server, block until shutdown.
- **Status code (2xx, 4xx, 5xx)** — quick at-a-glance signal: success, client-fault, server-fault.

## The rule of the through-line

> **The API is a shell.** Same engine, fourth host. The `Program.cs` reads inputs (HTTP requests), calls into the engine + persistence, returns outputs (HTTP responses). The engine has not changed.

That's the *third* shell now: console (Block 3), persistence (Block 4 — sort of), web API (Block 5). Still the same engine.

## Quiz / challenge

Open `quiz.md`.

## Connect

Module 3.2 introduces **DTOs at the API boundary** — the same pattern as JSON persistence. The API's request/response shapes are *not* the engine model; they're DTOs designed for the wire. We'll add a `POST /kingdom/tick` endpoint and watch the kingdom advance over HTTP.
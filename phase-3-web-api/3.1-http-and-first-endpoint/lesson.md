# Module 3.1 — HTTP and Your First Endpoint

By the end of today, your kingdom will live at `http://localhost:5000/kingdom`. You'll open the URL in a browser and the kingdom will appear as JSON. The browser is just one of infinitely many clients now — a friend's `curl`, a JavaScript fetch, your phone — they all speak the same language and all hit the same engine. The console you wrote in Phase 1 was one outer layer; today you add another.

We're going to learn HTTP in one screen, then write a single ASP.NET endpoint that serves your kingdom over the network. *ASP.NET Core* is Microsoft's web framework — the part of .NET that handles HTTP. Its **minimal API** style is the new, lightweight way of writing endpoints: a few lines instead of a folder full of controllers.

> **Words to watch**
>
> - **HTTP** — Hypertext Transfer Protocol — the language clients and servers speak over the network
> - **request** / **response** — one client-to-server message and the reply that comes back
> - **verb (or method)** — `GET`, `POST`, `PUT`, `DELETE` — what the client wants to do
> - **status code** — the three-digit number describing how the request went (200 OK, 404 Not Found, 500 Server Error)
> - **minimal API** — ASP.NET Core's lightweight syntax for writing endpoints (`app.MapGet(...)`)

---

## HTTP, in one screen

A client (a browser, `curl`, a mobile app — anything) sends a **request**:

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

Three parts you have to know: the **verb plus path** (`GET /kingdom`), the **status code** (200, 404, 500), and the **body** (the JSON). Everything else is detail you can pick up as you need it.

The five verbs you'll meet today and tomorrow:

| Verb | What it does | Idempotent? |
|---|---|---|
| `GET` | read data | yes |
| `POST` | create something or take an action | no |
| `PUT` | replace an entire resource | yes |
| `PATCH` | partial update | depends |
| `DELETE` | remove | yes |

*Idempotent* (eye-dem-poh-tent) means doing the action twice has the same effect as doing it once. That matters when networks blip and clients retry — you don't want a retry to charge a credit card a second time.

## Common status codes

| Code | Meaning |
|---|---|
| 200 OK | success |
| 201 Created | success — a new thing was made |
| 204 No Content | success — there's no body to return |
| 400 Bad Request | the client sent bad data |
| 401 Unauthorized | the client isn't signed in |
| 403 Forbidden | signed in but not allowed |
| 404 Not Found | the resource doesn't exist |
| 500 Internal Server Error | the server crashed |

You'll memorise these by using them. The most important split is between 4xx (client's fault) and 5xx (server's fault). Everything else is detail.

## What ships in the starter

A new project — `Kingdom.Api` — plus a placeholder test project. The earlier projects don't change.

- **NEW:** `Kingdom.Api/` (ASP.NET Core minimal API)
- **NEW:** `Kingdom.Api/Program.cs` — sets up the host and the first endpoint
- **MODIFIED:** `Kingdom.slnx` — adds the new project to the solution
- **NEW:** `tests/Kingdom.Api.Tests/` — placeholder; real integration tests come in M3.7

For this module the API has one endpoint: `GET /kingdom` returns a `KingdomSummary`.

## Step 0 — create the project

```powershell
dotnet new web -n Kingdom.Api
dotnet add Kingdom.Api reference Kingdom.Engine
dotnet add Kingdom.Api reference Kingdom.Persistence
dotnet sln Kingdom.slnx add Kingdom.Api
```

`dotnet new web` creates a minimal API project. The default `Program.cs` it generates already has a `MapGet("/", () => "Hello World!")` line — you can run it now and visit <http://localhost:5000> to see it work.

## Step 1 — your first kingdom endpoint

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
// (Module 3.5 switches this to a per-request, persisted kingdom.)
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

Three lines to slow down on:

- **`WebApplication.CreateBuilder(args)`** — the entry point. Configures hosting, logging, and configuration sources. It returns a *builder*; you call `Build()` on it to get the actual `app`.
- **`app.MapGet("/kingdom", () => ...)`** — registers a route. When a `GET /kingdom` arrives, run that lambda. The return value is automatically turned into JSON.
- **`app.Run()`** — start the server. This call blocks until you shut it down.

That's the entire setup. No XML config, no servlet container. Run it:

```powershell
dotnet run --project Kingdom.Api
```

You'll see `Now listening on: http://localhost:5xxx` in the console. Open the URL in a browser. The kingdom appears as JSON.

## Step 2 — try a real client

Open another terminal:

```powershell
curl http://localhost:5xxx/kingdom
```

Same JSON. The browser doesn't matter, `curl` doesn't matter — every client speaks HTTP. *`curl`* (pronounced like the word "curl") is a command-line tool that sends HTTP requests and prints the response.

Try a path that doesn't exist:

```powershell
curl -i http://localhost:5xxx/nonsense
```

The `-i` flag tells `curl` to include the response headers in the output. You'll see `HTTP/1.1 404 Not Found`. That's ASP.NET handling the route-not-matched case for you, with no extra code on your part.

## Step 3 — test the endpoint

`tests/Kingdom.Api.Tests/Kingdom.Api.Tests.csproj` already exists as a placeholder. Add a smoke test using a light pattern (real integration testing arrives in Module 3.7). For now, just verify the project builds and the endpoint compiles:

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

Module 3.7 brings real HTTP integration tests with `WebApplicationFactory<Program>`.

## Tinker

Hit your URL from a chat message. A friend can hit it too if you tunnel with `ngrok http 5000`. Don't deploy yet — there's no auth, and anyone could mess with your data.

Add a second endpoint: `app.MapGet("/", () => "Welcome, traveller.");`. Visit `/` in the browser.

Add `app.MapGet("/buildings", () => kingdom.Buildings.Select(b => new { b.Name, Kind = b.GetType().Name, b.Level }));`. The `new { ... }` is an *anonymous object* — ideal for one-off API responses.

Stop the server with Ctrl+C and try the URL again. The browser shows "Connection refused." That's how you know your own running process was the thing serving the page — when it's not running, nothing answers.

## The through-line, again

The API is another outer layer. Same engine, fourth host. `Program.cs` reads inputs (HTTP requests), calls into the engine plus persistence, returns outputs (HTTP responses). The engine has not changed at all.

That's the third outer layer now: console (Phase 1), persistence (Phase 2 — sort of), web API (this phase). Still the same engine inside.

## What you just did

Your kingdom is now reachable over HTTP. You created a new project, wrote a single `MapGet("/kingdom", ...)` line, and exposed your engine to any client in the world that speaks HTTP. You also met the request-response model in concrete form — verb, path, status code, body — and watched ASP.NET handle the boring parts (404 routing, JSON serialisation, content type headers) without a line of code from you. The engine you wrote in Phase 1 didn't change at all; only the outer layer is new. One endpoint live; many more to come.

**Key concepts you can now name:**

- **HTTP** — verb plus path plus body, with a status code in reply
- **status codes** — 2xx success, 4xx client's fault, 5xx server's fault
- **`MapGet` / `MapPost` / etc.** — the minimal-API way of registering routes
- **`WebApplication.CreateBuilder`** — the entry point that configures hosting and logging
- **idempotent** — doing it twice has the same effect as doing it once

## Quiz

Open `quiz.md`. When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.

## Next

Module 3.2 introduces **DTOs at the API boundary** — the same pattern as JSON persistence, applied to the wire. We'll add a `POST /kingdom/tick` endpoint and watch the kingdom advance over HTTP.

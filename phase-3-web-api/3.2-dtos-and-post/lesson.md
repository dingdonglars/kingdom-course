# Module 3.2 — DTOs at the API Boundary, and `POST /kingdom/tick`

Yesterday your kingdom was readable over HTTP. Today it *changes* over HTTP. A `POST /kingdom/tick` request advances one day; the response shows the new state. The kingdom now responds to the network the same way it responded to the keyboard in Phase 1 — same engine, same `AdvanceDay` method, just a different caller pulling the trigger.

While we're here, we'll formalise the rule we first met in Phase 2: **the data going across the wire is a DTO, not the engine type.** The engine returns objects with hidden state and constructors that need an `IRandom`; the API returns small `record` types that turn into JSON cleanly. Same lesson, second time.

> **Words to watch**
>
> - **request DTO** — the data layout the client sends in the request body
> - **response DTO** — the data layout the server returns in the response body
> - **`Results.Ok(...)` / `Results.NotFound()`** — minimal-API helpers that let you control the status code explicitly
> - **`[FromBody]`** — the attribute that says *read this parameter from the request JSON*; minimal APIs apply it implicitly for record parameters

---

## Why DTOs again

We met DTOs in Module 2.2 (JSON persistence). The same logic applies at the API boundary, doubled. The wire layout needs to:

- Turn into JSON cleanly — no constructors that need an `IRandom`, no virtual methods
- Stay *stable* even when the engine changes — adding a private field shouldn't break a client
- Stay *small* — return only what the client actually needs (saves bytes)
- Stay *explicit* — every property visible at the boundary should be intentional

`KingdomSummary` (from Module 2.2) already fits the bill for `GET /kingdom`. Today we add `TickResponse` for the new endpoint.

## What ships in the starter

- **NEW:** `Kingdom.Api/Dtos/TickResponse.cs`
- **MODIFIED:** `Kingdom.Api/Program.cs` — adds `POST /kingdom/tick` and the optional days-to-advance parameter
- **NEW:** `tests/Kingdom.Api.Tests/Endpoint_GET_Kingdom_Tests.cs` — smoke test; real integration tests come in Module 3.7

## Step 1 — the `TickResponse` DTO

`Kingdom.Api/Dtos/TickResponse.cs`:

```csharp
namespace Kingdom.Api.Dtos;

/// <summary>What the API returns after a tick.</summary>
public record TickResponse(
    int DaysAdvanced,
    string KingdomName,
    int CurrentDay,
    int Gold, int Wood, int Stone, int Food
);
```

A small explicit record. The client knows exactly what to expect — no surprises in the JSON, no risk of an internal engine field accidentally leaking out.

## Step 2 — the `POST /kingdom/tick` endpoint

Replace `Kingdom.Api/Program.cs`:

```csharp
using Kingdom.Api.Dtos;
using Kingdom.Engine;
using Kingdom.Engine.Buildings;
using Kingdom.Engine.Citizens;
using Kingdom.Engine.Infrastructure;
using Kingdom.Engine.Resources;
using Kingdom.Persistence;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

IRandom rng = new SystemRandom(seed: 42);
var kingdom = new Kingdom.Engine.Kingdom("Eldoria", rng, new SystemClock());
kingdom.AddBuilding(new Farm("Main Farm"));
kingdom.AddBuilding(new Lumberyard("Eastern Lumberyard"));
kingdom.AddCitizen(new Citizen("Lyra"));
for (int i = 0; i < 10; i++) kingdom.AdvanceDay();

// GET /kingdom — read state
app.MapGet("/kingdom", () => KingdomJsonStore.ToSummary(kingdom));

// POST /kingdom/tick — advance one or more days; return the new state
app.MapPost("/kingdom/tick", (int? days) =>
{
    var n = Math.Clamp(days ?? 1, 1, 100);   // refuse zero or absurd numbers
    for (int i = 0; i < n; i++) kingdom.AdvanceDay();

    return Results.Ok(new TickResponse(
        DaysAdvanced: n,
        KingdomName: kingdom.Name,
        CurrentDay:  kingdom.Day,
        Gold:  kingdom.Resources.Get(Resource.Gold),
        Wood:  kingdom.Resources.Get(Resource.Wood),
        Stone: kingdom.Resources.Get(Resource.Stone),
        Food:  kingdom.Resources.Get(Resource.Food)));
});

app.Run();
```

The new bits, slowly:

- **`(int? days)`** — minimal-API parameters bind by name. `days` is optional (the `?` allows it to be null); the framework will look for `?days=N` in the query string. Body binding for complex types comes from `[FromBody]`, which minimal APIs apply automatically for record parameters.
- **`Math.Clamp(days ?? 1, 1, 100)`** — input validation in one line. If `days` is null, default to 1; otherwise force the value into the range 1 to 100. Refuse the 1000-day tick that would lock up your server for a minute.
- **`Results.Ok(value)`** — return `200 OK` explicitly with `value` as the body. There's also `Results.NotFound()`, `Results.BadRequest("msg")`, `Results.Created(uri, value)`, and a few more. Use these when you want to control the status code; otherwise just `return value` and the framework picks 200 for you.

Build, run, try:

```powershell
dotnet build
dotnet run --project Kingdom.Api
# in another terminal
curl -X POST http://localhost:5xxx/kingdom/tick
curl -X POST "http://localhost:5xxx/kingdom/tick?days=5"
curl http://localhost:5xxx/kingdom
```

Each tick changes state. The `GET` reflects the new day.

## Step 3 — what the framework does for you

Notice all the things you *didn't* write:

- JSON parsing of the request (none here — `int?` is a primitive)
- JSON serialisation of the response (`TickResponse` becomes JSON automatically)
- Status code handling (`Results.Ok` becomes 200; 404 if the route doesn't match)
- Content-Type negotiation (the response gets `application/json; charset=utf-8` for free)
- Routing — `/kingdom/tick` knows it's the right handler

That's the value of a framework over a raw socket. You write the business logic; the framework handles the boilerplate.

## Step 4 — tests

For now, a placeholder smoke test confirming the project compiles. Real integration tests with `WebApplicationFactory<Program>` arrive in Module 3.7.

`tests/Kingdom.Api.Tests/Endpoint_GET_Kingdom_Tests.cs`:

```csharp
using Kingdom.Api.Dtos;
using Shouldly;

namespace Kingdom.Api.Tests;

public class Endpoint_GET_Kingdom_Tests
{
    [Fact]
    public void TickResponse_Record_HasExpectedProperties()
    {
        // Compile-time check that the DTO matches what the client expects
        var tr = new TickResponse(1, "X", 2, 100, 50, 20, 30);
        tr.DaysAdvanced.ShouldBe(1);
        tr.CurrentDay.ShouldBe(2);
    }
}
```

A real test would call `WebApplicationFactory<Program>().CreateClient()` and POST to the endpoint. We hold that for Module 3.7 — too much new ceremony to introduce in one go.

## Tinker

Try `?days=10000` and observe the clamp keeping you safe. Comment out the clamp and try again — the server still responds, but the call takes a noticeable moment. That's why we clamp.

Add a `GET /kingdom/buildings` endpoint returning `kingdom.Buildings.Select(b => new { b.Name, Kind = b.GetType().Name, b.Level })`. The anonymous record becomes JSON without you doing anything extra.

Add `GET /healthz` returning `Results.Ok("ok")`. That's the standard convention for *"is the server alive?"* — used by load balancers and monitoring tools to check whether the process is still healthy.

Try `curl -i -X POST http://localhost:5xxx/kingdom/tick`. The `-i` flag shows the response headers — `Content-Type: application/json` is auto-set, the `Date` header is auto-set, and so on.

## Validate at the boundary

Untrusted input enters at the outer layer. Clamp it, check it, reject the bad parts — all *before* the engine sees it. The engine should never have to defend itself against caller bugs. That's why `Math.Clamp` lives in the handler, not inside `AdvanceDay`.

## What you just did

Your kingdom now both reads and writes over HTTP. You wrote a `TickResponse` DTO — a small, explicit record designed for the wire — and a `POST /kingdom/tick` handler that advances the kingdom by one or more days, returning the new state. The handler validates its input with `Math.Clamp` so a thousand-day request can't lock up the server. You also met `Results.Ok(...)` and saw how the framework handles JSON serialisation, status codes, and content-type headers without you writing a line for any of it.

**Key concepts you can now name:**

- **DTO at the API boundary** — small explicit record, designed for the wire
- **`Results.Ok` / `NotFound` / `BadRequest`** — control the status code from a handler
- **optional query parameter** — `(int? days)` becomes `?days=5` in the URL
- **input validation at the boundary** — clamp, check, reject before the engine runs

## Quiz

Open `quiz.md`. When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.

## Next

Module 3.3 introduces **multiple endpoints with proper status codes** — `POST /kingdoms` to create, `DELETE /kingdoms/{id}` to remove, plus 404s and 400s when things don't fit. This is CRUD over HTTP, the pattern under every web API ever built.

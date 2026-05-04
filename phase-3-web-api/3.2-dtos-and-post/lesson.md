# Module 3.2 — DTOs at the API Boundary, and `POST /kingdom/tick`

> **Hook:** today the kingdom *changes* over HTTP. `POST /kingdom/tick` advances one day; the response shows the new state. We also formalise the rule from Block 4: **the shape on the wire is a DTO, not the engine type.** The engine returns objects with hidden state and constructors needing `IRandom`; the API returns small `record`s that JSON-serialise cleanly.

> **Words to watch**
> - **request DTO** — the data shape the client sends in the body
> - **response DTO** — the data shape the server returns
> - **`Results.Ok(...)` / `Results.NotFound()`** — explicit return types in minimal APIs that control status code
> - **`[FromBody]`** — implicit on minimal-API parameters: read this from the request JSON

---

## Why DTOs again

We met DTOs in M2.2 (JSON persistence). The same logic applies at the API boundary, doubled. The wire shape:

- Has to JSON-serialise cleanly — no constructors with `IRandom`, no virtual methods
- Should be *stable* even when the engine changes — adding a private field shouldn't break a client
- Should be *small* — return only what the client actually needs (saves bytes)
- Should be *explicit* — every property visible at the boundary should be intentional

`KingdomSummary` (from M2.2) already fits the bill for `GET /kingdom`. Today we add `TickResponse` for the new endpoint.

## Delta starter

- **NEW:** `Kingdom.Api/Dtos/TickResponse.cs`
- **MODIFIED:** `Kingdom.Api/Program.cs` — adds `POST /kingdom/tick` and the days-to-advance optional parameter
- **NEW:** `tests/Kingdom.Api.Tests/Endpoint_GET_Kingdom_Tests.cs` (smoke; real integration in M3.7)

## Step 1 — `TickResponse` DTO

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

A small explicit record. **The client knows exactly what to expect.** No surprises in the JSON.

## Step 2 — `POST /kingdom/tick` endpoint

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
    var n = Math.Clamp(days ?? 1, 1, 100);   // refuse 0 or absurd numbers
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

Read the new bits:

- **`(int? days)`** — minimal API parameters bind by name. `days` is optional (`?`); the framework will look for `?days=N` in the query string. (Body binding for complex types comes from `[FromBody]` — but minimal APIs assume that for record parameters.)
- **`Math.Clamp(days ?? 1, 1, 100)`** — input validation in one line. If `days` is null, default to 1; otherwise clamp to [1..100]. **Refuse 1000-day ticks** that would lock up your server.
- **`Results.Ok(value)`** — explicit `200 OK` with the value as the body. There's also `Results.NotFound()`, `Results.BadRequest("msg")`, `Results.Created(uri, value)`, etc. Use these when you want to control the status code.

Build + run + try:

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
- Status code handling (`Results.Ok` → 200; `404` if the route doesn't match)
- Content-Type negotiation (the response gets `application/json; charset=utf-8`)
- Routing: `/kingdom/tick` knows it's the right handler

That's the value of using a framework instead of a raw socket. **You write the business; the framework writes the boilerplate.**

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
        // Compile-time check that the DTO shape is what the client expects
        var tr = new TickResponse(1, "X", 2, 100, 50, 20, 30);
        tr.DaysAdvanced.ShouldBe(1);
        tr.CurrentDay.ShouldBe(2);
    }
}
```

A real test would `WebApplicationFactory<Program>().CreateClient()` and POST to the endpoint. We hold that until Module 3.7 — too much new ceremony for one module.

## Tinker

- POST with `?days=10000` — observe the clamp. Comment out the clamp, retry; the server responds but the call takes longer. **That's why we clamp.**
- Add a `GET /kingdom/buildings` endpoint returning `kingdom.Buildings.Select(b => new { b.Name, Kind = b.GetType().Name, b.Level })`. The anonymous record becomes JSON.
- Add `GET /healthz` returning `Results.Ok("ok")`. Standard convention for "is the server alive" — used by load balancers + monitoring.
- Try `curl -i -X POST http://localhost:5xxx/kingdom/tick` — `-i` shows the response headers. `Content-Type: application/json` auto-set; `Date` header auto-set; etc.

## Name it

- **DTO at the API boundary.** Same pattern as M2.2's `KingdomSummary`. Don't serialise your engine model directly — design the wire shape on purpose.
- **`Results.Ok` / `NotFound` / `BadRequest`** — control the status code from a minimal-API handler.
- **Optional query parameter** — `(int? days)` becomes `?days=5` in the URL.
- **Input validation at the boundary** — `Math.Clamp(...)` is the simplest defensive move. Stronger validation comes from data-annotations or a manual check.

## The rule of the through-line

> **Validate at the boundary.** The shell (HTTP) is where untrusted input enters. Clamp, check, reject — *before* the engine sees it. The engine should never have to defend itself against caller bugs.

## Quiz / challenge

Open `quiz.md`.

## Connect

Module 3.3 introduces **multiple endpoints + status codes** in earnest — `POST /kingdoms` to create, `DELETE /kingdoms/{id}`, plus 404s and 400s when things don't fit.
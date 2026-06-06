# Module 3.2 — DTOs at the API Boundary, and `POST /kingdom/tick`

Yesterday your kingdom could be read over HTTP. Today it can *change* over HTTP. A `POST /kingdom/tick` request advances one day, and the response shows the new state. The kingdom now answers the network the same way it answered the keyboard in Phase 1 — same engine, same `AdvanceDay` method, just a different caller asking for it.

While we're here, we'll set out clearly the rule we first met in Phase 2: **the data you send across the network is a DTO, not the engine type.** The engine returns objects with hidden state and constructors that need an `IRandom`. The API returns small `record` types that turn into JSON cleanly. Same lesson, second time.

> **Words to watch**
>
> - **request DTO** — the layout of the data the client sends in the request body
> - **response DTO** — the layout of the data the server returns in the response body
> - **`Results.Ok(...)` / `Results.NotFound()`** — minimal-API helpers that let you choose the status code yourself
> - **`[FromBody]`** — the attribute that says *read this parameter from the request JSON*; minimal APIs add it for you on record parameters

---

## Why DTOs again

We met DTOs in Module 2.2 (JSON persistence). The same reasoning applies at the API boundary, and it matters even more here. The data you send over the network needs to:

- Turn into JSON cleanly — no constructors that need an `IRandom`, no virtual methods
- Stay *stable* even when the engine changes — adding a private field shouldn't break a client
- Stay *small* — return only what the client actually needs (this sends fewer bytes)
- Stay *clear* — every property the client can see should be there on purpose

`KingdomSummary` (from Module 2.2) already works for `GET /kingdom`. Today we add `TickResponse` for the new endpoint.

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

A small, clear record. The client knows exactly what to expect — no surprises in the JSON, and no risk of a hidden engine field showing up by accident.

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

The new parts, slowly:

- **`(int? days)`** — minimal-API parameters are matched by name. `days` is optional (the `?` lets it be null), and the framework looks for `?days=N` in the query string. For bigger types, the data comes from the request body through `[FromBody]`, which minimal APIs add for you on record parameters.
- **`Math.Clamp(days ?? 1, 1, 100)`** — checking the input in one line. If `days` is null, use 1. Otherwise keep the value inside the range 1 to 100. This blocks the 1000-day tick that would freeze your server for a minute.
- **`Results.Ok(value)`** — return `200 OK` yourself, with `value` as the body. There's also `Results.NotFound()`, `Results.BadRequest("msg")`, `Results.Created(uri, value)`, and a few more. Use these when you want to choose the status code. Otherwise just `return value` and the framework picks 200 for you.

Build, run, try:

```powershell
dotnet build
dotnet run --project Kingdom.Api
# in another terminal
curl -X POST http://localhost:5xxx/kingdom/tick
curl -X POST "http://localhost:5xxx/kingdom/tick?days=5"
curl http://localhost:5xxx/kingdom
```

Each tick changes the state. The `GET` shows the new day.

## Step 3 — what the framework does for you

Notice all the things you *didn't* write:

- Reading the JSON from the request (none here — `int?` is a simple number)
- Turning the response into JSON (`TickResponse` becomes JSON automatically)
- Setting the status code (`Results.Ok` becomes 200; 404 if no route matches)
- Choosing the content type (the response gets `application/json; charset=utf-8` for free)
- Routing — `/kingdom/tick` finds the right handler on its own

That's what a framework gives you over writing everything from scratch. You write the part that's special to your app, and the framework handles the repetitive setup.

## Step 4 — tests

For now, a simple first test that confirms the project compiles. Real integration tests with `WebApplicationFactory<Program>` arrive in Module 3.7.

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

A real test would call `WebApplicationFactory<Program>().CreateClient()` and POST to the endpoint. We save that for Module 3.7 — it's too much new setup to learn all at once.

## Tinker

Try `?days=10000` and watch the clamp keep you safe. Comment out the clamp and try again — the server still answers, but the call takes a moment you can feel. That's why we clamp.

Add a `GET /kingdom/buildings` endpoint returning `kingdom.Buildings.Select(b => new { b.Name, Kind = b.GetType().Name, b.Level })`. The anonymous object becomes JSON without you doing anything extra.

Add `GET /healthz` returning `Results.Ok("ok")`. That's the common way to ask *"is the server alive?"* — load balancers and monitoring tools call an endpoint like this to check the program is still running.

Try `curl -i -X POST http://localhost:5xxx/kingdom/tick`. The `-i` flag shows the response headers — `Content-Type: application/json` is set for you, the `Date` header is set for you, and so on.

## Check the input at the boundary

Input you can't trust comes in at the outer layer. Clamp it, check it, and reject the bad values — all *before* the engine sees it. The engine should never have to protect itself against a caller's mistakes. That's why `Math.Clamp` is in the handler, not inside `AdvanceDay`.

## What you just did

Your kingdom now reads *and* writes over HTTP. You wrote a `TickResponse` DTO — a small, clear record made for sending over the network — and a `POST /kingdom/tick` handler that advances the kingdom by one or more days and returns the new state. The handler checks its input with `Math.Clamp` so a thousand-day request can't freeze the server. You also met `Results.Ok(...)` and saw how the framework turns objects into JSON, sets status codes, and adds content-type headers without you writing a line for any of it.

**Key concepts you can now name:**

- **DTO at the API boundary** — small, clear record made for sending over the network
- **`Results.Ok` / `NotFound` / `BadRequest`** — control the status code from a handler
- **optional query parameter** — `(int? days)` becomes `?days=5` in the URL
- **checking input at the boundary** — clamp, check, reject before the engine runs

## On your own

Time to put the book away. Don't scroll back up to the steps — write a small response DTO from your own head. No one marks this one — it's just for you. It's the easiest way to spot what *hasn't* stuck yet, while it's still simple to fix. Getting stuck here is completely fine — that's exactly what it's for.

Open a new file. Imagine the API needs to return a building over the network. Without looking:

1. Write a `record` called `BuildingResponse` holding the building's `Name`, its `Kind` (a string like "Farm"), and its `Level` (a number) — only what a client needs.
2. Build the project to check it compiles.

<details><summary>Stuck? Open this to check yourself.</summary>

```csharp
namespace Kingdom.Api.Dtos;

public record BuildingResponse(string Name, string Kind, int Level);
```

One short line does it. A DTO is just a small, clear `record` made for sending over the network — no hidden engine state, no constructors that need an `IRandom`. It turns into JSON cleanly, and the client knows exactly what it will get.

</details>

## Wrap up

1. **Quiz** — open `quiz.md`, jot your answers in `journal/quiz-notes.md`.
2. **Progress** — one line in `journal/progress.md`: `Module 3.2 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit and push** — stage both files, commit message `Module 3.2 done`, Sync.
4. **Post in `#wins`** — one line about today, plus the URL of the commit.

Module 0.1 covers the why and the panel/CLI steps if you need a refresher. Bring quiz answers you're least sure about to the next weekly sync.

## Next

Module 3.3 introduces **several endpoints with the right status codes** — `POST /kingdoms` to create one, `DELETE /kingdoms/{id}` to remove one, plus 404s and 400s when something doesn't fit. This is CRUD over HTTP, the pattern behind every web API ever built.

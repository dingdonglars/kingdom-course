# Module 3.3 — Routing, Status Codes & Multi-Kingdom CRUD

> **Hook:** `POST /kingdoms` to create one. `GET /kingdoms` to list them. `GET /kingdoms/{id}` for a specific. `DELETE /kingdoms/{id}` to remove. **CRUD over HTTP** — every web API ever built. Plus the right status codes (201 on create, 404 on missing, 204 on delete) so clients know what happened.

> **Words to watch**
> - **route parameter** — `{id}` in the path; bound to a method argument
> - **`MapGroup`** — group routes that share a prefix, like a folder for endpoints
> - **REST conventions** — informal rules for verb + path + status combinations
> - **`Created` (201)** — the right status when a `POST` makes a new thing — includes a `Location` header pointing at the new resource

---

## REST conventions, in one table

| Action | Verb | Path | Success | Failure |
|---|---|---|---|---|
| List | `GET` | `/kingdoms` | 200 + array | (rare) |
| Read one | `GET` | `/kingdoms/{id}` | 200 + object | 404 |
| Create | `POST` | `/kingdoms` | 201 + Location header + new object | 400 (bad input) |
| Update | `PUT` | `/kingdoms/{id}` | 200 + updated, or 204 No Content | 404 / 400 |
| Delete | `DELETE` | `/kingdoms/{id}` | 204 No Content | 404 |

Following this lets any client developer guess the URL just from the verbs and entity names. **Conventions reduce the cognitive load** — the second client team writes itself.

## Delta starter

Today's API switches from "one in-memory kingdom" to "many kingdoms, persisted via the EF store from Block 4."

- **NEW:** `Kingdom.Api/Dtos/CreateKingdomRequest.cs`, `KingdomCreated.cs`
- **MODIFIED:** `Kingdom.Api/Program.cs` — uses `KingdomEfStore` + `MapGroup("/kingdoms")` + 5 endpoints
- **MODIFIED:** `Kingdom.Api/Kingdom.Api.csproj` — already references Persistence (M3.1)

## Step 1 — request/response DTOs

`Dtos/CreateKingdomRequest.cs`:

```csharp
namespace Kingdom.Api.Dtos;

public record CreateKingdomRequest(string Name);
```

`Dtos/KingdomCreated.cs`:

```csharp
namespace Kingdom.Api.Dtos;

public record KingdomCreated(int Id, string Name);
```

Both small and explicit. The request says exactly what the client sends; the response says exactly what comes back.

## Step 2 — wire `KingdomEfStore` into the API

```csharp
using Kingdom.Api.Dtos;
using Kingdom.Engine.Infrastructure;
using Kingdom.Persistence;
using Kingdom.Persistence.EfCore;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// One DB file per process. (Module 3.6 will switch to a configurable path.)
var dbPath = Path.Combine(AppContext.BaseDirectory, "kingdoms.db");
var store = new KingdomEfStore(dbPath);
store.EnsureCreated();
IRandom rng = new SystemRandom();
IClock clock = new SystemClock();

// All kingdom endpoints under /kingdoms
var group = app.MapGroup("/kingdoms");

// LIST — GET /kingdoms
group.MapGet("/", () => store.ListSlots());

// READ ONE — GET /kingdoms/{id}
group.MapGet("/{id:int}", (int id) =>
{
    try
    {
        var k = store.Load(id, rng, clock);
        return Results.Ok(KingdomJsonStore.ToSummary(k));
    }
    catch (InvalidOperationException)
    {
        return Results.NotFound(new { error = $"No kingdom with id {id}." });
    }
});

// CREATE — POST /kingdoms  body: { "name": "..." }
group.MapPost("/", (CreateKingdomRequest req) =>
{
    if (string.IsNullOrWhiteSpace(req.Name))
        return Results.BadRequest(new { error = "Name is required." });

    var k = new Kingdom.Engine.Kingdom(req.Name.Trim(), rng, clock);
    var id = store.Save(k);
    return Results.Created($"/kingdoms/{id}", new KingdomCreated(id, k.Name));
});

// TICK — POST /kingdoms/{id}/tick?days=N
group.MapPost("/{id:int}/tick", (int id, int? days) =>
{
    var n = Math.Clamp(days ?? 1, 1, 100);
    Kingdom.Engine.Kingdom k;
    try { k = store.Load(id, rng, clock); }
    catch (InvalidOperationException) { return Results.NotFound(); }

    for (int i = 0; i < n; i++) k.AdvanceDay();
    store.Update(id, k);
    return Results.Ok(KingdomJsonStore.ToSummary(k));
});

// DELETE — DELETE /kingdoms/{id}
group.MapDelete("/{id:int}", (int id) =>
{
    store.Delete(id);
    return Results.NoContent();    // 204 — even if nothing existed; idempotent
});

app.Run();

public partial class Program { }
```

Five things to read carefully:

1. **`MapGroup("/kingdoms")`** — every endpoint registered on `group` gets the `/kingdoms` prefix. Cleans up the path strings.
2. **`{id:int}`** — route parameter with a *type constraint*. `/kingdoms/abc` won't match (no `int` parsing); `/kingdoms/5` does, with `id = 5`.
3. **`Results.Created(uri, value)`** — the right answer for a successful POST. Sets status 201 *and* `Location: /kingdoms/5` header so the client knows the URL of the new thing.
4. **`Results.NoContent()`** — 204 — successful, nothing to return. Standard for DELETE.
5. **`try/catch (InvalidOperationException)`** — `store.Load` throws if missing; we translate to a 404. **Not great** — exception-as-control-flow. M3.4 will introduce `TryLoad` to handle this without exceptions.

## Step 3 — try it all

```powershell
dotnet run --project Kingdom.Api
# in another terminal
curl http://localhost:5xxx/kingdoms                                                     # []
curl -X POST http://localhost:5xxx/kingdoms -H "Content-Type: application/json" -d '{"name":"Eldoria"}'
# Response: 201 Created, Location: /kingdoms/1, body { "id":1, "name":"Eldoria" }
curl http://localhost:5xxx/kingdoms                                                     # [ {id:1, ...} ]
curl http://localhost:5xxx/kingdoms/1                                                   # full summary
curl -X POST "http://localhost:5xxx/kingdoms/1/tick?days=5"
curl -X DELETE http://localhost:5xxx/kingdoms/1                                         # 204 No Content
curl -i http://localhost:5xxx/kingdoms/1                                                # 404 now
```

Every status code from the table makes an appearance.

## Tinker

- Try `POST /kingdoms` with `{"name": ""}`. **400 Bad Request.** With a missing body — also 400. The framework + your validation cover the obvious cases.
- Add `app.MapGet("/kingdoms/{id:int}/buildings", ...)` returning the buildings of one kingdom.
- Use `MapDelete` on `/kingdoms` (no id) to delete *all* kingdoms — risky! Most APIs require an explicit `?confirm=yes` flag for destructive batch ops.
- Run two `POST /kingdoms` calls — observe the auto-incrementing ids.

## Name it

- **Route parameter** (`{id}`). Path placeholder bound to a method argument. Use `:int` for type constraints.
- **`MapGroup`** — share a path prefix among related endpoints.
- **REST conventions** — informal rules everyone agrees on: verbs to mean what they mean, status codes to mean what they mean.
- **201 Created** — the right success status for a successful POST that made a new thing.
- **204 No Content** — success, nothing to send. Standard for DELETE.

## The rule of the through-line

> **Status codes are part of your API.** Returning the wrong one isn't a "minor detail"; it's a wire-shape bug. Clients branch on status. A `200 OK` with `{ "error": "not found" }` confuses every consumer.

## Quiz / challenge

Open `quiz.md`.

## Connect

Module 3.4 introduces **OpenAPI/Swagger** — auto-generated documentation for the API you just built — and **logging**, the structured kind. Both turn a working API into one you can hand to another developer.
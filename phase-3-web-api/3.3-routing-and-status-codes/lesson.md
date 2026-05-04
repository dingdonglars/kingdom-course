# Module 3.3 — Routing, Status Codes, and Multi-Kingdom CRUD

So far the API has been one kingdom in memory. Today the API supports many kingdoms, persisted via the EF store from Phase 2. `POST /kingdoms` to create one. `GET /kingdoms` to list them. `GET /kingdoms/{id}` for a specific one. `DELETE /kingdoms/{id}` to remove. This is **CRUD over HTTP** — *create, read, update, delete* — the pattern under every web API ever built.

The other half of today is using the *right* status codes for each operation. 201 when a `POST` creates something; 404 when the resource doesn't exist; 204 when a delete succeeds. Clients branch on status codes — returning the wrong one isn't a small detail, it's a wire-level bug.

> **Words to watch**
>
> - **route parameter** — `{id}` in the path, bound to a method argument
> - **`MapGroup`** — group routes that share a path prefix, like a folder for endpoints
> - **REST conventions** — informal rules everyone agrees on for verb plus path plus status combinations
> - **`Created` (201)** — the right status for a successful `POST` that made a new thing; includes a `Location` header pointing at the new resource

---

## REST conventions, in one table

| Action | Verb | Path | Success | Failure |
|---|---|---|---|---|
| List | `GET` | `/kingdoms` | 200 + array | (rare) |
| Read one | `GET` | `/kingdoms/{id}` | 200 + object | 404 |
| Create | `POST` | `/kingdoms` | 201 + Location header + new object | 400 (bad input) |
| Update | `PUT` | `/kingdoms/{id}` | 200 + updated, or 204 No Content | 404 / 400 |
| Delete | `DELETE` | `/kingdoms/{id}` | 204 No Content | 404 |

Following these conventions lets any client developer guess the URL just from the verb and the entity name. Conventions reduce the work of reading an API — the second client team writes itself, because they already know what to expect.

## What ships in the starter

Today the API switches from *one in-memory kingdom* to *many kingdoms, persisted via the EF store from Phase 2*.

- **NEW:** `Kingdom.Api/Dtos/CreateKingdomRequest.cs` and `KingdomCreated.cs`
- **MODIFIED:** `Kingdom.Api/Program.cs` — uses `KingdomEfStore` plus `MapGroup("/kingdoms")` and five endpoints
- **MODIFIED:** `Kingdom.Api/Kingdom.Api.csproj` — already references Persistence (from M3.1)

## Step 1 — request and response DTOs

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

// One DB file per process. (Module 3.6 switches this to a configurable path.)
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
2. **`{id:int}`** — a route parameter with a *type constraint*. `/kingdoms/abc` won't match (no int parsing); `/kingdoms/5` does, with `id = 5`.
3. **`Results.Created(uri, value)`** — the right answer for a successful `POST`. Sets status 201 *and* the `Location: /kingdoms/5` header so the client knows the URL of the new thing.
4. **`Results.NoContent()`** — 204 — the operation succeeded, there's nothing to return. Standard for `DELETE`.
5. **`try/catch (InvalidOperationException)`** — `store.Load` throws if the record is missing; we translate that to a 404. This is *not great* — it's exception-as-control-flow. M3.4 will introduce a `TryLoad` to handle the missing-record case without exceptions.

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

Try `POST /kingdoms` with `{"name": ""}`. You get a 400 Bad Request from your own validation. With a missing body, you also get a 400 — that one comes from the framework. Between you and the framework, the obvious cases are covered.

Add `app.MapGet("/kingdoms/{id:int}/buildings", ...)` returning the buildings of one kingdom.

Use `MapDelete` on `/kingdoms` (no id) to delete *all* kingdoms — risky! Most APIs require an explicit `?confirm=yes` flag for destructive batch operations.

Run two `POST /kingdoms` calls in a row. Watch the auto-incrementing ids in the responses.

## The through-line

Status codes are part of your API. Returning the wrong one isn't a minor detail; it's a wire-level bug. Clients branch on status. A `200 OK` with `{ "error": "not found" }` confuses every consumer. Use 201 for creates, 204 for deletes, 404 for missing resources, 400 for bad input. The conventions exist so any client team can guess the right behaviour without reading your code.

## What you just did

You moved the API from one-kingdom-in-memory to many-kingdoms-in-the-database, with a full set of CRUD endpoints. You used `MapGroup("/kingdoms")` to share a path prefix across five handlers, route constraints (`{id:int}`) to reject malformed URLs at the framework layer, and `Results.Created`, `Results.NoContent`, `Results.NotFound` and `Results.BadRequest` to return the right status codes for the right reasons. You also met your first code smell — exception-as-control-flow in the missing-record case — and named it as something M3.4 will clean up.

**Key concepts you can now name:**

- **route parameter** — path placeholder bound to a handler argument, with optional `:int` constraint
- **`MapGroup`** — share a path prefix among related endpoints
- **REST conventions** — verbs and status codes everyone agrees on
- **201 Created** — successful POST that made something new, with a `Location` header
- **204 No Content** — successful operation with nothing to return; standard for `DELETE`

## Git move of the week — `git reflog`

The thing nobody tells you about git: it almost never actually loses your work. Even after `git reset --hard`, even after a botched rebase, the commits are still in git's storage — git just stopped pointing at them.

`git reflog` is the safety net. It shows every recent position of HEAD, in reverse order. As long as the SHA you want is still in the reflog (default ~30 days), you can recover it.

> **This one's CLI-only — the panel doesn't have a button for it.**
>
> ```powershell
> git reflog                     # see where HEAD has been
> git reset --hard HEAD@{1}      # go back one HEAD position
> ```

Specifically: if you panic-`reset --hard` past commits you wanted to keep, run `git reflog`, find the SHA from before the reset, run `git reset --hard <that-sha>` — and they're back. We go properly into the safety net in B3.3 if you take that bonus.

## Quiz

Open `quiz.md`. When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.

## Next

Module 3.4 introduces **OpenAPI (Swagger)** — auto-generated documentation for the API you just built — and **structured logging**. Both turn a working API into one you can hand to another developer without a tour.

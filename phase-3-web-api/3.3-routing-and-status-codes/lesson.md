# Module 3.3 — Routing, Status Codes, and Multi-Kingdom CRUD

> Travou no inglês? Abra o `lesson.pt.md` — é esta mesma lição em português. Tente em inglês primeiro.

So far the API has held one kingdom in memory. Today the API handles many kingdoms, saved through the EF store from Phase 2. `POST /kingdoms` creates one. `GET /kingdoms` lists them. `GET /kingdoms/{id}` reads a specific one. `DELETE /kingdoms/{id}` removes one. This is **CRUD over HTTP** — *create, read, update, delete* — the pattern behind every web API ever built.

The other half of today is using the *right* status code for each action. 201 when a `POST` creates something. 404 when the thing isn't there. 204 when a delete works. Clients make decisions based on the status code, so returning the wrong one isn't a small detail. It's a real bug that the client will trip over.

> **Words to watch**
>
> - **route parameter** — `{id}` in the path, matched to a method argument
> - **`MapGroup`** — group routes that share the start of their path, like a folder for endpoints
> - **REST conventions** — agreed-upon rules everyone follows for which verb, path, and status code go together
> - **`Created` (201)** — the right status for a successful `POST` that made something new; it includes a `Location` header with the URL of the new thing

---

## REST conventions, in one table

| Action | Verb | Path | Success | Failure |
|---|---|---|---|---|
| List | `GET` | `/kingdoms` | 200 + array | (rare) |
| Read one | `GET` | `/kingdoms/{id}` | 200 + object | 404 |
| Create | `POST` | `/kingdoms` | 201 + Location header + new object | 400 (bad input) |
| Update | `PUT` | `/kingdoms/{id}` | 200 + updated, or 204 No Content | 404 / 400 |
| Delete | `DELETE` | `/kingdoms/{id}` | 204 No Content | 404 |

When you follow these conventions, any client developer can guess the URL just from the verb and the name of the thing. Conventions make an API easier to read. The next team that uses your API has less to learn, because they already know what to expect.

## What ships in the starter

Today the API changes from *one kingdom held in memory* to *many kingdoms, saved through the EF store from Phase 2*.

- **NEW:** `Kingdom.Api/Dtos/CreateKingdomRequest.cs` and `KingdomCreated.cs`
- **MODIFIED:** `Kingdom.Api/Program.cs` — uses `KingdomEfStore` plus `MapGroup("/kingdoms")` and five endpoints
- **MODIFIED:** `Kingdom.Api/Kingdom.Api.csproj` — already references Persistence (from Module 3.1)

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

Both are small and clear. The request says exactly what the client sends. The response says exactly what comes back.

## Step 2 — connect `KingdomEfStore` to the API

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

1. **`MapGroup("/kingdoms")`** — every endpoint added to `group` starts with `/kingdoms`. This keeps the path strings short.
2. **`{id:int}`** — a route parameter that only accepts a whole number. `/kingdoms/abc` won't match (it's not an int); `/kingdoms/5` does match, with `id = 5`.
3. **`Results.Created(uri, value)`** — the right answer for a successful `POST`. It sets status 201 *and* the `Location: /kingdoms/5` header, so the client knows the URL of the new thing.
4. **`Results.NoContent()`** — 204 — the action worked, and there's nothing to send back. This is the usual answer for `DELETE`.
5. **`try/catch (InvalidOperationException)`** — `store.Load` throws if the record isn't there, and we turn that into a 404. This is *not great* — we're using an exception to steer normal program flow. Module 3.4 will add a `TryLoad` so we can handle a missing record without an exception.

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

Every status code from the table shows up.

## Tinker

Try `POST /kingdoms` with `{"name": ""}`. You get a 400 Bad Request from your own check. With no body at all, you also get a 400 — that one comes from the framework. Between your check and the framework's, the obvious bad cases are covered.

Add `app.MapGet("/kingdoms/{id:int}/buildings", ...)` returning the buildings of one kingdom.

Use `MapDelete` on `/kingdoms` (no id) to delete *all* kingdoms — careful! Most APIs make you add an explicit `?confirm=yes` flag before they delete a whole group of things.

Run two `POST /kingdoms` calls one after the other. Watch the ids count up on their own in the responses.

## The main point

Status codes are part of your API. Returning the wrong one isn't a small detail — it's a real bug the client will trip over. Clients make decisions based on the status code. A `200 OK` with `{ "error": "not found" }` confuses everyone who uses your API. Use 201 for creates, 204 for deletes, 404 for things that aren't there, 400 for bad input. The conventions exist so any client team can guess the right behaviour without reading your code.

## What you just did

You moved the API from one kingdom in memory to many kingdoms in the database, with a full set of CRUD endpoints. You used `MapGroup("/kingdoms")` so five handlers could share the start of their path, route constraints (`{id:int}`) so the framework rejects broken URLs for you, and `Results.Created`, `Results.NoContent`, `Results.NotFound`, and `Results.BadRequest` to return the right status code for the right reason. You also met your first code smell — using an exception to steer normal program flow when a record is missing — and named it as something Module 3.4 will clean up.

**Key concepts you can now name:**

- **route parameter** — a placeholder in the path matched to a handler argument, with an optional `:int` constraint
- **`MapGroup`** — share the start of a path among related endpoints
- **REST conventions** — verbs and status codes everyone agrees on
- **201 Created** — successful POST that made something new, with a `Location` header
- **204 No Content** — successful operation with nothing to return; standard for `DELETE`

## On your own

Time to put the book away. Don't scroll back up to the steps — pick the right status code for each action from your own head. No one marks this one — it's just for you. It's the easiest way to spot what *hasn't* stuck yet, while it's still simple to fix. Getting stuck here is completely fine — that's exactly what it's for.

On paper, write the matching status code for each of these. For each one, say in a few words why.

1. A `POST /kingdoms` that creates a new kingdom — success.
2. A `GET /kingdoms/{id}` for an id that doesn't exist.
3. A `DELETE /kingdoms/{id}` that works.
4. A `POST /kingdoms` with an empty name.

<details><summary>Stuck? Open this to check yourself.</summary>

1. **201 Created** — a new thing was made; it also sends a `Location` header with the URL of the new kingdom.
2. **404 Not Found** — the thing the client asked for isn't there.
3. **204 No Content** — it worked, and there's nothing to send back.
4. **400 Bad Request** — the client sent bad data.

The big split to remember: 2xx means success, 4xx is the client's fault, 5xx is the server's fault. Clients make decisions based on the status code, so the wrong one is a real bug, not a small detail.

</details>

## Git move of the week — `git reflog`

Here's something nobody tells you about git: it almost never really loses your work. Even after `git reset --hard`, even after a rebase that went wrong, the commits are still saved by git — git just stopped pointing at them.

`git reflog` (short for *reference log*) is the safety net. It shows every recent position of HEAD (git's "you are here" pointer), newest first. As long as the SHA (a commit's unique fingerprint) you want is still in the reflog — about 30 days by default — you can get it back.

> **This one's CLI-only — the panel doesn't have a button for it.**
>
> ```powershell
> git reflog                     # see where HEAD has been
> git reset --hard HEAD@{1}      # go back one HEAD position
> ```

Here's the move: if you `reset --hard` in a panic and lose commits you wanted to keep, run `git reflog`, find the SHA from before the reset, then run `git reset --hard <that-sha>` — and they're back. We go through this safety net properly in B3.3 if you take that bonus.

## Wrap up

1. **Quiz** — open `quiz.md`, jot your answers in `journal/quiz-notes.md`.
2. **Progress** — one line in `journal/progress.md`: `Module 3.3 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit and push** — stage both files, commit message `Module 3.3 done`, Sync.
4. **Post in `#wins`** — one line about today, plus the URL of the commit.

Module 0.1 covers the why and the panel/CLI steps if you need a refresher. Bring quiz answers you're least sure about to the next weekly sync.

## Next

Module 3.4 introduces **OpenAPI (Swagger)** — documentation for your API that's generated for you — and **structured logging**. Both turn a working API into one you can hand to another developer without having to walk them through it.

# Module 3.6 — Multi-User Persistence

Today every kingdom belongs to a *user*. The signed-in user's `sub` is stored alongside every save. `GET /kingdoms` returns *only your* kingdoms — never anyone else's. This is what turns the API from a global free-for-all into a real multi-user product.

The change is small in lines of code. It's enormous in importance. Multi-user data is *the* most-bug-prone code in any web app, and the bugs are quiet: one user sees another user's data, or modifies it, and nobody notices for weeks. The fix is a single `WHERE` clause; the cost of forgetting is enormous. We're going to bake the discipline in now, while the codebase is small enough to grasp end-to-end.

> **Words to watch**
>
> - **owner** — the user who created the resource (we use Google's `sub` claim)
> - **authorisation** vs. **authentication** — auth*entication* is *who are you*; auth*orisation* is *what are you allowed to do*
> - **scoped query** — every `WHERE` clause includes `OwnerSub = currentUser` to prevent cross-user reads or writes
> - **migration with data preservation** — adding a non-null column to a table with existing rows requires a default value

---

## Why this matters more than it looks

This is the bug class that lands on the front page when it goes wrong. The classic version: a user types `/kingdoms/1234` into the URL bar, and gets back somebody else's kingdom. The fix is one `WHERE OwnerSub = ?` clause; the absence of that clause is what lets the bug exist. We codify the discipline now so it's automatic forever.

## What ships in the starter

- **MODIFIED:** `Kingdom.Persistence/EfCore/KingdomEntity.cs` — adds `string OwnerSub` (foreign-key-like, indexed)
- **NEW migration:** `dotnet ef migrations add AddOwnerSub` (you run this)
- **MODIFIED:** `Kingdom.Persistence/EfCore/KingdomEfStore.cs` — every method takes `ownerSub` and scopes its query
- **MODIFIED:** `Kingdom.Api/Program.cs` — reads `sub` from the auth cookie and passes it to the store
- **NEW:** `tests/Kingdom.Persistence.Tests/MultiUserTests.cs`

## Step 1 — entity gets `OwnerSub`

```csharp
public class KingdomEntity
{
    public int Id { get; set; }
    public string OwnerSub { get; set; } = "";    // NEW
    public string Name { get; set; } = "";
    // ... rest unchanged
}
```

In `KingdomDbContext.OnModelCreating(...)`, add an index for the lookup:

```csharp
protected override void OnModelCreating(ModelBuilder model)
{
    model.Entity<KingdomEntity>().HasIndex(k => k.OwnerSub);
}
```

Then generate the migration:

```powershell
dotnet ef migrations add AddOwnerSub --project Kingdom.Persistence --startup-project Kingdom.Console
```

The generated migration adds the column. For existing rows, EF will use the default `""` — meaning they'll be unowned. Real production migrations would either backfill the column or refuse to add it without a strategy. For our learning DB, the default is fine.

## Step 2 — store methods take `ownerSub`

Every public method gains a `string ownerSub` parameter and scopes the query. Notice the LINQ break-before-the-dot pattern when the chain has three or more methods:

```csharp
public int Save(string ownerSub, Kingdom.Engine.Kingdom kingdom)
{
    EnsureCreated();
    using var ctx = new KingdomDbContext(_dbPath);
    var entity = new KingdomEntity
    {
        OwnerSub = ownerSub,            // <-- the only new line in Save
        Name = kingdom.Name,
        // ... rest unchanged
    };
    ctx.Kingdoms.Add(entity);
    ctx.SaveChanges();
    return entity.Id;
}

public Kingdom.Engine.Kingdom Load(string ownerSub, int id, IRandom rng, IClock clock)
{
    using var ctx = new KingdomDbContext(_dbPath);
    var entity = ctx.Kingdoms
        .Include(k => k.Buildings)
        .Single(k => k.Id == id && k.OwnerSub == ownerSub);   // <-- scoped lookup
    // ... rest unchanged
}

public IReadOnlyList<KingdomSlotInfo> ListSlots(string ownerSub)
{
    using var ctx = new KingdomDbContext(_dbPath);
    return ctx.Kingdoms
        .AsNoTracking()
        .Where(k => k.OwnerSub == ownerSub)
        .OrderBy(k => k.Id)
        .Select(k => new KingdomSlotInfo(k.Id, k.Name, k.Day))
        .ToList();
}

// Same pattern for Update and Delete: WHERE Id = id AND OwnerSub = ownerSub
```

The bug-resistant pattern: **`ownerSub` is a required parameter on every method.** A caller who forgets it gets a compile error, not a security bug. Don't make it optional.

## Step 3 — extract `ownerSub` in `Program.cs`

```csharp
static string GetOwnerSub(HttpContext ctx)
{
    var sub = ctx.User.FindFirst("sub")?.Value
           ?? ctx.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
    return sub ?? throw new InvalidOperationException(
        "No sub claim — request is unauthenticated.");
}

group.MapGet("/", (HttpContext ctx) => store.ListSlots(GetOwnerSub(ctx)));

group.MapPost("/", (CreateKingdomRequest req, HttpContext ctx, ILogger<Program> log) =>
{
    var sub = GetOwnerSub(ctx);
    if (string.IsNullOrWhiteSpace(req.Name))
        return Results.BadRequest(new { error = "Name is required." });
    var k = new Kingdom.Engine.Kingdom(req.Name.Trim(), rng, clock);
    var id = store.Save(sub, k);
    log.LogInformation("Created kingdom {KingdomId} for {OwnerSub}", id, sub);
    return Results.Created($"/kingdoms/{id}", new KingdomCreated(id, k.Name));
});

// Same pattern for Load/Tick/Delete: read sub, pass to store. Always scoped.
```

## Step 4 — multi-user tests

`tests/Kingdom.Persistence.Tests/MultiUserTests.cs`:

```csharp
[Fact]
public void Save_ScopedToOwner_OtherUserCannotSee()
{
    var path = Path.Combine(Path.GetTempPath(), $"mu-{Guid.NewGuid():N}.db");
    try
    {
        var store = new KingdomEfStore(path);
        store.Save("alice", new global::Kingdom.Engine.Kingdom("AliceVille"));
        store.Save("bob",   new global::Kingdom.Engine.Kingdom("BobsTown"));

        store.ListSlots("alice").Single().Name.ShouldBe("AliceVille");
        store.ListSlots("bob").Single().Name.ShouldBe("BobsTown");
        store.ListSlots("eve").ShouldBeEmpty();   // unknown user sees nothing
    }
    finally { if (File.Exists(path)) File.Delete(path); }
}

[Fact]
public void Load_OfOtherUsersKingdom_Throws()
{
    var path = Path.Combine(Path.GetTempPath(), $"mu-{Guid.NewGuid():N}.db");
    try
    {
        var store = new KingdomEfStore(path);
        var aliceId = store.Save("alice", new global::Kingdom.Engine.Kingdom("AliceVille"));

        Should.Throw<InvalidOperationException>(() =>
            store.Load("bob", aliceId, new SystemRandom(0), new SystemClock()));
    }
    finally { if (File.Exists(path)) File.Delete(path); }
}
```

The second test is the one that would catch the bug. Always test the cross-user case.

## Tinker

Sign in as User A, create a kingdom. Sign out. Sign in as User B (a different Google account). `GET /kingdoms` is empty. Try `GET /kingdoms/<UserA_kingdom_id>` — 404. The scoped lookup returns nothing, treated as not-found.

Drop the `&& k.OwnerSub == ownerSub` clause from `Load`. Run the tests — the cross-user test fails. Restore the clause. That's the test that earns its keep.

Add an `IsPublic` boolean field to allow players to share kingdoms read-only. Now scoped queries become `OwnerSub == ownerSub OR IsPublic == true`. Same discipline, slightly relaxed scope.

## The through-line

Multi-user safety lives in the data layer, not the UI. The UI can hide buttons, but if the API doesn't scope its queries, anyone with `curl` can read anyone's data. The `WHERE OwnerSub = ?` clause is the contract.

## What you just did

You turned the API from *one big shared kingdom database* into *each user sees only their own kingdoms*. The change was one new column (`OwnerSub`), one new index, and a `WHERE OwnerSub = ?` clause on every read and write. The bug-resistant pattern: make `ownerSub` a required parameter on every store method, so a caller who forgets it gets a compile error, not a 4 a.m. security incident. The cross-user test you wrote — *Load of another user's kingdom throws* — is the test that matters most; it's the test that would catch a missing `WHERE` clause before it ships. Two new tests, eighty-plus passing total.

**Key concepts you can now name:**

- **owner** — the user who owns a resource; we use Google's `sub` claim
- **scoped query** — every read/write filters by owner
- **authorisation** — what you're allowed to do (vs. authentication = who you are)
- **`HasIndex(k => k.OwnerSub)`** — keeps lookup-by-owner fast as data grows
- **the cross-user test** — the test that earns its keep when somebody refactors

## Git move of the week — resolving a merge conflict

Sooner or later, two branches change the same lines and git can't auto-merge. You'll see a message: *"Conflicts must be resolved."*

In VS Code: open the conflicted file. Each conflicted *hunk* (a contiguous block of changed lines) shows inline buttons — *Accept Current Change*, *Accept Incoming Change*, *Accept Both Changes*, *Compare Changes*. Click whichever applies for each hunk. Once every conflict is resolved, the file is no longer marked conflicted. Stage it, commit (the commit message is pre-filled with *"Merge branch ..."*), push.

> **Same move, in the terminal:** conflicts appear as `<<<<<<<` / `=======` / `>>>>>>>` markers in the file. Edit the file by hand to keep the right version, remove the markers, run `git add <file>`, then `git commit` (or `git rebase --continue`).

The discipline: **read both versions before picking.** Auto-accepting without reading is how silent bugs get merged.

## Quiz

Open `quiz.md`. When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.

## Next

Module 3.7 introduces **integration tests** with `WebApplicationFactory<Program>`. Real HTTP, real auth, real DB — fully scripted, fully verified. The thing that gives you confidence to refactor without manually clicking through every endpoint.

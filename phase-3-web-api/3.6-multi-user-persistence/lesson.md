# Module 3.6 — Multi-User Persistence

> **Hook:** today every kingdom belongs to a *user*. The signed-in user's `sub` is stored alongside every save. `GET /kingdoms` returns *only your* kingdoms — never anyone else's. This is what makes the API a real multi-user product instead of a global free-for-all.

> **Words to watch**
> - **owner** — the user who created the resource (we use Google's `sub` claim)
> - **authorisation** (vs. authentication) — auth*entication* is "who are you"; auth*orisation* is "what are you allowed to do"
> - **scoped query** — every `WHERE` clause includes `OwnerSub = currentUser` to prevent cross-user reads/writes
> - **migration with data preservation** — adding a non-null column to a table with existing rows requires a default

---

## Why this matters more than it looks

Multi-user data is *the* most-bug-prone code in any web app. The bugs are quiet: one user sees another user's data, or modifies it. **The fix is small (one `WHERE` clause), the cost of forgetting is enormous.** We codify the discipline now.

## Delta starter

- **MODIFIED:** `Kingdom.Persistence/EfCore/KingdomEntity.cs` — adds `string OwnerSub` (foreign-key-like, indexed)
- **NEW migration:** `dotnet ef migrations add AddOwnerSub` (you run this)
- **MODIFIED:** `Kingdom.Persistence/EfCore/KingdomEfStore.cs` — every method takes `ownerSub` and scopes its query
- **MODIFIED:** `Kingdom.Api/Program.cs` — reads `sub` from the auth cookie and passes to the store
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

The generated migration will add the column. **For existing rows, EF will use the default `""`** — meaning they'll be unowned. Real production migrations would either backfill or refuse to add the column without a strategy. For our learning DB, the default is fine.

## Step 2 — store methods take `ownerSub`

Every public method gains a `string ownerSub` parameter and scopes the query:

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
    return ctx.Kingdoms.AsNoTracking()
        .Where(k => k.OwnerSub == ownerSub)                   // <-- scoped list
        .OrderBy(k => k.Id)
        .Select(k => new KingdomSlotInfo(k.Id, k.Name, k.Day))
        .ToList();
}

// Same pattern for Update + Delete: WHERE Id = id AND OwnerSub = ownerSub
```

The bug-resistant pattern: **`ownerSub` is a required parameter on every method.** A caller who forgets it gets a compile error, not a security bug. Don't make it optional.

## Step 3 — extract `ownerSub` in `Program.cs`

```csharp
static string GetOwnerSub(HttpContext ctx)
{
    var sub = ctx.User.FindFirst("sub")?.Value
           ?? ctx.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
    return sub ?? throw new InvalidOperationException("No sub claim — request is unauthenticated.");
}

group.MapGet("/", (HttpContext ctx) => store.ListSlots(GetOwnerSub(ctx)));

group.MapPost("/", (CreateKingdomRequest req, HttpContext ctx, ILogger<Program> log) =>
{
    var sub = GetOwnerSub(ctx);
    if (string.IsNullOrWhiteSpace(req.Name)) return Results.BadRequest(new { error = "Name is required." });
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

The second test is the one that would have caught the bug. **Always test the cross-user case.**

## Tinker

- Sign in as User A, create a kingdom. Sign out. Sign in as User B (different Google account). `GET /kingdoms` is empty. Try `GET /kingdoms/<UserA_kingdom_id>` — `404` (the scoped lookup returns nothing, treated as not-found).
- Drop the `&& k.OwnerSub == ownerSub` clause from `Load`. Run the tests — the cross-user test fails. **Restore the clause.** That's the test that earns its keep.
- Add a `IsPublic boolean` field to allow players to share kingdoms read-only. Now scoped queries become `OwnerSub == ownerSub OR IsPublic == true`. Same discipline, slightly relaxed scope.

## Name it

- **Owner** — the user who created/owns a resource. We use Google `sub` (stable id).
- **Scoped query** — every read/write filters by owner. Forgetting one is a real-world security bug.
- **Authorisation** — what you're allowed to do with the resource (vs. authentication = who you are).
- **`HasIndex(k => k.OwnerSub)`** — tells the DB to maintain an index on the column; lookup-by-owner stays fast as data grows.

## The rule of the through-line

> **Multi-user safety lives in the data layer, not the UI.** The UI can hide buttons, but if the API doesn't scope its queries, anyone with curl can read anyone's data. The `WHERE OwnerSub = ?` clause is the contract.

## Quiz / challenge

Open `quiz.md`.

## Connect

Module 3.7 introduces **integration tests** with `WebApplicationFactory<Program>`. Real HTTP, real auth, real DB — fully scripted, fully verified. The thing that gives you confidence to refactor without manual smoke testing every time.
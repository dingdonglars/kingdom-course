# Module 3.6 — Multi-User Persistence

Today every kingdom belongs to a *user*. The signed-in user's `sub` is saved along with every kingdom. `GET /kingdoms` returns *only your* kingdoms — never anyone else's. This is what turns the API from a single shared pile of data into a real product with separate users.

The change is small in lines of code. It is very important all the same. Multi-user data is the part of any web app where bugs hide most easily, and these bugs are quiet: one user sees another user's data, or changes it, and nobody notices for weeks. The fix is a single `WHERE` clause, and the cost of forgetting it is huge. We're going to build this habit in now, while the codebase is still small enough to understand from end to end.

> **Words to watch**
>
> - **owner** — the user who created the resource (we use Google's `sub` claim)
> - **authorisation** vs. **authentication** — auth*entication* is *who are you*; auth*orisation* is *what are you allowed to do*
> - **scoped query** — every `WHERE` clause includes `OwnerSub = currentUser`, so one user can't read or change another user's data
> - **migration with data preservation** — when you add a non-null column to a table that already has rows, you must give it a default value

---

## Why this matters more than it looks

This is the kind of bug that makes the news when it goes wrong. The classic version: a user types `/kingdoms/1234` into the address bar and gets back somebody else's kingdom. The fix is one `WHERE OwnerSub = ?` clause. Leaving that clause out is exactly what lets the bug happen. We build the habit in now so it becomes automatic.

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

The generated migration adds the column. For rows that already exist, EF will use the default `""`, which means they'll have no owner. A real production migration would either fill in the column for the old rows or refuse to run until you have a plan for them. For our learning DB, the default is fine.

## Step 2 — store methods take `ownerSub`

Every public method gets a `string ownerSub` parameter and adds it to the query. Notice the LINQ pattern of breaking the line before the dot when the chain has three or more methods:

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

The pattern that keeps bugs out: **`ownerSub` is a required parameter on every method.** A caller who forgets it gets a compile error, not a security bug. Don't make it optional.

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
    if (string.IsNullOrWhiteSpace(req.Name))
        return Results.BadRequest(new { error = "Name is required." });
    var ownerSub = GetOwnerSub(ctx);
    var k = new Kingdom.Engine.Kingdom(req.Name.Trim(), rng, clock);
    var id = store.Save(ownerSub, k);
    log.LogInformation("Created kingdom {KingdomId} '{KingdomName}' for {OwnerSub}", id, k.Name, ownerSub);
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

The second test is the one that would catch the bug. Always write a test for the cross-user case.

## Tinker

Sign in as User A and create a kingdom. Sign out. Sign in as User B (a different Google account). `GET /kingdoms` is empty. Try `GET /kingdoms/<UserA_kingdom_id>` — you get a 404. The scoped lookup finds nothing, which is treated as not-found.

Remove the `&& k.OwnerSub == ownerSub` clause from `Load`. Run the tests — the cross-user test fails. Put the clause back. That's the test doing its job.

Add an `IsPublic` boolean field so players can share kingdoms read-only. Now the scoped query becomes `OwnerSub == ownerSub OR IsPublic == true`. Same habit, with one careful exception.

## The main point

Multi-user safety belongs in the data layer, not the UI. The UI can hide buttons, but if the API doesn't scope its queries, anyone with `curl` can read anyone's data. The `WHERE OwnerSub = ?` clause is the real protection.

## What you just did

You turned the API from *one big shared kingdom database* into *each user sees only their own kingdoms*. The change was one new column (`OwnerSub`), one new index, and a `WHERE OwnerSub = ?` clause on every read and write. The pattern that keeps bugs out: make `ownerSub` a required parameter on every store method, so a caller who forgets it gets a compile error instead of a serious security bug. The cross-user test you wrote — *Load of another user's kingdom throws* — is the one that matters most. It's the test that would catch a missing `WHERE` clause before it goes live. Two new tests, eighty-plus passing in total.

**Key concepts you can now name:**

- **owner** — the user who owns a resource; we use Google's `sub` claim
- **scoped query** — every read/write filters by owner
- **authorisation** — what you're allowed to do (vs. authentication = who you are)
- **`HasIndex(k => k.OwnerSub)`** — keeps lookup-by-owner fast as the data grows
- **the cross-user test** — the test that proves its worth when somebody refactors

## Git move of the week — resolving a merge conflict

Sooner or later, two branches change the same lines and git can't merge them on its own. You'll see a message: *"Conflicts must be resolved."*

In VS Code: open the file with the conflict. Each conflicting *hunk* (a block of changed lines next to each other) shows buttons in the editor — *Accept Current Change*, *Accept Incoming Change*, *Accept Both Changes*, *Compare Changes*. Click the one that's right for each hunk. Once you've handled every conflict, the file is no longer marked as conflicting. Stage it, commit (the commit message is already filled in with *"Merge branch ..."*), and push.

> **Or in the terminal:** conflicts show up as `<<<<<<<` / `=======` / `>>>>>>>` markers in the file. Edit the file by hand to keep the version you want, delete the markers, run `git add <file>`, then `git commit` (or `git rebase --continue`).

The habit to build: **read both versions before you pick one.** Clicking accept without reading is how silent bugs get merged in.

## Wrap up

1. **Quiz** — open `quiz.md`, jot your answers in `journal/quiz-notes.md`.
2. **Progress** — one line in `journal/progress.md`: `Module 3.6 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit and push** — stage both files, commit message `Module 3.6 done`, Sync.
4. **Post in `#wins`** — one line about today, plus the URL of the commit.

Module 0.1 covers the why and the panel/CLI steps if you need a refresher. Bring quiz answers you're least sure about to the next weekly sync.

## Next

Module 3.7 introduces **integration tests** with `WebApplicationFactory<Program>`. Real HTTP, real auth, real DB — all written down as tests and checked automatically. This is what lets you refactor with confidence, instead of clicking through every endpoint by hand.

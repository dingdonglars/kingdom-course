# Module 2.6 — EF Core (Code-First)

Raw SQL is fine for one query. But twenty queries — each with its own `using` block for the connection, the command, and the reader — gets tiring fast. Today we meet **Entity Framework Core**, the .NET *ORM*. An ORM is a library that matches classes to database rows: you write C# classes, and EF turns them into tables. You write `db.Kingdoms.Add(k); db.SaveChanges();`, and EF writes the SQL for you.

> **Words to watch**
>
> - **ORM** *(oh-arr-em)* — *Object-Relational Mapper*. A library that maps classes to database rows.
> - **Entity** — a class EF maps to a table.
> - **DbContext** — your gateway to the database; a class with `DbSet<T>` properties (one per table).
> - **`DbSet<T>`** — represents a table; supports LINQ queries (`db.Kingdoms.Where(...)`).
> - **Code-first** — define the schema in C# code; EF generates the SQL.
> - **Database-first** — the opposite — generate C# from an existing schema. We're not doing that.

---

## Why an ORM

Once you've written `using var conn / cmd / reader` thirty times, you start to see the same patterns over and over. An ORM turns those patterns into ready-made code:

| Raw SQL | EF Core |
|---|---|
| `INSERT INTO kingdoms (name) VALUES ($name)` | `db.Kingdoms.Add(k); db.SaveChanges();` |
| `SELECT * FROM kingdoms WHERE name = $n` | `db.Kingdoms.Single(k => k.Name == n)` |
| Manual mapping rows → objects | `Kingdom` is the type |

EF is LINQ over a database. It's the same `Where`/`Select`/`OrderBy` you learned in Module 1.6 — EF turns them into SQL behind the scenes.

When should you *not* use an ORM? When you need a query hand-tuned for speed, or when the raw SQL is genuinely simpler than the EF version. In normal code, that doesn't happen often.

## Persistence entity vs engine model

We **don't** match `Kingdom.Engine.Kingdom` directly to a table — that class has interfaces, private fields, and a constructor that needs `IRandom` and `IClock`. EF can't read those cleanly.

Instead, we add **entity classes** in `Kingdom.Persistence` — flat C# classes with public properties, built for EF. Turning the engine model into entities and back is the persistence layer's job (just like the JSON DTO in Module 2.2).

```
Kingdom.Engine.Kingdom   <─ToEntity / FromEntity─>   KingdomEntity (EF)   <─EF─>   kingdoms (table)
```

Three layers, each with one job. The engine never imports `Microsoft.EntityFrameworkCore`.

(An *entity* here just means a class that EF maps to a table.)

## Delta starter

- **MODIFIED:** `Kingdom.Persistence/Kingdom.Persistence.csproj` (add EF Core SQLite package)
- **NEW:** `Kingdom.Persistence/EfCore/KingdomEntity.cs`, `BuildingEntity.cs`
- **NEW:** `Kingdom.Persistence/EfCore/KingdomDbContext.cs`
- **NEW:** `Kingdom.Persistence/EfCore/KingdomEfStore.cs` — Save/Load using EF
- **MODIFIED:** `Kingdom.Console/Program.cs` (a small EF Core demo at the end)
- **NEW:** `tests/Kingdom.Persistence.Tests/KingdomEfStoreTests.cs`

## Step 0 — install EF Core

```powershell
cd Kingdom.Persistence
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.Sqlite
```

Two packages: the core ORM, plus the SQLite driver. (For other databases you'd use `Microsoft.EntityFrameworkCore.SqlServer` or `.PostgreSQL` instead — the same EF code on top, just a different driver underneath.)

## Step 1 — entities

`Kingdom.Persistence/EfCore/KingdomEntity.cs`:

```csharp
namespace Kingdom.Persistence.EfCore;

public class KingdomEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public int Day { get; set; }
    public int Gold { get; set; }
    public int Wood { get; set; }
    public int Stone { get; set; }
    public int Food { get; set; }

    public List<BuildingEntity> Buildings { get; set; } = new();
}
```

`Kingdom.Persistence/EfCore/BuildingEntity.cs`:

```csharp
namespace Kingdom.Persistence.EfCore;

public class BuildingEntity
{
    public int Id { get; set; }
    public string Kind { get; set; } = "";   // "Farm", "Lumberyard", "Mine"
    public string Name { get; set; } = "";
    public int Level { get; set; }

    public int KingdomId { get; set; }       // foreign key
    public KingdomEntity? Kingdom { get; set; }   // navigation
}
```

The rules EF follows by default:

- A property called `Id` is automatically the primary key.
- If entity A has a `List<X>`, and entity X has an `int AId` plus an `A? A`, EF reads that as a one-to-many relationship (one A has many X). It works this out on its own.
- Properties need `set;` so EF can fill them in when it reads a row.

## Step 2 — `DbContext`

`Kingdom.Persistence/EfCore/KingdomDbContext.cs`:

```csharp
using Microsoft.EntityFrameworkCore;

namespace Kingdom.Persistence.EfCore;

public class KingdomDbContext : DbContext
{
    private readonly string _path;

    public KingdomDbContext(string dbPath) { _path = dbPath; }

    public DbSet<KingdomEntity> Kingdoms => Set<KingdomEntity>();
    public DbSet<BuildingEntity> Buildings => Set<BuildingEntity>();

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={_path};Pooling=False");
}
```

`DbSet<KingdomEntity> Kingdoms` is the `kingdoms` table. By default, EF names the table after the type, made plural. It works this out on its own. Same pattern: `DbSet<BuildingEntity> Buildings` is the `buildings` table.

`OnConfiguring` tells EF *"use SQLite at this path."* Real apps usually set the connection string somewhere else (in a DI container). For learning, writing it right here is fine.

## Step 3 — `KingdomEfStore` — save and load

`Kingdom.Persistence/EfCore/KingdomEfStore.cs`:

```csharp
using Kingdom.Engine.Buildings;
using Kingdom.Engine.Citizens;
using Kingdom.Engine.Infrastructure;
using Kingdom.Engine.Resources;
using Microsoft.EntityFrameworkCore;

namespace Kingdom.Persistence.EfCore;

public class KingdomEfStore
{
    private readonly string _dbPath;
    public KingdomEfStore(string dbPath) { _dbPath = dbPath; }

    /// <summary>Ensure the database file + schema exist.</summary>
    public void EnsureCreated()
    {
        using var ctx = new KingdomDbContext(_dbPath);
        ctx.Database.EnsureCreated();
    }

    public int Save(Kingdom.Engine.Kingdom kingdom)
    {
        EnsureCreated();
        using var ctx = new KingdomDbContext(_dbPath);

        var entity = new KingdomEntity
        {
            Name = kingdom.Name,
            Day  = kingdom.Day,
            Gold = kingdom.Resources.Get(Resource.Gold),
            Wood = kingdom.Resources.Get(Resource.Wood),
            Stone = kingdom.Resources.Get(Resource.Stone),
            Food  = kingdom.Resources.Get(Resource.Food),
            Buildings = kingdom.Buildings
                .Select(b => new BuildingEntity { Kind = b.GetType().Name, Name = b.Name, Level = b.Level })
                .ToList()
        };

        ctx.Kingdoms.Add(entity);
        ctx.SaveChanges();          // <-- writes everything in one transaction
        return entity.Id;            // EF populated this after SaveChanges
    }

    public Kingdom.Engine.Kingdom Load(int id, IRandom rng, IClock clock)
    {
        using var ctx = new KingdomDbContext(_dbPath);
        var entity = ctx.Kingdoms
            .Include(k => k.Buildings)   // <-- eager load the related buildings
            .Single(k => k.Id == id);

        var k = new Kingdom.Engine.Kingdom(entity.Name, rng, clock);
        k.Resources.SetTo(Resource.Gold, entity.Gold);
        k.Resources.SetTo(Resource.Wood, entity.Wood);
        k.Resources.SetTo(Resource.Stone, entity.Stone);
        k.Resources.SetTo(Resource.Food, entity.Food);

        foreach (var b in entity.Buildings)
        {
            Building bld = b.Kind switch
            {
                "Farm"        => new Farm(b.Name, b.Level),
                "Lumberyard"  => new Lumberyard(b.Name, b.Level),
                "Mine"        => new Mine(b.Name, b.Level),
                _ => throw new InvalidOperationException($"Unknown building kind '{b.Kind}'.")
            };
            k.AddBuilding(bld);
        }
        // (Day and Citizens not modelled in this minimal entity yet — Module 2.7 expands.)

        return k;
    }

    public IReadOnlyList<KingdomEntity> ListAll()
    {
        using var ctx = new KingdomDbContext(_dbPath);
        return ctx.Kingdoms
            .AsNoTracking()
            .OrderBy(k => k.Id)
            .ToList();
    }
}
```

Things to read carefully:

- `ctx.Database.EnsureCreated()` — creates the database file and tables if they don't exist yet. Handy for learning. We'll switch to *migrations* in Module 2.7 (the proper way).
- `ctx.Kingdoms.Add(entity); ctx.SaveChanges()` — two lines. EF turns them into the INSERT statements (it sends several at once). The `Buildings` list gets saved too, because EF keeps track of the link between them.
- `Include(k => k.Buildings)` — by default, EF doesn't load the related buildings. `Include` tells it *"also fetch the buildings."*
- `AsNoTracking()` — for read-only queries. It's faster and safer when you don't plan to change the results.

## Step 4 — call from console

In `Program.cs`, append:

```csharp
// EF Core demo (Module 2.6)
var efDb = Path.Combine(saveFolder, "kingdoms-ef.db");
if (File.Exists(efDb)) File.Delete(efDb);

var efStore = new KingdomEfStore(efDb);
var savedId = efStore.Save(kingdom);
Console.WriteLine();
Console.WriteLine($"=== EF Core demo ({efDb}) ===");
Console.WriteLine($"Saved kingdom #{savedId}");

var loaded = efStore.Load(savedId, new SystemRandom(0), new SystemClock());
Console.WriteLine($"Loaded: {loaded.Name} with {loaded.Buildings.Count} building(s), gold {loaded.Resources.Get(Resource.Gold)}");

var all = efStore.ListAll();
Console.WriteLine($"All saved kingdoms ({all.Count}):");
foreach (var e in all)
    Console.WriteLine($"  #{e.Id}  {e.Name}  day {e.Day}");
```

Build and run.

## Step 5 — tests

`tests/Kingdom.Persistence.Tests/KingdomEfStoreTests.cs`:

```csharp
using Kingdom.Engine;
using Kingdom.Engine.Buildings;
using Kingdom.Engine.Infrastructure;
using Kingdom.Engine.Resources;
using Kingdom.Persistence.EfCore;
using Shouldly;

namespace Kingdom.Persistence.Tests;

public class KingdomEfStoreTests
{
    [Fact]
    public void Save_ThenLoad_PreservesNameAndResources()
    {
        var path = Path.Combine(Path.GetTempPath(), $"ef-{Guid.NewGuid():N}.db");
        try
        {
            var k = new global::Kingdom.Engine.Kingdom("EFTest");
            k.Resources.Add(Resource.Gold, 500);
            k.AddBuilding(new Farm("MyFarm"));

            var store = new KingdomEfStore(path);
            var id = store.Save(k);

            var loaded = store.Load(id, new SystemRandom(0), new SystemClock());
            loaded.Name.ShouldBe("EFTest");
            loaded.Resources.Get(Resource.Gold).ShouldBe(600);  // 100 default + 500 added
            loaded.Buildings.Count.ShouldBe(1);
            loaded.Buildings.OfType<Farm>().Single().Name.ShouldBe("MyFarm");
        }
        finally { if (File.Exists(path)) File.Delete(path); }
    }

    [Fact]
    public void Save_TwoKingdoms_BothShowInListAll()
    {
        var path = Path.Combine(Path.GetTempPath(), $"ef-{Guid.NewGuid():N}.db");
        try
        {
            var store = new KingdomEfStore(path);
            store.Save(new global::Kingdom.Engine.Kingdom("Alpha"));
            store.Save(new global::Kingdom.Engine.Kingdom("Beta"));

            var all = store.ListAll();
            all.Count.ShouldBe(2);
            all.Select(e => e.Name).ShouldBe(new[] { "Alpha", "Beta" });
        }
        finally { if (File.Exists(path)) File.Delete(path); }
    }

    [Fact]
    public void Load_NonexistentId_Throws()
    {
        var path = Path.Combine(Path.GetTempPath(), $"ef-{Guid.NewGuid():N}.db");
        try
        {
            var store = new KingdomEfStore(path);
            store.EnsureCreated();    // file exists, just no rows
            Should.Throw<InvalidOperationException>(() =>
                store.Load(999, new SystemRandom(0), new SystemClock()));
        }
        finally { if (File.Exists(path)) File.Delete(path); }
    }
}
```

Run:

```powershell
dotnet test
```

Expect `Passed: 60` (57 + 3).

## Tinker

Open `bin/Debug/net10.0/saves/kingdoms-ef.db` in DB Browser for SQLite. The tables EF created are *exactly* what we'd have written by hand. Nothing hidden.

Add a property to `KingdomEntity`: `public DateTime SavedAt { get; set; }`. Run it again. It crashes — the database doesn't have that column yet. That's the problem migrations solve (Module 2.7).

Make EF print its SQL: in `OnConfiguring`, add `.LogTo(Console.WriteLine, LogLevel.Information)`. Run again. You'll see the real SQL that EF is writing. Reading it makes the ORM much less mysterious.

`db.Kingdoms.Where(k => k.Gold > 100).ToList()` — the same LINQ as `List<T>.Where`, turned into SQL. Try `OrderByDescending`, `Count`, `Sum`. They all work.

## What you just did

You replaced raw SQL with C# objects and let EF Core do the translating. Two entity classes, one `DbContext`, and a small store give you save, load, and list — with three new tests proving it works (60 total). You also met the three-layer pattern that holds the rest of this phase together: the engine model on one side, the database on the other, and an *entity* in the middle so the engine never has to import `Microsoft.EntityFrameworkCore`. Keeping those apart feels like extra work in a small project — and it is — but it's the difference between *"we can switch databases one day"* and *"we're stuck with this one forever."*

**Key concepts you can now name:**

- **ORM** — class-to-row translation library
- **`DbContext`** — your gateway to the database
- **`DbSet<T>`** — represents a table; LINQ-queryable
- **`Add` + `SaveChanges`** — EF's INSERT pattern
- **`Include`** — eager-load a related collection

## On your own

Time to put the book away. Don't scroll back up to the steps — show yourself, from your own head, that the one big idea stuck: with EF you add an object and call `SaveChanges`, and EF writes the SQL for you. No one marks this — the test runner does, which is the point. It's the fastest way to spot what hasn't stuck yet, while it's still small to fix. Getting stuck here is completely fine — that's exactly what it's for.

Without looking, write the three lines that save a new `KingdomEntity` (give it just a `Name`) to a `KingdomDbContext`, then a query that reads every kingdom back out. Run it. You should get your one kingdom back.

<details><summary>Stuck? Open this to check yourself.</summary>

```csharp
using var ctx = new KingdomDbContext(dbPath);
ctx.Database.EnsureCreated();

ctx.Kingdoms.Add(new KingdomEntity { Name = "Eldoria" });
ctx.SaveChanges();

foreach (var k in ctx.Kingdoms)
    Console.WriteLine($"#{k.Id} {k.Name}");   // #1 Eldoria
```

`Add` then `SaveChanges` is the whole save. EF gave the row an `Id` for you — you never wrote a line of SQL.

</details>

## Git move of the week — read a diff line by line

You changed a lot today: new EF entities, a DbContext, a new store. Before you merge or share this with someone (or with future-you), read the diff *carefully*, line by line.

In VS Code's Source Control panel: click any file under *Changes* to open the side-by-side diff. Go through the *hunks* one at a time (a hunk is a block of changed lines, with a few unchanged lines around it so you can see where you are). For each one, ask yourself *"why this line?"* The hunks you can't explain in plain English are the ones to either understand better or undo.

To read someone else's diff (a teammate's PR, or one of your own old commits): in the GitLens Commit Graph, click any commit. The panel on the right shows the same hunk-by-hunk view.

> **Or in the terminal:** `git show <commit-hash>` shows a single commit's diff. `git log -p` shows every commit's diff in your history.

## Wrap up

1. **Quiz** — open `quiz.md`, jot your answers in `journal/quiz-notes.md`.
2. **Progress** — one line in `journal/progress.md`: `Module 2.6 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit and push** — stage both files, commit message `Module 2.6 done`, Sync.
4. **Post in `#wins`** — one line about today, plus the URL of the commit.

Module 0.1 covers the why and the panel/CLI steps if you need a refresher. Bring quiz answers you're least sure about to the next weekly sync.

## Next

Module 2.7 introduces **migrations** — the proper way to change the database structure after the first release. `EnsureCreated` works for a *brand new database*. Migrations work when the *database already has data and the structure needs to change*.

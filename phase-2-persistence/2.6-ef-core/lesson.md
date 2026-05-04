# Module 2.6 — EF Core (Code-First)

> **Hook:** raw SQL is fine for one query. Twenty queries — each with its own connection/command/reader boilerplate — gets old fast. Today we meet **Entity Framework Core**, the .NET *ORM* (Object-Relational Mapper). You define C# classes; EF turns them into tables. You write `db.Kingdoms.Add(k); db.SaveChanges();` and the SQL writes itself.

> **Words to watch**
> - **ORM** — *Object-Relational Mapper*. Translates between objects in memory and rows in a database.
> - **Entity** — a class EF maps to a table.
> - **DbContext** — your gateway to the database; a class with `DbSet<T>` properties (one per table).
> - **DbSet<T>** — represents a table; supports LINQ queries (`db.Kingdoms.Where(...)`).
> - **Code-first** — define the schema in C# code; EF generates the SQL.
> - **Database-first** — opposite — generate C# from an existing schema. We're not doing that.

---

## Why an ORM

Once you've written `using var conn / cmd / reader` thirty times, you'll notice patterns. ORMs codify them:

| Raw SQL | EF Core |
|---|---|
| `INSERT INTO kingdoms (name) VALUES ($name)` | `db.Kingdoms.Add(k); db.SaveChanges();` |
| `SELECT * FROM kingdoms WHERE name = $n` | `db.Kingdoms.Single(k => k.Name == n)` |
| Manual mapping rows → objects | `Kingdom` is the type |

**EF is LINQ over a database.** The same `Where`/`Select`/`OrderBy` you learned in Module 1.6 — the LINQ provider translates them to SQL behind the scenes.

When *not* to use an ORM: when you need a hand-tuned query for performance, or when the SQL is genuinely simpler than the EF expression. That's rare in normal code.

## Persistence entity vs engine model

We **don't** map `Kingdom.Engine.Kingdom` directly to a table — that class has interfaces, private fields, and a constructor with `IRandom`/`IClock`. EF can't reflect on that cleanly.

Instead, we add **entity classes** in `Kingdom.Persistence` — flat C# classes with public properties, designed for EF. Conversion between engine model and entities is the persistence layer's job (just like the JSON DTO in Module 2.2).

```
Kingdom.Engine.Kingdom   <─ToEntity / FromEntity─>   KingdomEntity (EF)   <─EF─>   kingdoms (table)
```

Three layers, each with one job. **The engine never imports `Microsoft.EntityFrameworkCore`.**

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

Two packages: the core ORM + the SQLite driver. (You'd use `Microsoft.EntityFrameworkCore.SqlServer` or `.PostgreSQL` for those databases — same EF API on top of a different driver.)

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

EF conventions:
- A property called `Id` is automatically the primary key.
- A `List<X>` on entity A + an `int AId` + `A? A` on entity X = a one-to-many relationship. EF figures it out.
- Properties with `set;` are required for EF to populate them when reading.

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

`DbSet<KingdomEntity> Kingdoms` — that's the `kingdoms` table. The default name is the type name pluralised; EF figures that out. Same pattern: `DbSet<BuildingEntity> Buildings` = `buildings` table.

`OnConfiguring` tells EF *"use SQLite at this path."* Production apps usually configure the connection string elsewhere (DI container) — for our learning context, this inline form is fine.

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
        return ctx.Kingdoms.AsNoTracking().OrderBy(k => k.Id).ToList();
    }
}
```

Things to read carefully:

- `ctx.Database.EnsureCreated()` — creates the database file + tables if they don't exist. Convenient for learning; we'll switch to **migrations** in Module 2.7 (the proper way).
- `ctx.Kingdoms.Add(entity); ctx.SaveChanges()` — **two lines, one INSERT statement** (well, several, but EF batches them). The `Buildings` list is auto-saved too because EF tracks the relationship.
- `Include(k => k.Buildings)` — by default EF doesn't load navigation properties (lazy-loading is off). `Include` says "also fetch the buildings."
- `AsNoTracking()` — for read-only queries. Faster + safer when you don't intend to modify the results.

## Step 4 — call from console

In `Program.cs`, append:

```csharp
// EF Core demo (M2.6)
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

Build + run.

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

- Open `bin/Debug/net10.0/saves/kingdoms-ef.db` in DB Browser for SQLite. The schema EF created is *exactly* what we'd have written by hand. **No magic.**
- Add a property to `KingdomEntity`: `public DateTime SavedAt { get; set; }`. Re-run. **It crashes** — the existing schema doesn't have that column. That's the migrations problem (Module 2.7).
- Switch `EnsureCreated()` to log SQL: in `OnConfiguring` add `.LogTo(Console.WriteLine, LogLevel.Information)`. Re-run. **You'll see the actual SQL EF is generating.** Demystifies the ORM.
- `db.Kingdoms.Where(k => k.Gold > 100).ToList()` — same LINQ as `List<T>.Where`, translated to SQL. Try `OrderByDescending`, `Count`, `Sum`. They all work.

## Name it

- **ORM.** Object-Relational Mapper. Translates between objects (in memory) and rows (in a database).
- **`DbContext`.** Your handle to the database. One per *connection lifetime* — created, used, disposed.
- **`DbSet<T>`.** Represents a table. Supports LINQ queries.
- **Entity.** A class EF maps to a table.
- **Code-first.** You define entities in C#; EF creates the schema. Opposite of database-first.
- **`Add` + `SaveChanges`.** EF's INSERT pattern.
- **`Include`.** Eager-load a navigation property (a related table).
- **`AsNoTracking`.** For read-only queries — skip change-tracking, ~faster.
- **Migration.** A versioned schema change. Coming in Module 2.7.

## The rule of the through-line

> **The persistence layer maps two ways: engine ↔ entity ↔ database.** The middle (entity) exists so the engine doesn't depend on EF, and the database doesn't depend on engine quirks.

Three layers feels like a lot — and it is, for a 100-line program. But for any real app, **this separation is the difference between "we can change databases" and "we're stuck with this stack forever."**

## Quiz / challenge

Open `quiz.md`.

## Connect

Module 2.7 introduces **migrations** — the proper way to evolve the schema after the first deploy. `EnsureCreated` works for "fresh DB"; migrations work for "DB has data, schema needs to change."
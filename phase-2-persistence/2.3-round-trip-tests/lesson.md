# Module 2.3 — Round-Trip Tests

> **Hook:** yesterday we saved a *summary* of the kingdom. Today we save the *whole thing* and load it back. The discipline that lets us prove it works: a **round-trip test** — *"any kingdom I save will equal itself when I load it back."* Adding load support also forces a small redesign of the engine — and you'll learn why **persistence is the most honest pressure on a domain model.**

> **Words to watch**
> - **round-trip** — save then load; the loaded result should equal the original
> - **snapshot** — a complete data picture of the kingdom at a moment in time
> - **factory method** — a static method that returns an instance, instead of a constructor (e.g., `Kingdom.LoadFrom(snap, ...)`)
> - **property-based testing** — write *one* test that says "this should be true for *any* input." We'll do a light version with a loop.

---

## Why round-trip

Serialisation bugs are sneaky. The save looks fine in Notepad. The load returns *some* kingdom. But subtle things go wrong: a building's `Level` resets to 1, a citizen's job is lost, the resource ordering shifts. You don't notice until the player loads a save 3 weeks later and complains.

The cure: a test that says **"save *this* kingdom; load it back; everything equal."** Run it on dozens of randomised kingdoms; if any fails, the JSON shape is wrong. **One test, infinite confidence.**

## Persistence forces design

To round-trip a kingdom, we need to *reconstruct* it from data. Today's `Kingdom` constructor takes `(name, IRandom, IClock)` and lets you `AddBuilding(...)`, `AddCitizen(...)`. But `Day` has only a `private set;` — there's no way to load `Day = 47`. Same for `Building.Level`. Same for `Resources` (you can `Add` but the snapshot might say `Gold = 250`).

We have three options:

**A.** Make those setters public. *Unsafe* — anyone could now mess with state in production.
**B.** Add a `LoadFrom` static factory that knows how to set them. *Cleaner.*
**C.** Use reflection at load time to bypass setters. *Magical, fragile.*

Option B is the standard answer. We add a small **factory method** on `Kingdom` that takes a snapshot record and returns a fully-loaded `Kingdom`.

> **Lesson within a lesson:** *adding persistence often pushes you to redesign the model.* That's a feature, not a bug. The model that "looks right" sometimes only looks right for one runtime. Persistence forces you to confront the data shape.

## Delta starter

- **NEW:** `Kingdom.Engine/Snapshots/KingdomSnapshot.cs` (records: `KingdomSnapshot`, `BuildingSnapshot`, `CitizenSnapshot`)
- **MODIFIED:** `Kingdom.Engine/Kingdom.cs` (adds `ToSnapshot()`, `static LoadFrom(snap, rng, clock)`)
- **MODIFIED:** `Kingdom.Engine/Buildings/Building.cs` (adds protected constructor `(string name, int level)` for load)
- **MODIFIED:** `Kingdom.Engine/Buildings/Farm.cs`, `Lumberyard.cs`, `Mine.cs` (load constructor each)
- **MODIFIED:** `Kingdom.Persistence/KingdomJsonStore.cs` (gains `SaveFull` / `LoadFull` returning a `Kingdom`)
- **NEW:** `tests/Kingdom.Persistence.Tests/RoundTripTests.cs`

## Step 1 — `KingdomSnapshot` record

`Kingdom.Engine/Snapshots/KingdomSnapshot.cs`:

```csharp
namespace Kingdom.Engine.Snapshots;

public record KingdomSnapshot(
    string Name,
    int Day,
    int Gold, int Wood, int Stone, int Food,
    BuildingSnapshot[] Buildings,
    CitizenSnapshot[] Citizens
);

public record BuildingSnapshot(string Kind, string Name, int Level);

public record CitizenSnapshot(string Name);
```

A flat shape — arrays of small records. Easy for JSON, easy to read.

`Kind` is a string: `"Farm"`, `"Lumberyard"`, `"Mine"`. We'll switch on it during load.

## Step 2 — `Building` gains a load constructor

`Kingdom.Engine/Buildings/Building.cs`:

```csharp
namespace Kingdom.Engine.Buildings;

public class Building
{
    public string Name { get; }
    public int Level { get; private set; } = 1;

    public Building(string name) { Name = name; }

    // For load: rebuild a building at a specific level
    protected Building(string name, int level) { Name = name; Level = level; }

    public void Upgrade() => Level++;

    public virtual void Tick(ResourceLedger ledger) { }
}
```

`protected` — only the subclasses can use this constructor. Keeps the *normal* construction path the same (start at level 1).

`Farm.cs`, `Lumberyard.cs`, `Mine.cs` each get a matching constructor:

```csharp
public class Farm : Building
{
    public Farm(string name) : base(name) { }
    public Farm(string name, int level) : base(name, level) { }   // for load
    public override void Tick(ResourceLedger ledger) => ledger.Add(Resource.Food, 5 * Level);
}
```

(Same shape for Lumberyard and Mine.)

## Step 3 — `Kingdom.ToSnapshot()` and `Kingdom.LoadFrom(...)`

In `Kingdom.cs`, add:

```csharp
public KingdomSnapshot ToSnapshot()
{
    var buildings = Buildings
        .Select(b => new BuildingSnapshot(b.GetType().Name, b.Name, b.Level))
        .ToArray();
    var citizens = Citizens
        .Select(c => new CitizenSnapshot(c.Name))
        .ToArray();

    return new KingdomSnapshot(
        Name, Day,
        Resources.Get(Resource.Gold),
        Resources.Get(Resource.Wood),
        Resources.Get(Resource.Stone),
        Resources.Get(Resource.Food),
        buildings, citizens);
}

public static Kingdom LoadFrom(KingdomSnapshot snap, IRandom rng, IClock clock)
{
    var k = new Kingdom(snap.Name, rng, clock);

    // Resources are seeded in the constructor — overwrite to snapshot values
    k.Resources.SetTo(Resource.Gold, snap.Gold);
    k.Resources.SetTo(Resource.Wood, snap.Wood);
    k.Resources.SetTo(Resource.Stone, snap.Stone);
    k.Resources.SetTo(Resource.Food, snap.Food);

    foreach (var bs in snap.Buildings)
    {
        Building b = bs.Kind switch
        {
            "Farm"        => new Farm(bs.Name, bs.Level),
            "Lumberyard"  => new Lumberyard(bs.Name, bs.Level),
            "Mine"        => new Mine(bs.Name, bs.Level),
            _ => throw new InvalidOperationException($"Unknown building kind '{bs.Kind}'.")
        };
        k.AddBuilding(b);
    }
    foreach (var cs in snap.Citizens)
        k.AddCitizen(new Citizen(cs.Name));

    k._day = snap.Day;   // see step 4
    return k;
}
```

`SetTo` is a new method on `ResourceLedger` (Step 5). `_day` is a new private backing field (Step 4).

## Step 4 — `Kingdom.Day` becomes a backing field

To set Day from a static method that received an instance, we need to write to it. Options:
- Make `Day` setter `internal` (visible inside the engine).
- Add a private `_day` field that the property reads.

We'll go with the field. In `Kingdom.cs`:

```csharp
private int _day = 1;
public int Day => _day;

public void AdvanceDay()
{
    foreach (var b in Buildings) b.Tick(Resources);
    foreach (var _ in Citizens) Resources.Spend(Resource.Food, 1);

    var evt = _eventEngine.RollOnce(this);
    if (evt is not null) EventLog.Add(evt);

    _day++;
}
```

`Day` is now read-only from outside; `_day` is mutable from inside. Same external API — but `LoadFrom` can write to `_day` directly.

## Step 5 — `ResourceLedger.SetTo`

In `Kingdom.Engine/Resources/ResourceLedger.cs`, add:

```csharp
public void SetTo(Resource r, int amount)
{
    if (amount < 0) throw new ArgumentException("Amount must be non-negative.");
    _amounts[r] = amount;
}
```

Same validation as `Add`. Used only for load (and tests). Don't use it inside game logic.

## Step 6 — Persistence: `SaveFull` / `LoadFull`

In `Kingdom.Persistence/KingdomJsonStore.cs`, add:

```csharp
using Kingdom.Engine.Snapshots;

// ... existing Save/Load (summary) stays ...

public void SaveFull(Kingdom.Engine.Kingdom kingdom, string path)
{
    var snap = kingdom.ToSnapshot();
    File.WriteAllText(path, JsonSerializer.Serialize(snap, Options));
}

public Kingdom.Engine.Kingdom LoadFull(string path, IRandom rng, IClock clock)
{
    var snap = JsonSerializer.Deserialize<KingdomSnapshot>(File.ReadAllText(path))
        ?? throw new InvalidOperationException("Could not deserialize snapshot.");
    return Kingdom.Engine.Kingdom.LoadFrom(snap, rng, clock);
}
```

## Step 7 — round-trip tests

`tests/Kingdom.Persistence.Tests/RoundTripTests.cs`:

```csharp
using Kingdom.Engine;
using Kingdom.Engine.Buildings;
using Kingdom.Engine.Citizens;
using Kingdom.Engine.Infrastructure;
using Kingdom.Persistence;
using Shouldly;

namespace Kingdom.Persistence.Tests;

public class RoundTripTests
{
    [Fact]
    public void Empty_Kingdom_Roundtrips()
    {
        var k = new global::Kingdom.Engine.Kingdom("Empty");
        Roundtrip(k).Name.ShouldBe("Empty");
    }

    [Fact]
    public void NameAndDay_Survive_Roundtrip()
    {
        var k = new global::Kingdom.Engine.Kingdom("Test", new SystemRandom(7), new SystemClock());
        for (int i = 0; i < 25; i++) k.AdvanceDay();
        var loaded = Roundtrip(k);
        loaded.Name.ShouldBe("Test");
        loaded.Day.ShouldBe(26);
    }

    [Fact]
    public void Buildings_AndLevels_Survive_Roundtrip()
    {
        var k = new global::Kingdom.Engine.Kingdom("X");
        var f = new Farm("F"); f.Upgrade(); f.Upgrade();   // level 3
        k.AddBuilding(f);
        k.AddBuilding(new Mine("M"));
        k.AddBuilding(new Lumberyard("L"));

        var loaded = Roundtrip(k);

        loaded.Buildings.Count.ShouldBe(3);
        loaded.Buildings.OfType<Farm>().Single().Level.ShouldBe(3);
        loaded.Buildings.OfType<Mine>().Single().Name.ShouldBe("M");
    }

    [Fact]
    public void Resources_Survive_Roundtrip()
    {
        var k = new global::Kingdom.Engine.Kingdom("X");
        k.Resources.Add(Resource.Gold, 999);

        var loaded = Roundtrip(k);

        loaded.Resources.Get(Resource.Gold).ShouldBe(1099);  // 100 starting + 999 added
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(50)]
    [InlineData(200)]
    public void AnyKingdom_Roundtrips(int days)
    {
        // Property-style-ish: same setup, vary the day count
        var k = new global::Kingdom.Engine.Kingdom("Sweep", new SystemRandom(42), new SystemClock());
        k.AddBuilding(new Farm("F"));
        k.AddBuilding(new Lumberyard("L"));
        k.AddCitizen(new Citizen("A"));
        for (int i = 0; i < days; i++) k.AdvanceDay();

        var loaded = Roundtrip(k);

        loaded.Day.ShouldBe(k.Day);
        loaded.Buildings.Count.ShouldBe(k.Buildings.Count);
        foreach (var resource in Enum.GetValues<Resource>())
            loaded.Resources.Get(resource).ShouldBe(k.Resources.Get(resource));
    }

    private static Kingdom.Engine.Kingdom Roundtrip(Kingdom.Engine.Kingdom k)
    {
        var path = Path.Combine(Path.GetTempPath(), $"rt-{Guid.NewGuid():N}.json");
        try
        {
            var store = new KingdomJsonStore();
            store.SaveFull(k, path);
            return store.LoadFull(path, new SystemRandom(0), new SystemClock());
        }
        finally { if (File.Exists(path)) File.Delete(path); }
    }
}
```

The `[Theory]` test runs 4 times — each `[InlineData]` is one run. **All four pass = the round-trip works for that whole family of kingdoms.** That's the seed of property-based testing.

Run:

```powershell
dotnet test
```

Expect `Passed: 51` (43 + 8: 5 round-trip facts + the 1 theory expanding to 4 cases — actually 4 facts + 1 theory × 4 inputs = 8).

## Tinker

- Add a fifth `[InlineData]` — `999`. Runs in <1 second. Round-trip is *fast*.
- Edit a saved JSON file by hand: change `"Gold": 100` to `"Gold": 999999`. Reload. The kingdom now has 999999 gold. **JSON is honest** — whatever's in the file becomes the state.
- Delete the `Kind` field from a snapshot building. Reload. **Throws** at the `switch` — *"Unknown building kind ''."* That's a good failure: explicit and immediate.
- Add a fourth building subclass (`Quarry`?). Add it to the `switch`. **Watch the round-trip test pass without changes** — exactly because the test loops over whatever the kingdom contains.

## Name it

- **Round-trip.** Save then load; assert equal.
- **Snapshot.** Complete data shape of the model at a moment.
- **Factory method.** A static method returning an instance, used in place of a constructor. Common for "build from data" scenarios.
- **`protected` constructor.** Only subclasses can call it. Useful for "internal" ways of constructing without exposing them publicly.
- **Property-based testing (light).** Instead of testing one specific case, test a *property* that should hold across many cases — using `[Theory] + [InlineData]` or a real property library like FsCheck.

## The rule of the through-line

> **Persistence is the most honest pressure on a model.** When you can't load it back exactly, the model itself usually has the bug — not the persistence code.

The `Day` field redesign in this module wasn't *because* of JSON; it was already a flaw waiting to happen. JSON just made it visible.

## Quiz / challenge

Open `quiz.md`.

## Connect

Module 2.4 introduces **SQL** — the language databases speak. We'll set up SQLite (a self-contained file database) and write our first `INSERT` and `SELECT`. JSON files are great for "one save"; databases are great for "every save your player ever made, queryable in any direction."
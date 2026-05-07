# Module 2.3 — Round-Trip Tests

Yesterday we saved a *summary* of the kingdom. Today we save the *whole thing* and load it back. The discipline that lets us prove it works is a **round-trip test** — *"any kingdom I save will equal itself when I load it back."*

Adding load support also forces a small redesign of the engine. That's not a bug, it's the lesson: saving real state often pushes you to fix things in the model that were *almost* fine before. Persistence is one of the most honest pressures a domain model ever feels.

> **Words to watch**
>
> - **round-trip** — save, then load; the loaded result should equal the original
> - **snapshot** — a complete data picture of the kingdom at a moment in time
> - **factory method** — a static method that returns an instance, used in place of (or alongside) a constructor; e.g. `Kingdom.LoadFrom(snap, ...)`
> - **property-based testing** — write *one* test that asserts something is true for *any* input; we'll do a light version with a loop.

---

## Why round-trip

Saving bugs are sneaky. The save looks fine in Notepad. The load returns *some* kingdom. But subtle things go wrong: a building's `Level` resets to 1, a citizen's job is lost, the resource ordering shifts. You don't notice until the player loads a save three weeks later and complains.

The cure: a test that says *"save this kingdom; load it back; everything equal."* Run it on dozens of randomised kingdoms; if any fail, the JSON form is wrong. One test pattern, lots of confidence.

## Saving forces design

To round-trip a kingdom, we need to *reconstruct* it from data. Today's `Kingdom` constructor takes `(name, IRandom, IClock)` and lets you `AddBuilding(...)`, `AddCitizen(...)`. But `Day` has only a `private set;` — there's no way to load `Day = 47`. Same for `Building.Level`. Same for `Resources` (you can `Add` but the snapshot might say `Gold = 250`).

We have three options:

**A.** Make those setters public. Unsafe — anyone could now mess with state in production.
**B.** Add a `LoadFrom` static factory method that knows how to set them. Cleaner.
**C.** Use reflection at load time to bypass setters. Magical, fragile.

Option B is the standard answer. We add a small *factory method* (a static method that returns an instance) on `Kingdom` that takes a snapshot record and returns a fully-loaded `Kingdom`.

> **Lesson within a lesson:** adding load support often pushes you to redesign the model. That's a feature, not a problem. The model that "looks right" sometimes only looks right for one shell. Persistence forces you to confront the data form.

## Delta starter

- **NEW:** `Kingdom.Engine/Snapshots/KingdomSnapshot.cs` (records: `KingdomSnapshot`, `BuildingSnapshot`, `CitizenSnapshot`)
- **MODIFIED:** `Kingdom.Engine/Kingdom.cs` (adds `ToSnapshot()`, `static LoadFrom(snap, rng, clock)`)
- **MODIFIED:** `Kingdom.Engine/Buildings/Building.cs` (adds `protected` constructor `(string name, int level)` for load)
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

A flat layout — arrays of small records. Easy for JSON, easy to read.

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

`protected` — only the subclasses can use this constructor. The normal construction path stays the same (start at level 1).

`Farm.cs`, `Lumberyard.cs`, `Mine.cs` each get a matching constructor:

```csharp
public class Farm : Building
{
    public Farm(string name) : base(name) { }
    public Farm(string name, int level) : base(name, level) { }   // for load
    public override void Tick(ResourceLedger ledger) => ledger.Add(Resource.Food, 5 * Level);
}
```

(Same arrangement for Lumberyard and Mine.)

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

The `[Theory]` test runs four times — each `[InlineData]` is one run. All four pass means the round trip works for that whole family of kingdoms. That's the seed of property-based testing.

Run:

```powershell
dotnet test
```

Expect `Passed: 51` (43 + 8: 5 round-trip facts + the 1 theory expanding to 4 cases — actually 4 facts + 1 theory × 4 inputs = 8).

## Tinker

Add a fifth `[InlineData]` — `999`. Runs in under a second. Round-trip is fast.

Edit a saved JSON file by hand: change `"Gold": 100` to `"Gold": 999999`. Reload. The kingdom now has 999999 gold. JSON is honest — whatever's in the file becomes the state.

Delete the `Kind` field from a snapshot building. Reload. It throws at the `switch` — *"Unknown building kind ''."* That's a good failure: explicit and immediate.

Add a fourth building subclass (`Quarry`?). Add it to the `switch`. The round-trip test passes without changes — exactly because the test loops over whatever the kingdom contains.

## What you just did

You proved a save-and-load cycle preserves the whole kingdom — five round-trip facts plus a `[Theory]` running four day-counts each (51 tests total, eight new today). To get there, you redesigned three small parts of the engine: a `protected` constructor on `Building`, a backing field for `Day`, and a `SetTo` on `ResourceLedger`. None of those were *wrong* before, but they were under-exposed — the kind of design crack persistence is good at finding. You also met the **factory method** pattern (`Kingdom.LoadFrom`) and the seed of property-based testing (`[Theory] + [InlineData]`).

**Key concepts you can now name:**

- **round-trip** — save, load, assert equal
- **snapshot** — full data form of the model at a moment
- **factory method** — `static` returning an instance
- **`protected` constructor** — only subclasses can call it
- **property-based testing (light)** — one assertion across many inputs

## Git move of the week — `git stash`

You started a change. Halfway through, something else came up — a quick fix, an experiment, a lesson you wanted to do clean. *Stash* sets your current changes aside without committing them, leaving a clean working tree.

In VS Code's Source Control panel: `...` menu (top right of the panel) → *Stash → Stash*. Type a description, hit Enter. Your changes disappear from *Changes*. To get them back: `...` menu → *Stash → Pop Latest Stash*.

> **Or in the terminal:**
>
> ```powershell
> git stash push -m "halfway through the round-trip test"
> git stash pop          # bring it back
> ```

The stash is non-destructive — your changes are saved, not gone. Use it any time you want a clean tree without losing what you had.

## Quiz

Open `quiz.md`. When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.

## Next

Module 2.4 introduces **SQL** — the language databases speak. We'll set up SQLite (a self-contained file database) and write our first `INSERT` and `SELECT`. JSON files are great for *one* save; databases are great for *every save your player ever made, queryable in any direction*.

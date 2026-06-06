# Module 2.3 — Round-Trip Tests

> Travou no inglês? Abra o `lesson.pt.md` — é esta mesma lição em português. Tente em inglês primeiro.

Yesterday we saved a *summary* of the kingdom. Today we save the *whole kingdom* and load it back. The test that proves this works is a **round-trip test**: any kingdom I save should match itself when I load it back.

Adding load support also makes us change a small part of the engine. That's not a problem — it's part of the lesson. Saving real state often shows you things in the model that need fixing, even when they looked fine before. Saving puts honest pressure on your model and shows where it's weak.

> **Words to watch**
>
> - **round-trip** — save, then load; the loaded result should equal the original
> - **snapshot** — a complete data picture of the kingdom at a moment in time
> - **factory method** — a static method that returns an instance, used in place of (or alongside) a constructor; e.g. `Kingdom.LoadFrom(snap, ...)`
> - **property-based testing** — write *one* test that asserts something is true for *any* input; we'll do a light version with a loop.

---

## Why round-trip

Saving bugs are easy to miss. The save file looks fine in Notepad. The load returns *some* kingdom. But small things can go wrong: a building's `Level` resets to 1, a citizen's job is lost, the order of the resources changes. You don't notice until the player loads a save three weeks later and complains.

The fix: a test that says *"save this kingdom, load it back, and check that everything matches."* Run it on lots of random kingdoms. If any of them fail, the JSON form is wrong. One test pattern, and a lot of confidence that saving works.

## Saving forces design

To round-trip a kingdom, we need to *rebuild* it from data. Today's `Kingdom` constructor takes `(name, IRandom, IClock)` and lets you call `AddBuilding(...)` and `AddCitizen(...)`. But `Day` has only a `private set;`, so there's no way to load `Day = 47`. Same for `Building.Level`. Same for `Resources` (you can `Add`, but the snapshot might say `Gold = 250`).

We have three options:

**A.** Make those setters public. Unsafe — now anyone could change the state in a way they shouldn't.
**B.** Add a `LoadFrom` static factory method that knows how to set them. Cleaner.
**C.** Use reflection at load time to get around the setters. Clever, but fragile and hard to follow.

Option B is the standard answer. We add a small *factory method* (a static method that builds and returns an object) on `Kingdom`. It takes a snapshot record and returns a fully-loaded `Kingdom`.

> **Lesson within a lesson:** adding load support often makes you change the model. That's a good thing, not a problem. A model that "looks right" sometimes only looks right for one shell. Saving makes you face the real form of the data.

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

`Kind` is a string: `"Farm"`, `"Lumberyard"`, `"Mine"`. We'll use it to pick the right building type during load.

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

`protected` means only the subclasses can use this constructor. The normal way of making a building stays the same (it starts at level 1).

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

To set `Day` from a static method, we need a way to write to it. Two options:

- Make the `Day` setter `internal` (so it's visible inside the engine).
- Add a private `_day` field that the property reads from.

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

`Day` is now read-only from outside the class. `_day` can still be changed from inside. The outside still sees the same thing — but `LoadFrom` can now write to `_day` directly.

## Step 5 — `ResourceLedger.SetTo`

In `Kingdom.Engine/Resources/ResourceLedger.cs`, add:

```csharp
public void SetTo(Resource r, int amount)
{
    if (amount < 0) throw new ArgumentException("Amount must be non-negative.");
    _amounts[r] = amount;
}
```

Same check as `Add`. It's used only for loading (and in tests). Don't use it inside the game logic.

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

The `[Theory]` test runs four times — each `[InlineData]` is one run. When all four pass, the round trip works for that whole group of kingdoms. That's the start of property-based testing.

Run:

```powershell
dotnet test
```

Expect `Passed: 51` (43 + 8: 5 round-trip facts + the 1 theory expanding to 4 cases — actually 4 facts + 1 theory × 4 inputs = 8).

## Tinker

Add a fifth `[InlineData]` — `999`. It runs in under a second. The round trip is fast.

Edit a saved JSON file by hand: change `"Gold": 100` to `"Gold": 999999`. Reload. The kingdom now has 999999 gold. JSON is honest — whatever is in the file becomes the state.

Delete the `Kind` field from a building in a snapshot. Reload. It throws an error at the `switch` — *"Unknown building kind ''."* That's a good failure: clear, and it happens right away.

Add a fourth building subclass (a `Quarry`, maybe?). Add it to the `switch`. The round-trip test passes without any changes — because the test loops over whatever the kingdom actually holds.

## What you just did

You proved that saving and loading keeps the whole kingdom intact — five round-trip facts plus a `[Theory]` that runs four day-counts each (51 tests total, eight new today). To get there, you changed three small parts of the engine: a `protected` constructor on `Building`, a backing field for `Day`, and a `SetTo` on `ResourceLedger`. None of those were *wrong* before, but they didn't give you a way to set the state on load — the kind of small gap that saving is good at finding. You also met the **factory method** pattern (`Kingdom.LoadFrom`) and the start of property-based testing (`[Theory] + [InlineData]`).

**Key concepts you can now name:**

- **round-trip** — save, load, assert equal
- **snapshot** — full data form of the model at a moment
- **factory method** — `static` returning an instance
- **`protected` constructor** — only subclasses can call it
- **property-based testing (light)** — one assertion across many inputs

## On your own

Time to put the book away. Don't scroll back up to the steps — show yourself, from your own head, that the one big idea stuck: a round-trip test saves something, loads it back, and checks the two match. No one marks this — well, the test runner does, which is the point. It's the fastest way to spot what hasn't stuck yet, while it's still small to fix. Getting stuck here is completely fine — that's exactly what it's for.

Open the round-trip test file. Without looking at the others, write one new `[Fact]` that:

1. Makes a kingdom and advances it 30 days.
2. Saves it full, and loads it back through the `Roundtrip` helper.
3. Checks the loaded `Day` equals the original `Day`.
4. Run `dotnet test` — it should pass.

<details><summary>Stuck? Open this to check yourself.</summary>

```csharp
[Fact]
public void Day_Survives_30DayRoundtrip()
{
    var k = new global::Kingdom.Engine.Kingdom("Mine", new SystemRandom(1), new SystemClock());
    for (int i = 0; i < 30; i++) k.AdvanceDay();

    var loaded = Roundtrip(k);

    loaded.Day.ShouldBe(k.Day);   // both 31
}
```

The shape is always the same: build it, save+load it, assert the loaded thing equals the original. If the test passes, the save form keeps `Day`. If it fails, you found a real saving bug — that's a round-trip test earning its keep.

</details>

## Git move of the week — `git stash`

You started a change. Halfway through, something else came up — a quick fix, an experiment, or a lesson you wanted to start with a clean slate. *Stash* sets your current changes aside without committing them, and leaves you with a clean working tree.

In VS Code's Source Control panel: open the `...` menu (top right of the panel) → *Stash → Stash*. Type a description, press Enter. Your changes disappear from *Changes*. To get them back: `...` menu → *Stash → Pop Latest Stash*.

> **Or in the terminal:**
>
> ```powershell
> git stash push -m "halfway through the round-trip test"
> git stash pop          # bring it back
> ```

A stash is safe — your changes are saved, not lost. Use it any time you want a clean tree without throwing away what you had.

## Wrap up

1. **Quiz** — open `quiz.md`, jot your answers in `journal/quiz-notes.md`.
2. **Progress** — one line in `journal/progress.md`: `Module 2.3 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit and push** — stage both files, commit message `Module 2.3 done`, Sync.
4. **Post in `#wins`** — one line about today, plus the URL of the commit.

Module 0.1 covers the why and the panel/CLI steps if you need a refresher. Bring quiz answers you're least sure about to the next weekly sync.

## Next

Module 2.4 introduces **SQL** — the language databases speak. We'll set up SQLite (a database that lives in a single file) and write our first `INSERT` and `SELECT`. JSON files are great for *one* save. Databases are great for *every save your player ever made*, and you can ask them questions in any direction.

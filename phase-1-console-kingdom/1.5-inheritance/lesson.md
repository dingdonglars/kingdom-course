# Module 1.5 — Inheritance: Specialised Buildings

> **Hook:** yesterday `Tick()` did nothing. Today we make three kinds of building: `Farm` produces food, `Lumberyard` produces wood, `Mine` produces stone. Each one is a `Building` (same name, level, upgrade) — they just produce different things. This is **inheritance** — the OOP feature that says *"Farm is a kind of Building."*

> **Words to watch**
> - **inheritance** — when one class (the *child*) gets all the fields and methods of another (the *parent*) and can add its own
> - **subclass** (or *child class*) — `Farm`, `Lumberyard`, `Mine`
> - **base class** (or *parent class*) — `Building`
> - **`override`** — the keyword that says "I'm replacing the parent's `virtual` method"
> - **`base(...)`** — call the parent constructor from the child

---

## Why inheritance

Right now every `Building` is the same — a name and a level. Not very interesting. We want **three kinds**: farm, lumberyard, mine. Each ticks differently. Each has the same `Name` and `Level` though. We have two options:

**Option A — duplicate.** Three separate classes (`Farm`, `Lumberyard`, `Mine`), each with its own `Name`, `Level`, `Upgrade`. Every change has to be made three times. **Bad.**

**Option B — inheritance.** Define `Building` once with `Name`, `Level`, `Upgrade`, `Tick`. Then say *"`Farm` is a `Building`, plus it overrides `Tick` to produce food."* `Farm` inherits the rest for free. **This is what OOP is for.**

Inheritance has limits — it's easy to overuse and end up with confusing class trees (inheritance chains 5-deep). A common modern guideline is *"prefer composition over inheritance"* (we'll meet composition in Module 1.7). For *one level deep* — Building → Farm/Lumberyard/Mine — inheritance is exactly the right tool.

## Delta starter

This module's `starter/` adds three new subclass files and updates `Program.cs`:

- **NEW:** `Kingdom.Engine/Farm.cs`
- **NEW:** `Kingdom.Engine/Lumberyard.cs`
- **NEW:** `Kingdom.Engine/Mine.cs`
- **MODIFIED:** `Kingdom.Console/Program.cs` (uses the new subclasses)
- **NEW:** `tests/Kingdom.Engine.Tests/SubclassTests.cs`

`Building.cs`, `Kingdom.cs`, and the existing tests are unchanged from 1.4.

## Step 1 — three subclasses

Open `Farm.cs`:

```csharp
namespace Kingdom.Engine;

public class Farm : Building
{
    public Farm(string name) : base(name) { }

    public override void Tick(ResourceLedger ledger)
    {
        // Each level produces 5 food per day
        ledger.Add(Resource.Food, 5 * Level);
    }
}
```

Three things to read carefully:

1. **`: Building`** — this colon, in a class declaration, means *"inherit from."* `Farm` now has `Name`, `Level`, `Upgrade`, and `Tick` for free.
2. **`: base(name)`** — in the constructor, this passes `name` up to `Building`'s constructor. The base class still does the work of setting `Name`. The child only adds anything *new*.
3. **`override void Tick(...)`** — this replaces the empty default. The parameter `ledger` is the same `ResourceLedger` that `Kingdom.AdvanceDay` passes in. The farm reaches into the ledger and adds food.

The same pattern for `Lumberyard.cs`:

```csharp
namespace Kingdom.Engine;

public class Lumberyard : Building
{
    public Lumberyard(string name) : base(name) { }

    public override void Tick(ResourceLedger ledger)
    {
        ledger.Add(Resource.Wood, 3 * Level);
    }
}
```

And `Mine.cs`:

```csharp
namespace Kingdom.Engine;

public class Mine : Building
{
    public Mine(string name) : base(name) { }

    public override void Tick(ResourceLedger ledger)
    {
        ledger.Add(Resource.Stone, 2 * Level);
    }
}
```

Three almost-identical classes — that *feels* like duplication. It is, a little. The discipline question: *would each of these grow differently over time?* A future `Farm` might add a `Crop` enum; a future `Mine` might track `OreVein`. Yes, they would. So the classes are worth keeping separate. (When two classes really would never diverge, fold them into one. Don't make a class for every word in the design doc.)

## Step 2 — use the subclasses in `Program.cs`

```csharp
using Kingdom.Engine;

var kingdom = new Kingdom.Engine.Kingdom("Eldoria");
kingdom.AddBuilding(new Farm("Main Farm"));
kingdom.AddBuilding(new Lumberyard("Eastern Lumberyard"));
kingdom.AddBuilding(new Mine("Old Stone Mine"));
kingdom.AddCitizen(new Citizen("Lyra"));
kingdom.AddCitizen(new Citizen("Roric"));

PrintKingdom(kingdom);

for (int day = 0; day < 5; day++)
{
    kingdom.AdvanceDay();
    PrintKingdom(kingdom);
}

void PrintKingdom(Kingdom.Engine.Kingdom k)
{
    Console.WriteLine();
    Console.WriteLine($"== Day {k.Day} — {k.Name} ==");
    Console.WriteLine($"Buildings ({k.Buildings.Count}):");
    foreach (var b in k.Buildings)
        Console.WriteLine($"  - {b.GetType().Name} '{b.Name}' (level {b.Level})");
    Console.WriteLine($"Citizens:  {k.Citizens.Count}");
    Console.Write("Resources: ");
    foreach (var (r, n) in k.Resources.Snapshot())
        Console.Write($"{r}={n}  ");
    Console.WriteLine();
}
```

Notice `b.GetType().Name`. **`GetType()`** is a method every object has — it returns the *runtime type*. `.Name` gives the type's short name as a string (`"Farm"`, `"Lumberyard"`, `"Mine"`). That's how the same loop can show different building kinds without us writing a switch statement.

Build + run:

```powershell
dotnet build
dotnet run --project Kingdom.Console
```

You should see Day 1 → Day 6, with food *climbing* (5 from farm, minus 2 for citizens = +3/day), wood +3/day, stone +2/day. The resources actually move now.

## Step 3 — test the subclasses

`tests/Kingdom.Engine.Tests/SubclassTests.cs`:

```csharp
using Kingdom.Engine;
using Shouldly;

namespace Kingdom.Engine.Tests;

public class SubclassTests
{
    [Fact]
    public void Farm_Tick_AddsFoodEqualToFiveTimesLevel()
    {
        var ledger = new ResourceLedger();
        var farm = new Farm("F");
        farm.Tick(ledger);
        ledger.Get(Resource.Food).ShouldBe(5);
    }

    [Fact]
    public void Lumberyard_Tick_AddsWoodEqualToThreeTimesLevel()
    {
        var ledger = new ResourceLedger();
        var ly = new Lumberyard("L");
        ly.Tick(ledger);
        ledger.Get(Resource.Wood).ShouldBe(3);
    }

    [Fact]
    public void Mine_Tick_AddsStoneEqualToTwoTimesLevel()
    {
        var ledger = new ResourceLedger();
        var m = new Mine("M");
        m.Tick(ledger);
        ledger.Get(Resource.Stone).ShouldBe(2);
    }

    [Fact]
    public void Farm_Upgraded_ProducesMore()
    {
        var ledger = new ResourceLedger();
        var farm = new Farm("F");
        farm.Upgrade();   // level 2 now
        farm.Tick(ledger);
        ledger.Get(Resource.Food).ShouldBe(10);
    }

    [Fact]
    public void Subclass_InheritsName()
    {
        var farm = new Farm("Main Farm");
        farm.Name.ShouldBe("Main Farm");
        farm.Level.ShouldBe(1);
    }

    [Fact]
    public void Kingdom_AdvanceDay_RunsAllSubclassTicks()
    {
        var k = new global::Kingdom.Engine.Kingdom("Test");
        k.AddBuilding(new Farm("F"));
        k.AddBuilding(new Lumberyard("L"));
        k.AddBuilding(new Mine("M"));

        var foodBefore = k.Resources.Get(Resource.Food);
        var woodBefore = k.Resources.Get(Resource.Wood);
        var stoneBefore = k.Resources.Get(Resource.Stone);

        k.AdvanceDay();

        k.Resources.Get(Resource.Food).ShouldBe(foodBefore + 5);  // no citizens this test
        k.Resources.Get(Resource.Wood).ShouldBe(woodBefore + 3);
        k.Resources.Get(Resource.Stone).ShouldBe(stoneBefore + 2);
    }
}
```

Run:

```powershell
dotnet test
```

Expect `Passed: 22` (16 + 6).

## Tinker

- Add a `Quarry` (think marble for fancy buildings) — copy `Mine.cs`, change the name and the resource produced. Add a test. ~5 minutes.
- Try making `Building` `abstract` (`public abstract class Building`). The compiler now refuses `new Building("Generic")` anywhere. Does that break Module 1.3's tests? (Yes — `BuildingTests` uses `new Building("Farm")`.) Revert. Note: `abstract` is the next step we *could* take. We'll meet it when needed.
- Reverse the constructor: write `Building(string name) : this("default") { }` — what happens? (Stack overflow — recursive constructor calls. A real-world gotcha worth seeing once.)
- Add a method to `Building` called `Describe()` that returns `$"{GetType().Name} {Name} (level {Level})"`. Call it from `Program.cs`. Same pattern, less typing.

## Name it

- **Inheritance.** A class declares it inherits from another with `:` — gets all of its fields and methods, can add more, can override `virtual` ones.
- **`override`.** The keyword for replacing a `virtual` method. The C# compiler insists you write it (so you can't override by accident).
- **`base(...)`.** Calls the parent's constructor from the child's. Almost every child class does this for at least one field.
- **`GetType().Name`.** Gives the runtime type's short name. Useful for logging and debugging.

## The rule of the through-line

> **Inherit when a child genuinely *is a* parent.** If `Farm` is a `Building`, fine. If `LogManager` is a `Building`, that's a stretch — don't.

Inheritance is one tool, not the whole toolbox. You'll see *composition* (Module 1.7) and *interfaces* (Module 1.8) — both alternatives that often fit better.

## Quiz / challenge

Open `quiz.md`.

## Connect

Module 1.6 introduces **LINQ** — the query methods (`Where`, `Select`, `Sum`, `OrderBy`) that make working with the `Buildings` and `Citizens` lists much more pleasant than writing manual `for` loops.
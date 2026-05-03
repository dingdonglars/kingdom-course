# Module 1.6 — LINQ on the Kingdom

> **Hook:** today, almost no new lines of code, but a big change in how you *think*. The kingdom has lists — buildings, citizens. So far, every time you wanted to know something about them ("how many farms?", "highest-level building?"), you'd write a `for` loop. Today you stop. **LINQ** lets you describe the answer rather than the loop.

> **Words to watch**
> - **LINQ** — Language Integrated Query. A pile of helper methods (`Where`, `Select`, `Sum`, `Count`, `Any`, `OrderBy`, `First`) that work on every collection.
> - **predicate** — a function that returns `bool`. LINQ's `Where(b => b.Level > 1)` takes a predicate.
> - **lambda** — the `x => ...` syntax. A throwaway function written inline.
> - **deferred execution** — LINQ doesn't run immediately. It builds a recipe; the recipe runs when you ask for results.

---

## Why LINQ

Suppose Athos asks: *"How many farms does my kingdom have?"*

The before-LINQ way:

```csharp
int farmCount = 0;
foreach (var b in kingdom.Buildings)
    if (b is Farm) farmCount++;
```

Three lines. Easy to write. Easy to read once. But you'll write *fifty* of these in this kingdom, and they all blur together.

The LINQ way:

```csharp
int farmCount = kingdom.Buildings.OfType<Farm>().Count();
```

One line. Reads as *"of the buildings, the ones that are farms, count."* Once you know LINQ's twenty-or-so methods, you almost never write a loop just to count or filter.

LINQ is built into C# — no install needed, just `using System.Linq;` (often already there).

## Delta starter

This module changes only `Program.cs` and adds one test file. Engine classes don't change.

- **MODIFIED:** `Kingdom.Console/Program.cs` (uses LINQ for the report)
- **NEW:** `tests/Kingdom.Engine.Tests/LinqTests.cs`

Optionally, drop a tiny `Stats` helper into the engine — see Step 3.

## Step 1 — six methods to know

Type these into the C# REPL or a scratch file to feel them.

```csharp
using System.Linq;

var nums = new List<int> { 1, 2, 3, 4, 5 };

// Where — keep only the items the predicate likes
nums.Where(n => n > 2);              // 3, 4, 5

// Select — transform each item
nums.Select(n => n * n);             // 1, 4, 9, 16, 25

// Count — how many (optionally with a predicate)
nums.Count();                        // 5
nums.Count(n => n > 2);              // 3

// Sum — add them up
nums.Sum();                          // 15
nums.Sum(n => n * 10);               // 150 (sums after transforming)

// Any / All — return bool
nums.Any(n => n > 4);                // true (at least one)
nums.All(n => n > 0);                // true (every one)

// First / FirstOrDefault — get one item
nums.First(n => n > 3);              // 4 (throws if none)
nums.FirstOrDefault(n => n > 100);   // 0 (returns default if none)

// OrderBy / OrderByDescending — sort
nums.OrderByDescending(n => n);      // 5, 4, 3, 2, 1
```

That's 90% of the LINQ you'll ever use.

## Step 2 — apply to the kingdom

Update `Program.cs` to print a richer summary using LINQ:

```csharp
using Kingdom.Engine;

var kingdom = new Kingdom.Engine.Kingdom("Eldoria");
kingdom.AddBuilding(new Farm("Main Farm"));
kingdom.AddBuilding(new Farm("River Farm"));
kingdom.AddBuilding(new Lumberyard("Eastern Lumberyard"));
kingdom.AddBuilding(new Mine("Old Stone Mine"));
kingdom.AddCitizen(new Citizen("Lyra"));
kingdom.AddCitizen(new Citizen("Roric"));
kingdom.AddCitizen(new Citizen("Mira"));

PrintReport(kingdom);

for (int day = 0; day < 5; day++)
    kingdom.AdvanceDay();

PrintReport(kingdom);

void PrintReport(Kingdom.Engine.Kingdom k)
{
    Console.WriteLine();
    Console.WriteLine($"== Day {k.Day} — {k.Name} ==");

    var farms = k.Buildings.OfType<Farm>().Count();
    var lumberyards = k.Buildings.OfType<Lumberyard>().Count();
    var mines = k.Buildings.OfType<Mine>().Count();
    var totalLevels = k.Buildings.Sum(b => b.Level);
    var topBuilding = k.Buildings.OrderByDescending(b => b.Level).First();

    Console.WriteLine($"Buildings: {k.Buildings.Count} ({farms} farm, {lumberyards} lumberyard, {mines} mine) — total levels {totalLevels}");
    Console.WriteLine($"Top building: {topBuilding.GetType().Name} '{topBuilding.Name}' (level {topBuilding.Level})");
    Console.WriteLine($"Citizens: {k.Citizens.Count}");

    var foodPerDay = k.Buildings.OfType<Farm>().Sum(f => 5 * f.Level) - k.Citizens.Count;
    Console.WriteLine($"Food net per day: {foodPerDay:+0;-0;0}");

    Console.Write("Resources: ");
    foreach (var (r, n) in k.Resources.Snapshot())
        Console.Write($"{r}={n}  ");
    Console.WriteLine();
}
```

A lot of LINQ in one method. Read each line slowly:

- `OfType<Farm>()` — *"the buildings that are farms"*
- `.Count()` — *"how many"*
- `Sum(b => b.Level)` — *"add up `b.Level` across every building"*
- `OrderByDescending(b => b.Level).First()` — *"sort by level descending, take the top one"*
- `Sum(f => 5 * f.Level) - k.Citizens.Count` — daily food production minus daily food consumption

`{foodPerDay:+0;-0;0}` is a format string: positive numbers get `+`, negatives stay `-`, zero stays `0`. A small thing but it makes the report read better.

Build + run:

```powershell
dotnet build
dotnet run --project Kingdom.Console
```

Day 1 you'll see "Food net per day: +7" (2 farms × 5 = 10, minus 3 citizens = 7).

## Step 3 — (optional) extract a `Stats` helper

If `Program.cs` is starting to look heavy, push the LINQ into the engine:

`Kingdom.Engine/KingdomStats.cs` (new):

```csharp
namespace Kingdom.Engine;

public static class KingdomStats
{
    public static int FarmCount(this Kingdom k) => k.Buildings.OfType<Farm>().Count();
    public static int LumberyardCount(this Kingdom k) => k.Buildings.OfType<Lumberyard>().Count();
    public static int MineCount(this Kingdom k) => k.Buildings.OfType<Mine>().Count();
    public static int TotalBuildingLevels(this Kingdom k) => k.Buildings.Sum(b => b.Level);
    public static Building? TopBuilding(this Kingdom k) =>
        k.Buildings.OrderByDescending(b => b.Level).FirstOrDefault();
    public static int DailyFoodNet(this Kingdom k) =>
        k.Buildings.OfType<Farm>().Sum(f => 5 * f.Level) - k.Citizens.Count;
}
```

Notice **`this Kingdom k`** — the `this` keyword in the first parameter makes these *extension methods*. They're static methods that *look* like instance methods at the call site:

```csharp
kingdom.FarmCount();   // really KingdomStats.FarmCount(kingdom)
```

Extension methods are LINQ's secret weapon — `Where`, `Select`, etc. are themselves extension methods on `IEnumerable<T>`.

## Step 4 — test it

`tests/Kingdom.Engine.Tests/LinqTests.cs`:

```csharp
using Kingdom.Engine;
using Shouldly;

namespace Kingdom.Engine.Tests;

public class LinqTests
{
    [Fact]
    public void OfType_FiltersToFarmsOnly()
    {
        var k = new global::Kingdom.Engine.Kingdom("Test");
        k.AddBuilding(new Farm("F1"));
        k.AddBuilding(new Lumberyard("L1"));
        k.AddBuilding(new Farm("F2"));

        k.Buildings.OfType<Farm>().Count().ShouldBe(2);
    }

    [Fact]
    public void Sum_OfBuildingLevels_AddsUp()
    {
        var k = new global::Kingdom.Engine.Kingdom("Test");
        k.AddBuilding(new Farm("F1"));
        var f2 = new Farm("F2");
        f2.Upgrade(); f2.Upgrade();      // level 3
        k.AddBuilding(f2);

        k.Buildings.Sum(b => b.Level).ShouldBe(4);   // 1 + 3
    }

    [Fact]
    public void OrderByDescending_TopBuilding_IsHighestLevel()
    {
        var k = new global::Kingdom.Engine.Kingdom("Test");
        k.AddBuilding(new Farm("F1"));
        var top = new Mine("Big Mine");
        top.Upgrade(); top.Upgrade(); top.Upgrade();  // level 4
        k.AddBuilding(top);
        k.AddBuilding(new Lumberyard("L1"));

        k.Buildings.OrderByDescending(b => b.Level).First().Name.ShouldBe("Big Mine");
    }

    [Fact]
    public void Any_NoFarms_ReturnsFalse()
    {
        var k = new global::Kingdom.Engine.Kingdom("Test");
        k.AddBuilding(new Mine("M"));
        k.Buildings.Any(b => b is Farm).ShouldBeFalse();
    }

    [Fact]
    public void All_AllAtLevelOne_ReturnsTrue()
    {
        var k = new global::Kingdom.Engine.Kingdom("Test");
        k.AddBuilding(new Farm("F"));
        k.AddBuilding(new Mine("M"));
        k.Buildings.All(b => b.Level == 1).ShouldBeTrue();
    }
}
```

Run:

```powershell
dotnet test
```

Expect `Passed: 27` (22 + 5).

## Tinker

- Replace `OfType<Farm>().Count()` with `Count(b => b is Farm)`. Both work — the second uses the `is` keyword inline. Pick the one you find easier to read.
- Sort buildings *ascending* (cheapest first) with `OrderBy(b => b.Level)`. Print them. The output looks like a ranked list.
- Get the second-most-leveled building: `.OrderByDescending(b => b.Level).Skip(1).First()`. `Skip` is one of the rarer methods, but useful.
- Try `kingdom.Buildings.Where(b => b.Level > 5).First()` when no buildings have level > 5. **It throws.** Switch to `FirstOrDefault()`. Returns `null`. *That's the difference.*

## Name it

- **LINQ.** Language Integrated Query. ~50 methods on `IEnumerable<T>`.
- **Predicate.** A function returning `bool`. `b => b.Level > 1` is a predicate. LINQ uses them everywhere — `Where`, `Any`, `All`, `Count`, `First`.
- **Lambda.** The `x => expr` syntax. Reads as *"x goes to expr."* It's a method literal. The compiler cooks up a real method behind the scenes.
- **Extension method.** A static method whose first parameter has `this` — looks like an instance method at the call site. LINQ is built on extension methods.
- **Deferred execution.** `Where(...)` doesn't run yet — it returns a query object. The work happens when you call `.ToList()`, `.Count()`, `.First()`, or iterate with `foreach`. (Subtle. We'll meet this fully if it bites.)

## The rule of the through-line

> **Describe results, not loops.** When you want to *say what you want from a collection*, reach for LINQ. When you want to *do something with side effects*, a `foreach` is fine.

Every `for` you write in this codebase from now on is worth a second look — could LINQ say it more clearly?

## Quiz / challenge

Open `quiz.md`.

## Connect

Module 1.7 introduces **events** — when a citizen dies, a building burns down, a trader visits. Events are stored in a list, and LINQ will be exactly the right tool for "show me all the events from the last 7 days."
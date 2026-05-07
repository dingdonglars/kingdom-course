# Module 1.6 — LINQ on the Kingdom

Today is almost no new lines of code, but a real change in how you *think*. The kingdom has lists — buildings, citizens. So far, every time you wanted to know something about them ("how many farms?", "highest-level building?"), you'd write a `for` loop. Today you stop. **LINQ** lets you describe the answer instead of the loop.

LINQ stands for *Language Integrated Query*. It's a pile of helper methods (`Where`, `Select`, `Count`, `Sum`, `Any`, `OrderBy`, `First`, and friends) that work on every collection in C#. It's built into the language — no install needed, just `using System.Linq;` (often brought in for you already). Once you know about twenty of these methods, you almost never write a loop just to count or filter again.

> **Words to watch**
>
> - **LINQ** (Language Integrated Query) — a set of methods (`Where`, `Select`, `Sum`, `Count`, `Any`, `OrderBy`, `First`) that work on any collection
> - **predicate** — a function that returns `bool`. `Where(b => b.Level > 1)` takes a predicate.
> - **lambda** — the `x => ...` syntax. A throwaway function written inline.
> - **deferred execution** — LINQ doesn't run immediately. It builds a recipe; the recipe runs when you ask for results.
> - **extension method** — a static method whose first parameter has `this`, called like an instance method

---

## Why LINQ

Suppose you ask: *"How many farms does my kingdom have?"*

Without LINQ:

```csharp
int farmCount = 0;
foreach (var b in kingdom.Buildings)
    if (b is Farm) farmCount++;
```

Three lines. Easy to write. Easy to read once. But you'll write fifty of these in a kingdom this size, and they all blur together.

With LINQ:

```csharp
int farmCount = kingdom.Buildings.OfType<Farm>().Count();
```

One line. Reads as *"of the buildings, the ones that are farms — count."*

## Step 1 — six methods to know

Type these into a scratch file to feel them.

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
nums.First(n => n > 3);              // 4 (throws if no match)
nums.FirstOrDefault(n => n > 100);   // 0 (returns default if no match)

// OrderBy / OrderByDescending — sort
nums.OrderByDescending(n => n);      // 5, 4, 3, 2, 1
```

That's about 90% of the LINQ you'll ever use.

## Step 2 — apply to the kingdom

This module changes only `Program.cs` and adds one test file. Engine classes don't change.

- **MODIFIED:** `Kingdom.Console/Program.cs` (uses LINQ for the report)
- **NEW:** `tests/Kingdom.Engine.Tests/LinqTests.cs`

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
    var topBuilding = k.Buildings
        .OrderByDescending(b => b.Level)
        .First();

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

Read each LINQ line slowly. `OfType<Farm>()` keeps the items that are farms. `.Count()` returns how many. `Sum(b => b.Level)` adds up `b.Level` across every building. The `topBuilding` chain sorts by level descending and takes the top one. The food-per-day line says *"sum of (level × 5) across the farms, minus the number of citizens."*

`{foodPerDay:+0;-0;0}` is a format string for the printout — positive numbers get `+`, negatives stay `-`, zero stays `0`. A small thing that makes the output read better.

Build and run:

```powershell
dotnet build
dotnet run --project Kingdom.Console
```

Day 1 you'll see something like *"Food net per day: +7"* — two farms times five gives ten, minus three citizens gives seven.

## Step 3 — a `KingdomStats` helper (optional)

If `Program.cs` is starting to look heavy, push the LINQ into the engine.

`Kingdom.Engine/KingdomStats.cs`:

```csharp
namespace Kingdom.Engine;

public static class KingdomStats
{
    public static int FarmCount(this Kingdom k) => k.Buildings.OfType<Farm>().Count();
    public static int LumberyardCount(this Kingdom k) => k.Buildings.OfType<Lumberyard>().Count();
    public static int MineCount(this Kingdom k) => k.Buildings.OfType<Mine>().Count();
    public static int TotalBuildingLevels(this Kingdom k) => k.Buildings.Sum(b => b.Level);
    public static Building? TopBuilding(this Kingdom k) =>
        k.Buildings
            .OrderByDescending(b => b.Level)
            .FirstOrDefault();
    public static int DailyFoodNet(this Kingdom k) =>
        k.Buildings.OfType<Farm>().Sum(f => 5 * f.Level) - k.Citizens.Count;
}
```

Notice the `this Kingdom k` in the first parameter. The `this` keyword turns each of these into an **extension method** — a static method that *looks like* an instance method when you call it:

```csharp
kingdom.FarmCount();   // really KingdomStats.FarmCount(kingdom)
```

Extension methods are how LINQ itself works. `Where`, `Select`, and friends are all extension methods on `IEnumerable<T>` — that's why every collection in C# has them automatically.

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

        k.Buildings
            .OrderByDescending(b => b.Level)
            .First()
            .Name
            .ShouldBe("Big Mine");
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

You should see `Passed: 27` (22 plus 5 new ones).

## Tinker

Replace `OfType<Farm>().Count()` with `Count(b => b is Farm)`. Both work — the second uses the `is` keyword inline. Pick the one you find easier to read.

Sort buildings ascending (cheapest first) with `OrderBy(b => b.Level)`. Print the result. The output looks like a ranked list.

Get the second-highest-leveled building with `.OrderByDescending(b => b.Level).Skip(1).First()`. `Skip` is one of the rarer methods, but it's useful when you do reach for it.

Try `kingdom.Buildings.Where(b => b.Level > 5).First()` when no buildings have level > 5. It throws. Switch to `FirstOrDefault()`. It returns `null`. That's the difference — `First` says *"this had better be there"* and `FirstOrDefault` says *"if it's not there, that's fine."*

## A note on deferred execution

`Where(...)` doesn't actually filter the list when you call it. It returns a *recipe* — a query object that knows how to filter, but hasn't run yet. The work happens when you call `.ToList()`, `.Count()`, `.First()`, or iterate with `foreach`. This is called **deferred execution**. Subtle, but worth knowing — if you store a `Where(...)` result in a variable and then change the source list, the next time you iterate, you get the new filtered result, not the old one. We'll mention it again if it bites; for now, just be aware that LINQ is lazy by default.

## The through-line

The through-line in this module: **describe the result, not the loop**. When you want to *say what you want* from a collection, reach for LINQ. When you want to *do something with side effects* (print, save, modify), a `foreach` is fine. From here on, every `for` you write in this codebase is worth a second look — could LINQ say it more clearly?

## What you just did

You met LINQ — about ten methods that cover almost every "ask the list a question" you'll write this year. You used them to print a richer kingdom report, and you wrote five tests that prove the queries do what you said. You also met *extension methods*, which is the trick that makes LINQ feel built-in (every collection has these methods because someone declared them on `IEnumerable<T>` once). Twenty-seven passing tests now.

**Key concepts you can now name:**

- **LINQ** — query methods that work on any collection
- **predicate** — function returning bool, fed to `Where`/`Any`/`All`
- **lambda** — `x => expr`, a one-line inline function
- **extension method** — static method, `this` first parameter, called like instance
- **deferred execution** — LINQ runs lazily, when results are asked for

## Git move of the week — see your history as a graph

Your commit log is getting long enough to picture. Want to see it?

In VS Code, install the **GitLens** extension if you haven't (Extensions sidebar, search *"GitLens"*). Then `Ctrl + Shift + P` → *"GitLens: Show Commit Graph"*. The graph view opens — every commit you've made, parent links drawn, branches as coloured lanes. Reading your own commit graph is the first step toward reasoning about it.

> **Or in the terminal:** `git log --oneline --graph --decorate --all`.

We go properly into the model behind this graph in B3.1 if you take that bonus.

## Quiz

Open `quiz.md`. When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.

## Next

Module 1.7 introduces **events** — a citizen falls ill, a building burns down, a trader visits. Each event becomes a record stored in a list, and LINQ will be exactly the right tool for *"show me the events from the last seven days."*

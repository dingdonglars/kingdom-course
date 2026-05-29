# Module 1.6 — LINQ on the Kingdom

Today there are almost no new lines of code, but a real change in how you *think*. The kingdom has lists — buildings, citizens. So far, every time you wanted to know something about them ("how many farms?", "which building has the highest level?"), you wrote a `for` loop. Today you stop. **LINQ** (say it "link") lets you describe the answer instead of writing the loop.

LINQ stands for *Language Integrated Query*. It's a set of helper methods (`Where`, `Select`, `Count`, `Sum`, `Any`, `OrderBy`, `First`, and more) that work on every collection in C#. It's built into the language — nothing to install, just `using System.Linq;` (often added for you already). Once you know about twenty of these methods, you almost never write a loop just to count or filter again.

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

Three lines. Easy to write. Easy to read once. But you'll write fifty of these in a kingdom this size, and after a while they all look the same and are hard to tell apart.

With LINQ:

```csharp
int farmCount = kingdom.Buildings.OfType<Farm>().Count();
```

One line. Reads as *"of the buildings, the ones that are farms — count."*

## Step 1 — six methods to know

Type these into a spare file to try them out.

```csharp
using System.Linq;

var nums = new List<int> { 1, 2, 3, 4, 5 };

// Where — keep only the items that match the predicate
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

That's about 90% of the LINQ you'll ever need.

## Step 2 — apply to the kingdom

This module changes only `Program.cs` and adds one test file. Engine classes don't change.

- **MODIFIED:** `Kingdom.Console/Program.cs` (uses LINQ for the report)
- **NEW:** `tests/Kingdom.Engine.Tests/LinqTests.cs`

Update `Program.cs` to print a fuller summary using LINQ:

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

Read each LINQ line slowly. `OfType<Farm>()` keeps the items that are farms. `.Count()` gives back how many. `Sum(b => b.Level)` adds up `b.Level` across every building. The `topBuilding` chain sorts the buildings from highest level to lowest and takes the first one. The food-per-day line says *"add up (level × 5) across the farms, then subtract the number of citizens."*

`{foodPerDay:+0;-0;0}` is a format string for the printout — positive numbers get a `+`, negatives keep their `-`, and zero stays `0`. A small thing that makes the output easier to read.

Build and run:

```powershell
dotnet build
dotnet run --project Kingdom.Console
```

On Day 1 you'll see something like *"Food net per day: +7"* — two farms times five gives ten, minus three citizens gives seven.

## Step 3 — a `KingdomStats` helper (optional)

If `Program.cs` is starting to look crowded, move the LINQ into the engine.

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

Notice the `this Kingdom k` in the first parameter. The `this` keyword turns each of these into an **extension method** — a static method that *looks like* a method on the object when you call it:

```csharp
kingdom.FarmCount();   // really KingdomStats.FarmCount(kingdom)
```

Extension methods are how LINQ itself works. `Where`, `Select`, and the rest are all extension methods on `IEnumerable<T>` — that's why every collection in C# has them automatically.

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

Replace `OfType<Farm>().Count()` with `Count(b => b is Farm)`. Both work — the second one uses the `is` keyword right inside the call. Pick the one you find easier to read.

Sort buildings from lowest level to highest with `OrderBy(b => b.Level)`. Print the result. The output looks like a ranked list.

Get the second-highest-level building with `.OrderByDescending(b => b.Level).Skip(1).First()`. `Skip` is one of the less common methods, but it's useful when you do need it.

Try `kingdom.Buildings.Where(b => b.Level > 5).First()` when no buildings have a level above 5. It throws an error. Now switch to `FirstOrDefault()`. It returns `null` instead. That's the difference — `First` means *"there should be one here"*, and `FirstOrDefault` means *"if there isn't one, that's okay."*

## A note on deferred execution

`Where(...)` doesn't actually filter the list at the moment you call it. It gives back a *recipe* — a query that knows how to filter, but hasn't run yet. The real work happens when you call `.ToList()`, `.Count()`, `.First()`, or go through it with `foreach`. This is called **deferred execution** (deferred means "put off until later"). It's a small detail, but worth knowing: if you store a `Where(...)` result in a variable, then change the source list, the next time you go through it you get the new filtered result, not the old one. We'll bring it up again if it ever causes a problem. For now, just know that LINQ waits until the last moment to do its work.

## The through-line

The through-line in this module: **describe the result, not the loop**. When you want to *say what you want* from a collection, use LINQ. When you want to *do something that has side effects* (print, save, change), a `foreach` is fine. From here on, every `for` you write in this code is worth a second look — could LINQ say it more clearly?

## What you just did

You met LINQ — about ten methods that cover almost every "ask the list a question" you'll write this year. You used them to print a fuller kingdom report, and you wrote five tests that prove the queries do what you said. You also met *extension methods*, which are what makes LINQ feel built-in (every collection has these methods because someone added them to `IEnumerable<T>` once). Twenty-seven passing tests now.

**Key concepts you can now name:**

- **LINQ** — query methods that work on any collection
- **predicate** — a function that returns true or false, given to `Where`/`Any`/`All`
- **lambda** — `x => expr`, a short function written right where it's used
- **extension method** — static method with a `this` first parameter, called like a method on the object
- **deferred execution** — LINQ waits, and runs when you ask for the results

## On your own

Time to put the book away. Don't scroll back up to the steps — prove to yourself, from your own head, that the one big idea stuck: asking a list a question with LINQ instead of writing a loop. No one marks this one — it's just for you. It's the easiest way to spot what *hasn't* stuck yet, while it's still simple to fix. Getting stuck here is completely fine — that's exactly what it's for.

Here is the old `for`-loop way to count the mines:

```csharp
int mineCount = 0;
foreach (var b in kingdom.Buildings)
    if (b is Mine) mineCount++;
```

Without looking, rewrite that as a single LINQ line. Then write one more LINQ line on your own: the **total of all building levels** added up. Print both and run.

<details><summary>Stuck? Open this to check yourself.</summary>

```csharp
int mineCount = kingdom.Buildings.OfType<Mine>().Count();
int totalLevels = kingdom.Buildings.Sum(b => b.Level);
```

`OfType<Mine>()` keeps only the mines, then `.Count()` gives how many. `Sum(b => b.Level)` adds up `b.Level` across every building. One line each, and they read almost like the question you asked.

</details>

## Git move of the week — see your history as a graph

Your commit log is now long enough to draw as a picture. Want to see it?

In VS Code, install the **GitLens** extension if you haven't (Extensions sidebar, search *"GitLens"*). Then `Ctrl + Shift + P` → *"GitLens: Show Commit Graph"*. The graph view opens — every commit you've made, with lines drawn between each commit and the one before it, and branches shown as coloured lanes. Reading your own commit graph is the first step to understanding it.

> **Or in the terminal:** `git log --oneline --graph --decorate --all`.

We explain how this graph really works in B3.1, if you take that bonus.

## Wrap up

1. **Quiz** — open `quiz.md`, jot your answers in `journal/quiz-notes.md`.
2. **Progress** — one line in `journal/progress.md`: `Module 1.6 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit and push** — stage both files, commit message `Module 1.6 done`, Sync.
4. **Post in `#wins`** — one line about today, plus the URL of the commit.

Module 0.1 covers the why and the panel/CLI steps if you need a refresher. Bring quiz answers you're least sure about to the next weekly sync.

## Next

Module 1.7 introduces **events** — a citizen falls ill, a building burns down, a trader visits. Each event becomes a record stored in a list, and LINQ will be exactly the right tool for *"show me the events from the last seven days."*

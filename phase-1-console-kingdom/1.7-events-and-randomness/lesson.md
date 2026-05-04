# Module 1.7 — Events and Randomness

The kingdom is too predictable right now. Same farms produce same food, same citizens eat the same amount, every day forever. Today we add a roll — each tick, with some chance, *something happens*. A trader arrives with gold. A citizen falls ill. A building burns. Each event is recorded in a list, so by Day 50 the kingdom has a story you can read back.

Two reasons for an event log. The first is **the story** — a management game without events is a spreadsheet. Random visitors, fires, illnesses are the texture that makes the kingdom feel alive. The second is **memory** — when something interesting happens, you want to know *when*. The event log is the kingdom's memory. With LINQ from yesterday, you can ask *"what happened in the last seven days?"* in one line.

There's a problem we're going to introduce on purpose today, and fix in Module 1.8. The events come from `Random` — and using `Random` directly inside the engine makes the engine non-deterministic. The same starting state produces different events on different runs, and you can't write a meaningful test for that. We're going to feel that pain in Step 5 of this lesson, then fix it tomorrow.

> **Words to watch**
>
> - **event** — a thing that happened in the kingdom on a specific day. An object — title, day, kind.
> - **event log** — the list of events the kingdom has accumulated, in order
> - **`record`** — a C# keyword for a small immutable data class. Two records with the same fields are equal automatically.
> - **`Random`** — the standard library class for "give me a random number"
> - **deterministic** — same inputs always produce the same outputs

---

## Step 1 — `KingdomEvent` records

This module's starter:

- **NEW:** `Kingdom.Engine/KingdomEvent.cs` (one base record + three subclass records)
- **NEW:** `Kingdom.Engine/EventEngine.cs` (rolls each tick, returns an event or null)
- **MODIFIED:** `Kingdom.Engine/Kingdom.cs` (gains `EventLog` and runs the event engine each tick)
- **MODIFIED:** `Kingdom.Console/Program.cs` (prints the event log)
- **NEW:** `tests/Kingdom.Engine.Tests/EventLogTests.cs`

Open `KingdomEvent.cs`:

```csharp
namespace Kingdom.Engine;

public abstract record KingdomEvent(int Day, string Description);

public record TraderArrived(int Day, int GoldAmount)
    : KingdomEvent(Day, $"A trader arrived with {GoldAmount} gold.");

public record CitizenIll(int Day, string CitizenName)
    : KingdomEvent(Day, $"{CitizenName} fell ill.");

public record BuildingBurned(int Day, string BuildingName)
    : KingdomEvent(Day, $"{BuildingName} burned to the ground.");
```

The `record` keyword is C#'s shorthand for a small immutable data class. The line `public record TraderArrived(int Day, int GoldAmount)` is roughly the same as writing a class with two read-only properties, a constructor that sets them, an equality check that compares fields, a `ToString` that prints them, and a deconstructor — all generated for you. Two records with the same values are equal automatically, which is exactly the behaviour you want for events.

The pattern here: an `abstract record KingdomEvent` with the common fields (`Day`, `Description`), then three subclass records that add specifics. Each subclass passes a friendly description string up to the base record. That description is what gets printed in the log.

## Step 2 — `EventEngine`

Open `EventEngine.cs`:

```csharp
namespace Kingdom.Engine;

// NOTE: This class uses System.Random directly. That's bad for testing.
// Module 1.8 rewrites it to take an IRandom interface.
public class EventEngine
{
    private readonly Random _rng = new();

    public KingdomEvent? RollOnce(Kingdom k)
    {
        // 30% chance something happens this tick
        if (_rng.NextDouble() > 0.3) return null;

        // pick which event
        var pick = _rng.Next(3);
        return pick switch
        {
            0 => new TraderArrived(k.Day, _rng.Next(10, 51)),       // 10..50 gold
            1 when k.Citizens.Count > 0 =>
                new CitizenIll(k.Day, k.Citizens[_rng.Next(k.Citizens.Count)].Name),
            2 when k.Buildings.Count > 0 =>
                new BuildingBurned(k.Day, k.Buildings[_rng.Next(k.Buildings.Count)].Name),
            _ => null   // chosen event has no valid target — no event this day
        };
    }
}
```

Three things to notice. First, the **switch expression** — modern C# pattern: `pick switch { 0 => ..., 1 => ..., _ => default }`. The underscore means *"anything else."* It's cleaner than a stack of `if`/`else if`. Second, **pattern matching with `when`**: `1 when k.Citizens.Count > 0` says *"case 1, but only if there are citizens."* If there are none, the next pattern is checked. Third, the trader event doesn't actually add the gold to the ledger here — events are informational for now. We'll wire that up properly in M1.10's polish.

## Step 3 — wire it into `Kingdom`

Open `Kingdom.cs` and add the new fields and the roll inside `AdvanceDay`:

```csharp
namespace Kingdom.Engine;

public class Kingdom
{
    public string Name { get; }
    public int Day { get; private set; } = 1;
    public List<Building> Buildings { get; } = new();
    public List<Citizen> Citizens { get; } = new();
    public ResourceLedger Resources { get; } = new();
    public List<KingdomEvent> EventLog { get; } = new();    // NEW

    private readonly EventEngine _eventEngine = new();      // NEW

    public Kingdom(string name)
    {
        Name = name;
        Resources.Add(Resource.Gold, 100);
        Resources.Add(Resource.Wood, 50);
        Resources.Add(Resource.Stone, 20);
        Resources.Add(Resource.Food, 30);
    }

    public void AddBuilding(Building b) => Buildings.Add(b);
    public void AddCitizen(Citizen c) => Citizens.Add(c);

    public void AdvanceDay()
    {
        foreach (var b in Buildings) b.Tick(Resources);
        foreach (var _ in Citizens) Resources.Spend(Resource.Food, 1);

        var evt = _eventEngine.RollOnce(this);
        if (evt is not null) EventLog.Add(evt);

        Day++;
    }
}
```

Two new properties (`EventLog`, `_eventEngine`) and two new lines in `AdvanceDay`. Everything else is unchanged.

## Step 4 — print the event log

Run for 30 days instead of 5, and add an event-log section:

```csharp
using Kingdom.Engine;

var kingdom = new Kingdom.Engine.Kingdom("Eldoria");
kingdom.AddBuilding(new Farm("Main Farm"));
kingdom.AddBuilding(new Lumberyard("Eastern Lumberyard"));
kingdom.AddBuilding(new Mine("Old Stone Mine"));
kingdom.AddCitizen(new Citizen("Lyra"));
kingdom.AddCitizen(new Citizen("Roric"));

for (int day = 0; day < 30; day++)
    kingdom.AdvanceDay();

Console.WriteLine($"== {kingdom.Name} after {kingdom.Day - 1} days ==");
Console.WriteLine($"Buildings: {kingdom.Buildings.Count}");
Console.WriteLine($"Citizens:  {kingdom.Citizens.Count}");
Console.Write("Resources: ");
foreach (var (r, n) in kingdom.Resources.Snapshot())
    Console.Write($"{r}={n}  ");
Console.WriteLine();

Console.WriteLine();
Console.WriteLine($"=== Event log ({kingdom.EventLog.Count} entries) ===");
foreach (var e in kingdom.EventLog)
    Console.WriteLine($"  Day {e.Day,3}: {e.Description}");
```

`{e.Day,3}` is a format hint — pad the day to three characters wide. Aligns the log nicely.

Build and run twice:

```powershell
dotnet build
dotnet run --project Kingdom.Console
dotnet run --project Kingdom.Console
```

Different events each time. The kingdom is no longer deterministic. Hold that thought for the next step.

## Step 5 — test what we can (and feel the pain)

`tests/Kingdom.Engine.Tests/EventLogTests.cs`:

```csharp
using Kingdom.Engine;
using Shouldly;

namespace Kingdom.Engine.Tests;

public class EventLogTests
{
    [Fact]
    public void NewKingdom_HasEmptyEventLog()
    {
        var k = new global::Kingdom.Engine.Kingdom("Test");
        k.EventLog.ShouldBeEmpty();
    }

    [Fact]
    public void After50Days_LogHasSomeEvents()
    {
        var k = new global::Kingdom.Engine.Kingdom("Test");
        k.AddCitizen(new Citizen("A"));
        k.AddBuilding(new Farm("F"));
        for (int i = 0; i < 50; i++) k.AdvanceDay();

        // 30% per day x 50 days ~= 15 expected. We assert "some".
        k.EventLog.Count.ShouldBeGreaterThan(0);
    }

    [Fact]
    public void EventDay_AlwaysReflectsKingdomDayWhenLogged()
    {
        var k = new global::Kingdom.Engine.Kingdom("Test");
        k.AddCitizen(new Citizen("A"));
        for (int i = 0; i < 30; i++) k.AdvanceDay();

        // Every event's Day must be in [1, k.Day-1]
        k.EventLog.All(e => e.Day >= 1 && e.Day < k.Day).ShouldBeTrue();
    }
}
```

Three tests, and you can already see the trouble. We *cannot* test things like *"when the dice rolls 0, a TraderArrived event is created"*, because we have no way to control the dice. We can't say *"TraderArrived's GoldAmount is between 10 and 50"* — well, we could run it a thousand times and hope. We can't say *"this scenario produces this exact event sequence"* at all. So the tests are vague — *"some events happen"*, *"days are in range"*. They pass, but they barely verify anything. This is the pain that motivates Module 1.8.

Run:

```powershell
dotnet test
```

You should see `Passed: 30` (27 plus 3 new ones).

## Tinker

Run `dotnet run --project Kingdom.Console` ten times in a row. Different output every time. That's the non-determinism in action.

Try seeding `Random` with a fixed number — change `new Random()` to `new Random(42)`. Now runs are identical every time. Reproducible. But the seed is baked into the engine, which is also wrong for a real game (you want different worlds for different players). Module 1.8's `IRandom` lets the *shell* pick.

Increase the chance from 30% to 90%. The console gets noisy fast.

Add a fourth event subclass — `SecretFound`? — mostly mechanical. Wire it into the `switch`. Watch your tests still pass without changes. The tests are too loose to catch new behaviour — exactly the problem we'll fix tomorrow.

## The through-line

The through-line in this module: **engines should be deterministic by default**. Anything random must come *in* through a parameter, never sourced from `new Random()` deep inside. Today we broke this rule on purpose, to feel why it matters. Tomorrow we fix it.

## What you just did

You added a non-trivial system to the engine in about forty lines: a base record, three event subclasses, a small dice-roller, and a list to hold the results. You met `record` (C# shorthand for a small immutable data class with field-by-field equality), the modern `switch` expression, and pattern matching with `when`. You also met the limit of testing when randomness is hidden inside the engine — three vague tests is the best you can do, which is the lesson tomorrow opens with. Thirty passing tests now.

**Key concepts you can now name:**

- **event** — small immutable record describing one thing that happened
- **event log** — list of events in order, the kingdom's memory
- **`record`** — concise C# data class, equality compares fields
- **`switch` expression** — value-returning switch, `_` for default
- **non-deterministic engine** — same inputs, different outputs, untestable

## Quiz

Open `quiz.md`. When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.

## Next

Module 1.8 introduces **`IRandom`**, **`IClock`**, and **FakeItEasy** — the fix for everything painful in this module. Same `EventEngine`, this time properly testable. You'll feel the difference in the first three minutes.

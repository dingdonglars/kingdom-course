# Module 1.7 — Events & Randomness

> **Hook:** the kingdom is too predictable. Same farms produce same food, same citizens eat same amount, every day forever. Today we add a roll: **each tick, with some chance, something happens.** A trader arrives with gold. A citizen falls ill. A building burns. Each event is recorded — so by Day 50 the kingdom has a *story.*

> **Words to watch**
> - **event** — a thing that happened in the kingdom on a specific day. An object — title, day, kind.
> - **event log** — the list of events, in order, that the kingdom has accumulated.
> - **`record`** — a C# keyword for a small data class with auto value-equality. Perfect for events.
> - **`Random`** — the standard library class for "give me a random number."

---

## Why an event log

Two reasons.

**1. The story.** A management game without events is a spreadsheet. Random visitors, fires, illnesses are the texture that makes the kingdom feel alive. Today, the events are simple text. Tomorrow (1.8) they'll be testable. By Phase 4 the browser will animate them.

**2. Memory.** When something interesting happens, you want to know *when*. The event log is the kingdom's memory. With LINQ (1.6) you can ask "what happened in the last 7 days?" — a one-line query.

## Delta starter

- **NEW:** `Kingdom.Engine/KingdomEvent.cs` — record + 3 subclass records
- **NEW:** `Kingdom.Engine/EventEngine.cs` — rolls dice each tick, returns an event or null
- **MODIFIED:** `Kingdom.Engine/Kingdom.cs` — gains `EventLog` and runs the event engine each tick
- **MODIFIED:** `Kingdom.Console/Program.cs` — prints the event log
- **NEW:** `tests/Kingdom.Engine.Tests/EventLogTests.cs`

> ⚠ **Random is a problem we're going to fix tomorrow.** This module uses C#'s built-in `Random` directly. That makes the engine *non-deterministic* — the same starting state can produce different events. **You can't write a meaningful test for that.** We'll feel the pain in the tests. Module 1.8 introduces `IRandom` to make this testable. If you want the punchline now: **never `new Random()` deep in your engine. Take an `IRandom` as a constructor parameter.** Module 1.8 will rewrite this code to follow the rule.

## Step 1 — `KingdomEvent` records

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

**`record`** is C#'s shorthand for a small immutable data class with value equality. The line `public record TraderArrived(int Day, int GoldAmount)` is roughly equivalent to:

```csharp
public class TraderArrived
{
    public int Day { get; }
    public int GoldAmount { get; }
    public TraderArrived(int day, int goldAmount) { Day = day; GoldAmount = goldAmount; }
    // + auto-generated equality, ToString, deconstructor
}
```

Records are *the* C# way to model events. They're cheap to create and trivially safe to share.

The pattern is: an `abstract record KingdomEvent` with the common fields (`Day`, `Description`), then subclass records that add specifics. Each subclass passes a friendly `Description` to the base record. That description is what gets printed in the console log.

## Step 2 — `EventEngine`

Open `EventEngine.cs`:

```csharp
namespace Kingdom.Engine;

// NOTE: This class uses System.Random directly. That's bad for testing.
// Module 1.8 will rewrite it to take an IRandom interface.

public class EventEngine
{
    private readonly Random _rng = new();

    public KingdomEvent? RollOnce(Kingdom k)
    {
        // 30% chance something happens
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

Three things to notice:

1. **`switch` expression.** Modern C# pattern: `pick switch { 0 => ..., 1 => ..., _ => default }`. `_` is "anything else." Cleaner than a stack of `if/else if`.
2. **Pattern matching with `when`.** `1 when k.Citizens.Count > 0` says "case 1, *but only if* there are citizens." If not, the next pattern is checked.
3. **The trader event has a side effect we haven't implemented.** A real "trader arrived with 50 gold" should add the gold to the ledger. We're skipping that for now — events here are *informational*. We'll reconnect them in Module 1.10's polish, when the engine is fully wired.

## Step 3 — wire the engine into `Kingdom`

Open `Kingdom.cs` and add:

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

Two adds (`EventLog`, `_eventEngine`) and two new lines in `AdvanceDay` (the roll + log). Everything else is unchanged.

## Step 4 — print the event log

Open `Program.cs`, run for **30 days** instead of 5, and add an event-log section:

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

`{e.Day,3}` is a format hint: pad the day to 3 chars wide. Aligns the log nicely.

Build + run *twice*:

```powershell
dotnet build
dotnet run --project Kingdom.Console
dotnet run --project Kingdom.Console
```

Notice — different events each time. **The kingdom is no longer deterministic.** Read on.

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

        // 30% chance per day × 50 days ≈ 15 events expected. We assert "some".
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

Three tests — and you can already see the trouble. **We can't test:**
- "When the dice rolls 0, a TraderArrived event is created" — we can't control the dice
- "TraderArrived's GoldAmount is between 10 and 50" — yes, but only by running it 1000 times and hoping
- "A specific scenario produces a specific event sequence" — impossible

So our tests are vague: *"some events happen"*, *"days are in range."* They pass, but they barely verify anything. **This is the pain that motivates Module 1.8.**

Run:

```powershell
dotnet test
```

Expect `Passed: 30` (27 + 3).

## Tinker

- Run `dotnet run --project Kingdom.Console` ten times. Different output every time.
- Try seeding `Random` with a fixed number: change `new Random()` to `new Random(42)`. Now runs are *identical* every time. That's the right trick for reproducibility — but it bakes the seed into the engine, which is also wrong for production (you want different worlds for different players). Module 1.8's `IRandom` lets the *shell* pick.
- Increase the chance from 30% to 90%. Console gets noisy fast.
- Add a fourth event subclass (`SecretFound`?) — mostly mechanical. Wire it into the `switch`. Watch your tests still pass without changes (the test bar is too low — a problem we'll fix).

## Name it

- **Event.** A small immutable record describing something that happened.
- **Event log.** A list of events, in order. The kingdom's memory.
- **`record`.** C# keyword for a small data class with auto value-equality.
- **`Random`.** Standard-library class for random numbers. **Powerful and dangerous** — using it directly inside an engine breaks testing.
- **`switch` expression.** `value switch { pattern => result, ... }`. Cleaner than `if`/`else if` ladders.

## The rule of the through-line

> **Engines should be deterministic by default.** Anything random must come *in* through a parameter — never sourced from `new Random()` deep inside.

Today we broke this rule on purpose to feel why it matters. Tomorrow we fix it.

## Quiz / challenge

Open `quiz.md`.

## Connect

Module 1.8 introduces **`IRandom`**, **`IClock`**, and **FakeItEasy** — the fix for everything painful in this module. Same `EventEngine`, this time properly testable.
# Module 1.7 — Events and Randomness

The kingdom is too predictable right now. The same farms produce the same food, the same citizens eat the same amount, every day forever. Today we add chance. Each tick, there is some chance that *something happens*. A trader arrives with gold. A citizen falls ill. A building burns down. Each event is saved in a list, so by Day 50 the kingdom has a story you can read back.

There are two reasons for an event log. The first is **the story** — a management game with no events is just a spreadsheet. Random visitors, fires, and illnesses are what make the kingdom feel alive. The second is **memory** — when something interesting happens, you want to know *when* it happened. The event log is the kingdom's memory. With LINQ from yesterday, you can ask *"what happened in the last seven days?"* in one line.

There's a problem we're going to add on purpose today, then fix in Module 1.8. The events come from `Random`, and using `Random` directly inside the engine makes the engine non-deterministic. That means the same starting state produces different events each run, and you can't write a useful test for something that keeps changing. You'll run into this problem in Step 5 of this lesson, and we fix it tomorrow.

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

The `record` keyword is C#'s short way to write a small data class that can't be changed after it's made. The line `public record TraderArrived(int Day, int GoldAmount)` does about the same as writing a class with two read-only properties, a constructor that sets them, an equality check that compares the fields, a `ToString` that prints them, and a deconstructor — and C# writes all of that for you. Two records with the same values are equal automatically, which is exactly what you want for events.

The pattern here: one `abstract record KingdomEvent` with the shared fields (`Day`, `Description`), then three subclass records that add their own details. Each subclass passes a clear description string up to the base record. That description is what gets printed in the log.

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

Three things to notice. First, the **switch expression** — a modern C# pattern: `pick switch { 0 => ..., 1 => ..., _ => default }`. The underscore means *"anything else."* It's cleaner than a long chain of `if`/`else if`. Second, **pattern matching with `when`**: `1 when k.Citizens.Count > 0` says *"case 1, but only if there are citizens."* If there are none, C# checks the next pattern instead. Third, the trader event doesn't actually add the gold to the ledger here — for now, events just describe what happened. We'll connect that up properly in Module 1.10's polish.

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

Two new properties (`EventLog`, `_eventEngine`) and two new lines in `AdvanceDay`. Everything else stays the same.

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

`{e.Day,3}` is a formatting hint — it pads the day number to three characters wide. That lines up the log neatly.

Build and run twice:

```powershell
dotnet build
dotnet run --project Kingdom.Console
dotnet run --project Kingdom.Console
```

Different events each time. The kingdom is no longer deterministic. Keep that in mind for the next step.

## Step 5 — test what we can (and see the problem)

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

Three tests, and you can already see the problem. We *cannot* test things like *"when the dice roll a 0, a TraderArrived event is created"*, because we have no way to control the dice. We can't check that *"TraderArrived's GoldAmount is between 10 and 50"* — well, we could run it a thousand times and hope. We can't check that *"this exact setup produces this exact list of events"* at all. So the tests stay vague — *"some events happen"*, *"days are in range"*. They pass, but they barely check anything. This is the problem that Module 1.8 fixes.

Run:

```powershell
dotnet test
```

You should see `Passed: 30` (27 plus 3 new ones).

## Tinker

Run `dotnet run --project Kingdom.Console` ten times in a row. Different output every time. That's the non-determinism in action.

Try giving `Random` a fixed starting number — change `new Random()` to `new Random(42)`. Now every run is identical. You can repeat it exactly. But that starting number is now fixed inside the engine, which is also wrong for a real game (you want a different world for each player). Module 1.8's `IRandom` lets the *shell* choose instead.

Raise the chance from 30% to 90%. The console fills up with events fast.

Add a fourth event subclass — maybe `SecretFound`? — it's mostly the same steps. Add it to the `switch`. Notice that your tests still pass without any changes. The tests are too loose to notice the new behaviour — exactly the problem we'll fix tomorrow.

## The through-line

The through-line in this module: **engines should be deterministic by default**. Anything random must come *in* through a parameter, never be created with `new Random()` deep inside. Today we broke this rule on purpose, so you can see why it matters. Tomorrow we fix it.

## What you just did

You added a real system to the engine in about forty lines: a base record, three event subclasses, a small piece of code that rolls the dice, and a list to hold the results. You met `record` (C#'s short way to write a small data class that can't change, where equality compares the fields), the modern `switch` expression, and pattern matching with `when`. You also met the limit of testing when randomness is hidden inside the engine — three vague tests are the best you can do, which is exactly where tomorrow's lesson begins. Thirty passing tests now.

**Key concepts you can now name:**

- **event** — a small record that can't change, describing one thing that happened
- **event log** — a list of events in order, the kingdom's memory
- **`record`** — a short C# data class where equality compares the fields
- **`switch` expression** — a switch that returns a value, `_` for "anything else"
- **non-deterministic engine** — same inputs, different outputs, hard to test

## On your own

Time to put the book away. Don't scroll back up to the steps — prove to yourself, from your own head, that the one big idea stuck: writing a `record`. No one marks this one — it's just for you. It's the easiest way to spot what *hasn't* stuck yet, while it's still simple to fix. Getting stuck here is completely fine — that's exactly what it's for.

Add a fourth kind of event from memory: `SecretFound`. It should be a `record` that inherits from `KingdomEvent`, take a `Day` and a `string Place`, and pass a clear description up to the base record, like `$"A secret was found at {Place}."`. You don't have to add it to the `switch` — just get the record itself to compile.

<details><summary>Stuck? Open this to check yourself.</summary>

```csharp
public record SecretFound(int Day, string Place)
    : KingdomEvent(Day, $"A secret was found at {Place}.");
```

That one line gives you a small data class that can't be changed after it's made, with a constructor, value equality, and a `ToString` — all written for you. Two `SecretFound` records with the same `Day` and `Place` count as equal automatically.

</details>

## Wrap up

1. **Quiz** — open `quiz.md`, jot your answers in `journal/quiz-notes.md`.
2. **Progress** — one line in `journal/progress.md`: `Module 1.7 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit and push** — stage both files, commit message `Module 1.7 done`, Sync.
4. **Post in `#wins`** — one line about today, plus the URL of the commit.

Module 0.1 covers the why and the panel/CLI steps if you need a refresher. Bring quiz answers you're least sure about to the next weekly sync.

## Next

Module 1.8 introduces **`IRandom`**, **`IClock`**, and **FakeItEasy** — the fix for everything that was hard in this module. Same `EventEngine`, but this time you can test it properly. You'll see the difference in the first three minutes.

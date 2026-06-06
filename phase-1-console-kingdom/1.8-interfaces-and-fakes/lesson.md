# Module 1.8 — Interfaces, IRandom, IClock and FakeItEasy

> Travou no inglês? Abra o `lesson.pt.md` — é esta mesma lição em português. Tente em inglês primeiro.

> **Warm up — 30 seconds, from memory.** Before today, bring back what you built in Module 1.7:
>
> 1. What is the *event log*, and why is it called the kingdom's memory?
> 2. You added randomness last time. Why does that make the engine hard to test?
>
> Fuzzy on either? Re-read **Module 1.7** first — today fixes exactly that testing problem, so it stands right on top of it. Carry anything that felt shaky to the weekly sync.

Today is the lesson Module 1.7 was setting up. We take two interfaces out of the engine — `IRandom` (the dice) and `IClock` (the clock). The console gives the engine the real versions. Tests give it **fakes**, built with **FakeItEasy**. Now we can write tests like *"if the dice roll a 0, the next event is a TraderArrived for exactly 50 gold"*, and have them be true *every single time*.

The fix here is one of the most common patterns in modern code: **make an interface, take it in through the constructor, and let the caller choose the version**. Yesterday's `EventEngine` had `private readonly Random _rng = new();` — a hidden dependency that you couldn't change. Today's version has `private readonly IRandom _rng;`, set from a constructor parameter — a dependency you can see and can swap out. The change is small to read, but the difference in what you can test is huge.

An **interface** is a *contract* — a list of what something can do, with no say in how. Once `EventEngine` asks for "anything that fulfils the `IRandom` contract", you get to choose which version it gets:

```text
            IRandom            the contract: "ask me for a number, I give one back"
           /        \
   SystemRandom      a fake
   (real dice —      (test dice — returns
    the console       exactly what a test
    hands this in)    tells it to)
```

Same socket, two plugs. The engine doesn't know or care which one it got — it just calls the contract. That's the whole trick that makes the engine testable.

> **Words to watch**
>
> - **interface** — a contract: a list of method shapes with no bodies. Many classes can implement the same interface.
> - **dependency injection (DI)** — passing the other classes a class needs in through its constructor, instead of creating them with `new` inside
> - **fake / mock / stub** — a stand-in for a real class, used in tests. We use **FakeItEasy** to make them in one line.
> - **deterministic** — same inputs always produce the same outputs. The trait that makes engines testable.

---

## Why this matters

Yesterday's `EventEngine` had three problems. It was **hard to test** — tests couldn't say "given dice X, the result is Y", because there was no way to make the dice roll X. It was **impossible to repeat** — two players starting on the same day got different worlds. And it had a **hidden dependency** — reading the constructor didn't tell you that `EventEngine` secretly needed a source of random numbers. Bugs in `Random`'s behaviour (real ones do happen) would affect every engine that used it, with no warning.

The fix is the same in all three cases:

```csharp
// Before
public class EventEngine
{
    private readonly Random _rng = new();    // hidden, fixed
}

// After
public class EventEngine
{
    private readonly IRandom _rng;
    public EventEngine(IRandom rng) { _rng = rng; }   // visible, swappable
}
```

The shell decides. For the real program, give it a `SystemRandom`. For tests, give it a fake that returns whatever you want.

## Step 0 — install FakeItEasy

In your existing tests project:

```powershell
cd tests/Kingdom.Engine.Tests
dotnet add package FakeItEasy
```

This adds one line to the `.csproj`:

```xml
<PackageReference Include="FakeItEasy" Version="..." />
```

This module also adds:

- **NEW:** `Kingdom.Engine/IRandom.cs` and `SystemRandom.cs`
- **NEW:** `Kingdom.Engine/IClock.cs` and `SystemClock.cs`
- **MODIFIED:** `Kingdom.Engine/EventEngine.cs` (takes `IRandom`)
- **MODIFIED:** `Kingdom.Engine/Kingdom.cs` (takes `IRandom` + `IClock`, passes to `EventEngine`)
- **MODIFIED:** `Kingdom.Console/Program.cs` (builds the dependencies and passes them in)
- **NEW:** `tests/Kingdom.Engine.Tests/EventEngineTests.cs` (uses FakeItEasy)

## Step 1 — `IRandom` and `SystemRandom`

`Kingdom.Engine/IRandom.cs`:

```csharp
namespace Kingdom.Engine;

public interface IRandom
{
    /// <summary>Returns an integer in [minInclusive, maxExclusive).</summary>
    int Next(int minInclusive, int maxExclusive);

    /// <summary>Returns a double in [0.0, 1.0).</summary>
    double NextDouble();
}
```

The two methods are exactly the parts of `Random` that `EventEngine` was using. **Interfaces should be small** — just what the user of it needs, no more. If a class only uses two methods of `Random`, the interface should have those two and stop there.

`Kingdom.Engine/SystemRandom.cs`:

```csharp
namespace Kingdom.Engine;

/// <summary>Production implementation. Wraps System.Random.</summary>
public class SystemRandom : IRandom
{
    private readonly Random _rng;
    public SystemRandom() { _rng = new Random(); }
    public SystemRandom(int seed) { _rng = new Random(seed); }    // for reproducible runs

    public int Next(int minInclusive, int maxExclusive) => _rng.Next(minInclusive, maxExclusive);
    public double NextDouble() => _rng.NextDouble();
}
```

Two constructors — the one with no arguments means *"give me a fresh world"*, and the one with a seed means *"give me the same world I had last time"*. The shell picks which one it wants.

## Step 2 — `IClock` and `SystemClock`

Same pattern, but for *"what time is it?"*

`Kingdom.Engine/IClock.cs`:

```csharp
namespace Kingdom.Engine;

public interface IClock
{
    DateTime Now { get; }
}
```

`Kingdom.Engine/SystemClock.cs`:

```csharp
namespace Kingdom.Engine;

public class SystemClock : IClock
{
    public DateTime Now => DateTime.UtcNow;
}
```

We don't use `IClock` much this module, but the engine will start needing it in Phase 2 (saving) for *"saved at"* timestamps. We set it up today so the later code is ready for it.

## Step 3 — change `EventEngine` to take an `IRandom`

```csharp
namespace Kingdom.Engine;

public class EventEngine
{
    private readonly IRandom _rng;
    public EventEngine(IRandom rng) { _rng = rng; }

    public KingdomEvent? RollOnce(Kingdom k)
    {
        if (_rng.NextDouble() > 0.3) return null;

        var pick = _rng.Next(0, 3);
        return pick switch
        {
            0 => new TraderArrived(k.Day, _rng.Next(10, 51)),
            1 when k.Citizens.Count > 0 =>
                new CitizenIll(k.Day, k.Citizens[_rng.Next(0, k.Citizens.Count)].Name),
            2 when k.Buildings.Count > 0 =>
                new BuildingBurned(k.Day, k.Buildings[_rng.Next(0, k.Buildings.Count)].Name),
            _ => null
        };
    }
}
```

Three small changes from yesterday: the field type changed from `Random` to `IRandom`, the constructor now requires the dice, and `_rng.Next(3)` is now `_rng.Next(0, 3)` because `IRandom`'s contract uses the clearer two-argument form.

## Step 4 — `Kingdom` takes the dependencies

```csharp
namespace Kingdom.Engine;

public class Kingdom
{
    public string Name { get; }
    public int Day { get; private set; } = 1;
    public List<Building> Buildings { get; } = new();
    public List<Citizen> Citizens { get; } = new();
    public ResourceLedger Resources { get; } = new();
    public List<KingdomEvent> EventLog { get; } = new();

    private readonly EventEngine _eventEngine;
    private readonly IClock _clock;

    public Kingdom(string name, IRandom rng, IClock clock)
    {
        Name = name;
        _eventEngine = new EventEngine(rng);
        _clock = clock;

        Resources.Add(Resource.Gold, 100);
        Resources.Add(Resource.Wood, 50);
        Resources.Add(Resource.Stone, 20);
        Resources.Add(Resource.Food, 30);
    }

    // Convenience constructor for older tests (1.3-1.7).
    // Uses real System implementations.
    public Kingdom(string name) : this(name, new SystemRandom(), new SystemClock()) { }

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

The short extra constructor at the bottom is what keeps every earlier test working. `new Kingdom("Test")` still creates a kingdom — but now it builds a real `SystemRandom` and `SystemClock` for you behind the scenes. The pattern is worth remembering: when a class goes from needing no dependencies to needing several, keep the no-argument form by chaining to the longer one (`: this(...)`). New tests use the full form with fakes, and old tests still work.

## Step 5 — `Program.cs` builds the dependencies

```csharp
using Kingdom.Engine;

IRandom rng = new SystemRandom(seed: 42);   // remove the seed for unpredictable runs
IClock clock = new SystemClock();

var kingdom = new Kingdom.Engine.Kingdom("Eldoria", rng, clock);
kingdom.AddBuilding(new Farm("Main Farm"));
kingdom.AddBuilding(new Lumberyard("Eastern Lumberyard"));
kingdom.AddBuilding(new Mine("Old Stone Mine"));
kingdom.AddCitizen(new Citizen("Lyra"));
kingdom.AddCitizen(new Citizen("Roric"));

for (int day = 0; day < 30; day++)
    kingdom.AdvanceDay();

Console.WriteLine($"== {kingdom.Name} after {kingdom.Day - 1} days ==");
Console.Write("Resources: ");
foreach (var (r, n) in kingdom.Resources.Snapshot())
    Console.Write($"{r}={n}  ");
Console.WriteLine();

Console.WriteLine();
Console.WriteLine($"=== Event log ({kingdom.EventLog.Count} entries) ===");
foreach (var e in kingdom.EventLog)
    Console.WriteLine($"  Day {e.Day,3}: {e.Description}");
```

Run with the seed in place — the output is identical every run. Take the seed out — the output changes each run. The shell decides; the engine just does what it's told.

## Step 6 — testing with FakeItEasy

`tests/Kingdom.Engine.Tests/EventEngineTests.cs`:

```csharp
using FakeItEasy;
using Kingdom.Engine;
using Shouldly;

namespace Kingdom.Engine.Tests;

public class EventEngineTests
{
    [Fact]
    public void RollOnce_HighRoll_ReturnsNull()
    {
        // Arrange — make the dice roll above 0.3 so the engine returns nothing
        var rng = A.Fake<IRandom>();
        A.CallTo(() => rng.NextDouble()).Returns(0.9);
        var engine = new EventEngine(rng);
        var k = new global::Kingdom.Engine.Kingdom("Test");

        // Act
        var evt = engine.RollOnce(k);

        // Assert
        evt.ShouldBeNull();
    }

    [Fact]
    public void RollOnce_LowRollPickZero_GivesTrader()
    {
        var rng = A.Fake<IRandom>();
        A.CallTo(() => rng.NextDouble()).Returns(0.1);          // pass the threshold
        A.CallTo(() => rng.Next(0, 3)).Returns(0);              // pick 0 -> trader
        A.CallTo(() => rng.Next(10, 51)).Returns(50);           // exactly 50 gold
        var engine = new EventEngine(rng);
        var k = new global::Kingdom.Engine.Kingdom("Test");

        var evt = engine.RollOnce(k);

        evt.ShouldBeOfType<TraderArrived>();
        ((TraderArrived)evt!).GoldAmount.ShouldBe(50);
    }

    [Fact]
    public void RollOnce_LowRollPickOne_NoCitizens_ReturnsNull()
    {
        // Pick 1 = CitizenIll, but no citizens -> fall through to default -> null
        var rng = A.Fake<IRandom>();
        A.CallTo(() => rng.NextDouble()).Returns(0.1);
        A.CallTo(() => rng.Next(0, 3)).Returns(1);
        var engine = new EventEngine(rng);
        var k = new global::Kingdom.Engine.Kingdom("Test");
        // No citizens added.

        engine.RollOnce(k).ShouldBeNull();
    }

    [Fact]
    public void RollOnce_LowRollPickOne_WithCitizen_GivesIllness()
    {
        var rng = A.Fake<IRandom>();
        A.CallTo(() => rng.NextDouble()).Returns(0.1);
        A.CallTo(() => rng.Next(0, 3)).Returns(1);
        // The engine then calls rng.Next(0, k.Citizens.Count). One citizen -> 0.
        A.CallTo(() => rng.Next(0, 1)).Returns(0);

        var engine = new EventEngine(rng);
        var k = new global::Kingdom.Engine.Kingdom("Test");
        k.AddCitizen(new Citizen("Lyra"));

        var evt = engine.RollOnce(k);
        evt.ShouldBeOfType<CitizenIll>();
        ((CitizenIll)evt!).CitizenName.ShouldBe("Lyra");
    }

    [Fact]
    public void Kingdom_WithFixedRandom_IsFullyDeterministic()
    {
        // Two kingdoms with the same seed produce identical event logs
        var k1 = new global::Kingdom.Engine.Kingdom("A", new SystemRandom(seed: 42), new SystemClock());
        var k2 = new global::Kingdom.Engine.Kingdom("B", new SystemRandom(seed: 42), new SystemClock());
        k1.AddCitizen(new Citizen("X")); k1.AddBuilding(new Farm("F"));
        k2.AddCitizen(new Citizen("X")); k2.AddBuilding(new Farm("F"));

        for (int i = 0; i < 30; i++) { k1.AdvanceDay(); k2.AdvanceDay(); }

        k1.EventLog.Count.ShouldBe(k2.EventLog.Count);
        for (int i = 0; i < k1.EventLog.Count; i++)
            k1.EventLog[i].Description.ShouldBe(k2.EventLog[i].Description);
    }
}
```

Two kinds of tests there. Fakes (FakeItEasy) for the unit tests of `EventEngine` — exact control over what each call returns. And a real `SystemRandom` with a seed for the larger test, which proves that two full runs with the same seed produce the same story. Both kinds are deterministic.

Run:

```powershell
dotnet test
```

You should see `Passed: 35` (30 plus 5 new ones).

## Tinker

Remove the short `Kingdom(string name)` constructor that chains to the new one. Now every old test breaks with *"no constructor takes 1 argument"*. That's why we added the chained one. Put it back.

Try changing the seed in `Program.cs` from 42 to 7. You get a different kingdom story, but the same one every run.

In a test, set up the dice to roll three traders in three days. It's easy with FakeItEasy: `A.CallTo(() => rng.NextDouble()).ReturnsNextFromSequence(0.1, 0.1, 0.1);` and `A.CallTo(() => rng.Next(0, 3)).ReturnsNextFromSequence(0, 0, 0);`.

Add a property to `IRandom`: `int Seed { get; }`. The tests still compile, because `A.Fake<IRandom>()` builds a fake that matches any interface. But `SystemRandom` doesn't have this property yet, so the build fails at that class. Add the property. The interface made sure both sides stayed in agreement.

## The through-line

The through-line in this module: **every outside dependency comes in through an interface**. Random, clock, file system, network, database — none of them are created with `new` inside the engine. The shell supplies them. This is the rule that makes the same engine work in the console, in a web API, in a browser, and in Roblox. Each one swaps in the versions that fit it. The engine doesn't know or care which it got.

## What you just did

You took `IRandom` and `IClock` out of the engine, passed them in through the `EventEngine` and `Kingdom` constructors, and used **FakeItEasy** to build fakes that let your tests control the dice exactly. You wrote five new tests — four with fakes, one with a real seeded random — and reached a level of test precision that was impossible yesterday. The short extra constructor on `Kingdom` kept every old test working without changes. Thirty-five passing tests now, every one deterministic.

**Key concepts you can now name:**

- **interface** — a contract of method shapes, with no bodies
- **dependency injection** — the classes a class needs come in through its constructor
- **fake** (FakeItEasy) — a test-time stand-in for an interface
- **`A.CallTo(...).Returns(...)`** — exact control of what a fake returns
- **deterministic** — same inputs always give the same outputs

## On your own

Time to put the book away. Don't scroll back up to the steps — prove to yourself, from your own head, that the one big idea stuck: taking a dependency in through the constructor instead of creating it with `new` inside. No one marks this one — it's just for you. It's the easiest way to spot what *hasn't* stuck yet, while it's still simple to fix. Getting stuck here is completely fine — that's exactly what it's for.

Here is a class with a hidden dependency:

```csharp
public class Greeter
{
    private readonly IClock _clock = new SystemClock();
    public string Greet() => $"Hello! It is now {_clock.Now}.";
}
```

Without looking:

1. Rewrite `Greeter` so the `IClock` comes in through the constructor instead of being made inside.
2. In a test, use FakeItEasy to make a fake `IClock` and set its `Now` to a fixed date.
3. Check that `Greet()` puts that exact date in the message.

<details><summary>Stuck? Open this to check yourself.</summary>

```csharp
public class Greeter
{
    private readonly IClock _clock;
    public Greeter(IClock clock) { _clock = clock; }
    public string Greet() => $"Hello! It is now {_clock.Now}.";
}
```

```csharp
var clock = A.Fake<IClock>();
A.CallTo(() => clock.Now).Returns(new DateTime(2030, 1, 1));
var greeter = new Greeter(clock);
greeter.Greet().ShouldContain("2030");
```

Now the dependency is **visible** in the constructor and you can **swap it** for a fake. That's what made the dice testable in this lesson: the shell hands the engine its real version, and the test hands it one it controls.

</details>

## Git move of the week — branches

Until now your work has all been on `main`. From here on, anything bigger than a tiny change deserves its own branch — that way your `main` stays clean for finished, reviewed work.

In VS Code: click the branch name in the bottom-left status bar (it says `main`). A menu opens with *"Create new branch"* at the top. Type a name — like `feature/event-engine` — and you're on it. The bottom-left now shows the new branch.

To switch back: same place, pick `main`.

> **Or in the terminal:**
>
> ```powershell
> git switch -c feature/event-engine    # create + switch
> git switch main                        # switch back
> ```

A branch in git is just a marker that points at a commit and moves as you add more; making one costs nothing. We explain how this really works in B3.1.

## Wrap up

1. **Quiz** — open `quiz.md`, jot your answers in `journal/quiz-notes.md`.
2. **Progress** — one line in `journal/progress.md`: `Module 1.8 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit and push** — stage both files, commit message `Module 1.8 done`, Sync.
4. **Post in `#wins`** — one line about today, plus the URL of the commit.

Module 0.1 covers the why and the panel/CLI steps if you need a refresher. Bring quiz answers you're least sure about to the next weekly sync.

## Next

Module 1.9 slows down and looks at **code organisation** — folders, files, namespaces, what stays in `Engine` and what moves to `Engine.Events` (a sub-namespace). After 1.8, the engine has enough types that grouping them is worth doing.

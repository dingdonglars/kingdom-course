# Module 1.8 — Interfaces, IRandom, IClock & FakeItEasy

> **Hook:** today is the lesson Module 1.7 was setting up. We extract two interfaces — `IRandom` (the dice) and `IClock` (the wall-clock) — out of the engine. Production code passes in real implementations. Tests pass in **fakes** with `FakeItEasy`. Suddenly we can write tests like *"given the dice rolls 0, the next event is a TraderArrived for exactly 50 gold"* and have them be true *every time.*

> **Words to watch**
> - **interface** — a contract: a list of method shapes, no bodies. Many classes can implement the same interface.
> - **dependency injection (DI)** — passing a class's collaborators in through its constructor (or method parameters), instead of newing them up inside.
> - **fake / mock / stub** — a stand-in for a collaborator in a test. We use **FakeItEasy** to make these one-liners.
> - **deterministic** — same inputs always produce the same outputs. The trait that makes engines testable.

---

## Why this matters

Yesterday's `EventEngine` had `private readonly Random _rng = new();`. Three problems:

1. **Untestable.** Tests can't say "given the dice rolls X, the result is Y" — there's no way to make the dice roll X.
2. **Unrepeatable.** Two players starting on the same day get different worlds.
3. **Hidden coupling.** `EventEngine` *secretly depends on* `Random`. Reading the constructor doesn't tell you that. Bugs in `Random`'s behavior would affect every engine that uses it, with no signal.

The fix is one of the most common patterns in modern code: **define an interface, accept it via the constructor, let the caller pick the implementation.**

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

The shell decides — for production, hand it a `SystemRandom`. For tests, hand it a fake that returns whatever you want.

## Delta starter

- **NEW:** `Kingdom.Engine/IRandom.cs` + `SystemRandom.cs`
- **NEW:** `Kingdom.Engine/IClock.cs` + `SystemClock.cs`
- **MODIFIED:** `Kingdom.Engine/EventEngine.cs` (takes `IRandom`)
- **MODIFIED:** `Kingdom.Engine/Kingdom.cs` (takes `IRandom` + `IClock`, passes to `EventEngine`)
- **MODIFIED:** `Kingdom.Console/Program.cs` (constructs and injects)
- **NEW:** `tests/Kingdom.Engine.Tests/EventEngineTests.cs` (uses FakeItEasy)
- **MODIFIED:** `tests/Kingdom.Engine.Tests/Kingdom.Engine.Tests.csproj` (adds FakeItEasy package)

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

The two methods are *exactly* the bits of `Random` that `EventEngine` was using. **Interfaces should be small** — just what the consumer needs.

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

Two constructors — the no-arg one for "give me a fresh world", the seeded one for "give me the same world I had last time."

## Step 2 — `IClock` and `SystemClock`

Same pattern, for "what time is it":

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

We don't use `IClock` heavily in this module — but the engine will start needing it in Block 4 (persistence) for "saved at" timestamps. **Today we set up the door so that future code can walk through it.**

## Step 3 — wire `EventEngine` to take an `IRandom`

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

Three small changes:
- The field type changed from `Random` to `IRandom`.
- The constructor now requires the rng.
- `_rng.Next(3)` is now `_rng.Next(0, 3)` — `IRandom`'s contract has the explicit two-arg form (cleaner).

## Step 4 — `Kingdom` takes an `IRandom` and an `IClock`

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

    // Convenience constructor for 1.3-1.7-style code paths (and BuildingTests).
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

The **convenience constructor** keeps every prior test working — `new Kingdom("Test")` now creates a kingdom with real implementations behind the scenes.

> **Pattern:** when a class grows from 0 dependencies to N, keep the no-arg form by chaining (`: this(...)`). New tests use the explicit constructor with fakes; old tests don't break.

## Step 5 — `Program.cs` constructs the dependencies

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

Run twice with the seed in place — output is *identical*. Remove the seed — output varies. **The shell decides; the engine just does what it's told.**

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
        // Arrange — make the dice roll *above* 0.3 so the engine returns nothing
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
        A.CallTo(() => rng.NextDouble()).Returns(0.1);          // pass the gate
        A.CallTo(() => rng.Next(0, 3)).Returns(0);              // pick 0 → trader
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
        // Pick 1 = CitizenIll, but no citizens → fall through to default → null
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
        // The engine will then call rng.Next(0, k.Citizens.Count). One citizen → 0.
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
        // Two kingdoms with the *same seed* should produce identical event logs
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

Two kinds of tests there — fakes (FakeItEasy) for the unit tests of `EventEngine`, and a real `SystemRandom` with a *seed* for the integration test. **Both are deterministic.** The fake gives surgical control; the seeded random gives end-to-end realism.

Run:

```powershell
dotnet test
```

Expect `Passed: 35` (30 + 5).

## Tinker

- Remove the `: this(name, new SystemRandom(), new SystemClock())` convenience constructor. Watch every old test break with "no constructor takes 1 argument." That's why we added the chain.
- Try changing the production seed in `Program.cs` from 42 to 7. Different kingdom story, but consistent across runs.
- In a test, set up the dice to roll three trades in three days. Not hard with FakeItEasy: `A.CallTo(() => rng.NextDouble()).ReturnsNextFromSequence(0.1, 0.1, 0.1);`
- Add a property to `IRandom`: `int Seed { get; }`. **Tests fail to compile** — everywhere that did `A.Fake<IRandom>()` is fine, but `SystemRandom` doesn't implement it. Add it. Notice how the contract enforces consistency.

## Name it

- **Interface.** A contract — method/property *shapes*, no bodies. Many classes can implement.
- **Dependency injection (DI).** Passing collaborators in via the constructor, instead of newing them up inside.
- **Fake / mock.** A test-time stand-in for an interface. FakeItEasy creates them with `A.Fake<IRandom>()`.
- **Determinism.** Same inputs → same outputs. The trait that makes engines testable.
- **`A.CallTo(() => x.M()).Returns(y)`.** FakeItEasy syntax: *"when someone calls `M()` on `x`, return `y`."*

## The rule of the through-line

> **Every external dependency comes in through an interface.** Random, clock, file system, network, database — none get newed up inside the engine. The shell wires them.

This is the rule that makes the same engine usable in console, in a web API, in a browser, in Roblox. The runtime can swap each dependency for whatever fits the host.

## Quiz / challenge

Open `quiz.md`.

## Connect

Module 1.9 takes a step back to **code organisation** — folders, files, namespaces, what stays in `Engine` vs what moves to `Engine.Events` (sub-namespace). After 1.8, the engine has enough types to start grouping them.
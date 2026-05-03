# Module 1.4 — The Game Loop

> **Hook:** until now your kingdom is a static photograph. You build it, you print it, the program ends. Today it starts to **tick**. Each day, every farm makes food, every lumberyard makes wood, every mine makes stone. Citizens eat. Numbers move. The kingdom is alive.

> **Words to watch**
> - **tick** — one step of game time. In our kingdom, one tick = one day.
> - **game loop** — the loop that advances the world one tick at a time and prints the new state
> - **side effect** — when a method changes state somewhere (vs. just returning a value)
> - **delta** — the change since last time (today's gold − yesterday's gold)

---

## Why a tick

Real-time games tick many times per second. Turn-based games tick once per turn. A management game like ours ticks once per *day*. The exact unit doesn't matter — what matters is **the engine has a verb that means "move time forward."**

Once you have `AdvanceDay()`, three doors open at once:
- The console can call it in a loop and print the result (today)
- The web API can expose it as `POST /tick` (Phase 3)
- The browser/Roblox client can call it on a button click (Phase 4-5)

Same engine, three triggers. That's the through-line again.

## Delta starter

This module's `starter/` only has the **new and changed files**. Open your 1.3 code, copy these on top:

- `Kingdom.Engine/Building.cs` — gains a virtual `Tick(ResourceLedger)` method
- `Kingdom.Engine/Kingdom.cs` — gains `AdvanceDay()` and `Day` property
- `Kingdom.Console/Program.cs` — runs a 5-day loop and prints the deltas
- `tests/Kingdom.Engine.Tests/KingdomTickTests.cs` — new test file

If you'd rather start fresh, copy your 1.3 `starter/` into a new working folder and apply the changes manually as you read.

## Step 1 — `Building.Tick`

Open `Building.cs`. Right now it's just a holder for `Name` and `Level`. Today it gets a verb:

```csharp
namespace Kingdom.Engine;

public class Building
{
    public string Name { get; }
    public int Level { get; private set; } = 1;

    public Building(string name) { Name = name; }

    public void Upgrade() => Level++;

    // NEW: each subclass will override this. Default does nothing.
    public virtual void Tick(ResourceLedger ledger) { }
}
```

The keyword is **`virtual`**. It says: *"subclasses are allowed to replace this method."* In Module 1.5 we'll add `Farm`, `Lumberyard`, and `Mine` subclasses that each override `Tick` to produce a different resource. Today the base class has an empty default — buildings tick but produce nothing.

> **Why empty?** Because today we don't have subclasses yet. The empty default means "the system runs end-to-end before the subclasses exist." We'll fill it in tomorrow.

## Step 2 — `Kingdom.AdvanceDay` and `Day`

Open `Kingdom.cs`. Add a `Day` property and an `AdvanceDay` method:

```csharp
namespace Kingdom.Engine;

public class Kingdom
{
    public string Name { get; }
    public int Day { get; private set; } = 1;
    public List<Building> Buildings { get; } = new();
    public List<Citizen> Citizens { get; } = new();
    public ResourceLedger Resources { get; } = new();

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
        // 1. Every building ticks
        foreach (var b in Buildings)
            b.Tick(Resources);

        // 2. Every citizen eats one food (no food = nothing happens, for now)
        foreach (var _ in Citizens)
            Resources.Spend(Resource.Food, 1);

        // 3. Day counter advances
        Day++;
    }
}
```

Three things happen each tick: buildings produce, citizens consume, day counter ticks. Order matters — buildings produce first, *then* citizens eat. (If you flipped them, on day 1 citizens would eat food that hasn't been produced yet.)

> **Side effects.** `AdvanceDay()` returns `void` — but it changes the ledger, the day counter, and (eventually) building state. That's a *side effect*. Methods that "do things" rather than "return things" are common in engines. The downside: harder to reason about. The upside: the call site is clean (`k.AdvanceDay()` reads like English).

## Step 3 — the console loop

Open `Program.cs`. Today's shell runs **5 days** and prints the kingdom each day:

```csharp
using Kingdom.Engine;

var kingdom = new Kingdom.Engine.Kingdom("Eldoria");
kingdom.AddBuilding(new Building("Main Farm"));
kingdom.AddBuilding(new Building("Old Mine"));
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
    Console.WriteLine($"Buildings: {k.Buildings.Count}");
    Console.WriteLine($"Citizens:  {k.Citizens.Count}");
    Console.Write("Resources: ");
    foreach (var (r, n) in k.Resources.Snapshot())
        Console.Write($"{r}={n}  ");
    Console.WriteLine();
}
```

Build + run:

```powershell
dotnet build
dotnet run --project Kingdom.Console
```

You should see Day 1 → Day 6, with food going down by 2 each day (two citizens, one food each). The other resources don't move yet — that's tomorrow.

## Step 4 — test the loop

> **Heads up — small C# quirk.** Inside the test namespace `Kingdom.Engine.Tests`, the unqualified word `Kingdom` is ambiguous: it could mean the outer `Kingdom` *namespace*, or the `Kingdom` *class*. The compiler picks the namespace and the test won't compile. Workaround: write `global::Kingdom.Engine.Kingdom` — the `global::` prefix says *"start at the top of the namespace tree."* You'll see this once or twice; that's it.

Open `tests/Kingdom.Engine.Tests/KingdomTickTests.cs`:

```csharp
using Kingdom.Engine;
using Shouldly;

namespace Kingdom.Engine.Tests;

public class KingdomTickTests
{
    [Fact]
    public void NewKingdom_StartsAtDay1()
    {
        var k = new global::Kingdom.Engine.Kingdom("Test");
        k.Day.ShouldBe(1);
    }

    [Fact]
    public void AdvanceDay_IncrementsDayCounter()
    {
        var k = new global::Kingdom.Engine.Kingdom("Test");
        k.AdvanceDay();
        k.Day.ShouldBe(2);
    }

    [Fact]
    public void AdvanceDay_CitizensConsumeFood()
    {
        var k = new global::Kingdom.Engine.Kingdom("Test");
        k.AddCitizen(new Citizen("A"));
        k.AddCitizen(new Citizen("B"));
        var foodBefore = k.Resources.Get(Resource.Food);
        k.AdvanceDay();
        k.Resources.Get(Resource.Food).ShouldBe(foodBefore - 2);
    }

    [Fact]
    public void AdvanceDay_NoFood_DoesNotCrash()
    {
        var k = new global::Kingdom.Engine.Kingdom("Test");
        k.AddCitizen(new Citizen("A"));
        // Drain food
        k.Resources.Spend(Resource.Food, k.Resources.Get(Resource.Food));
        Should.NotThrow(() => k.AdvanceDay());
    }

    [Fact]
    public void AdvanceDay_TenDays_CountsCorrectly()
    {
        var k = new global::Kingdom.Engine.Kingdom("Test");
        for (int i = 0; i < 10; i++) k.AdvanceDay();
        k.Day.ShouldBe(11);
    }
}
```

Run:

```powershell
dotnet test
```

Expect `Passed: 16` (11 from 1.3 + 5 new).

## Tinker

- Change the loop length from 5 to 100 days. Watch food go negative? It shouldn't — `Spend` returns false rather than going negative. Confirm by `Get(Resource.Food)` after the loop.
- Add a third citizen. Now food drops 3/day instead of 2.
- Try calling `AdvanceDay()` 1000 times. How long does it take? (Should be instant — engines should be fast.)
- Move the `Day++` line to the *top* of `AdvanceDay()` instead of the bottom. Does anything visibly break? (Subtle — first day's tick now runs on Day 2. Shows why ordering matters.)

## Name it

- **Tick.** A single step of game time. The engine's heartbeat. Every game has one — fast for action games, slow for management games.
- **Game loop.** A loop that calls `Tick` over and over. Today's loop is `for (5 days)`. Phase 4's browser version will be `setInterval(() => kingdom.tick(), 1000)`. Phase 5's Roblox version will be `RunService.Heartbeat:Connect(...)`. Same loop pattern, three runtimes.
- **Side effect.** When a method changes state (vs. just returning a value). `AdvanceDay()` is one big side effect.
- **`virtual`.** A method that subclasses can override. The base class provides a default (here: empty). Tomorrow's `Farm.Tick` will override it to add food.

## The rule of the through-line

> **The engine ticks. The shell decides when.**

The engine doesn't know how often `AdvanceDay` gets called — that's the shell's choice. Today: 5 times in a `for` loop. Tomorrow: every time a button is clicked. Engine doesn't care.

## Quiz / challenge

Open `quiz.md`.

## Connect

Module 1.5 introduces inheritance — `Farm`, `Lumberyard`, `Mine` will all override `Tick`. Then `AdvanceDay` will start moving *all* the resources, not just food.
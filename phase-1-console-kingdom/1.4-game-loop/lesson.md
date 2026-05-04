# Module 1.4 — The Game Loop

Right now your kingdom is a photograph. You build it, you print it, the program ends and nothing has changed. Today the kingdom learns to *move*. Each day, every farm makes a bit of food, every mine makes a bit of stone, citizens eat. Numbers go up; numbers go down. By the end of this module the program runs five days and shows you what happened.

We need one new word for the day. **Tick.** A tick is one step of game time. In a fast game like a shooter, a tick might happen sixty times a second. In a slow game like ours — a kingdom you run for many days — a tick is once per *day*. The exact length doesn't matter; what matters is that the engine has a method that means *"move time forward by one step."* That method will be called `AdvanceDay`. Once it exists, anything that wants to push time forward — the console today, a button on a webpage in a few months, a Roblox game later in the year — calls the same method. Same engine, many shells.

> **Words to watch**
>
> - **tick** — one step of game time. In our kingdom, one tick is one day.
> - **game loop** — the loop that calls `AdvanceDay` over and over and shows the result each time.
> - **side effect** — when a method changes something instead of (or in addition to) returning a value.
> - **subclass** — a more specific kind of class built on top of another (we meet these properly in Module 1.5).

---

## What you're building

Three small changes to the engine, plus a console loop and tests:

| File | Change |
| --- | --- |
| `Kingdom.Engine/Building.cs` | Gains a `Tick(ResourceLedger)` method. Empty for now. |
| `Kingdom.Engine/Kingdom.cs` | Gains a `Day` property and an `AdvanceDay()` method. |
| `Kingdom.Console/Program.cs` | Runs the kingdom for five days and prints each day. |
| `tests/Kingdom.Engine.Tests/KingdomTickTests.cs` | New test file. |

The starter for this module ships only those four files. Drop them on top of your Module 1.3 working folder.

## Step 1 — give `Building` a `Tick` method

Open `Building.cs`. Right now it just holds a `Name` and a `Level`. Today we add a method that says *"a tick of game time happened — do whatever you do."*

```csharp
namespace Kingdom.Engine;

public class Building
{
    public string Name { get; }
    public int Level { get; private set; } = 1;

    public Building(string name) { Name = name; }

    public void Upgrade() => Level++;

    // New today. The default does nothing.
    // Specific kinds of building (Farm, Mine, ...) will fill this in tomorrow.
    public virtual void Tick(ResourceLedger ledger) { }
}
```

The new keyword is **`virtual`**. It marks a method as *replaceable* — tomorrow's `Farm` and `Mine` (the *subclasses*) will write their own version of `Tick` and the engine will use those instead of this empty default. Today the default is empty on purpose: the rest of the system can run end-to-end before the specific kinds exist. We add the *shape* now and the *behaviour* tomorrow.

## Step 2 — give `Kingdom` a `Day` and an `AdvanceDay`

Open `Kingdom.cs`. Two things go in:

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
        // 1. Every building ticks.
        foreach (var b in Buildings)
            b.Tick(Resources);

        // 2. Every citizen eats one food. If there's none left, Spend
        //    just returns false and the day continues — nobody starves yet.
        foreach (var _ in Citizens)
            Resources.Spend(Resource.Food, 1);

        // 3. Time moves forward.
        Day++;
    }
}
```

Three things happen in a tick: buildings produce, citizens eat, the day counter ticks up. **The order matters.** Buildings have to produce *before* citizens eat — otherwise on Day 1 you'd be eating food that the farms haven't grown yet. It's a small thing on Day 1; it's a confusing bug on Day 50. Get the order right now.

`AdvanceDay()` returns `void` — it doesn't *give back* a value. Instead, it changes the ledger, the day counter, and (soon) building state. Doing-things-to-state is what we call a **side effect**. Engines lean on side effects because the alternative — returning a brand-new copy of the kingdom every tick — would mean copying everything every time. The trade-off is honest: more powerful, slightly harder to reason about. The wins.md tradition is to write down the rules you're building on; this is one of them.

## Step 3 — run it from the console

Open `Program.cs`. The console shell now does two things: build a kingdom, then push five days through it.

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

Build and run it:

```powershell
dotnet build
dotnet run --project Kingdom.Console
```

You should see Day 1 through Day 6 printed, and food going down by 2 each day — two citizens, one bite each. The other resources don't move yet. That's tomorrow's job; today's job was to make the world tick at all.

## Step 4 — write the tests

> **A small C# corner you'll meet here.** The test file lives in the namespace `Kingdom.Engine.Tests`. Inside that namespace, if you write the unqualified word `Kingdom`, the C# compiler isn't sure whether you mean the outer `Kingdom` *namespace* or the `Kingdom` *class* — so it picks the namespace, and the test won't compile. The fix is to write `global::Kingdom.Engine.Kingdom` — the `global::` prefix tells the compiler *"start at the very top of the namespace tree, then come down."* You'll see this once or twice in the next few modules; that's it.

`tests/Kingdom.Engine.Tests/KingdomTickTests.cs`:

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
        // Drain food to zero first.
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

```powershell
dotnet test
```

You should see `Passed: 16` — eleven from Module 1.3 plus five new ones.

## Tinker

Try changing the loop length from 5 to 100 days. Will food go negative? It shouldn't — `Spend` refuses to take more than the ledger has and returns `false`. Check by reading `Get(Resource.Food)` after the loop and seeing whether it sits at zero or below.

Add a third citizen. Food now drops by 3 per day instead of 2.

Call `AdvanceDay()` a thousand times in a row. How long does it take? If your engine is honest, it's near-instant — engines should be fast.

Move the `Day++` line to the *top* of `AdvanceDay` instead of the bottom. The program still runs but the meaning shifts: the very first call now ticks buildings on "Day 2." Subtle, easy to miss, exactly the kind of bug that ships and then confuses you a week later.

## The rule of the through-line

The *through-line* of this course is a single rule we keep coming back to: the engine never decides *when* it ticks — that's the shell's job. Today the console runs five ticks in a `for` loop. Later this year the same engine will tick once per click in a browser, once per heartbeat in Roblox, or whenever a player calls a web endpoint. The engine just exposes `AdvanceDay()` and waits to be called.

## What you just did

The kingdom went from a one-shot photograph to a thing that *moves*. You added a method on `Building` called `Tick` — empty for now, but ready for tomorrow's farms and mines to fill in. You added `Day` and `AdvanceDay` to `Kingdom`, and the console pushed the world through five days in a `for` loop. Along the way you met two ideas worth keeping: a **side effect** (a method that changes state instead of returning a value), and **`virtual`** (a method that more specific classes are allowed to replace). You also met the through-line of the course in concrete form: the engine knows *how* to tick; the shell decides *when*. Five new tests passed; sixteen total now.

**Key concepts you can now name:**

- a *tick* and a *game loop* — the engine's heartbeat
- *side effects* — methods that change state instead of returning values
- *`virtual`* — a method more specific classes are allowed to replace
- the through-line — engine knows *how*; shell decides *when*

## Words to add to the glossary

- **tick** — one step of game time.
- **game loop** — a loop that calls the engine's tick method and shows the result.
- **side effect** — a method that changes something rather than (or as well as) returning a value.
- **`virtual`** — a method marked as replaceable by a more specific class.

## Quiz

Open `quiz.md`. When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same shape as the entries that came before. Bring whichever you're least sure about to the next weekly sync.

## Next

Module 1.5 introduces those *more specific kinds* — `Farm`, `Lumberyard`, `Mine`. Each will fill in `Tick` so it produces a different resource. After tomorrow, all four resources move every day, not just food.

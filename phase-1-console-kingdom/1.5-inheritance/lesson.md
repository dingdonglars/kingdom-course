# Module 1.5 — Inheritance: Specialised Buildings

Yesterday `Tick()` did nothing. Today we make three specific kinds of building — `Farm` produces food, `Lumberyard` produces wood, `Mine` produces stone. Each one *is* a building (same name, same level, same upgrade), but each one ticks differently. The OOP feature that lets you say *"a farm is a kind of building"* is called **inheritance**, and that's today's lesson.

The other choice would be three separate classes — `Farm`, `Lumberyard`, `Mine` — each with its own `Name`, `Level`, `Upgrade`, and `Tick`. Then every change to one would have to be made three times. Inheritance lets you write `Building` once, then say *"Farm is a Building, and here's the small part that's special to farms."* Farm gets everything else for free.

It's a bit like animals. A dog is a kind of animal. It already has everything an animal has — it breathes, it eats, it moves. You only have to describe the part that makes a dog a dog: it barks. You don't re-describe breathing.

Drawn out, today's three buildings look like this:

```text
                     Building                  the parent
                     Name, Level, Upgrade()
                  /      |       \
                 /       |        \
             Farm    Lumberyard    Mine        the children: each one IS
              |          |          |          a Building (it gets all of
            food       wood       stone        the above for free)...
                                               ...and only adds its own Tick()
```

Each child writes *one* small thing — what it produces each tick — and gets `Name`, `Level`, and `Upgrade()` from the parent without copying them.

> **Words to watch**
>
> - **inheritance** — when one class (the *child*) gets all the fields and methods of another (the *parent*) and can add or replace its own
> - **subclass** (or *child class*) — `Farm`, `Lumberyard`, `Mine`
> - **base class** (or *parent class*) — `Building`
> - **`override`** — the keyword that says *"I'm replacing the parent's `virtual` method"*
> - **`base(...)`** — calls the parent's constructor from inside the child's

---

## Why inheritance, and when to be careful

Inheritance is one of the older ideas in object-oriented programming, and it's easy to use too much. A common piece of modern advice is *"prefer composition over inheritance."* That means: when one thing wants to use another, it's often cleaner for it to *contain* the other instead of *inheriting* from it. Long inheritance chains (five or six levels deep) get hard to change. A change near the top reaches down into every class below it, and that's easy to break.

For one level deep, though — `Building` → `Farm`, `Lumberyard`, `Mine` — inheritance is the right tool. Three classes, three things they really do share, and no long chain. We'll come back to composition later, when there's a clear reason for it.

## Step 1 — three subclasses

This module's `starter/` adds three new files and updates `Program.cs`:

- **NEW:** `Kingdom.Engine/Farm.cs`
- **NEW:** `Kingdom.Engine/Lumberyard.cs`
- **NEW:** `Kingdom.Engine/Mine.cs`
- **MODIFIED:** `Kingdom.Console/Program.cs` (uses the new subclasses)
- **NEW:** `tests/Kingdom.Engine.Tests/SubclassTests.cs`

`Building.cs`, `Kingdom.cs`, and the existing tests are unchanged from 1.4.

`Farm.cs`:

```csharp
namespace Kingdom.Engine;

public class Farm : Building
{
    public Farm(string name) : base(name) { }

    public override void Tick(ResourceLedger ledger)
    {
        // Each level produces 5 food per day
        ledger.Add(Resource.Food, 5 * Level);
    }
}
```

Three things to read carefully. The `: Building` after `class Farm` means *"inherit from Building."* `Farm` now has `Name`, `Level`, `Upgrade`, and `Tick` for free. The `: base(name)` in the constructor passes the `name` parameter up to `Building`'s constructor — the parent class still does the work of setting `Name`, and the child only adds whatever is new (which here is nothing). And `override void Tick(...)` replaces the empty default from the parent. The compiler makes you write `override` when you replace a `virtual` method, so you can never replace one by accident.

`Lumberyard.cs`:

```csharp
namespace Kingdom.Engine;

public class Lumberyard : Building
{
    public Lumberyard(string name) : base(name) { }

    public override void Tick(ResourceLedger ledger)
    {
        ledger.Add(Resource.Wood, 3 * Level);
    }
}
```

`Mine.cs`:

```csharp
namespace Kingdom.Engine;

public class Mine : Building
{
    public Mine(string name) : base(name) { }

    public override void Tick(ResourceLedger ledger)
    {
        ledger.Add(Resource.Stone, 2 * Level);
    }
}
```

Three classes that look almost the same. That feels like repeating yourself, and it is, a little. The question worth asking is *"would each of these change differently over time?"* A future `Farm` might add a `Crop` enum. A future `Mine` might track an `OreVein`. Yes, they probably would. So having separate classes is worth it. If two classes really would never grow apart, put them together as one — don't make a new class for every word in the design.

## Step 2 — use the subclasses in `Program.cs`

```csharp
using Kingdom.Engine;

var kingdom = new Kingdom.Engine.Kingdom("Eldoria");
kingdom.AddBuilding(new Farm("Main Farm"));
kingdom.AddBuilding(new Lumberyard("Eastern Lumberyard"));
kingdom.AddBuilding(new Mine("Old Stone Mine"));
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
    Console.WriteLine($"Buildings ({k.Buildings.Count}):");
    foreach (var b in k.Buildings)
        Console.WriteLine($"  - {b.GetType().Name} '{b.Name}' (level {b.Level})");
    Console.WriteLine($"Citizens:  {k.Citizens.Count}");
    Console.Write("Resources: ");
    foreach (var (r, n) in k.Resources.Snapshot())
        Console.Write($"{r}={n}  ");
    Console.WriteLine();
}
```

Notice `b.GetType().Name`. **`GetType()`** is a method every object has — it gives back the type the object really is while the program runs. `.Name` gives the type's short name as a string (`"Farm"`, `"Lumberyard"`, `"Mine"`). That's how the same loop can show different kinds of building without us writing a `switch` statement. The list holds `Building` references, but each item *runs* its own `Tick`, because each item is really a `Farm` or a `Lumberyard` or a `Mine`. The name for this is **polymorphism** — they all look like the same type on the outside, but each one behaves differently on the inside.

Build and run:

```powershell
dotnet build
dotnet run --project Kingdom.Console
```

You should see Day 1 through Day 6, with food going up (5 from the farm, minus 2 for the citizens, so +3 a day), wood +3 a day, stone +2 a day. The resources really change now.

## Step 3 — test the subclasses

`tests/Kingdom.Engine.Tests/SubclassTests.cs`:

```csharp
using Kingdom.Engine;
using Shouldly;

namespace Kingdom.Engine.Tests;

public class SubclassTests
{
    [Fact]
    public void Farm_Tick_AddsFoodEqualToFiveTimesLevel()
    {
        var ledger = new ResourceLedger();
        var farm = new Farm("F");
        farm.Tick(ledger);
        ledger.Get(Resource.Food).ShouldBe(5);
    }

    [Fact]
    public void Lumberyard_Tick_AddsWoodEqualToThreeTimesLevel()
    {
        var ledger = new ResourceLedger();
        var ly = new Lumberyard("L");
        ly.Tick(ledger);
        ledger.Get(Resource.Wood).ShouldBe(3);
    }

    [Fact]
    public void Mine_Tick_AddsStoneEqualToTwoTimesLevel()
    {
        var ledger = new ResourceLedger();
        var m = new Mine("M");
        m.Tick(ledger);
        ledger.Get(Resource.Stone).ShouldBe(2);
    }

    [Fact]
    public void Farm_Upgraded_ProducesMore()
    {
        var ledger = new ResourceLedger();
        var farm = new Farm("F");
        farm.Upgrade();   // level 2 now
        farm.Tick(ledger);
        ledger.Get(Resource.Food).ShouldBe(10);
    }

    [Fact]
    public void Subclass_InheritsName()
    {
        var farm = new Farm("Main Farm");
        farm.Name.ShouldBe("Main Farm");
        farm.Level.ShouldBe(1);
    }

    [Fact]
    public void Kingdom_AdvanceDay_RunsAllSubclassTicks()
    {
        var k = new global::Kingdom.Engine.Kingdom("Test");
        k.AddBuilding(new Farm("F"));
        k.AddBuilding(new Lumberyard("L"));
        k.AddBuilding(new Mine("M"));

        var foodBefore = k.Resources.Get(Resource.Food);
        var woodBefore = k.Resources.Get(Resource.Wood);
        var stoneBefore = k.Resources.Get(Resource.Stone);

        k.AdvanceDay();

        k.Resources.Get(Resource.Food).ShouldBe(foodBefore + 5);  // no citizens this test
        k.Resources.Get(Resource.Wood).ShouldBe(woodBefore + 3);
        k.Resources.Get(Resource.Stone).ShouldBe(stoneBefore + 2);
    }
}
```

Run:

```powershell
dotnet test
```

You should see `Passed: 22` — sixteen from earlier modules, six new ones.

## Tinker

Add a `Quarry` (marble, for fancy buildings later). Copy `Mine.cs`, change the class name and the resource it produces. Add a test for it. About five minutes of work.

Try making `Building` `abstract` (`public abstract class Building`). Now the compiler will refuse `new Building("Generic")` anywhere — an abstract class can only be inherited from, not created on its own. Does that break Module 1.3's tests? It does — `BuildingTests` uses `new Building("Farm")`. Take it back out. `abstract` is a step we *could* take next, and we'll come to it when we need it.

Change the constructor to call itself: write `Building(string name) : this("default") { }`. Run the program. You'll get a stack overflow error — the constructor keeps calling itself with no end. Worth seeing once, so you remember it.

Add a method to `Building` called `Describe()` that returns `$"{GetType().Name} {Name} (level {Level})"`. Call it from `Program.cs` instead of building the string by hand. Same output, less typing, and now the engine owns the format string.

## What you just did

Three subclasses build on `Building` — `Farm`, `Lumberyard`, `Mine` — and each one replaces the empty `Tick` with its own production rule. The same `foreach` loop in `Kingdom.AdvanceDay` ticks all three correctly, because each item in the list *runs as the type it really is*, not as the type the list says it holds. That's polymorphism, in one paragraph. You also met `: base(...)` for calling a parent's constructor, and you saw the compiler require `override` so you can't replace a `virtual` method by accident. Six new tests passed; twenty-two total now.

**Key concepts you can now name:**

- **inheritance** — child class gets parent's methods and fields
- **`override`** — clearly replacing a `virtual` method
- **`: base(...)`** — calls the parent's constructor from the child's
- **`GetType().Name`** — the real type's name while the program runs
- **polymorphism** — same type on the outside, different behaviour per subclass

## On your own

Time to put the book away. Don't scroll back up to the steps — prove to yourself, from your own head, that the one big idea stuck: making a subclass. No one marks this one — it's just for you. It's the easiest way to spot what *hasn't* stuck yet, while it's still simple to fix. Getting stuck here is completely fine — that's exactly what it's for.

Write a new building from memory: a `Quarry` that produces marble. (First add `Marble` to your `Resource` enum, or produce `Stone` if you'd rather not touch the enum.)

1. `Quarry` inherits from `Building`.
2. Pass the name up to the parent with `: base(name)`.
3. `override` the `Tick` method so it adds `4 * Level` of its resource to the ledger.
4. Add it to the kingdom in `Program.cs` and run.

<details><summary>Stuck? Open this to check yourself.</summary>

```csharp
public class Quarry : Building
{
    public Quarry(string name) : base(name) { }

    public override void Tick(ResourceLedger ledger)
    {
        ledger.Add(Resource.Stone, 4 * Level);
    }
}
```

Three things had to be right: `: Building` to inherit, `: base(name)` to pass the name to the parent's constructor, and `override` on `Tick`. If you forget `override`, the compiler complains — and that's on purpose, so you can never replace a `virtual` method by accident.

</details>

## Wrap up

1. **Quiz** — open `quiz.md`, jot your answers in `journal/quiz-notes.md`.
2. **Progress** — one line in `journal/progress.md`: `Module 1.5 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit and push** — stage both files, commit message `Module 1.5 done`, Sync.
4. **Post in `#wins`** — one line about today, plus the URL of the commit.

Module 0.1 covers the why and the panel/CLI steps if you need a refresher. Bring quiz answers you're least sure about to the next weekly sync.

## Next

Module 1.6 introduces **LINQ** — the query methods (`Where`, `Select`, `Sum`, `OrderBy`) that turn manual `for` loops into one-line questions about the kingdom's lists. You'll use them every day for the rest of the course.

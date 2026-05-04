# Module 1.5 — Inheritance: Specialised Buildings

Yesterday `Tick()` did nothing. Today we make three specific kinds of building — `Farm` produces food, `Lumberyard` produces wood, `Mine` produces stone. Each one *is* a building (same name, same level, same upgrade) but each ticks differently. The OOP feature that lets you say *"a farm is a kind of building"* is called **inheritance**, and that's the lesson today.

The alternative would be three separate classes — `Farm`, `Lumberyard`, `Mine` — each with its own `Name`, `Level`, `Upgrade`, and `Tick`. Every change to one would have to be made three times. Inheritance lets you write `Building` once, then say *"Farm is a Building, and here's the bit that's specific to farms."* Farm gets all the rest for free.

> **Words to watch**
>
> - **inheritance** — when one class (the *child*) gets all the fields and methods of another (the *parent*) and can add or replace its own
> - **subclass** (or *child class*) — `Farm`, `Lumberyard`, `Mine`
> - **base class** (or *parent class*) — `Building`
> - **`override`** — the keyword that says *"I'm replacing the parent's `virtual` method"*
> - **`base(...)`** — calls the parent's constructor from inside the child's

---

## Why inheritance, and where it gets dangerous

Inheritance is one of the older ideas in object-oriented programming, and it's easy to overuse. A common modern guideline is *"prefer composition over inheritance"* — meaning, when one thing wants to use another, it's often cleaner for it to *contain* the other rather than *inherit* from it. Long inheritance chains (five or six levels deep) become rigid: a change near the top ripples through every descendant.

For one level deep, though — `Building` → `Farm`, `Lumberyard`, `Mine` — inheritance is the right tool. Three classes, three things they really do share, no chain. We'll meet composition properly when it earns its place.

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

Three things to read carefully. The `: Building` after `class Farm` means *"inherit from Building."* `Farm` now has `Name`, `Level`, `Upgrade`, and `Tick` for free. The `: base(name)` in the constructor passes the `name` parameter up to `Building`'s constructor — the parent class still does the work of setting `Name`; the child only adds anything new (which here is nothing). And `override void Tick(...)` replaces the empty default from the parent. The compiler insists you write `override` when you replace a `virtual` method, so you can never replace one by accident.

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

Three almost-identical classes. That feels like duplication, and it is, a little. The question worth asking is *"would each of these grow differently over time?"* A future `Farm` might add a `Crop` enum; a future `Mine` might track an `OreVein`. Yes, they probably would. So separate classes are worth it. When two classes really would never split apart, fold them into one — don't make a class for every word in the design doc.

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

Notice `b.GetType().Name`. **`GetType()`** is a method every object has — it returns the type the object actually is at runtime. `.Name` gives the type's short name as a string (`"Farm"`, `"Lumberyard"`, `"Mine"`). That's how the same loop can show different building kinds without us writing a `switch` statement. The list holds `Building` references, but each item *runs* its own `Tick`, because each item is really a `Farm` or `Lumberyard` or `Mine`. The technical name for that is **polymorphism** — same type at the surface, different behaviour at the implementation.

Build and run:

```powershell
dotnet build
dotnet run --project Kingdom.Console
```

You should see Day 1 through Day 6, with food climbing (5 from the farm, minus 2 for the citizens equals net +3 a day), wood +3 a day, stone +2 a day. The resources actually move now.

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

Add a `Quarry` (think marble for fancy buildings later). Copy `Mine.cs`, change the class name and the resource produced. Add a test for it. About five minutes of work.

Try making `Building` `abstract` (`public abstract class Building`). The compiler will refuse `new Building("Generic")` anywhere — abstract classes can only be inherited from, not instantiated directly. Does that break Module 1.3's tests? It does — `BuildingTests` uses `new Building("Farm")`. Take it back out. `abstract` is the next step we *could* take, and we'll meet it when we need it.

Reverse the constructor: write `Building(string name) : this("default") { }`. Run the program. You'll get a stack overflow — the constructor is calling itself recursively. Worth seeing once, never to forget.

Add a method to `Building` called `Describe()` that returns `$"{GetType().Name} {Name} (level {Level})"`. Call it from `Program.cs` instead of building the string by hand. Same output, less typing, and the engine owns the format string.

## What you just did

Three subclasses extend `Building` — `Farm`, `Lumberyard`, `Mine` — and each replaces the empty `Tick` with its own production rule. The same `foreach` loop in `Kingdom.AdvanceDay` ticks all three correctly, because each list item *runs as the type it really is*, not as the type the list says it holds. That's polymorphism in one paragraph. You also met `: base(...)` for chaining to a parent constructor, and you saw the compiler insist on `override` so you can't replace a `virtual` method by accident. Six new tests passed; twenty-two total now.

**Key concepts you can now name:**

- **inheritance** — child class gets parent's methods and fields
- **`override`** — explicit replacement of a `virtual` method
- **`: base(...)`** — calls parent's constructor from child's
- **`GetType().Name`** — runtime type name, useful for logging
- **polymorphism** — same surface type, different behaviour per subclass

## Quiz

Open `quiz.md`. When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.

## Next

Module 1.6 introduces **LINQ** — the query methods (`Where`, `Select`, `Sum`, `OrderBy`) that turn manual `for` loops into one-line questions about the kingdom's lists. You'll use them every day for the rest of the course.

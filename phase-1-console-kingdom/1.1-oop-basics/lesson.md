# Module 1.1 — OOP Basics

Today the Kingdom begins. You're going to make four classes — `Building`, `Resource`, `Citizen`, `Kingdom` — and from those four, build a tiny medieval realm that exists in your computer's memory while the program runs. By the end of the lesson you'll print Eldoria to the terminal, with two buildings, a citizen, and a treasury. None of it does anything yet. That's fine. The first thing the kingdom needs is a body; we'll teach it to move in Module 1.4.

The big idea this lesson is **classes**. A class is a blueprint — it describes what a thing *is* and what it *can do*. `new Building("Main Farm")` then makes one specific thing from that blueprint, called an *object* (or an *instance*). One class, many objects. One `Building` blueprint, many farms and mines and lumberyards built from it.

> **Words to watch**
>
> - **class** — a blueprint for creating objects
> - **object** (also *instance*) — a thing created from a class with `new`
> - **property** — a named value on an object, with a `get` and an optional `set`
> - **constructor** — the special method that runs when an object is created (`new Building(...)`)
> - **encapsulation** — a class hiding its internals behind methods and properties so the outside can't poke at them
> - **enum** — a named set of allowed values (e.g. `Resource.Gold`, `Resource.Wood`)
> - **`new`** — the C# keyword that calls a constructor and gives you back a fresh object

---

## Step 1 — start a fresh project

Make a new project for the Kingdom:

```powershell
cd <your-repo-root>
dotnet new console -n KingdomConsole
cd KingdomConsole
```

You'll create five files in here. Each holds one class (or one enum). The convention in C# is one type per file, and the filename matches the type name. We'll keep it.

## Step 2 — `Resource.cs`, the resource enum

```csharp
namespace KingdomConsole;

public enum Resource
{
    Gold,
    Wood,
    Stone,
    Food
}
```

An *enum* is a fixed set of named values. Anywhere you use `Resource`, the only valid options are `Resource.Gold`, `Resource.Wood`, `Resource.Stone`, `Resource.Food`. The compiler refuses anything else. That's the win — you can't accidentally pass `42` or `"goldd"` to something expecting a resource.

## Step 3 — `Building.cs`

```csharp
namespace KingdomConsole;

public class Building
{
    public string Name { get; }
    public int Level { get; private set; } = 1;

    public Building(string name)
    {
        Name = name;
    }

    public void Upgrade()
    {
        Level++;
    }
}
```

Three things to read carefully here. `Name` is a property — a named value on the building — and it has only a `get`, no `set`. That means you can read `b.Name` from outside, but you can never write `b.Name = "something else"`. Once a building is built, its name doesn't change. `Level` has a `get` and a `private set` — anyone can read it, but only code inside the `Building` class can write it. The default is `1`. To raise the level, you call `Upgrade()`. That's encapsulation: the outside world doesn't reach in and set numbers; it asks the class to do something, and the class decides what changes.

The line `public Building(string name)` is the **constructor**. Same name as the class. When you write `new Building("Main Farm")`, that constructor runs once, and its job is to set things up — here, copy the `name` parameter into the `Name` property.

## Step 4 — `Citizen.cs`

```csharp
namespace KingdomConsole;

public class Citizen
{
    public string Name { get; }
    public string Job { get; set; } = "Idle";

    public Citizen(string name)
    {
        Name = name;
    }
}
```

Same pattern — read-only `Name`, read-write `Job` defaulting to `"Idle"`. The job is something the kingdom changes over time; the name isn't.

## Step 5 — `Kingdom.cs`

```csharp
namespace KingdomConsole;

public class Kingdom
{
    public string Name { get; }
    public List<Building> Buildings { get; } = new();
    public List<Citizen> Citizens { get; } = new();
    public Dictionary<Resource, int> Resources { get; } = new();

    public Kingdom(string name)
    {
        Name = name;
        Resources[Resource.Gold] = 100;
        Resources[Resource.Wood] = 50;
        Resources[Resource.Stone] = 20;
        Resources[Resource.Food] = 30;
    }

    public void AddBuilding(Building b) => Buildings.Add(b);
    public void AddCitizen(Citizen c) => Citizens.Add(c);
}
```

A `Kingdom` owns three collections — its buildings, its citizens, and its resources — plus a name. The constructor seeds the treasury with 100 gold, 50 wood, 20 stone, 30 food. The two short methods at the bottom let outside code add buildings and citizens. They're written with C#'s **expression-bodied** syntax — `=>` instead of `{ ... }` for one-line methods. Same meaning, less typing.

## Step 6 — `Program.cs`

```csharp
using KingdomConsole;

var kingdom = new Kingdom("Eldoria");
kingdom.AddBuilding(new Building("Main Farm"));
kingdom.AddBuilding(new Building("Old Mine"));
kingdom.AddCitizen(new Citizen("Lyra"));

Console.WriteLine($"== {kingdom.Name} ==");
Console.WriteLine($"Buildings ({kingdom.Buildings.Count}):");
foreach (var b in kingdom.Buildings)
    Console.WriteLine($"  - {b.Name} (level {b.Level})");

Console.WriteLine($"Citizens ({kingdom.Citizens.Count}):");
foreach (var c in kingdom.Citizens)
    Console.WriteLine($"  - {c.Name}: {c.Job}");

Console.WriteLine("Resources:");
foreach (var (resource, count) in kingdom.Resources)
    Console.WriteLine($"  {resource}: {count}");
```

Run it:

```powershell
dotnet run
```

You should see Eldoria printed — two buildings, one citizen, four resources. Entirely in your computer's memory, gone the moment the program ends. Persistence comes in Phase 2.

## Tinker

Add a third building to `Program.cs` and run again. Then call `kingdom.Buildings[0].Upgrade()` before the print loop — the first building's level should now show as 2.

Try writing `kingdom.Buildings[0].Name = "New Name";`. The compiler will refuse — `Name` has no setter. Good. That's encapsulation working: a kingdom can't quietly rename one of its own buildings by accident.

Add a method on `Kingdom` called `HireCitizen` that takes a name, creates a `Citizen`, and adds it to the list. Use it from `Program.cs` instead of the long form. The kingdom does the work; the program just asks for it.

## What you just did

You wrote four classes and saw them connect — a `Kingdom` that owns lists of `Building` and `Citizen` and a dictionary of `Resource`. You met the parts of a class that you'll use every day from now on: properties with `get` and `private set`, a constructor that sets things up, the difference between a class (the blueprint) and an object (the thing you got from `new`). You also saw encapsulation in action when the compiler refused to let outside code rewrite a building's name. Five files of real C#, and a kingdom prints to the terminal — all of it living in memory for the eight seconds the program runs.

**Key concepts you can now name:**

- **class vs object** — blueprint versus the thing built from it
- **property** — named value with `get` and optional `set`
- **constructor** — the special method that runs at `new`
- **encapsulation** — class controls who can change what
- **enum** — fixed set of named values, compiler-checked

## Quiz

Open `quiz.md`. When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.

## Next

Right now everything's in one project. That's fine for one lesson. Module 1.2 splits it: the kingdom's rules (Building, Citizen, Resource, Kingdom) move into their own *class library*, and the program becomes a thin layer on top of it. That refactor is the lesson the rest of the course is named after.

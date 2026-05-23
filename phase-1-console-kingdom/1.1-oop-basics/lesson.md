# Module 1.1 — OOP Basics

Today the Kingdom begins. You're going to make four classes — `Building`, `Resource`, `Citizen`, `Kingdom`. From those four, you'll build a tiny medieval kingdom. It exists in your computer's memory while the program runs. By the end of the lesson you'll print Eldoria to the terminal, with two buildings, a citizen, and a treasury. None of it does anything yet. That's fine. First the kingdom needs a body. We'll teach it to move in Module 1.4.

The big idea this lesson is **classes**. A class is a blueprint. It describes what a thing *is* and what it *can do*. `new Building("Main Farm")` then makes one specific thing from that blueprint. That thing is called an *object* (or an *instance*). One class, many objects. One `Building` blueprint, and many farms and mines and lumberyards built from it.

Think of a class like the blueprint for a house. The blueprint is one piece of paper. From it you can build many real houses. Each house is a separate thing, but they all came from the same blueprint.

> **Words to watch**
>
> - **class** — a blueprint for creating objects
> - **object** (also *instance*) — a thing created from a class with `new`
> - **property** — a named value on an object, with a `get` and an optional `set`
> - **constructor** — the special method that runs when an object is created (`new Building(...)`)
> - **encapsulation** — a class hiding its inner values behind methods and properties, so code outside the class can't change them directly
> - **enum** — a named set of allowed values (e.g. `Resource.Gold`, `Resource.Wood`)
> - **`new`** — the C# keyword that calls a constructor and gives you back a fresh object

---

## Phase opener — make a branch for Phase 1's work

Run this first, before any code:

```powershell
cd C:\code\kingdom
git switch -c phase-1
```

You're now on a *branch* called `phase-1`. A branch is a separate line of work in git. From now until the end of Phase 1, your commits go onto `phase-1` instead of `main`. At Module 1.10 (the end of Phase 1), you'll bring all that work back into `main` through a **pull request**. A pull request is the way GitHub lets someone review your work before it joins `main`. Lars reviews it, approves it, and you merge it. The reason: `main` stays the line of *good, reviewed work*, and the pull request at the end of the phase shows your whole phase as one piece to review. This is how real teams add changes.

**If "branch" and "pull request" feel unclear right now, that's expected.** It's the first time you've seen them. What you actually have to do is small. Run one command at the start of each phase (`git switch -c phase-N`), and open a pull request on github.com at the end. That's it for today. You'll understand what branches and pull requests really are over the next ten modules. Module 1.8 comes back to branches once you've used one for a few weeks. Module 1.10 walks through the pull request step by step. Bonus B3 (much later) goes deep into how git works inside, if you ever want it.

For now: run the command, confirm you're on the new branch, and move on. After a few weeks of using branches, the idea stops feeling fuzzy.

Confirm:

```powershell
git status
```

The first line should say *"On branch phase-1"*. Now the actual lesson.

---

## Step 1 — start a fresh project

Make a new project for the Kingdom:

```powershell
cd <your-repo-root>
dotnet new console -n KingdomConsole
cd KingdomConsole
```

You'll create five files in here. Each one holds one class (or one enum). The usual rule in C# is one type per file, and the filename matches the type name. We'll follow that rule.

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

An *enum* is a fixed set of named values. Anywhere you use `Resource`, the only allowed options are `Resource.Gold`, `Resource.Wood`, `Resource.Stone`, `Resource.Food`. The compiler refuses anything else. That's the good part: you can't pass `42` or `"goldd"` by accident to something that expects a resource. It's like a drop-down menu where you can only pick from the four choices given.

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

Three things to read carefully here. `Name` is a property — a named value on the building. It has only a `get`, no `set`. That means you can read `b.Name` from outside, but you can never write `b.Name = "something else"`. Once a building is made, its name doesn't change. `Level` has a `get` and a `private set`. Anyone can read it, but only code inside the `Building` class can change it. It starts at `1`. To raise the level, you call `Upgrade()`. That's encapsulation: code outside the class doesn't set the numbers itself. It asks the class to do something, and the class decides what changes.

The line `public Building(string name)` is the **constructor**. It has the same name as the class. When you write `new Building("Main Farm")`, that constructor runs once. Its job is to set things up. Here, it copies the `name` parameter into the `Name` property.

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

Same pattern — `Name` is read-only, `Job` can be read and written and starts at `"Idle"`. The kingdom changes the job over time. The name stays the same.

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

A `Kingdom` owns three collections — its buildings, its citizens, and its resources — plus a name. The constructor fills the treasury to start with: 100 gold, 50 wood, 20 stone, 30 food. The two short methods at the bottom let outside code add buildings and citizens. They use C#'s **expression-bodied** form — `=>` instead of `{ ... }` for one-line methods. Same meaning, less typing.

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

You should see Eldoria printed — two buildings, one citizen, four resources. It all lives in your computer's memory, and it's gone the moment the program ends. Saving it to disk comes in Phase 2.

## Tinker

Add a third building to `Program.cs` and run again. Then call `kingdom.Buildings[0].Upgrade()` before the print loop — the first building's level should now show as 2.

Try writing `kingdom.Buildings[0].Name = "New Name";`. The compiler will refuse — `Name` has no setter. Good. That's encapsulation working: a kingdom can't rename one of its own buildings by accident.

Add a method on `Kingdom` called `HireCitizen` that takes a name, creates a `Citizen`, and adds it to the list. Use it from `Program.cs` instead of the long version. The kingdom does the work; the program just asks for it.

## What you just did

You wrote four classes and saw them connect — a `Kingdom` that owns lists of `Building` and `Citizen` and a dictionary of `Resource`. You met the pieces of a class that you'll use every day from now on: properties with `get` and `private set`, a constructor that sets things up, and the difference between a class (the blueprint) and an object (the thing you got from `new`). You also saw encapsulation at work when the compiler refused to let outside code rewrite a building's name. Five files of real C#, and a kingdom prints to the terminal — all of it living in memory for the few seconds the program runs.

**Key concepts you can now name:**

- **class vs object** — blueprint versus the thing built from it
- **property** — named value with `get` and optional `set`
- **constructor** — the special method that runs at `new`
- **encapsulation** — class controls who can change what
- **enum** — fixed set of named values, compiler-checked

## Wrap up

1. **Quiz** — open `quiz.md`, jot your answers in `journal/quiz-notes.md`.
2. **Progress** — one line in `journal/progress.md`: `Module 1.1 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit and push** — stage both files, commit message `Module 1.1 done`, Sync.
4. **Post in `#wins`** — one line about today, plus the URL of the commit.

Module 0.1 covers the why and the panel/CLI steps if you need a refresher. Bring quiz answers you're least sure about to the next weekly sync.

## Next

Right now everything is in one project. That's fine for one lesson. Module 1.2 splits it apart: the kingdom's rules (Building, Citizen, Resource, Kingdom) move into their own *class library*, and the program becomes a thin layer on top. That change is what the rest of the course is named after.

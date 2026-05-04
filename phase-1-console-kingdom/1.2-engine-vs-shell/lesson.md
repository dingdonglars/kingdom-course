# Module 1.2 — Engine vs Shell

This is the lesson the rest of the course is named after. You take the kingdom you wrote in 1.1 — all in one project — and split it into two. The kingdom's *rules* (buildings, resources, the math) move into a class library called `Kingdom.Engine`. The program that prints things to the terminal becomes a small `Kingdom.Console` project that *uses* the engine. Same code in the end; completely different layout. The point of today is to feel why that layout matters before any of the later phases ask you to live with it.

We're using a piece of vocabulary you'll meet a lot from here on: **shell**. A shell is whatever talks to the outside world — the console here, a web page later in the year, Roblox after that. The engine never talks to the outside. The shell does. The engine just knows about the kingdom; the shell knows about humans.

> **Words to watch**
>
> - **class library** — a project that compiles to a `.dll`, not an `.exe`. No `Main`. Other projects use it.
> - **engine** — the part of the code that's about the kingdom and its rules
> - **shell** — the part that talks to the outside world (console, files, network, browser, Roblox)
> - **project reference** — one project saying *"I depend on this other project"*
> - **solution** (`.slnx` or `.sln`) — a file that groups related projects so they build together

---

## Why split

In Module 1.1 your `Building`, `Resource`, `Kingdom`, and `Citizen` classes lived in the same project as `Program.cs`. That works for now. But ask yourself: if you wanted the same kingdom on a website later in the year, what would you do? You can't reuse `Program.cs` — websites don't have a console to print to. You'd be copying the kingdom classes out of one project and into another. The split today prevents that copy. The engine becomes the kingdom's logic; the console becomes one way of *playing* it. Phase 4's browser version will be a different way of playing the same logic. Phase 5's Roblox port will translate the same logic into Luau. The engine is the bet.

## Step 1 — create the new layout

You have a `KingdomConsole` project from 1.1. We'll restructure into two:

```
your-repo/
├─ Kingdom.Engine/                  ← class library (no Main, no Console)
│   ├─ Kingdom.cs
│   ├─ Building.cs
│   ├─ Citizen.cs
│   ├─ Resource.cs
│   ├─ ResourceLedger.cs            ← new — wraps the dictionary
│   └─ Kingdom.Engine.csproj
├─ Kingdom.Console/                 ← console app (no game logic)
│   ├─ Program.cs
│   └─ Kingdom.Console.csproj
└─ Kingdom.slnx                     ← ties them together
```

```powershell
cd <your-repo-root>
# Back up your 1.1 folder so you can compare later
Rename-Item KingdomConsole KingdomConsole-v1-backup

dotnet new sln -n Kingdom
dotnet new classlib -n Kingdom.Engine
dotnet new console -n Kingdom.Console
dotnet sln add Kingdom.Engine Kingdom.Console
dotnet add Kingdom.Console reference Kingdom.Engine
```

The last line is the important one. It writes a `<ProjectReference>` into `Kingdom.Console.csproj` that says *"the console project depends on the engine project."* The build system uses that line to compile the engine first, then the console with the engine's `.dll` available.

## Step 2 — move the classes into Engine

Move `Building.cs`, `Citizen.cs`, `Resource.cs`, `Kingdom.cs` from your backup folder into `Kingdom.Engine/`. Open each one and change the namespace from `KingdomConsole` to `Kingdom.Engine`. The convention is namespaces match folders — same idea you'll meet again in Module 1.9.

## Step 3 — introduce `ResourceLedger`

The dictionary on `Kingdom` is going to grow logic over the next few modules — refusing to spend more than you have, refusing negative amounts, and so on. That's class-shaped behaviour, not dictionary-shaped. Wrap it now while it's small.

`Kingdom.Engine/ResourceLedger.cs`:

```csharp
namespace Kingdom.Engine;

public class ResourceLedger
{
    private readonly Dictionary<Resource, int> _amounts = new();

    public ResourceLedger()
    {
        foreach (Resource r in Enum.GetValues<Resource>())
            _amounts[r] = 0;
    }

    public int Get(Resource r) => _amounts[r];

    public void Add(Resource r, int amount)
    {
        if (amount < 0) throw new ArgumentException("Use Spend for negative amounts.");
        _amounts[r] += amount;
    }

    public bool Spend(Resource r, int amount)
    {
        if (amount < 0) throw new ArgumentException("Spend amount must be non-negative.");
        if (_amounts[r] < amount) return false;
        _amounts[r] -= amount;
        return true;
    }

    public IReadOnlyDictionary<Resource, int> Snapshot() => _amounts;
}
```

The dictionary is `private readonly` — outside code can't reach it. The only way to change it is through `Add` and `Spend`, both of which check their inputs. `Snapshot()` returns it as `IReadOnlyDictionary` — outside code can read all four amounts in a loop, but it can't write.

Update `Kingdom.cs`:

```csharp
namespace Kingdom.Engine;

public class Kingdom
{
    public string Name { get; }
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
}
```

## Step 4 — rewrite `Program.cs`

`Kingdom.Console/Program.cs`:

```csharp
using Kingdom.Engine;

var kingdom = new Kingdom.Engine.Kingdom("Eldoria");
kingdom.AddBuilding(new Building("Main Farm"));
kingdom.AddBuilding(new Building("Old Mine"));
kingdom.AddCitizen(new Citizen("Lyra"));

PrintKingdom(kingdom);

void PrintKingdom(Kingdom.Engine.Kingdom k)
{
    Console.WriteLine($"== {k.Name} ==");
    Console.WriteLine($"Buildings ({k.Buildings.Count}):");
    foreach (var b in k.Buildings)
        Console.WriteLine($"  - {b.Name} (level {b.Level})");
    Console.WriteLine($"Citizens ({k.Citizens.Count}):");
    foreach (var c in k.Citizens)
        Console.WriteLine($"  - {c.Name}: {c.Job}");
    Console.WriteLine("Resources:");
    foreach (var (resource, count) in k.Resources.Snapshot())
        Console.WriteLine($"  {resource}: {count}");
}
```

Notice the type is `Kingdom.Engine.Kingdom` — the full name, because `Kingdom` is also the name of a namespace and C# wants to know which one you mean. (You'll meet this same situation again in M1.4 with the `global::` prefix in tests; same family of compiler-confusion.)

## Step 5 — build and run

```powershell
dotnet build
dotnet run --project Kingdom.Console
```

Same output as 1.1. But the layout is completely different.

## Tinker

Open `Kingdom.Engine/Kingdom.Engine.csproj`. There's no `<OutputType>Exe</OutputType>` line — that's why it's a library, not an executable. Try adding one. The build fails because the engine has no `Main` method. Take the line back out. The engine has nothing to *run*; it's a thing other projects use.

Try adding `Console.WriteLine("hello");` to a method on `Kingdom`. It compiles, but you've broken the rule. The engine is not allowed to print. If something inside the engine wants to communicate, it returns a value; the shell decides what to do with it.

Try changing `_amounts` in `ResourceLedger` from `private` to `public`. The compiler still likes it. Now external code could reach in and write whatever it likes. Why is that bad? Because the ledger is supposed to refuse negative amounts and refuse overspending — `_amounts.Add(...)` directly skips both checks. Make it private again.

## The through-line

The course has a single rule we keep coming back to — the **through-line**. It shows up in different flavours per module, but the underlying idea stays the same. This module's flavour: **the engine never references the shell. The shell references the engine.** If you ever find yourself wanting to call `Console.WriteLine` from inside the engine, that's the engine asking to be coupled to the console. The fix is always the same — the engine returns a value, and the shell decides what to print.

You'll see this rule grow as the course goes. In Phase 2 the engine returns save data and the persistence shell writes it to a file. In Phase 3 the engine returns a result and the web shell wraps it in JSON. The engine never knows how it's being used. That ignorance is what lets the same engine work in five different runtimes.

## What you just did

You took a single project and split it into two — an engine that holds the kingdom's rules and a console that talks to a human. You introduced `ResourceLedger`, the first class whose whole job is to *protect* a dictionary from being misused. You wrote a project reference in one direction (console depends on engine) and never the other way, which is the whole point — the engine is the part you'll keep, the console is the part you'll replace four times this year. Same output as 1.1, but the codebase has been rearranged, and that rearrangement is the entire bet of the course.

**Key concepts you can now name:**

- **engine vs shell** — rules vs everything that talks outside
- **class library** — `.dll` project, no `Main`, used by others
- **project reference** — one-way dependency arrow between projects
- **solution** — file that groups projects to build together
- **read-only wrapping** — class around a dictionary, controlled access

## Quiz

Open `quiz.md`. When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.

## Next

Module 1.3 introduces unit testing. Tests are about the engine, not the shell — and now that the engine is a project of its own, a test project can reference it without dragging the console along. The split you did today is what makes the next lesson possible.

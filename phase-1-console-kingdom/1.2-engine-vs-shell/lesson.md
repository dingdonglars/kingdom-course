# Module 1.2 — Engine vs Shell

> **Hook:** today is the lesson the rest of the course is named after. You take the Kingdom you wrote in 1.1 — all in one project — and split it into a *class library* (the engine: data + logic) and a *console app* (the shell: input + output). The engine never knows about the console. The console depends on the engine. Same code, completely different shape.

> **Words to watch**
> - **class library** — a project that compiles to a `.dll`, not an executable. Has no `Main`. Used by other projects.
> - **engine** — the part of your code that's about the *domain* (the kingdom, its rules)
> - **shell** — the part that talks to the outside world (console, files, network, UI)
> - **project reference** — one project saying "I depend on this other project"
> - **solution** (`.slnx` or `.sln`) — a file that groups multiple related projects so you can build them together

---

## Why split

In 1.1 your `Building`, `Resource`, `Kingdom`, and `Citizen` classes lived in the same project as `Program.cs`. That works for now. But ask yourself: *if you wanted the same Kingdom on a website later (Phase 4), what would you do?* You can't reuse `Program.cs` — websites don't have console output. You'd have to copy the kingdom classes out.

The split prevents that copy. **The engine is the kingdom's logic; the shell is just one way to interact with it.** Tomorrow's web shell will use the same engine. Phase 5's Roblox port will be a Luau translation of the same engine. The engine is the bet.

## Do it — refactor

You have a `KingdomConsole` project from 1.1. We'll restructure into two:

```
your-repo/
├─ Kingdom.Engine/      ← class library (no Main, no Console)
│   ├─ Kingdom.cs
│   ├─ Building.cs
│   ├─ Citizen.cs
│   ├─ Resource.cs
│   ├─ ResourceLedger.cs        ← new — wraps the dict
│   └─ Kingdom.Engine.csproj
├─ Kingdom.Console/     ← console app (no game logic, just input/output)
│   ├─ Program.cs
│   └─ Kingdom.Console.csproj
└─ Kingdom.slnx         ← ties them together
```

### Step 1: create the new project layout

```powershell
cd <your-repo-root>
# Backup your 1.1 folder
Rename-Item KingdomConsole KingdomConsole-v1-backup

# Create new structure
dotnet new sln -n Kingdom
dotnet new classlib -n Kingdom.Engine
dotnet new console -n Kingdom.Console
dotnet sln add Kingdom.Engine Kingdom.Console
dotnet add Kingdom.Console reference Kingdom.Engine
```

The last line is **critical** — it says *"the Console project depends on the Engine project."*

### Step 2: move the classes into Engine

Move `Building.cs`, `Citizen.cs`, `Resource.cs`, `Kingdom.cs` from your backup into `Kingdom.Engine/`. Change their namespaces from `KingdomConsole` to `Kingdom.Engine`.

### Step 3: introduce `ResourceLedger`

Replace the `Dictionary<Resource, int> Resources` in `Kingdom.cs` with a `ResourceLedger`:

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

### Step 4: rewrite `Program.cs` to use the engine

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

### Step 5: build + run

```powershell
dotnet build
dotnet run --project Kingdom.Console
```

Same output as 1.1. **But the shape is completely different.**

## Tinker

- Open `Kingdom.Engine/Kingdom.Engine.csproj`. Notice it has no `<OutputType>Exe</OutputType>` — that's why it's a library. Try adding it. **The build fails** because the engine has no `Main`. *Why is this good?*
- In `Kingdom.Engine/Kingdom.cs`, try adding `Console.WriteLine("hello");`. **It compiles, but you've broken the rule.** The engine should not write to the console. Comment it out. Discuss with Lars why.
- Try changing `_amounts` in `ResourceLedger` from `private` to `public`. The compiler still likes it. Now external code could mess with it directly. *Why is this bad?* (Encapsulation. The ledger should control its own state.)

## Name it

- **Class library.** `dotnet new classlib` makes a project with no `Main`. It compiles to a `.dll`. Other projects reference it with `dotnet add <project> reference <library>`.
- **Engine vs shell.** *Engine* = domain logic. Doesn't know about IO, networks, UI. *Shell* = the IO. Console is one shell; later you'll have an HTTP API shell, a browser shell, a Roblox shell. **Engines outlive their shells.**
- **Project reference.** A `.csproj` line that says "I depend on this project." `dotnet add reference` adds it for you.
- **Solution.** A `.slnx` (or older `.sln`) file that groups projects. `dotnet build` at the solution level builds them all, in dependency order.

## The rule of the through-line

> **The engine never references the shell. The shell references the engine.**

If you ever find yourself wanting to call `Console.WriteLine` from the engine — that's a sign the engine should *return* a string and let the shell decide what to do with it. The engine should not assume how it's used.

## Quiz / challenge

Open `quiz.md`.

## Connect

This refactor is the bet of the entire course. Phase 2 will add a *persistence shell* (file/SQLite). Phase 3 will add a *web API shell*. Phase 4 will add a *browser shell*. Phase 5 will port the engine to *Luau*. Through all five — same engine, different shell.

Module 1.3 introduces unit testing — and tests are fundamentally about the engine, not the shell.
# Module 1.2 — Engine vs Shell

This is the lesson the rest of the course is named after. You take the kingdom you wrote in 1.1 — all in one project — and split it into two. The kingdom's *rules* (buildings, resources, the math) move into a class library called `Kingdom.Engine`. The program that prints things to the terminal becomes a small `Kingdom.Console` project that *uses* the engine. Same code in the end, but a very different layout. The point of today is to see why that layout matters before the later phases ask you to use it.

We're introducing a word you'll see a lot from here on: **shell**. A shell is whatever talks to the outside world — the console here, a web page later in the year, Roblox after that. The engine never talks to the outside. The shell does. The engine only knows about the kingdom. The shell knows about people.

> **Words to watch**
>
> - **class library** — a project that compiles to a `.dll`, not an `.exe`. No `Main`. Other projects use it.
> - **engine** — the part of the code that's about the kingdom and its rules
> - **shell** — the part that talks to the outside world (console, files, network, browser, Roblox)
> - **project reference** — one project saying *"I depend on this other project"*
> - **solution** (`.slnx` or `.sln`) — a file that groups related projects so they build together

---

## Why split

In Module 1.1 your `Building`, `Resource`, `Kingdom`, and `Citizen` classes lived in the same project as `Program.cs`. That works for now. But ask yourself: if you wanted the same kingdom on a website later in the year, what would you do? You can't reuse `Program.cs` — websites don't have a console to print to. You'd be copying the kingdom classes out of one project and into another. The split today stops you from having to copy. The engine becomes the kingdom's logic. The console becomes one way of *playing* it. Phase 4's browser version will be a different way of playing the same logic. Phase 5's Roblox version will turn the same logic into Luau. The engine is the part you keep across all of them.

## Step 1 — create the new layout

You have a `KingdomConsole` project from 1.1. We'll rearrange it into two:

```
your-repo/
├─ Kingdom.Engine/                  ← class library (no Main, no Console)
│   ├─ Kingdom.cs
│   ├─ Building.cs
│   ├─ Citizen.cs
│   ├─ Resource.cs
│   ├─ ResourceLedger.cs            ← new — a class around the dictionary
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

The last line is the important one. It writes a `<ProjectReference>` into `Kingdom.Console.csproj` that says *"the console project depends on the engine project."* The build system reads that line, compiles the engine first, then compiles the console with the engine's `.dll` ready to use.

## Step 2 — move the classes into Engine

Move `Building.cs`, `Citizen.cs`, `Resource.cs`, `Kingdom.cs` from your backup folder into `Kingdom.Engine/`. Open each one and change the namespace from `KingdomConsole` to `Kingdom.Engine`. The rule is that namespaces match folders — the same idea you'll see again in Module 1.9.

## Step 3 — introduce `ResourceLedger`

The dictionary on `Kingdom` is going to gain rules over the next few modules — refusing to spend more than you have, refusing negative amounts, and so on. A plain dictionary can't do that on its own; a class can. So let's put it inside a class now, while it's still small.

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

The dictionary is `private readonly` — code outside the class can't reach it. The only way to change it is through `Add` and `Spend`, and both of those check their inputs first. `Snapshot()` gives it back as an `IReadOnlyDictionary` — outside code can read all four amounts in a loop, but it can't write to them.

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

Notice the type is `Kingdom.Engine.Kingdom` — the full name. We write the full name because `Kingdom` is also the name of a namespace, so C# needs to know which one you mean. (You'll see this same situation again in Module 1.4, with the `global::` prefix in tests. It's the same kind of confusion for the compiler.)

## Step 5 — build and run

```powershell
dotnet build
dotnet run --project Kingdom.Console
```

Same output as 1.1. But the layout is completely different.

## Tinker

Open `Kingdom.Engine/Kingdom.Engine.csproj`. There's no `<OutputType>Exe</OutputType>` line — that's why it's a library, not a program you can run. Try adding one. The build fails, because the engine has no `Main` method. Take the line back out. The engine has nothing to *run* on its own; it's a thing other projects use.

Try adding `Console.WriteLine("hello");` to a method on `Kingdom`. It compiles, but you've broken the rule. The engine is not allowed to print. If something inside the engine needs to say something, it returns a value, and the shell decides what to do with it.

Try changing `_amounts` in `ResourceLedger` from `private` to `public`. The compiler is still happy. But now outside code could reach in and write whatever it wants. Why is that bad? Because the ledger is supposed to refuse negative amounts and refuse overspending, and calling `_amounts.Add(...)` directly skips both of those checks. Make it private again.

## The through-line

The course has one rule we keep coming back to — the **through-line**. It looks a little different in each module, but the idea underneath stays the same. In this module the rule is: **the engine never references the shell. The shell references the engine.** If you ever want to call `Console.WriteLine` from inside the engine, that's the engine trying to tie itself to the console. The fix is always the same — the engine returns a value, and the shell decides what to print.

You'll see this rule grow as the course goes on. In Phase 2 the engine returns save data, and the saving shell writes it to a file. In Phase 3 the engine returns a result, and the web shell turns it into JSON. The engine never knows how it's being used. Because it doesn't know, the same engine can work in five different places.

## What you just did

You took a single project and split it into two — an engine that holds the kingdom's rules, and a console that talks to a person. You added `ResourceLedger`, the first class whose whole job is to *protect* a dictionary from being used wrongly. You wrote a project reference in one direction (console depends on engine) and never the other way. That one direction is the whole point: the engine is the part you'll keep, and the console is the part you'll replace four times this year. Same output as 1.1, but the code is arranged differently, and that new arrangement is the main idea of the course.

**Key concepts you can now name:**

- **engine vs shell** — rules vs everything that talks outside
- **class library** — `.dll` project, no `Main`, used by others
- **project reference** — one-way dependency arrow between projects
- **solution** — file that groups projects to build together
- **read-only wrapping** — class around a dictionary, controlled access

## On your own

Time to put the book away. Don't scroll back up to the steps — prove to yourself, from your own head, that the one big idea stuck: the engine and the shell, and which one is allowed to use the other. No one marks this one — it's just for you. It's the easiest way to spot what *hasn't* stuck yet, while it's still simple to fix. Getting stuck here is completely fine — that's exactly what it's for.

Without looking, answer these three out loud or on paper. (1) Which project depends on the other — does the engine depend on the console, or the console on the engine? (2) The engine has a rule it must never break. What is it? (3) If something inside the engine needs to tell the player a message, what should it do, since it isn't allowed to print?

Then test rule 2 yourself: open any engine file and add `Console.WriteLine("hi");` inside a method. It still builds — the compiler won't stop you. But you've broken the rule. Take the line back out.

<details><summary>Stuck? Open this to check yourself.</summary>

- The **console depends on the engine**, never the other way. The project reference points one way: `Kingdom.Console` → `Kingdom.Engine`.
- The rule: **the engine never talks to the outside world.** No `Console.WriteLine`, no files, no network. That's the shell's job.
- If the engine needs to say something, it **returns a value**, and the shell decides what to print. This is why the same engine can later run in a browser or in Roblox — it doesn't know or care which shell is using it.

</details>

## Wrap up

1. **Quiz** — open `quiz.md`, jot your answers in `journal/quiz-notes.md`.
2. **Progress** — one line in `journal/progress.md`: `Module 1.2 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit and push** — stage both files, commit message `Module 1.2 done`, Sync.
4. **Post in `#wins`** — one line about today, plus the URL of the commit.

Module 0.1 covers the why and the panel/CLI steps if you need a refresher. Bring quiz answers you're least sure about to the next weekly sync.

## Next

Module 1.3 introduces unit testing. Tests are about the engine, not the shell — and now that the engine is its own project, a test project can reference it without pulling in the console too. The split you did today is what makes the next lesson possible.

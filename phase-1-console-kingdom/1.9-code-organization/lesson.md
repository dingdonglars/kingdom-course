# Module 1.9 — Code Organisation

`Kingdom.Engine/` now has fourteen files in a single flat folder. It's already getting hard to scan. Today we don't add features — we move things. Folders by topic, sub-namespaces, and a small naming pass. The kingdom *behaves* identically; the *codebase* feels twice the size in the right way and half the size to read.

The reason to organise *now*, before the engine is huge, is the same reason to tidy a desk before it becomes a disaster: cheap when small, expensive when large. A practical rule: once a folder has more than about seven files, it's time to think about subfolders. A flat structure is fine until it isn't.

> **Words to watch**
>
> - **sub-namespace** — `Kingdom.Engine.Events` is a sub-namespace of `Kingdom.Engine`. Same project, different bucket.
> - **using directive** — the `using Kingdom.Engine.Events;` line that brings names from a namespace into scope
> - **folder** — pure organisation; the compiler doesn't care about folders, only namespaces. We line them up by hand, by convention.
> - **aggregate root** — the class that owns everything else in a model. `Kingdom` owns its buildings, citizens, resources. *(First use: this lesson.)*

---

## The new layout

```
Kingdom.Engine/
├─ Kingdom.cs              ← the aggregate root stays at top level
├─ Kingdom.Engine.csproj
├─ Buildings/
│   ├─ Building.cs
│   ├─ Farm.cs
│   ├─ Lumberyard.cs
│   └─ Mine.cs
├─ Citizens/
│   └─ Citizen.cs
├─ Resources/
│   ├─ Resource.cs
│   └─ ResourceLedger.cs
├─ Events/
│   ├─ KingdomEvent.cs     (and the three subclass records)
│   └─ EventEngine.cs
└─ Infrastructure/
    ├─ IRandom.cs
    ├─ SystemRandom.cs
    ├─ IClock.cs
    └─ SystemClock.cs
```

`Kingdom.cs` stays at the top because it's the **aggregate root** — the class that ties everything together and owns the others. (You'll meet this term in design vocabulary too: the aggregate root is the entry point through which all changes to the model flow.) The five subfolders are areas of concern: buildings, citizens, resources, events, and infrastructure (the boring plumbing).

## Sub-namespaces

Each subfolder gets a matching sub-namespace:

```csharp
// Buildings/Farm.cs
namespace Kingdom.Engine.Buildings;

public class Farm : Building { ... }
```

```csharp
// Events/EventEngine.cs
namespace Kingdom.Engine.Events;
using Kingdom.Engine.Infrastructure;
using Kingdom.Engine.Buildings;
using Kingdom.Engine.Citizens;

public class EventEngine { ... }
```

Two effects. First, **discoverability** — typing `Kingdom.Engine.B` in the editor immediately suggests `Buildings.Farm`, `Buildings.Lumberyard`, `Buildings.Mine`. Without sub-namespaces, you'd see all fourteen types at once. Second, **intent** — a class declared in `Kingdom.Engine.Infrastructure` is signalling *"this is plumbing, not domain."* Anyone reading the engine knows where to look for what.

## The cost

Every file that crosses a boundary needs a `using` line. `EventEngine` now needs `using Kingdom.Engine.Buildings;`, `using Kingdom.Engine.Citizens;`, and `using Kingdom.Engine.Infrastructure;`. That's three new lines. Pay this cost when your engine is mid-sized; don't pre-emptively split a four-file project.

## Step 1 — apply the move

Two options, same end state.

**Option A — copy from this module's `starter/`.** This module's starter is a *full snapshot*, not a delta. Replace your entire `Kingdom.Engine/` folder with the one in `starter/Kingdom.Engine/`. Same for the test project (only the `using` lines change).

**Option B — do the move yourself.** In your IDE, select each file, right-click → Move to folder, pick the target. Then update each file's `namespace` line. Then add `using` directives wherever needed (the compiler tells you with red squiggles). This is closer to how moves happen in real codebases — practice it.

Either way, do not change *behaviour*. Same buildings, same events, same tests.

## Step 2 — update `Kingdom.cs`

`Kingdom.cs` (the file) now needs `using` directives because the types it uses live elsewhere:

```csharp
using Kingdom.Engine.Buildings;
using Kingdom.Engine.Citizens;
using Kingdom.Engine.Events;
using Kingdom.Engine.Infrastructure;
using Kingdom.Engine.Resources;

namespace Kingdom.Engine;

public class Kingdom { ... unchanged ... }
```

## Step 3 — update `Program.cs`

`Program.cs` adds a wider set of `using`s (or, more readable, brings them in via *global usings* — see "Tinker"):

```csharp
using Kingdom.Engine;
using Kingdom.Engine.Buildings;
using Kingdom.Engine.Citizens;
using Kingdom.Engine.Infrastructure;

IRandom rng = new SystemRandom(seed: 42);
IClock clock = new SystemClock();
var kingdom = new Kingdom.Engine.Kingdom("Eldoria", rng, clock);
kingdom.AddBuilding(new Farm("Main Farm"));
// ... rest unchanged
```

## Step 4 — update test files

Each test file gets the right `using` for the types it uses. For example, `EventEngineTests.cs`:

```csharp
using FakeItEasy;
using Kingdom.Engine;
using Kingdom.Engine.Buildings;
using Kingdom.Engine.Citizens;
using Kingdom.Engine.Events;
using Kingdom.Engine.Infrastructure;
using Shouldly;
```

Lots of usings. That's the cost of the structure. The benefit is that each file's `using` block is now a *map* — *"this code touches buildings, citizens, events, infrastructure."*

## Step 5 — verify

Nothing has changed about behaviour. Run:

```powershell
dotnet build
dotnet test
```

You should still see 35 passing — same as before. If anything fails, you reorganised wrong, not "the code broke." Read the error: typically a missing `using`.

## Tinker

**Global usings.** Add a file `Kingdom.Engine/GlobalUsings.cs` with:

```csharp
global using Kingdom.Engine.Buildings;
global using Kingdom.Engine.Citizens;
global using Kingdom.Engine.Resources;
global using Kingdom.Engine.Infrastructure;
global using Kingdom.Engine.Events;
```

Now files inside `Kingdom.Engine` don't need to repeat those usings — they're brought in everywhere. Cuts the noise. Use sparingly, though — global usings hide where types come from. Good for a project's own sub-namespaces; bad for third-party libraries.

Try moving `Kingdom.cs` *into* `Buildings/`. The compiler's fine, but it's a bad smell — `Kingdom` isn't a building. Move it back. The folder layout encodes the model.

Make `EventEngine` `internal` instead of `public`. The compiler is happy because `EventEngine` is only used from inside the engine. Now external code (the console, the tests) can't poke at it directly. That's encapsulation at the assembly level — `internal` types are visible inside the same project only.

Try making the `_eventEngine` field on `Kingdom` `protected` instead of `private`. The compiler accepts it, but anyone subclassing `Kingdom` could now reach in. Keep it `private` — `Kingdom` doesn't have subclasses today, and won't.

## The through-line

The through-line in this module: **folders by intent, not by type**. Group `Farm`, `Lumberyard`, `Mine`, `Building` together (all "buildings"), not `Farm.cs`, `FarmTests.cs`, `FarmDocs.md` together. The reader cares about the *concept*; the rest is supporting cast. You'll see this rule scaled up in Phase 3 (`Server/Controllers/`, `Server/Services/`) and Phase 4 (`Web/Components/Buildings/`, `Web/Components/Citizens/`).

## What you just did

You moved fourteen files into five subfolders, gave each subfolder a matching sub-namespace, and added the `using` directives that crossed the boundaries. Nothing about the kingdom changed; everything about how the codebase reads did. You met the **aggregate root** idea (the class at the top of an owned model — Kingdom, here) and the **internal** access modifier (visible inside the same project only). All thirty-five tests still passed at the end, which was the only proof that mattered.

**Key concepts you can now name:**

- **sub-namespace** — same project, deeper namespace path
- **folder = namespace** — convention, lined up by hand
- **`global using`** — directive applied to every file in the project
- **`internal` vs `public`** — visible to project vs visible everywhere
- **aggregate root** — top class that owns the model

## Quiz

Open `quiz.md`. When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.

## Next

Module 1.10 is the **polish + repo rescue** module — README, comments, naming pass, plus the *"if your repo is wrecked, rescue it from a known-good state"* workflow. M2 closes there.

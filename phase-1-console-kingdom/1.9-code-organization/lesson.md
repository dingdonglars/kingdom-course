# Module 1.9 — Code Organisation

> **Hook:** `Kingdom.Engine/` now has 14 files. They all live in a single flat folder. It's already getting hard to scan. Today we **don't add features.** We move things. Folders by topic, sub-namespaces, and a small naming pass. The kingdom *behaves* identically; the *codebase* feels twice the size in the right way and half the size to read.

> **Words to watch**
> - **sub-namespace** — `Kingdom.Engine.Events` is a sub-namespace of `Kingdom.Engine`. Same project, different bucket.
> - **using directive** — the `using Kingdom.Engine.Events;` line that brings names from a namespace into scope.
> - **folder** — pure organisation; the compiler doesn't care about folders, only namespaces. (We use both, in lockstep, by convention.)

---

## Why now

You can build a 14-file engine without folders. You can build a 50-file one without folders. **You'll regret it both times.** The reason to organise *now* is the same reason to organise a desk before it's a disaster: cheap when small, expensive when large.

A practical rule: **once a folder has more than ~7 files, it's time to think about sub-folders.** A flatter structure is fine until it isn't.

## The new shape

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
│   ├─ KingdomEvent.cs     (and the 3 subclass records)
│   └─ EventEngine.cs
└─ Infrastructure/
    ├─ IRandom.cs
    ├─ SystemRandom.cs
    ├─ IClock.cs
    └─ SystemClock.cs
```

`Kingdom.cs` stays at the top because it's the *aggregate root* — the class that ties everything together. The five subfolders are *areas of concern*: buildings, citizens, resources, events, infrastructure (boring plumbing).

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

Two effects:

1. **Discoverability.** Typing `Kingdom.Engine.B` in the editor immediately suggests `Buildings.Farm`, `Buildings.Lumberyard`, `Buildings.Mine`. Without sub-namespaces, you'd see all 14 types at once.
2. **Intent.** A class declared in `Kingdom.Engine.Infrastructure` is *signalling* — *"this is plumbing, not domain."* Anyone reading the engine knows where to look for what.

## Cost of sub-namespaces

Every file that crosses a boundary needs a `using` line. `EventEngine` now needs `using Kingdom.Engine.Buildings;` and `using Kingdom.Engine.Citizens;` and `using Kingdom.Engine.Infrastructure;`. That's three new lines. **Pay this cost when your engine is mid-sized.** Don't pre-emptively sub-namespace a 4-file project.

## Step 1 — apply the move

You have two options. Both produce identical end state.

**Option A — copy from this module's `starter/`.** This module's starter is a *full snapshot*, not a delta. Replace your entire `Kingdom.Engine/` folder with the one in `starter/Kingdom.Engine/`. Same for the test project (only the `using` lines change).

**Option B — do the move yourself.** In your IDE: select each file, right-click → Move to folder, pick the target. Then update each file's `namespace` line. Then add `using` directives wherever needed (the compiler will tell you with red squiggles). This is closer to how moves happen in real codebases — practice it.

Either way, do not change *behavior*. Same buildings, same events, same tests.

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

Lots of usings. That's the cost of the structure. The benefit is each file's `using` block is now a *map* — *"this code touches buildings, citizens, events, infrastructure."*

## Step 5 — verify

Nothing has changed semantically. Run:

```powershell
dotnet build
dotnet test
```

Expect 35 passing — same as before. **If anything fails, you reorganised wrong, not "the code broke."** Read the error: typically a missing `using`.

## Tinker

- **Global usings.** Add a file `Kingdom.Engine/GlobalUsings.cs` with:
  ```csharp
  global using Kingdom.Engine.Buildings;
  global using Kingdom.Engine.Citizens;
  global using Kingdom.Engine.Resources;
  global using Kingdom.Engine.Infrastructure;
  global using Kingdom.Engine.Events;
  ```
  Now files inside `Kingdom.Engine` don't need to repeat the usings — they're brought in everywhere. Cuts the noise. **Use sparingly** — global usings hide where types come from. Good for a project's own sub-namespaces; bad for third-party libraries.
- **Try moving `Kingdom.cs` *into* `Buildings/`.** Bad smell — `Kingdom` is the root, not a building. Move it back. The folder shape encodes the model.
- **Make `EventEngine` `internal`** instead of `public`. The compiler is fine — it's only used inside the engine. **Now external code can't poke at it.** That's encapsulation at the assembly level.
- **Try making the `_eventEngine` field on `Kingdom` `protected` instead of `private`.** Compiler accepts it, but anyone subclassing `Kingdom` could now mess with the engine. Keep it `private`.

## Name it

- **Sub-namespace.** Same project, deeper namespace path. Use to group related types when a project gets large.
- **Folder ↔ namespace convention.** Folders are organisation; namespaces are scope. We line them up by hand — the compiler doesn't enforce it, but every team does.
- **Global using.** A `global using` directive applies to every file in the project. Tames sub-namespace noise.
- **`internal` vs `public`.** `internal` types are visible inside the same assembly only. Use it for "this is implementation, not API."

## The rule of the through-line

> **Folders by intent, not by type.** Group `Farm`, `Lumberyard`, `Mine`, `Building` together (all "buildings"); not `Farm.cs`, `FarmTests.cs`, `FarmDocs.md` together. The reader cares about the *concept* — the rest is supporting cast.

You'll see this rule scaled up in Phase 3 (`Server/Controllers/`, `Server/Services/`) and Phase 4 (`Web/Components/Buildings/`, `Web/Components/Citizens/`).

## Quiz / challenge

Open `quiz.md`.

## Connect

Module 1.10 is the **polish + repo rescue** module — README, comments, naming pass, plus the "if your repo is wrecked, rescue it from a known-good snapshot" workflow. Closing Block 3.
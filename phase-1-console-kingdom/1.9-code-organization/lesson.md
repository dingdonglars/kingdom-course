# Module 1.9 — Code Organisation

> Travou no inglês? Abra o `lesson.pt.md` — é esta mesma lição em português. Tente em inglês primeiro.

`Kingdom.Engine/` now has fourteen files all in one folder. It's already getting hard to read through. Today we don't add features — we move things. Folders by topic, sub-namespaces, and a small naming pass. The kingdom *works* exactly the same; the *code* just becomes much easier to read.

Why organise *now*, before the engine is huge? For the same reason you tidy your desk before it gets buried: it's quick while there's little, and slow once there's a lot. A useful rule: once a folder has more than about seven files, it's time to think about subfolders. One flat folder is fine for a while, and then one day it isn't.

> **Words to watch**
>
> - **sub-namespace** — `Kingdom.Engine.Events` is a sub-namespace of `Kingdom.Engine`. Same project, a separate group inside it.
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

`Kingdom.cs` stays at the top because it's the **aggregate root** — the class that ties everything together and owns the others. (You'll meet this term in design talk too: the aggregate root is the one entry point that all changes to the model go through.) The five subfolders each cover one area: buildings, citizens, resources, events, and infrastructure (the boring support code).

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

Two effects. First, **it's easier to find things** — typing `Kingdom.Engine.B` in the editor right away suggests `Buildings.Farm`, `Buildings.Lumberyard`, `Buildings.Mine`. Without sub-namespaces, you'd see all fourteen types at once. Second, **it shows what each class is for** — a class in `Kingdom.Engine.Infrastructure` is telling you *"this is support code, not the kingdom's rules."* Anyone reading the engine knows where to look for what.

## The cost

Every file that uses a type from another sub-namespace needs a `using` line. `EventEngine` now needs `using Kingdom.Engine.Buildings;`, `using Kingdom.Engine.Citizens;`, and `using Kingdom.Engine.Infrastructure;`. That's three new lines. Take on this cost when your engine has grown to a medium size; don't split a four-file project early for no reason.

## Step 1 — apply the move

Two options, same end state.

**Option A — copy from this module's `starter/`.** This module's starter is a *full copy* of the whole folder, not just the changed parts. Replace your entire `Kingdom.Engine/` folder with the one in `starter/Kingdom.Engine/`. Do the same for the test project (only the `using` lines change there).

**Option B — do the move yourself.** In your editor, select each file, right-click → Move to folder, and pick the target folder. Then update each file's `namespace` line. Then add `using` lines wherever needed (the compiler shows you with red underlines). This is close to how moves happen in real projects, so it's good practice.

Either way, do not change what the code *does*. Same buildings, same events, same tests.

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

`Program.cs` adds a wider set of `using`s (or, to keep it cleaner, brings them in with *global usings* — see "Tinker"):

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

Lots of usings. That's the cost of the structure. The benefit is that each file's `using` block now works like a map — it says *"this code uses buildings, citizens, events, and infrastructure."*

## Step 5 — verify

Nothing has changed about what the code does. Run:

```powershell
dotnet build
dotnet test
```

You should still see 35 passing — same as before. If anything fails, it means you moved something wrong, not that the code itself broke. Read the error; it's usually a missing `using`.

## Tinker

**Global usings.** Add a file `Kingdom.Engine/GlobalUsings.cs` with:

```csharp
global using Kingdom.Engine.Buildings;
global using Kingdom.Engine.Citizens;
global using Kingdom.Engine.Resources;
global using Kingdom.Engine.Infrastructure;
global using Kingdom.Engine.Events;
```

Now files inside `Kingdom.Engine` don't need to repeat those usings — they apply everywhere. Less clutter. Use this carefully, though: global usings hide where each type comes from. They're good for a project's own sub-namespaces, but not good for outside libraries.

Try moving `Kingdom.cs` *into* `Buildings/`. The compiler is fine with it, but it's a clear mistake — `Kingdom` isn't a building. Move it back. The folder layout should match the model.

Make `EventEngine` `internal` instead of `public`. The compiler is happy, because `EventEngine` is only used from inside the engine. Now code outside the engine (the console, the tests) can't use it directly. That's encapsulation at the whole-project level — `internal` types are visible only inside the same project.

Try making the `_eventEngine` field on `Kingdom` `protected` instead of `private`. The compiler accepts it, but now any class that inherits from `Kingdom` could reach into it. Keep it `private` — `Kingdom` has no subclasses today, and won't.

## The through-line

The through-line in this module: **group folders by what the code is about, not by what type of file it is**. Keep `Farm`, `Lumberyard`, `Mine`, `Building` together (all "buildings"), instead of putting `Farm.cs`, `FarmTests.cs`, `FarmDocs.md` together. The reader cares about the *idea* first. You'll see this same rule used at a bigger scale in Phase 3 (`Server/Controllers/`, `Server/Services/`) and Phase 4 (`Web/Components/Buildings/`, `Web/Components/Citizens/`).

## What you just did

You moved fourteen files into five subfolders, gave each subfolder a matching sub-namespace, and added the `using` lines for types used across them. Nothing about the kingdom changed, but a lot about how the code reads did. You met the **aggregate root** idea (the class at the top that owns the model — Kingdom, here) and the **internal** access modifier (visible only inside the same project). All thirty-five tests still passed at the end, which was the only proof that mattered.

**Key concepts you can now name:**

- **sub-namespace** — same project, deeper namespace path
- **folder = namespace** — convention, lined up by hand
- **`global using`** — directive applied to every file in the project
- **`internal` vs `public`** — visible to project vs visible everywhere
- **aggregate root** — top class that owns the model

## On your own

Time to put the book away. Don't scroll back up to the steps — prove to yourself, from your own head, that the one big idea stuck: a folder and its sub-namespace line up by hand. No one marks this one — it's just for you. It's the easiest way to spot what *hasn't* stuck yet, while it's still simple to fix. Getting stuck here is completely fine — that's exactly what it's for.

Without looking, do this from memory in your engine:

1. Make a new subfolder called `Trade/`.
2. Add an empty class file `Caravan.cs` inside it.
3. Write the right `namespace` line at the top of that file so it matches the folder.
4. Back in `Kingdom.cs`, picture the one new line you'd need if `Kingdom` wanted to use `Caravan`.
5. Build to check the file compiles.

<details><summary>Stuck? Open this to check yourself.</summary>

- The file in `Trade/` gets the namespace that matches the folder:

  ```csharp
  namespace Kingdom.Engine.Trade;

  public class Caravan { }
  ```

- For `Kingdom.cs` to use `Caravan`, it would need one `using` line: `using Kingdom.Engine.Trade;`. The compiler doesn't care about folders — only namespaces. We line the two up by hand, by convention, so the layout is easy to read.

</details>

## Wrap up

1. **Quiz** — open `quiz.md`, jot your answers in `journal/quiz-notes.md`.
2. **Progress** — one line in `journal/progress.md`: `Module 1.9 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit and push** — stage both files, commit message `Module 1.9 done`, Sync.
4. **Post in `#wins`** — one line about today, plus the URL of the commit.

Module 0.1 covers the why and the panel/CLI steps if you need a refresher. Bring quiz answers you're least sure about to the next weekly sync.

## Next

Module 1.10 is the **polish + repo rescue** module — README, comments, naming pass, plus the *"if your repo is wrecked, rescue it from a known-good state"* workflow. M2 closes there.

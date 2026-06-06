# Module 2.2 — JSON Serialisation

Yesterday you wrote five lines of plain text to disk. Today the kingdom learns to write itself as **JSON** — the `{ "Name": "Eldoria", "Day": 11, ... }` format used by almost every API on the internet. You'll also add a brand new project — `Kingdom.Persistence` — to keep the JSON code separate from both the engine and the console. Same kingdom, third shell.

JSON is also where you meet a small but important pattern: the **DTO**. The letters stand for *Data Transfer Object*. It's a small record that holds only data, built for one job: moving that data across a boundary, like writing it to disk or sending it over the network. You'll use DTOs again in every phase from here on. We're introducing them properly today.

> **Words to watch**
>
> - **JSON** *(jay-son)* — *JavaScript Object Notation*. Plain text in `{ "key": value, ... }` form. Universally readable.
> - **serialise** — turn an object into a string (or bytes). *Deserialise* is the reverse.
> - **`System.Text.Json`** — the modern .NET JSON library. Built in. Use this rather than the older Newtonsoft.
> - **DTO** — *Data Transfer Object*. A small data-only record purpose-built for crossing a boundary (disk, network, etc.).

---

## Why JSON, why a new project

Phase 1 showed that *small immutable records* are a good way to model data. JSON is what records turn into when they leave the program — to go to disk, to the network, or even to another language. A `record` in C# converts to JSON and back again cleanly, with almost no extra work.

The new project — `Kingdom.Persistence` — exists for one reason: the engine should not know about JSON. If we put JSON code in `Kingdom.Engine`, every shell built on top of it would carry JSON along, even when it doesn't need it. (Roblox doesn't use JSON; the database shell uses SQL, not JSON.) By giving JSON its own project, each shell can choose whether to use it.

Here is which project depends on which:

```
Kingdom.Console  ──┐
                   ├──► Kingdom.Persistence  ──►  Kingdom.Engine
Kingdom.Persistence┤
                   └──► Kingdom.Engine
```

Console depends on both. Persistence depends on Engine. Engine depends on nothing.

## Delta starter

- **NEW project:** `Kingdom.Persistence/` (classlib) with `KingdomSummary.cs` + `KingdomJsonStore.cs`
- **NEW:** `Kingdom.Persistence/Kingdom.Persistence.csproj`
- **MODIFIED:** `Kingdom.slnx` (add the new project)
- **MODIFIED:** `Kingdom.Console/Program.cs` (uses the store)
- **MODIFIED:** `Kingdom.Console/Kingdom.Console.csproj` (project reference to Persistence)
- **NEW:** `tests/Kingdom.Persistence.Tests/` test project (or add tests to the existing engine-tests project — see Step 5)

## Step 1 — set up the new project

From the repo root:

```powershell
dotnet new classlib -n Kingdom.Persistence
dotnet add Kingdom.Persistence reference Kingdom.Engine
dotnet add Kingdom.Console reference Kingdom.Persistence
dotnet sln Kingdom.slnx add Kingdom.Persistence
```

You now have a third project alongside `Kingdom.Engine` and `Kingdom.Console`.

## Step 2 — the DTO record

`Kingdom.Persistence/KingdomSummary.cs`:

```csharp
namespace Kingdom.Persistence;

/// <summary>
/// A small data-only snapshot of a kingdom. Designed for the wire/disk —
/// not the engine model. Lossy on purpose (drops EventLog, internal state).
/// </summary>
public record KingdomSummary(
    string Name,
    int Day,
    int BuildingCount,
    int CitizenCount,
    int Gold,
    int Wood,
    int Stone,
    int Food
);
```

Why a separate record, instead of saving `Kingdom` directly? JSON serialisation reads from properties. But `Kingdom` has interfaces, private fields, and a constructor that needs `IRandom` and `IClock`. `JsonSerializer` doesn't know what to do with any of that. A DTO built for the job is much simpler — it holds only what we care about, in the form we want.

This is the **DTO pattern.** You'll see it everywhere: APIs, message queues, file formats. There's always a small separate record at the boundary.

## Step 3 — the store

`Kingdom.Persistence/KingdomJsonStore.cs`:

```csharp
using System.Text.Json;
using Kingdom.Engine;
using Kingdom.Engine.Resources;

namespace Kingdom.Persistence;

public class KingdomJsonStore
{
    private static readonly JsonSerializerOptions Options = new()
    {
        WriteIndented = true       // human-readable output; flip to false for compactness
    };

    public void Save(Kingdom.Engine.Kingdom kingdom, string path)
    {
        var summary = ToSummary(kingdom);
        var json = JsonSerializer.Serialize(summary, Options);
        File.WriteAllText(path, json);
    }

    public KingdomSummary Load(string path)
    {
        var json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<KingdomSummary>(json)
            ?? throw new InvalidOperationException("Could not deserialize kingdom.");
    }

    public static KingdomSummary ToSummary(Kingdom.Engine.Kingdom k) =>
        new(
            k.Name,
            k.Day,
            k.Buildings.Count,
            k.Citizens.Count,
            k.Resources.Get(Resource.Gold),
            k.Resources.Get(Resource.Wood),
            k.Resources.Get(Resource.Stone),
            k.Resources.Get(Resource.Food)
        );
}
```

`JsonSerializer.Serialize(value, options)` turns the record into JSON. `Deserialize<T>(json)` turns JSON back into `T`. For a `record`, that's all you need — no attributes, no mapping by hand.

`WriteIndented = true` gives you JSON across several lines, easy to read. Set it to `false` once you no longer need to read the file yourself.

## Step 4 — use it from the console

`Kingdom.Console/Program.cs`:

```csharp
using Kingdom.Engine;
using Kingdom.Engine.Buildings;
using Kingdom.Engine.Citizens;
using Kingdom.Engine.Infrastructure;
using Kingdom.Persistence;

IRandom rng = new SystemRandom(seed: 42);
var kingdom = new Kingdom.Engine.Kingdom("Eldoria", rng, new SystemClock());
kingdom.AddBuilding(new Farm("Main Farm"));
kingdom.AddBuilding(new Lumberyard("Eastern Lumberyard"));
kingdom.AddCitizen(new Citizen("Lyra"));
kingdom.AddCitizen(new Citizen("Roric"));
for (int i = 0; i < 10; i++) kingdom.AdvanceDay();

var saveFolder = Path.Combine(AppContext.BaseDirectory, "saves");
Directory.CreateDirectory(saveFolder);
var savePath = Path.Combine(saveFolder, "kingdom.json");

var store = new KingdomJsonStore();
store.Save(kingdom, savePath);
Console.WriteLine($"Saved to {savePath}");

var loaded = store.Load(savePath);
Console.WriteLine();
Console.WriteLine("=== Loaded summary ===");
Console.WriteLine($"  Name: {loaded.Name}");
Console.WriteLine($"  Day:  {loaded.Day}");
Console.WriteLine($"  Buildings: {loaded.BuildingCount}, Citizens: {loaded.CitizenCount}");
Console.WriteLine($"  Gold: {loaded.Gold}, Wood: {loaded.Wood}, Stone: {loaded.Stone}, Food: {loaded.Food}");
```

Build and run:

```powershell
dotnet build
dotnet run --project Kingdom.Console
```

Open `bin/Debug/net10.0/saves/kingdom.json` — clean, indented JSON. You can even edit it by hand in Notepad and it will still load back. That's the nice thing about plain text formats.

## Step 5 — tests

A choice to make: do the persistence tests go in the existing `Kingdom.Engine.Tests` project, or in a new `Kingdom.Persistence.Tests` project?

- **Add to existing:** simpler. The tests reference both the Engine and Persistence projects.
- **New project:** cleaner — each library gets its own tests. This is how bigger codebases do it.

We'll go with the **new project** — it teaches the right pattern. From the repo root:

```powershell
dotnet new xunit -n Kingdom.Persistence.Tests -o tests/Kingdom.Persistence.Tests
dotnet add tests/Kingdom.Persistence.Tests reference Kingdom.Engine
dotnet add tests/Kingdom.Persistence.Tests reference Kingdom.Persistence
dotnet add tests/Kingdom.Persistence.Tests package Shouldly
dotnet sln Kingdom.slnx add tests/Kingdom.Persistence.Tests
```

`tests/Kingdom.Persistence.Tests/KingdomJsonStoreTests.cs`:

```csharp
using Kingdom.Engine;
using Kingdom.Engine.Buildings;
using Kingdom.Engine.Citizens;
using Kingdom.Engine.Infrastructure;
using Kingdom.Persistence;
using Shouldly;

namespace Kingdom.Persistence.Tests;

public class KingdomJsonStoreTests
{
    [Fact]
    public void Save_ThenLoad_RoundtripsName()
    {
        var path = Path.Combine(Path.GetTempPath(), $"kingdom-{Guid.NewGuid():N}.json");
        try
        {
            var k = new global::Kingdom.Engine.Kingdom("Roundtripper", new SystemRandom(7), new SystemClock());
            var store = new KingdomJsonStore();
            store.Save(k, path);
            var loaded = store.Load(path);
            loaded.Name.ShouldBe("Roundtripper");
        }
        finally { if (File.Exists(path)) File.Delete(path); }
    }

    [Fact]
    public void Save_ProducesIndentedJson()
    {
        var path = Path.Combine(Path.GetTempPath(), $"kingdom-{Guid.NewGuid():N}.json");
        try
        {
            var k = new global::Kingdom.Engine.Kingdom("X");
            new KingdomJsonStore().Save(k, path);
            var raw = File.ReadAllText(path);
            raw.ShouldContain("\n");                // multi-line
            raw.ShouldContain("\"Name\": \"X\"");   // pretty form
        }
        finally { if (File.Exists(path)) File.Delete(path); }
    }

    [Fact]
    public void ToSummary_CapturesAllKnownFields()
    {
        var k = new global::Kingdom.Engine.Kingdom("Test");
        k.AddBuilding(new Farm("F"));
        k.AddCitizen(new Citizen("A"));
        for (int i = 0; i < 5; i++) k.AdvanceDay();

        var s = KingdomJsonStore.ToSummary(k);
        s.Name.ShouldBe("Test");
        s.Day.ShouldBe(6);
        s.BuildingCount.ShouldBe(1);
        s.CitizenCount.ShouldBe(1);
    }

    [Fact]
    public void Load_MissingFile_Throws()
    {
        var store = new KingdomJsonStore();
        var path = Path.Combine(Path.GetTempPath(), $"missing-{Guid.NewGuid():N}.json");
        Should.Throw<FileNotFoundException>(() => store.Load(path));
    }

    [Fact]
    public void Load_InvalidJson_ThrowsJsonException()
    {
        var path = Path.Combine(Path.GetTempPath(), $"bad-{Guid.NewGuid():N}.json");
        try
        {
            File.WriteAllText(path, "{ this is not json");
            var store = new KingdomJsonStore();
            Should.Throw<System.Text.Json.JsonException>(() => store.Load(path));
        }
        finally { if (File.Exists(path)) File.Delete(path); }
    }
}
```

Run all tests across both projects:

```powershell
dotnet test
```

Expect `Passed: 43` (38 from Module 2.1 + 5 new in the persistence test project).

## Tinker

Set `WriteIndented = false`. Save a kingdom. The JSON is now one line — same data, much smaller. That's the form that gets sent over the network.

Add a property to `KingdomSummary` — say, `int FarmCount`. Run the test that loads an old JSON file (one without `FarmCount`). It loads fine, with `FarmCount = 0` (the default value). JSON doesn't mind when a field is missing.

Add `[JsonPropertyName("name")]` above `Name` in the record. Now the JSON output uses lowercase `"name"`. This is useful when you need to match an API that already exists.

Try saving a thousand kingdoms in a loop, each with a different name. Saving starts to feel slow. That's the point where a database makes more sense.

## What you just did

You built your first real save format. Same kingdom from yesterday, but now it serialises to JSON and loads back cleanly — five tests prove it (43 passing total). Along the way you added a third project, `Kingdom.Persistence`, so the engine doesn't carry JSON along to shells that don't want it. You also met the **DTO** pattern, which you'll see in every phase from here on: a small record that holds only data, built for crossing a boundary, kept separate from the engine's model. The engine project still has no changes — that's two modules in a row.

**Key concepts you can now name:**

- **JSON** — universal plain-text data format
- **serialise / deserialise** — object-to-text, text-to-object
- **`JsonSerializer`** — `Serialize(value, opts)`, `Deserialize<T>(json)`
- **DTO** — small data-only record at a boundary
- **`WriteIndented`** — readable vs compact JSON output

## On your own

Time to put the book away. Don't scroll back up to the steps — show yourself, from your own head, that the one big idea stuck: turn a record into JSON, then turn the JSON back into a record. No one marks this — it's just for you. It's the fastest way to spot what hasn't stuck yet, while it's still small to fix. Getting stuck here is completely fine — that's exactly what it's for.

Open a new empty file. Make a tiny `record Player(string Name, int Score)`. Then, without looking:

1. Serialise one `Player` to a JSON string with `JsonSerializer.Serialize(...)`.
2. Print the string so you can see it.
3. Deserialise it back into a `Player` with `JsonSerializer.Deserialize<Player>(...)`.
4. Run it — the name and score you get back should match the ones you put in.

<details><summary>Stuck? Open this to check yourself.</summary>

```csharp
using System.Text.Json;

var player = new Player("Lyra", 250);

var json = JsonSerializer.Serialize(player);
Console.WriteLine(json);                 // {"Name":"Lyra","Score":250}

var back = JsonSerializer.Deserialize<Player>(json);
Console.WriteLine($"{back!.Name} {back.Score}");   // Lyra 250

record Player(string Name, int Score);
```

A `record` needs no extra setup — `Serialize` reads its properties, `Deserialize<T>` fills them back in. That's the whole idea.

</details>

## Wrap up

1. **Quiz** — open `quiz.md`, jot your answers in `journal/quiz-notes.md`.
2. **Progress** — one line in `journal/progress.md`: `Module 2.2 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit and push** — stage both files, commit message `Module 2.2 done`, Sync.
4. **Post in `#wins`** — one line about today, plus the URL of the commit.

Module 0.1 covers the why and the panel/CLI steps if you need a refresher. Bring quiz answers you're least sure about to the next weekly sync.

## Next

Module 2.3 — **round-trip tests** — takes today's pattern further: any kingdom you save should match itself when you load it back. We'll write the *full* snapshot (not just the summary) and prove that the engine state can be rebuilt exactly.

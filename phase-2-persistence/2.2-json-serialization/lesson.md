# Module 2.2 — JSON Serialisation

> **Hook:** today the kingdom learns to write itself as **JSON** — the `{ "Name": "Eldoria", "Day": 11, ... }` format that powers ~every API on the internet. We add a brand-new project — `Kingdom.Persistence` — to keep the JSON code separate from both engine and console. Same kingdom, third shell.

> **Words to watch**
> - **JSON** — *JavaScript Object Notation*. Plain text in `{ "key": value, ... }` shape. Universally readable.
> - **serialise** — turn an object into a string (or bytes). *Deserialise* is the reverse.
> - **`System.Text.Json`** — the modern .NET JSON library. Built in. Use this, not the older Newtonsoft.
> - **DTO** — *Data Transfer Object*. A small data-only record purpose-built for crossing a boundary (disk, network, etc.).

---

## Why JSON, why a new project

Block 3's events were a great sign that *small immutable records* are how we model data. JSON is what records become when they leave the program — for disk, for network, for another language entirely. **A `record` in C# round-trips cleanly to JSON; almost no plumbing needed.**

The new project — `Kingdom.Persistence` — exists for one reason: **the engine should not know about JSON.** If we put JSON code in `Kingdom.Engine`, every shell ever built would drag JSON along, even when it doesn't need it (Roblox doesn't use JSON; the database shell uses SQL not JSON). By giving JSON its own project, every shell can pick whether to depend on it.

The dependency direction is:

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

**Why a separate record and not just save `Kingdom` directly?**

JSON serialisation reflects on properties. `Kingdom` has interfaces, private fields, and a constructor that requires `IRandom` + `IClock`. `JsonSerializer` doesn't know what to do with any of that. A purpose-built DTO is *much* simpler — it has only what we care about, in the shape we want.

This is the **DTO pattern.** You'll see it everywhere: APIs, message queues, file formats. Always a separate small record at the boundary.

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

`JsonSerializer.Serialize(value, options)` — turns the record into JSON. `Deserialize<T>(json)` — turns JSON back into `T`. **For a `record`, that's everything.** No attributes, no manual mapping.

`WriteIndented = true` produces multi-line, human-readable JSON. Toggle to `false` once you stop reading the file by hand.

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

Build + run:

```powershell
dotnet build
dotnet run --project Kingdom.Console
```

Open `bin/Debug/net10.0/saves/kingdom.json` — beautiful, indented JSON. Notice you can **edit it by hand** in Notepad and it'll load back. That's the magic of plain text formats.

## Step 5 — tests

Decision time: do persistence tests live in the existing `Kingdom.Engine.Tests` project, or in a new `Kingdom.Persistence.Tests`?

- **Add to existing:** simpler. Tests reference both Engine and Persistence projects.
- **New project:** more proper — each library has its own tests. Mirrors what bigger codebases do.

We'll go with **new project** — it teaches the right pattern. From the repo root:

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

Expect `Passed: 43` (38 from M2.1 + 5 new in the persistence test project).

## Tinker

- Set `WriteIndented = false`. Save a kingdom. The JSON is now one line. **Same data, much smaller** — that's what gets sent over the network.
- Add a property to `KingdomSummary` — e.g., `int FarmCount`. Run the test that loads an old JSON file (without `FarmCount`). It loads cleanly with `FarmCount = 0` (the default). **JSON tolerates missing fields.**
- Add `[JsonPropertyName("name")]` above `Name` in the record. Now JSON output is lowercase `"name"`. Useful when matching an existing API.
- Try saving 1000 kingdoms in a loop with different names. Filesystem starts to feel slow. **That's the boundary where you reach for a database.**

## Name it

- **JSON.** Plain text format using `{}` and `[]`. Universal cross-language data format.
- **`System.Text.Json`.** Modern .NET JSON library. Use this; the older `Newtonsoft.Json` is fine but not the default any more.
- **`JsonSerializer.Serialize` / `Deserialize<T>`.** Two static methods. That's the API.
- **DTO (Data Transfer Object).** A small purpose-built record for crossing a boundary. Separate from your domain model on purpose.
- **`WriteIndented`.** `JsonSerializerOptions` flag for human-readable vs compact JSON.

## The rule of the through-line

> **Use a DTO at the boundary.** Don't serialise your domain model directly. The shape on the wire/disk and the shape in memory are *different concerns* — let them differ.

This rule will repeat in Phase 3 (HTTP request/response DTOs distinct from the engine), Phase 4 (browser-side TypeScript types distinct from C#), and Phase 5 (Roblox tables distinct from the C# engine).

## Quiz / challenge

Open `quiz.md`.

## Connect

Module 2.3 — **round-trip tests** — generalises: any kingdom we save should equal itself when loaded. We'll write the *full* snapshot (not just the summary) and prove the engine state can be reconstructed exactly.
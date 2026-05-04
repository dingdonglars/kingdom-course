# Module 2.1 — File I/O

> **Hook:** until now, your kingdom dies the moment you close the program. Today you write its first save: a plain text file on disk. Tomorrow we'll make that JSON; the day after, SQLite. But first — paths, encoding, the *very specific* way `File.WriteAllText` deals with line endings on Windows. You'll never confuse `C:\foo` with `C:/foo` again.

> **Words to watch**
> - **path** — the address of a file on disk: `C:\Users\Athos\save.txt` or, written portably, `Path.Combine(...)`
> - **absolute vs relative** — `C:\foo\bar.txt` is absolute; `bar.txt` is relative (to the program's working directory)
> - **encoding** — how text becomes bytes. UTF-8 is the answer for ~everything written this decade.
> - **`File.WriteAllText`** / **`File.ReadAllText`** — the two simplest ways to put a string on disk and get it back.

---

## Why a file first

Files are the simplest persistence: open, write a string, close. **Nothing is faster to add and nothing is more familiar to debug** — you can open the file in Notepad and see it. JSON (Module 2.2) and SQLite (Module 2.4) are layers on top of "write some bytes to disk." Start with the raw thing.

This module *doesn't change the engine yet.* All the file work happens in `Kingdom.Console/Program.cs`. The engine still doesn't know that disk exists. Tomorrow we'll change that — but by then we'll have the full picture.

## Step 1 — paths

In Windows, paths use backslashes. In .NET source code, backslashes inside a string are escape characters, so you either *double* them or use a *verbatim* string:

```csharp
string a = "C:\\Users\\Athos\\save.txt";       // double
string b = @"C:\Users\Athos\save.txt";          // verbatim @"..." — what you'll usually see
```

But hard-coding paths is brittle. Use `Path.Combine` so your code works on any OS:

```csharp
using System.IO;

var saveFolder = Path.Combine(AppContext.BaseDirectory, "saves");
var savePath   = Path.Combine(saveFolder, "kingdom.txt");
```

`AppContext.BaseDirectory` is *"the folder where the .exe is running from."* Combine with `"saves"` and you get a folder next to the program — predictable and clean.

## Step 2 — write it, read it

```csharp
using System.IO;
using Kingdom.Engine;
using Kingdom.Engine.Buildings;
using Kingdom.Engine.Citizens;
using Kingdom.Engine.Infrastructure;

IRandom rng = new SystemRandom(seed: 42);
var kingdom = new Kingdom.Engine.Kingdom("Eldoria", rng, new SystemClock());
kingdom.AddBuilding(new Farm("Main Farm"));
kingdom.AddCitizen(new Citizen("Lyra"));
for (int i = 0; i < 10; i++) kingdom.AdvanceDay();

var saveFolder = Path.Combine(AppContext.BaseDirectory, "saves");
Directory.CreateDirectory(saveFolder);                       // no-op if it exists
var savePath = Path.Combine(saveFolder, "kingdom.txt");

// Build a tiny human-readable snapshot
var lines = new List<string>
{
    $"Name: {kingdom.Name}",
    $"Day: {kingdom.Day}",
    $"Buildings: {kingdom.Buildings.Count}",
    $"Citizens: {kingdom.Citizens.Count}",
    $"Gold: {kingdom.Resources.Get(Resource.Gold)}"
};

File.WriteAllText(savePath, string.Join('\n', lines));
Console.WriteLine($"Saved to {savePath}");

// Now read it back
var loaded = File.ReadAllText(savePath);
Console.WriteLine();
Console.WriteLine("=== File contents ===");
Console.WriteLine(loaded);
```

Run:

```powershell
dotnet run --project Kingdom.Console
```

Open `bin/Debug/net10.0/saves/kingdom.txt` in Notepad. There it is — text on disk. **Close the program. Reopen it. The file is still there.** Tiny step, big idea.

## Step 3 — encoding pitfall (one-time pain)

`File.WriteAllText(path, text)` writes UTF-8 *without a BOM* (byte-order mark) by default in modern .NET. That's the right choice for ~everything. If you want to be explicit:

```csharp
File.WriteAllText(savePath, contents, System.Text.Encoding.UTF8);
```

If a file ever opens in another tool with a strange `` (BOM-mangled) at the start, encoding mismatch is the cause. **Always UTF-8 unless you're talking to a legacy system.**

## Step 4 — round-trip what you wrote

A real test: write something, read it back, assert they're equal.

`tests/Kingdom.Engine.Tests/FileIOTests.cs` (new):

```csharp
namespace Kingdom.Engine.Tests;

public class FileIOTests
{
    [Fact]
    public void WriteAllText_ThenReadAllText_RoundtripsExactly()
    {
        var path = Path.Combine(Path.GetTempPath(), $"kingdom-test-{Guid.NewGuid():N}.txt");
        var original = "Line one\nLine two\nLine three";
        try
        {
            File.WriteAllText(path, original);
            var roundtripped = File.ReadAllText(path);
            roundtripped.ShouldBe(original);
        }
        finally
        {
            if (File.Exists(path)) File.Delete(path);
        }
    }

    [Fact]
    public void Path_Combine_HandlesTrailingSeparators()
    {
        // Path.Combine is forgiving about trailing slashes
        Path.Combine("a", "b").ShouldBe("a" + Path.DirectorySeparatorChar + "b");
        Path.Combine("a/", "b").ShouldBe("a/b");
    }

    [Fact]
    public void Directory_CreateDirectory_IsIdempotent()
    {
        var dir = Path.Combine(Path.GetTempPath(), $"kingdom-test-{Guid.NewGuid():N}");
        try
        {
            Directory.CreateDirectory(dir);
            Directory.CreateDirectory(dir);   // second call doesn't throw
            Directory.Exists(dir).ShouldBeTrue();
        }
        finally
        {
            if (Directory.Exists(dir)) Directory.Delete(dir);
        }
    }
}
```

Three tests. **`try / finally` to clean up** — never leave temp files around in tests. Run:

```powershell
dotnet test
```

Expect `Passed: 38` (35 + 3).

## Tinker

- Try saving a different file every day: `kingdom-day-{kingdom.Day}.txt`. Now your `saves/` folder fills up. Look at it after 30 days.
- Try `File.AppendAllText(path, text)` — appends to the file instead of overwriting. Use it to log every event to `events.log`.
- Try writing 1 million lines: `File.WriteAllText(path, string.Join('\n', Enumerable.Range(1, 1_000_000)))`. Open the file in Notepad. Notepad will struggle. **That's the file size limit you'll hit before you reach for a database.**
- Comment out `Directory.CreateDirectory(saveFolder)`. Run again. **`DirectoryNotFoundException`.** That's why we create-or-skip first.

## Name it

- **path** — a file's address on disk. Build with `Path.Combine` for portability.
- **absolute vs relative** — `C:\foo\bar.txt` is absolute; `bar.txt` resolves to the program's working directory.
- **`File.WriteAllText` / `File.ReadAllText`** — the two simplest persistence calls. UTF-8 by default.
- **`Directory.CreateDirectory`** — make a folder; idempotent (safe to call when it already exists).
- **`Path.GetTempPath()`** — system temp folder. Where tests should write throwaway files.

## The rule of the through-line

> **The engine never knows about disk.** Files are a *shell* concern (today: console). The engine produces and consumes plain values; the shell decides where they live.

Notice that `Kingdom.Engine/` has *zero* changes this module. That's the proof.

## Quiz / challenge

Open `quiz.md`.

## Connect

Module 2.2 introduces **JSON serialisation** — instead of writing 5 lines of human-readable text, we serialise the entire kingdom as JSON and roundtrip it back to a real `Kingdom` object. That's where persistence stops being a toy.
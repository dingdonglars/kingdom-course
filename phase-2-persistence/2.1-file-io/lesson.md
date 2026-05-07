# Module 2.1 — File I/O

Until now, your kingdom dies the moment you close the program. Today we change that. By the end of this module, your kingdom writes itself to a plain text file on disk; tomorrow we'll make that JSON; the day after, SQLite. But you have to start with the simplest thing: open a file, write some text, close it. The first time you watch a file appear and survive a program restart, the trick of *persistence* stops feeling like a trick.

Along the way you'll meet the small but very specific way Windows deals with paths and line endings. After today you'll never confuse `C:\foo` with `C:/foo` again.

> **Words to watch**
>
> - **path** — a file's address on disk: `C:\Users\Athos\save.txt`, or written portably with `Path.Combine(...)`
> - **absolute vs relative** — `C:\foo\bar.txt` is absolute; `bar.txt` is relative (to the program's working directory)
> - **encoding** — how text becomes bytes. UTF-8 is the answer for almost everything written this decade.
> - **`File.WriteAllText`** / **`File.ReadAllText`** — the two simplest ways to put a string on disk and get it back.

---

## Phase opener — `phase-2` branch

Before any code (the why is in Module 1.1):

```powershell
cd C:\code\kingdom
git switch -c phase-2
```

Every commit this phase lands on `phase-2`. At Module 2.11 (M3 close), you'll PR it back to `main`.

---

## Why a file first

Files are the simplest way to save: open, write a string, close. Nothing is faster to add and nothing is more familiar to debug — you can open the file in Notepad and read what's inside. JSON (Module 2.2) and SQLite (Module 2.4) are layers on top of *"write some bytes to disk."* So we start with the raw thing.

This module *doesn't change the engine yet.* All the file work happens in `Kingdom.Console/Program.cs`. The engine still doesn't know that disk exists. Tomorrow we'll change that — but only after you've seen the full picture.

## Step 1 — paths

In Windows, paths use backslashes. In .NET source code, a backslash inside a string is an escape character, so you either *double* it or use a *verbatim* string:

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

`AppContext.BaseDirectory` is *"the folder where the .exe is running from."* Combine that with `"saves"` and you get a folder next to the program — predictable, clean, and the same on Windows or Mac.

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

Open `bin/Debug/net10.0/saves/kingdom.txt` in Notepad. There it is — text on disk. Close the program. Reopen it. The file is still there. Tiny step, big idea.

## Step 3 — encoding (one-time pain)

`File.WriteAllText(path, text)` writes UTF-8 *without a BOM* (byte-order mark) by default in modern .NET. That's the right choice for almost everything. If you ever want to be explicit:

```csharp
File.WriteAllText(savePath, contents, System.Text.Encoding.UTF8);
```

If a file ever opens in another tool with a strange `` (BOM-mangled) at the start, encoding mismatch is the cause. Always UTF-8 unless you're talking to a legacy system that demands something else.

## Step 4 — round-trip what you wrote

A real test: write something, read it back, check they're equal.

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

Three tests. The `try / finally` blocks clean up — never leave temp files lying around in tests. Run:

```powershell
dotnet test
```

Expect `Passed: 38` (35 + 3).

## Tinker

Try saving a different file every day: `kingdom-day-{kingdom.Day}.txt`. Now your `saves/` folder fills up. Look at it after thirty days — that's a tiny event log.

Try `File.AppendAllText(path, text)` — it adds to the end of a file instead of overwriting. Use it to log every event to `events.log` and watch the log grow.

Try writing one million lines: `File.WriteAllText(path, string.Join('\n', Enumerable.Range(1, 1_000_000)))`. Open the file in Notepad. Notepad will struggle. That's the file size where you start reaching for a database.

Comment out `Directory.CreateDirectory(saveFolder)`. Run again. You'll get `DirectoryNotFoundException`. That's why we create-or-skip first.

## What you just did

Your kingdom went from a program that prints to a program that *saves*. You wrote a tiny snapshot to a real file on disk, read it back, and confirmed the round trip with three tests — total now is 38 passing. You also met two facts about Windows that will save you hours later: paths use backslashes (which need either doubling or `@"..."`), and modern .NET writes UTF-8 without a BOM, which is what you want every time. The interesting thing is what you *didn't* change: the engine has zero edits this module. Disk is a shell concern; the engine doesn't know about it.

**Key concepts you can now name:**

- **path** — a file's address; build it with `Path.Combine`
- **absolute vs relative** — full address vs *"relative to where I'm running"*
- **`File.WriteAllText` / `ReadAllText`** — the simplest persistence
- **`Directory.CreateDirectory`** — make a folder, idempotent
- **engine vs shell** — disk lives in the shell; engine stays clean

## Git move of the week — `.gitignore`

You started writing files to disk this module. Some files belong in git (your code, your `.csproj`, the test files). Some don't (the build outputs in `bin/` and `obj/`, user secrets, `.env` files, OS junk like `.DS_Store`).

Your `kingdom` repo already has a `.gitignore` from the day-1 kit — open it at the repo root. Each line is a pattern of files git should *ignore*. When you create a new file matching one of those patterns, the Source Control panel quietly skips it; it never shows up in *Changes*.

If you're confused why VS Code isn't offering a file you expected: check `.gitignore`. Most *"git is being weird about this file"* moments are this.

> **Or in the terminal:** `git status --ignored` lists what git is currently ignoring — useful when you want to be sure.

## Quiz

Open `quiz.md`. When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.

## Next

Module 2.2 introduces **JSON serialisation** — instead of writing five lines of human-readable text, we serialise the entire kingdom as JSON and load it back into a real `Kingdom` object. That's where saving stops being a toy.

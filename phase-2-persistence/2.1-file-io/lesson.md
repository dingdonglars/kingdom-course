# Module 2.1 — File I/O

> Travou no inglês? Abra o `lesson.pt.md` — é esta mesma lição em português. Tente em inglês primeiro.

Until now, your kingdom is gone the moment you close the program. Today we change that. By the end of this module, your kingdom writes itself to a plain text file on disk. Tomorrow we'll make that JSON. The day after, SQLite. But we start with the simplest thing: open a file, write some text, close it.

Saving your work so it stays after the program ends is called *persistence*. The first time you see a file appear, close the program, and find the file still there, persistence will start to make sense.

Here's the whole idea of this phase in one picture:

```text
   IN MEMORY (while running)              ON DISK (a file)
   -------------------------             -------------------
   your Kingdom object          save      save.txt
   gold 100, wood 50, ...      ------->   "Eldoria 100 50 20 30"
                                          |
   vanishes the moment          load      | still there after you close,
   the program closes         <-------     reboot, and come back tomorrow
```

Everything so far has lived only in the left column — gone the instant the program stops. Phase 2 is about the arrows: writing the kingdom *out* to disk, and reading it back *in* next time. Today's file is the simplest version of that; JSON, then SQLite, then a real database are the same two arrows done better.

Along the way you'll learn the exact way Windows handles file paths and line endings. After today you'll know the difference between `C:\foo` and `C:/foo`.

> **Words to watch**
>
> - **path** — a file's address on disk: `C:\code\kingdom\save.txt`, or written portably with `Path.Combine(...)`
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

Every commit this phase goes on the `phase-2` branch. At Module 2.11 (the M3 milestone), you'll open a PR to merge it back into `main`.

---

## Why a file first

Files are the simplest way to save: open, write a string, close. Nothing is faster to add, and nothing is easier to check — you can open the file in Notepad and read what's inside. JSON (Module 2.2) and SQLite (Module 2.4) both build on the same basic idea: write some bytes to disk. So we start with that basic idea.

This module *doesn't change the engine yet.* All the file work happens in `Kingdom.Console/Program.cs`. The engine still doesn't know the disk exists. Tomorrow we'll change that — but only after you've seen how it all fits together.

## Step 1 — paths

In Windows, file paths use backslashes. But in C# code, a backslash inside a string is a special character. So you have two choices: write it *twice*, or put an `@` in front of the string (this is called a *verbatim* string):

```csharp
string a = "C:\\code\\kingdom\\save.txt";       // double
string b = @"C:\code\kingdom\save.txt";          // verbatim @"..." — what you'll usually see
```

But writing the full path by hand is risky — it breaks easily. Use `Path.Combine` so your code works on any operating system:

```csharp
using System.IO;

var saveFolder = Path.Combine(AppContext.BaseDirectory, "saves");
var savePath   = Path.Combine(saveFolder, "kingdom.txt");
```

`AppContext.BaseDirectory` is the folder where the program is running from. Combine that with `"saves"` and you get a folder right next to the program. It works the same way on Windows or Mac, and you always know where it is.

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

Open `bin/Debug/net10.0/saves/kingdom.txt` in Notepad. There it is — your text on disk. Close the program. Open it again. The file is still there. It's a small step, but it's a big idea.

## Step 3 — encoding (learn it once)

*Encoding* is how text gets turned into bytes for storing. `File.WriteAllText(path, text)` uses an encoding called UTF-8 *without a BOM* (byte-order mark) by default in modern .NET. That's the right choice for almost everything. If you ever want to say it out loud in your code:

```csharp
File.WriteAllText(savePath, contents, System.Text.Encoding.UTF8);
```

If a file ever opens in another tool with a strange `` at the start, the encoding is the cause — the two tools disagree on how to read the bytes. Always use UTF-8, unless you're working with an old system that needs something else.

## Step 4 — round-trip what you wrote

Here's a real test: write something, read it back, and check that the two match.

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

Three tests. The `try / finally` blocks delete the temp files at the end — a test should never leave files behind on disk. Run:

```powershell
dotnet test
```

Expect `Passed: 38` (35 + 3).

## Tinker

Try saving a different file every day: `kingdom-day-{kingdom.Day}.txt`. Now your `saves/` folder fills up with files. Look at it after thirty days — you have a small record of every day.

Try `File.AppendAllText(path, text)`. It adds to the end of a file instead of replacing what's there. Use it to write every event to `events.log` and watch the file grow.

Try writing one million lines: `File.WriteAllText(path, string.Join('\n', Enumerable.Range(1, 1_000_000)))`. Open the file in Notepad. Notepad will be slow. That's the file size where a database starts to make sense instead.

Delete the line `Directory.CreateDirectory(saveFolder)`, or turn it into a comment. Run again. You'll get a `DirectoryNotFoundException` error. That's why we make the folder first if it isn't there yet.

## What you just did

Your kingdom went from a program that only prints to a program that *saves*. You wrote a small snapshot to a real file on disk, read it back, and checked the round trip with three tests — your total is now 38 passing. You also learned two facts about Windows that will save you hours later: paths use backslashes (which need either doubling or `@"..."`), and modern .NET writes UTF-8 without a BOM, which is what you want every time. The interesting part is what you *didn't* change: the engine has no edits at all this module. The disk is the shell's job; the engine doesn't know about it.

**Key concepts you can now name:**

- **path** — a file's address; build it with `Path.Combine`
- **absolute vs relative** — full address vs *"relative to where I'm running"*
- **`File.WriteAllText` / `ReadAllText`** — the simplest persistence
- **`Directory.CreateDirectory`** — make a folder, idempotent
- **engine vs shell** — disk lives in the shell; engine stays clean

## On your own

Time to put the book away. Don't scroll back up to the steps — show yourself, from your own head, that the one big idea stuck: write a string to a file, then read it back. No one marks this — it's just for you. It's the fastest way to spot what hasn't stuck yet, while it's still small to fix. Getting stuck here is completely fine — that's exactly what it's for.

Open a new empty file. Without looking:

1. Build a save path with `Path.Combine`.
2. Make the folder.
3. Write the text `"Hello kingdom"` to a file.
4. Read the same file back and print what you read.
5. Run it — the text you print should match the text you wrote.

<details><summary>Stuck? Open this to check yourself.</summary>

```csharp
using System.IO;

var folder = Path.Combine(AppContext.BaseDirectory, "saves");
Directory.CreateDirectory(folder);
var path = Path.Combine(folder, "hello.txt");

File.WriteAllText(path, "Hello kingdom");

var back = File.ReadAllText(path);
Console.WriteLine(back);          // Hello kingdom
```

If the text you read matches the text you wrote, you have it. If you forgot `Directory.CreateDirectory`, you'll get a `DirectoryNotFoundException` — that's the lesson from the Tinker section, right on cue.

</details>

## Git move of the week — `.gitignore`

You started writing files to disk this module. Some files belong in git (your code, your `.csproj`, the test files). Some don't (the build outputs in `bin/` and `obj/`, secret keys, `.env` files, and extra files the operating system makes, like `.DS_Store`).

Your `kingdom` repo already has a `.gitignore` from the day-1 kit — open it at the repo root. Each line is a pattern for files git should *ignore*. When you make a new file that matches one of those patterns, the Source Control panel skips it. It never shows up in *Changes*.

If you can't figure out why VS Code isn't showing a file you expected, check `.gitignore`. Most of the time, that's the reason.

> **Or in the terminal:** `git status --ignored` lists every file git is ignoring right now — useful when you want to be sure.

## Wrap up

1. **Quiz** — open `quiz.md`, jot your answers in `journal/quiz-notes.md`.
2. **Progress** — one line in `journal/progress.md`: `Module 2.1 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit and push** — stage both files, commit message `Module 2.1 done`, Sync.
4. **Post in `#wins`** — one line about today, plus the URL of the commit.

Module 0.1 covers the why and the panel/CLI steps if you need a refresher. Bring quiz answers you're least sure about to the next weekly sync.

## Next

Module 2.2 introduces **JSON serialisation** — instead of writing five lines of plain text, we save the whole kingdom as JSON and load it back into a real `Kingdom` object. That's where saving becomes truly useful.

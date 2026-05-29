# Module 2.10 — Save-Slot UI (Interactive Console)

The demo `Program.cs` does the right things, but it's just a script — *open the program, it runs once, it's done*. Real games keep going in a loop. Today we put the CRUD operations from Module 2.9 behind an interactive menu — *"1. New, 2. Load, 3. Delete, 4. Quit"* — and at last have something you can play.

> **Words to watch**
>
> - **menu loop** — print options, read input, dispatch, repeat
> - **`Console.ReadLine`** — read a line of input from the user
> - **input validation** — handle the moment the user types `"banana"` instead of a number
> - **`switch` statement** (vs expression) — useful when each branch *does something* rather than returning a value

---

## Why a real loop matters

A script `Program.cs` is fine for testing your code. But a *user* — even if that user is just you — needs to *interact* with it. Today's loop is your first interactive shell. You'll see the same pattern at every level:

- Console: `while (true) { print menu; read input; act; }`
- Web API: `while (true) { receive request; route; act; respond; }`
- Browser: `eventListener('click', () => act())` (driven by events, but the same idea)

The form changes, but the core idea stays the same.

## Delta starter

- **NEW:** `Kingdom.Console/SaveSlotUI.cs` — the menu + handlers
- **MODIFIED:** `Kingdom.Console/Program.cs` — replaces the demo bottom-half with a single `SaveSlotUI.Run(...)` call
- **NEW:** `tests/Kingdom.Persistence.Tests/SaveSlotUITests.cs` — uses `Console.SetIn` / `Console.SetOut` to drive the UI in a test

## Step 1 — `SaveSlotUI`

`Kingdom.Console/SaveSlotUI.cs`:

```csharp
using Kingdom.Engine;
using Kingdom.Engine.Buildings;
using Kingdom.Engine.Citizens;
using Kingdom.Engine.Infrastructure;
using Kingdom.Persistence.EfCore;

namespace Kingdom.Console;

public static class SaveSlotUI
{
    public static void Run(KingdomEfStore store, IRandom rng, IClock clock)
    {
        store.EnsureCreated();

        while (true)
        {
            System.Console.WriteLine();
            System.Console.WriteLine("=== Kingdom — Save Slots ===");
            System.Console.WriteLine("  1. New kingdom");
            System.Console.WriteLine("  2. Load existing");
            System.Console.WriteLine("  3. Delete a slot");
            System.Console.WriteLine("  4. Quit");
            System.Console.Write("> ");

            var line = System.Console.ReadLine();
            if (line is null) return;   // EOF (input redirected and ran out)

            switch (line.Trim())
            {
                case "1":  NewKingdom(store, rng, clock); break;
                case "2":  LoadKingdom(store, rng, clock); break;
                case "3":  DeleteSlot(store); break;
                case "4":  return;
                default:   System.Console.WriteLine("Pick 1, 2, 3, or 4."); break;
            }
        }
    }

    private static void NewKingdom(KingdomEfStore store, IRandom rng, IClock clock)
    {
        System.Console.Write("Name: ");
        var name = (System.Console.ReadLine() ?? "").Trim();
        if (string.IsNullOrEmpty(name)) { System.Console.WriteLine("Cancelled."); return; }

        var k = new global::Kingdom.Engine.Kingdom(name, rng, clock);
        k.AddBuilding(new Farm("Main Farm"));
        k.AddCitizen(new Citizen("Lyra"));
        var id = store.Save(k);
        System.Console.WriteLine($"Created '{name}' as slot #{id}.");
        PlayLoop(store, id, k);
    }

    private static void LoadKingdom(KingdomEfStore store, IRandom rng, IClock clock)
    {
        var slots = store.ListSlots();
        if (slots.Count == 0) { System.Console.WriteLine("No saves yet."); return; }
        ShowSlots(slots);

        System.Console.Write("Slot id (or blank to cancel): ");
        var raw = (System.Console.ReadLine() ?? "").Trim();
        if (!int.TryParse(raw, out var id)) { System.Console.WriteLine("Not a number."); return; }
        if (slots.All(s => s.Id != id)) { System.Console.WriteLine($"No slot with id {id}."); return; }

        var k = store.Load(id, rng, clock);
        System.Console.WriteLine($"Loaded #{id} '{k.Name}' at day {k.Day}.");
        PlayLoop(store, id, k);
    }

    private static void DeleteSlot(KingdomEfStore store)
    {
        var slots = store.ListSlots();
        if (slots.Count == 0) { System.Console.WriteLine("No saves to delete."); return; }
        ShowSlots(slots);

        System.Console.Write("Slot id to delete: ");
        var raw = (System.Console.ReadLine() ?? "").Trim();
        if (!int.TryParse(raw, out var id)) { System.Console.WriteLine("Not a number."); return; }
        store.Delete(id);
        System.Console.WriteLine($"Deleted #{id}.");
    }

    private static void PlayLoop(KingdomEfStore store, int id, Kingdom.Engine.Kingdom k)
    {
        while (true)
        {
            System.Console.WriteLine();
            System.Console.WriteLine($"--- {k.Name} day {k.Day} ---");
            System.Console.WriteLine("  a. Advance 1 day   d. Advance 10 days   s. Save & exit slot");
            System.Console.Write("> ");
            var c = (System.Console.ReadLine() ?? "").Trim();
            switch (c)
            {
                case "a": k.AdvanceDay(); break;
                case "d": for (int i = 0; i < 10; i++) k.AdvanceDay(); break;
                case "s": store.Update(id, k); System.Console.WriteLine($"Saved #{id} at day {k.Day}."); return;
                default:  System.Console.WriteLine("Pick a, d, or s."); break;
            }
        }
    }

    private static void ShowSlots(IReadOnlyList<KingdomSlotInfo> slots)
    {
        System.Console.WriteLine("Saved kingdoms:");
        foreach (var s in slots)
            System.Console.WriteLine($"  #{s.Id,-3} {s.Name,-20} day {s.Day}");
    }
}
```

Read this carefully. Every method does one thing. The `switch` in `Run` is the part that decides what happens next, based on what the user typed. Each handler then calls the store and comes back.

Why `System.Console.WriteLine` instead of just `Console.WriteLine`? Our class lives in `namespace Kingdom.Console`, and that name clashes with the `System.Console` type. Writing the full `System.Console` clears up which one we mean.

## Step 2 — `Program.cs` becomes a one-liner

Replace `Program.cs` with:

```csharp
using Kingdom.Console;
using Kingdom.Engine.Infrastructure;
using Kingdom.Persistence.EfCore;

var saveFolder = Path.Combine(AppContext.BaseDirectory, "saves");
Directory.CreateDirectory(saveFolder);
var dbPath = Path.Combine(saveFolder, "kingdoms-ef.db");

IRandom rng = new SystemRandom();
IClock clock = new SystemClock();

var store = new KingdomEfStore(dbPath);
SaveSlotUI.Run(store, rng, clock);
```

That's the whole shell. Read input, decide what to do, save. The engine and the persistence layer do the real work.

Build and run:

```powershell
dotnet build
dotnet run --project Kingdom.Console
```

You're now in your kingdom's first real game loop. Make a kingdom, advance a few days, save, quit, start again, and load. The kingdom remembers.

## Step 3 — testing an interactive UI

For a test, we need to *feed in* the user's input ahead of time. .NET makes this easy: you redirect `Console.In` and `Console.Out` so the test reads from a string and writes to a string.

`tests/Kingdom.Persistence.Tests/SaveSlotUITests.cs`:

```csharp
using Kingdom.Console;
using Kingdom.Engine;
using Kingdom.Engine.Infrastructure;
using Kingdom.Persistence.EfCore;
using Shouldly;

namespace Kingdom.Persistence.Tests;

public class SaveSlotUITests
{
    [Fact]
    public void Run_Quit_ExitsImmediately()
    {
        var path = Path.Combine(Path.GetTempPath(), $"ui-{Guid.NewGuid():N}.db");
        try
        {
            using var input = new StringReader("4\n");           // quit
            using var output = new StringWriter();
            System.Console.SetIn(input);
            System.Console.SetOut(output);

            SaveSlotUI.Run(new KingdomEfStore(path), new SystemRandom(0), new SystemClock());

            output.ToString().ShouldContain("Quit");
        }
        finally
        {
            ResetConsole();
            if (File.Exists(path)) File.Delete(path);
        }
    }

    [Fact]
    public void Run_NewKingdom_ThenSaveExit_PersistsTheSlot()
    {
        var path = Path.Combine(Path.GetTempPath(), $"ui-{Guid.NewGuid():N}.db");
        try
        {
            // 1 = new, "Test" = name, a = advance, s = save+exit, 4 = quit
            using var input = new StringReader("1\nTest\na\ns\n4\n");
            using var output = new StringWriter();
            System.Console.SetIn(input);
            System.Console.SetOut(output);

            var store = new KingdomEfStore(path);
            SaveSlotUI.Run(store, new SystemRandom(0), new SystemClock());

            store.ListSlots().Count.ShouldBe(1);
            store.ListSlots()[0].Name.ShouldBe("Test");
            store.ListSlots()[0].Day.ShouldBe(2);   // started at 1, advanced once
        }
        finally
        {
            ResetConsole();
            if (File.Exists(path)) File.Delete(path);
        }
    }

    [Fact]
    public void Run_BadMenuPick_ShowsHelp_AndContinues()
    {
        var path = Path.Combine(Path.GetTempPath(), $"ui-{Guid.NewGuid():N}.db");
        try
        {
            using var input = new StringReader("banana\n4\n");
            using var output = new StringWriter();
            System.Console.SetIn(input);
            System.Console.SetOut(output);

            SaveSlotUI.Run(new KingdomEfStore(path), new SystemRandom(0), new SystemClock());

            output.ToString().ShouldContain("Pick 1, 2, 3, or 4");
        }
        finally
        {
            ResetConsole();
            if (File.Exists(path)) File.Delete(path);
        }
    }

    private static void ResetConsole()
    {
        // Reattach the real console — important so other tests aren't poisoned
        var stdOut = new StreamWriter(System.Console.OpenStandardOutput()) { AutoFlush = true };
        System.Console.SetOut(stdOut);
        System.Console.SetIn(new StreamReader(System.Console.OpenStandardInput()));
    }
}
```

These tests don't try to cover everything — they're quick checks of the basics. Three things you really want to be sure of:

- Quit works
- A full create → play → save → quit cycle keeps the data
- Bad input doesn't crash the loop

(The first test checks that *"Quit"* appears in the output. This works because typing `4` simply returns from `Run`. If you want a stronger check, make sure the menu doesn't appear *twice*.)

Run:

```powershell
dotnet test
```

Expect `Passed: 71` (68 + 3).

## Tinker

Add a `5. Quick stats` menu option that prints the total number of saves, plus the oldest and the richest. One LINQ query inside the handler.

Add a rename option to the play loop. Read a new name, then `k.Name = newName`... wait, `Name` is read-only. Add a `Rename(string)` method to the engine. Saving made you change the model back in Module 2.3, and now the user interface makes you change it again.

Try running the program with input coming from a file: `dotnet run --project Kingdom.Console < script.txt`. Whatever is in `script.txt` becomes the input. Any console UI can be run from a file like this.

Replace `IsNullOrEmpty` with `string.IsNullOrWhiteSpace(name)`. Now a name of `"   "` (just spaces) also fails the check. Always check for spaces too, not just for an empty string.

## What you just did

You put your save-slot CRUD behind a real interactive menu. `Program.cs` shrank from a long demo script to about five lines — set up the store, then hand control to `SaveSlotUI.Run`. The UI loop reads input and sends it to the right handler, and each handler does just what you'd expect. Three new tests feed in the input through `Console.SetIn`/`SetOut` (71 passing total): quit works, a full cycle keeps the data, and bad input recovers. From here on, the kingdom is something you can sit down and play across more than one session.

**Key concepts you can now name:**

- **menu loop** — print, read, dispatch, repeat
- **`Console.ReadLine`** — read a line, returns `null` at EOF
- **`int.TryParse`** — safe number parsing without exceptions
- **`Console.SetIn` / `SetOut`** — script and capture in tests
- **EOF handling** — return cleanly when input runs out

## On your own

Time to put the book away. Don't scroll back up to the steps — show yourself, from your own head, that the one big idea stuck: a menu loop prints the options, reads a line, decides what to do, and repeats. No one marks this — it's just for you. It's the fastest way to spot what hasn't stuck yet, while it's still small to fix. Getting stuck here is completely fine — that's exactly what it's for.

Open a new empty file. Without looking, write a small menu loop: print three options (`1. Say hi`, `2. Count up`, `3. Quit`), read a line, and use a `switch` to act — option 1 prints a greeting, option 2 prints a number that goes up each time, option 3 returns out of the loop. Anything else prints a short "pick 1, 2, or 3" line. Run it and click through your own menu.

<details><summary>Stuck? Open this to check yourself.</summary>

```csharp
int count = 0;
while (true)
{
    Console.WriteLine("1. Say hi   2. Count up   3. Quit");
    Console.Write("> ");
    var line = Console.ReadLine();
    if (line is null) return;          // input ran out

    switch (line.Trim())
    {
        case "1": Console.WriteLine("Hi!"); break;
        case "2": Console.WriteLine(++count); break;
        case "3": return;
        default:  Console.WriteLine("Pick 1, 2, or 3."); break;
    }
}
```

The shape is always the same: `while (true)` around print → read → `switch`, with one branch that `return`s to leave. The `if (line is null)` guard handles the moment input runs out, so the loop never spins forever.

</details>

## Git move of the week — merge vs rebase (preview)

By now your `git log` is full of commits. There are two ways to bring work from one branch into another, and they leave different histories:

- **Merge** keeps the picture of two branches running side by side, joined by a *merge commit*. It's honest about how the work really happened.
- **Rebase** replays your commits on top of another branch. It gives them new SHAs (the SHA is the unique fingerprint git uses to identify each commit) and a single straight line of history. It reads more cleanly, but you lose the side-by-side picture.

In VS Code's Source Control panel: `...` menu → *Branch → Merge from* (or *Rebase from*). Pick the source branch.

> **Or in the terminal:**
>
> ```powershell
> git switch main && git merge feature/save-slots     # merge
> git switch feature/save-slots && git rebase main    # rebase
> ```

A good rule to follow: **rebase your own branches that you haven't pushed yet** to tidy them up before merging. **Merge** when the work goes into a shared branch like `main`. **Never rebase** a branch that other people might have already pulled — your commits get new SHAs but theirs don't, and the next `git pull` gets confused.

We cover both moves properly in B3.2 if you take that bonus.

## Wrap up

1. **Quiz** — open `quiz.md`, jot your answers in `journal/quiz-notes.md`.
2. **Progress** — one line in `journal/progress.md`: `Module 2.10 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit and push** — stage both files, commit message `Module 2.10 done`, Sync.
4. **Post in `#wins`** — one line about today, plus the URL of the commit.

Module 0.1 covers the why and the panel/CLI steps if you need a refresher. Bring quiz answers you're least sure about to the next weekly sync.

## Next

Module 2.11 closes Phase 2: **Names That Earn Their Keep** — a careful pass over the names of everything we've built this phase. It's what turns *"good code"* into *"good code anyone can read six months later."*

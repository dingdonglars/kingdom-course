# Module 2.10 — Save-Slot UI (Interactive Console)

The demo `Program.cs` does the right things, but it's a script — *open program → it runs → done*. Real games loop. Today we wrap the CRUD operations from Module 2.9 in an interactive menu — *"1. New, 2. Load, 3. Delete, 4. Quit"* — and finally have something playable.

> **Words to watch**
>
> - **menu loop** — print options, read input, dispatch, repeat
> - **`Console.ReadLine`** — read a line of input from the user
> - **input validation** — handle the moment the user types `"banana"` instead of a number
> - **`switch` statement** (vs expression) — useful when each branch *does something* rather than returning a value

---

## Why a real loop matters

A script `Program.cs` is fine for testing your code. But a *user* — even an audience of one (you) — needs to *interact*. Today's loop is your first interactive runtime. The pattern repeats at every layer:

- Console: `while (true) { print menu; read input; act; }`
- Web API: `while (true) { receive request; route; act; respond; }`
- Browser: `eventListener('click', () => act())` (event-driven; same idea)

The form changes; the heart is the same.

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

Read this carefully. Every method does one thing. The dispatching `switch` in `Run` is the central nervous system. The handlers call out to the store and back.

`System.Console.WriteLine` (instead of just `Console.WriteLine`) is a workaround — our class lives in `namespace Kingdom.Console`, which collides with the `System.Console` type name. Use the explicit `System.Console`.

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

That's the whole runtime. Read input, dispatch, save. Engine and persistence do the actual work.

Build and run:

```powershell
dotnet build
dotnet run --project Kingdom.Console
```

You're now in your kingdom's first real game loop. New a kingdom, advance some days, save, quit, restart, load. The kingdom has memory.

## Step 3 — testing an interactive UI

Tests need to *script* the user input. .NET makes this easy: redirect `Console.In` and `Console.Out`.

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

The tests don't try to be exhaustive — they're *sanity* tests. Three things you really want to know:

- Quit works
- A full create → play → save → quit cycle persists
- Bad input doesn't crash the loop

(The first test asserts *"Quit"* appears in output; this works because `4` simply returns from `Run` — but if you want stronger evidence, assert that the menu doesn't appear *twice*.)

Run:

```powershell
dotnet test
```

Expect `Passed: 71` (68 + 3).

## Tinker

Add a `5. Quick stats` menu option that prints total saves, oldest, richest. One LINQ query inside the handler.

Add a renaming option to the play loop — read a new name, `k.Name = newName`... wait, `Name` is read-only. Add a method `Rename(string)` to the engine. Saving forced a model change in Module 2.3; UX forces another one here.

Try running the program with input redirected from a file: `dotnet run --project Kingdom.Console < script.txt`. Whatever's in `script.txt` becomes the input. Any console UI is automatable that way.

`string.IsNullOrWhiteSpace(name)` — replace `IsNullOrEmpty`. Now `"   "` also fails validation. Always check for whitespace, not just empty.

## What you just did

You wrapped your save-slot CRUD in a real interactive menu. `Program.cs` collapsed from a long demo script to about five lines — set up the store, hand control to `SaveSlotUI.Run`. The UI loop reads input, dispatches to handlers, and the handlers do exactly what you'd expect. Three new tests script the input through `Console.SetIn`/`SetOut` (71 passing total): quit-works, full-cycle-persists, bad-input-recovers. From here on, the kingdom is something you can actually sit down and play across sessions.

**Key concepts you can now name:**

- **menu loop** — print, read, dispatch, repeat
- **`Console.ReadLine`** — read a line, returns `null` at EOF
- **`int.TryParse`** — safe number parsing without exceptions
- **`Console.SetIn` / `SetOut`** — script and capture in tests
- **EOF handling** — return cleanly when input runs out

## Quiz

Open `quiz.md`. When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.

## Next

Module 2.11 closes Phase 2: **Names That Earn Their Keep** — a deliberate naming pass over everything we've built across this phase. The thing that turns *"good code"* into *"good code anyone can read in six months."*

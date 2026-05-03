# Module 0.8 — Errors, Debugging, and M1

> **Hook:** today you make Inventory Tool *not crash* when things go wrong, and you meet the VS Code debugger — the single most important tool for understanding code that isn't doing what you expect. Then you ship M1.

> **Words to watch**
> - **exception** — a runtime error; something the program couldn't handle and threw up its hands about
> - **`try / catch`** — the C# pattern for "try this; if it throws, do that instead"
> - **`throw`** — the keyword for raising your own exception
> - **debugger** — a tool that pauses your program at chosen points so you can see what's happening
> - **breakpoint** — a marked line where the debugger pauses
> - **call stack** — the chain of methods that called each other to get to where you are now

---

## Do it — exceptions and `try/catch`

Open the Inventory Tool you wrote in Module 0.7. Find the `load` case. As written, what happens if `inventory.txt` exists but is corrupt? Or if a line has `apple=banana` instead of `apple=2`?

The `int.TryParse` we used handles bad numbers gracefully. But other things can still go wrong:

- The file might exist but be **locked** by another program → `IOException`.
- The file path might be on a read-only drive → `UnauthorizedAccessException`.
- The user might type `add` with no argument → currently we silently add an item named `""` (empty string).

Replace your `Program.cs` with this hardened version (changes flagged with `// NEW`):

```csharp
// Inventory Tool — v2 (Module 0.8)
//
// Hardened with try/catch and input validation.

var inventory = new Dictionary<string, int>();
const string SaveFile = "inventory.txt";

Console.WriteLine("Inventory Tool. Type 'help' for commands.");

while (true)
{
    Console.Write("> ");
    var line = Console.ReadLine();
    if (line is null) break;
    line = line.Trim();
    if (line.Length == 0) continue;

    var parts = line.Split(' ', 2);
    var cmd = parts[0].ToLower();
    var arg = parts.Length > 1 ? parts[1].Trim() : "";

    try   // NEW: wrap everything so a bug in one command doesn't kill the program
    {
        switch (cmd)
        {
            case "add":
                if (string.IsNullOrEmpty(arg))   // NEW: validate
                {
                    Console.WriteLine("Usage: add <item>");
                    break;
                }
                inventory[arg] = inventory.GetValueOrDefault(arg, 0) + 1;
                Console.WriteLine($"Added: {arg} (now have {inventory[arg]})");
                break;

            case "remove":
                if (string.IsNullOrEmpty(arg))   // NEW
                {
                    Console.WriteLine("Usage: remove <item>");
                    break;
                }
                if (inventory.ContainsKey(arg) && inventory[arg] > 0)
                {
                    inventory[arg]--;
                    if (inventory[arg] == 0) inventory.Remove(arg);
                    Console.WriteLine($"Removed: {arg}");
                }
                else
                {
                    Console.WriteLine($"Not found: {arg}");
                }
                break;

            case "find":
                if (string.IsNullOrEmpty(arg))   // NEW
                {
                    Console.WriteLine("Usage: find <item>");
                    break;
                }
                if (inventory.ContainsKey(arg))
                    Console.WriteLine($"Found: {arg} (count: {inventory[arg]})");
                else
                    Console.WriteLine($"Not found: {arg}");
                break;

            case "list":
                if (inventory.Count == 0)
                {
                    Console.WriteLine("Empty.");
                }
                else
                {
                    Console.WriteLine($"You have:");
                    foreach (var (item, count) in inventory.OrderBy(kvp => kvp.Key))
                        Console.WriteLine($"  - {item} x{count}");
                }
                break;

            case "save":
                var savedLines = inventory.Select(kvp => $"{kvp.Key}={kvp.Value}");
                File.WriteAllText(SaveFile, string.Join("\n", savedLines));
                Console.WriteLine($"Saved {inventory.Count} item(s) to {SaveFile}.");
                break;

            case "load":
                if (!File.Exists(SaveFile))
                {
                    Console.WriteLine($"No save file at {SaveFile}.");
                    break;
                }
                inventory.Clear();
                int loaded = 0, skipped = 0;
                foreach (var l in File.ReadAllLines(SaveFile))
                {
                    var kv = l.Split('=', 2);
                    if (kv.Length == 2 && int.TryParse(kv[1], out var n) && n > 0)
                    {
                        inventory[kv[0]] = n;
                        loaded++;
                    }
                    else
                    {
                        skipped++;     // NEW: count bad lines instead of silently dropping
                    }
                }
                Console.WriteLine($"Loaded {loaded} item(s) from {SaveFile}." + (skipped > 0 ? $" Skipped {skipped} bad line(s)." : ""));
                break;

            case "help":
                Console.WriteLine("Commands: add <item>, remove <item>, find <item>, list, save, load, quit");
                break;

            case "quit":
            case "exit":
                Console.WriteLine("Bye.");
                return;

            default:
                Console.WriteLine($"Unknown command: {cmd}. Type 'help'.");
                break;
        }
    }
    catch (IOException ex)   // NEW: file is locked, drive full, etc.
    {
        Console.WriteLine($"File problem: {ex.Message}");
    }
    catch (UnauthorizedAccessException ex)   // NEW: permission denied
    {
        Console.WriteLine($"Permission problem: {ex.Message}");
    }
    catch (Exception ex)   // NEW: catch-all for anything we didn't anticipate
    {
        Console.WriteLine($"Unexpected error: {ex.Message}");
    }
}
```

Run again. Try `add` (no arg). Try `remove` (no arg). Try saving to a folder you don't have permission to write to (modify `SaveFile` temporarily to `"C:\\Windows\\inventory.txt"` if you want to see the catch fire). The program no longer crashes.

## Now — the debugger

Open `Program.cs` in VS Code. Click in the gutter (the empty space to the left of the line numbers) next to the line `inventory[arg] = inventory.GetValueOrDefault(arg, 0) + 1;`. A red dot appears. **That's a breakpoint.**

Press `F5` (or *Run → Start Debugging*). VS Code may prompt you to install the C# Dev Kit debugger if you haven't already. Confirm. The program runs, then pauses *at* your breakpoint when you type `add apple` — before that line executes.

The left panel shows:
- **Variables.** The current value of `arg` (`"apple"`), `inventory` (empty dictionary), `cmd` (`"add"`), etc.
- **Call stack.** Just `Program.<Main>$` (top-level) for now.

Hover your mouse over `arg` in the code. You see its value pop up.

Press `F10` (*step over*). The line executes; you advance to the next line. `inventory` now contains `{"apple": 1}`. **You watched the dictionary change.**

Press `F5` to continue. The program prints `Added: apple (now have 1)` and waits for your next command.

## Tinker with the debugger

- Set a breakpoint inside the `load` case. Run the program. Type `load` and watch the loop iterate over your file.
- Set a breakpoint inside the `catch` block. Provoke an `IOException` by holding `inventory.txt` open in another program while you `save`.
- Use *Watch* (right-click a variable → *Add to Watch*) to keep an eye on `inventory.Count` as you add items.

## Name it

- **Exception.** A runtime error. C# represents it as a value of type `Exception` (or one of its subclasses like `IOException`, `FileNotFoundException`, `ArgumentException`).
- **`try / catch`.** Wrap risky code in a `try` block. If something throws, a `catch (TypeOfException)` block runs instead. The program continues after the `catch`.
- **Catch order matters.** Catches are tested top-to-bottom. The first matching one runs. Catch specific types first; `catch (Exception)` last as a safety net.
- **`finally`** (we didn't use it today). A block that always runs after `try`, whether it threw or not. Useful for closing files, releasing locks, etc.
- **Breakpoint.** A marked line where the debugger pauses.
- **Step over (`F10`)** vs **step into (`F11`)**. *Step over* runs the current line and moves to the next. *Step into* dives into a method call.
- **Call stack.** When a method calls another method that calls another, the debugger shows the chain. Useful for understanding *how you got here*.

## M1 — Inventory Tool, shipped

You now have the M1 deliverable. Make sure your repo has:
- `InventoryTool/` folder at the root with the v2 `Program.cs` and a `.csproj`.
- A top-level `README.md` describing the tool (use the four-section anatomy from Module 0.4).
- A `journal/wins.md` entry for M1 (your milestone ritual).

Run the M1 challenge:

```powershell
dotnet test path\to\challenges\M1\M1.Tests.csproj
```

Green = M1 met. Commit and push.

**Per the milestone ritual** (see `STYLE.md`):

1. `journal/wins.md` entry — one paragraph in your words.
2. `#wins` Slack post — link to the PR + a screenshot of the tool running.
3. Before/after one-liner — *"Six weeks ago I'd never written a line of code. Today I shipped a tool I'll actually use."*

## Quiz / challenge

Open `quiz.md`.

## Connect

That's the end of Phase 0. Block 3 (Console Kingdom) starts the *real* mainstay — the Kingdom itself. You'll meet your first **classes** (Module 1.1), then split your code into an *engine* and a *shell* (1.2 — the lesson the rest of the course is named after). After that, you write your first **tests**.
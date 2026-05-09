# Module 0.8 — Errors, Debugging, and M1

Today you make Inventory Tool *not crash* when things go wrong, and you meet the VS Code debugger — the single most important tool for understanding code that isn't doing what you expect. Then you ship M1. End of Foundations, end of Phase 0.

> **Words to watch**
>
> - **exception** — a runtime error; something the program couldn't handle and threw up its hands about
> - **`try / catch`** — the C# pattern for "try this; if it throws, do that instead"
> - **`throw`** — the keyword for raising your own exception
> - **debugger** — a tool that pauses your program at chosen points so you can see what's happening
> - **breakpoint** — a marked line where the debugger pauses
> - **call stack** — the chain of methods that called each other to get to where you are now

---

## Step 1 — what can go wrong

Open the Inventory Tool you wrote in Module 0.7. Look at the `load` case. As written, what happens if `inventory.txt` exists but is corrupt? Or if a line has `apple=banana` instead of `apple=2`? The `int.TryParse` we used handles bad numbers gracefully — that's why TryParse exists, to give back success or failure instead of throwing. But other things can still break things.

The file might exist but be locked by another program — that throws `IOException`. The path might be on a read-only drive — `UnauthorizedAccessException`. The user might type `add` with no argument — currently we silently add an item named `""` (empty string), which is a quiet bug rather than a noisy one. Quiet bugs are worse.

## Step 2 — harden the tool

Replace your `Program.cs` with this hardened version. Changes are flagged with `// NEW`:

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

The `try` block wraps the whole switch. If any case throws, control jumps to whichever `catch` matches the exception type. The catches are tested top-to-bottom; the first match wins. We catch `IOException` and `UnauthorizedAccessException` specifically because we can describe those problems to the user in plain language. The final `catch (Exception)` is the safety net — it handles anything we didn't think of, so a single bad input never kills the program.

Run again. Try `add` with no argument. Try `remove` with nothing after it. Try saving to a folder you don't have permission to write to (modify `SaveFile` temporarily to `"C:\\Windows\\inventory.txt"` if you want to see the catch fire). The program no longer crashes.

## Step 3 — the debugger

Open `Program.cs` in VS Code. Click in the gutter — the empty space to the left of the line numbers — next to the line `inventory[arg] = inventory.GetValueOrDefault(arg, 0) + 1;`. A red dot appears. That's a **breakpoint**.

Press `F5`, or use *Run → Start Debugging*. VS Code may prompt you to install the C# Dev Kit debugger if you haven't already; confirm. The program runs, then pauses *at* your breakpoint when you type `add apple` — before that line executes.

The left panel shows two things worth knowing about. **Variables** lists the current value of `arg` (`"apple"`), `inventory` (an empty dictionary right now), `cmd` (`"add"`), and so on. **Call stack** shows the chain of methods that called each other to get here. Right now it's just `Program.<Main>$` — the top-level entry point — but once you're calling your own methods you'll see the chain.

Hover your mouse over `arg` in the code. The value pops up. Press `F10` (*step over*) — the line executes and you advance to the next line. `inventory` now contains `{"apple": 1}`. You watched the dictionary change. Press `F5` to continue; the program prints `Added: apple (now have 1)` and waits for your next command.

## Tinker with the debugger

Set a breakpoint inside the `load` case. Run the program. Type `load` and step through the loop with `F10`, watching `kv`, `loaded`, and `skipped` change as each line is parsed.

Set a breakpoint inside the `catch (IOException)` block. Provoke an `IOException` by holding `inventory.txt` open in another program (Notepad works) while you call `save`.

Right-click a variable in the editor and choose *Add to Watch*. The variable now appears in the *Watch* panel and its value updates live as you step.

## Name it

**Exception.** A runtime error. C# represents it as a value of type `Exception` (or one of its subclasses like `IOException`, `FileNotFoundException`, `ArgumentException`).

**`try / catch`.** Wrap risky code in a `try` block. If something throws, a `catch (TypeOfException)` block runs instead. Execution continues after the `catch`.

**Catch order matters.** Catches are tested top-to-bottom; the first matching one runs. Catch specific types first; `catch (Exception)` last as a safety net.

**`finally`** (we didn't use it today, but you'll see it). A block that always runs after `try`, whether it threw or not. Useful for closing files, releasing locks, and other cleanup that has to happen no matter what.

**Breakpoint.** A marked line where the debugger pauses *before* executing that line.

**Step over (`F10`)** runs the current line and moves to the next line in the same method. **Step into (`F11`)** dives into a method call so you can step through it line by line.

**Call stack.** When a method calls another method which calls another, the debugger shows the chain. Useful for understanding *how you got here* — often more useful than knowing where the error happened.

## M1 — Inventory Tool, shipped

You now have the M1 deliverable. Make sure your repo has:

- `InventoryTool/` folder at the root with the v2 `Program.cs` and a `.csproj`
- A top-level `README.md` describing the tool (use the four-section anatomy from Module 0.4)
- A `journal/wins.md` entry for M1 (your milestone ritual, below)

Run the M1 challenge:

```powershell
dotnet test path\to\challenges\M1\M1.Tests.csproj
```

Green means M1 met.

## Commit your work

In VS Code's Source Control panel (`Ctrl + Shift + G G`):

1. Stage your changes — hover **Changes** and click `+`.
2. Commit message: *"M1: Inventory Tool v2 — hardened + debugged"*.
3. Click the blue **checkmark** to commit.
4. Click **Sync Changes** to push to GitHub.

> **Or in the terminal:**
>
> ```powershell
> git add .
> git commit -m "M1: Inventory Tool v2 — hardened + debugged"
> git push
> ```

## What you just did

You hardened a real program. The Inventory Tool went from "crashes on weird input" to "tells the user what went wrong and keeps running." You met `try/catch`, the three rules of catch ordering, and the `Exception` type tree. You met the VS Code debugger — breakpoints, step over, the variables panel, the call stack. The skipped-bad-lines counter in `load` is a small thing, but it's the difference between a tool you trust and one you don't. Eight modules of Phase 0 are behind you; you have two shipped programs (M0 toolbox, M1 Inventory Tool) on your GitHub, and the foundation pieces of C# are named.

**Key concepts you can now name:**

- **exception** — runtime error you can catch
- **`try / catch`** — risky block plus handler
- **catch ordering** — specific first, generic last
- **breakpoint** — debugger pauses before this line
- **step over vs step into** — skip a call, vs follow it in
- **call stack** — chain of who called whom

## M1 close — the milestone ritual

You just shipped M1. Time for the ritual.

1. **`journal/wins.md`** — open it in your repo and write one paragraph about M1 in your own words. What the Inventory Tool does, what was harder than expected, what you trust about it now that you didn't trust about v1.
2. **`#wins` Slack post** — paste the link to your repo plus a screenshot of the tool running. One-line caption like *"M1 shipped — Inventory Tool v2."*
3. **Before/after one-liner** — *"Six weeks ago I'd never written a line of code. Today I shipped a tool I'll actually use."* Say it out loud.
4. **Tag the milestone.** This one's CLI-only — the panel doesn't have a button for tags:

   ```powershell
   git tag m1-inventory-tool-complete
   git push origin m1-inventory-tool-complete
   ```

Then take the rest of the day off. You earned it.

## Wrap up

1. **Quiz** — open `quiz.md`, jot your answers in `journal/quiz-notes.md`.
2. **Progress** — one line in `journal/progress.md`: `Module 0.8 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit and push** — first check Source Control says `kingdom` (not `kingdom-course`!), then stage both files, commit message `Module 0.8 done`, Sync.
4. **Post in `#wins`** — one line about today, plus the URL of the commit.

Module 0.1 covers the why and the panel/CLI steps if you need a refresher. Bring quiz answers you're least sure about to the next weekly sync.

## Next

Phase 1 starts the real mainstay — the Kingdom itself. You'll meet your first **classes** (Module 1.1), then split your code into an *engine* and a *shell* (1.2 — the lesson the rest of the course is named after). After that, you write your first **tests**.

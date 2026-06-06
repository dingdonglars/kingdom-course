# Module 0.8 — Errors, Debugging, and M1

Today you make the Inventory Tool *not crash* when things go wrong, and you meet the VS Code debugger — the most important tool there is for understanding code that isn't doing what you expect. Then you finish M1. End of Foundations, end of Phase 0.

> **Words to watch**
>
> - **exception** — a runtime error; a problem the program ran into while running and couldn't deal with
> - **`try / catch`** — the C# pattern for "try this; if it throws, do that instead"
> - **`throw`** — the keyword for raising your own exception
> - **debugger** — a tool that pauses your program at chosen points so you can see what's happening
> - **breakpoint** — a marked line where the debugger pauses
> - **call stack** — the chain of methods that called each other to get to where you are now

---

## Step 1 — what can go wrong

Open the Inventory Tool you wrote in Module 0.7. Look at the `load` case. As it stands, what happens if `inventory.txt` exists but is broken? Or if a line says `apple=banana` instead of `apple=2`? The `int.TryParse` we used deals with bad numbers calmly — that's the whole reason TryParse exists, to tell you whether it worked instead of crashing. But other things can still go wrong.

The file might exist but be locked by another program — that gives an `IOException`. The file might be on a drive you can't write to — that gives an `UnauthorizedAccessException`. The user might type `add` with nothing after it — right now we quietly add an item with an empty name (`""`), which is a silent bug. Silent bugs are worse than loud ones, because nobody notices them.

## Step 2 — harden the tool

Replace your `Program.cs` with this stronger version. The changes are marked with `// NEW`:

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

The `try` block goes around the whole switch. If any case throws an error, the program jumps to whichever `catch` matches the type of error. C# checks the catches from top to bottom, and the first match is the one that runs. We catch `IOException` and `UnauthorizedAccessException` by name, because we can explain those problems to the user in plain words. The final `catch (Exception)` is the backstop — it handles anything we didn't think of, so one bad input never crashes the whole program.

Run it again. Try `add` with nothing after it. Try `remove` with nothing after it. Try saving to a folder you don't have permission to write to (change `SaveFile` for a moment to `"C:\\Windows\\inventory.txt"` if you want to see a catch run). The program no longer crashes.

## Step 3 — the debugger

Open `Program.cs` in VS Code. Click in the narrow empty strip just left of the line numbers, next to the line `inventory[arg] = inventory.GetValueOrDefault(arg, 0) + 1;`. A red dot appears. That's a **breakpoint**.

First, one quick check: the VS Code window must be open on the **`InventoryTool` folder itself** — the title bar and the file tree should say `InventoryTool`, not the whole `kingdom` repo. If they don't, do *File → Open Folder…* → the `InventoryTool` folder. (One program, one window — otherwise F5 won't know which program to start. The short guide is `running-your-project.md`.)

Now press `F5`, or use *Run → Start Debugging*. VS Code may ask you to install the C# Dev Kit debugger if you haven't already; say yes. The program runs, then pauses *at* your breakpoint when you type `add apple` — just before that line runs.

The left panel shows two things worth knowing about. **Variables** lists the current value of `arg` (`"apple"`), `inventory` (an empty dictionary right now), `cmd` (`"add"`), and so on. **Call stack** shows which methods called which to get to this point. Right now it's just `Program.<Main>$`, the place where your program starts. Once you're calling your own methods, you'll see the full chain here.

Hover your mouse over `arg` in the code. Its value pops up. Press `F10` (*step over*) — the line runs and you move to the next line. `inventory` now contains `{"apple": 1}`. You just watched the dictionary change. Press `F5` to carry on; the program prints `Added: apple (now have 1)` and waits for your next command.

## Tinker with the debugger

Set a breakpoint inside the `load` case. Run the program. Type `load` and step through the loop with `F10`, watching `kv`, `loaded`, and `skipped` change as each line is read.

Set a breakpoint inside the `catch (IOException)` block. Cause an `IOException` on purpose by keeping `inventory.txt` open in another program (Notepad works) while you call `save`.

Right-click a variable in the editor and choose *Add to Watch*. The variable now appears in the *Watch* panel, and its value updates as you step through the code.

## Name it

**Exception.** A runtime error. C# stores it as a value of type `Exception`, or one of its more specific kinds like `IOException`, `FileNotFoundException`, or `ArgumentException`.

**`try / catch`.** Put risky code in a `try` block. If something throws an error, a `catch (TypeOfException)` block runs instead. The program then carries on after the `catch`.

**Catch order matters.** C# checks the catches from top to bottom, and the first one that matches runs. Put the specific types first, and `catch (Exception)` last as a backstop.

**`finally`** (we didn't use it today, but you'll see it). A block that always runs after `try`, whether an error happened or not. Useful for closing files, freeing locks, and other tidy-up that has to happen no matter what.

**Breakpoint.** A marked line where the debugger pauses, *before* running that line.

**Step over (`F10`)** runs the current line and moves to the next line in the same method. **Step into (`F11`)** goes inside a method call so you can step through that method line by line.

**Call stack.** When one method calls another, which calls another, the debugger shows that chain. It's useful for seeing *how the program reached this point* — often more useful than just knowing where the error happened.

## M1 — Inventory Tool, shipped

You now have the program to hand in for M1. Make sure your repo has:

- An `InventoryTool/` folder at the top, with the v2 `Program.cs` and a `.csproj`
- A `README.md` at the top describing the tool (use the four sections from Module 0.4)
- A `journal/wins.md` entry for M1 (the milestone steps below)

Run the M1 challenge:

```powershell
dotnet test path\to\challenges\M1\M1.Tests.csproj
```

If the tests pass (green), M1 is done.

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

You made a real program stronger. The Inventory Tool went from "crashes on strange input" to "tells the user what went wrong and keeps running." You met `try/catch`, the three rules for the order of catches, and the family of `Exception` types. You met the VS Code debugger — breakpoints, step over, the variables panel, the call stack. The "skipped bad lines" counter in `load` is a small thing, but it's the difference between a tool you trust and one you don't. Eight modules of Phase 0 are behind you. You have two finished programs on your GitHub (the M0 toolbox and the M1 Inventory Tool), and the basic parts of C# now have names.

**Key concepts you can now name:**

- **exception** — runtime error you can catch
- **`try / catch`** — risky block plus handler
- **catch ordering** — specific first, generic last
- **breakpoint** — debugger pauses before this line
- **step over vs step into** — skip a call, vs follow it in
- **call stack** — chain of who called whom

## On your own

Time to put the book away. Don't scroll back up to the steps — prove to yourself, from your own head, that the two big ideas stuck. No one marks this one — it's just for you. It's the easiest way to spot what *hasn't* stuck yet, while it's still simple to fix. Getting stuck here is completely fine — that's exactly what it's for.

### 1. The `try / catch` shape, from memory

Open a new empty file. Without looking:

1. Write a `try` block with **two** `catch` blocks after it — one catching `IOException`, a last one catching plain `Exception`.
2. Put a line inside the `try` that throws on purpose — `throw new IOException("test");` will do.
3. Put a `Console.WriteLine` inside each catch so you can see which one runs. Run it.

Then swap the order: put `catch (Exception)` *first*. What does the compiler say? That error is the whole reason for "specific first, generic last."

<details><summary>Stuck? Open this to check yourself.</summary>

```csharp
try
{
    throw new IOException("test");
}
catch (IOException ex)
{
    Console.WriteLine($"IO: {ex.Message}");
}
catch (Exception ex)
{
    Console.WriteLine($"Other: {ex.Message}");
}
```

With `catch (Exception)` first, the build fails: *"A previous catch clause already catches all exceptions."* The general catch would swallow everything before the specific one ever got a turn — so C# won't let you.

</details>

### 2. Use the debugger from memory

Open your Inventory Tool. Without scrolling back to Step 3, from memory:

1. Set a breakpoint on the line inside `add` that changes the dictionary.
2. Start debugging, and type `add sword`.
3. When it pauses, find the value of `arg` in the Variables panel.
4. Step over one line and watch `inventory` change. Then let it finish.

<details><summary>Stuck? Open this to check yourself.</summary>

- Breakpoint: click the narrow strip just left of the line number — a red dot appears.
- Start debugging: `F5`.
- It pauses *before* the breakpoint line runs.
- Variables panel (left): `arg` is `"sword"`.
- Step over: `F10` — `inventory` now shows `{"sword": 1}`.
- Carry on: `F5`.

</details>

## M1 close — the milestone steps

You just finished M1. Here are the steps to close it out.

1. **`journal/wins.md`** — open it in your repo and write one paragraph about M1 in your own words. What the Inventory Tool does, what was harder than you expected, and what you trust about it now that you didn't trust in v1.
2. **`#wins` Slack post** — paste the link to your repo plus a screenshot of the tool running. Add a one-line caption like *"M1 done — Inventory Tool v2."*
3. **Before-and-after line** — *"Six weeks ago I'd never written a line of code. Today I finished a tool I'll actually use."* Say it out loud.
4. **Tag the milestone.** This one is terminal-only — the Source Control panel doesn't have a button for tags:

   ```powershell
   git tag m1-inventory-tool-complete
   git push origin m1-inventory-tool-complete
   ```

Then take the rest of the day off. You've earned it.

## Wrap up

1. **Quiz** — open `quiz.md`, jot your answers in `journal/quiz-notes.md`.
2. **Progress** — one line in `journal/progress.md`: `Module 0.8 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit and push** — first check Source Control says `kingdom` (not `kingdom-course`!), then stage both files, commit message `Module 0.8 done`, Sync.
4. **Post in `#wins`** — one line about today, plus the URL of the commit.

Module 0.1 covers the why and the panel/CLI steps if you need a refresher. Bring quiz answers you're least sure about to the next weekly sync.

## Next

Phase 1 starts the main project — the Kingdom itself. You'll meet your first **classes** (Module 1.1), then split your code into an *engine* and a *shell* (1.2 — the lesson the rest of the course is named after). After that, you write your first **tests**.

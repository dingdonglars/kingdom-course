# Running your project — the one rule that saves you

> Read this once. Come back to it any time *Run* or the debugger doesn't behave.
>
> **Setup assumed:** Windows + VS Code, your `kingdom` repo at `C:\code\kingdom`.

There is one habit that makes running and debugging "just work", and skipping it is the single most common way a beginner gets stuck for half an hour on something that isn't even a bug. Here it is:

## The rule: one program, one window

**When you work on a program, open *that program's own folder* as the VS Code window — not the whole `kingdom` folder.**

Each toy you build in Phase 0 lives in its own folder inside `kingdom`:

```
C:\code\kingdom\
├─ RoastOMatic\        <- a program (has its own .csproj)
├─ InventoryTool\      <- a program (has its own .csproj)
├─ QuestBoard\         <- a program (has its own .csproj)
└─ journal\            <- not a program, just your notes
```

When you want to run `QuestBoard`, the VS Code window must be **`C:\code\kingdom\QuestBoard`** — the one folder. Not `C:\code\kingdom`.

## Why this matters (the thing that bit people)

VS Code and the `dotnet` tool work out *what to run* from the folder you have open.

- Open **one program's folder**, and there's exactly one thing to run. *Run* and the debugger pick it with no questions asked.
- Open the **whole `kingdom` folder**, and they see *several* programs side by side and don't know which one you mean. So `dotnet run` stops with an error like *"Specify which project to use because this folder contains more than one project,"* and pressing **F5** does nothing useful.

That error isn't your code being broken. It's the tool saying *"which one?"* The fix is never to change your code — it's to open the right folder.

## Make a new program (you'll do this at the start of each checkpoint)

In the terminal, from inside your `kingdom` folder:

```powershell
cd C:\code\kingdom
dotnet new console -o QuestBoard
```

That makes a new folder `QuestBoard` with an empty program inside (swap `QuestBoard` for whatever you're building). Then open it as its own window — next step.

## Open it as the window

**File → Open Folder…** → pick `C:\code\kingdom\QuestBoard` → Open.

The title bar and the file tree on the left should now show **QuestBoard**, not kingdom. That's the signal you did it right.

> **Your git still works.** Even though the window only shows the one program, VS Code looks *upward* and finds the `kingdom` repo, so the Source Control panel still says **`kingdom`** and Sync still pushes everywhere. (More on the journal step at the bottom.)

## Run it

In the terminal (open one with the terminal shortcut, or *View → Terminal*):

```powershell
dotnet run
```

No `--project`, no folder name needed — there's only one program in view, so plain `dotnet run` is enough.

## Debug it

1. Open `Program.cs`.
2. Click in the narrow strip just left of a line number. A **red dot** appears — that's a breakpoint.
3. Press **F5**. The program starts and pauses when it reaches that line.
4. Use the **Variables** panel on the left to see what's in each variable, **F10** to run one line at a time, and **F5** again to carry on.

Because the window holds exactly one program, F5 knows what to start. No extra setup, no menus.

## When *Run* or F5 won't behave — check this first

| What you see | What it means | The fix |
|---|---|---|
| `dotnet run` says *"more than one project"* | You're in the `kingdom` folder, not a program's folder | Open the single program's folder as the window |
| **F5** does nothing, or asks you to "select a project" | Same thing — too many programs in view | Open the single program's folder as the window |
| The file tree on the left says **kingdom**, not your program | The whole repo is open | **File → Open Folder…** → the program's folder |

Nine times out of ten, "the debugger is broken" is really "the wrong folder is open." Check the title bar before anything else.

## The one catch — and how to handle it

Your `journal\` notes (`progress.md`, `quiz-notes.md`) live at the **top** of `kingdom`, not inside the program folder. So when a checkpoint's *Wrap up* asks you to edit `journal/progress.md` and commit, you have two easy choices:

- **Commit from the terminal.** The terminal inside your program window still sees the whole repo, so this works as-is:
  ```powershell
  git add .
  git commit -m "Module 0.9b done"
  git push
  ```
- **Or reopen the kingdom window** for the wrap-up: **File → Open Folder…** → `C:\code\kingdom`, edit your journal files, then commit and Sync from the Source Control panel as usual.

Either is fine. Pick the one that feels less fiddly. Everything else in the lesson — building, running, debugging — stays in the one-program window.

# Running your project — the one rule that saves you

> Travou no inglês? Abra o `running-your-project.pt.md` — é este mesmo guia em português. Tente em inglês primeiro.

> Read this once. Come back to it any time *Run* or the debugger doesn't behave.
>
> **Setup assumed:** Windows + VS Code, your `kingdom` repo at `C:\code\kingdom`.

There is one habit that makes running and debugging "just work", and skipping it is the single most common way a beginner gets stuck for half an hour on something that isn't even a bug. The habit has two shapes, depending on where you are in the course — but they're really the same idea: **open the one thing you're working on, and nothing else.**

## Phase 0 (Spark Week): one *program*, one window

In Spark Week you build lots of small, separate programs. Each one lives in its own folder inside `kingdom`:

```
C:\code\kingdom\
├─ RoastOMatic\        <- a program (has its own .csproj)
├─ TavernTab\          <- a program (has its own .csproj)
├─ QuestBoard\         <- a program (has its own .csproj)
└─ journal\            <- not a program, just your notes
```

**When you work on one of these, open *that program's own folder* as the window — not the whole `kingdom` folder.** To work on `QuestBoard`, the VS Code window must be **`C:\code\kingdom\QuestBoard`**.

**Make a new one** (you'll do this at the start of each Spark-Week checkpoint):

```powershell
cd C:\code\kingdom
dotnet new console -o QuestBoard
```

Then **File → Open Folder…** → `C:\code\kingdom\QuestBoard`. The title bar should say **QuestBoard**. Run it with plain `dotnet run` (only one program in view, so no extra words needed), and debug with **F5**.

## Phase 1 onward (the Kingdom): one *solution*, one window

From Module 1.0 on, you stop building separate little programs and build **one** thing — the Kingdom. It lives in its own folder, `kingdom-game`, as a **solution**: one container holding several projects (the engine, the console, later the web and tests).

```
C:\code\kingdom\
├─ (your Spark Week toys, left alone)
└─ kingdom-game\          <- open THIS, and keep it open all year
   ├─ Kingdom.slnx        <- the solution
   ├─ Kingdom.Console\    <- the program you run
   ├─ Kingdom.Engine\     <- the rules (added in 1.2)
   └─ tests\              <- your tests (added in 1.3)
```

**Open `C:\code\kingdom\kingdom-game` as the window — once — and stay there.** You do *not* hop in and out of `Kingdom.Console` and `Kingdom.Engine` folders. The Kingdom is one thing, so it's one window. The title bar should say **kingdom-game**.

**Run it** by naming the project you want:

```powershell
dotnet run --project Kingdom.Console
```

The `--project Kingdom.Console` part says *which* program to run. You name it because the solution holds more than one project — and from Phase 3 it'll hold more than one program you *can* run (the console and the web API). Naming it is never ambiguous, so it's the habit to keep.

**Run your tests** (from Module 1.3 on) from the same window's terminal:

```powershell
dotnet test
```

That builds the whole solution and runs every test in it. (The Test Explorer panel on the left shows them too, if you'd rather click.)

**Debug** with **F5**: open `Kingdom.Console/Program.cs`, drop a red breakpoint to the left of a line, press F5. While the console is the only program that can run (Phases 1–2), F5 starts it with no fuss. The first time, VS Code may ask you to pick the launch target once — choose `Kingdom.Console`.

## When *Run* or F5 won't behave — check this first

| What you see | What it means | The fix |
|---|---|---|
| `dotnet run` says *"more than one project"* | **Phase 0:** you opened the whole `kingdom` folder, not one program's folder | Open the single program's folder | 
| `dotnet run` says *"more than one project"* | **Phase 1+:** you didn't name the project | Add `--project Kingdom.Console` (or the project you mean) |
| **F5** does nothing, or asks you to "select a project" | Too many programs in view, or no launch target picked | Phase 0: open the one program's folder. Phase 1+: pick `Kingdom.Console` when asked |
| The title bar shows **kingdom**, not your program or **kingdom-game** | The whole repo is open | **File → Open Folder…** → the right folder for your phase |

Nine times out of ten, "the debugger is broken" is really "the wrong folder is open." Check the title bar before anything else.

## The one catch — the journal

Your `journal\` notes (`progress.md`, `quiz-notes.md`, `wins.md`) live at the **top** of `kingdom`, not inside your program or `kingdom-game`. Good news: from any of these windows VS Code looks *upward*, finds the `kingdom` repo, and the **Source Control panel still shows the whole repo** — so you can edit `journal/` files and commit them right from the panel as usual. (The journal files just won't appear in the file tree on the left, because they're above the folder you opened.)

If you'd rather see them in the tree for a wrap-up, either:

- **Commit from the terminal** — it sees the whole repo:
  ```powershell
  git add .
  git commit -m "Module 1.1 done"
  git push
  ```
- **Or reopen the `kingdom` window** briefly, edit your journal files, then commit and Sync from the Source Control panel.

Either is fine. Everything else — building, running, debugging, testing — stays in your one phase-appropriate window.

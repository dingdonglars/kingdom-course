# Module 1.10 — Polish & Repo Rescue (M2 close)

> **Hook:** Block 3 ends today. The kingdom is a real engine — 35 tests, deterministic, organised. Now we shine it. Better README, a few well-placed comments, the names you wish you'd picked first time. Plus: the **repo rescue workflow** for the day your git branch is a smoking ruin.

> **Words to watch**
> - **README** — the doc at the top of the repo. Four sections that always matter: *what / how to run / what you learned / what's next*.
> - **XML doc comment** — `///` comments above a public type or method. The IDE shows them as tooltips and IntelliSense.
> - **stash** — `git stash` — set aside uncommitted changes for later, returning the working tree to clean.
> - **rescue** — when your branch is a mess: snapshot, reset, recover only what you wanted.

---

## Polish, briefly

You've shipped a working engine. Most of the polish is small:

### 1. README at the repo root

Four sections, in this order:

```markdown
# Kingdom

A console kingdom-management game. Block 1 of the Kingdom Curriculum.

## Run it

\`\`\`powershell
dotnet run --project Kingdom.Console
\`\`\`

Output: Eldoria runs for 30 days, prints the final state and a deterministic event log.

## What I learned

- Engine vs shell — the engine never knows about Console
- Inheritance — Farm/Lumberyard/Mine override `Building.Tick`
- LINQ — `.OfType<>().Count()` instead of manual `for` loops
- Interfaces + dependency injection — `IRandom`/`IClock` make the engine testable
- FakeItEasy — surgical control of dependencies in tests
- Sub-namespaces — `Kingdom.Engine.Buildings` etc.

## What's next

- Phase 2 (M3): persistence — save/load to a file, then SQLite
- Phase 3 (M4): web API — same engine, HTTP shell
```

### 2. XML doc comments on public types

A small number of `///` comments above the *public* surface. Skip private fields and obvious methods.

```csharp
/// <summary>
/// The aggregate root of the kingdom. Owns buildings, citizens, resources, the event log.
/// Advanced one tick at a time via <see cref="AdvanceDay"/>.
/// </summary>
public class Kingdom { ... }

/// <summary>Random number source. Production: <see cref="SystemRandom"/>. Tests: a FakeItEasy fake.</summary>
public interface IRandom { ... }
```

The `<see cref="..."/>` is a hyperlink in the IDE. Useful, but **don't go overboard**. A doc comment on every property is noise. **Comment the *why* and the *non-obvious*; leave the obvious.**

### 3. Naming pass

Walk every public name. Could it be clearer? Examples:

- `RollOnce` is fine — it rolls once.
- `Snapshot()` on `ResourceLedger` returns a read-only dict — fine.
- `_eventEngine` (private field) — fine.
- If you find a stale name, rename it. Modern IDEs make rename across the codebase + tests trivial.

### 4. Tinker section

Add three lines to the README under "What's next":

> Stretch ideas if you want to explore further before Phase 2:
> - Add a `Quarry` building (marble?)
> - Add a `Mood` enum to `Citizen` and an event `CitizenHappy`
> - Print a CSV of the event log at the end

These are *invitations*, not assignments.

## Repo rescue (the workflow that matters)

Sometimes your git tree is a mess: half-finished commits, a branch that diverged, edits you don't want. Knowing how to *rescue* a working state saves hours.

### Scenario A — uncommitted changes you don't want

```powershell
git status     # confirm what's modified
git diff       # review one last time
git stash      # set them aside (recover later with: git stash pop)
# OR
git restore .  # throw them away (irreversible!)
```

### Scenario B — committed work on the wrong branch

```powershell
# You committed to main, but it should be on a feature branch.
git log --oneline -5
git branch feature/my-thing       # save the commit on a new branch
git reset --hard origin/main      # rewind main
git checkout feature/my-thing     # continue on the right branch
```

### Scenario C — your local branch is broken; the remote is good

```powershell
git fetch origin
git reset --hard origin/main      # nuclear: discard ALL local work, match remote
```

> ⚠ `git reset --hard` is **destructive**. Always `git stash` or commit-then-reset first if you might want anything back.

### Scenario D — you want only some of your changes

```powershell
git diff HEAD                      # review everything that's changed
git add -p                         # interactive: stage hunks one by one
git commit -m "feat: just the bits I wanted"
git stash                          # set aside the rest for later
```

### Scenario E — total rescue: cherry-pick the good commits to a fresh branch

```powershell
git log --oneline                  # find the commit hashes you want to keep
git checkout main
git pull
git checkout -b feature/clean
git cherry-pick <hash1> <hash2>    # reapply just those commits
```

### The rule of the rescue

> **If you don't know what `git status` and `git log --oneline -10` say, stop. Read. Then decide.**

90% of git incidents are *"I don't actually know the state I'm in, so I tried something and it got worse."* The rescue is in the read.

## Wins log

Open `wins.md` (or create at the repo root) and write three lines:

```markdown
## M2 — Block 3 — Console Kingdom

- 35 tests passing, deterministic
- Engine vs shell pattern internalised — same engine will run in 4 hosts
- Repo organised by area (Buildings/, Events/, Infrastructure/)

Before: Module 0.0 — `Console.WriteLine("Hello, kingdom!");`
After:  Module 1.10 — A 14-file engine with a deterministic event log

Posted to #wins on 2026-MM-DD.
```

Post a screenshot to your `#wins` Discord channel. The before/after one-liner is what your future self will read in a year.

## Step-by-step ritual

There is no new code in this lesson. Walk through *your* repo:

1. Check the README at root. Edit until it satisfies the 4-section template above.
2. Add `<summary>` doc-comments to `Kingdom`, `IRandom`, `IClock`, `EventEngine` (the four most "public" types). Skip the rest.
3. `dotnet build` — must still be 0 errors.
4. `dotnet test` — must still be 35 passing.
5. Commit the polish: `git commit -am "[M2] polish: README + doc comments"`
6. Run the per-milestone ritual: `wins.md` + Discord post + before/after.

## Tinker

- Run `dotnet build /verbosity:diagnostic` — overwhelming, but skim it once just to see how much the build does.
- Run `dotnet --list-sdks` — confirm you're on `.NET 10`.
- Run `git log --oneline --graph --decorate -20` — read your last 20 commits as a tree. The discipline of small commits with `[P###-T##]` prefixes is now visible.

## Name it

- **README.** The doc at the repo root. Four sections: what, run, learned, next.
- **XML doc comment.** `/// <summary>...</summary>` on a public type/method.
- **`git stash` / `git reset --hard` / `git cherry-pick`.** The three power tools of repo rescue.
- **The rescue rule.** Read the state (`git status`, `git log`) before you act on it.

## The rule of the through-line

> **The repo is the artefact.** Polish the code, polish the README, polish the commit log. The kingdom you can show your future self in a year is the one that's *organised*, not just functional.

## Quiz / challenge

Open `quiz.md`. (Lighter than usual — ritual-themed.)

## Connect

**Phase 2 begins.** Block 4 introduces *persistence*: write the kingdom to a file (JSON), then to SQLite. Same engine, brand new shell — the persistence shell. The first real proof that engine/shell separation pays off.
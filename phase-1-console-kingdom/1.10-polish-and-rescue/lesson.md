# Module 1.10 — Polish and Repo Rescue (M2 close)

Phase 1 ends today. The kingdom is a real engine — thirty-five tests, deterministic, organised by area. Now we shine it. Better README, a few well-placed comments, the names you wish you'd picked the first time. Then we cover the **repo rescue workflow** — the moves to know on the day your git branch is a smoking ruin.

There's no big new code in this lesson. It's a quiet day on purpose. Quiet competence is what closing a milestone looks like — read your own repo end-to-end, make the small fixes, write a short README that future-you will appreciate, then mark the moment with the milestone ritual.

> **Words to watch**
>
> - **README** — the document at the top of a repo. Four sections that always matter: *what / how to run / what you learned / what's next.*
> - **XML doc comment** — `///` comments above a public type or method. The IDE shows them as tooltips and IntelliSense.
> - **stash** — `git stash` — set aside uncommitted changes for later, returning the working tree to clean
> - **rescue** — when your branch is a mess: snapshot, reset, recover only what you wanted

---

## Polish

### 1. README at the repo root

Four sections, in this order:

```markdown
# Kingdom

A console kingdom-management game. Phase 1 of the Kingdom Curriculum.

## Run it

\`\`\`powershell
dotnet run --project Kingdom.Console
\`\`\`

Output: Eldoria runs for 30 days, prints the final state and a deterministic event log.

## What I learned

- Engine vs shell — the engine never knows about Console
- Inheritance — Farm/Lumberyard/Mine override `Building.Tick`
- LINQ — `.OfType<>().Count()` instead of manual `for` loops
- Interfaces and dependency injection — `IRandom`/`IClock` make the engine testable
- FakeItEasy — surgical control of dependencies in tests
- Sub-namespaces — `Kingdom.Engine.Buildings` and friends

## What's next

- Phase 2 (M3): persistence — save/load to a file, then SQLite
- Phase 3 (M4): web API — same engine, HTTP shell
```

### 2. XML doc comments on the public surface

A small number of `///` comments above the public types. Skip private fields and obvious methods — comments on every property is noise. Comment the *why* and the *non-obvious*.

```csharp
/// <summary>
/// The aggregate root of the kingdom. Owns buildings, citizens, resources,
/// and the event log. Advance one tick at a time via <see cref="AdvanceDay"/>.
/// </summary>
public class Kingdom { ... }

/// <summary>Random number source. Production: <see cref="SystemRandom"/>. Tests: a FakeItEasy fake.</summary>
public interface IRandom { ... }
```

The `<see cref="..."/>` becomes a hyperlink in the IDE. Useful, but don't go overboard.

### 3. Naming pass

Walk every public name. Could it be clearer?

`RollOnce` is fine — it rolls once. `Snapshot()` on `ResourceLedger` returns a read-only dict — fine. `_eventEngine` (private field) — fine. If you find a stale name, rename it; modern IDEs make rename across the codebase and tests trivial.

### 4. Tinker section in the README

Add three lines under "What's next":

> Stretch ideas if you want to explore further before Phase 2:
> - Add a `Quarry` building (marble?)
> - Add a `Mood` enum to `Citizen` and an event `CitizenHappy`
> - Print a CSV of the event log at the end

These are invitations, not assignments.

---

## Repo rescue

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

Most git incidents are *"I didn't actually know the state I was in, so I tried something and it got worse."* The rescue is in the read.

---

## Walk through your repo

There's no new code today. Walk through *your* repo:

1. Check the README at the root. Edit until it satisfies the four-section template above.
2. Add `<summary>` doc comments to `Kingdom`, `IRandom`, `IClock`, `EventEngine` (the four most "public" types). Skip the rest.
3. `dotnet build` — must still be 0 errors.
4. `dotnet test` — must still be 35 passing.
5. **Commit the polish.** *"[M2] polish: README + doc comments"*. (Source Control panel → stage → commit → Sync. Or CLI: `git add . && git commit -m "[M2] polish: README + doc comments" && git push`.)

## Tinker

Run `dotnet build /verbosity:diagnostic`. Overwhelming, but skim it once just to see how much the build does behind the scenes.

Run `dotnet --list-sdks` to confirm you're on .NET 10.

Run `git log --oneline --graph --decorate -20`. You'll see your last twenty commits as a tree. The discipline of small commits with `[P###-T##]` prefixes is now visible at a glance.

## The through-line

The through-line in this module: **the repo is what you keep**. Polish the code, polish the README, polish the commit log. The kingdom you can show your future self in a year is the one that's organised, not just functional.

## What you just did

Phase 1 closes today. You wrote a four-section README, dropped doc comments on four public types, walked your own naming once, and learned the five rescue moves you'll reach for some random Tuesday in 2026 when your tree is on fire. The code didn't change in any meaningful way — the same thirty-five tests still pass — and that's the point. Closing well is itself a skill.

**Key concepts you can now name:**

- **README four sections** — what, run, learned, next
- **XML doc comment** — `///` on the public surface, sparingly
- **`git stash`** — non-destructive set-aside of uncommitted work
- **`git reset --hard`** — destructive rewind, useful when sure
- **the rescue rule** — read state before acting on it

## Quiz

Open `quiz.md`. (Lighter than usual — milestone-themed.) When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.

---

## Milestone ritual — M2

You just shipped **M2 — Kingdom v1, Console**. Time for the ritual:

1. **`journal/wins.md` entry.** Open `wins.md` (in your repo's `journal/` folder) and add a paragraph in your own words about what M2 felt like. Keep it short.

2. **`#wins` Slack post.** Drop a screenshot of the running kingdom + a link to the PR + a one-line caption.

3. **Before/after one-liner.** Pick the thing you couldn't do six weeks ago and the thing you can do today, and put them in one sentence. Save it in `wins.md`. Future-you will thank present-you.

   Example: *"Six weeks ago I'd never opened a terminal. Today I shipped a deterministic kingdom engine with thirty-five tests."*

Then take the rest of the day off.

## Next

**Phase 2 begins.** Phase 2 introduces *persistence* — write the kingdom to a file (JSON), then to SQLite. Same engine, brand new shell — the persistence shell. The first real proof that engine/shell separation pays off.

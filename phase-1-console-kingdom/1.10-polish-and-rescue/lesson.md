# Module 1.10 — Polish and Repo Rescue (M2 close)

Phase 1 ends today. The kingdom is a real engine — thirty-five tests, deterministic, organised by area. Now we make it nicer. A better README, a few well-placed comments, and the names you wish you'd picked the first time. Then we cover the **repo rescue workflow** — the moves to know on the day your git branch is a complete mess.

There's no big new code in this lesson. It's a calm day on purpose. Closing a milestone looks like this: read through your own repo from start to finish, make the small fixes, write a short README that will help you later, then mark the moment with the milestone steps at the end.

> **Words to watch**
>
> - **README** — the main document at the top of a repo. Four sections that always matter: *what / how to run / what you learned / what's next.*
> - **XML doc comment** — `///` comments above a public type or method. The editor shows them as tooltips and in IntelliSense.
> - **stash** — `git stash` — set aside changes you haven't committed for later, leaving the working tree clean
> - **rescue** — when your branch is a mess: save a copy, reset, then get back only what you wanted

---

## Polish

### 1. README at the repo root

The README is the first thing people see when they open the repo. It's the first thing a stranger reads on GitHub — and it's the first thing *you* read six months from now, when you've forgotten how this project runs. A good README answers four questions before anyone has to ask: *what is this, how do I run it, what did I learn building it, what's next.* Four short sections are better than one long paragraph, and you'll be glad later that you wrote them.

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
- FakeItEasy — exact control of dependencies in tests
- Sub-namespaces — `Kingdom.Engine.Buildings` and friends

## What's next

- Phase 2 (M3): persistence — save/load to a file, then SQLite
- Phase 3 (M4): web API — same engine, HTTP shell
```

### 2. XML doc comments on the public types

A few `///` comments above the public types. Skip private fields and obvious methods — a comment on every property just adds clutter. Comment the *why*, and the things that aren't obvious.

```csharp
/// <summary>
/// The aggregate root of the kingdom. Owns buildings, citizens, resources,
/// and the event log. Advance one tick at a time via <see cref="AdvanceDay"/>.
/// </summary>
public class Kingdom { ... }

/// <summary>Random number source. Production: <see cref="SystemRandom"/>. Tests: a FakeItEasy fake.</summary>
public interface IRandom { ... }
```

The `<see cref="..."/>` becomes a clickable link in the editor. Useful, but don't add too many.

### 3. Naming pass

Look at every public name. Could it be clearer?

`RollOnce` is fine — it rolls once. `Snapshot()` on `ResourceLedger` returns a read-only dictionary — fine. `_eventEngine` (private field) — fine. If you find an old name that no longer fits, rename it. Modern editors make it easy to rename across the whole project and the tests at once.

### 4. Tinker section in the README

Add three lines under "What's next":

> Stretch ideas if you want to explore further before Phase 2:
> - Add a `Quarry` building (marble?)
> - Add a `Mood` enum to `Citizen` and an event `CitizenHappy`
> - Print a CSV of the event log at the end

These are optional ideas, not required work.

---

## Repo rescue

Sometimes your git tree is a mess: half-finished commits, a branch that has gone its own way, edits you don't want. Knowing how to *rescue* a working state saves hours.

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
git reset --hard origin/main      # warning: discards ALL local work, matches remote
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

Most git accidents are *"I didn't actually know what state I was in, so I tried something and it got worse."* Reading the state first is most of the rescue.

---

## Walk through your repo

There's no new code today. Walk through *your* repo:

1. Check the README at the root. Edit it until it matches the four-section template above.
2. Add `<summary>` doc comments to `Kingdom`, `IRandom`, `IClock`, `EventEngine` (the four most public types). Skip the rest.
3. `dotnet build` — must still be 0 errors.
4. `dotnet test` — must still be 35 passing.
5. **Commit the polish.** *"[M2] polish: README + doc comments"*. (Source Control panel → stage → commit → Sync. Or CLI: `git add . && git commit -m "[M2] polish: README + doc comments" && git push`.)

## Tinker

Run `dotnet build /verbosity:diagnostic`. It's a lot of output, but read over it once just to see how much the build does for you.

Run `dotnet --list-sdks` to check you're on .NET 10.

Run `git log --oneline --graph --decorate -20`. You'll see your last twenty commits as a tree. The habit of making small commits now shows up clearly in the picture.

## The through-line

The through-line in this module: **the repo is what you keep**. Tidy the code, tidy the README, tidy the commit log. The kingdom you'll be glad to look back on in a year is the one that's organised, not just the one that works.

## What you just did

Phase 1 closes today. You wrote a four-section README, added doc comments to four public types, checked your own naming once, and learned the five rescue moves you'll need on some ordinary day when your git tree is a mess. The code didn't really change — the same thirty-five tests still pass — and that's the point. Finishing well is its own skill.

**Key concepts you can now name:**

- **README four sections** — what, run, learned, next
- **XML doc comment** — `///` on the public types, used sparingly
- **`git stash`** — set work aside safely without losing it
- **`git reset --hard`** — throws work away, useful when you're sure
- **the rescue rule** — read the state before you act on it

## Wrap up

1. **Quiz** — open `quiz.md` (lighter than usual — milestone-themed). Jot your answers in `journal/quiz-notes.md`.
2. **Progress** — one line in `journal/progress.md`: `Module 1.10 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit and push** — stage both files, commit message `Module 1.10 done`, Sync.
4. **Post in `#wins`** — one line about today, plus the URL of the commit.

Module 0.1 covers the why and the panel/CLI steps if you need a refresher. Bring quiz answers you're least sure about to the next weekly sync.

---

## Open the milestone PR

You've been committing to the `phase-1` branch since Module 1.1. Now it's time to send the whole phase to Lars as a *pull request* — the place where he reviews your work — and merge it into `main`.

On github.com, open your `kingdom` repo. A yellow banner near the top says something like *"phase-1 had recent pushes — Compare & pull request"*. Click **Compare & pull request**. (No banner? Go to the *Pull requests* tab → *New pull request* → base: `main`, compare: `phase-1`.)

Fill in:

- **Title:** `M2 — Phase 1 — Console Kingdom`
- **Body:** the four `wins.md` bullet points + a line `**Reviewer:** @dingdonglars`

Click **Create pull request**. GitHub tells Lars; he reviews on the *Files changed* tab, leaves comments or clicks **Approve**. If he asks for changes, push more commits to `phase-1` — they show up on the pull request automatically. When the review is **Approved**, click **Merge pull request** → **Confirm merge**. GitHub offers to delete the `phase-1` branch — accept it; the merged history now lives on `main`.

Switch back locally:

```powershell
git switch main
git pull
```

You're back on `main` with Phase 1 merged in. Phase 2 begins in Module 2.1 with a new `phase-2` branch.

---

## Milestone ritual — M2

You just finished **M2 — Kingdom v1, Console**. Time for the milestone steps:

1. **`journal/wins.md` entry.** Open `wins.md` (in your repo's `journal/` folder) and add a paragraph, in your own words, about what M2 felt like. Keep it short.

2. **`#wins` Slack post.** Post a screenshot of the running kingdom, a link to the merged pull request, and a one-line caption.

3. **Before/after one-liner.** Pick something you couldn't do six weeks ago and something you can do today, and put them in one sentence. Save it in `wins.md`. You'll be glad later that you wrote it down.

   Example: *"Six weeks ago I'd never opened a terminal. Today I built a deterministic kingdom engine with thirty-five tests."*

Then take the rest of the day off.

## Next

**Phase 2 begins.** Phase 2 introduces *persistence* (saving) — write the kingdom to a file (JSON), then to SQLite. Same engine, a brand new shell — the saving shell. The first real proof that splitting the engine from the shell was worth it.

# Module 2.11 — Names That Earn Their Keep (M3 close)

> Travou no inglês? Abra o `lesson.pt.md` — é esta mesma lição em português. Tente em inglês primeiro.

The kingdom now has 35 engine tests, JSON, SQLite, EF Core, migrations, save slots, and an interactive UI. The code works. Today is about a deeper skill: a careful pass over the names of everything you've written this phase. This is what turns *"code that works"* into *"code anyone can read six months later — including future-you."*

> **Words to watch**
>
> - **rename party** — a focused session that does *only* renames, nothing else
> - **scope of a name** — how far a name reaches: local (5 lines) → method (50) → class (500) → module → repo → the whole world. The wider the reach, the more work the name has to do.
> - **noise word** — generic words like `Manager`, `Helper`, `Util`, `Data`, `Info` that don't tell you what the thing actually is
> - **earns its keep** — every name you keep should be doing real work for the reader

---

## Why a separate module for naming

Names are your documentation. They're the first thing a reader sees. A good name makes the code around it clear. A weak name forces you to read the whole body just to understand where it's used. Bad names add up — every line that calls `ProcessData(d)` teaches you nothing about what it does.

A *rename party* — doing only renames in one focused session — works well because:

- Modern IDEs make renames safe (Refactor → Rename, F2 in VS / Rider).
- A rename-only PR is easy to review, because there are no logic changes mixed in.
- One sitting catches related names that should change together (`KingdomData` → `KingdomEntity`, plus `ToData()` → `ToEntity()`).

You'll do this exercise once per big chunk of work. After three or four rounds, your first-try names start coming out right more often.

## The five questions

For every public name in your engine, persistence, and console, ask:

1. **Does it say what the thing *is*?** `Buffer` (a buffer of what?) vs `KingdomSnapshotJson` (a JSON snapshot of a kingdom).
2. **Could a new reader guess what it does from the name alone?** `ToSummary()` vs `Convert()`.
3. **Is the name the right length for its scope?** A 3-letter name is fine for a 5-line method (`b` for building). A class deserves the full form (`KingdomEntity`).
4. **Is there a noise word?** `KingdomManager` — manager of *what*? Drop it or replace it.
5. **Does it match the names next to it?** If you already have `Save` and `Load`, the third method should be `Delete`, not `Remove`. Pick one set of words and stick to it.

## Walkthrough — one example from your code

Look at `KingdomEfStore`:

```csharp
public int Save(Kingdom k);
public Kingdom Load(int id, IRandom rng, IClock clock);
public void Update(int id, Kingdom k);
public void Delete(int id);
public IReadOnlyList<KingdomEntity> ListAll();
public IReadOnlyList<KingdomSlotInfo> ListSlots();
```

Six methods. Read them again with the questions:

- `Save` / `Load` / `Update` / `Delete` — the standard CRUD words. They match.
- `ListAll` *vs* `ListSlots` — *both list things*. The first returns full entities; the second returns small, light info. Both names start with `List`, which is confusing, and `ListAll` isn't honest (it doesn't actually return everything; it doesn't load the related buildings). Better:
  - `ListSlots()` stays.
  - `ListAll()` → either delete it (it's only used in tests, and you can replace it with `using var ctx; ctx.Kingdoms.ToList();`) or rename it to `ListAllEntities()` to be honest about what it returns.

Let's decide: drop `ListAll`. The tests don't really need it, and `ListSlots` covers the public methods.

That's a real rename with a reason behind it — not just "for style." Afterward, the codebase has *one fewer name to learn*.

## Walkthrough — second example

Look at `KingdomEntity`. Consider:

- `KingdomEntity.cs` and `BuildingEntity.cs` — clear: these are EF entities (DTOs).
- `KingdomSnapshot.cs` (in the engine) and `BuildingSnapshot.cs` — both are data-only forms. So why two different names?
  - `*Snapshot` is the engine's data form (used by JSON in Module 2.3).
  - `*Entity` is EF's data form (used by SQLite from Module 2.6 on).
  - **They are not the same thing** — the snapshot has `Kind` and `Citizens[]`; the entity has navigation properties. Different forms for different ways of saving. Both names earn their keep.

Sometimes the right answer is to rename nothing.

## The exercise — actually do it in your repo

In one focused 30-minute sitting:

1. Open the engine project. Go through every public type and method. Ask the five questions about each one.
2. Do the same for persistence.
3. Do the same for the console.
4. Use the IDE's **Rename refactoring** (F2 in VS / Rider; Ctrl+Shift+R in some others). It updates every place the name is used — tests, comments, and XML doc references — all at once. Never do a rename with search-and-replace by hand; you'll miss a use somewhere.
5. After each rename: `dotnet build` (must still be 0 errors) and `dotnet test` (must still be 71 passing).
6. **Commit after *each* rename**, with the prefix `[M3-rename]` — for example, *"[M3-rename] drop KingdomEfStore.ListAll (callers used ListSlots; redundant)"*. (Source Control panel → stage → commit → Sync. Or in the terminal: `git commit -am "[M3-rename] ..." && git push`.) Small commits are easy to review, and easy to undo if a rename turns out to be wrong.

## Common renames you might do

- `_eventEngine` → `_events` (a private field; a short name is fine for a short scope)
- `KingdomDbContext.Kingdoms` → fine; this is the normal EF DbSet plural
- `KingdomEfStore.EnsureCreated()` → still a fine name, even though it now calls `.Migrate()`; what it promises to do hasn't changed
- `KingdomSummary.BuildingCount` → fine; it says exactly what it is
- Any class ending in `Manager`, `Helper`, `Util`, or `Data` — look at it hard
- Any property named `Info` — info about what? Make it specific.

## Delta starter

This module includes only:

- `M3-rename-checklist.md` — a checkbox list of common rename targets in *your* repo
- `wins.md.append.md` — a one-paragraph M3 entry for your wins log

There's no code change to add — the renames you do are **specific to your code**.

## Step-by-step ritual (M3 close)

1. Run the rename party against your repo.
2. `dotnet build` — 0 errors.
3. `dotnet test` — still 71 passing.
4. Append the M3 entry to `wins.md`.
5. Post your before/after to `#wins` on Slack.
6. **Tag the milestone.** This one's CLI-only — the panel doesn't have a button for tags:

   ```powershell
   git tag m3-phase-2-complete
   git push origin m3-phase-2-complete
   ```

7. **Open the M3 PR.** On github.com → your `kingdom` repo → the banner *"phase-2 had recent pushes — Compare & pull request"* (or *Pull requests → New pull request*, base `main`, compare `phase-2`). Title: `M3 — Phase 2 — Persistence`. Body: this milestone's `wins.md` bullets, plus `**Reviewer:** @dingdonglars`. Lars reviews, then approves. You Merge, and delete the `phase-2` branch when it asks. Then, locally: `git switch main && git pull`. (Full walkthrough: Module 1.10.)

## Tinker

Read your most recent commit message. Is it specific? *"refactor"* says nothing; *"drop ListAll, keep ListSlots — same callers, fewer methods"* tells the story.

Pick the worst-named thing in your repo (you'll know which one). Rename it. Notice how much easier the code is to read at every place it's used.

Try the opposite — rename `Kingdom` to `K` everywhere. Save it in a branch. Read your code with that name. Notice how much harder it is to follow. Long names earn their keep when the scope is large.

Read [`Naming Things`](https://en.wikipedia.org/wiki/Naming_(parameter)) in your own time. The well-known essays on naming stay useful for years.

## What you just did

You went through the codebase and made every public name tighter. Some you renamed, some you dropped, and some you left alone once you understood why they were right. The code didn't get smarter — but a future reader will hit far fewer surprises. You also closed M3: tests still 71 passing, saving works across four shells (text, JSON, SQLite, EF Core), and the kingdom remembers itself across sessions. That's the milestone. Don't skip the steps that follow.

**Key concepts you can now name:**

- **rename party** — a focused session of only renames
- **scope of a name** — short for short scope; long for long scope
- **noise word** — `Manager`/`Helper`/`Util`/`Info` to review hard
- **vocabulary discipline** — pick `Save`/`Load`/`Delete`, not a mix
- **IDE Rename** — the only safe way to change a name everywhere

## On your own

Time to put the book away. Don't scroll back up to the five questions — show yourself, from your own head, that the one big idea stuck: a good name says what the thing is, and the wider its reach, the harder it has to work. No one marks this — it's just for you. It's the fastest way to spot what hasn't stuck yet, while it's still small to fix. Getting stuck here is completely fine — that's exactly what it's for.

Here are three weak names. For each one, pick a better name and say in one sentence *why* it's better:

1. a class called `DataManager` that loads and saves players
2. a method called `Process(p)` that turns a player into a save record
3. a method called `DoIt()` that deletes a save slot

<details><summary>Stuck? Open this to check yourself.</summary>

There's no single right answer — names are a judgement. But good answers look like these:

- `DataManager` → `PlayerStore` (or `PlayerRepository`). *Why:* `Manager` is a noise word that says nothing; `Store` says what it does — it stores players.
- `Process(p)` → `ToSaveRecord(p)` (or `ToSnapshot`). *Why:* `Process` could mean anything; the new name says what it returns.
- `DoIt()` → `DeleteSlot()`. *Why:* a name should say what happens; and if the class already has `Save`/`Load`, `Delete` matches the words next to it.

The pattern in every answer: drop noise words, say what the thing *is* or *does*, and match the words already around it.

</details>

## Wrap up

1. **Quiz** — open `quiz.md` (lighter than usual — naming-themed). Jot your answers in `journal/quiz-notes.md`.
2. **Progress** — one line in `journal/progress.md`: `Module 2.11 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit and push** — stage both files, commit message `Module 2.11 done`, Sync.
4. **Post in `#wins`** — one line about today, plus the URL of the commit.

Module 0.1 covers the why and the panel/CLI steps if you need a refresher. Bring quiz answers you're least sure about to the next weekly sync.

---

> **You just shipped M3.** Time for the ritual:
>
> 1. **README refresh** — re-walk the four sections from Module 0.4 (*what / how to run / what I learned / what's next*). Since M2 close, you added persistence in four backends and a save-slot picker; *How to run* and *What I learned* both need a paragraph that didn't exist before. Polishing the README is a milestone discipline — every milestone close from here on circles back to it.
> 2. **`journal/wins.md` entry** — a paragraph in your own words about what changed between M2 and M3. Include the test count, the four save backends, the slot picker.
> 3. **`#wins` Slack post** — link to the PR + a short screenshot or terminal capture, and one line: *"Kingdom v2 — Persisted. Save it, quit, reopen, still there."*
> 4. **Before/after one-liner** — *"A few weeks ago my kingdom died on close. Today it survives across sessions, with save slots."*
> 5. **Tag it** — `git tag m3-phase-2-complete` then `git push origin m3-phase-2-complete`. (CLI-only — the panel doesn't have a button for tags.)
>
> Then take the rest of the day off.

## Phase 2 wins log entry (template)

```markdown
## M3 — Phase 2 — Persistence

- 71 tests, deterministic, across engine + persistence
- Same engine now savable in four ways: text file, JSON, raw SQLite, EF Core
- Real save-slot UI; you can play across sessions

Before: `Console.WriteLine($"Day {kingdom.Day}");` and the kingdom dies on close
After:  Save, quit, reopen days later — your kingdom is exactly where you left it

Posted to #wins on YYYY-MM-DD.
```

## Next

**Phase 3 begins.** It introduces the **web API** — your engine, served over HTTP. Same engine again, a fourth shell. The browser comes in Phase 4, and the AI Unlock happens at the end of Phase 3.

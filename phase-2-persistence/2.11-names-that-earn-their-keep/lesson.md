# Module 2.11 — Names That Earn Their Keep (M3 close)

The kingdom now has 35-engine-tests, JSON, SQLite, EF Core, migrations, save slots, and an interactive UI. The mechanics work. Today is the deeper craft: a deliberate naming pass over everything you've written across this phase. The thing that turns *"code that works"* into *"code anyone can read in six months — including future-you."*

> **Words to watch**
>
> - **rename party** — a focused session that does *only* renames, nothing else
> - **scope of a name** — local (5 lines) → method (50) → class (500) → module → repo → the whole world. Bigger scope means the name has to do more work.
> - **noise word** — generic words like `Manager`, `Helper`, `Util`, `Data`, `Info` that don't tell you what the thing actually is
> - **earns its keep** — every name you keep should be doing real work for the reader

---

## Why a separate module for naming

Names are the documentation. They're what a reader sees first. A great name makes the surrounding code obvious; a weak name forces you to read the body to understand the place where it's used. Bad names compound — every line that calls `ProcessData(d)` learns nothing from the call.

A *rename party* — doing only renames in one focused session — works because:

- Modern IDEs make renames safe (Refactor → Rename, F2 in VS / Rider).
- A pure-rename PR is easy to review (no logic changes mixed in).
- One sitting catches related names that should change together (`KingdomData` → `KingdomEntity` plus `ToData()` → `ToEntity()`).

You'll do this exercise once per major arc. After three or four rounds, your default names start landing right the first time.

## The five questions

For every public name in your engine + persistence + console, ask:

1. **Does it say what the thing *is*, not what it does to memory?** `Buffer` (does what?) vs `KingdomSnapshotJson` (a JSON-serialised snapshot of a kingdom).
2. **Could a fresh reader guess what it does, given just the name?** `ToSummary()` vs `Convert()`.
3. **Is the scope right?** A 3-letter name is fine for a 5-line method (`b` for building); a class deserves the long form (`KingdomEntity`).
4. **Is there a noise word?** `KingdomManager` — manager of *what*? Drop or replace.
5. **Does it match its neighbours?** If you have `Save`/`Load`, the third method should be `Delete`, not `Remove`. Pick a vocabulary and stick to it.

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

- `Save` / `Load` / `Update` / `Delete` — the standard CRUD vocabulary. They match.
- `ListAll` *vs* `ListSlots` — *both list*. The first returns full entities; the second returns lightweight info. Both names start with `List` (confusing), and `ListAll` is dishonest (it doesn't actually return everything; it doesn't load relations). Better:
  - `ListSlots()` stays.
  - `ListAll()` → either delete (it's only used for tests; can be replaced by `using var ctx; ctx.Kingdoms.ToList();`) or rename to `ListAllEntities()` to be honest about what it returns.

Decision time: drop `ListAll`. The tests don't really need it; `ListSlots` covers the public API.

That's a real, motivated rename — not "for style." The codebase is *one fewer noun* afterward.

## Walkthrough — second example

Look at `KingdomEntity`. Consider:

- `KingdomEntity.cs` and `BuildingEntity.cs` — clear: these are EF entities (DTOs).
- `KingdomSnapshot.cs` (in engine) and `BuildingSnapshot.cs` — both entities-for-data. Why two names?
  - `*Snapshot` is the engine's data form (used by JSON in M2.3).
  - `*Entity` is EF's data form (used by SQLite in M2.6+).
  - **They're not the same thing** — the snapshot has `Kind` and `Citizens[]`; the entity has navigation properties. Different forms for different stores. Both names earn their keep.

Sometimes the right answer is *no rename*.

## The exercise — actually do it in your repo

In one focused 30-minute sitting:

1. Open the engine project. Walk every public type + method. Apply the five questions.
2. Same for persistence.
3. Same for console.
4. Use the IDE's **Rename refactoring** (F2 in VS / Rider; Ctrl+Shift+R in some others). It updates all places where the name is used + tests + comments + XML doc references in one shot. Never search-and-replace by hand for renames — you'll miss a usage.
5. After each rename: `dotnet build` (must still be 0 errors) + `dotnet test` (must still be 71 passing).
6. **Commit at *each* rename**, with prefix `[M3-rename]` — for example, *"[M3-rename] drop KingdomEfStore.ListAll (callers used ListSlots; redundant)"*. (Source Control panel → stage → commit → Sync. Or CLI: `git commit -am "[M3-rename] ..." && git push`.) Small commits. Easy to review. Easy to revert if a rename was wrong.

## Common renames you might do

- `_eventEngine` → `_events` (private field; shorter is fine for short scope)
- `KingdomDbContext.Kingdoms` → fine; conventional EF DbSet plural
- `KingdomEfStore.EnsureCreated()` → fine name even though it now `.Migrate()`s; the *contract* hasn't changed
- `KingdomSummary.BuildingCount` → fine; explicit
- Any class ending in `Manager`, `Helper`, `Util`, `Data` — review hard
- Any property named `Info` — what kind of info? Make it specific.

## Delta starter

This module ships only:

- `M3-rename-checklist.md` — a checkbox list of common rename targets in *your* repo
- `wins.md.append.md` — a one-paragraph M3 entry for your wins log

There is no code change to ship — the renames you do are **specific to your code**.

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

## Tinker

Read your most recent commit message. Is it specific? *"refactor"* says nothing; *"drop ListAll, keep ListSlots — same callers, less surface"* tells the story.

Pick the worst-named thing in your repo (you'll know which one). Rename it. Notice the read-fluency improvement at every place where it's used.

Try the opposite — rename `Kingdom` to `K` everywhere. Save in a branch. Read your code with that name. Notice how much harder it is. Long names earn their keep when scope is large.

Read [`Naming Things`](https://en.wikipedia.org/wiki/Naming_(parameter)) on your own time. The classic essays compound for years.

## What you just did

You sat with the codebase and tightened every public name. Some you renamed; some you dropped; some you left alone with new respect. The code didn't get smarter — but a future reader will hit half as many surprises. You also closed M3: tests still 71 passing, persistence works across four shells (text, JSON, SQLite, EF Core), the kingdom remembers itself across sessions. That's the milestone. Don't skip the ritual that follows.

**Key concepts you can now name:**

- **rename party** — a focused session of only renames
- **scope of a name** — short for short scope; long for long scope
- **noise word** — `Manager`/`Helper`/`Util`/`Info` to review hard
- **vocabulary discipline** — pick `Save`/`Load`/`Delete`, not a mix
- **IDE Rename** — the only safe way to change a name everywhere

## Quiz

Open `quiz.md`. (Lighter than usual — naming-themed.) When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.

---

> **You just shipped M3.** Time for the ritual:
>
> 1. **README refresh** — re-walk the four sections from M0.4 (*what / how to run / what I learned / what's next*). Since M2 close, you added persistence in four backends and a save-slot picker; *How to run* and *What I learned* both need a paragraph that didn't exist before. Polishing the README is a milestone discipline — every milestone close from here on circles back to it.
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

**Phase 3 begins.** It introduces the **web API** — your engine, served over HTTP. Same engine again, fourth shell. The browser will follow in Phase 4; the AI Unlock fires at the end of Phase 3.

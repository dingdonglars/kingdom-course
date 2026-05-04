# Module 2.11 — Names That Earn Their Keep (M3 close)

> **Hook:** the kingdom now has 35-engine-tests, JSON, SQLite, EF Core, migrations, save slots, an interactive UI. **The mechanics work.** Today is the deeper craft: a deliberate naming pass over everything you've written across Block 4. The thing that turns "code that works" into "code anyone can read in six months — including future-you."

> **Words to watch**
> - **rename party** — a focused session that does *only* renames, nothing else
> - **scope of a name** — local (5 lines) → method (50) → class (500) → module → repo → the whole world. Bigger scope = name does more work.
> - **noise word** — generic words like `Manager`, `Helper`, `Util`, `Data`, `Info` that don't tell you what the thing actually is
> - **earn its keep** — every name you keep should be doing real work for the reader

---

## Why a separate module for naming

Names are the documentation. They're what a reader sees first. **A great name makes the surrounding code obvious; a weak name forces you to read the body to understand the call site.** And bad names compound — every caller of `ProcessData(d)` learns nothing from the call.

A *rename party* — doing only renames in one focused session — works because:
- Modern IDEs make renames safe (Refactor → Rename, F2 in VS / Rider).
- A pure-rename PR is easy to review (no logic changes).
- One sitting catches related names that should change together (`KingdomData` → `KingdomEntity` plus `ToData()` → `ToEntity()`).

You'll do this exercise once per major arc. After 3-4 rounds, your default names start landing right the first time.

## The five questions

For every public name in your engine + persistence + console, ask:

1. **Does it say what the thing *is*, not what it *does* to memory?** `Buffer` (does what?) vs `KingdomSnapshotJson` (a JSON-serialised snapshot of a kingdom).
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

Six methods. Read it again with the questions:

- `Save` / `Load` / `Update` / `Delete` — the standard CRUD vocabulary. Match.
- `ListAll` *vs* `ListSlots` — *both list*. The first returns full entities; the second returns lightweight info. **Both names start with `List` — confusing — and `ListAll` is dishonest** (it doesn't list everything; it doesn't load relations). Better:
  - `ListSlots()` stays.
  - `ListAll()` → either delete (it's only used for tests; can be replaced by `using var ctx; ctx.Kingdoms.ToList();`) or rename to `ListAllEntities()` to be honest about what it returns.

**Decision time:** drop `ListAll`. The tests don't really need it; `ListSlots` covers the public API.

That's a real, motivated rename — not "for style." The codebase is *one fewer noun* afterward.

## Walkthrough — second example

Look at `KingdomEntity`. Consider:
- `KingdomEntity.cs` and `BuildingEntity.cs` — clear: these are EF entities (DTOs).
- `KingdomSnapshot.cs` (in engine) and `BuildingSnapshot.cs` — both entities-for-data. Why two names?
  - `*Snapshot` is the engine's data shape (used by JSON in M2.3).
  - `*Entity` is EF's data shape (used by SQLite in M2.6+).
  - **They're not the same thing** — the snapshot has `Kind` and `Citizens[]`; the entity has navigation properties. Different shapes for different stores. **Both names earn their keep.**

Sometimes the right answer is *no rename*.

## The exercise — actually do it in your repo

In one focused 30-minute sitting:

1. Open the engine project. Walk every public type + method. Apply the five questions.
2. Same for persistence.
3. Same for console.
4. Use the IDE's **Rename refactoring** (F2 in VS / Rider; Ctrl+Shift+R in some others). It updates all call sites + tests + comments + XML doc references in one shot. **Never search-and-replace by hand for renames** — you'll miss a usage.
5. After each rename: `dotnet build` (must still be 0 errors) + `dotnet test` (must still be 71 passing).
6. Commit at *each* rename, with prefix `[M3-rename]`:
   ```
   git commit -am "[M3-rename] drop KingdomEfStore.ListAll (callers used ListSlots; redundant)"
   ```
   Small commits. Easy to review. Easy to revert if a rename was wrong.

## Common renames you might do

- `_eventEngine` → `_events` (private field; shorter is fine for short scope)
- `KingdomDbContext.Kingdoms` → fine; conventional EF DbSet pluralised name
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
5. Post your before/after to `#wins` on Discord.
6. Tag locally: `git tag m3-block-4-complete` (then `git push origin m3-block-4-complete` if your remote is set up).

## Tinker

- Read your most recent commit message. Is it specific? *"refactor"* is a noise word; *"drop ListAll, keep ListSlots — same callers, less surface"* tells the story.
- Pick the worst-named thing in your repo (you'll know which one). Rename it. Notice the read-fluency improvement at every call site.
- Try the opposite — rename `Kingdom` to `K` everywhere. **Save in a branch.** Read your code with that name. Notice how much harder it is. *Long names earn their keep when scope is large.*
- Read [`Naming Things`](https://en.wikipedia.org/wiki/Naming_(parameter)) on your own time. The classic essays compound for years.

## Name it (the meta-name-it)

- **Rename party.** A focused session of *only* renames. One sitting, lots of small commits.
- **Scope of a name.** Local (5-line) names can be terse; class/module/repo names earn their keep at length.
- **Noise word.** Words like `Manager`, `Helper`, `Util`, `Data`, `Info` that say "thing" in a fancy hat. Replace with what the thing actually is.
- **Vocabulary discipline.** `Save`/`Load`/`Update`/`Delete`/`List` together. Don't mix `Save`/`Read`/`Modify`/`Remove`.

## The rule of the through-line

> **Every name pays a cost (the reader has to learn it) and earns a benefit (it tells the reader what the thing is). Drop the names whose benefit is less than their cost. Improve the names whose benefit could be much higher.**

Naming is the cheapest cleanup with the highest reader return. Do it now while the codebase is small and you remember why.

## Quiz / challenge

Open `quiz.md`. (Lighter than usual — naming-themed.)

## Block 4 wins log

```markdown
## M3 — Block 4 — Persistence

- 71 tests, deterministic, across engine + persistence
- Same engine now savable in 4 ways: text file, JSON, raw SQLite, EF Core
- Real save-slot UI; you can play across sessions

Before: `Console.WriteLine($"Day {kingdom.Day}");` and the kingdom dies on close
After:  Save, quit, reopen days later — your kingdom is exactly where you left it

Posted to #wins on YYYY-MM-DD.
```

## Connect

**Phase 3 begins.** Block 5 introduces the **web API** — your engine, served over HTTP. Same engine again, fourth shell. The browser will follow in Phase 4; the AI Unlock Gate fires at the end of Block 5.
# Quiz — Module 2.7

## 1. Why can't we keep using `EnsureCreated()` once the app ships?

a. It's slow
b. It only works on an empty database. If the schema needs to change after data exists, `EnsureCreated` does nothing — and you can't drop the database without losing player data.
c. It's deprecated
d. It uses too much memory

## 2. What does `dotnet ef migrations add InitialCreate` produce?

a. A new database file
b. C# files in `Migrations/` describing the schema change as `Up` (apply) and `Down` (revert), plus a model snapshot
c. Raw SQL files
d. Nothing visible

## 3. What's the `__EFMigrationsHistory` table for?

a. Logging errors
b. EF's bookkeeping — tracks which migrations have been applied to this database, so the next `Migrate()` only runs new ones
c. Performance metrics
d. Optional debug info

## 4. Why don't `EnsureCreated` and `Migrate` mix?

a. They're identical
b. Once a database is created via `EnsureCreated`, EF refuses to apply migrations to it (and vice versa). Pick one strategy per project.
c. They use different SQL
d. Performance

## 5. What's "schema drift"?

a. SQL injection
b. The state where the database schema diverges from the code's model — the bug migrations exist to prevent
c. A type of error
d. Nothing — just a name

---

When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.

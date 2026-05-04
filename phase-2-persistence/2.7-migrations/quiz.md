# Quiz — Module 2.7

> Don't write your answers in this file — open `journal/quiz-notes.md` and write them there.

## 1. Why can't we keep using `EnsureCreated()` once the app ships?

- **a.** It runs slower than migrations on real machines
- **b.** It only works on an empty database; once data exists, schema changes can't be applied without dropping it
- **c.** It was deprecated in EF Core 9 and removed in EF Core 10
- **d.** It uses too much memory on production servers

## 2. What does `dotnet ef migrations add InitialCreate` produce?

- **a.** A new database file with the schema applied
- **b.** C# files in `Migrations/` describing the change as `Up`/`Down`, plus a model snapshot
- **c.** Raw SQL files matching the dialect of the configured provider
- **d.** Nothing visible until you also run `database update`

## 3. What's the `__EFMigrationsHistory` table for?

- **a.** Logging migration errors that happened during apply
- **b.** EF's bookkeeping — tracks which migrations have been applied to this database
- **c.** Storing performance metrics for query tuning
- **d.** Holding optional debug information for the EF designer

## 4. Why don't `EnsureCreated` and `Migrate` mix?

- **a.** They are functionally identical and EF picks one for you
- **b.** Once a database is created via `EnsureCreated`, it has no migration history; `Migrate` then thinks every migration is new
- **c.** They use incompatible SQL dialects internally
- **d.** Performance — running both is twice as slow as running one

## 5. What's *schema drift*?

- **a.** Another name for SQL injection
- **b.** The state where the database schema diverges from the code's model — the bug migrations exist to prevent
- **c.** A specific kind of EF Core error code
- **d.** Nothing real; just a name with no underlying concept

---

When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.

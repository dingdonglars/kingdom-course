# Quiz — Module 2.8

> Don't write your answers in this file — open `journal/quiz-notes.md` and write them there.
>
> Travou no inglês? Abra o `quiz.pt.md` — é este mesmo quiz em português. Tente em inglês primeiro.

## 1. Why install a database GUI when you have EF Core in your code?

- **a.** EF refuses to connect without one running alongside it
- **b.** EF lets you read and write data; a GUI lets you *see* what's actually in the DB right now
- **c.** They are functionally identical and the GUI just looks nicer
- **d.** A GUI is required by the .NET runtime for SQLite support

## 2. What does `.schema kingdoms` show in `sqlite3`?

- **a.** The data inside the kingdoms table, formatted as rows
- **b.** The CREATE TABLE statement — the structure of the table
- **c.** The indexes defined on the table, but not the columns
- **d.** Nothing useful; the command exists for backwards compatibility

## 3. What does `dotnet ef migrations script` do?

- **a.** Runs all pending migrations against the current database
- **b.** Outputs the SQL the migrations *would* run, without running them
- **c.** Deletes the migration files from the project
- **d.** Lists the migration files by name in the order they were added

## 4. Where in a SQLite database does EF track which migrations have been applied?

- **a.** In a sidecar file next to the .db
- **b.** In a special table called `__EFMigrationsHistory`
- **c.** In memory only — the list is rebuilt every connection
- **d.** Embedded in the connection string at build time

## 5. The lesson says *"have a window into your database before you need one."* Why?

- **a.** Future-proofing — when something breaks, you want the diagnostic tool already installed and known
- **b.** .NET requires a tool to be configured before EF will connect
- **c.** It's part of the M3 milestone challenge requirements
- **d.** Pure cosmetic preference; no real practical reason

---

When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.

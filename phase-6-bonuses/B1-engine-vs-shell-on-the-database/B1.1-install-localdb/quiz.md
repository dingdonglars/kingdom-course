# Quiz — Bonus B1.1

> Don't write your answers in this file — open `journal/quiz-notes.md` and write them there.

## 1. What is LocalDB?

- **a.** A NoSQL database for storing JSON documents on a single machine
- **b.** The developer edition of SQL Server — single-user, one instance per user, no service to manage
- **c.** A drop-in replacement for SQLite written by Microsoft for cross-platform use
- **d.** A logging tool that captures queries against a remote SQL Server

## 2. Why is the swap from SQLite to SQL Server set up as the lesson at all?

- **a.** Because SQLite is unsuitable for production and we have to switch off it before shipping
- **b.** Because the engine-vs-shell rule predicts the swap will be small — and proving it small is the whole point
- **c.** Because LocalDB is faster than SQLite for the kingdom workload
- **d.** Because Microsoft pays better than the SQLite team for tutorials

## 3. The connection string for LocalDB starts with...

- **a.** `Server=(localdb)\MSSQLLocalDB;Database=...`
- **b.** `Server=localhost:5432;Database=...`
- **c.** `Data Source=:memory:;Mode=Shared`
- **d.** `mongodb://localhost:27017/kingdom`

## 4. Why is LocalDB Windows-only?

- **a.** Microsoft hasn't ported it; on macOS or Linux you run the SQL Server Docker image instead
- **b.** Performance reasons — the codebase relies on a Windows-specific kernel feature
- **c.** It's a license restriction; Microsoft sells the macOS edition separately
- **d.** Tradition; nobody has asked Microsoft for a port

## 5. What does `sqllocaldb info` do?

- **a.** Prints the LocalDB version number for the installed package
- **b.** Lists the LocalDB instances on this machine (default instance is `MSSQLLocalDB`)
- **c.** Runs a small performance test against the default instance
- **d.** Required at install time; sets up the system tables

---

When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.

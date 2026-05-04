# Quiz — Module 2.4

> Don't write your answers in this file — open `journal/quiz-notes.md` and write them there.

## 1. SQLite is best described as...

- **a.** A web service you connect to over HTTPS
- **b.** A database server like PostgreSQL or MySQL, but lighter
- **c.** A library plus a single-file database — no server, no install
- **d.** A backup tool for moving SQL between systems

## 2. Why use parameters (`$name`) instead of string concatenation in SQL?

- **a.** Parameters perform faster on the database side
- **b.** To prevent SQL injection — pasted user input lets an attacker run arbitrary SQL
- **c.** The C# compiler refuses concatenated SQL strings
- **d.** SQLite throws an error on concatenated commands

## 3. Which of `CREATE` / `INSERT` / `SELECT` / `UPDATE` / `DELETE` returns rows?

- **a.** `INSERT` — returns the rows it just inserted
- **b.** `UPDATE` — returns the rows it modified
- **c.** `SELECT` — returns the rows that match the query
- **d.** `CREATE` — returns the structure of the new table

## 4. Why is `using var conn = new SqliteConnection(...)` important?

- **a.** It opens the connection automatically when execution reaches the line
- **b.** It guarantees `Dispose` runs (closing the connection) even if an exception is thrown
- **c.** It enables async query support without extra setup
- **d.** It is purely cosmetic and could be omitted

## 5. The lesson says *"the database is a runtime."* What does that mean here?

- **a.** SQLite ships with its own command-line shell program
- **b.** Saving is a runtime concern, not an engine one — same engine, different storage backend
- **c.** SQL queries run inside an embedded scripting environment
- **d.** Nothing significant; just terminology

---

When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.

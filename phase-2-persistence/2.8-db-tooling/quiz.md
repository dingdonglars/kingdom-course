# Quiz — Module 2.8

## 1. Why install a database GUI when you have EF Core in your code?

a. EF doesn't work without one
b. EF lets you read/write data; a GUI lets you *see* what's actually in the DB right now — invaluable for debugging "is it broken on the read or the write?"
c. They're identical
d. Required by the .NET runtime

## 2. What does `.schema kingdoms` show in `sqlite3`?

a. The data
b. The CREATE TABLE statement (the structure of the table)
c. Indexes only
d. Nothing

## 3. What does `dotnet ef migrations script` do?

a. Runs the migrations
b. Outputs the SQL the migrations would run (without running it) — useful for preview, code review, sharing with a DBA
c. Deletes migrations
d. Lists migration files

## 4. Where in a SQLite database does EF track which migrations have been applied?

a. A file next to the .db
b. A special table called `__EFMigrationsHistory`
c. In memory only
d. In the connection string

## 5. The lesson says "have a window into your database before you need one." Why?

a. Future-proofing — when something breaks, you want the diagnostic tool already installed and known. Investigation under stress is the worst time to learn a new tool.
b. Required by .NET
c. To pass the M3 challenge
d. To impress reviewers

---

When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.

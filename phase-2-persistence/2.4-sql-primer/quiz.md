# Quiz — Module 2.4

## 1. SQLite is best described as...

a. A web service
b. A database server like PostgreSQL or MySQL
c. A library + a single-file database — no server, no install
d. A backup tool

## 2. Why use parameters (`$name`) instead of string concatenation in SQL?

a. Parameters are faster
b. To prevent SQL injection — concatenated user input lets attackers run arbitrary SQL
c. The compiler enforces it
d. Required by SQLite

## 3. Which of these CREATE / INSERT / SELECT / UPDATE / DELETE returns rows?

a. INSERT
b. UPDATE
c. SELECT
d. CREATE

## 4. Why is `using var conn = new SqliteConnection(...)` important?

a. It opens the connection automatically
b. It ensures `Dispose` runs (which closes the connection) even if an exception is thrown — no leaked handles
c. It enables async support
d. It's optional decoration

## 5. The lesson says "the database is a shell." What does that mean here?

a. SQLite has a CLI shell
b. Persistence is a shell concern, not an engine concern. The engine is unchanged when storage moves from JSON to SQLite — same model, different runtime.
c. SQL runs in a shell environment
d. Nothing significant

---

When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.

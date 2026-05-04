# Quiz — B1.2

## 1. How many *real* lines of code change to swap SQLite for SQL Server?

a. ~3 — package + provider line in `OnConfiguring` + connection string format
b. ~50
c. Hundreds
d. The entire codebase

## 2. Why must migrations be regenerated when changing providers?

a. Migrations contain provider-specific SQL (e.g., AUTOINCREMENT vs IDENTITY); the same C# `Add Column` produces different SQL output per provider
b. Performance
c. EF requires it
d. Style

## 3. The lesson calls the result "anticlimactic." That's the point. Why?

a. The engine-vs-shell discipline predicts the swap is small. The boredom IS the proof. If it had taken weeks, the discipline would have been a lie.
b. To save time
c. SQL Server is boring
d. EF is boring

## 4. The same three-line pattern works for...

a. Only SQL Server
b. SQL Server, PostgreSQL, MySQL, anything EF Core has a provider for
c. Only SQLite + SQL Server
d. Only Microsoft databases

## 5. Why is "your tests pass unchanged" the test that matters here?

a. Tests verify behavior, not implementation. If they pass after the swap, the engine sees the same kingdom — proof the discipline held.
b. Required by the lesson
c. Performance
d. Style
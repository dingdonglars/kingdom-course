# Module 2.4 — SQL Primer (with SQLite)

JSON files are great for *one* save. But what if you want a *list* of all your saved kingdoms? What if you want to ask *"give me the kingdom with the most gold"*? Files force you to load everything to find anything. Today we meet **the database** — and the language databases speak: **SQL**. We use **SQLite** *(es-queue-el-ite)*, a database that lives in a single `.db` file, no server, no install.

> **Words to watch**
>
> - **database** — a structured store of data that you can query
> - **table** — a grid of rows and columns; the unit of storage in a relational database
> - **column** — a typed field in a table (e.g. `Name TEXT`, `Day INTEGER`)
> - **row** — one record in a table
> - **SQL** *(see-quel)* — *Structured Query Language*. The language for talking to relational databases.
> - **SQLite** — a self-contained SQL database engine; one library, zero servers, the database is a file
> - **CREATE / INSERT / SELECT / UPDATE / DELETE** — the five most-used SQL commands

---

## Why a database

A file holds *one* thing well. A database holds *many* things and lets you ask questions about them.

Suppose you save 100 kingdoms over a year. With files: 100 JSON files sorted by filename. *"Find the one with the most gold"* means open all 100, parse each, compare. With a database: `SELECT name FROM kingdoms ORDER BY gold DESC LIMIT 1;`. One line. Milliseconds.

That's why every non-trivial application uses one. The harder question is *which* database. For a kingdom that lives on your laptop, SQLite is perfect: no server to run, no port to open, the entire database is a single file you can copy around.

## SQLite, in one paragraph

SQLite is a **library**, not a server. You include it in your project, point it at a path, and it manages everything. It's used everywhere — your phone, your browser, every Mac, your TV. Probably the most-deployed software of all time. It runs at real production scale (Stack Overflow uses it for some things), but it's also perfect for a single-player game or a learning exercise.

## The five SQL commands

```sql
-- Create a table (define columns and their types)
CREATE TABLE kingdoms (
    id    INTEGER PRIMARY KEY AUTOINCREMENT,
    name  TEXT NOT NULL,
    day   INTEGER NOT NULL,
    gold  INTEGER NOT NULL
);

-- Insert a row
INSERT INTO kingdoms (name, day, gold) VALUES ('Eldoria', 11, 250);

-- Read all rows
SELECT id, name, day, gold FROM kingdoms;

-- Read with a filter
SELECT * FROM kingdoms WHERE gold > 100;

-- Update rows
UPDATE kingdoms SET gold = gold + 50 WHERE name = 'Eldoria';

-- Delete rows
DELETE FROM kingdoms WHERE gold = 0;
```

That's 80% of SQL. The other 20% is JOINs (Module 2.5), aggregates (`COUNT`, `SUM`), and indexes (later).

## Delta starter

- **MODIFIED:** `Kingdom.Persistence/Kingdom.Persistence.csproj` (adds `Microsoft.Data.Sqlite` package)
- **NEW:** `Kingdom.Persistence/SqliteDemo.cs` (small class demonstrating connect + CREATE + INSERT + SELECT)
- **MODIFIED:** `Kingdom.Console/Program.cs` (calls the demo)
- **NEW:** `tests/Kingdom.Persistence.Tests/SqliteDemoTests.cs`

Engine code unchanged. JSON code unchanged. SQLite is purely additive — a *new* save option living alongside JSON.

## Step 0 — install the package

```powershell
cd Kingdom.Persistence
dotnet add package Microsoft.Data.Sqlite
```

This brings in `SqliteConnection` and friends.

## Step 1 — `SqliteDemo`

`Kingdom.Persistence/SqliteDemo.cs`:

```csharp
using Microsoft.Data.Sqlite;

namespace Kingdom.Persistence;

public static class SqliteDemo
{
    /// <summary>Run a tiny end-to-end sequence: open, create, insert, query.</summary>
    public static IReadOnlyList<(int Id, string Name, int Day, int Gold)> RunDemo(string dbPath)
    {
        // The connection string says "the file is at <path>".
        // Pooling=False so the OS file handle releases on Dispose.
        // (Pooling is great in production but keeps the file locked, which bites in tests.)
        var connStr = $"Data Source={dbPath};Pooling=False";

        using var conn = new SqliteConnection(connStr);
        conn.Open();

        // CREATE TABLE — IF NOT EXISTS makes it idempotent
        using (var cmd = conn.CreateCommand())
        {
            cmd.CommandText = @"
                CREATE TABLE IF NOT EXISTS kingdoms (
                    id   INTEGER PRIMARY KEY AUTOINCREMENT,
                    name TEXT NOT NULL,
                    day  INTEGER NOT NULL,
                    gold INTEGER NOT NULL
                );
            ";
            cmd.ExecuteNonQuery();
        }

        // INSERT — note: parameters, not string concatenation (SQL injection safety!)
        using (var cmd = conn.CreateCommand())
        {
            cmd.CommandText = "INSERT INTO kingdoms (name, day, gold) VALUES ($name, $day, $gold)";
            cmd.Parameters.AddWithValue("$name", "Eldoria");
            cmd.Parameters.AddWithValue("$day", 11);
            cmd.Parameters.AddWithValue("$gold", 250);
            cmd.ExecuteNonQuery();
        }

        // SELECT — iterate the rows
        var results = new List<(int, string, int, int)>();
        using (var cmd = conn.CreateCommand())
        {
            cmd.CommandText = "SELECT id, name, day, gold FROM kingdoms ORDER BY id";
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                results.Add((
                    reader.GetInt32(0),
                    reader.GetString(1),
                    reader.GetInt32(2),
                    reader.GetInt32(3)));
            }
        }

        return results;
    }
}
```

Three things to read carefully:

1. **`using var conn = new SqliteConnection(...)`.** `using` ensures `Dispose` runs automatically (which closes the connection). Always `using` for connections, commands, readers — otherwise you'll leak file handles.
2. **`$name`, `$day`, `$gold` parameters.** *Never* paste user input into SQL with string concatenation — that's SQL injection. Use parameters; SQLite quotes and escapes them safely.
3. **`reader.GetInt32(0)`.** Read the first column as an int. The reader is a one-row-at-a-time cursor.

## Step 2 — call it from the console

`Kingdom.Console/Program.cs` — append at the bottom:

```csharp
// SQLite demo (Module 2.4)
var dbPath = Path.Combine(saveFolder, "kingdoms.db");
if (File.Exists(dbPath)) File.Delete(dbPath);   // start fresh each run
var rows = SqliteDemo.RunDemo(dbPath);

Console.WriteLine();
Console.WriteLine($"=== SQLite demo — {rows.Count} row(s) ===");
foreach (var (id, name, day, gold) in rows)
    Console.WriteLine($"  #{id}  {name}, day {day}, gold {gold}");
```

Run:

```powershell
dotnet build
dotnet run --project Kingdom.Console
```

You'll see the row come out of the database. Find the file:

```
bin/Debug/net10.0/saves/kingdoms.db
```

You can open it in any SQLite browser (DB Browser for SQLite is free). One file, queryable forever.

## Step 3 — tests

`tests/Kingdom.Persistence.Tests/SqliteDemoTests.cs`:

```csharp
using Kingdom.Persistence;
using Shouldly;

namespace Kingdom.Persistence.Tests;

public class SqliteDemoTests
{
    [Fact]
    public void RunDemo_FirstRun_ReturnsOneRow()
    {
        var path = Path.Combine(Path.GetTempPath(), $"kdb-{Guid.NewGuid():N}.db");
        try
        {
            var rows = SqliteDemo.RunDemo(path);
            rows.Count.ShouldBe(1);
            rows[0].Name.ShouldBe("Eldoria");
            rows[0].Day.ShouldBe(11);
            rows[0].Gold.ShouldBe(250);
        }
        finally { if (File.Exists(path)) File.Delete(path); }
    }

    [Fact]
    public void RunDemo_TwoRuns_AccumulatesRows()
    {
        var path = Path.Combine(Path.GetTempPath(), $"kdb-{Guid.NewGuid():N}.db");
        try
        {
            SqliteDemo.RunDemo(path);
            var rows = SqliteDemo.RunDemo(path);   // CREATE IF NOT EXISTS, second INSERT
            rows.Count.ShouldBe(2);
            rows[0].Id.ShouldBe(1);
            rows[1].Id.ShouldBe(2);
        }
        finally { if (File.Exists(path)) File.Delete(path); }
    }

    [Fact]
    public void DatabaseFile_IsCreated_OnFirstRun()
    {
        var path = Path.Combine(Path.GetTempPath(), $"kdb-{Guid.NewGuid():N}.db");
        try
        {
            File.Exists(path).ShouldBeFalse();
            SqliteDemo.RunDemo(path);
            File.Exists(path).ShouldBeTrue();
        }
        finally { if (File.Exists(path)) File.Delete(path); }
    }
}
```

Run:

```powershell
dotnet test
```

Expect `Passed: 54` (51 + 3).

## Tinker

Add a column: `wood INTEGER NOT NULL DEFAULT 0`. `ALTER TABLE` works — the existing rows get the default. (We'll cover migrations properly in Module 2.7.)

Run a query directly via the `sqlite3` CLI: `sqlite3 saves/kingdoms.db "SELECT * FROM kingdoms"`. Same data, different tool.

Try concatenating user input: `cmd.CommandText = "INSERT INTO kingdoms (name) VALUES ('" + userInput + "')";`. If `userInput` is `'); DROP TABLE kingdoms; --`, your data dies. That's SQL injection — and exactly why we always use parameters.

Insert 10,000 rows in a loop. Notice how fast SQLite is on small data. (For *bulk* inserts, wrap in `BEGIN TRANSACTION ... COMMIT` — a 100x speedup.)

## What you just did

Your kingdom now has a third place to live: a SQLite database file. You wrote your first `CREATE TABLE`, your first parametrised `INSERT`, your first `SELECT` reader loop — and three tests to prove the round trip (54 passing total). Along the way you met the five SQL commands (`CREATE`, `INSERT`, `SELECT`, `UPDATE`, `DELETE`) which between them cover most of what you'll write for the rest of the year. You also met the most-published security bug in the world — *SQL injection* — and saw why parameters, never concatenation, are the only safe answer. The engine and the JSON code didn't change today; that's the third shell over the same engine.

**Key concepts you can now name:**

- **SQL** — the language relational databases speak
- **SQLite** — a library + a single-file database
- **CREATE / INSERT / SELECT / UPDATE / DELETE** — the five core commands
- **parameters (`$name`)** — the only safe way to put user input in SQL
- **`using var conn`** — guarantees the connection closes on scope exit

## Wrap up

1. **Quiz** — open `quiz.md`, jot your answers in `journal/quiz-notes.md`.
2. **Progress** — one line in `journal/progress.md`: `Module 2.4 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit and push** — stage both files, commit message `Module 2.4 done`, Sync.
4. **Post in `#wins`** — one line about today, plus the URL of the commit.

Module 0.1 covers the why and the panel/CLI steps if you need a refresher. Bring quiz answers you're least sure about to the next weekly sync.

## Next

Module 2.5 introduces **JOINs** — the SQL feature that makes relational databases *relational*. We'll add a `buildings` table with a foreign key to `kingdoms`, and learn how to query across them.

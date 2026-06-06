# Module 2.4 — SQL Primer (with SQLite)

JSON files are great for *one* save. But what if you want a *list* of all your saved kingdoms? What if you want to ask *"which kingdom has the most gold?"* With files, you have to load everything just to find one thing. Today we meet **the database**, and the language databases speak: **SQL**. We use **SQLite** *(es-queue-el-ite)*, a database that lives in a single `.db` file — no server to run, nothing to install.

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

Say you save 100 kingdoms over a year. With files, you have 100 JSON files sorted by name. To *"find the one with the most gold"*, you have to open all 100, read each one, and compare them. With a database, it's one line: `SELECT name FROM kingdoms ORDER BY gold DESC LIMIT 1;`. It finishes in a few milliseconds.

That's why almost every real program uses a database. The harder question is *which* one. For a kingdom that lives on your laptop, SQLite is a great fit: no server to run, no port to open, and the whole database is a single file you can copy anywhere.

## SQLite, in one paragraph

SQLite is a **library**, not a server. You add it to your project, tell it where the file is, and it handles the rest. It's used everywhere — your phone, your browser, every Mac, your TV. It may be the most-installed software ever made. It can handle real, large workloads (Stack Overflow uses it for some tasks), but it's also a great fit for a single-player game or a learning project.

## What a table looks like

A database keeps data in **tables** — grids of rows and columns. One table for your saved kingdoms might look like this:

```text
   a table called  kingdoms
   +----+-----------+-----+------+
   | id | name      | day | gold |   <- columns: each one has a type
   +----+-----------+-----+------+
   |  1 | Eldoria   |  12 |  340 |   <- a row: one saved kingdom
   |  2 | Stormhold |   4 |   90 |   <- another row
   +----+-----------+-----+------+
```

Each **row** is one saved kingdom. Each **column** is one fact about it, with a fixed type (`name` is text, `gold` is a whole number). SQL — the five commands below — is how you add rows, change them, and ask questions across the whole grid without loading it all yourself.

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

That's most of SQL right there. The rest is JOINs (Module 2.5), aggregates (`COUNT`, `SUM`), and indexes (later).

## Delta starter

- **MODIFIED:** `Kingdom.Persistence/Kingdom.Persistence.csproj` (adds `Microsoft.Data.Sqlite` package)
- **NEW:** `Kingdom.Persistence/SqliteDemo.cs` (small class demonstrating connect + CREATE + INSERT + SELECT)
- **MODIFIED:** `Kingdom.Console/Program.cs` (calls the demo)
- **NEW:** `tests/Kingdom.Persistence.Tests/SqliteDemoTests.cs`

Engine code unchanged. JSON code unchanged. SQLite only adds something new — a second way to save, right next to JSON.

## Step 0 — install the package

```powershell
cd Kingdom.Persistence
dotnet add package Microsoft.Data.Sqlite
```

This adds `SqliteConnection` and the related classes to your project.

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

1. **`using var conn = new SqliteConnection(...)`.** The `using` keyword makes sure `Dispose` runs by itself, which closes the connection. Always use `using` for connections, commands, and readers. If you don't, the program keeps file handles open that it should have released.
2. **`$name`, `$day`, `$gold` parameters.** *Never* paste user input straight into a SQL string. That opens you up to SQL injection (more on this below). Use parameters instead. SQLite quotes and escapes them for you, safely.
3. **`reader.GetInt32(0)`.** This reads the first column as an int. The reader hands you one row at a time, in order.

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

You'll see the row come back out of the database. Find the file here:

```
bin/Debug/net10.0/saves/kingdoms.db
```

You can open it in any SQLite browser (DB Browser for SQLite is free). One file, and you can ask it questions any time.

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

Add a column: `wood INTEGER NOT NULL DEFAULT 0`. `ALTER TABLE` works — the rows that already exist get the default value. (We'll cover migrations properly in Module 2.7.)

Run a query straight from the `sqlite3` command line: `sqlite3 saves/kingdoms.db "SELECT * FROM kingdoms"`. Same data, a different tool.

Try pasting user input into the SQL string: `cmd.CommandText = "INSERT INTO kingdoms (name) VALUES ('" + userInput + "')";`. If `userInput` is `'); DROP TABLE kingdoms; --`, your whole table gets deleted. That's SQL injection — and exactly why we always use parameters.

Insert 10,000 rows in a loop. See how fast SQLite is on small amounts of data. (For big batches of inserts, put them inside `BEGIN TRANSACTION ... COMMIT` — that makes it about 100 times faster.)

## What you just did

Your kingdom now has a third place to live: a SQLite database file. You wrote your first `CREATE TABLE`, your first `INSERT` with parameters, and your first `SELECT` reader loop — plus three tests to prove the round trip (54 passing total). Along the way you met the five SQL commands (`CREATE`, `INSERT`, `SELECT`, `UPDATE`, `DELETE`), which together cover most of what you'll write for the rest of the year. You also met the most common security bug in the world — *SQL injection* — and saw why using parameters, never pasting input into the string, is the only safe answer. The engine and the JSON code didn't change today. This is the third shell over the same engine.

**Key concepts you can now name:**

- **SQL** — the language relational databases speak
- **SQLite** — a library + a single-file database
- **CREATE / INSERT / SELECT / UPDATE / DELETE** — the five core commands
- **parameters (`$name`)** — the only safe way to put user input in SQL
- **`using var conn`** — guarantees the connection closes on scope exit

## On your own

Time to put the book away. Don't scroll back up to the steps — show yourself, from your own head, that the one big idea stuck: write SQL that makes a table, puts a row in it, and reads it back. No one marks this — the database engine does, which is the point. It's the fastest way to spot what hasn't stuck yet, while it's still small to fix. Getting stuck here is completely fine — that's exactly what it's for.

Open the `sqlite3` prompt on a fresh file: `sqlite3 scratch.db`. Without looking, write SQL that:

1. Creates a table called `heroes` — an `id`, a `name`, and a `level`.
2. Inserts one hero.
3. Selects every row back out.

You should see your row come back.

<details><summary>Stuck? Open this to check yourself.</summary>

```sql
CREATE TABLE heroes (
    id    INTEGER PRIMARY KEY AUTOINCREMENT,
    name  TEXT NOT NULL,
    level INTEGER NOT NULL
);

INSERT INTO heroes (name, level) VALUES ('Lyra', 7);

SELECT id, name, level FROM heroes;
```

If the `SELECT` shows your hero, all three commands worked. The same `CREATE` / `INSERT` / `SELECT` shape is most of the SQL you'll write all year.

</details>

## Wrap up

1. **Quiz** — open `quiz.md`, jot your answers in `journal/quiz-notes.md`.
2. **Progress** — one line in `journal/progress.md`: `Module 2.4 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit and push** — stage both files, commit message `Module 2.4 done`, Sync.
4. **Post in `#wins`** — one line about today, plus the URL of the commit.

Module 0.1 covers the why and the panel/CLI steps if you need a refresher. Bring quiz answers you're least sure about to the next weekly sync.

## Next

Module 2.5 introduces **JOINs** — the SQL feature that makes relational databases *relational*. We'll add a `buildings` table that links back to `kingdoms`, and learn how to query across both tables at once.

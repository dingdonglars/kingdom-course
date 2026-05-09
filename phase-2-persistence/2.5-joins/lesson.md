# Module 2.5 — JOINs

In Module 2.4 we had one table. Real apps have ten or fifty. Today we add a `buildings` table that *belongs to* the `kingdoms` table — and learn how to ask *"give me each kingdom along with all its buildings"* in one query. This is what makes a database *relational*.

> **Words to watch**
>
> - **foreign key** — a column whose value is the `id` of a row in *another* table
> - **JOIN** — combine rows from two tables on a matching condition
> - **`INNER JOIN`** — only rows that exist on *both* sides
> - **`LEFT JOIN`** — every row from the left table; matching from the right (or `NULL`)
> - **`GROUP BY`** + aggregates (`COUNT`, `SUM`, `AVG`) — collapse many rows into one per group

---

## Why two tables

Storing buildings inside the `kingdoms` table is awkward. How would you fit a list of buildings into one cell? You'd have to JSON-encode them — which works, but you can't query *inside* JSON cleanly. Each entity gets its own table; relationships are foreign keys.

```
kingdoms                 buildings
========                 =========
id  name      day        id  kingdom_id  kind     name      level
1   Eldoria   11         1   1           Farm     Main      3
2   Briar     7          2   1           Mine     Old       1
                         3   2           Farm     East      2
```

The `buildings.kingdom_id` *points back* at `kingdoms.id`. That's the foreign key. You can now ask: *"all buildings for kingdom 1"* (`WHERE kingdom_id = 1`), *"the kingdom for building 3"* (`JOIN ... ON kingdom_id = id`), and *"each kingdom along with how many buildings it has"* (`JOIN ... GROUP BY ...`).

## The three JOIN types you'll use 99% of the time

```sql
-- INNER JOIN: rows that exist on both sides
SELECT k.name, b.name FROM kingdoms k
INNER JOIN buildings b ON b.kingdom_id = k.id;

-- LEFT JOIN: every kingdom, even those with no buildings (b.* will be NULL)
SELECT k.name, b.name FROM kingdoms k
LEFT JOIN buildings b ON b.kingdom_id = k.id;

-- Aggregates with GROUP BY: one row per kingdom, with building count
SELECT k.name, COUNT(b.id) AS building_count FROM kingdoms k
LEFT JOIN buildings b ON b.kingdom_id = k.id
GROUP BY k.id;
```

`k` and `b` are *aliases* — short names for the tables in the query. Without them you'd write `kingdoms.name` and `buildings.name` everywhere.

## Delta starter

- **NEW:** `Kingdom.Persistence/SqliteJoinsDemo.cs` — sets up two tables, inserts data, runs three queries
- **MODIFIED:** `Kingdom.Console/Program.cs` (calls the new demo)
- **NEW:** `tests/Kingdom.Persistence.Tests/SqliteJoinsDemoTests.cs`

Engine and JSON unchanged.

## Step 1 — `SqliteJoinsDemo`

`Kingdom.Persistence/SqliteJoinsDemo.cs`:

```csharp
using Microsoft.Data.Sqlite;

namespace Kingdom.Persistence;

public static class SqliteJoinsDemo
{
    public record KingdomRow(int Id, string Name);
    public record BuildingRow(int Id, int KingdomId, string Kind, string Name, int Level);
    public record KingdomCount(string Name, int BuildingCount);

    public static (IReadOnlyList<KingdomRow> Kingdoms,
                   IReadOnlyList<BuildingRow> Buildings,
                   IReadOnlyList<KingdomCount> Counts)
        RunDemo(string dbPath)
    {
        var connStr = $"Data Source={dbPath};Pooling=False";
        using var conn = new SqliteConnection(connStr);
        conn.Open();

        // --- schema ---
        Exec(conn, @"
            CREATE TABLE kingdoms (
                id   INTEGER PRIMARY KEY AUTOINCREMENT,
                name TEXT NOT NULL
            );
            CREATE TABLE buildings (
                id          INTEGER PRIMARY KEY AUTOINCREMENT,
                kingdom_id  INTEGER NOT NULL REFERENCES kingdoms(id),
                kind        TEXT NOT NULL,
                name        TEXT NOT NULL,
                level       INTEGER NOT NULL DEFAULT 1
            );
        ");

        // --- seed ---
        var eldoria = InsertKingdom(conn, "Eldoria");
        var briar   = InsertKingdom(conn, "Briarholm");
        var empty   = InsertKingdom(conn, "Stoneholt");        // no buildings — for LEFT JOIN demo
        InsertBuilding(conn, eldoria, "Farm", "Main",      3);
        InsertBuilding(conn, eldoria, "Mine", "Old Vein",  1);
        InsertBuilding(conn, briar,   "Farm", "East Farm", 2);

        // --- queries ---
        var kingdoms = Read(conn, "SELECT id, name FROM kingdoms ORDER BY id",
            r => new KingdomRow(r.GetInt32(0), r.GetString(1)));

        var inner = Read(conn, @"
                SELECT b.id, b.kingdom_id, b.kind, b.name, b.level
                FROM buildings b
                INNER JOIN kingdoms k ON k.id = b.kingdom_id
                ORDER BY b.id",
            r => new BuildingRow(r.GetInt32(0), r.GetInt32(1), r.GetString(2), r.GetString(3), r.GetInt32(4)));

        var counts = Read(conn, @"
                SELECT k.name, COUNT(b.id) AS building_count
                FROM kingdoms k
                LEFT JOIN buildings b ON b.kingdom_id = k.id
                GROUP BY k.id
                ORDER BY k.id",
            r => new KingdomCount(r.GetString(0), r.GetInt32(1)));

        return (kingdoms, inner, counts);
    }

    private static int InsertKingdom(SqliteConnection conn, string name)
    {
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "INSERT INTO kingdoms (name) VALUES ($name); SELECT last_insert_rowid();";
        cmd.Parameters.AddWithValue("$name", name);
        return (int)(long)cmd.ExecuteScalar()!;
    }

    private static void InsertBuilding(SqliteConnection conn, int kingdomId, string kind, string name, int level)
    {
        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            INSERT INTO buildings (kingdom_id, kind, name, level)
            VALUES ($kid, $kind, $name, $level)";
        cmd.Parameters.AddWithValue("$kid", kingdomId);
        cmd.Parameters.AddWithValue("$kind", kind);
        cmd.Parameters.AddWithValue("$name", name);
        cmd.Parameters.AddWithValue("$level", level);
        cmd.ExecuteNonQuery();
    }

    private static void Exec(SqliteConnection conn, string sql)
    {
        using var cmd = conn.CreateCommand();
        cmd.CommandText = sql;
        cmd.ExecuteNonQuery();
    }

    private static List<T> Read<T>(SqliteConnection conn, string sql, Func<SqliteDataReader, T> map)
    {
        using var cmd = conn.CreateCommand();
        cmd.CommandText = sql;
        using var r = cmd.ExecuteReader();
        var list = new List<T>();
        while (r.Read()) list.Add(map(r));
        return list;
    }
}
```

Notice the helpers (`Exec`, `Read<T>`, `InsertKingdom`, `InsertBuilding`). They're not magic — just small reductions of the boilerplate. Once you write SQL three times, extract a helper.

> **Don't worry about the angle brackets yet.** `Read<T>` and `Func<SqliteDataReader, T>` use a feature called *generics* — a way to write one helper that works for any type `T`. We'll formally meet generics later; for now, read `T` as *"whatever type the caller asks for"* and the helper will fill in. The fact that it works is enough.

`last_insert_rowid()` is a SQLite function that returns the autoincrement id of the last `INSERT`. The standard pattern for *"I just inserted; what's its id?"*

## Step 2 — call from console

In `Program.cs`, append:

```csharp
// Joins demo (Module 2.5)
var joinsDb = Path.Combine(saveFolder, "kingdoms-joins.db");
if (File.Exists(joinsDb)) File.Delete(joinsDb);
var (kingdomRows, joinedBuildings, counts) = SqliteJoinsDemo.RunDemo(joinsDb);

Console.WriteLine();
Console.WriteLine($"=== JOINs demo ({joinsDb}) ===");
Console.WriteLine($"Kingdoms ({kingdomRows.Count}):");
foreach (var k in kingdomRows)
    Console.WriteLine($"  #{k.Id} {k.Name}");
Console.WriteLine($"Buildings (INNER JOIN, {joinedBuildings.Count}):");
foreach (var b in joinedBuildings)
    Console.WriteLine($"  #{b.Id}  k{b.KingdomId}  {b.Kind} '{b.Name}' (lvl {b.Level})");
Console.WriteLine($"Counts (LEFT JOIN + GROUP BY):");
foreach (var c in counts)
    Console.WriteLine($"  {c.Name}: {c.BuildingCount} building(s)");
```

`Stoneholt` shows up with `0` because `LEFT JOIN` keeps it; `INNER JOIN` would have dropped it.

Run it and look at the output.

## Step 3 — tests

`tests/Kingdom.Persistence.Tests/SqliteJoinsDemoTests.cs`:

```csharp
using Kingdom.Persistence;
using Shouldly;

namespace Kingdom.Persistence.Tests;

public class SqliteJoinsDemoTests
{
    [Fact]
    public void RunDemo_HasThreeKingdomsAndThreeBuildings()
    {
        var path = Path.Combine(Path.GetTempPath(), $"kjoin-{Guid.NewGuid():N}.db");
        try
        {
            var (kingdoms, buildings, counts) = SqliteJoinsDemo.RunDemo(path);
            kingdoms.Count.ShouldBe(3);
            buildings.Count.ShouldBe(3);   // INNER JOIN: only matched buildings
            counts.Count.ShouldBe(3);      // LEFT JOIN: every kingdom listed
        }
        finally { if (File.Exists(path)) File.Delete(path); }
    }

    [Fact]
    public void Counts_ShowZero_ForKingdomWithNoBuildings()
    {
        var path = Path.Combine(Path.GetTempPath(), $"kjoin-{Guid.NewGuid():N}.db");
        try
        {
            var (_, _, counts) = SqliteJoinsDemo.RunDemo(path);
            counts.Single(c => c.Name == "Stoneholt").BuildingCount.ShouldBe(0);
            counts.Single(c => c.Name == "Eldoria").BuildingCount.ShouldBe(2);
            counts.Single(c => c.Name == "Briarholm").BuildingCount.ShouldBe(1);
        }
        finally { if (File.Exists(path)) File.Delete(path); }
    }

    [Fact]
    public void InnerJoin_OnlyReturnsBuildingsThatHaveAKingdom()
    {
        var path = Path.Combine(Path.GetTempPath(), $"kjoin-{Guid.NewGuid():N}.db");
        try
        {
            var (_, buildings, _) = SqliteJoinsDemo.RunDemo(path);
            buildings.All(b => b.KingdomId > 0).ShouldBeTrue();
            buildings.Select(b => b.KingdomId).ShouldContain(1);   // Eldoria
            buildings.Select(b => b.KingdomId).ShouldContain(2);   // Briarholm
            buildings.Select(b => b.KingdomId).ShouldNotContain(3); // Stoneholt has no buildings
        }
        finally { if (File.Exists(path)) File.Delete(path); }
    }
}
```

Run:

```powershell
dotnet test
```

Expect `Passed: 57` (54 + 3).

## Tinker

Switch the LEFT JOIN to INNER JOIN in the counts query. Stoneholt disappears. That's the difference made visible.

Add a `WHERE k.name LIKE 'B%'` clause. Only Briarholm shows up. `LIKE` + `%` is SQL's wildcard.

Try `SELECT k.name, b.kind, AVG(b.level) FROM kingdoms k JOIN buildings b ON b.kingdom_id = k.id GROUP BY k.id, b.kind`. Aggregate by *two* dimensions at once.

Drop the foreign key reference: `kingdom_id INTEGER NOT NULL` (without `REFERENCES kingdoms(id)`). Insert a building with `kingdom_id = 999`. It works — by default, SQLite doesn't enforce foreign keys (a quirk). Run `PRAGMA foreign_keys = ON;` first to enforce them.

## What you just did

You went from one table to two and from one query to four. Buildings now belong to kingdoms via a foreign key, and you can ask the database real questions about both at once: every building with its kingdom (`INNER JOIN`), every kingdom *even if it has no buildings* (`LEFT JOIN`), and the count of buildings per kingdom (`GROUP BY` + `COUNT`). Three new tests prove the queries return what you expect — 57 passing total. You also met `last_insert_rowid()`, the standard SQLite trick for getting the id of the row you just inserted.

**Key concepts you can now name:**

- **foreign key** — a column pointing at another table's id
- **`INNER JOIN`** — only rows that match on both sides
- **`LEFT JOIN`** — every row from the left, even unmatched
- **`GROUP BY` + aggregate** — `COUNT`/`SUM`/`AVG`, one row per group
- **table alias** — `k` for `kingdoms`; readability win

## Wrap up

1. **Quiz** — open `quiz.md`, jot your answers in `journal/quiz-notes.md`.
2. **Progress** — one line in `journal/progress.md`: `Module 2.5 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit and push** — stage both files, commit message `Module 2.5 done`, Sync.
4. **Post in `#wins`** — one line about today, plus the URL of the commit.

Module 0.1 covers the why and the panel/CLI steps if you need a refresher. Bring quiz answers you're least sure about to the next weekly sync.

## Next

Module 2.6 introduces **EF Core** — the .NET ORM (object-relational mapper) that maps `class Kingdom { }` to a row in a table, and lets you write `dbContext.Kingdoms.Add(kingdom)` instead of raw SQL. Same database underneath; less ceremony on top.

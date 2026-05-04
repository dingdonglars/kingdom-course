# Module 2.8 — DB Tooling

You can write the perfect schema, the cleanest migrations, the tightest queries — and still lose hours when something feels wrong because you can't see what's in the database. Today is short and tool-focused: three ways to *look at* your SQLite file, plus the EF script trick that makes *"what would this migration do"* answerable in ten seconds.

> **Words to watch**
>
> - **DB Browser** — a free GUI for SQLite — open the `.db` file, browse tables, run queries
> - **`sqlite3`** — the official command-line tool — same power, no GUI
> - **VS Code SQLTools** — an extension that lets you query the DB inside your editor
> - **migration script** — generated SQL showing exactly what a migration will do

---

## Why this module exists

A database is opaque until you have a window into it. The right tool turns *"the persistence is broken somehow"* into *"row 47 has `gold = -1`, here's why."* The fix is then trivial. Without the tool you spend an hour adding `Console.WriteLine` statements.

Every developer carries one or two go-to DB tools. Today we tour three; pick whichever feels least friction.

---

## Tool 1 — DB Browser for SQLite

The friendliest. Free. Cross-platform.

- Download: <https://sqlitebrowser.org/>
- Open it, File → Open Database → pick `bin/Debug/net10.0/saves/kingdoms-ef.db`
- *Browse Data* tab — see every row in every table
- *Execute SQL* tab — write SQL, hit run, see the result grid

When to use: 90% of the time. Click around. The data is right there.

---

## Tool 2 — `sqlite3` CLI

Already on macOS. On Windows: download `sqlite-tools` from <https://sqlite.org/download.html> and put `sqlite3.exe` on your PATH.

```powershell
sqlite3 saves/kingdoms-ef.db
```

You're now in an interactive shell. Useful commands:

```
.tables                       # list tables
.schema kingdoms              # show CREATE TABLE statement
SELECT * FROM kingdoms;       # query
.headers on                   # column headers in output
.mode column                  # tabular output
.quit                         # exit
```

When to use: scripting, CI scripts, any context where opening a GUI is overkill. Way faster for one-shot queries once you're used to it.

---

## Tool 3 — VS Code SQLTools extension

If you live in VS Code:

1. Install extensions: `SQLTools` and `SQLTools SQLite`.
2. Open Command Palette → *"SQLTools: Add new connection"* → SQLite → point at the `.db` file.
3. The Connections sidebar now shows your tables. Right-click → *"Open Table"*.
4. Open a `.sql` file, write a query, hit Ctrl+E twice to execute.

When to use: if your workflow is *"everything in VS Code, never alt-tab,"* this beats the others by integrating tightly.

---

## EF tool — `migrations script`

The single most useful EF tool nobody mentions:

```powershell
dotnet ef migrations script --project Kingdom.Persistence --startup-project Kingdom.Console
```

Outputs the *exact* SQL all migrations would run on a fresh database. No execution — just the SQL.

Or, the SQL between two specific migrations:

```powershell
dotnet ef migrations script PreviousName CurrentName --project Kingdom.Persistence --startup-project Kingdom.Console
```

When to use:

- You want to know what a migration *will* do before running it. Especially in production.
- You want to share the SQL with a teammate, a DBA, or a human reviewer.
- You're doing the schema review for a code change.

---

## Delta starter

This module is purely text-and-tools. The starter contains:

- `tools/sqlite-tour.md` — a one-page cheatsheet of the commands above
- `tools/sample-queries.sql` — five ready-to-copy queries for the kingdoms DB

No engine or persistence changes; no new tests.

`tools/sample-queries.sql`:

```sql
-- Five sample queries against your kingdoms DB.
-- Copy any of these into DB Browser, sqlite3, or SQLTools.

-- 1. Every kingdom + how many buildings it has
SELECT k.name, COUNT(b.id) AS building_count
FROM Kingdoms k
LEFT JOIN Buildings b ON b.KingdomId = k.Id
GROUP BY k.Id
ORDER BY building_count DESC;

-- 2. The richest kingdom (by gold)
SELECT name, gold FROM Kingdoms ORDER BY gold DESC LIMIT 1;

-- 3. Buildings of each kind (Farm/Lumberyard/Mine), with total levels
SELECT kind, COUNT(*) AS n, SUM(level) AS total_levels
FROM Buildings GROUP BY kind;

-- 4. Kingdoms that have at least one Mine
SELECT DISTINCT k.name
FROM Kingdoms k
JOIN Buildings b ON b.KingdomId = k.Id
WHERE b.kind = 'Mine';

-- 5. The migration history (what EF has applied)
SELECT * FROM __EFMigrationsHistory;
```

## Tinker

Run query #1 against a database with three saves — see how `LEFT JOIN` plus `GROUP BY` answers a real question in one line.

Edit the data directly in DB Browser (set a kingdom's gold to 9999), save, run the program again — the change persists.

In `sqlite3`: `.import data.csv kingdoms` to bulk-import. Useful when you have 1000 test rows you'd rather not generate by hand.

Run `dotnet ef migrations script -i` (idempotent) — the SQL guards each migration with an `IF NOT EXISTS`-style check, so it's safe to apply multiple times.

## What you just did

You picked up three windows into your database: DB Browser (the friendly GUI), the `sqlite3` CLI (the no-frills shell), and the VS Code SQLTools extension (in-editor). Plus one EF trick — `migrations script` — that prints the SQL a migration *would* run without actually running it. None of these change your code. They change how fast you can answer *"what does the database actually contain right now?"* — which, six months from now, will be the difference between a five-minute fix and a five-hour chase.

**Key concepts you can now name:**

- **DB Browser** — friendly free GUI for SQLite
- **`sqlite3` CLI** — official command-line tool
- **SQLTools (VS Code)** — DB queries inside the editor
- **`dotnet ef migrations script`** — preview SQL without applying
- **`__EFMigrationsHistory`** — visible in any of the tools above

## Quiz

Open `quiz.md`. When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.

## Next

Module 2.9 — **save slots** — uses everything we have. Multiple kingdoms in one DB, list them, load any of them. Real save-slot UX.

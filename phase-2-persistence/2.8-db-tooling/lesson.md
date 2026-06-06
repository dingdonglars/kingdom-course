# Module 2.8 — DB Tooling

You can write the perfect structure, the cleanest migrations, and the best queries — and still lose hours when something looks wrong, because you can't see what's actually in the database. Today is short and about tools: three ways to *look inside* your SQLite file, plus an EF command that answers *"what would this migration do?"* in ten seconds.

> **Words to watch**
>
> - **DB Browser** — a free GUI for SQLite — open the `.db` file, browse tables, run queries
> - **`sqlite3`** — the official command-line tool — same power, no GUI
> - **VS Code SQLTools** — an extension that lets you query the DB inside your editor
> - **migration script** — generated SQL showing exactly what a migration will do

---

## Why this module exists

You can't see inside a database until you have a way to look. The right tool turns *"saving is broken somehow"* into *"row 47 has `gold = -1`, and here's why."* The fix after that is easy. Without the tool, you spend an hour adding `Console.WriteLine` lines instead.

Every developer has one or two favourite database tools. Today we look at three. Pick whichever one feels easiest for you.

---

## Tool 1 — DB Browser for SQLite

The easiest to use. Free. Runs on any operating system.

- Download: <https://sqlitebrowser.org/>
- Open it, File → Open Database → pick `bin/Debug/net10.0/saves/kingdoms-ef.db`
- *Browse Data* tab — see every row in every table
- *Execute SQL* tab — write SQL, hit run, see the result grid

When to use it: most of the time. Click around. The data is right in front of you.

---

## Tool 2 — `sqlite3` CLI

Already there on macOS. On Windows: download `sqlite-tools` from <https://sqlite.org/download.html> and put `sqlite3.exe` on your PATH.

```powershell
sqlite3 saves/kingdoms-ef.db
```

You're now in an interactive prompt. Useful commands:

```
.tables                       # list tables
.schema kingdoms              # show CREATE TABLE statement
SELECT * FROM kingdoms;       # query
.headers on                   # column headers in output
.mode column                  # tabular output
.quit                         # exit
```

When to use it: in scripts, in CI, and any time opening a full app is more than you need. Once you're used to it, it's much faster for a quick one-off query.

---

## Tool 3 — VS Code SQLTools extension

If you live in VS Code:

1. Install extensions: `SQLTools` and `SQLTools SQLite`.
2. Open Command Palette → *"SQLTools: Add new connection"* → SQLite → point at the `.db` file.
3. The Connections sidebar now shows your tables. Right-click → *"Open Table"*.
4. Open a `.sql` file, write a query, and press Ctrl+E twice to run it.

When to use it: if you like to keep everything in VS Code and never switch windows, this fits best because it lives right in the editor.

---

## EF tool — `migrations script`

A very useful EF command that few people talk about:

```powershell
dotnet ef migrations script --project Kingdom.Persistence --startup-project Kingdom.Console
```

It prints the *exact* SQL that all the migrations would run on a brand new database. It doesn't run anything — it just shows you the SQL.

Or the SQL between two specific migrations:

```powershell
dotnet ef migrations script PreviousName CurrentName --project Kingdom.Persistence --startup-project Kingdom.Console
```

When to use it:

- You want to know what a migration *will* do before you run it — especially on a live app.
- You want to share the SQL with a teammate, a database admin, or a reviewer.
- You're reviewing a structure change as part of a code change.

---

## Delta starter

This module is only reading and tools. The starter contains:

- `tools/sqlite-tour.md` — a one-page list of the commands above
- `tools/sample-queries.sql` — five ready-to-copy queries for the kingdoms DB

No engine or persistence changes, and no new tests.

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

Run query #1 against a database with three saves. See how `LEFT JOIN` plus `GROUP BY` answers a real question in one line.

Edit the data right inside DB Browser (set a kingdom's gold to 9999), save, and run the program again. The change is still there.

In `sqlite3`: use `.import data.csv kingdoms` to load many rows at once. Handy when you have 1000 test rows and don't want to type them by hand.

Run `dotnet ef migrations script -i`. The `-i` makes it idempotent — that means the SQL checks each migration with an `IF NOT EXISTS`-style guard first, so running it more than once does no harm.

## What you just did

You picked up three ways to look inside your database: DB Browser (the easy-to-use app), the `sqlite3` command line (plain and quick), and the VS Code SQLTools extension (right in your editor). Plus one EF command — `migrations script` — that prints the SQL a migration *would* run without running it. None of these change your code. What they change is how fast you can answer *"what's actually in the database right now?"* Six months from now, that's the difference between a five-minute fix and a five-hour search.

**Key concepts you can now name:**

- **DB Browser** — free, easy-to-use app for SQLite
- **`sqlite3` CLI** — official command-line tool
- **SQLTools (VS Code)** — database queries inside the editor
- **`dotnet ef migrations script`** — see the SQL without applying it
- **`__EFMigrationsHistory`** — visible in any of the tools above

## On your own

Time to put the book away. Don't scroll back up to the steps — show yourself, from your own head, that the one big idea stuck: you can open the `.db` file in a tool and look at what's really inside it. No one marks this — it's just for you. It's the fastest way to spot what hasn't stuck yet, while it's still small to fix. Getting stuck here is completely fine — that's exactly what it's for.

Pick whichever tool you liked best — DB Browser, `sqlite3`, or SQLTools. Then:

1. Open your `kingdoms-ef.db` file.
2. Find the list of tables.
3. Look at the rows in one table.
4. Run one `SELECT` of your own.

No notes — just open it and look.

<details><summary>Stuck? Open this to check yourself.</summary>

What you should have managed:

- Opened `bin/Debug/net10.0/saves/kingdoms-ef.db` in your chosen tool.
- Seen the tables EF made: `Kingdoms`, `Buildings`, and `__EFMigrationsHistory`.
- Browsed the rows of one table (or ran `SELECT * FROM Kingdoms;`).
- Run one query you wrote yourself and got a result grid back.

If you used `sqlite3`, the quick path is `.tables` to list them, then `SELECT * FROM Kingdoms;`. If the tool shows your rows, you can now see inside any database you build — that's the whole skill for today.

</details>

## Wrap up

1. **Quiz** — open `quiz.md`, jot your answers in `journal/quiz-notes.md`.
2. **Progress** — one line in `journal/progress.md`: `Module 2.8 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit and push** — stage both files, commit message `Module 2.8 done`, Sync.
4. **Post in `#wins`** — one line about today, plus the URL of the commit.

Module 0.1 covers the why and the panel/CLI steps if you need a refresher. Bring quiz answers you're least sure about to the next weekly sync.

## Next

Module 2.9 — **save slots** — uses everything we have. Many kingdoms in one database: list them, and load any one of them. A real save-slot experience, like a game's load screen.

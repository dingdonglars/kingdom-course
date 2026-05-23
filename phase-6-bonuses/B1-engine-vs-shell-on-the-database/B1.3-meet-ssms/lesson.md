# Bonus B1.3 — Meet SSMS

Yesterday you moved your kingdom onto SQL Server with three lines of config. Now your data lives in a real Microsoft database, and there is a professional tool for working with it: **SQL Server Management Studio**, or SSMS for short. It is the GUI that most working database admins keep open all day. It is free, it has every feature, and ten minutes of clicking around is enough to feel comfortable. After that you can browse any SQL Server you ever meet, for the rest of your career.

The point of today is not to learn every part of SSMS — it has parts no one ever uses. The point is to learn the five basic moves so the tool stops feeling strange.

> **Words to watch**
>
> - **SSMS** — SQL Server Management Studio; the standard GUI for SQL Server
> - **Object Explorer** — left-hand tree showing every server, database, table, view
> - **query window** — the editor where you type SQL and press F5 to run it
> - **execution plan** — a diagram showing how the database will run a query
> - **Activity Monitor** — live view of what queries are running right now

---

## Step 1 — install SSMS

Download from [the Microsoft SSMS page](https://learn.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms). It's free and about 700 MB.

SSMS is Windows-only. On macOS or Linux, install **Azure Data Studio** instead — Microsoft's version that runs on any system. The UI is a little different, but the job is the same: connect to a SQL Server, browse it, run queries, and look at execution plans.

After install, launch SSMS. The Connect dialog appears.

## Step 2 — connect to LocalDB

Fill in the dialog:

- **Server type:** Database Engine
- **Server name:** `(localdb)\MSSQLLocalDB`
- **Authentication:** Windows Authentication

Click **Connect**. The Object Explorer panel on the left shows your LocalDB instance. Expand:

- **Databases**
- → your `Kingdom_*` database (one per save slot from yesterday's tests)
- → **Tables**
- → `Kingdoms`

Right-click the `Kingdoms` table and pick **Select Top 1000 Rows**. A query window opens with auto-generated SQL at the top and the rows shown below. Those are real rows your engine wrote yesterday, shown by Microsoft's own database tool. Same data, seen through a different tool.

## Step 3 — five moves to know

Once you are connected, these are the things you use every day:

1. **`Ctrl+N`** — open a new query window. Paste any SQL. Press `F5` to run.
2. **Right-click table → Design** — see the schema (columns, types, constraints) in a form view.
3. **Right-click database → Properties → Files** — see where the `.mdf` data file lives on disk.
4. **Activity Monitor** (toolbar, looks like a graph icon) — a live view of running queries, blocks, and waits. Read this when something feels slow.
5. **Query → Display Estimated Execution Plan** (`Ctrl+L`) — shows how the database plans to run your query. Index seeks, scans, joins, all of it. Read this when a query *is* slow.

The execution plan is the best feature. When a query takes longer than you expect, the plan tells you why — usually because an index is missing or a join is matching far more rows than you thought. Even at the level you are at now, looking at one execution plan tells you more about how a database thinks than any blog post will.

## Step 4 — try a few things

In a new query window, run:

```sql
SELECT * FROM Kingdoms;
```

Then:

```sql
SELECT name FROM sys.databases;
```

That second one queries the system catalogue — SQL Server's own list of databases. Useful for answering "what is actually here?"

To export a database: right-click your `Kingdom_*` database → **Tasks** → **Generate Scripts**. Pick *"Schema and data"* in the wizard. The result is one big `.sql` file with the table definitions plus the `INSERT` statements to recreate every row. Useful for backups, for sharing a copy with someone, or for checking what changed in a migration.

## Tinker

Open one of EF's generated migration files. Or run `dotnet ef migrations script -o init.sql` to write the whole migration history as one SQL file. Open it in SSMS. Read it. EF's output is just SQL — once you can read what it produces, the framework stops feeling like magic.

Right-click an index → **Properties** → see the usage stats. SSMS keeps track of how often each index is used. Indexes that nothing reads are wasted space; indexes that everything reads are doing their job.

Open the Activity Monitor with nothing running. Then run a slow query on purpose — `SELECT COUNT(*) FROM sys.objects, sys.objects, sys.objects` — and watch it appear. Cancel it before your laptop overheats.

## What you just did

You installed SSMS, connected to your LocalDB instance, and used the five basic moves: query window, Design view, file properties, Activity Monitor, and execution plan. The same tool works against any SQL Server anywhere — your laptop, a teammate's machine, an Azure SQL Database in the cloud — and the moves don't change. Five minutes to install, ten minutes of clicking, and you can now inspect any SQL Server you'll meet for years.

**Key concepts you can now name:**

- **SSMS** — the standard SQL Server GUI
- **Object Explorer** — server, database, and table tree
- **query window (`Ctrl+N`)** — write and run SQL
- **execution plan** — visual map of how a query will run
- **Activity Monitor** — live view of running queries

## Wrap up

1. **Quiz** — open `quiz.md`, jot your answers in `journal/quiz-notes.md`.
2. **Progress** — one line in `journal/progress.md`: `Module B1.3 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit and push** — stage both files, commit message `Module B1.3 done`, Sync.
4. **Post in `#wins`** — one line about today, plus the URL of the commit.

Module 0.1 covers the why and the panel/CLI steps if you need a refresher. Bring quiz answers you're least sure about to the next weekly sync.

## Next

B1.4 closes the bonus with a short reflection: one paragraph in `journal/B1-what-i-learned.md` saying what the change proved.

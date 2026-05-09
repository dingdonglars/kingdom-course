# Bonus B1.3 — Meet SSMS

Yesterday you swapped your kingdom onto SQL Server with three lines of config. Now your data lives in a real Microsoft database, and there's a professional tool for talking to it: **SQL Server Management Studio**, or SSMS for short. It's the GUI every working database admin on the planet has open in some tab. Free, full-featured, ten minutes of clicking around to feel comfortable. After that you can browse any SQL Server you ever encounter for the rest of your career.

The point of today isn't to learn every corner of SSMS — there are corners no human has ever visited. The point is to get the five basic moves under your fingers so the tool stops feeling foreign.

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

SSMS is Windows-only. On macOS or Linux, install **Azure Data Studio** instead — Microsoft's cross-platform alternative. The UI is a little different but the job is the same: connect to a SQL Server, browse it, run queries, look at execution plans.

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

Right-click the `Kingdoms` table and pick **Select Top 1000 Rows**. A query window opens with auto-generated SQL at the top and the rows displayed below. *Those are real rows your engine wrote yesterday, displayed by Microsoft's own database tool.* Same data, different lens.

## Step 3 — five moves to know

Once you're connected, these are the things that come up every day:

1. **`Ctrl+N`** — open a new query window. Paste any SQL. Press `F5` to run.
2. **Right-click table → Design** — see the schema (columns, types, constraints) in a form view.
3. **Right-click database → Properties → Files** — see where the `.mdf` data file lives on disk.
4. **Activity Monitor** (toolbar, looks like a graph icon) — live view of running queries, blocks, waits. Read this when something feels slow.
5. **Query → Display Estimated Execution Plan** (`Ctrl+L`) — visualises how the database plans to run your query. Index seeks, scans, joins, the lot. Read this when a query *is* slow.

The execution plan is the killer feature. When a query takes longer than you expect, the plan tells you why — usually because some index is missing or some join is fanning out. Even at the level you're at now, glancing at one execution plan tells you more about how a database thinks than any blog post will.

## Step 4 — try a few things

In a new query window, run:

```sql
SELECT * FROM Kingdoms;
```

Then:

```sql
SELECT name FROM sys.databases;
```

That second one queries the system catalogue — SQL Server's own list of databases. Useful for "what's actually here?"

For the export trick: right-click your `Kingdom_*` database → **Tasks** → **Generate Scripts**. Pick *"Schema and data"* in the wizard. The result is one big `.sql` file containing the table definitions plus the `INSERT` statements to recreate every row. Useful for backups, sharing a snapshot with someone, or sanity-checking what changed in a migration.

## Tinker

Open one of EF's generated migration files. Or run `dotnet ef migrations script -o init.sql` to dump the whole migration history as one SQL file. Open it in SSMS. Read it. EF's output is just SQL — once you can read what it produces, the framework stops feeling like magic.

Right-click an index → **Properties** → see the usage stats. SSMS keeps track of how often each index is hit. Indexes that nothing reads are dead weight; indexes that everything reads are earning their keep.

Open the Activity Monitor with nothing running. Then run a deliberately slow query — `SELECT COUNT(*) FROM sys.objects, sys.objects, sys.objects` — and watch it appear. Cancel it before your laptop melts.

## What you just did

You installed SSMS, connected to your LocalDB instance, and used the five basic moves: query window, Design view, file properties, Activity Monitor, execution plan. The same tool runs against any SQL Server anywhere — your laptop, a teammate's machine, an Azure SQL Database in the cloud — and the moves don't change. Five minutes to install, ten minutes of clicking, and you're now able to inspect any SQL Server you'll meet for years.

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

B1.4 closes the bonus with a short reflection: one paragraph in `journal/B1-what-i-learned.md` naming what the swap proved.

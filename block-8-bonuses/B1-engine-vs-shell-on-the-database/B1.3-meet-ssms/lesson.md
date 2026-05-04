# Bonus B1.3 — Meet SSMS

> **Hook:** SQL Server Management Studio (SSMS) is the professional tool for working with SQL Server. **Free, full-featured, used by every working DBA on the planet.** Five minutes to install, ten minutes to feel comfortable. After that, you can browse any SQL Server you ever encounter.

> **Words to watch**
> - **SSMS** — SQL Server Management Studio; the standard GUI
> - **Object Explorer** — left tree of every server / DB / table / view
> - **query window** — Ctrl+N — paste SQL, F5 to run
> - **Activity Monitor** — see what queries are running right now

---

## Install

Download from <https://learn.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms>. Free. ~700MB. **Windows only** (on macOS/Linux, use Azure Data Studio — the cross-platform alternative — same job, slightly different UI).

After install, launch SSMS. The Connect dialog appears.

## Connect to LocalDB

- Server type: Database Engine
- Server name: `(localdb)\MSSQLLocalDB`
- Authentication: Windows Authentication
- Click Connect

You're in. The Object Explorer (left) shows the LocalDB instance. Expand → Databases → your `Kingdom_*` database → Tables → `Kingdoms`. Right-click → **Select Top 1000 Rows.** A query window opens with the generated SQL + the rows displayed below.

## Five things to know

1. **Ctrl+N** — new query window. Paste any SQL. F5 to run.
2. **Right-click table → Design** — see the schema (columns, types, constraints).
3. **Right-click DB → Properties → Files** — see where the .mdf data file lives on disk.
4. **Activity Monitor (toolbar)** — live view of queries, blocks, waits. Useful for "why is this slow."
5. **Query → Display Estimated Execution Plan (Ctrl+L)** — visualises how the DB plans to execute a query. Index scans, seeks, joins. **Read this when a query is slow.**

## Tinker

- Run `SELECT * FROM Kingdoms` from a query window.
- Right-click your DB → **Generate Scripts** → choose "Schema and data" → exports the entire DB to one `.sql` file. **Useful for backups and copying between machines.**
- Open one of EF's generated migration `.sql` files (`dotnet ef migrations script -o init.sql`). Paste into SSMS. Read it. **EF's output is just SQL.**
- Right-click an index → Properties → see usage stats. Indexes you've created vs ones the DB suggested.

## Name it

- **SSMS** — SQL Server Management Studio.
- **Object Explorer** — server/db/table tree.
- **Query window (Ctrl+N)** — write + run SQL.
- **Activity Monitor** — live view of running queries.
- **Estimated Execution Plan** — how the DB plans to execute a query.

## The rule of the through-line

> **Tools beat hammers.** SSMS is the professional way to talk to SQL Server. Doing it without SSMS is possible — `sqlcmd`, EF logs, raw SQL via your code — but slower. Knowing the tool is the difference between "I think the DB looks fine" and "I can see it does."

## Quiz / challenge

Open `quiz.md`.

## Connect

B1.4 closes the bonus: a one-paragraph reflection on what the swap proved. **The bonus's whole point lands in writing.**
# Bonus B1.1 — Install LocalDB

> **Hook:** today's job: install **SQL Server LocalDB** — a free, lightweight version of Microsoft SQL Server that runs on your laptop. Tomorrow we'll point your existing EF Core code at it (3-line config change). The lesson lands by *anticlimax*.

> **Words to watch**
> - **SQL Server** — Microsoft's flagship relational DB; production-grade
> - **LocalDB** — a developer edition; instance per user; no service to manage
> - **connection string** — the URL-like string EF uses to find the DB
> - **SSMS** — SQL Server Management Studio; the GUI tool

---

## Why bother

You shipped the kingdom on SQLite. It works. Why install another DB?

Because **the engine-vs-shell rule predicts something**: swapping the database should be ~3 lines of config, not a rewrite. Today is the experiment that proves the prediction.

LocalDB is the right testbed: it's *real* SQL Server (full T-SQL, full feature set), but installs in 5 minutes and runs on your machine. No license needed.

## Install

**Windows:**

1. Download SQL Server LocalDB: <https://learn.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb>
2. Run the installer; pick "LocalDB" only (skip the full SQL Server).
3. After install, open PowerShell:
   ```powershell
   sqllocaldb info
   ```
4. You should see `MSSQLLocalDB` (the default instance).
5. Start it: `sqllocaldb start MSSQLLocalDB`.

**macOS / Linux:** LocalDB is Windows-only. Use SQL Server in a Docker container instead:
```bash
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=Strong!Pass1" -p 1433:1433 -d mcr.microsoft.com/mssql/server:2022-latest
```
Then your connection string will use `localhost,1433` and the SA password.

## The connection string

For LocalDB:
```
Server=(localdb)\\MSSQLLocalDB;Database=Kingdom;Trusted_Connection=True;
```

For Docker SQL Server:
```
Server=localhost,1433;Database=Kingdom;User Id=sa;Password=Strong!Pass1;TrustServerCertificate=true;
```

**Notice what's the same:** `Server=...;Database=...`. SQL Server's connection string is shaped like SQLite's (`Data Source=...`) — different keys, same role.

## Tinker

- After install, run `sqlcmd -S "(localdb)\MSSQLLocalDB" -Q "SELECT @@VERSION"`. Confirms the install.
- Open Notepad as admin → connect to LocalDB via SSMS (next module). The DB is there but empty.

## Name it

- **LocalDB** — single-user dev edition of SQL Server.
- **`(localdb)\MSSQLLocalDB`** — the standard instance name.
- **`sqllocaldb info` / `start`** — CLI for instance management.
- **Trusted_Connection / Integrated Security** — Windows auth (no password); SQL auth uses User Id + Password.

## The rule of the through-line

> **The engine doesn't care what database it's on.** Today: install. Tomorrow: prove the swap is trivial.

## Quiz / challenge

Open `quiz.md`.

## Connect

B1.2 — three lines of config and your EF Core engine code now writes to SQL Server instead of SQLite. **Your tests pass unchanged.**
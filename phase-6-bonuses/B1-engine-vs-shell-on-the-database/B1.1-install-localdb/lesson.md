# Bonus B1.1 — Install LocalDB

Back in Phase 2 you put the kingdom on SQLite. It works. So why install another database? Because the engine-vs-shell rule made a promise. The engine doesn't care which database it talks to. Changing the database should be a config change, not a rewrite. B1 is the test that checks the promise. Today's job is the small first step — install **SQL Server LocalDB** on your machine. Tomorrow we point your existing EF Core code at it and run the tests.

LocalDB is the right tool for this. It is *real* SQL Server — full T-SQL, the same engine that big companies run in production — but the version made for developers. It installs in five minutes. It runs on your laptop. There is no background service to manage, and it costs nothing. Real, but small.

> **Words to watch**
>
> - **SQL Server** — Microsoft's flagship relational database, used heavily in industry
> - **LocalDB** — the single-user developer edition; one instance per user, no daemon
> - **connection string** — the line of text EF uses to find the database
> - **SSMS** — SQL Server Management Studio; the GUI tool we meet in B1.3

---

## Step 1 — install LocalDB

**Windows (the main path):**

1. Go to [the Microsoft LocalDB page](https://learn.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb).
2. Run the installer. When it asks what to install, pick **LocalDB only** — skip the full SQL Server.
3. After install, open PowerShell:

   ```powershell
   sqllocaldb info
   ```

4. You should see `MSSQLLocalDB` in the output — that's the default instance.
5. Start it:

   ```powershell
   sqllocaldb start MSSQLLocalDB
   ```

**macOS or Linux:** LocalDB is Windows-only. Use the SQL Server image in Docker instead:

```bash
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=Strong!Pass1" \
  -p 1433:1433 -d mcr.microsoft.com/mssql/server:2022-latest
```

That is the same SQL Server engine, just packaged for Docker. The connection string in B1.2 will use `localhost,1433` and the SA password instead of the LocalDB form.

## Step 2 — confirm it's there

Run a quick query against your new instance:

```powershell
sqlcmd -S "(localdb)\MSSQLLocalDB" -Q "SELECT @@VERSION"
```

You should see a long version string come back. That is SQL Server answering you. If it does, the install worked and you are done.

## Step 3 — look at the connection string format

In B1.2 you'll paste a connection string into your EF Core setup. There are two forms, depending on which path you took:

**LocalDB:**

```
Server=(localdb)\\MSSQLLocalDB;Database=Kingdom;Trusted_Connection=True;
```

**Docker SQL Server:**

```
Server=localhost,1433;Database=Kingdom;User Id=sa;Password=Strong!Pass1;TrustServerCertificate=true;
```

Notice what is the same and what is different. The keys are different — `Server` instead of `Data Source`, and `Trusted_Connection` is new — but the layout is familiar. You tell the driver where the database is, what to call it, and who you are. SQLite's connection string says the same things. It is just shorter, because SQLite is one file on disk.

## Tinker

Try `sqlcmd -S "(localdb)\MSSQLLocalDB" -Q "SELECT name FROM sys.databases"` — you'll see the system databases (`master`, `tempdb`, `model`, `msdb`) but no `Kingdom` yet. That comes tomorrow.

If you already have SSMS (we install it properly in B1.3), connect to `(localdb)\MSSQLLocalDB` with Windows Authentication. The instance is empty, but you can reach it. Seeing that it is there makes B1.2 feel more real.

## What you just did

You installed SQL Server LocalDB — the developer version of Microsoft's main database. You confirmed it is running with `sqllocaldb info`, and ran your first query against it through `sqlcmd`. The setup took maybe ten minutes, and the install itself was almost boring. That is the point. B1's whole story is that changing the database happens in tiny steps, and this is the first one. Tomorrow's three-line config change will put your existing EF Core code on this new instance.

**Key concepts you can now name:**

- **LocalDB** — single-user developer edition of SQL Server
- **`(localdb)\MSSQLLocalDB`** — the default instance name on your machine
- **`sqllocaldb info` / `start`** — command-line control of LocalDB instances
- **connection string** — the text EF uses to find a database
- **Trusted_Connection / Integrated Security** — Windows auth (no password needed)

## Wrap up

1. **Quiz** — open `quiz.md`, jot your answers in `journal/quiz-notes.md`.
2. **Progress** — one line in `journal/progress.md`: `Module B1.1 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit and push** — stage both files, commit message `Module B1.1 done`, Sync.
4. **Post in `#wins`** — one line about today, plus the URL of the commit.

Module 0.1 covers the why and the panel/CLI steps if you need a refresher. Bring quiz answers you're least sure about to the next weekly sync.

## Next

B1.2 is the test itself. Three lines of config, and your EF Core engine code writes to SQL Server instead of SQLite. Your tests pass without changing.

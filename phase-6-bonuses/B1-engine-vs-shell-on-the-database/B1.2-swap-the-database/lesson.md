# Bonus B1.2 — Swap SQLite for SQL Server (Three Lines)

This is the lesson the whole bonus exists for. You're going to change three lines of config, regenerate migrations, run the test suite — and watch every test pass without touching a single line of engine code. The whole point is how unremarkable this feels. The engine-vs-shell rule predicted the swap would be small; today is the day you cash that prediction in.

If this lands and feels boring, the discipline you built up across Phases 1 and 2 just paid you back. *Boring on purpose* — the engine treated the database as a swappable detail, and now we get to prove it.

> **Words to watch**
>
> - **provider** — EF Core's adapter for one specific database (`UseSqlite(...)` vs `UseSqlServer(...)`)
> - **dialect** — small SQL differences between databases (Identity vs AUTOINCREMENT, etc.). EF hides these.
> - **migration regen** — when you change providers, you regenerate migration files because the SQL output differs

---

## Step 1 — change the package

Open `Kingdom.Persistence.csproj` and swap the EF Core provider package:

```xml
<!-- Was -->
<PackageReference Include="Microsoft.EntityFrameworkCore.Sqlite" Version="9.0.0" />

<!-- Now -->
<PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="9.0.0" />
```

That's line one of three.

## Step 2 — change the provider call

Open `KingdomDbContext.cs`. Find the line where you tell EF which database to use:

```csharp
// Was
options.UseSqlite($"Data Source={_path};Pooling=False");

// Now
options.UseSqlServer(
    $"Server=(localdb)\\MSSQLLocalDB;Database=Kingdom_{_path};" +
    "Trusted_Connection=True;TrustServerCertificate=true;");
```

The `_path` value used to be the SQLite filename. SQL Server doesn't have files in the same sense — it has named databases on a server. So we use `_path` as part of the database name instead, which keeps each save slot isolated the way they were before. (Pick whatever scheme makes sense to you.)

That's lines two and three. Three lines total — though if you count the connection-string format change separately, call it four. Either way, it's a config change, not a rewrite.

## Step 3 — regenerate the migration files

Migration files contain SQL that's specific to one provider. SQLite says `AUTOINCREMENT`; SQL Server says `IDENTITY(1,1)`. Same C# code on the EF side, different SQL on the database side. So when you change providers, the migration files have to be regenerated.

```powershell
# Delete the old SQLite migrations
Remove-Item -Recurse Kingdom.Persistence/Migrations

# Generate fresh ones for SQL Server
dotnet ef migrations add InitialCreate `
    --project Kingdom.Persistence `
    --startup-project Kingdom.Console
```

> A real production system never deletes its migrations on a live database — that would lose the history of how the schema evolved. For a learning swap on a database that contains nothing precious, fresh migrations are fine.

## Step 4 — run the tests

```powershell
dotnet build
dotnet test
```

Watch the count. **Every test passes.** Same engine code, same store API, different database underneath. The engine never noticed the swap.

This is the moment the bonus exists for. Sit with it for a second. The test suite you wrote against SQLite — the one that asserted kingdoms persist, ledgers round-trip, slots load by id — runs unchanged against a different database vendor. That's what the engine-vs-shell rule was promising the whole time.

## Step 5 — confirm with sqlcmd

```powershell
sqlcmd -S "(localdb)\MSSQLLocalDB" -Q "SELECT name FROM sys.databases"
```

You should see `Kingdom_*` entries in the list — one per save slot the tests created. Pick one and look inside:

```powershell
sqlcmd -S "(localdb)\MSSQLLocalDB" -d Kingdom_test01 `
    -Q "SELECT TOP 5 * FROM Kingdoms"
```

Real rows, real SQL Server, written by your unchanged engine code. *That* is the result of three lines of config.

## Tinker

Switch back to SQLite. The same three-line change in reverse. The swap is symmetric — and that symmetry is the proof the engine treats both databases as equals.

Try a third provider. Add the `Npgsql.EntityFrameworkCore.PostgreSQL` package, swap `UseSqlServer(...)` for `UseNpgsql(...)`, point it at a Postgres instance (Docker is fine), regenerate migrations, run the tests. Same result. Same boring result.

Turn on EF logging to see the SQL that's actually being sent:

```csharp
options.LogTo(Console.WriteLine, LogLevel.Information);
```

The C# above your store methods hasn't moved. The SQL coming out the bottom is wildly different between providers. That's the abstraction layer doing its job.

## What you just did

You proved the engine-vs-shell discipline isn't a slogan — it's a real, load-bearing pattern. Three lines of config (plus a migration regen) swapped the kingdom from SQLite to SQL Server, and your tests passed unchanged. The engine never knew. That's the whole bonus, in one Step 4. Everything else here — the install yesterday, SSMS tomorrow, the reflection — is wrapping around this single result.

**Key concepts you can now name:**

- **provider** — EF Core's database-specific adapter
- **`UseSqlite` / `UseSqlServer` / `UseNpgsql`** — provider config calls, same layout
- **migration regen** — change provider, regenerate migrations
- **dialect** — small SQL differences EF hides for you
- **the engine-vs-shell discipline pays back** — same engine, swappable storage, tests prove it

## Wrap up

1. **Quiz** — open `quiz.md`, jot your answers in `journal/quiz-notes.md`.
2. **Progress** — one line in `journal/progress.md`: `Module B1.2 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit and push** — stage both files, commit message `Module B1.2 done`, Sync.
4. **Post in `#wins`** — one line about today, plus the URL of the commit.

Module 0.1 covers the why and the panel/CLI steps if you need a refresher. Bring quiz answers you're least sure about to the next weekly sync.

## Next

B1.3 introduces **SSMS** — SQL Server Management Studio — the professional GUI for browsing and querying SQL Server. Five minutes to learn the basics; useful for the rest of your career.

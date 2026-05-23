# Bonus B1.2 — Swap SQLite for SQL Server (Three Lines)

This is the lesson the whole bonus exists for. You are going to change three lines of config, regenerate migrations, and run the tests. Then you watch every test pass without touching a single line of engine code. The whole point is how ordinary this feels. The engine-vs-shell rule said the change would be small. Today is the day you prove it.

If this feels boring, the careful work you put in across Phases 1 and 2 just paid you back. Boring on purpose. The engine treated the database as a part it could swap out, and now we get to prove it.

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

The `_path` value used to be the SQLite filename. SQL Server doesn't work with files in the same way — it has named databases on a server. So we use `_path` as part of the database name instead. That keeps each save slot separate, the way it was before. (Pick whatever naming makes sense to you.)

That is lines two and three. Three lines in total — though if you count the connection-string change on its own, call it four. Either way, it is a config change, not a rewrite.

## Step 3 — regenerate the migration files

Migration files contain SQL that is written for one provider. SQLite says `AUTOINCREMENT`; SQL Server says `IDENTITY(1,1)`. Same C# code on the EF side, different SQL on the database side. So when you change providers, you have to regenerate the migration files.

```powershell
# Delete the old SQLite migrations
Remove-Item -Recurse Kingdom.Persistence/Migrations

# Generate fresh ones for SQL Server
dotnet ef migrations add InitialCreate `
    --project Kingdom.Persistence `
    --startup-project Kingdom.Console
```

> A real production system never deletes its migrations on a live database — that would throw away the record of how the schema changed over time. For a practice change on a database that holds nothing important, fresh migrations are fine.

## Step 4 — run the tests

```powershell
dotnet build
dotnet test
```

Watch the count. **Every test passes.** Same engine code, same store API, different database underneath. The engine never noticed the change.

This is the moment the bonus exists for. Stop and think about it for a second. The tests you wrote against SQLite — the ones that check that kingdoms save, ledgers save and load back correctly, and slots load by id — run unchanged against a database from a different company. That is what the engine-vs-shell rule was promising the whole time.

## Step 5 — confirm with sqlcmd

```powershell
sqlcmd -S "(localdb)\MSSQLLocalDB" -Q "SELECT name FROM sys.databases"
```

You should see `Kingdom_*` entries in the list — one per save slot the tests created. Pick one and look inside:

```powershell
sqlcmd -S "(localdb)\MSSQLLocalDB" -d Kingdom_test01 `
    -Q "SELECT TOP 5 * FROM Kingdoms"
```

Real rows, real SQL Server, written by your engine code that you did not change. That is the result of three lines of config.

## Tinker

Switch back to SQLite. The same three-line change, in reverse. The change works the same way in both directions — and that is the proof that the engine treats both databases the same.

Try a third provider. Add the `Npgsql.EntityFrameworkCore.PostgreSQL` package, swap `UseSqlServer(...)` for `UseNpgsql(...)`, point it at a Postgres instance (Docker is fine), regenerate migrations, run the tests. Same result. Same boring result.

Turn on EF logging to see the SQL that's actually being sent:

```csharp
options.LogTo(Console.WriteLine, LogLevel.Information);
```

The C# above your store methods hasn't changed. But the SQL coming out the bottom is very different between providers. That is the abstraction layer doing its job.

## What you just did

You proved the engine-vs-shell rule isn't just a nice phrase — it's a real pattern that holds up your code. Three lines of config (plus a migration regen) moved the kingdom from SQLite to SQL Server, and your tests passed unchanged. The engine never knew. That is the whole bonus, in one Step 4. Everything else here — the install yesterday, SSMS tomorrow, the reflection — is built around this one result.

**Key concepts you can now name:**

- **provider** — EF Core's database-specific adapter
- **`UseSqlite` / `UseSqlServer` / `UseNpgsql`** — provider config calls, same layout
- **migration regen** — change provider, regenerate migrations
- **dialect** — small SQL differences EF hides for you
- **the engine-vs-shell rule pays back** — same engine, swappable storage, tests prove it

## Wrap up

1. **Quiz** — open `quiz.md`, jot your answers in `journal/quiz-notes.md`.
2. **Progress** — one line in `journal/progress.md`: `Module B1.2 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit and push** — stage both files, commit message `Module B1.2 done`, Sync.
4. **Post in `#wins`** — one line about today, plus the URL of the commit.

Module 0.1 covers the why and the panel/CLI steps if you need a refresher. Bring quiz answers you're least sure about to the next weekly sync.

## Next

B1.3 introduces **SSMS** — SQL Server Management Studio — the professional GUI for browsing and querying SQL Server. Five minutes to learn the basics, and useful for the rest of your career.

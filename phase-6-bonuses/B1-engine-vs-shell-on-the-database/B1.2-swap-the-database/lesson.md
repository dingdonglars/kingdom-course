# Bonus B1.2 — Swap SQLite for SQL Server (3 lines)

> **Hook:** today is the experiment. Three lines of config — package + provider + connection string — and your EF Core code now writes to SQL Server instead of SQLite. Run your tests. **They pass.** That's the lesson.

> **Words to watch**
> - **provider** — EF's adapter for a specific DB. `UseSqlite(...)` vs `UseSqlServer(...)`.
> - **dialect** — small SQL differences between databases (Identity vs AUTOINCREMENT, etc.). EF hides these.
> - **migration regen** — when changing providers, you regenerate migrations because the SQL output differs.

---

## The three lines

In `Kingdom.Persistence.csproj`:

```xml
<!-- Was -->
<PackageReference Include="Microsoft.EntityFrameworkCore.Sqlite" Version="9.0.0" />
<!-- Now -->
<PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="9.0.0" />
```

In `KingdomDbContext.cs`:

```csharp
// Was
options.UseSqlite($"Data Source={_path};Pooling=False");

// Now
options.UseSqlServer($"Server=(localdb)\\MSSQLLocalDB;Database=Kingdom_{_path};Trusted_Connection=True;TrustServerCertificate=true;");
```

(The `_path` becomes part of the database name now — pick a sensible scheme.)

That's the swap. **Three lines. Maybe four.**

## Re-generate migrations

Migrations contain provider-specific SQL. When you change providers, regenerate:

```powershell
# Delete the old migrations
rm -r Kingdom.Persistence/Migrations

# Generate fresh ones for SQL Server
dotnet ef migrations add InitialCreate --project Kingdom.Persistence --startup-project Kingdom.Console
```

(In production you'd never `rm -r` migrations on a live DB. For a learning swap, fresh is fine.)

## Run

```powershell
dotnet build
dotnet test
```

**All tests pass.** Same engine, same store API, different DB underneath. **The engine doesn't care.**

## Confirm with SSMS or LocalDB

```powershell
sqlcmd -S "(localdb)\MSSQLLocalDB" -Q "SELECT name FROM sys.databases"
```

You'll see `Kingdom_xxx` in the list. The kingdoms table is there with rows from your tests.

## Tinker

- Read the EF Core docs for one provider-specific feature (e.g., `HasIndex().IsUnique()`). Some work the same; some have small differences.
- Switch back to SQLite. **Same three-line change in reverse.** The swap is symmetric.
- Try PostgreSQL: `Npgsql.EntityFrameworkCore.PostgreSQL` package. Same shape — `UsePostgresql(...)`. Same anticlimax.
- Look at the generated SQL. Run with `LogTo(Console.WriteLine, LogLevel.Information)`. **Different SQL than SQLite** — but the C# above hasn't changed.

## Name it

- **Provider** — EF's DB-specific adapter.
- **`UseSqlite` / `UseSqlServer` / `UsePostgresql`** — provider config calls.
- **Migration regen** — change provider → fresh migrations.
- **Dialect** — small SQL differences between DBs. EF hides them.

## The rule of the through-line

> **Engine vs shell, across the wire.** SQLite was a shell choice. SQL Server is another shell choice. The engine — the entities, the contexts, the queries — didn't change. **Three lines. Tests pass. The discipline pays off.**

## Quiz / challenge

Open `quiz.md`.

## Connect

B1.3 introduces **SSMS** — SQL Server Management Studio — the professional GUI for browsing + querying SQL Server. Five minutes to learn; useful for the rest of your career.
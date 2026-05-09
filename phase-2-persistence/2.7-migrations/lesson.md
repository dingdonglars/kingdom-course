# Module 2.7 — Migrations

`EnsureCreated()` works for an empty database. But what happens when you ship version 1, players have data, and version 2 needs a new column? You can't just delete and recreate — you'd lose every save. Today we meet **migrations** — the proper, versioned way to evolve a schema.

A *migration* (in the database sense) is a small recorded change to the schema — an "Up" step that applies the change and a "Down" step that reverses it. We're using the word for the first time today; it'll come up again in Phase 3 and beyond. The idea is the same anywhere a database lives.

> **Words to watch**
>
> - **migration** — a versioned schema change. Each one has an "Up" (apply) and "Down" (revert).
> - **`dotnet ef`** — the EF Core CLI tool for generating and applying migrations.
> - **`add`** — generate a new migration from the current entity model.
> - **`update`** — apply pending migrations to the database.
> - **drift** — when the database schema doesn't match the model. The thing migrations exist to prevent.

---

## Why migrations

Imagine version 1 of Kingdom shipped. Players have 50 kingdoms saved. Version 2 adds a `Citizens` table. You can't:

- Drop the database (player data lost)
- Edit the existing tables manually (every player has to do this)
- Hope EF figures it out (`EnsureCreated` won't, and shouldn't)

What you need is **a recorded list of schema changes**, applied in order, kept in sync with the code. That's a migration.

```
00_InitialCreate     → CREATE TABLE kingdoms (...);
01_AddBuildings      → CREATE TABLE buildings (...);
02_AddCitizens       → CREATE TABLE citizens (...);
03_AddSavedAt        → ALTER TABLE kingdoms ADD COLUMN saved_at TEXT;
```

Each migration has a timestamp, an "Up" method (apply), and a "Down" method (revert). EF stores the applied list in a special `__EFMigrationsHistory` table. When you deploy v2, EF sees `00`, `01`, `02` are applied, `03` is not — runs only `03`. Ship-safe schema evolution.

## Delta starter

This module is more workflow than code. The starter:

- **MODIFIED:** `Kingdom.Persistence/Kingdom.Persistence.csproj` (adds `Microsoft.EntityFrameworkCore.Design` package)
- **NEW:** `Kingdom.Persistence/Migrations/` (folder with the generated migration files — created by `dotnet ef migrations add`)
- **MODIFIED:** `Kingdom.Persistence/EfCore/KingdomEfStore.cs` — uses `Database.Migrate()` instead of `EnsureCreated()`
- **NEW:** `tests/Kingdom.Persistence.Tests/MigrationsTests.cs` — verifies the migration apply path works

## Step 0 — install the design package and tool

```powershell
cd Kingdom.Persistence
dotnet add package Microsoft.EntityFrameworkCore.Design

# install the dotnet-ef CLI globally (one-time)
dotnet tool install --global dotnet-ef
# or update if installed: dotnet tool update --global dotnet-ef
```

`dotnet-ef` is a CLI tool, not a runtime dependency. It generates code at *design time*. Production deploys don't need it.

## Step 1 — generate the initial migration

From the repo root:

```powershell
dotnet ef migrations add InitialCreate --project Kingdom.Persistence --startup-project Kingdom.Console
```

What you'll see:

- A new folder `Kingdom.Persistence/Migrations/` with files like:
  - `20260503120000_InitialCreate.cs` — the Up/Down methods
  - `20260503120000_InitialCreate.Designer.cs` — model snapshot at this migration
  - `KingdomDbContextModelSnapshot.cs` — the *current* model snapshot

The `Up` method has `migrationBuilder.CreateTable(...)` calls — readable C# that EF translates to SQL at apply time.

> **Project + startup-project.** The migration tool needs to *load your DbContext*, which means starting up a project. We use `Kingdom.Console` because that's our entry point. The migration code goes in `Kingdom.Persistence` (where `KingdomDbContext` lives).

## Step 2 — apply the migration

Two ways:

**A. CLI:**

```powershell
dotnet ef database update --project Kingdom.Persistence --startup-project Kingdom.Console
```

Updates the DB at the path your `OnConfiguring` is using. You'll see `Applying migration '20260503120000_InitialCreate'`.

**B. From code (more useful in a real app):**

In `Kingdom.Persistence/EfCore/KingdomEfStore.cs`, replace `EnsureCreated()` with:

```csharp
public void EnsureCreated()
{
    using var ctx = new KingdomDbContext(_dbPath);
    ctx.Database.Migrate();   // applies all pending migrations
}
```

Now every Save call brings the database forward to the current model. Hands-off for the player.

`EnsureCreated` and `Migrate` *don't mix* — once a database is created via `EnsureCreated`, EF won't accept migrations on it (and vice versa). Pick one and stick with it. **Migrate** is the production answer.

## Step 3 — change the model and add a second migration

Suppose we want to track when each save happened. Add a property to `KingdomEntity`:

```csharp
public DateTime SavedAt { get; set; }
```

Generate a new migration:

```powershell
dotnet ef migrations add AddSavedAt --project Kingdom.Persistence --startup-project Kingdom.Console
```

EF compares the *current* model to the *previous* snapshot, generates only the difference:

```csharp
// Inside the generated 20260503130000_AddSavedAt.cs
protected override void Up(MigrationBuilder migrationBuilder)
{
    migrationBuilder.AddColumn<DateTime>(
        name: "SavedAt",
        table: "Kingdoms",
        type: "TEXT",
        nullable: false,
        defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
}
```

Apply it: `dotnet ef database update --project Kingdom.Persistence --startup-project Kingdom.Console`. Existing rows get `SavedAt = 0001-01-01` (the default). New rows get whatever you write.

(For the starter we'll only ship `InitialCreate` — adding `SavedAt` is left as a Tinker.)

## Step 4 — set `SavedAt` in `Save`

In `KingdomEfStore.Save`, add:

```csharp
SavedAt = DateTime.UtcNow,
```

To the entity initialiser. Now every save records its timestamp.

## Step 5 — tests

`tests/Kingdom.Persistence.Tests/MigrationsTests.cs`:

```csharp
using Kingdom.Persistence.EfCore;
using Microsoft.EntityFrameworkCore;
using Shouldly;

namespace Kingdom.Persistence.Tests;

public class MigrationsTests
{
    [Fact]
    public void Migrate_OnFreshDatabase_CreatesSchema()
    {
        var path = Path.Combine(Path.GetTempPath(), $"mig-{Guid.NewGuid():N}.db");
        try
        {
            using (var ctx = new KingdomDbContext(path))
                ctx.Database.Migrate();

            using (var ctx = new KingdomDbContext(path))
                ctx.Kingdoms.Count().ShouldBe(0);   // table exists, empty
        }
        finally { if (File.Exists(path)) File.Delete(path); }
    }

    [Fact]
    public void Migrate_OnAlreadyMigratedDatabase_IsIdempotent()
    {
        var path = Path.Combine(Path.GetTempPath(), $"mig-{Guid.NewGuid():N}.db");
        try
        {
            using (var ctx = new KingdomDbContext(path)) ctx.Database.Migrate();
            using (var ctx = new KingdomDbContext(path)) ctx.Database.Migrate();   // no-op second time
            using (var ctx = new KingdomDbContext(path)) ctx.Kingdoms.Count().ShouldBe(0);
        }
        finally { if (File.Exists(path)) File.Delete(path); }
    }

    [Fact]
    public void GetPendingMigrations_AfterMigrate_IsEmpty()
    {
        var path = Path.Combine(Path.GetTempPath(), $"mig-{Guid.NewGuid():N}.db");
        try
        {
            using var ctx = new KingdomDbContext(path);
            ctx.Database.Migrate();
            ctx.Database.GetPendingMigrations().ShouldBeEmpty();
        }
        finally { if (File.Exists(path)) File.Delete(path); }
    }
}
```

Run:

```powershell
dotnet test
```

Expect `Passed: 63` (60 + 3).

## Tinker

Add the `SavedAt` migration as described in Step 3. Look at the generated SQL: `ALTER TABLE Kingdoms ADD ...`. Apply it on a database that has rows. Existing rows get the default; nothing's lost. That's the win.

Run `dotnet ef migrations remove --project Kingdom.Persistence --startup-project Kingdom.Console` to undo the *last unapplied* migration. (You can't `remove` an applied one — for that you use `update <PreviousMigration>`.)

Run `dotnet ef migrations script --project Kingdom.Persistence --startup-project Kingdom.Console` — outputs the SQL the migrations would run. Useful for review with a DBA on a real product.

Open the DB in DB Browser. Notice the `__EFMigrationsHistory` table. That's how EF knows what's been applied.

## What you just did

You moved from *"create the schema if it doesn't exist"* to *"evolve the schema as the code evolves."* You generated your first migration with `dotnet ef migrations add InitialCreate`, applied it both from the CLI and from code, and proved the apply path is safe to call repeatedly (three new tests, 63 passing total). You also met `__EFMigrationsHistory` — the small bookkeeping table that turns a list of migrations into ship-safe schema evolution. From here on, schema changes are a code review concern, not a production scare.

**Key concepts you can now name:**

- **migration** — a versioned, reversible schema change
- **`dotnet ef migrations add`** — generate from the model
- **`Database.Migrate()`** — apply pending migrations in code
- **`__EFMigrationsHistory`** — EF's record of applied migrations
- **schema drift** — DB and model diverging; what migrations prevent

## Wrap up

1. **Quiz** — open `quiz.md`, jot your answers in `journal/quiz-notes.md`.
2. **Progress** — one line in `journal/progress.md`: `Module 2.7 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit and push** — stage both files, commit message `Module 2.7 done`, Sync.
4. **Post in `#wins`** — one line about today, plus the URL of the commit.

Module 0.1 covers the why and the panel/CLI steps if you need a refresher. Bring quiz answers you're least sure about to the next weekly sync.

## Next

Module 2.8 — **DB tooling** — covers the *tools* around databases. DB Browser for SQLite, the `sqlite3` CLI, the VS Code SQLTools extension. Knowing the tools is half the fight.

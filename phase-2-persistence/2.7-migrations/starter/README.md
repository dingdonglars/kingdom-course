# Module 2.7 starter — migrations

Delta from M2.6:

- **MODIFIED:** `Kingdom.Persistence/Kingdom.Persistence.csproj` (adds `Microsoft.EntityFrameworkCore.Design` package)
- **MODIFIED:** `Kingdom.Persistence/EfCore/KingdomEfStore.cs` — `EnsureCreated()` now calls `Database.Migrate()` instead of `EnsureCreated()`
- **NEW:** `tests/Kingdom.Persistence.Tests/MigrationsTests.cs` (3 tests verifying the migrate path)

**Generate the actual migration files yourself** (one-time):

```powershell
# Install the global EF CLI tool if you don't have it
dotnet tool install --global dotnet-ef

# From the repo root, generate the initial migration
dotnet ef migrations add InitialCreate --project Kingdom.Persistence --startup-project Kingdom.Console
```

That writes a `Kingdom.Persistence/Migrations/` folder with:
- `<timestamp>_InitialCreate.cs` (the schema change as Up/Down)
- `<timestamp>_InitialCreate.Designer.cs`
- `KingdomDbContextModelSnapshot.cs`

Add the package and verify:

```powershell
cd Kingdom.Persistence && dotnet add package Microsoft.EntityFrameworkCore.Design && cd ..
dotnet build       # 0 errors
dotnet test        # 63 passing (60 + 3 migrations tests)
```

The tests will cause the migration to apply against temp DBs.
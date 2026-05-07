# Module 2.6 starter — EF Core (code-first)

Delta from Module 2.5:

- **MODIFIED:** `Kingdom.Persistence/Kingdom.Persistence.csproj` (adds EF Core + EF Core SQLite packages)
- **NEW:** `Kingdom.Persistence/EfCore/KingdomEntity.cs`
- **NEW:** `Kingdom.Persistence/EfCore/BuildingEntity.cs`
- **NEW:** `Kingdom.Persistence/EfCore/KingdomDbContext.cs`
- **NEW:** `Kingdom.Persistence/EfCore/KingdomEfStore.cs`
- **MODIFIED:** `Kingdom.Console/Program.cs` (EF demo)
- **NEW:** `tests/Kingdom.Persistence.Tests/KingdomEfStoreTests.cs` (3 tests)

```powershell
cd Kingdom.Persistence && dotnet add package Microsoft.EntityFrameworkCore && dotnet add package Microsoft.EntityFrameworkCore.Sqlite && cd ..
dotnet build       # 0 errors
dotnet run --project Kingdom.Console
dotnet test        # 60 passing (57 + 3)
```
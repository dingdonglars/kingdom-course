# Module 2.2 starter — JSON serialisation

Delta from Module 2.1. Files in this folder:

- **NEW project:** `Kingdom.Persistence/` (classlib) — `KingdomSummary.cs` + `KingdomJsonStore.cs` + `Kingdom.Persistence.csproj`
- **MODIFIED:** `Kingdom.Console/Program.cs` (uses the JSON store)
- **MODIFIED:** `Kingdom.Console/Kingdom.Console.csproj` (refs Persistence)
- **NEW project:** `tests/Kingdom.Persistence.Tests/` — `KingdomJsonStoreTests.cs` + csproj

Engine code unchanged.

After applying:

```powershell
dotnet sln Kingdom.slnx add Kingdom.Persistence
dotnet sln Kingdom.slnx add tests/Kingdom.Persistence.Tests
dotnet build       # 0 errors
dotnet run --project Kingdom.Console
dotnet test        # 43 passing (38 from Module 2.1 + 5 new persistence tests)
```

`saves/kingdom.json` lands as nicely indented multi-line JSON — open it in any editor.
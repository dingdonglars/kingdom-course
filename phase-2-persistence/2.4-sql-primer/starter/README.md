# Module 2.4 starter — SQL primer (SQLite)

Delta from Module 2.3:

- **MODIFIED:** `Kingdom.Persistence/Kingdom.Persistence.csproj` (adds `Microsoft.Data.Sqlite`)
- **NEW:** `Kingdom.Persistence/SqliteDemo.cs` (CREATE + INSERT + SELECT demo)
- **MODIFIED:** `Kingdom.Console/Program.cs` (calls the demo)
- **NEW:** `tests/Kingdom.Persistence.Tests/SqliteDemoTests.cs` (3 tests)

Engine and JSON code unchanged.

```powershell
cd Kingdom.Persistence && dotnet add package Microsoft.Data.Sqlite && cd ..
dotnet build       # 0 errors
dotnet run --project Kingdom.Console
dotnet test        # 54 passing (51 + 3)
```

`saves/kingdoms.db` is a real SQLite file — open in DB Browser for SQLite or query with `sqlite3 saves/kingdoms.db "SELECT * FROM kingdoms"`.
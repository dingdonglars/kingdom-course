# Module 2.5 starter — JOINs

Delta from M2.4:

- **NEW:** `Kingdom.Persistence/SqliteJoinsDemo.cs` — two tables, INNER JOIN, LEFT JOIN, GROUP BY/COUNT
- **MODIFIED:** `Kingdom.Console/Program.cs` (calls the demo)
- **NEW:** `tests/Kingdom.Persistence.Tests/SqliteJoinsDemoTests.cs` (3 tests)

After applying:

- `dotnet build` — 0 errors
- `dotnet run --project Kingdom.Console`
- `dotnet test` — 57 passing (54 from M2.4 + 3 new)
# Module 1.6 starter — LINQ

Delta from Module 1.5. Files in this folder:

- **MODIFIED:** `Kingdom.Console/Program.cs` (uses LINQ in the report)
- **NEW:** `tests/Kingdom.Engine.Tests/LinqTests.cs`

The optional `KingdomStats.cs` extension-method helper from Step 3 is **not** in the starter — copy it from the lesson if you want to try the extraction.

After applying the delta:

- `dotnet build` — 0 errors
- `dotnet run --project Kingdom.Console` — Day 1 with "Food net per day: +7" (2 farms × 5 = 10, minus 3 citizens)
- `dotnet test` — 27 passing (22 from 1.5 + 5 new)
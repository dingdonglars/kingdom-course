# Module 1.5 starter — inheritance (subclasses)

Delta from Module 1.4. Files in this folder:

- **NEW:** `Kingdom.Engine/Farm.cs`
- **NEW:** `Kingdom.Engine/Lumberyard.cs`
- **NEW:** `Kingdom.Engine/Mine.cs`
- **MODIFIED:** `Kingdom.Console/Program.cs`
- **NEW:** `tests/Kingdom.Engine.Tests/SubclassTests.cs`

`Building.cs`, `Kingdom.cs`, `Citizen.cs`, `Resource.cs`, `ResourceLedger.cs`, and the `1.4` test files are unchanged — keep your 1.4 versions.

After applying the delta:

- `dotnet build` — 0 errors
- `dotnet run --project Kingdom.Console` — Day 1 → Day 6 with Food climbing (+3/day net), Wood +3/day, Stone +2/day
- `dotnet test` — 22 passing (16 from 1.4 + 6 new)
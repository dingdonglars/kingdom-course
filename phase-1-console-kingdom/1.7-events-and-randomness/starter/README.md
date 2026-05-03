# Module 1.7 starter — events & randomness

Delta from Module 1.6. Files in this folder:

- **NEW:** `Kingdom.Engine/KingdomEvent.cs`
- **NEW:** `Kingdom.Engine/EventEngine.cs`
- **MODIFIED:** `Kingdom.Engine/Kingdom.cs` (adds `EventLog` + roll inside `AdvanceDay`)
- **MODIFIED:** `Kingdom.Console/Program.cs` (30-day run + log printout)
- **NEW:** `tests/Kingdom.Engine.Tests/EventLogTests.cs`

After applying:

- `dotnet build` — 0 errors
- `dotnet run --project Kingdom.Console` (twice) — different events each run
- `dotnet test` — 30 passing (27 from 1.6 + 3 new)

The tests are weak (we can't deterministically test `Random`-driven code). That pain is the setup for Module 1.8.
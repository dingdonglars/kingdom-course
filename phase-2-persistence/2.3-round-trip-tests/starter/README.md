# Module 2.3 starter — round-trip tests + full snapshot

Delta from Module 2.2. The engine grows a load surface (the meaningful redesign):

**Engine changes:**
- **NEW:** `Kingdom.Engine/Snapshots/KingdomSnapshot.cs`
- **MODIFIED:** `Kingdom.Engine/Kingdom.cs` (`_day` field, `ToSnapshot()`, `static LoadFrom(...)`)
- **MODIFIED:** `Kingdom.Engine/Buildings/Building.cs` (protected `(name, level)` constructor)
- **MODIFIED:** `Kingdom.Engine/Buildings/Farm.cs`, `Lumberyard.cs`, `Mine.cs` (load constructor each)
- **MODIFIED:** `Kingdom.Engine/Resources/ResourceLedger.cs` (`SetTo(r, amount)` for load only)

**Persistence changes:**
- **MODIFIED:** `Kingdom.Persistence/KingdomJsonStore.cs` (gains `SaveFull` / `LoadFull`)

**New tests:**
- **NEW:** `tests/Kingdom.Persistence.Tests/RoundTripTests.cs` — 5 facts + 1 theory (4 inline cases) = 8 round-trip tests

After applying:

- `dotnet build` — 0 errors
- `dotnet test` — 51 passing (43 from M2.2 + 8 new round-trip tests)
- All 35 engine tests still pass — the engine API is *additive*, no breaking changes
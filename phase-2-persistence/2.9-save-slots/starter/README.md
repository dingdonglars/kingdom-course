# Module 2.9 starter — save slots (CRUD)

Delta from Module 2.7:

- **NEW:** `Kingdom.Persistence/EfCore/KingdomSlotInfo.cs` — DTO for the slot list
- **MODIFIED:** `Kingdom.Persistence/EfCore/KingdomEfStore.cs` — adds `Update(id, kingdom)`, `Delete(id)`, `ListSlots()`
- **MODIFIED:** `Kingdom.Console/Program.cs` — multi-slot demo
- **NEW:** `tests/Kingdom.Persistence.Tests/SlotCrudTests.cs` — 5 tests

After applying:

- `dotnet build` — 0 errors
- `dotnet run --project Kingdom.Console`
- `dotnet test` — 68 passing (63 + 5)
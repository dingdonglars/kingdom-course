# Kingdom

A console kingdom-management game. Block 3 of the [Kingdom Curriculum](https://github.com/<your-org>/kingdom-course).

## Run it

```powershell
dotnet run --project Kingdom.Console
```

Output: Eldoria runs for 30 days, prints the final state and a deterministic event log (seeded random).

## Test it

```powershell
dotnet test
```

35 tests across `Building`, `ResourceLedger`, `Kingdom.AdvanceDay`, the building subclasses, LINQ helpers, the event log, and the event engine (with FakeItEasy + seeded random).

## What I learned

- **Engine vs shell** — engine never knows about Console; shell never knows about game rules
- **Inheritance** — `Farm`/`Lumberyard`/`Mine` override `Building.Tick`
- **LINQ** — `.OfType<>().Count()`, `.Sum(b => b.Level)`, `.OrderByDescending(...).First()`
- **Interfaces + dependency injection** — `IRandom`/`IClock` swappable
- **FakeItEasy** — surgical control of dependencies in tests
- **Sub-namespaces + global usings** — `Kingdom.Engine.Buildings` etc.

## What's next

- Phase 2 (M3): persistence — save/load the kingdom to a file, then SQLite
- Phase 3 (M4): web API — same engine, HTTP shell
- Phase 4 (M5): browser client
- Phase 5 (M6): Roblox port

## Stretch ideas

- Add a `Quarry` building (marble?)
- Add a `Mood` enum on `Citizen` and a `CitizenHappy` event
- Print the event log as CSV at the end of `Program.cs`

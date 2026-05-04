# Module 2.10 starter — interactive save-slot UI

Delta from M2.9:

- **NEW:** `Kingdom.Console/SaveSlotUI.cs` — menu loop, handlers
- **MODIFIED:** `Kingdom.Console/Program.cs` — calls `SaveSlotUI.Run(...)`
- **NEW:** `tests/Kingdom.Persistence.Tests/SaveSlotUITests.cs` — 3 tests using `Console.SetIn`/`SetOut` redirection

After applying, **wire the test project to reference the console project** (so it can see `SaveSlotUI`):

```powershell
dotnet add tests/Kingdom.Persistence.Tests reference Kingdom.Console
```

(Referencing an exe project from a test project is unusual but supported. The cleaner alternative is to extract `SaveSlotUI` into its own classlib — more ceremony than this single class warrants.)

Then verify:

- `dotnet build` — 0 errors
- `dotnet run --project Kingdom.Console` — interactive menu starts
- `dotnet test` — 71 passing (68 + 3)
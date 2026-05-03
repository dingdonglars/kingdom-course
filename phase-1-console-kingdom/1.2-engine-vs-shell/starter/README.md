# starter/

Kingdom v1 (post engine/shell split). Two projects:

- `Kingdom.Engine/` — class library, no `Main`, no `Console.WriteLine`
- `Kingdom.Console/` — console app referencing the engine

Build + run:

```powershell
dotnet build
dotnet run --project Kingdom.Console
```
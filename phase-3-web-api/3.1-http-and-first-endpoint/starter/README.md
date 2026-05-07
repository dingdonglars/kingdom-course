# Module 3.1 starter — first HTTP endpoint

Delta from Module 2.10:

- **NEW project:** `Kingdom.Api/` — minimal API
- **NEW:** `Kingdom.Api/Program.cs` (one `GET /kingdom` endpoint)
- **MODIFIED:** `Kingdom.slnx` (add the new project)
- **NEW:** `tests/Kingdom.Api.Tests/SmokeTests.cs` (placeholder)

```powershell
dotnet new web -n Kingdom.Api
dotnet add Kingdom.Api reference Kingdom.Engine
dotnet add Kingdom.Api reference Kingdom.Persistence
dotnet sln Kingdom.slnx add Kingdom.Api
```

Then replace `Kingdom.Api/Program.cs` with the version in `starter/Kingdom.Api/`.

After applying:

- `dotnet build` — 0 errors
- `dotnet run --project Kingdom.Api` — server starts on `localhost:5xxx`
- Open in a browser — JSON kingdom returned
- `dotnet test` — still 71 + 1 (smoke) = 72 passing
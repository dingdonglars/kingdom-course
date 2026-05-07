# Module 3.2 starter — DTOs + POST tick

Delta from Module 3.1:

- **NEW:** `Kingdom.Api/Dtos/TickResponse.cs`
- **MODIFIED:** `Kingdom.Api/Program.cs` — `POST /kingdom/tick?days=N`
- **NEW:** `tests/Kingdom.Api.Tests/Endpoint_GET_Kingdom_Tests.cs` (smoke; real integration in Module 3.7)

```powershell
dotnet build
dotnet run --project Kingdom.Api
# in another terminal
curl -X POST "http://localhost:5xxx/kingdom/tick?days=5"
curl http://localhost:5xxx/kingdom
```

Test count: 71 + 2 (smoke) = 73 passing.
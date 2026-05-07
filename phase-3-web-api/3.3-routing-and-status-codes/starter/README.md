# Module 3.3 starter — routing + status codes + CRUD endpoints

Delta from Module 3.2:

- **NEW:** `Kingdom.Api/Dtos/CreateKingdomRequest.cs`, `KingdomCreated.cs`
- **MODIFIED:** `Kingdom.Api/Program.cs` — backed by `KingdomEfStore`; 5 endpoints under `MapGroup("/kingdoms")`

```powershell
dotnet build
dotnet run --project Kingdom.Api
# CRUD via curl — see lesson Step 3
```

A `kingdoms.db` file lands next to the .exe. Open it in DB Browser to see the rows you created via the API.
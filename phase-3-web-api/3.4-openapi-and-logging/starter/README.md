# Module 3.4 starter — OpenAPI + logging

Delta from Module 3.3:

- **MODIFIED:** `Kingdom.Api/Kingdom.Api.csproj` — adds `Microsoft.AspNetCore.OpenApi` + `Scalar.AspNetCore`
- **MODIFIED:** `Kingdom.Api/Program.cs` — wires OpenAPI + Scalar + adds `ILogger<Program>` to the create handler

```powershell
cd Kingdom.Api && dotnet add package Microsoft.AspNetCore.OpenApi && dotnet add package Scalar.AspNetCore && cd ..
dotnet build
dotnet run --project Kingdom.Api
# Open: http://localhost:5xxx/openapi/v1.json    (the spec)
# Open: http://localhost:5xxx/scalar/v1          (the UI — Try It buttons live)
```
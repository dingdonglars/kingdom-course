# Module 3.7 starter — integration tests

Delta from M3.6:

- **MODIFIED:** `tests/Kingdom.Api.Tests/Kingdom.Api.Tests.csproj` — adds `Microsoft.AspNetCore.Mvc.Testing` + `Shouldly` + project reference to `Kingdom.Api`
- **NEW:** `tests/Kingdom.Api.Tests/IntegrationFixture.cs`
- **NEW:** `tests/Kingdom.Api.Tests/Endpoints_Integration_Tests.cs`

```powershell
cd tests/Kingdom.Api.Tests
dotnet add package Microsoft.AspNetCore.Mvc.Testing
dotnet add package Shouldly
dotnet add reference ..\..\Kingdom.Api\Kingdom.Api.csproj
cd ../..
dotnet test
```

The integration tests boot the whole API in-process. Auth-required endpoints return 401 unless you add a test auth scheme — see ASP.NET docs for `TestAuthHandler` if you want to test the full happy path.
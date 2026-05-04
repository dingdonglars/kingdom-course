# Module 3.7 — Integration Tests with `WebApplicationFactory<T>`

> **Hook:** today the test project starts your *whole API* in-memory, makes real HTTP calls to it, and asserts on the real responses. **No more "but it worked locally" surprises** — the integration test catches anything the unit tests miss because it tests *all the layers together.*

> **Words to watch**
> - **integration test** — a test that exercises multiple components together (vs. unit = one component)
> - **`WebApplicationFactory<TEntryPoint>`** — ASP.NET Core test helper that boots the app in-process; `TEntryPoint` is your `Program` class
> - **`HttpClient`** — the test gets a real client pointed at the in-memory server
> - **test fixture** — shared setup across multiple tests; `IClassFixture<T>` in xUnit

---

## Why integration tests now

Unit tests verify one method. Integration tests verify the whole path: HTTP → routing → handler → store → DB → back. **They catch the bugs only the seams produce** — wrong content-type, wrong status code, missing auth wiring, JSON shape divergence between the docs and the code.

The cost is: integration tests are slower (~100ms each vs <1ms for unit) and more brittle (any layer change can ripple). The right ratio is small: **for most APIs, ~5-10 integration tests covering the critical paths is enough.** Keep them as one fixture; share the server.

## `WebApplicationFactory` — what it is

ASP.NET Core ships with `Microsoft.AspNetCore.Mvc.Testing`. Reference it from your test project, and:

```csharp
var factory = new WebApplicationFactory<Program>();
var client = factory.CreateClient();
var response = await client.GetAsync("/kingdoms");
```

Three lines and the entire API is running, in-process, no port. The `client` is a real `HttpClient` — make any request the framework supports.

## Delta starter

- **MODIFIED:** `tests/Kingdom.Api.Tests/Kingdom.Api.Tests.csproj` — adds `Microsoft.AspNetCore.Mvc.Testing`
- **NEW:** `tests/Kingdom.Api.Tests/IntegrationFixture.cs` — shared `WebApplicationFactory` + test DB cleanup
- **NEW:** `tests/Kingdom.Api.Tests/Endpoints_Integration_Tests.cs`

## Step 0 — install the testing package

```powershell
cd tests/Kingdom.Api.Tests
dotnet add package Microsoft.AspNetCore.Mvc.Testing
dotnet add package Shouldly
dotnet add reference ..\..\Kingdom.Api\Kingdom.Api.csproj
```

`Program` needs to be visible to the test project. The `public partial class Program { }` line at the bottom of `Kingdom.Api/Program.cs` makes it so.

## Step 1 — fixture

`IntegrationFixture.cs`:

```csharp
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Hosting;

namespace Kingdom.Api.Tests;

public class IntegrationFixture : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // Use a per-fixture temp DB so each run starts clean
        var dbPath = Path.Combine(Path.GetTempPath(), $"itest-{Guid.NewGuid():N}.db");
        builder.UseSetting("ConnectionStrings:KingdomDb", dbPath);
        builder.ConfigureAppConfiguration((ctx, cfg) =>
        {
            cfg.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Google:ClientId"] = "test-client-id",
                ["Google:ClientSecret"] = "test-client-secret"
            });
        });
    }
}
```

Real auth would require a real Google flow — out of scope for tests. We swap the auth scheme to a **test scheme** that signs every request as a fixed test user (omitted here for brevity; ASP.NET docs cover `TestAuthHandler`). For this lesson, our integration tests will hit *unauthenticated* endpoints (e.g., `/`, `/openapi/v1.json`).

## Step 2 — first integration test

`Endpoints_Integration_Tests.cs`:

```csharp
using Shouldly;

namespace Kingdom.Api.Tests;

public class Endpoints_Integration_Tests : IClassFixture<IntegrationFixture>
{
    private readonly HttpClient _client;
    public Endpoints_Integration_Tests(IntegrationFixture f) => _client = f.CreateClient();

    [Fact]
    public async Task OpenApi_Spec_IsServed()
    {
        var resp = await _client.GetAsync("/openapi/v1.json");
        resp.IsSuccessStatusCode.ShouldBeTrue();
        var json = await resp.Content.ReadAsStringAsync();
        json.ShouldContain("\"openapi\":");      // every spec has this
        json.ShouldContain("/kingdoms");          // our endpoints registered
    }

    [Fact]
    public async Task UnauthenticatedKingdomsList_Returns401()
    {
        var resp = await _client.GetAsync("/kingdoms");
        ((int)resp.StatusCode).ShouldBe(401);   // requires auth (M3.5)
    }

    [Fact]
    public async Task Login_RedirectsToGoogle()
    {
        // The /login challenge should redirect to accounts.google.com
        // Disable redirect-following on this client to inspect the redirect itself
        var noFollow = new HttpClient(new HttpClientHandler { AllowAutoRedirect = false });
        // ...
        // (Concrete implementation skipped here for brevity. The test is "Status is 302 and Location starts with https://accounts.google.com".)
    }
}
```

Run:

```powershell
dotnet test
```

The integration tests boot the API in-process, make real HTTP calls, assert on real responses. **No web server, no port, no manual cleanup.**

## Step 3 — tradeoffs

Integration tests:

- Slower (~100ms each)
- Catch bugs that span layers (routing, serialisation, auth wiring)
- Fewer needed (5-10 covers the paths)
- Should be deterministic (use temp DBs, fixed seeds)

Unit tests:

- Fast (sub-ms)
- Catch logic bugs in one place
- Many needed (one per behaviour)
- Easy to keep deterministic

**Use both. Use unit tests for breadth; integration tests for the seams.**

## Tinker

- Add a test that POSTs `{"name":"Test"}` and asserts the response is `Created` with a `Location:` header. (Requires test-auth wiring — see ASP.NET docs for `TestAuthHandler`.)
- Time `dotnet test --logger "console;verbosity=detailed"` — observe how integration tests are slower than unit tests. **The cost is real.**
- Try changing a route from `/kingdoms` to `/realms` in the API. Run integration tests — they catch the renamed-route bug instantly. **That's the value.**

## Name it

- **Integration test** — exercises multiple components together.
- **`WebApplicationFactory<TEntryPoint>`** — boots your app in-process for testing.
- **`IClassFixture<T>`** — xUnit's "share this expensive setup across tests in the class."
- **Test scheme** — a fake auth scheme that signs every request as a fixed test user. Replaces the real OAuth dance in tests.
- **Per-fixture temp DB** — each test run gets its own isolated DB, gone after the run.

## The rule of the through-line

> **Test the seams.** Unit tests prove individual pieces work; integration tests prove they work *together*. Skip integration tests and the "everything looks fine but it broke in prod" surprises pile up.

## Quiz / challenge

Open `quiz.md`.

## Connect

Module 3.8 is the last *content* module of Block 5: **deploy to Azure App Service + GitHub Actions CI/CD**. After that, Module 3.9 closes Block 5 with the **AI Unlock Gate** trigger and the M4 milestone.
# Module 3.7 — Integration Tests with `WebApplicationFactory<T>`

Today the test project starts your *whole API* in-memory, makes real HTTP calls to it, and asserts on the real responses. No real port, no manual cleanup — the framework hosts the app inside the test process. This is the test class that catches the bugs unit tests can't see, the ones that live in the seams: a wrong content-type header, a route that compiles but doesn't match, an auth handler wired in the wrong order.

Up to now your tests have been *unit tests* — one method, one assertion, sub-millisecond runtime. Integration tests are the other half of a healthy test suite. They cost more (~100ms each, more setup) but pay for themselves the first time they catch a renamed-route bug before deploy.

> **Words to watch**
>
> - **integration test** — a test that exercises multiple components together (vs. a unit test, which tests one)
> - **`WebApplicationFactory<TEntryPoint>`** — ASP.NET Core test helper that boots the app in-process; `TEntryPoint` is your `Program` class
> - **`HttpClient`** — the test gets a real client pointed at the in-memory server
> - **test fixture** — shared setup across multiple tests; `IClassFixture<T>` in xUnit

---

## Why integration tests now

Unit tests verify one method. Integration tests verify the whole path: HTTP → routing → handler → store → DB → response. They catch the bugs only the seams produce — wrong content-type, wrong status code, missing auth wiring, JSON layout divergence between the docs and the code.

The cost is real: integration tests are slower (~100ms each vs <1ms for unit tests) and more brittle (any layer change can ripple). The right ratio is small: for most APIs, five to ten integration tests covering the critical paths is enough. Keep them in one fixture; share the server across tests in the class.

## `WebApplicationFactory` — what it is

ASP.NET Core ships a NuGet package called `Microsoft.AspNetCore.Mvc.Testing`. Reference it from your test project, and:

```csharp
var factory = new WebApplicationFactory<Program>();
var client = factory.CreateClient();
var response = await client.GetAsync("/kingdoms");
```

Three lines and the entire API is running, in-process, with no port. The `client` is a real `HttpClient` — make any request the framework supports, get a real response back.

## What ships in the starter

- **MODIFIED:** `tests/Kingdom.Api.Tests/Kingdom.Api.Tests.csproj` — adds `Microsoft.AspNetCore.Mvc.Testing`
- **NEW:** `tests/Kingdom.Api.Tests/IntegrationFixture.cs` — shared `WebApplicationFactory` plus test DB cleanup
- **NEW:** `tests/Kingdom.Api.Tests/Endpoints_Integration_Tests.cs`

## Step 0 — install the testing package

```powershell
cd tests/Kingdom.Api.Tests
dotnet add package Microsoft.AspNetCore.Mvc.Testing
dotnet add package Shouldly
dotnet add reference ..\..\Kingdom.Api\Kingdom.Api.csproj
```

`Program` needs to be visible to the test project. The `public partial class Program { }` line at the bottom of `Kingdom.Api/Program.cs` makes it visible — the type now exists in the API project's compiled assembly under that name.

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

Real Google auth would require a real OAuth flow — out of scope for tests. We swap the auth scheme to a **test scheme** that signs every request as a fixed test user (the ASP.NET docs cover `TestAuthHandler`). For this lesson, our integration tests will hit *unauthenticated* endpoints (`/`, `/openapi/v1.json`).

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
        json.ShouldContain("/kingdoms");          // our endpoints are registered
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
        // The /login challenge should redirect to accounts.google.com.
        // Disable auto-redirect so we can inspect the 302 itself instead of following it.
        var handler = new HttpClientHandler { AllowAutoRedirect = false };
        using var noFollow = _factory.CreateDefaultClient(handler);

        var resp = await noFollow.GetAsync("/login");

        ((int)resp.StatusCode).ShouldBe(302);
        resp.Headers.Location!.ToString().ShouldStartWith("https://accounts.google.com");
    }
}
```

Run:

```powershell
dotnet test
```

The integration tests boot the API in-process, make real HTTP calls, and assert on real responses. No web server, no port, no manual cleanup.

## Step 3 — trade-offs

Integration tests:

- Slower (~100ms each)
- Catch bugs that span layers (routing, serialisation, auth wiring)
- Fewer needed — five to ten covers the critical paths
- Should be deterministic — use temp DBs, fixed seeds

Unit tests:

- Fast (sub-millisecond)
- Catch logic bugs in one place
- Many needed — one per behaviour
- Easy to keep deterministic

Use both. Use unit tests for breadth; use integration tests for the seams.

## Tinker

Add a test that POSTs `{"name":"Test"}` and asserts the response is `Created` with a `Location:` header. (Requires test-auth wiring — see ASP.NET docs for `TestAuthHandler`.)

Time `dotnet test --logger "console;verbosity=detailed"`. Notice how integration tests run measurably slower than unit tests. The cost is real, even if it's small.

Try changing a route from `/kingdoms` to `/realms` in the API. Run the integration tests — they catch the renamed-route bug instantly. That's the value.

## The through-line

Test the seams. Unit tests prove individual pieces work; integration tests prove they work *together*. Skip integration tests and the *"everything looks fine but it broke in prod"* surprises pile up.

## What you just did

You added integration tests that boot your whole API in-memory and hit it with a real `HttpClient`. Three lines start the API; five-to-ten tests cover the critical seams — the OpenAPI spec endpoint, the auth-required guard, the redirect to Google for sign-in. You also met the trade-off explicitly: integration tests run roughly a hundred times slower than unit tests, so you keep the count small and let unit tests do the breadth. Together they form the test suite you can refactor against without manually clicking through every endpoint.

**Key concepts you can now name:**

- **integration test** — exercises multiple components together
- **`WebApplicationFactory<TEntryPoint>`** — boots your app in-process for testing
- **`IClassFixture<T>`** — xUnit's *share this expensive setup across tests in the class*
- **test scheme** — fake auth that signs every request as a fixed test user
- **per-fixture temp DB** — each run gets its own isolated DB, gone after the run

## Quiz

Open `quiz.md`. When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.

## Next

Module 3.8 is the last *content* module of Phase 3: **deploy to Azure App Service plus GitHub Actions CI/CD**. After that, Module 3.9 closes Phase 3 with the **AI Unlock** trigger and the M4 milestone.

# Module 3.7 — Integration Tests with `WebApplicationFactory<T>`

> Travou no inglês? Abra o `lesson.pt.md` — é esta mesma lição em português. Tente em inglês primeiro.

Today the test project starts your *whole API* in memory, makes real HTTP calls to it, and checks the real responses. No real port, no cleanup by hand — the framework runs the app inside the test process. This is the test class that catches the bugs unit tests can't see: the ones that happen where two parts meet. A wrong content-type header. A route that compiles but doesn't match. An auth handler set up in the wrong order.

Up to now your tests have been *unit tests* — one method, one check, and they run in under a millisecond. Integration tests are the other half of a healthy test suite. They cost more (about 100ms each, and more setup), but they pay for themselves the first time they catch a renamed-route bug before you deploy.

> **Words to watch**
>
> - **integration test** — a test that exercises multiple components together (vs. a unit test, which tests one)
> - **`WebApplicationFactory<TEntryPoint>`** — ASP.NET Core test helper that starts the app inside the test process; `TEntryPoint` is your `Program` class
> - **`HttpClient`** — the test gets a real client pointed at the in-memory server
> - **test fixture** — shared setup across multiple tests; `IClassFixture<T>` in xUnit

---

## Why integration tests now

Unit tests check one method. Integration tests check the whole path: HTTP → routing → handler → store → DB → response. They catch the bugs that only happen where two parts meet — a wrong content-type, a wrong status code, missing auth setup, or JSON that doesn't match between the docs and the code.

The cost is real: integration tests are slower (about 100ms each, against under 1ms for unit tests) and they break more easily (a change in any layer can affect them). You don't need many: for most APIs, five to ten integration tests covering the most important paths is enough. Keep them in one fixture and share the server across the tests in the class.

## `WebApplicationFactory` — what it is

ASP.NET Core ships a NuGet package called `Microsoft.AspNetCore.Mvc.Testing`. Reference it from your test project, and:

```csharp
var factory = new WebApplicationFactory<Program>();
var client = factory.CreateClient();
var response = await client.GetAsync("/kingdoms");
```

Three lines and the entire API is running, inside the test process, with no port. The `client` is a real `HttpClient` — make any request the framework supports, and you get a real response back.

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

`Program` needs to be visible to the test project. The `public partial class Program { }` line at the bottom of `Kingdom.Api/Program.cs` makes it visible — the type now exists in the API project's compiled output under that name.

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

Real Google auth would need a real OAuth flow, which we don't want inside a test. Instead, we swap the auth scheme for a **test scheme** that signs every request as one fixed test user (the ASP.NET docs cover `TestAuthHandler`). For this lesson, our integration tests will call the endpoints that *don't* need sign-in (`/`, `/openapi/v1.json`).

## Step 2 — first integration test

`Endpoints_Integration_Tests.cs`:

```csharp
using Microsoft.AspNetCore.Mvc.Testing;
using Shouldly;

namespace Kingdom.Api.Tests;

public class Endpoints_Integration_Tests : IClassFixture<IntegrationFixture>
{
    private readonly IntegrationFixture _factory;
    private readonly HttpClient _client;

    public Endpoints_Integration_Tests(IntegrationFixture f)
    {
        _factory = f;
        _client = f.CreateClient();
    }

    [Fact]
    public async Task OpenApi_Spec_IsServed()
    {
        var resp = await _client.GetAsync("/openapi/v1.json");
        resp.IsSuccessStatusCode.ShouldBeTrue();
        var json = await resp.Content.ReadAsStringAsync();
        json.ShouldContain("\"openapi\":");      // every spec has this
        json.ShouldContain("/kingdoms");          // our endpoints are registered
    }

    [Theory]
    [InlineData("GET", "/kingdoms")]
    [InlineData("POST", "/kingdoms")]
    [InlineData("GET", "/kingdoms/1")]
    [InlineData("POST", "/kingdoms/1/tick")]
    [InlineData("DELETE", "/kingdoms/1")]
    public async Task UnauthenticatedKingdomEndpoints_Return401(string method, string path)
    {
        var resp = await _client.SendAsync(new HttpRequestMessage(new HttpMethod(method), path));
        // Every /kingdoms endpoint needs auth — and refuses with a clean 401,
        // not a 302 redirect to a login page (Module 3.5). One [Theory] proves
        // it for all five at once, so a new endpoint can't slip through public.
        ((int)resp.StatusCode).ShouldBe(401);
    }

    [Fact]
    public async Task Login_RedirectsToGoogle()
    {
        // The /login challenge should redirect to accounts.google.com.
        // Disable auto-redirect so we can inspect the 302 itself instead of following it.
        using var noFollow = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });

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

The integration tests start the API inside the test process, make real HTTP calls, and check the real responses. No web server, no port, no cleanup by hand.

## Step 3 — trade-offs

Integration tests:

- Slower (about 100ms each)
- Catch bugs that cross several layers (routing, JSON, auth setup)
- You need fewer — five to ten covers the most important paths
- Should give the same result every time — use temp DBs and fixed seeds

Unit tests:

- Fast (under a millisecond)
- Catch logic bugs in one place
- You need many — about one per behaviour
- Easy to keep giving the same result every time

Use both. Use unit tests to cover a lot of ground, and integration tests to cover the places where parts meet.

## Tinker

Add a test that POSTs `{"name":"Test"}` and checks the response is `Created` with a `Location:` header. (This needs the test auth set up — see the ASP.NET docs for `TestAuthHandler`.)

Time `dotnet test --logger "console;verbosity=detailed"`. Notice that the integration tests run noticeably slower than the unit tests. The cost is real, even if it's small.

Try renaming a route from `/kingdoms` to `/realms` in the API. Run the integration tests — they catch the renamed-route bug right away. That's the value.

## The main point

Test the places where parts meet. Unit tests prove each piece works on its own. Integration tests prove the pieces work *together*. Skip integration tests, and you'll keep getting the *"everything looked fine, but it broke in production"* surprise.

## What you just did

You added integration tests that start your whole API in memory and call it with a real `HttpClient`. Three lines start the API; five to ten tests cover the most important places where parts meet — the OpenAPI endpoint, the sign-in guard, and the redirect to Google for sign-in. You also saw the trade-off clearly: integration tests run about a hundred times slower than unit tests, so you keep the number small and let unit tests cover most of the ground. Together they make a test suite you can refactor against without clicking through every endpoint by hand.

**Key concepts you can now name:**

- **integration test** — tests several components working together
- **`WebApplicationFactory<TEntryPoint>`** — starts your app inside the test process for testing
- **`IClassFixture<T>`** — xUnit's *share this expensive setup across tests in the class*
- **test scheme** — fake auth that signs every request as a fixed test user
- **per-fixture temp DB** — each run gets its own isolated DB, gone after the run

## On your own

Time to put the book away. Don't scroll back up to the steps — write the shape of an integration test from your own head. No one marks this one — it's just for you. It's the easiest way to spot what *hasn't* stuck yet, while it's still simple to fix. Getting stuck here is completely fine — that's exactly what it's for.

Write a small `[Fact]` test that starts the whole API, calls `GET /openapi/v1.json`, and checks the response is a success. Getting a live client needs three things — write them without looking:

1. The factory.
2. The client.
3. The request.

<details><summary>Stuck? Open this to check yourself.</summary>

```csharp
[Fact]
public async Task OpenApi_Spec_IsServed()
{
    var factory = new WebApplicationFactory<Program>();
    var client = factory.CreateClient();

    var resp = await client.GetAsync("/openapi/v1.json");

    resp.IsSuccessStatusCode.ShouldBeTrue();
}
```

`WebApplicationFactory<Program>` starts your whole API inside the test process — no real port, no server to start by hand. `CreateClient()` gives you a real `HttpClient` pointed at it. Then you make a normal HTTP call and check the real response. Real test code keeps the factory in a fixture and shares it, since starting the app is the slow part.

</details>

## Wrap up

1. **Quiz** — open `quiz.md`, jot your answers in `journal/quiz-notes.md`.
2. **Progress** — one line in `journal/progress.md`: `Module 3.7 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit and push** — stage both files, commit message `Module 3.7 done`, Sync.
4. **Post in `#wins`** — one line about today, plus the URL of the commit.

Module 0.1 covers the why and the panel/CLI steps if you need a refresher. Bring quiz answers you're least sure about to the next weekly sync.

## Next

Module 3.8 is the last *content* module of Phase 3: **deploy to Azure App Service plus GitHub Actions CI/CD**. After that, Module 3.9 closes Phase 3 with the **AI Unlock** trigger and the M4 milestone.

# Module 3.5 — OAuth (Sign In With Google)

Today every request can know *who* it's for. Your friend opens the URL, clicks Sign in with Google, and now the API can attribute every save to a real human. No hand-rolled passwords, no email-and-confirm flows, no arguments about hashing at two in the morning — Google does the hard part.

We're going to wire up the smallest correct version of authentication: Sign In With Google plus a session cookie. The API will gain a `/login` and `/me` endpoint, and we'll mark the kingdom endpoints as auth-required. By the end of this lesson, an unsigned-in client gets a 401; a signed-in client gets the full API.

> **Words to watch**
>
> - **OAuth 2.0** (oh-auth two-point-zero) — the protocol for "let *another service* (Google) verify the user; tell *me* who they are"
> - **OIDC** (OpenID Connect) — a layer on top of OAuth 2.0 specifically for *identity*, not just access
> - **client ID / client secret** — credentials *your app* gets from Google identifying it as a registered app
> - **claim** — a piece of identity info Google asserts about the user — `email`, `name`, `sub` (subject id)
> - **cookie auth** — after sign-in, the server sets a cookie; the browser sends it on every later request

---

## Why never roll your own auth

Building a username-plus-password-plus-reset-email-plus-2FA system *correctly* is a six-month project. Even huge companies (LinkedIn, Yahoo) have leaked their entire user databases over a single bug in this code path. **The first rule of auth: don't build it.** Use a well-tested provider — Google, Microsoft, GitHub, Auth0, Clerk — and let them carry the risk.

For a learning project, **Sign In With Google** is the simplest meaningful answer:

- One library on the .NET side: `Microsoft.AspNetCore.Authentication.Google`
- A five-minute setup in [Google Cloud Console](https://console.cloud.google.com/) to register your app and get credentials
- Users click a button; Google handles email, password, two-factor, account recovery — none of that is your code
- You get back a **claim**: *"this is user `sub=12345`, email `lyra@gmail.com`"*

## The OAuth dance, in six steps

1. **User clicks Sign in with Google** on your site.
2. **Browser redirects to Google** with `?client_id=YOUR_ID&redirect_uri=YOUR_URL&scope=email+profile`.
3. **User logs into Google** (or is already logged in).
4. **Google redirects back to your URL** with `?code=AUTHCODE`.
5. **Your server exchanges the code for an ID token** — a JWT containing the claims. This happens on the server, using your client *secret*. The user's browser never sees the secret.
6. **Your server reads the claims** and sets an auth cookie. The user is now signed in for the lifetime of the cookie.

Steps 2 through 5 are handled by the ASP.NET Core Google authentication middleware. You write almost no auth code yourself — you install the package, configure it, and protect endpoints with `.RequireAuthorization()`.

## What ships in the starter

OAuth setup has a manual side (Google Cloud Console) and a code side (NuGet packages plus `Program.cs` config).

- **MODIFIED:** `Kingdom.Api/Kingdom.Api.csproj` — adds `Microsoft.AspNetCore.Authentication.Google` plus `Microsoft.AspNetCore.Authentication.Cookies`
- **MODIFIED:** `Kingdom.Api/Program.cs` — wires Google auth, cookies, and `.RequireAuthorization()` on the `/kingdoms/*` endpoints
- **NEW:** `Kingdom.Api/appsettings.Development.json` — placeholder for `Google:ClientId` and `Google:ClientSecret`
- **NEW:** `journal/3.5-google-setup.md` — your notes from the Google Cloud Console setup

The biggest thing for this module is the Google Cloud Console setup, which you do by hand once.

## Step 0 — Google Cloud Console

In a browser:

1. Go to [console.cloud.google.com](https://console.cloud.google.com).
2. Create a new project (e.g., `kingdom-api`).
3. APIs & Services → OAuth consent screen → External → fill in the form. App name = "Kingdom"; user support email = yours. Skip the scopes step for now.
4. APIs & Services → Credentials → Create credentials → OAuth client ID:
   - Application type: Web application
   - Authorized redirect URI: `https://localhost:7XXX/signin-google` (you'll see the actual port in `dotnet run --project Kingdom.Api`)
5. Save. **Copy the Client ID and Client Secret immediately** — the secret won't be shown again.

Document this in `journal/3.5-google-setup.md`. Keep it private — never commit secrets!

## Step 1 — install packages

```powershell
cd Kingdom.Api
dotnet add package Microsoft.AspNetCore.Authentication.Google
dotnet add package Microsoft.AspNetCore.Authentication.Cookies
```

## Step 2 — store the secrets *outside* the repo

**Never** commit `ClientId` and `ClientSecret` to git. Use `dotnet user-secrets` for local development:

```powershell
cd Kingdom.Api
dotnet user-secrets init
dotnet user-secrets set "Google:ClientId" "YOUR_ID.apps.googleusercontent.com"
dotnet user-secrets set "Google:ClientSecret" "YOUR_SECRET"
```

The secrets land in `%APPDATA%/Microsoft/UserSecrets/<id>/secrets.json` — outside the repo, only on your machine. ASP.NET Core auto-loads them in development.

For deployment, the environment variables `Google__ClientId` and `Google__ClientSecret` work the same way (Azure App Service, container envs, etc.).

> **Heads up — name collision.** `Microsoft.AspNetCore.Authentication` ships its own `SystemClock` class, which clashes with our `Kingdom.Engine.Infrastructure.SystemClock`. When you write `new SystemClock()` in this file, the C# compiler sees both names and stops. Fully qualify ours: `new Kingdom.Engine.Infrastructure.SystemClock()`. This is the same family of issue as Module 1.4's `global::Kingdom.Engine.Kingdom`.

## Step 3 — wire auth in `Program.cs`

```csharp
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();

builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        // We're a JSON API. Cookie auth defaults to *redirecting* the browser to a
        // login page on auth failure (302 to /Account/Login). That's right for an
        // MVC site; for an API, every unauth request should return a clean 401, not
        // a redirect to a path that doesn't exist. Override both events.
        options.Events.OnRedirectToLogin = ctx =>
        {
            ctx.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return Task.CompletedTask;
        };
        options.Events.OnRedirectToAccessDenied = ctx =>
        {
            ctx.Response.StatusCode = StatusCodes.Status403Forbidden;
            return Task.CompletedTask;
        };
    })
    .AddGoogle(options =>
    {
        options.ClientId     = builder.Configuration["Google:ClientId"]!;
        options.ClientSecret = builder.Configuration["Google:ClientSecret"]!;
    });

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

// Login + logout
app.MapGet("/login", () => Results.Challenge(
    new AuthenticationProperties { RedirectUri = "/" },
    [GoogleDefaults.AuthenticationScheme]));

app.MapPost("/logout", async (HttpContext ctx) =>
{
    await ctx.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    return Results.Ok();
});

app.MapGet("/me", (HttpContext ctx) =>
{
    if (ctx.User.Identity?.IsAuthenticated != true)
        return Results.Unauthorized();
    return Results.Ok(new
    {
        // ASP.NET's Google handler maps each Google claim to the matching
        // `ClaimTypes` constant. Try the short name first, fall back to the
        // mapped one — different middleware versions surface different keys.
        Email = ctx.User.FindFirst("email")?.Value ?? ctx.User.FindFirst(ClaimTypes.Email)?.Value,
        Name  = ctx.User.FindFirst("name")?.Value  ?? ctx.User.FindFirst(ClaimTypes.Name)?.Value,
        Sub   = ctx.User.FindFirst("sub")?.Value   ?? ctx.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
    });
});

// ... rest of /kingdoms endpoints from M3.4 ...
// Add .RequireAuthorization() to each:
// group.MapGet("/", () => store.ListSlots()).RequireAuthorization();
// (etc.)
```

The new bits, slowly:

- **`AddAuthentication(...).AddCookie().AddGoogle(...)`** — registers two schemes: cookies for "remember the user across requests" and Google for "let me hand the actual sign-in off to Google."
- **`Results.Challenge(...)`** — kicks off the OAuth dance. The framework redirects to Google for you.
- **`ctx.User.FindFirst("email")`** — read claims from the cookie. The user is identified, parsed, ready to use.
- **`.RequireAuthorization()`** — endpoint-level guard. Without a valid auth cookie, the framework returns `401 Unauthorized` before your handler runs.

## Step 4 — try it

```powershell
dotnet run --project Kingdom.Api
```

Visit `https://localhost:7XXX/login`. You're redirected to Google → sign in → redirected back to `/`. The cookie is set.

Visit `https://localhost:7XXX/me`. JSON: `{ "Email": "...", "Name": "...", "Sub": "..." }`.

Visit `https://localhost:7XXX/kingdoms` — works (cookie present). In a private window — `401 Unauthorized` (no cookie).

## Tinker

Sign in. Open browser dev tools → Application → Cookies. You'll see a `.AspNetCore.Cookies` entry with a long opaque value. That's the auth cookie.

POST `/logout` (use Postman or `curl -X POST -b cookies.txt`). The cookie clears. `/me` now returns 401.

Try a custom authorization policy. Add `.RequireAuthorization("Admin")` somewhere and define the policy in `AddAuthorization` to require a specific claim — like `email == "you@gmail.com"`.

**Don't ship without HTTPS.** Cookie auth over plain HTTP is insecure — the cookie can be intercepted on the wire. For local dev, `https://localhost:...` is auto-set up; in production, App Service does it for you.

## The through-line

Two rules, side by side: **never commit secrets, never roll your own auth.** Together they prevent roughly half of all real-world security incidents. Use a provider; use user-secrets or environment variables; never the repo. These are the low-effort, high-payoff disciplines that separate a hobby project from one you can let other humans use.

## What you just did

You added real authentication to your API without writing a single password-handling line. Five lines of `AddAuthentication / AddCookie / AddGoogle` config wired Google's full OAuth flow into your app. Your endpoints can now know *which Google account* sent each request via the `sub` claim. Secrets live outside the repo — in `dotnet user-secrets` for dev, environment variables for prod. You also met your second name-collision teaching point: `Microsoft.AspNetCore.Authentication.SystemClock` and `Kingdom.Engine.Infrastructure.SystemClock` look identical to the compiler unless you fully qualify yours. Same family of issue as the `global::Kingdom.Engine.Kingdom` quirk in M1.4.

**Key concepts you can now name:**

- **OAuth 2.0** — protocol for handing identity verification to another service
- **OIDC** — the identity layer on top of OAuth
- **claim** — key/value asserted by the identity provider about the user
- **cookie auth** — server sets a session cookie; browser sends it on every request
- **`.RequireAuthorization()`** — endpoint guard that returns 401 if the cookie is missing
- **`dotnet user-secrets`** — local-only secret storage; never in the repo

## Quiz

Open `quiz.md`. When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.

## Next

Module 3.6 takes the *signed-in user* and links every kingdom they create to *their* `sub` (Google user id). Multiple users, each with their own kingdoms, queryable. Real multi-user persistence.

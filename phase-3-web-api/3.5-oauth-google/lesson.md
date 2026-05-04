# Module 3.5 — OAuth (Sign In With Google)

> **Hook:** today every request can know *who* it's for. Athos's friend opens the URL, clicks "Sign in with Google," and now the API can attribute every save to a real human. **No DIY passwords, no email-and-confirm flows, no hashing arguments at 2am — Google does the hard part.**

> **Words to watch**
> - **OAuth 2.0** — the protocol for "let *another service* (Google) verify the user; tell *me* who they are"
> - **OIDC** (OpenID Connect) — a layer on OAuth 2.0 specifically for *identity* (vs. just access tokens)
> - **client ID / client secret** — credentials *your app* gets from Google identifying it as a registered app
> - **claim** — a piece of identity info Google asserts about the user (`email`, `name`, `sub` (subject id))
> - **cookie auth** — after sign-in, the server sets a cookie; the browser sends it on every subsequent request

---

## Why never roll your own auth

Building a "username + password + reset email + 2FA" system right is a 6-month project. Even huge companies (LinkedIn, Yahoo) have leaked their entire user databases because of *one* bug in this code path. **The first rule of auth: don't build it.** Use a well-tested provider — Google, Microsoft, GitHub, Auth0, Clerk — and let them carry the risk.

For a learning project, **Sign In With Google** is the simplest meaningful answer:

- One library on the .NET side (`Microsoft.AspNetCore.Authentication.Google`)
- A 5-minute setup in [Google Cloud Console](https://console.cloud.google.com/) to register your app + get credentials
- Users click a button; Google handles email, password, 2FA, account recovery — none of that is your code
- You get back a **claim**: *"this is user `sub=12345`, email `lyra@gmail.com`"*

## The OAuth dance, in 6 steps

1. **User clicks "Sign in with Google"** on your site.
2. **Browser redirects to Google** with `?client_id=YOUR_ID&redirect_uri=YOUR_URL&scope=email+profile`.
3. **User logs into Google** (or is already logged in).
4. **Google redirects back to your URL** with `?code=AUTHCODE`.
5. **Your server exchanges the code for an ID token** (a JWT containing the claims) — this happens on the server, using your client *secret*. The user's browser never sees the secret.
6. **Your server reads the claims** + sets an auth cookie — the user is now signed in for the duration of the cookie.

Steps 2-5 are handled by the ASP.NET Core Google authentication middleware. **You write almost no auth code yourself** — you just install the package, configure it, and protect endpoints with `.RequireAuthorization()`.

## Delta starter

OAuth setup has a manual side (Google Cloud Console) and a code side (NuGet packages + `Program.cs` config).

- **MODIFIED:** `Kingdom.Api/Kingdom.Api.csproj` — adds `Microsoft.AspNetCore.Authentication.Google` + `Microsoft.AspNetCore.Authentication.Cookies`
- **MODIFIED:** `Kingdom.Api/Program.cs` — wires Google auth + cookies + `[Authorize]` on the `/kingdoms/*` endpoints
- **NEW:** `Kingdom.Api/appsettings.Development.json` — placeholder for `Google:ClientId` / `Google:ClientSecret`
- **NEW:** `journal/3.5-google-setup.md` — your notes from the Google Cloud Console setup

This module's *biggest* artefact is the Google Cloud Console setup — you'll do it by hand once.

## Step 0 — Google Cloud Console

In a browser:

1. Go to [console.cloud.google.com](https://console.cloud.google.com).
2. Create a new project (e.g., `kingdom-api`).
3. APIs & Services → OAuth consent screen → External → fill in the form. App name = "Kingdom"; user support email = yours. Skip the "scopes" step for now.
4. APIs & Services → Credentials → Create credentials → OAuth client ID:
   - Application type: Web application
   - Authorized redirect URI: `https://localhost:7XXX/signin-google` (you'll see this port in `dotnet run --project Kingdom.Api`)
5. Save. **Copy the Client ID and Client Secret immediately** — the secret won't be shown again.

Document this in `journal/3.5-google-setup.md` (private — never commit secrets!).

## Step 1 — install packages

```powershell
cd Kingdom.Api
dotnet add package Microsoft.AspNetCore.Authentication.Google
dotnet add package Microsoft.AspNetCore.Authentication.Cookies
```

## Step 2 — store the secrets *outside* the repo

**Never** commit `ClientId` and `ClientSecret` to git. Use **dotnet user-secrets** for local dev:

```powershell
cd Kingdom.Api
dotnet user-secrets init
dotnet user-secrets set "Google:ClientId" "YOUR_ID.apps.googleusercontent.com"
dotnet user-secrets set "Google:ClientSecret" "YOUR_SECRET"
```

Secrets land in `%APPDATA%/Microsoft/UserSecrets/<id>/secrets.json` — outside the repo, only on your machine. ASP.NET Core auto-loads them in development.

For deployment, the environment variables `Google__ClientId` / `Google__ClientSecret` work the same way (Azure App Service, container envs, etc.).

## Step 3 — wire auth in `Program.cs`

```csharp
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie()
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
app.MapGet("/login", () => Results.Challenge(new()
{
    RedirectUri = "/"
}, [GoogleDefaults.AuthenticationScheme]));

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
        Email = ctx.User.FindFirst("email")?.Value,
        Name  = ctx.User.FindFirst("name")?.Value,
        Sub   = ctx.User.FindFirst("sub")?.Value      // stable Google user id
    });
});

// ... rest of /kingdoms endpoints from M3.4 ...
// Add .RequireAuthorization() to each:
// group.MapGet("/", () => store.ListSlots()).RequireAuthorization();
// (etc.)
```

Read the new bits:

- **`AddAuthentication(...).AddCookie().AddGoogle(...)`** — registers two schemes: cookies for "remember the user across requests" and Google for "let me hand off the actual sign-in to Google."
- **`Results.Challenge(...)`** — kicks off the OAuth dance. Triggers a redirect to Google.
- **`ctx.User.FindFirst("email")`** — read claims from the cookie. The user is identified, parsed, ready to use.
- **`.RequireAuthorization()`** — endpoint-level guard. Without a valid auth cookie, returns `401 Unauthorized` before your handler runs.

## Step 4 — try it

```powershell
dotnet run --project Kingdom.Api
```

Visit `https://localhost:7XXX/login`. You're redirected to Google → sign in → redirected back to `/`. The cookie is set.

Visit `https://localhost:7XXX/me`. JSON: `{ "Email": "...", "Name": "...", "Sub": "..." }`.

Visit `https://localhost:7XXX/kingdoms` — works (cookie present). In a private window — `401 Unauthorized` (no cookie).

## Tinker

- Sign in. Open browser dev tools → Application → Cookies → see `.AspNetCore.Cookies` set with a long opaque value. That's the auth cookie.
- POST `/logout` (use Postman or `curl -X POST -b cookies.txt`). The cookie is cleared. `/me` now returns 401.
- Add `[Authorize]` (the attribute) to `Kingdom.cs` — wait, no — it's an endpoint thing. Try `.RequireAuthorization("Admin")` with a custom policy if you go further.
- **Don't ship without HTTPS.** Cookie auth over HTTP is insecure (the cookie can be intercepted). For local dev `https://localhost:...` is auto-set up.

## Name it

- **OAuth 2.0** — protocol for delegated authorization.
- **OIDC** — identity layer on top.
- **Claim** — a key/value asserted by the identity provider about the user.
- **Cookie auth** — the server sets a session cookie; the browser sends it on every request.
- **`AddAuthentication(...).AddCookie().AddGoogle(...)`** — the standard ASP.NET Core wiring.
- **`.RequireAuthorization()`** — endpoint guard; 401 if not signed in.
- **dotnet user-secrets** — local-only secret storage; never in the repo.

## The rule of the through-line

> **Never commit secrets. Never roll your own auth.**

These two together prevent ~half of all real-world security incidents. Use a provider; use user-secrets / env vars; never the repo.

## Quiz / challenge

Open `quiz.md`.

## Connect

Module 3.6 takes the *signed-in user* and links every kingdom they create to *their* `sub` (Google user id). Multiple users, each with their own kingdoms, queryable. Real multi-user persistence.
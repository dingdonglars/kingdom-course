# Module 3.5 starter — OAuth (Sign In With Google)

Delta from Module 3.4:

- **MODIFIED:** `Kingdom.Api/Kingdom.Api.csproj` — adds Google + Cookie auth packages
- **MODIFIED:** `Kingdom.Api/Program.cs` — wires auth, adds `/login`, `/logout`, `/me`, guards `/kingdoms/*` with `.RequireAuthorization()`
- **NEW:** `journal/3.5-google-setup.md` — your notes from Google Cloud Console setup

**Manual setup before running:**

```powershell
cd Kingdom.Api
dotnet add package Microsoft.AspNetCore.Authentication.Google
dotnet add package Microsoft.AspNetCore.Authentication.Cookies
dotnet user-secrets init
dotnet user-secrets set "Google:ClientId" "YOUR_CLIENT_ID"
dotnet user-secrets set "Google:ClientSecret" "YOUR_CLIENT_SECRET"
```

In Google Cloud Console: create OAuth client ID, set redirect URI `https://localhost:7XXX/signin-google`.

```powershell
dotnet run --project Kingdom.Api
# Visit https://localhost:7XXX/login → Google → callback → /me returns your identity
```

**Never commit secrets.** Use user-secrets (local) or env vars (deploy).
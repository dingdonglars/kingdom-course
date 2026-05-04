# Module 4.6 — Deploy the Frontend (Azure Static Web Apps)

> **Hook:** today the browser kingdom goes live. **Azure Static Web Apps** hosts your Vite build for free, with auto-SSL, global CDN, and the same-style GitHub Actions auto-deploy you set up for the API. Two URLs: `kingdom-api-yourname.azurewebsites.net` (back end) + `kingdom-yourname.azurestaticapps.net` (front end).

> **Words to watch**
> - **Static Web Apps (Azure)** — managed hosting for static frontends; integrates with GitHub
> - **CDN** — Content Delivery Network; serves your files from the edge nearest each user
> - **build artifact** — what Vite produces in `dist/`; the deploy uploads this
> - **same-origin / cross-origin** — your frontend at one domain calling your API at another (back to CORS)

---

## Why Static Web Apps

Static Web Apps is Azure's "free + managed" answer for hobby frontends:

- Free tier: 100GB bandwidth/month, custom domain, free SSL — generous
- Native GitHub integration: connect a repo + branch + the build folder; auto-deploys on push
- Global CDN: fast loads from anywhere
- Optional: built-in auth, API integration, password protection

(Alternatives: Cloudflare Pages, Netlify, Vercel, GitHub Pages. Same patterns; same outcome.)

## The deploy in 4 steps

1. **Create the Static Web App** in Azure Portal:
   - Name: `kingdom-yourname`
   - Plan: **Free**
   - Source: GitHub → pick your repo + branch
   - Build presets: Custom
   - App location: `web-vite/`
   - Output location: `dist/`
2. **Wait** ~2 minutes — Azure creates the resource AND commits a workflow file to your repo (`.github/workflows/azure-static-web-apps-*.yml`).
3. **Pull** the change to your local repo (`git pull`).
4. **Done** — your frontend is live at `kingdom-yourname.azurestaticapps.net`.

The Azure-generated workflow knows how to find the Vite build (because you told it `dist/`). Every push to `main` builds + deploys.

## Update CORS on the API

Your API now needs to allow the Static Web Apps origin:

```csharp
app.UseCors(p => p
    .WithOrigins(
        "https://localhost:5173",                       // Vite dev
        "https://kingdom-yourname.azurestaticapps.net"  // prod
    )
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials());                                // for the auth cookie
```

`AllowCredentials()` is needed because cookie auth requires it — `AllowAnyOrigin()` is *incompatible* with `AllowCredentials()` (browser security). You must list specific origins.

## Update the OAuth redirect URI

In Google Cloud Console: add the Static Web App callback. *But wait* — the auth flow happens on the API, not the frontend. So no new redirect URI needed there. The frontend redirects to `https://kingdom-api-yourname.azurewebsites.net/login` (which then redirects to Google, etc.). Same OAuth dance as before; the new origin is just a different page that initiates it.

## Update the frontend's `API` constant for prod

Two options:

**A. Hard-code per environment:**

```ts
const API = import.meta.env.PROD
  ? 'https://kingdom-api-yourname.azurewebsites.net'
  : 'https://localhost:5xxx';
```

`import.meta.env.PROD` is `true` in `npm run build` output, `false` in `npm run dev`.

**B. Vite env vars:**

`.env.production`:

```
VITE_API_URL=https://kingdom-api-yourname.azurewebsites.net
```

`.env.development`:

```
VITE_API_URL=https://localhost:5xxx
```

In code: `const API = import.meta.env.VITE_API_URL;`. Cleaner; harder to misconfigure.

## Delta starter

- **NEW:** `web-vite/.env.development` + `.env.production`
- **MODIFIED:** `web-vite/src/main.ts` — uses `import.meta.env.VITE_API_URL`
- **MODIFIED:** `Kingdom.Api/Program.cs` — CORS allow-list with credentials
- **NEW:** `journal/4.6-deploy-frontend.md` — your Azure setup notes

## Tinker

- Open `https://kingdom-yourname.azurestaticapps.net` in an incognito window. **You can play through.**
- `npm run build` locally + `npm run preview` → tests the prod build before deploy. Catches "works in dev, breaks in prod" issues early.
- Add a `404.html` for SPA routing (when a deep URL is hit). Static Web Apps handles this via `staticwebapp.config.json`.
- Check the GitHub Actions tab — every push triggers a build + deploy. ~3 minutes from push to live.

## Name it

- **Static Web Apps** — Azure's free static hosting + GitHub integration.
- **Build artifact (`dist/`)** — what Vite outputs; what Azure deploys.
- **`import.meta.env.VITE_*`** — Vite's compile-time env vars.
- **`AllowCredentials()`** — CORS flag needed for cookie auth; incompatible with `AllowAnyOrigin()`.

## The rule of the through-line

> **Two services, one architecture.** Frontend (Static Web Apps) + backend (App Service) is the standard production shape. Each scales independently; each deploys independently. The CORS dance is the necessary friction.

## Quiz / challenge

Open `quiz.md`.

## Connect

Module 4.7 closes Block 6: **M5 milestone close + reflection.** Re-read your Phase 0 code (Spark Week toys) — notice how far you've come.
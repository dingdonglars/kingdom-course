# Module 4.6 — Deploy the Frontend (Azure Static Web Apps)

The browser kingdom goes live today. **Azure Static Web Apps** hosts your Vite build for free, with auto-SSL, a global content delivery network, and the same kind of GitHub Actions auto-deploy you set up for the API in Phase 3. Two URLs at the end of the day: one for the back end (`kingdom-api-yourname.azurewebsites.net`) and one for the front end (`kingdom-yourname.azurestaticapps.net`).

> **Words to watch**
>
> - **Static Web Apps (Azure)** — managed hosting for static frontends; integrates with GitHub.
> - **CDN** — Content Delivery Network; serves your files from the edge server nearest each user.
> - **build output** — what Vite produces in `dist/`; the deploy uploads this.
> - **same-origin / cross-origin** — your frontend at one domain calling your API at another (back to CORS).

---

## Why Static Web Apps

Static Web Apps is Azure's free, managed answer for hobby frontends. The free tier gives 100GB of bandwidth a month, custom-domain support, free SSL — generous for a project like this. It integrates natively with GitHub, so connecting a repo, branch, and build folder makes every push auto-deploy. A global CDN means the page loads quickly from anywhere. Built-in auth and password protection are optional extras.

(Alternatives exist: Cloudflare Pages, Netlify, Vercel, GitHub Pages. Same patterns, same outcome.)

## Step 1 — create the Static Web App

In the Azure Portal:

- Name: `kingdom-yourname`
- Plan: **Free**
- Source: GitHub → pick your repo and branch
- Build presets: Custom
- App location: `web-vite/`
- Output location: `dist/`

Wait about two minutes. Azure creates the resource and commits a workflow file to your repo at `.github/workflows/azure-static-web-apps-*.yml`. Pull the change locally — in VS Code's Source Control panel, click the `...` menu → **Pull**. (Or in the terminal: `git pull`.)

That's the deploy. Your frontend is live at `kingdom-yourname.azurestaticapps.net`. The Azure-generated workflow knows how to find the Vite build (because you told it `dist/`), and every push to `main` rebuilds and redeploys.

## Step 2 — update CORS on the API

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

`AllowCredentials()` is needed because cookie auth requires it. There's a CORS rule worth knowing: `AllowAnyOrigin()` is *incompatible* with `AllowCredentials()`. The browser refuses the combination because it would let any website on the internet send authenticated requests to your API on behalf of your user. You have to list specific origins.

## Step 3 — the OAuth redirect URI (no change today)

In Google Cloud Console you'd usually add the new origin's callback. *Wait* — the auth flow happens on the API, not the frontend. The frontend redirects to `https://kingdom-api-yourname.azurewebsites.net/login`, which then redirects to Google. So no new redirect URI is needed in Google's console; the new origin is just a different page that initiates the same dance.

## Step 4 — point the frontend at the prod API

Two options. Pick one.

**Option A — hard-code per environment:**

```ts
const API = import.meta.env.PROD
  ? 'https://kingdom-api-yourname.azurewebsites.net'
  : 'https://localhost:5xxx';
```

`import.meta.env.PROD` is `true` in `npm run build` output and `false` in `npm run dev`.

**Option B — Vite env vars (cleaner):**

`.env.production`:

```
VITE_API_URL=https://kingdom-api-yourname.azurewebsites.net
```

`.env.development`:

```
VITE_API_URL=https://localhost:5xxx
```

In code: `const API = import.meta.env.VITE_API_URL;`. Cleaner, harder to misconfigure.

## What changes in this module

- **NEW:** `web-vite/.env.development` and `web-vite/.env.production`
- **MODIFIED:** `web-vite/src/main.ts` — uses `import.meta.env.VITE_API_URL`
- **MODIFIED:** `Kingdom.Api/Program.cs` — CORS allow-list with credentials
- **NEW:** `journal/4.6-deploy-frontend.md` — your Azure setup notes

## Tinker

Open `https://kingdom-yourname.azurestaticapps.net` in an incognito window. You can play through. Show a friend; they don't even need to install anything.

Run `npm run build` locally, then `npm run preview`. That tests the production build before deploy and catches "works in dev, breaks in prod" issues early.

Add a `staticwebapp.config.json` for SPA routing — when a deep URL is hit, the file tells Static Web Apps to fall back to `index.html`.

Check the GitHub Actions tab. Every push triggers a build and deploy. About three minutes from push to live.

## What you just did

The frontend is on the internet. You created a Static Web App in Azure (free tier), let it generate the GitHub Actions workflow, pulled the workflow file locally, and your Vite project now builds and deploys on every push to `main`. You added the production origin to the API's CORS allow-list and learned why `AllowAnyOrigin()` and `AllowCredentials()` can't coexist. The frontend reads `import.meta.env.VITE_API_URL` so dev and prod point at different APIs without any code change. Two services live side by side now: the API at one URL, the frontend at another. Standard production layout.

**Key concepts you can now name:**

- **Static Web Apps** — Azure's free static hosting plus GitHub integration
- **build output (`dist/`)** — what Vite outputs; what Azure deploys
- **`import.meta.env.VITE_*`** — Vite's compile-time env vars
- **`AllowCredentials()`** — CORS flag needed for cookie auth; incompatible with `AllowAnyOrigin()`
- **two services, one architecture** — frontend and backend deploy independently

## Quiz

Open `quiz.md`. When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.

## Next

Module 4.7 closes Phase 4: the M5 milestone ritual plus the Phase 0 reflection — re-read your Spark Week code with new eyes and notice how far you've come.

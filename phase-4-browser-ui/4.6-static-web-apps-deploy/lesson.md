# Module 4.6 — Deploy the Frontend (Azure Static Web Apps)

The browser kingdom goes live on the internet today. **Azure Static Web Apps** hosts your Vite build for free, with automatic SSL, a global content delivery network, and the same kind of GitHub Actions auto-deploy you set up for the API in Phase 3. By the end of the day you'll have two URLs: one for the back end (`kingdom-api-yourname.azurewebsites.net`) and one for the front end (`kingdom-yourname.azurestaticapps.net`).

> **Words to watch**
>
> - **Static Web Apps (Azure)** — managed hosting for static frontends; integrates with GitHub.
> - **CDN** — Content Delivery Network; serves your files from the edge server nearest each user.
> - **build output** — what Vite produces in `dist/`; the deploy uploads this.
> - **same-origin / cross-origin** — your frontend at one domain calling your API at another (back to CORS).

---

## Why Static Web Apps

Static Web Apps is Azure's free, managed option for small frontends. The free tier gives 100GB of bandwidth a month, custom-domain support, and free SSL — plenty for a project like this. It connects directly to GitHub, so once you point it at a repo, a branch, and a build folder, every push deploys on its own. A global CDN means the page loads quickly no matter where in the world the user is. Built-in auth and password protection are there if you want them.

(Other services do the same job: Cloudflare Pages, Netlify, Vercel, GitHub Pages. Same patterns, same result.)

## Step 1 — create the Static Web App

In the Azure Portal:

- Name: `kingdom-yourname`
- Plan: **Free**
- Source: GitHub → pick your repo and branch
- Build presets: Custom
- App location: `web-vite/`
- Output location: `dist/`

Wait about two minutes. Azure creates the resource and commits a workflow file to your repo at `.github/workflows/azure-static-web-apps-*.yml`. Pull that change down to your machine — in VS Code's Source Control panel, click the `...` menu → **Pull**. (Or in the terminal: `git pull`.)

That's the deploy done. Your frontend is live at `kingdom-yourname.azurestaticapps.net`. The workflow Azure made knows where to find the Vite build (because you told it `dist/`), and every push to `main` builds and deploys it again.

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

`AllowCredentials()` is needed because cookie auth requires it. There's a CORS rule worth knowing: `AllowAnyOrigin()` *cannot be used together with* `AllowCredentials()`. The browser refuses that combination, because it would let any website on the internet send signed-in requests to your API while pretending to be your user. So you have to list the specific origins instead.

## Step 3 — the OAuth redirect URI (no change today)

In Google Cloud Console you'd normally add the new origin's callback. But wait — the auth flow happens on the API, not on the frontend. The frontend sends the user to `https://kingdom-api-yourname.azurewebsites.net/login`, which then sends them to Google. So you don't need a new redirect URI in Google's console; the new origin is just a different page that starts the same login flow.

## Step 4 — point the frontend at the prod API

Two options. Pick one.

**Option A — hard-code per environment:**

```ts
const API = import.meta.env.PROD
  ? 'https://kingdom-api-yourname.azurewebsites.net'
  : 'https://localhost:5xxx';
```

`import.meta.env.PROD` is `true` in the `npm run build` output and `false` in `npm run dev`.

**Option B — Vite env vars (cleaner):**

`.env.production`:

```
VITE_API_URL=https://kingdom-api-yourname.azurewebsites.net
```

`.env.development`:

```
VITE_API_URL=https://localhost:5xxx
```

In code: `const API = import.meta.env.VITE_API_URL;`. Cleaner, and harder to set up wrong.

## What changes in this module

- **NEW:** `web-vite/.env.development` and `web-vite/.env.production`
- **MODIFIED:** `web-vite/src/main.ts` — uses `import.meta.env.VITE_API_URL`
- **MODIFIED:** `Kingdom.Api/Program.cs` — CORS allow-list with credentials
- **NEW:** `journal/4.6-deploy-frontend.md` — your Azure setup notes

## Tinker

Open `https://kingdom-yourname.azurestaticapps.net` in an incognito window. You can play through. Show a friend; they don't have to install anything.

Run `npm run build` on your machine, then `npm run preview`. That tests the production build before you deploy and catches "works in dev, breaks in prod" problems early.

Add a `staticwebapp.config.json` for SPA routing — when someone opens a deep URL, the file tells Static Web Apps to serve `index.html` instead.

Check the GitHub Actions tab. Every push starts a build and a deploy. About three minutes from push to live.

## What you just did

The frontend is on the internet. You created a Static Web App in Azure (free tier), let it generate the GitHub Actions workflow, pulled the workflow file down to your machine, and now your Vite project builds and deploys on every push to `main`. You added the production origin to the API's CORS allow-list and learned why `AllowAnyOrigin()` and `AllowCredentials()` can't be used together. The frontend reads `import.meta.env.VITE_API_URL`, so dev and prod point at different APIs with no code change. Two services now run next to each other: the API at one URL, the frontend at another. This is the normal way production apps are set up.

**Key concepts you can now name:**

- **Static Web Apps** — Azure's free static hosting plus GitHub integration
- **build output (`dist/`)** — what Vite outputs; what Azure deploys
- **`import.meta.env.VITE_*`** — Vite's compile-time env vars
- **`AllowCredentials()`** — CORS flag needed for cookie auth; incompatible with `AllowAnyOrigin()`
- **two services, one architecture** — frontend and backend deploy independently

## On your own

Time to put the book away. Don't scroll back up to the steps — from your own head, list the steps that took your frontend from your machine to a live URL. What did you tell Azure? What did Azure make for you? What makes it deploy again after the first time? No one marks this — it's just for you. It's the easiest way to spot what hasn't stuck yet, while it's still simple to fix. Getting stuck here is completely fine — that's exactly what it's for.

<details><summary>Stuck? Open this to check yourself.</summary>

You should be able to name these:

- In the Azure Portal you made a **Static Web App**, picked the **Free** plan, and pointed it at your GitHub repo and branch.
- You set the **app location** to `web-vite/` and the **output location** to `dist/` (where Vite puts the build).
- Azure made a **GitHub Actions workflow file** in `.github/workflows/` and committed it to your repo. You **pulled** that file down to your machine.
- From then on, **every push to `main`** builds and deploys on its own — no extra steps.
- You added the live frontend URL to the API's **CORS allow-list**, with `AllowCredentials()` for the auth cookie (and you cannot pair that with `AllowAnyOrigin()`).

If you can say roughly that, the procedure stuck. The exact menu names matter less than the flow: tell Azure where the build is, it wires up auto-deploy, every push goes live.

</details>

## Wrap up

1. **Quiz** — open `quiz.md`, jot your answers in `journal/quiz-notes.md`.
2. **Progress** — one line in `journal/progress.md`: `Module 4.6 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit and push** — stage both files, commit message `Module 4.6 done`, Sync.
4. **Post in `#wins`** — one line about today, plus the URL of the commit.

Module 0.1 covers the why and the panel/CLI steps if you need a refresher. Bring quiz answers you're least sure about to the next weekly sync.

## Next

Module 4.7 closes Phase 4: the M5 milestone ritual plus the Phase 0 reflection — read your Spark Week code again, now that you know more, and notice how far you've come.

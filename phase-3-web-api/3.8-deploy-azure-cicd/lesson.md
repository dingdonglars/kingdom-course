# Module 3.8 — Deploy to Azure Plus GitHub Actions CI/CD

Today the kingdom is *on the internet*. A real URL — `https://kingdom-api-yourname.azurewebsites.net` — that anyone in the world can visit. And every push to `main` redeploys the site automatically through GitHub Actions. From *code on my laptop* to *live URL that deploys itself* in one module.

This is also the module where we slow down for a moment on the **good habits that keep a live service safe**. Secrets stay out of the repo. HTTPS is on. Logs are structured. The habits you set today are the ones you'll keep for the rest of your career.

> **Words to watch**
>
> - **App Service (Azure)** — Azure runs and hosts your web app for you; the Free F1 tier costs nothing
> - **CI/CD** — Continuous Integration and Continuous Deployment — build and deploy automatically on every push
> - **GitHub Actions** — GitHub's built-in way to run jobs; YAML files in `.github/workflows/`
> - **publish profile** — the login details Azure gives you so deploys can sign in
> - **environment variables** — settings for production; never put secrets in the repo, ever

---

## Why Azure App Service Free

Three reasons:

1. **Free F1 tier** — $0/month. The limits: 1 GB RAM, shared CPU, it goes to sleep after 20 minutes with no traffic, and 60 minutes of compute time per day. Plenty for a learning project.
2. **Runs .NET directly** — no Docker container needed for the simplest path; you just push the build output.
3. **Works with GitHub** — Azure has a one-click *deploy from GitHub* wizard that sets up the action for you.

Other options: Azure Container Apps, Render, Fly.io, AWS App Runner. Same ideas, different control panels.

## The deploy in five by-hand steps (one-time)

> **Watch out — this is real cloud setup. Best done with the Azure portal open. Write down each step in `journal/3.8-deploy-api.md`.**

1. **Create an App Service**
   - portal.azure.com → Create resource → Web App
   - Name: `kingdom-api-<yourname>` (must be globally unique)
   - Runtime stack: .NET 10 (or latest LTS)
   - Pricing: **F1 Free**
   - Region: the one nearest to you (so it responds faster)
2. **Add the OAuth redirect URI to your Google client** in Google Cloud Console:
   - `https://kingdom-api-<yourname>.azurewebsites.net/signin-google`
3. **Set production environment variables** in the App Service → Configuration:
   - `Google__ClientId` = your prod (or same dev) client id
   - `Google__ClientSecret` = the secret
   - Note: double underscore `__` becomes `:` for ASP.NET config — `Google:ClientId`
4. **Get the publish profile** (or set up Federated identity):
   - App Service → Get publish profile → download the `.PublishSettings` file
5. **Add the publish profile to GitHub Secrets**:
   - GitHub repo → Settings → Secrets and variables → Actions → New secret
   - Name: `AZURE_WEBAPP_PUBLISH_PROFILE`
   - Value: paste the contents of the `.PublishSettings` file

## The CI/CD workflow

`.github/workflows/deploy.yml`:

```yaml
name: Build & deploy to Azure App Service

on:
  push:
    branches: [main]
  workflow_dispatch:        # also let me trigger manually from the UI

env:
  AZURE_WEBAPP_NAME: kingdom-api-yourname        # CHANGE this
  DOTNET_VERSION: '10.0.x'

jobs:
  build:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-dotnet@v4
        with:
          dotnet-version: ${{ env.DOTNET_VERSION }}

      - name: Restore
        run: dotnet restore

      - name: Build
        run: dotnet build --configuration Release --no-restore

      - name: Test
        run: dotnet test --configuration Release --no-build --verbosity minimal

      - name: Publish
        run: dotnet publish Kingdom.Api/Kingdom.Api.csproj -c Release -o publish

      - name: Upload artifact
        uses: actions/upload-artifact@v4
        with:
          name: app
          path: publish/

  deploy:
    needs: build
    runs-on: ubuntu-latest
    steps:
      - uses: actions/download-artifact@v4
        with:
          name: app
          path: publish/

      - name: Deploy to Azure
        uses: azure/webapps-deploy@v3
        with:
          app-name: ${{ env.AZURE_WEBAPP_NAME }}
          publish-profile: ${{ secrets.AZURE_WEBAPP_PUBLISH_PROFILE }}
          package: publish/
```

Read what this does:

- **`on: push: branches: [main]`** — runs on every push to `main`
- **Job 1 (`build`)** — restore, build, test, then publish to a `publish/` directory and upload it as a build output
- **Job 2 (`deploy`)** — download the build output, then deploy it to Azure
- **The tests run before the deploy.** A failing test stops the deploy. **CI is the guard.**

**Commit and push the workflow.** In VS Code's Source Control panel (`Ctrl + Shift + G G`):

1. Stage `.github/workflows/deploy.yml` — click `+` next to it.
2. Commit message: *"[infra] add deploy-to-Azure workflow"*.
3. Click the blue **checkmark** to commit.
4. Click **Sync Changes** to push to GitHub.

> **Or in the terminal:**
>
> ```powershell
> git add .github/workflows/deploy.yml
> git commit -m "[infra] add deploy-to-Azure workflow"
> git push
> ```

GitHub → Actions tab → watch the workflow run. About three minutes later, your site is live.

## Checklist — habits that keep a live service safe

- [x] **Secrets**: in env vars / Key Vault, never the repo (Module 3.5 + this module)
- [x] **HTTPS only**: App Service → TLS/SSL settings → "HTTPS Only = On"
- [x] **OAuth redirect**: prod URL added to Google client
- [x] **Logging**: structured logs visible in App Service → Log stream
- [ ] **DB**: SQLite on App Service is fine for a hobby project, but on the Free tier the file is temporary — it's lost when the app restarts. For Module 3.9 and later, switch to Azure SQL Database (still free-tier eligible) or PostgreSQL.
- [ ] **Custom domain plus certificate**: optional; handled for you if you move off the Free tier.
- [ ] **Monitoring**: Application Insights (free tier) for real numbers and traces.

## Tinker

After the deploy, visit `https://kingdom-api-<yourname>.azurewebsites.net`. The kingdom is live. Send the URL to a friend. They click Sign In with Google, sign in, and start playing.

Watch `App Service → Log stream` while a friend uses it. Your structured `LogInformation` calls show up in real time.

Push a small change. Watch the action run. Watch the URL update. That's the deploy loop you'll keep using for years.

Try `https://kingdom-api-<yourname>.azurewebsites.net/openapi/v1.json` — the OpenAPI description is live too. Anyone can read how your API works.

## The main point

Never deploy by hand twice. The first deploy by hand is fine. The second one is a sign you should automate it. Automation makes deploys boring. Boring deploys are deploys you do often. When you deploy often, each change is small. Small changes mean you can move fast and fix things quickly. The workflow you wrote today is what makes all of that smooth from here on.

## What you just did

You took your local API and put it on the public internet at a URL anyone can visit. You set up an Azure App Service on the Free tier, set the production environment variables, downloaded a publish profile, and stored it as a GitHub secret. Then you wrote a GitHub Actions workflow that builds, tests, and deploys on every push to `main` — three minutes from `git push` to a live URL. The tests guard the deploy, so a failing test stops the release. You also went through the checklist of habits that keep a live service safe: HTTPS-only, secrets out of the repo, and structured logs you can watch in the App Service log stream. The deploy loop you set up today is the one you'll keep using for years.

**Key concepts you can now name:**

- **App Service** — Azure runs and hosts your web app for you; the Free F1 tier is for hobby use
- **CI/CD** — build and deploy automatically on every push
- **GitHub Actions** — runs jobs from a YAML file in `.github/workflows/`
- **publish profile** — the login details for the deploy step, stored as a GitHub secret
- **environment variables (prod)** — settings and secrets, set on the platform, never in the repo
- **HTTPS-only** — required for cookie auth; toggle on in App Service settings

## On your own

Time to put the book away. Don't scroll back up to the steps — describe the deploy loop from your own head. No one marks this one — it's just for you. It's the easiest way to spot what *hasn't* stuck yet, while it's still simple to fix. Getting stuck here is completely fine — that's exactly what it's for.

On paper, write the steps that happen automatically between you running `git push` to `main` and your site being live. Put them in order. Then answer one question: where in that order do the tests run, and what happens if a test fails?

<details><summary>Stuck? Open this to check yourself.</summary>

The loop, in order:

1. You `git push` to `main`.
2. GitHub Actions starts the workflow (because it watches `push` to `main`).
3. **Build job:** restore, build, **test**, then publish the output.
4. **Deploy job:** download that output and send it to Azure App Service.
5. About three minutes later, the live URL is updated.

The tests run in the build job, **before** the deploy. If a test fails, the build job stops, so the deploy job never runs and the broken code never reaches the live site. That's why the rule is *"never deploy by hand twice"* — automate it once, the tests guard every release, and deploys become boring and safe.

</details>

## Git move of the week — `gh pr` from the CLI

You've been opening pull requests by clicking around on github.com. The `gh` CLI is a faster way once you've used it a couple of times. Install it from [cli.github.com](https://cli.github.com/) — it's a single-file installer.

After install, run `gh auth login` once to connect it to your GitHub account.

> **This one's CLI-only — the panel doesn't have a button for it.** `gh` is a command-line tool by design:
>
> ```powershell
> gh pr create --title "M4 — Web API" --body "..."
> gh pr list                          # PRs in the current repo
> gh pr view 12                       # see PR #12 details
> gh pr checks                        # CI status of the current PR
> ```

The web page on github.com works fine. `gh` is just faster once you know the commands. Try `gh pr create` for your next PR and see whether you like it.

## Wrap up

1. **Quiz** — open `quiz.md`, jot your answers in `journal/quiz-notes.md`.
2. **Progress** — one line in `journal/progress.md`: `Module 3.8 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit and push** — stage both files, commit message `Module 3.8 done`, Sync.
4. **Post in `#wins`** — one line about today, plus the URL of the commit.

Module 0.1 covers the why and the panel/CLI steps if you need a refresher. Bring quiz answers you're least sure about to the next weekly sync.

## Next

Module 3.9 closes Phase 3: **the M4 milestone close plus the AI Unlock**. This is a big one — you set up Claude Code for the first time, and the AI mode flag changes from `pre-unlock` (Claude's starting default) to `post-unlock` (real help). Everything you build from Phase 4 onwards has Claude on as a real collaborator.

# Module 3.8 — Deploy to Azure Plus GitHub Actions CI/CD

Today the kingdom is *on the internet*. A real URL — `https://kingdom-api-yourname.azurewebsites.net` — that anyone in the world can visit. And every push to `main` redeploys the site automatically via GitHub Actions. From *code on my laptop* to *live URL with auto-deploy* in one module.

This is also the module where we slow down on **production hygiene** for a moment. Secrets stay out of the repo. HTTPS is on. Logs are structured. The patterns you set today are the ones you'll keep for the rest of your career.

> **Words to watch**
>
> - **App Service (Azure)** — managed hosting for web apps; the Free F1 tier costs nothing
> - **CI/CD** — Continuous Integration and Continuous Deployment — auto-build, auto-deploy on push
> - **GitHub Actions** — workflow runner built into GitHub; YAML files in `.github/workflows/`
> - **publish profile** — credentials Azure provides for deploys
> - **environment variables** — production config; never secrets in the repo, ever

---

## Why Azure App Service Free

Three reasons:

1. **Free F1 tier** — $0/month. Limits: 1 GB RAM, shared CPU, sleeps after 20 minutes idle, 60 minutes of compute per day. Plenty for a learning project.
2. **Native .NET hosting** — no Docker container needed for the simplest path; just push the build output.
3. **GitHub integration** — Azure has a one-click *deploy from GitHub* wizard that sets up the action for you.

Alternatives: Azure Container Apps, Render, Fly.io, AWS App Runner. Same patterns, different consoles.

## The deploy in five manual steps (one-time)

> **Heads up — real cloud setup. Best followed with the Azure portal open. Document each step in `journal/3.8-deploy-api.md`.**

1. **Create an App Service**
   - portal.azure.com → Create resource → Web App
   - Name: `kingdom-api-<yourname>` (must be globally unique)
   - Runtime stack: .NET 10 (or latest LTS)
   - Pricing: **F1 Free**
   - Region: nearest to you (latency)
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
- **Job 1 (`build`)** — restore, build, test, publish to a `publish/` directory; upload as a build output
- **Job 2 (`deploy`)** — download the build output; deploy to Azure
- **Tests run before deploy.** A failing test fails the deploy. **CI is the guard.**

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

## Production hygiene checklist

- [x] **Secrets**: in env vars / Key Vault, never the repo (Module 3.5 + this module)
- [x] **HTTPS only**: App Service → TLS/SSL settings → "HTTPS Only = On"
- [x] **OAuth redirect**: prod URL added to Google client
- [x] **Logging**: structured logs visible in App Service → Log stream
- [ ] **DB**: SQLite on App Service is fine for a hobby project, but the file is ephemeral on Free tier (lost on restart). For Module 3.9 / future, swap to Azure SQL Database (still free-tier eligible) or PostgreSQL.
- [ ] **Custom domain plus cert**: optional; auto-managed if you upgrade off Free tier.
- [ ] **Monitoring**: Application Insights (free tier) for real metrics and traces.

## Tinker

After deploy, visit `https://kingdom-api-<yourname>.azurewebsites.net`. Kingdom is live. Send the URL to a friend. They click Sign In with Google, sign in, start playing.

Watch `App Service → Log stream` while a friend uses it. Your structured `LogInformation` calls show up in real time.

Push a small change. Watch the action run. Watch the URL update. That's the deploy loop you'll keep for years.

Try `https://kingdom-api-<yourname>.azurewebsites.net/openapi/v1.json` — the OpenAPI spec is also live. Anyone can read your API contract.

## The through-line

Never deploy by hand twice. The first manual deploy is fine; the second one means you should automate. Automation makes deploys boring; boring deploys mean frequent deploys; frequent deploys mean small changes; small changes mean fast iteration. The workflow you wrote today is the lubricant under everything you'll build from this point on.

## What you just did

You took your local API and put it on the public internet at a URL anyone can visit. You set up an Azure App Service on the Free tier, configured the production environment variables, downloaded a publish profile, and stored it as a GitHub secret. Then you wrote a GitHub Actions workflow that builds, tests, and deploys on every push to `main` — three minutes from `git push` to live URL. Tests guard the deploy, so a failing test fails the release. You also walked through the production-hygiene checklist: HTTPS-only, secrets out of repo, structured logs visible in the App Service log stream. The deploy loop you set up today is the loop you'll keep using for years.

**Key concepts you can now name:**

- **App Service** — Azure's managed PaaS for web apps; Free F1 tier for hobby use
- **CI/CD** — auto-build plus auto-deploy on push
- **GitHub Actions** — workflow runner via YAML in `.github/workflows/`
- **publish profile** — credentials for the deploy step, stored as a GitHub secret
- **environment variables (prod)** — config and secrets, set in the platform, never the repo
- **HTTPS-only** — required for cookie auth; toggle on in App Service settings

## Git move of the week — `gh pr` from the CLI

You've been opening pull requests by clicking around on github.com. The `gh` CLI is a faster way once you've used it twice. Install it from [cli.github.com](https://cli.github.com/) — single-file installer.

After install, run `gh auth login` once to connect it to your GitHub account.

> **This one's CLI-only — the panel doesn't have a button for it.** `gh` is a command-line tool by design:
>
> ```powershell
> gh pr create --title "M4 — Web API" --body "..."
> gh pr list                          # PRs in the current repo
> gh pr view 12                       # see PR #12 details
> gh pr checks                        # CI status of the current PR
> ```

The web UI on github.com is fine; `gh` is just faster once your fingers know it. Try `gh pr create` for your next PR and see whether it's a fit.

## Quiz

Open `quiz.md`. When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.

## Next

Module 3.9 closes Phase 3: **the M4 milestone close plus the AI Unlock**. The big one — you set up Claude Code for the first time, and the AI mode flag flips from `pre-unlock` (Claude's brief default) to `post-unlock` (real assistance). Everything you build from Phase 4 onwards has Claude on as a real collaborator.

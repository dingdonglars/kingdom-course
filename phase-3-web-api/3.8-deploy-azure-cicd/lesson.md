# Module 3.8 — Deploy to Azure + GitHub Actions CI/CD

> **Hook:** today the kingdom is *on the internet*. A real URL — `https://kingdom-api-yourname.azurewebsites.net` — that anyone can visit. **And every push to `main` redeploys automatically** via GitHub Actions. From "code on my laptop" to "live URL with auto-deploy" in one module.

> **Words to watch**
> - **App Service (Azure)** — managed hosting for web apps; Free F1 tier costs $0
> - **CI/CD** — Continuous Integration / Continuous Deployment — auto-build, auto-deploy on push
> - **GitHub Actions** — workflow runner built into GitHub; YAML files in `.github/workflows/`
> - **publish profile** — credentials Azure provides for deploys
> - **environment variables** — production config (no secrets in repo, ever)

---

## Why Azure App Service Free

Three reasons:

1. **Free F1 tier** — $0/month. Limits: 1GB RAM, shared CPU, sleeps after 20 min idle, 60 min/day compute. Plenty for a learning project.
2. **Native .NET hosting** — no Docker needed for the simplest path; just push the build artefact.
3. **GitHub integration** — Azure has a one-click "deploy from GitHub" wizard that sets up the action for you.

(Alternatives: Azure Container Apps, Render, Fly.io, AWS App Runner. Same patterns, different consoles.)

## The deploy in 5 manual steps (one-time)

> **Heads up — real cloud setup. These steps are best followed with the Azure portal open. Document each step in `journal/3.8-deploy-api.md`.**

1. **Create an App Service**
   - portal.azure.com → Create resource → Web App
   - Name: `kingdom-api-<yourname>` (must be globally unique)
   - Runtime stack: .NET 10 (or latest LTS)
   - Pricing: **F1 Free**
   - Region: nearest to you (latency)
2. **Add the OAuth redirect URI to your Google client** — in Google Cloud Console:
   - `https://kingdom-api-<yourname>.azurewebsites.net/signin-google`
3. **Set production environment variables** in the App Service → Configuration:
   - `Google__ClientId` = your prod (or same dev) client id
   - `Google__ClientSecret` = the secret
   - (Note: double underscore `__` becomes `:` for ASP.NET config — `Google:ClientId`)
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
- **Job 1 (`build`)** — restore, build, test, publish to a `publish/` directory; upload as an artifact
- **Job 2 (`deploy`)** — download the artifact; deploy to Azure
- **Tests run before deploy.** A failing test fails the deploy. **CI is the gatekeeper.**

Commit + push the workflow:

```powershell
git add .github/workflows/deploy.yml
git commit -m "[infra] add deploy-to-Azure workflow"
git push
```

GitHub → Actions tab → watch the workflow run. ~3 minutes later, your site is live.

## Production hygiene checklist

- [x] **Secrets**: in env vars / Key Vault, never the repo (M3.5 + this module)
- [x] **HTTPS only**: App Service → TLS/SSL settings → "HTTPS Only = On"
- [x] **OAuth redirect**: prod URL added to Google client
- [x] **Logging**: structured logs visible in App Service → Log stream
- [ ] **DB**: SQLite on App Service is fine for a hobby project, but the file is ephemeral on free tier (lost on restart). **For Module 3.9 / future:** swap to Azure SQL Database (still free tier eligible) or PostgreSQL.
- [ ] **Custom domain + cert**: optional; auto-managed if you upgrade off Free.
- [ ] **Monitoring**: Application Insights (free tier) for real metrics + traces.

## Tinker

- After deploy, visit `https://kingdom-api-<yourname>.azurewebsites.net` — kingdom is live. Send the URL to a friend. They click "Sign In with Google," sign in, and start playing.
- Watch `App Service → Log stream` while a friend uses it. **Your structured `LogInformation` calls show up in real time.**
- Push a small change. Watch the action run. Watch the URL update. **That's the deploy loop you'll keep for years.**
- Try `https://kingdom-api-<yourname>.azurewebsites.net/openapi/v1.json` — the OpenAPI spec is also live. Anyone can read your API contract.

## Name it

- **App Service** — Azure's managed PaaS for web apps. Free F1 tier for hobby use.
- **CI/CD** — auto-build + auto-deploy on push. The lubricant of any non-trivial project.
- **GitHub Actions** — workflow runner via YAML files in `.github/workflows/`.
- **Publish profile** — credentials for the deploy step.
- **Environment variables (prod)** — config + secrets, set in the platform, *never* in the repo.
- **HTTPS-only** — required for cookie auth. Toggle on in App Service settings.

## The rule of the through-line

> **Never deploy by hand twice.** The first manual deploy is fine; the second one means you should automate. Automation makes deploy boring. Boring deploy = frequent deploy = small changes = fast iteration.

## Quiz / challenge

Open `quiz.md`.

## Connect

Module 3.9 closes Block 5: **M4 milestone close + AI Unlock Gate**. The big one — after M4, the AI mode flag flips from `pre-gate` (friction-only) to `post-gate` (real assistance). Everything you build from Phase 4 onwards has the AI on as a real collaborator.
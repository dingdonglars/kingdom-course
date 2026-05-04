# Module 3.8 starter — deploy + CI/CD

The bulk of this module is portal/console work. The starter ships:

- **NEW:** `.github/workflows/deploy.yml` — the GitHub Actions workflow
- **NEW:** `journal/3.8-deploy-api.md` — your notes from the Azure setup

**Manual setup before the workflow can run:**
- Create App Service (Free F1 tier)
- Add `Google__ClientId` / `Google__ClientSecret` as App Service env vars
- Add Google OAuth redirect URI: `https://<your-app>.azurewebsites.net/signin-google`
- Download the App Service publish profile
- Add it to GitHub Secrets as `AZURE_WEBAPP_PUBLISH_PROFILE`
- Edit the workflow YAML — change `AZURE_WEBAPP_NAME` to your app's name
- Enable "HTTPS Only" in App Service → TLS/SSL settings

After this once-off setup, every push to `main` triggers a new deploy automatically.
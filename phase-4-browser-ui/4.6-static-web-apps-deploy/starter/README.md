# Module 4.6 starter — Static Web Apps deploy

Mostly Azure Portal work. The starter ships:

- `web-vite/.env.development` + `.env.production` — env-driven API URL
- `web-vite/src/main.ts` — uses `import.meta.env.VITE_API_URL`
- `Kingdom.Api/Program.cs.snippet` — CORS allow-list update
- `journal/4.6-deploy.md` — your Azure setup notes

**Manual:**
1. Create Static Web App (Free) → connect GitHub repo, app location `web-vite/`, output location `dist/`
2. Pull the auto-generated workflow file
3. Update API CORS to allow the SWA URL with `AllowCredentials`
4. Add `VITE_API_URL` env vars per environment
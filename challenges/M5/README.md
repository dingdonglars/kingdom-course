# Challenge — M5 — *Kingdom v4 (Browser-Playable)*

Wraps **Block 6 (Browser Kingdom)**.

## What this checks

- A `web-vite/` (or `web/`) frontend project exists with TypeScript
- `web-vite/package.json` declares `vite`, `vitest`, and a build script
- Frontend tests exist (`*.test.ts` files in `web-vite/src/`)
- The build succeeds (`npm run build` produces `dist/`)
- Frontend tests pass (`npm test`)
- A static-web-apps deploy workflow exists (`.github/workflows/azure-static-web-apps-*.yml`)
- `journal/wins.md` has an M5 entry mentioning M5 / Block 6 / browser

## What this does NOT check

- The exact URL is live (impossible from this test runner)
- The exact components / shape of the UI
- Real OAuth flow (tested separately by signing in manually)

## How to run

```powershell
dotnet test path\to\challenges\M5\M5.Tests.csproj
```

Green = M5 met. Run the per-milestone ritual: wins post + before/after + Discord.

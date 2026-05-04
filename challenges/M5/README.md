# Challenge — M5 — *Browser-Playable Kingdom*

Wraps **Phase 4 — Browser Kingdom**.

M5 confirms the frontend is real: a Vite + TypeScript project that builds, has its own tests, and ships to a real public URL via a static-web-apps workflow. The test runner can't open a browser, so it checks the files that prove the rest.

## What this verifies

| Check | Looks for |
| --- | --- |
| Frontend project | `web-vite/` (or `web/`) with TypeScript |
| Dependencies | `web-vite/package.json` declares `vite`, `vitest`, and a build script |
| Frontend tests | `*.test.ts` files in `web-vite/src/` |
| Build succeeds | `npm run build` produces `dist/` |
| Tests pass | `npm test` is green |
| Deploy workflow | `.github/workflows/azure-static-web-apps-*.yml` exists |
| Wins entry | `journal/wins.md` with an M5 entry mentioning M5 / Phase 4 / browser |

## What this skips

- That the live URL is actually serving your site — impossible from the test runner.
- The exact components or visual layout — the mentor reviews these.
- The real OAuth flow — you sign in manually to verify that one.

## How to run

From your repo root:

```powershell
dotnet test path\to\challenges\M5\M5.Tests.csproj
```

Green = M5 met. Then the milestone ritual.

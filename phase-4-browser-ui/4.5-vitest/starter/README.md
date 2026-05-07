# Module 4.5 starter — Vitest

Delta from Module 4.4:

- **MODIFIED:** `web-vite/package.json` — adds `vitest` + `happy-dom`
- **MODIFIED:** `web-vite/vite.config.ts` — wires Vitest config (happy-dom env)
- **NEW:** `web-vite/src/components/__tests__/escape.test.ts`
- **NEW:** `web-vite/src/components/__tests__/KingdomCard.test.ts`

```powershell
cd web-vite
npm install -D vitest happy-dom
npm test
```
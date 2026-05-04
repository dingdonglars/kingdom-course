# Module 4.3 starter — TypeScript + Vite

Replaces `web/` with a Vite + TypeScript project at `web-vite/`.

```powershell
npm create vite@latest web-vite -- --template vanilla-ts
cd web-vite
npm install
# Replace src/main.ts and add src/types.ts (see lesson)
npm run dev
```

Production build: `npm run build` → static files in `web-vite/dist/`.
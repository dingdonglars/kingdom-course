# Module 4.3 — TypeScript + Vite + JS Modules

> **Hook:** today the browser code grows up. **TypeScript** brings the type-safety you've had in C# all year. **Vite** brings the modern toolchain — hot-reload, modules, fast builds. **ES modules** replace the global `<script src=...>` style. The web project starts to feel like a real codebase.

> **Words to watch**
> - **TypeScript (TS)** — JavaScript + types. Compiles to JS the browser actually runs.
> - **Vite** — modern frontend dev server + build tool. Fast.
> - **ES modules** — `import` / `export` between JS files
> - **`tsconfig.json`** — TypeScript compiler config
> - **HMR** — Hot Module Replacement; edit code, page updates without losing state

---

## Why TypeScript

Plain JS lets `slot.naem` typo through without warning. Three months later, a refactor renames `name` → `kingdomName` and you don't notice — until the field is just empty in the UI. **Same class of bug C# catches at compile time.**

TypeScript adds types: `interface KingdomSlot { id: number; name: string; day: number; }`. Editor + compiler now refuse `slot.naem`. Same JS underneath; richer authoring.

You'll keep using plain JS for tiny scripts; reach for TS as soon as a project has 3+ files or shapes anyone else needs to know.

## Why Vite

`<script src="kingdom.js">` only scales so far. Real frontends:
- Use *modules* (`import { x } from './y.ts'`) — but browsers' module support has quirks
- Compile TS → JS — needs a build step
- Want hot-reload (edit code; browser updates without manual refresh)
- Want to bundle for production (many small files → one minified)

Vite does all of that. **One-line setup, near-zero config.** It's replaced webpack/rollup as the default modern choice.

## Delta starter

Switch from `web/` (raw HTML/JS) to a Vite project:

- **NEW project:** `web-vite/` (created via `npm create vite@latest`)
- **PORTED:** the `kingdom.js` logic into `web-vite/src/main.ts` with TypeScript
- **MODIFIED:** the API project's CORS to allow Vite's dev server (default `http://localhost:5173`)

## Step 1 — create the Vite project

```powershell
npm create vite@latest web-vite -- --template vanilla-ts
cd web-vite
npm install
npm run dev
```

Vite scaffolds, installs ~20 small packages, starts a dev server. Open `http://localhost:5173`. Default Vite welcome page.

`vanilla-ts` is the template for "TypeScript, no framework." We'll layer Svelte/React/anything later if you want — for now, vanilla TS is enough.

## Step 2 — types for the API

`web-vite/src/types.ts`:

```ts
export interface KingdomSlot {
  id: number;
  name: string;
  day: number;
}
```

Single source of truth for what the slot looks like.

## Step 3 — port `kingdom.js` to `main.ts`

Replace `web-vite/src/main.ts`:

```ts
import './style.css';
import type { KingdomSlot } from './types';

const API = 'https://localhost:5xxx';   // CHANGE

async function loadKingdom(): Promise<KingdomSlot | null> {
  const resp = await fetch(`${API}/kingdoms`);
  if (!resp.ok) throw new Error(`HTTP ${resp.status}`);
  const slots = (await resp.json()) as KingdomSlot[];
  return slots[0] ?? null;
}

function render(slot: KingdomSlot | null) {
  const root = document.querySelector<HTMLDivElement>('#app')!;
  if (!slot) {
    root.innerHTML = '<p>No kingdoms yet.</p>';
    return;
  }
  root.innerHTML = `
    <header>
      <h1>${slot.name}</h1>
      <p>Day ${slot.day}</p>
    </header>
  `;
}

loadKingdom().then(render).catch(err => {
  document.querySelector<HTMLDivElement>('#app')!.textContent = `Error: ${err.message}`;
});
```

Read what's new:

- **`import './style.css';`** — Vite knows what to do with this; bundles CSS automatically.
- **`import type { KingdomSlot }`** — type-only import; erased at compile time.
- **`Promise<KingdomSlot | null>`** — return type. The compiler checks every usage.
- **`document.querySelector<HTMLDivElement>(...)`** — generic version returns the typed element.
- **`!`** (the non-null assertion) — *"trust me, this is not null"*. Use sparingly.
- **`as KingdomSlot[]`** — type cast for the parsed JSON; the compiler doesn't know the API shape until you tell it.

## Step 4 — try it

```powershell
npm run dev
```

Visit `http://localhost:5173`. The page renders. Edit `main.ts` — change a heading. **Save. The browser updates without refresh.** That's HMR.

The terminal shows TypeScript errors as you save (`error TS2322: Type 'string' is not assignable to type 'number'`). **Fix at compile time, not at runtime.**

## Step 5 — production build

```powershell
npm run build
```

Outputs `dist/` — minified JS, hashed filenames, ready to deploy. The next step would be `npm run preview` to test the production build locally, then deploy to Static Web Apps (M4.7).

## Tinker

- Add a typo: `slot.naem`. **The editor squiggles immediately.** TypeScript catches what JS lets through.
- Add a number/string mismatch: `const day: number = "twelve";`. Same — caught at editor time.
- Try `import` of a CSS module (`import styles from './stuff.module.css'`). Vite supports it natively. Class names are scoped automatically.
- Set up `tsconfig.json`'s `"strict": true`. Re-build. **Lots of errors surface** — opportunity to tighten the code.

## Name it

- **TypeScript** — JS with types. Compiles to JS.
- **Vite** — fast modern frontend dev tool + bundler.
- **ES modules** — `import` / `export` between files; the modern way.
- **HMR** — Hot Module Replacement; edit, save, see — no manual refresh.
- **`tsconfig.json`** — TS compiler settings.

## The rule of the through-line

> **Types at every boundary.** You used DTOs at the C# JSON/API boundary. TypeScript brings the same discipline to the browser side. The shape is defined once, in `types.ts`, and the rest of the code stays honest.

## Quiz / challenge

Open `quiz.md`.

## Connect

Module 4.4 builds the **componentised UI** — extracting reusable pieces (a `<KingdomCard>`, a `<ResourceList>`) so adding new screens stays cheap.
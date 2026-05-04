# Module 4.3 — TypeScript + Vite + JS Modules

The browser code grows up today. Two new tools join: **TypeScript** brings the type-checking you've had in C# all year — same `slot.day is a number` discipline, written in a JavaScript file. **Vite** brings the modern toolchain — a dev server with hot-reload, real module support, fast builds. The third change is quieter but matters: `import` and `export` between files replace the old `<script src=...>` global-namespace style. The web project starts to feel like a real codebase.

This is also the first time you'll meet TypeScript and Vite by name, so a one-line introduction for each. **TypeScript** is JavaScript with a type system bolted on; the compiler turns it into ordinary JavaScript the browser runs. **Vite** (pronounced *"veet"* — French for *fast*) is a frontend dev server and bundler. It's replaced webpack as the default modern starter; one command sets up the whole project.

> **Words to watch**
>
> - **TypeScript (TS)** — JavaScript with types. Compiles to plain JS the browser actually runs.
> - **Vite** — modern frontend dev server and bundler. Pronounced *"veet"*.
> - **ES modules** — `import` / `export` between JavaScript files; the modern way.
> - **`tsconfig.json`** — TypeScript compiler config.
> - **HMR** — Hot Module Replacement. Edit a file, save, the browser updates without losing state.

---

## Why TypeScript

Plain JavaScript lets `slot.naem` typo through without warning. Three months later, a refactor renames `name` → `kingdomName` and you don't notice — until the field is just empty in the UI. The same class of bug C# catches at compile time.

TypeScript adds types: `interface KingdomSlot { id: number; name: string; day: number; }`. Editor and compiler now refuse `slot.naem`. The same JavaScript runs underneath — TypeScript is just a richer authoring layer that gets stripped away at build time.

You'll keep using plain JavaScript for tiny scripts. Reach for TypeScript as soon as a project has three or more files, or interfaces anyone else needs to know.

## Why Vite

`<script src="kingdom.js">` only scales so far. Real frontends want modules (`import { x } from './y.ts'`), need a build step to compile TypeScript, want hot-reload while editing, and want bundling for production (many small files into a few minified ones). Vite does all of that with one-line setup and near-zero config. It's the default modern choice.

## What changes in this module

You're switching from the raw `web/` folder (HTML and JavaScript) to a Vite project.

- **NEW:** `web-vite/` — created via `npm create vite@latest`
- **PORTED:** the `kingdom.js` logic into `web-vite/src/main.ts` with TypeScript
- **MODIFIED:** the API project's CORS to allow Vite's dev server (default `http://localhost:5173`)

## Step 1 — create the Vite project

```powershell
npm create vite@latest web-vite -- --template vanilla-ts
cd web-vite
npm install
npm run dev
```

Vite scaffolds the project, installs about twenty small packages, and starts a dev server. Open `http://localhost:5173`. You'll see Vite's default welcome page.

`vanilla-ts` is the template name for "TypeScript, no framework." We're sticking with plain TypeScript for this phase; a framework can layer on later if you want.

## Step 2 — types for the API

Create `web-vite/src/types.ts`:

```ts
export interface KingdomSlot {
  id: number;
  name: string;
  day: number;
}
```

One source of truth for what a slot looks like. Anywhere in the project that handles a slot imports this interface; if you change the layout, every usage is checked.

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

loadKingdom()
    .then(render)
    .catch(err => {
        document.querySelector<HTMLDivElement>('#app')!.textContent = `Error: ${err.message}`;
    });
```

A few things in that file are new and worth naming. `import './style.css'` works because Vite knows how to bundle CSS imports. `import type { KingdomSlot }` is a type-only import — erased at compile time. `Promise<KingdomSlot | null>` is the function's return type; the compiler checks every caller. `document.querySelector<HTMLDivElement>(...)` is the typed version of `querySelector` — passing the element type means you don't have to cast the result. The `!` after the call is the non-null assertion: *"trust me, this isn't null."* Use it sparingly. The `as KingdomSlot[]` cast on the parsed JSON is your assertion that the API returns the layout you've declared — TypeScript can't verify the runtime layout, so you tell it.

## Step 4 — try it

```powershell
npm run dev
```

Visit `http://localhost:5173`. The page renders. Now edit `main.ts` — change a heading. Save. The browser updates without a full reload, and any state you had on the page survives. That's HMR. Multiplied across thousands of edits, it's hours saved.

The terminal shows TypeScript errors as you save (`error TS2322: Type 'string' is not assignable to type 'number'`). Caught at compile time, not at runtime — same win you've had in C# all year.

## Step 5 — the production build

```powershell
npm run build
```

Outputs a `dist/` folder — minified JavaScript, hashed filenames, ready to deploy. The next step would be `npm run preview` to test the production build locally, then deploy to Static Web Apps in M4.6.

## Tinker

Add a typo: `slot.naem`. The editor squiggles immediately. TypeScript catches what JavaScript would let through.

Add a number-to-string mismatch: `const day: number = "twelve";`. Same — flagged in the editor before you've saved.

Try importing a CSS module: `import styles from './stuff.module.css'`. Vite supports it natively, and class names get scoped to the file automatically.

Set `"strict": true` in `tsconfig.json` and re-build. Plenty of warnings will surface — that's the opportunity to tighten the code while it's still small.

## What you just did

The project upgraded. You scaffolded a Vite project (`web-vite/`), defined a `KingdomSlot` interface in `types.ts`, and ported the kingdom-loading code into `main.ts` as TypeScript. The compiler now refuses typos and type mismatches; HMR means edits show up in the browser without losing the page state. A production build is one command (`npm run build`) and outputs a deployable `dist/` folder. The TypeScript file is about twenty lines longer than the JavaScript version it replaced — that overhead pays back the first time a refactor catches a mistake at compile time.

**Key concepts you can now name:**

- **TypeScript** — JavaScript with types; compiles to plain JS
- **Vite** — modern frontend dev server and bundler
- **ES modules** — `import` and `export` between files
- **HMR** — edit, save, browser updates without losing state
- **types at every boundary** — the same DTO discipline, in the browser

## Quiz

Open `quiz.md`. When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.

## Next

Module 4.4 builds the **componentised UI** — extracting reusable pieces (a `KingdomCard`, a `ResourceList`) so adding a new screen stays cheap.

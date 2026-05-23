# Module 4.3 — TypeScript + Vite + JS Modules

The browser code gets more serious today. Two new tools join. **TypeScript** brings the type-checking you've had in C# all year — the same `slot.day is a number` checking, written in a JavaScript file. **Vite** brings the modern tools — a dev server with hot-reload, real module support, and fast builds. The third change is quieter but it matters: `import` and `export` between files replace the old `<script src=...>` style where everything shared one global space. The web project starts to feel like a real codebase.

This is also the first time you'll meet TypeScript and Vite by name, so here's a one-line introduction for each. **TypeScript** is JavaScript with a type system added on top; the compiler turns it into ordinary JavaScript that the browser runs. **Vite** (pronounced *"veet"* — French for *fast*) is a frontend dev server and bundler. It has replaced webpack as the default modern starting point; one command sets up the whole project.

> **Words to watch**
>
> - **TypeScript (TS)** — JavaScript with types. Compiles to plain JS the browser actually runs.
> - **Vite** — modern frontend dev server and bundler. Pronounced *"veet"*.
> - **ES modules** — `import` / `export` between JavaScript files; the modern way.
> - **`tsconfig.json`** — TypeScript compiler config.
> - **HMR** — Hot Module Replacement. Edit a file, save, the browser updates without losing state.

---

## Why TypeScript

Plain JavaScript lets the typo `slot.naem` through with no warning. Three months later, a change renames `name` → `kingdomName` and you don't notice — until the field shows up empty in the UI. It's the same kind of bug C# catches when it compiles.

TypeScript adds types: `interface KingdomSlot { id: number; name: string; day: number; }`. Now the editor and the compiler both reject `slot.naem`. The same JavaScript runs underneath — TypeScript is just an extra layer you write in, and it's removed when the code is built.

You'll keep using plain JavaScript for tiny scripts. Use TypeScript as soon as a project has three or more files, or interfaces that other people need to know about.

## Why Vite

`<script src="kingdom.js">` only takes you so far. Real frontends want modules (`import { x } from './y.ts'`), need a build step to compile TypeScript, want hot-reload while editing, and want bundling for production (many small files squeezed into a few minified ones). Vite does all of that with a one-line setup and almost no config to write. It's the default modern choice.

## What changes in this module

You're moving from the plain `web/` folder (HTML and JavaScript) to a Vite project.

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

Vite sets up the project, installs about twenty small packages, and starts a dev server. Open `http://localhost:5173`. You'll see Vite's default welcome page.

`vanilla-ts` is the template name for "TypeScript, no framework." We're staying with plain TypeScript for this phase; you can add a framework on top later if you want.

## Step 2 — types for the API

Create `web-vite/src/types.ts`:

```ts
export interface KingdomSlot {
  id: number;
  name: string;
  day: number;
}
```

One place that says what a slot looks like. Everywhere in the project that handles a slot imports this interface; if you change the layout, every place that uses it gets checked.

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

A few things in that file are new and worth naming. `import './style.css'` works because Vite knows how to bundle CSS imports. `import type { KingdomSlot }` is a type-only import — it's removed when the code is built. `Promise<KingdomSlot | null>` is the function's return type; the compiler checks every place that calls it. `document.querySelector<HTMLDivElement>(...)` is the typed version of `querySelector` — you pass in the element type, so you don't have to convert the result yourself. The `!` after the call is the non-null assertion: it tells TypeScript *"trust me, this isn't null."* Only use it when you're sure. The `as KingdomSlot[]` on the parsed JSON is you telling TypeScript that the API returns the layout you declared. TypeScript can't check the real layout while the program runs, so you tell it what to expect.

## Step 4 — try it

```powershell
npm run dev
```

Visit `http://localhost:5173`. The page renders. Now edit `main.ts` — change a heading. Save. The browser updates without a full reload, and anything you'd already set up on the page stays as it was. That's HMR. Over thousands of edits, it saves you hours.

The terminal shows TypeScript errors as you save (`error TS2322: Type 'string' is not assignable to type 'number'`). Caught when the code compiles, not while it runs — the same help you've had from C# all year.

## Step 5 — the production build

```powershell
npm run build
```

This makes a `dist/` folder — minified JavaScript, hashed filenames, ready to deploy. The next step would be `npm run preview` to test the production build on your own machine, then deploy to Static Web Apps in Module 4.6.

## Tinker

Add a typo: `slot.naem`. The editor underlines it at once. TypeScript catches what JavaScript would let through.

Add a number-and-string mismatch: `const day: number = "twelve";`. Same thing — the editor flags it before you've even saved.

Try importing a CSS module: `import styles from './stuff.module.css'`. Vite handles this on its own, and the class names are kept to that one file automatically.

Set `"strict": true` in `tsconfig.json` and build again. You'll see plenty of warnings — that's your chance to clean up the code while it's still small.

## What you just did

The project moved up a level. You set up a Vite project (`web-vite/`), defined a `KingdomSlot` interface in `types.ts`, and moved the kingdom-loading code into `main.ts` as TypeScript. The compiler now rejects typos and type mismatches; HMR means edits show up in the browser without losing what's on the page. A production build is one command (`npm run build`) and makes a `dist/` folder you can deploy. The TypeScript file is about twenty lines longer than the JavaScript version it replaced — and those extra lines pay for themselves the first time a change catches a mistake at compile time.

**Key concepts you can now name:**

- **TypeScript** — JavaScript with types; compiles to plain JS
- **Vite** — modern frontend dev server and bundler
- **ES modules** — `import` and `export` between files
- **HMR** — edit, save, browser updates without losing state
- **types at every boundary** — the same DTO discipline, in the browser

## Wrap up

1. **Quiz** — open `quiz.md`, jot your answers in `journal/quiz-notes.md`.
2. **Progress** — one line in `journal/progress.md`: `Module 4.3 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit and push** — stage both files, commit message `Module 4.3 done`, Sync.
4. **Post in `#wins`** — one line about today, plus the URL of the commit.

Module 0.1 covers the why and the panel/CLI steps if you need a refresher. Bring quiz answers you're least sure about to the next weekly sync.

## Next

Module 4.4 builds the **componentised UI** — extracting reusable pieces (a `KingdomCard`, a `ResourceList`) so adding a new screen stays cheap.

# Module 4.3 — TypeScript + Vite + JS Modules

> Travou no inglês? Abra o `lesson.pt.md` — é esta mesma lição em português. Tente em inglês primeiro.

The browser code gets more serious today. Two new tools join. **TypeScript** brings the type-checking you've had in C# all year — the same `slot.day is a number` checking, written in a JavaScript file. **Vite** brings the modern tools — a dev server with hot-reload, real module support, and fast builds. The third change is quieter but it matters: `import` and `export` between files replace the old `<script src=...>` style where everything shared one global space. The web project starts to feel like a real codebase.

This is the first time you'll meet TypeScript and Vite by name. **TypeScript** is JavaScript with a type system added on top — the same kind of type-checking C# has given you all year. **Vite** (you say it *"veet"*) is a bundle of tools for web projects. Two of those tools matter today, and both have plain jobs:

- A **dev server** — a small program that runs while you work. It serves your page at `http://localhost:5173`, and every time you save a file it instantly updates the browser. Think of it as the web version of `dotnet run`, except it refreshes itself as you type. (That instant-update trick has a name — *hot-reload*, or HMR.)
- A **bundler** — when the project is ready to go live, it gathers all your separate files and the libraries they use and packs them into a few small, fast files for the browser to download. You don't run this by hand often; it's the `npm run build` step at the very end.

People used to use an older, slower tool called *webpack* for these jobs. Vite is the newer one most projects reach for now. You don't need to learn webpack — just know Vite is quietly doing those two things for you.

One thing that trips everyone up at first: the browser never actually runs TypeScript. You write the typed version; a build step strips the types away and hands the browser plain JavaScript.

```text
   you write              build step                 browser runs
   main.ts      --Vite / tsc compiles-->   main.js       (plain JavaScript;
   (with types)                            (types gone)    it never sees the types)
```

The types are there to catch *your* mistakes while you write — once the code ships, they've done their job and they're gone. That's why TypeScript can add so much safety while costing the browser nothing.

> **Words to watch**
>
> - **TypeScript (TS)** — JavaScript with types. Compiles to plain JS the browser actually runs.
> - **Vite** — modern frontend dev server and bundler. Pronounced *"veet"*.
> - **ES modules** — `import` / `export` between JavaScript files; the modern way.
> - **`tsconfig.json`** — TypeScript compiler config.
> - **HMR** — Hot Module Replacement. Edit a file, save, the browser updates without losing state.

---

## Why TypeScript

All year, C# has been quietly protecting you — and it's worth noticing now, because today you've felt what it's like *without* that protection.

Every time you wrote C#, the compiler checked your work *before the program ever ran*. Type `kingdom.Buildigs` instead of `Buildings` and you got a red squiggle and the build refused to run. Pass a `string` where a method wanted an `int` — caught. Forget an argument, rename a property and miss one spot, read a field that doesn't exist — caught, caught, caught, each time with the exact line pointed out. That whole safety net is the **type system**: because every value had a known type (`int`, `string`, `Building`), the compiler could tell when something didn't fit. You leaned on it so often it turned invisible.

Plain JavaScript has none of that. It runs whatever you typed. The typo `slot.naem` doesn't error — it hands you `undefined`, and you only find out later when the page shows a blank where the name should be. Three months on, a change renames `name` to `kingdomName` and nothing warns you until the UI quietly breaks. Every mistake the C# compiler used to catch at build time, plain JavaScript happily ships.

**TypeScript puts that safety net back.** You write the types — `interface KingdomSlot { id: number; name: string; day: number; }` — and now the editor and the compiler reject `slot.naem` and the wrong-type mistakes, exactly the way C# did all year. The same JavaScript still runs underneath; TypeScript is just the checking layer you write in, and it's removed when the code is built. It's not a new idea to learn — it's the C# habit you already have, carried over to the browser.

You'll keep using plain JavaScript for tiny one-off scripts. Reach for TypeScript the moment a project has three or more files, or types that other people — or next-month you — need to rely on.

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

First, a word on `npm`, since this is your first time using it. **npm** is JavaScript's package manager — the same job NuGet does in C#. It downloads the outside libraries a project needs (into a folder called `node_modules`) and runs a project's commands. The three lines above are exactly that: `npm create vite@latest` makes the project, `npm install` downloads its libraries, and `npm run dev` starts the dev server you just read about. You'll use these same few commands on every web project from here on.

Vite sets up the project, installs about twenty small packages, and starts the dev server. Open `http://localhost:5173`. You'll see Vite's default welcome page.

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

## On your own

Time to put the book away. Don't scroll back up to the steps. In a `.ts` file, from your own head:

1. Write and `export` an `interface` that describes one kingdom slot — an `id` that's a number, a `name` that's a string, and a `day` that's a number.
2. Make a variable of that type, and put a string where the `day` number should go.
3. Save, and watch what happens.

No one marks this — it's just for you. It's the easiest way to spot what hasn't stuck yet, while it's still simple to fix. Getting stuck here is completely fine — that's exactly what it's for. The compiler is your grader: it should underline the wrong type at once.

<details><summary>Stuck? Open this to check yourself.</summary>

```ts
export interface KingdomSlot {
  id: number;
  name: string;
  day: number;
}

const slot: KingdomSlot = { id: 1, name: "Eldoria", day: "twelve" };
// TypeScript flags this: "twelve" is a string, but day must be a number.
```

The point: each field has a type after the colon, and TypeScript checks every value against it. The last line is meant to fail — that red underline is the whole reason for adding types.

</details>

## Wrap up

1. **Quiz** — open `quiz.md`, jot your answers in `journal/quiz-notes.md`.
2. **Progress** — one line in `journal/progress.md`: `Module 4.3 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit and push** — stage both files, commit message `Module 4.3 done`, Sync.
4. **Post in `#wins`** — one line about today, plus the URL of the commit.

Module 0.1 covers the why and the panel/CLI steps if you need a refresher. Bring quiz answers you're least sure about to the next weekly sync.

## Next

Module 4.4 builds the **componentised UI** — extracting reusable pieces (a `KingdomCard`, a `ResourceList`) so adding a new screen stays cheap.

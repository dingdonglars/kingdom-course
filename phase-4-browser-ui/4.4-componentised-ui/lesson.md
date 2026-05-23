# Module 4.4 — Componentised UI

Today the page is split into reusable parts. A `KingdomCard` renders one slot. A `ResourceList` renders the resources. The main file becomes a small *orchestrator* — the file that loads the data and tells each component what to do, like a conductor in front of a band. It's the same idea as splitting a script into classes back in Module 1.1: pull out reusable pieces, and the rest of the code is easier to read.

You're not using a framework yet. These components are plain TypeScript functions: data goes in, HTML or DOM comes out. The same idea works in React (`function Component({ slot })`), Vue, Svelte, and any other framework. Frameworks add automatic re-rendering on top of this idea, but the idea itself stays the same.

> **Words to watch**
>
> - **component** — a reusable function that turns data into UI.
> - **template literal** — a backtick string that lets you interpolate values: `` `Day ${slot.day}` ``.
> - **render function** — a function that takes data and returns DOM (or an HTML string).
> - **event listener** — `el.addEventListener('click', fn)` — react to user interaction.
> - **delegation** — listen on a parent element for events bubbling from many children.
> - **XSS** — Cross-Site Scripting. The bug class where unescaped user input runs as script.

---

## Why componentise

When `main.ts` grows past a hundred lines, it gets hard to keep track of what connects to what. Components are a way to keep code in separate boxes — each one does one rendering job. `KingdomCard.ts` knows how to draw one slot. `main.ts` becomes *load data, hand it to the components, done*.

## Two component styles

There are two common ways to write a render function. Pick one for a project and use it everywhere in that project.

The first is **render-as-string** — return an HTML string and let the parent set it as `innerHTML`:

```ts
export function KingdomCard(slot: KingdomSlot): string {
  return `
    <article class="card">
      <h2>${escapeHtml(slot.name)}</h2>
      <p>Day ${slot.day}</p>
    </article>
  `;
}
```

Easy to read and write. Fine for small pages, but slower for big trees, because every render builds the whole string again and the browser has to read it again.

The second is **render-as-DOM** — build elements directly and return them:

```ts
export function KingdomCard(slot: KingdomSlot): HTMLElement {
  const card = document.createElement('article');
  card.className = 'card';

  const h2 = document.createElement('h2');
  h2.textContent = slot.name;
  card.appendChild(h2);

  const p = document.createElement('p');
  p.textContent = `Day ${slot.day}`;
  card.appendChild(p);

  return card;
}
```

More to write. Faster for big trees, and `textContent` escapes user input on its own — so you can't cause an XSS bug by mistake.

For the kingdom UI, render-as-string with an `escapeHtml` helper is plenty. That's what we'll use.

## The XSS trap

> ⚠ **`innerHTML` plus user input is a security bug.** This kind of bug is called **XSS** — Cross-Site Scripting. If a kingdom name is `<script>alert(1)</script>` and you put it straight into HTML, that script runs in the browser of everyone who views the page. Either use `textContent` and `appendChild` (the DOM-mode option), or escape every string you drop into the HTML. Never put raw user data into `innerHTML`.

## What changes in this module

- **NEW:** `web-vite/src/components/KingdomCard.ts`
- **NEW:** `web-vite/src/components/escape.ts` — the HTML escape helper
- **MODIFIED:** `web-vite/src/main.ts` — uses the components

`web-vite/src/components/escape.ts`:

```ts
export function escapeHtml(s: string): string {
  return s
      .replace(/&/g, '&amp;')
      .replace(/</g, '&lt;')
      .replace(/>/g, '&gt;')
      .replace(/"/g, '&quot;')
      .replace(/'/g, '&#039;');
}
```

`web-vite/src/components/KingdomCard.ts`:

```ts
import type { KingdomSlot } from '../types';
import { escapeHtml } from './escape';

export function KingdomCard(slot: KingdomSlot): string {
  return `
    <article class="card">
      <h2>${escapeHtml(slot.name)}</h2>
      <p>Day ${slot.day}</p>
    </article>
  `;
}
```

## `main.ts` orchestrates

```ts
import './style.css';
import type { KingdomSlot } from './types';
import { KingdomCard } from './components/KingdomCard';

const API = 'https://localhost:5xxx';

async function main() {
  const root = document.querySelector<HTMLDivElement>('#app')!;
  try {
    const resp = await fetch(`${API}/kingdoms`);
    if (!resp.ok) throw new Error(`HTTP ${resp.status}`);
    const slots = (await resp.json()) as KingdomSlot[];

    if (slots.length === 0) {
      root.innerHTML = '<p>No kingdoms yet.</p>';
      return;
    }
    root.innerHTML = slots.map(KingdomCard).join('');
  } catch (err) {
    root.textContent = `Error: ${(err as Error).message}`;
  }
}

main();
```

`slots.map(KingdomCard)` — the component is just a function, so `map` over the slots and you get an array of HTML strings. `.join('')` joins them into one string. One line of orchestration per component.

## Event delegation

When you have many cards, adding a click handler to each one wastes effort. Instead, listen on the parent and check which child was clicked:

```ts
root.addEventListener('click', (e) => {
  const card = (e.target as HTMLElement).closest('article.card');
  if (!card) return;
  console.log('Clicked card:', card);
});
```

`closest` moves up from the clicked element until it finds a parent that matches. This pattern works for any UI with "many similar items" — one handler can cover thousands of cards.

## Tinker

Add a `ResourceList` component that takes a `Map<string, number>` and renders a `<ul>`. Use it in `main`.

Add a Tick button to each card. On click, POST to `/kingdoms/{id}/tick` and render again.

Add CSS to `.card` — a border, padding, a small drop-shadow. The same render function now looks nicer right away; that's the point of having a render function at all.

Try leaving out `escapeHtml` once. Add a kingdom with the name `<script>alert(1)</script>` through the API. The script runs. That's a real XSS bug. Put the escape back; the alert disappears.

## What you just did

You split the page into components. `KingdomCard` is a function that turns a `KingdomSlot` into HTML; `escapeHtml` keeps user input safe inside `innerHTML`; `main.ts` just loads data and calls `slots.map(KingdomCard).join('')`. You also met **XSS** — the kind of bug where unescaped strings run as script — and the `escapeHtml` helper that prevents it. About forty lines of TypeScript across three files. The same pattern works in React, Vue, or any framework you'll meet later.

**Key concepts you can now name:**

- **component** — a reusable function from data to UI
- **`escapeHtml`** — make user input safe for `innerHTML` interpolation
- **XSS** — Cross-Site Scripting; an attack through unescaped strings
- **event delegation** — listen on a parent; dispatch by `e.target`
- **render function** — `(data) => HTML`; the idea every framework copies

## Wrap up

1. **Quiz** — open `quiz.md`, jot your answers in `journal/quiz-notes.md`.
2. **Progress** — one line in `journal/progress.md`: `Module 4.4 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit and push** — stage both files, commit message `Module 4.4 done`, Sync.
4. **Post in `#wins`** — one line about today, plus the URL of the commit.

Module 0.1 covers the why and the panel/CLI steps if you need a refresher. Bring quiz answers you're least sure about to the next weekly sync.

## Next

Module 4.5 introduces **Vitest** — the test runner for browser code. Same xUnit instinct, JavaScript-flavored. It catches the bugs your TypeScript types can't.

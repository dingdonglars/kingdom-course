# Module 4.4 — Componentised UI

The page splits into reusable parts today. A `KingdomCard` renders one slot. A `ResourceList` renders the resources. The main file becomes a small *orchestrator* — the file that loads the data and tells each component what to do, like a conductor in front of a band. The same idea as splitting a script into classes back in M1.1 — extract reusable units and the rest of the code reads cleaner.

You're not using a framework yet. These components are plain TypeScript functions: data goes in, HTML or DOM comes out. The mental model transfers cleanly to React (`function Component({ slot })`), Vue, Svelte, anything. Frameworks add change-detection on top of this idea; the idea itself is the same.

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

When `main.ts` grows past a hundred lines, you start to lose track of what's wired to what. Components are folders for code — each one owns one rendering job. `KingdomCard.ts` knows how to draw one slot. `main.ts` becomes *load data, dispatch to components, done*.

## Two component styles

There are two common ways to write a render function. Pick one per project and stick with it.

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

Simple to read and write. Fine at small scale; slower for big trees because every render rebuilds and the browser re-parses.

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

More verbose. Faster for big trees, and `textContent` automatically escapes user input — so no XSS risk by accident.

For the kingdom UI, render-as-string with an `escapeHtml` helper is plenty. We'll use that.

## The XSS trap

> ⚠ **`innerHTML` plus user input is a security bug.** The class is called **XSS** — Cross-Site Scripting. If a kingdom name is `<script>alert(1)</script>` and you paste it raw into HTML, the script runs in every viewer's browser. Either use `textContent` and `appendChild` (the DOM-mode option), or escape every interpolated string. Never paste raw user data into `innerHTML`.

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

`slots.map(KingdomCard)` — the component is just a function, so `map` over the slots and you get an array of HTML strings. `.join('')` glues them into one string. One line of orchestration per component.

## Event delegation

When you have many cards, attaching a click handler to each is wasteful. Listen on the parent and check the target instead:

```ts
root.addEventListener('click', (e) => {
  const card = (e.target as HTMLElement).closest('article.card');
  if (!card) return;
  console.log('Clicked card:', card);
});
```

`closest` walks up from the click target until it finds a matching ancestor. The pattern works for any "many similar items" UI — one handler scales to thousands of cards.

## Tinker

Add a `ResourceList` component that takes a `Map<string, number>` and renders a `<ul>`. Use it in `main`.

Add a Tick button per card; on click, POST to `/kingdoms/{id}/tick` and re-render.

Add CSS to `.card` — a border, padding, a small drop-shadow. The same render function gets nicer-looking instantly; that's the win of having a render function in the first place.

Try forgetting `escapeHtml` once. Insert a kingdom with the name `<script>alert(1)</script>` via the API. The script runs. That's XSS in the wild. Restore the escape; the alert disappears.

## What you just did

You split the page into components. `KingdomCard` is a function from `KingdomSlot` to HTML; `escapeHtml` keeps user input safe inside `innerHTML`; `main.ts` just loads data and calls `slots.map(KingdomCard).join('')`. You also met **XSS** — the bug class where unescaped strings run as script — and the `escapeHtml` helper that prevents it. About forty lines of TypeScript across three files; the pattern carries straight through to React, Vue, or any framework you'll meet later.

**Key concepts you can now name:**

- **component** — a reusable function from data to UI
- **`escapeHtml`** — make user input safe for `innerHTML` interpolation
- **XSS** — Cross-Site Scripting; injection via unescaped strings
- **event delegation** — listen on a parent; dispatch by `e.target`
- **render function** — `(data) => HTML`; the idea every framework copies

## Quiz

Open `quiz.md`. When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.

## Next

Module 4.5 introduces **Vitest** — the test runner for browser code. Same xUnit instinct, JavaScript-flavored. It catches the bugs your TypeScript types can't.

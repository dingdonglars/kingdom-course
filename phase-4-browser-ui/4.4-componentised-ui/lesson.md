# Module 4.4 — Componentised UI

> **Hook:** today the page splits into reusable parts. A `KingdomCard` renders one slot. `ResourceList` renders the resources. The main file becomes a small orchestrator. **Same idea as classes vs script in Block 1.1 — extract reusable units, the rest of the code reads cleaner.**

> **Words to watch**
> - **component** — a reusable unit that renders a piece of UI from data
> - **template literal** — backticks `` `...${x}...` `` — JS's string interpolation
> - **render function** — a function that takes data + returns DOM (or an HTML string)
> - **event listener** — `el.addEventListener('click', fn)` — react to user interaction
> - **delegation** — listen on a parent for events bubbling from many children

---

## Why componentise

When `main.ts` grows past 100 lines, you start to lose track of what's wired to what. **Components are folders for code.** A `KingdomCard.ts` file owns one specific rendering job. `main.ts` becomes "load data, dispatch to components."

You're not using a framework yet — these are just plain TS functions. The mental model transfers cleanly to React (`function Component({ slot })`), Vue, Svelte, anything later.

## Two component shapes

**Render-as-string** (simpler, slower for big trees):

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

**Render-as-DOM** (faster, more flexible):

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

Pick one style and stick with it per project. **Render-as-string + `escapeHtml` is fine for our scale**; render-as-DOM scales better.

> ⚠ **`innerHTML` + user input is a security bug** (XSS — Cross-Site Scripting). Either use `textContent`/`appendChild` (DOM-mode), or escape every interpolated string. **Never paste raw user data into `innerHTML`.**

## Delta starter

- **NEW:** `web-vite/src/components/KingdomCard.ts`
- **NEW:** `web-vite/src/components/escape.ts` (the HTML escape helper)
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

`slots.map(KingdomCard)` — the component is just a function. `.join('')` concatenates the strings. **One line of orchestration per component.**

## Event delegation (light intro)

When you have 100 cards, attaching a click handler to each is wasteful. Listen on the parent + check the target:

```ts
root.addEventListener('click', (e) => {
  const card = (e.target as HTMLElement).closest('article.card');
  if (!card) return;
  console.log('Clicked card:', card);
});
```

`closest` walks up from the target until it finds a matching ancestor. Pattern works for any "many similar items" UI.

## Tinker

- Add a `ResourceList` component that takes a `Map<string, number>` and renders an `<ul>`. Use it in main.
- Add a Tick button per card; on click, POST to `/kingdoms/{id}/tick` and re-render.
- Add CSS to `.card` — border, padding, shadow. **The same render function gets nicer-looking instantly.**
- Try forgetting `escapeHtml`. Insert a kingdom with name `<script>alert(1)</script>` via the API. **The script runs.** That's XSS. Restore the escape.

## Name it

- **Component** — a reusable function that turns data into UI.
- **`escapeHtml`** — make user input safe for `innerHTML` interpolation.
- **XSS** — Cross-Site Scripting; injecting JS via unescaped strings.
- **Event delegation** — listen on a parent, dispatch by `e.target`. Scales to many children.
- **Render function** — `(data) => HTML` (or DOM). The shape every UI framework copies.

## The rule of the through-line

> **Components for the same reason classes are useful: reusable units with one job.** Every framework you'll ever use is built on this idea. Vanilla functions teach the shape; frameworks just add change-detection on top.

## Quiz / challenge

Open `quiz.md`.

## Connect

Module 4.5 introduces **Vitest** — the test runner for browser code. Same xUnit instinct, JS-flavored. Catches the bugs your TypeScript types can't.
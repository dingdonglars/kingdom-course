# Module 4.5 — Vitest (Frontend Testing)

The browser code gets unit tests today. **Vitest** is xUnit's JavaScript-flavoured cousin — fast, Vite-aware, the same `expect(x).toBe(y)` discipline you've been using all year. We'll test the components from Module 4.4: given a `KingdomSlot`, the `KingdomCard` renders the expected HTML; given a string with angle brackets, `escapeHtml` returns escaped output.

> **Words to watch**
>
> - **Vitest** — the test runner. Vite-aware; the API is nearly drop-in compatible with Jest.
> - **`describe` / `it` / `expect`** — the standard test vocabulary.
> - **`toBe` / `toEqual` / `toContain`** — common assertions, Jest-style.
> - **happy-dom** — a fake DOM environment for tests that need `document` without a real browser.

---

## Why a frontend test runner

C# tests gave you confidence to refactor. Frontend tests give the same. Without them, `KingdomCard`'s rendering can quietly drift from what `main.ts` expects. With them, every change to a component runs against its tests; regressions surface in seconds.

Frontend tests fall into three rough buckets. *Unit tests* exercise one function or component (Vitest alone). *Integration tests* run several components together against a fake DOM (Vitest plus `happy-dom`). *End-to-end tests* drive a real browser (Playwright — its own world, not in scope here). We'll do unit and light integration today.

## What changes in this module

- **MODIFIED:** `web-vite/package.json` — adds `vitest` + `happy-dom`
- **MODIFIED:** `web-vite/vite.config.ts` — wires Vitest config
- **NEW:** `web-vite/src/components/__tests__/escape.test.ts`
- **NEW:** `web-vite/src/components/__tests__/KingdomCard.test.ts`

## Step 1 — install

```powershell
cd web-vite
npm install -D vitest happy-dom
```

## Step 2 — Vitest config

`web-vite/vite.config.ts` (create or modify):

```ts
import { defineConfig } from 'vitest/config';

export default defineConfig({
  test: {
    environment: 'happy-dom',
    globals: true
  }
});
```

`globals: true` lets `describe`, `it`, and `expect` be used without imports — same xUnit feel.

## Step 3 — first tests

`src/components/__tests__/escape.test.ts`:

```ts
import { describe, it, expect } from 'vitest';
import { escapeHtml } from '../escape';

describe('escapeHtml', () => {
  it('escapes the five characters', () => {
    expect(escapeHtml('<script>')).toBe('&lt;script&gt;');
    expect(escapeHtml('"')).toBe('&quot;');
    expect(escapeHtml("'")).toBe('&#039;');
    expect(escapeHtml('A & B')).toBe('A &amp; B');
  });

  it('leaves safe strings alone', () => {
    expect(escapeHtml('hello world')).toBe('hello world');
  });
});
```

`src/components/__tests__/KingdomCard.test.ts`:

```ts
import { describe, it, expect } from 'vitest';
import { KingdomCard } from '../KingdomCard';

describe('KingdomCard', () => {
  it('renders the kingdom name and day', () => {
    const html = KingdomCard({ id: 1, name: 'Eldoria', day: 11 });
    expect(html).toContain('Eldoria');
    expect(html).toContain('Day 11');
  });

  it('escapes the name', () => {
    const html = KingdomCard({ id: 1, name: '<script>x</script>', day: 1 });
    expect(html).toContain('&lt;script&gt;');
    expect(html).not.toContain('<script>');     // the literal tag must NOT appear
  });
});
```

The second test is the security test — proof that `escapeHtml` is wired up correctly.

## Step 4 — run

```powershell
npm test
```

Vitest finds `*.test.ts` files and runs each `describe`/`it`. You'll see green ticks (or red exes) instantly. For watch mode, `npm test -- --watch` re-runs only the affected tests on file change. That tight feedback loop is the rhythm of modern frontend work.

## Step 5 — light DOM integration

To test components that touch the DOM, the `happy-dom` environment provides a fake `document`:

```ts
import { describe, it, expect, beforeEach } from 'vitest';

describe('main rendering', () => {
  beforeEach(() => {
    document.body.innerHTML = '<div id="app"></div>';
  });

  it('renders the empty state', () => {
    const root = document.querySelector('#app')!;
    root.innerHTML = '<p>No kingdoms yet.</p>';
    expect(root.textContent).toContain('No kingdoms yet');
  });
});
```

`document` exists in tests because `happy-dom` provides one — about ten times faster than spinning up real Chrome. For full-browser testing, Playwright is the tool; explore it on your own time if you want.

## Tinker

Add coverage reporting: `npm test -- --coverage`. See which lines aren't covered by tests. Don't aim for 100% — aim for *the parts that would silently break if changed*. Coverage chasing produces bad tests.

Mock `fetch` with `vi.fn()` — Vitest's spy and mock helper. Test main's error handling without making a real network call.

Add a snapshot test: `expect(KingdomCard(slot)).toMatchSnapshot()`. Vitest writes the expected output to a `__snapshots__/` file the first time, and flags any change after that.

## What you just did

The browser code now has tests. You wired Vitest into the Vite project, set the test environment to `happy-dom`, and wrote four tests across two files: `escapeHtml` covers the five characters and the no-op case, and `KingdomCard` covers both the happy path and the security path (proving the escape is wired through). About sixty lines of test code; one command (`npm test`) runs them all in under a second. The same testing instinct you've been building since Module 1.3 — *if it can break, write a test* — now reaches into the browser.

**Key concepts you can now name:**

- **Vitest** — Vite-native test runner; xUnit cousin in JavaScript
- **`describe` / `it` / `expect`** — the standard test vocabulary
- **happy-dom** — fast fake DOM for tests
- **watch mode** — re-runs affected tests on save
- **snapshot test** — capture output once; fail on any future change

## Quiz

Open `quiz.md`. When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.

## Next

Module 4.6 is the last technical module of Phase 4: deploy the frontend to Azure Static Web Apps with GitHub Actions. Then Module 4.7 closes Phase 4 with M5 and a quiet exercise where you re-read your Phase 0 code.

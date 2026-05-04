# Module 4.5 — Vitest (Frontend Testing)

> **Hook:** today the browser code gets unit tests. **Vitest** is xUnit's JS-flavored cousin — fast, Vite-native, the same `expect(x).toBe(y)` discipline. We test the components from M4.4: given a `KingdomSlot`, the `KingdomCard` renders the expected HTML.

> **Words to watch**
> - **Vitest** — the test runner; Vite-aware, ~drop-in compatible with Jest
> - **`describe` / `it` / `expect`** — the standard test vocabulary
> - **`toBe` / `toEqual` / `toContain`** — Jest-style assertions
> - **happy-dom** / **jsdom** — fake DOM environment for tests that need `document`

---

## Why a frontend test runner

C# tests gave you confidence to refactor. **JS tests give the same.** Without them, `KingdomCard`'s rendering can quietly diverge from what `main.ts` expects. With them, every change to a component runs against its tests; regressions surface in seconds.

Frontend tests fall into three rough buckets:

| Type | What | Tool |
|---|---|---|
| Unit | One function/component | Vitest |
| Integration | Multiple components together | Vitest + DOM env |
| E2E | A real browser driving the whole app | Playwright (out of scope) |

We'll do unit + light DOM integration with Vitest. E2E is its own block.

## Delta starter

- **MODIFIED:** `web-vite/package.json` — add `vitest` + `happy-dom`
- **MODIFIED:** `web-vite/vite.config.ts` — wire vitest config
- **NEW:** `web-vite/src/components/__tests__/escape.test.ts`
- **NEW:** `web-vite/src/components/__tests__/KingdomCard.test.ts`

## Step 0 — install

```powershell
cd web-vite
npm install -D vitest happy-dom
```

## Step 1 — vite config

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

`globals: true` lets `describe`, `it`, `expect` be used without `import` — same xUnit-feel.

## Step 2 — first tests

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

The second test is the security test — the one that proves `escapeHtml` is wired up correctly.

## Step 3 — run

```powershell
npm test
```

Vitest finds `*.test.ts` files, runs each `describe`/`it`. You'll see green ticks (or red exes) instantly. **Watch mode:** `npm test -- --watch` re-runs only the affected tests on file change.

## Step 4 — light DOM integration

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

`document` exists in tests because `happy-dom` provides one. **Faster than spinning up real Chrome.** For full-browser testing, Playwright is the tool — covered in your own time if you want.

## Tinker

- Add `coverage` reporting: `npm test -- --coverage`. See which lines aren't covered by tests. **Don't aim for 100%** — aim for "the parts that would silently break if changed."
- Mock `fetch` with `vi.fn()` — Vitest's spy/mock helper. Test main's error handling without a real network.
- Add a snapshot test: `expect(KingdomCard(slot)).toMatchSnapshot()`. Vitest writes the expected output to a `__snapshots__/` file the first time; flags any change after.

## Name it

- **Vitest** — the test runner. Vite-native; Jest-compatible API.
- **`describe` / `it` / `expect`** — the standard vocabulary.
- **`happy-dom`** — fast fake DOM for tests.
- **Watch mode** — re-runs affected tests on save.
- **Snapshot test** — capture output once; fail on any future change.

## The rule of the through-line

> **Same testing discipline, every layer.** Engine had xUnit; persistence had xUnit; API had xUnit + WebApplicationFactory; browser has Vitest + happy-dom. Different runtimes; same instinct: *if it can break, write a test.*

## Quiz / challenge

Open `quiz.md`.

## Connect

Module 4.6 is the last technical module of Block 6: **deploy the frontend to Azure Static Web Apps + GitHub Actions**. Then M4.7 closes Block 6 with M5.
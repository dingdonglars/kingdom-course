# Module 4.5 — Vitest (Frontend Testing)

> Travou no inglês? Abra o `lesson.pt.md` — é esta mesma lição em português. Tente em inglês primeiro.

The browser code gets unit tests today. **Vitest** is the JavaScript version of xUnit — fast, made to work with Vite, and using the same `expect(x).toBe(y)` style you've been using all year. We'll test the components from Module 4.4: given a `KingdomSlot`, `KingdomCard` should render the HTML we expect; given a string with angle brackets, `escapeHtml` should return escaped output.

> **Words to watch**
>
> - **Vitest** — the test runner. Made to work with Vite; its API is almost the same as Jest's.
> - **`describe` / `it` / `expect`** — the standard test vocabulary.
> - **`toBe` / `toEqual` / `toContain`** — common assertions, Jest-style.
> - **happy-dom** — a fake DOM environment for tests that need `document` without a real browser.

---

## Why a frontend test runner

C# tests gave you the confidence to change code. Frontend tests give you the same confidence. Without them, what `KingdomCard` renders can slowly stop matching what `main.ts` expects, and you won't notice. With them, every change to a component runs against its tests, and a broken one shows up in seconds.

Frontend tests come in three rough groups. *Unit tests* check one function or component (Vitest on its own). *Integration tests* run several components together against a fake DOM (Vitest plus `happy-dom`). *End-to-end tests* run a real browser (Playwright — a big topic on its own, not covered here). Today we'll do unit tests and a little integration testing.

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

`globals: true` lets you use `describe`, `it`, and `expect` without importing them — the same feel as xUnit.

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

The second test is the security test — it proves that `escapeHtml` is connected correctly.

## Step 4 — run

```powershell
npm test
```

Vitest finds `*.test.ts` files and runs each `describe`/`it`. You'll see green ticks (or red crosses) right away. For watch mode, `npm test -- --watch` re-runs only the tests affected by a file change. That fast feedback is a big part of how modern frontend work feels.

## Step 5 — light DOM integration

To test components that use the DOM, the `happy-dom` environment gives you a fake `document`:

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

`document` works in tests because `happy-dom` gives you one — about ten times faster than starting up real Chrome. For full-browser testing, Playwright is the tool; look into it on your own if you want.

## Tinker

Add coverage reporting: `npm test -- --coverage`. See which lines aren't covered by tests. Don't aim for 100% — aim to cover *the parts that would break without warning if you changed them*. Chasing a high coverage number leads to bad tests.

Mock `fetch` with `vi.fn()` — Vitest's spy and mock helper. Test main's error handling without making a real network call.

Add a snapshot test: `expect(KingdomCard(slot)).toMatchSnapshot()`. Vitest writes the expected output to a `__snapshots__/` file the first time, then flags any change after that.

## What you just did

The browser code now has tests. You connected Vitest to the Vite project, set the test environment to `happy-dom`, and wrote four tests across two files: `escapeHtml` covers the five characters and the do-nothing case, and `KingdomCard` covers both the normal case and the security case (proving the escape is connected). About sixty lines of test code; one command (`npm test`) runs them all in under a second. The same testing habit you've been building since Module 1.3 — *if it can break, write a test* — now works in the browser too.

**Key concepts you can now name:**

- **Vitest** — test runner built for Vite; the JavaScript version of xUnit
- **`describe` / `it` / `expect`** — the standard test vocabulary
- **happy-dom** — fast fake DOM for tests
- **watch mode** — re-runs affected tests on save
- **snapshot test** — capture output once; fail on any future change

## On your own

Time to put the book away. Don't scroll back up to the steps. No one marks this — it's just for you. It's the easiest way to spot what hasn't stuck yet, while it's still simple to fix. Getting stuck here is completely fine — that's exactly what it's for. The test runner is your grader: a green tick means you got the shape right.

In a `.test.ts` file, from your own head, write one Vitest test for `escapeHtml`:

1. Use a `describe`, an `it`, and an `expect(...).toBe(...)`.
2. Check that `escapeHtml('<script>')` turns the angle brackets into `&lt;script&gt;`.
3. Run `npm test`.

<details><summary>Stuck? Open this to check yourself.</summary>

```ts
import { describe, it, expect } from 'vitest';
import { escapeHtml } from '../escape';

describe('escapeHtml', () => {
  it('escapes angle brackets', () => {
    expect(escapeHtml('<script>')).toBe('&lt;script&gt;');
  });
});
```

The shape to remember: `describe` names the thing under test, `it` names one behaviour, and `expect(actual).toBe(expected)` makes the check. It is the same `expect(x).toBe(y)` instinct you have had since your C# tests.

</details>

## Wrap up

1. **Quiz** — open `quiz.md`, jot your answers in `journal/quiz-notes.md`.
2. **Progress** — one line in `journal/progress.md`: `Module 4.5 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit and push** — stage both files, commit message `Module 4.5 done`, Sync.
4. **Post in `#wins`** — one line about today, plus the URL of the commit.

Module 0.1 covers the why and the panel/CLI steps if you need a refresher. Bring quiz answers you're least sure about to the next weekly sync.

## Next

Module 4.6 is the last technical module of Phase 4: deploy the frontend to Azure Static Web Apps with GitHub Actions. Then Module 4.7 closes Phase 4 with M5 and a quiet exercise where you re-read your Phase 0 code.

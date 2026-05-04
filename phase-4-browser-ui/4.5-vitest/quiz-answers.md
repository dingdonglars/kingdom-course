# Quiz answers — Module 4.5

## 1. b
Vitest is JS's xUnit. Same vocabulary (`describe`/`it`/`expect`), same instinct (assert behaviour, run on save, fail loud). The patterns transfer.

## 2. b
`happy-dom` (and the older `jsdom`) provide a fake `document` and `window` so tests can call `document.querySelector(...)` etc. without launching a real browser. ~10x faster than real Chrome for unit/integration tests.

## 3. a
Two assertions catch different bugs. `toContain('&lt;script&gt;')` proves escaping happened. `not.toContain('<script>')` proves the literal tag isn't anywhere — catches the case where escape produced *both* the escaped form *and* the original (unlikely but possible). Both = airtight.

## 4. a
Watch mode is the rapid feedback loop. Edit a file, save, only its tests rerun, you see green or red in <1 second. Multiplied across a refactor session, this is hours. Always-on watch mode is the rhythm of modern frontend work.

## 5. a
Coverage as a goal produces bad tests. People game it: tests of getters, mocks of mocks, integration tests that don't really integrate. Better: cover the parts where a silent change would hurt — security-sensitive code (escaping), critical paths (auth checks), shapes that other code depends on (DTOs).
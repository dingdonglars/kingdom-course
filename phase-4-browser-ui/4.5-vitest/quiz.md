# Quiz — Module 4.5

> Don't write your answers in this file — open `journal/quiz-notes.md` and write them there.
>
> Travou no inglês? Abra o `quiz.pt.md` — é este mesmo quiz em português. Tente em inglês primeiro.

## 1. Vitest is to JavaScript as ___ is to C#.

- **a.** EF Core — the data-access layer used in your persistence project
- **b.** xUnit — the test runner you've been using since Module 1.3
- **c.** ASP.NET Core — the web framework you're using on the API
- **d.** NuGet — the package manager that pulls in third-party libraries

## 2. What does `happy-dom` give you?

- **a.** A real browser running headless on your developer machine for tests
- **b.** A fast fake DOM (`document`, `window`) for tests that need to manipulate elements
- **c.** A logging library used by Vitest internally to track failed assertions
- **d.** A type-checker that runs alongside Vitest during the test suite

## 3. The XSS test asserts both `toContain('&lt;script&gt;')` AND `not.toContain('<script>')`. Why both?

- **a.** Belt and suspenders — the first proves escaping happened; the second proves the literal tag isn't anywhere in the output
- **b.** Required by Vitest's assertion API for any string that contains HTML markup
- **c.** Performance reasons — single assertions run more slowly in modern test suites
- **d.** Tradition from the early Jest era of frontend testing patterns

## 4. What's watch mode?

- **a.** `npm test -- --watch` — re-runs only the affected tests on file change; tight feedback loop while editing
- **b.** A manual run of the entire test suite after each save you make
- **c.** A continuous integration mode that runs tests on every push to GitHub
- **d.** A required setting in `package.json` for tests to find their files

## 5. The lesson says aim for "the parts that would silently break if changed" not 100% coverage. Why?

- **a.** Coverage chasing produces bad tests — testing trivial getters, mocking everything; covering the parts that matter is what pays back
- **b.** 100% coverage is technically impossible to achieve in any modern frontend codebase
- **c.** Higher coverage runs measurably more slowly during development
- **d.** Vitest's coverage tool refuses to count the trivial cases anyway

---

When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.

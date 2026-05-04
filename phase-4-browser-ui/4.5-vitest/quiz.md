# Quiz — Module 4.5

## 1. Vitest is to JavaScript as ___ is to C#.

a. EF Core
b. xUnit
c. ASP.NET Core
d. NuGet

## 2. What does `happy-dom` give you?

a. A real browser
b. A fast fake DOM (`document`, `window`) for tests that need to manipulate elements without spinning up a real browser
c. A logging library
d. A type checker

## 3. The XSS test asserts both `toContain('&lt;script&gt;')` AND `not.toContain('<script>')`. Why both?

a. Belt + suspenders — the first proves escaping happened, the second proves the literal tag isn't anywhere in the output. Together they're solid; either alone misses cases.
b. Required by Vitest
c. Performance
d. Tradition

## 4. What's the watch-mode test loop?

a. `npm test -- --watch` — re-runs only the affected tests on file change. Tight feedback loop while writing or refactoring.
b. Manual run after each save
c. CI runs all tests
d. Required by GitHub

## 5. The lesson says aim for "the parts that would silently break if changed" not 100% coverage. Why?

a. Coverage chasing produces bad tests (testing trivial getters, mocking everything). Test the behaviours that matter; let the rest be.
b. 100% is impossible
c. Performance
d. Required by Vitest
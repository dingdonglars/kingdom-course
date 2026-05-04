# Quiz — Module 1.3

> Don't write your answers in this file — open `journal/quiz-notes.md` and write them there.

## 1. What's the difference between `[Fact]` and `[Theory]`?

- **a.** They're the same; the keyword you use is just style preference
- **b.** `[Fact]` is one test; `[Theory]` runs the same logic with different inputs from `[InlineData]`
- **c.** `[Fact]` is for true things; `[Theory]` is for things you suspect might be true
- **d.** `[Theory]` is older syntax that no longer runs in modern xUnit

## 2. What does `b.Level.ShouldBe(2)` do if `b.Level` is `5`?

- **a.** Sets `b.Level` to `2` so the test can continue
- **b.** Returns `false` so the next assertion can decide what to do
- **c.** Throws an assertion exception, and the test runner reports a failure
- **d.** Logs a warning and the test still passes

## 3. What's the conventional structure of a unit test?

- **a.** Setup / Teardown / Assert
- **b.** Arrange / Act / Assert
- **c.** Build / Test / Deploy
- **d.** Try / Catch / Finally

## 4. Why test the engine and not the console?

- **a.** The console is too small to bother testing
- **b.** Engine code holds the rules; testing it once verifies them across every shell that comes later
- **c.** The console can't be tested — there's no way to capture its output
- **d.** It's a stylistic choice with no real reasoning behind it

## 5. What does the test name `Spend_WhenInsufficient_ReturnsFalse` tell you?

- **a.** Nothing — the name is just for humans
- **b.** Method (`Spend`), scenario (insufficient funds), expected behaviour (returns false). The convention from `STANDARDS.md`.
- **c.** That the test will spend something during the run
- **d.** That someone named "Spend" wrote the test

---

When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.

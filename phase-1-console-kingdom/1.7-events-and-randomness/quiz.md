# Quiz — Module 1.7

> Don't write your answers in this file — open `journal/quiz-notes.md` and write them there.
>
> Travou no inglês? Abra o `quiz.pt.md` — é este mesmo quiz em português. Tente em inglês primeiro.

## 1. What's a `record` in C#?

- **a.** A logging type used to write entries to a file
- **b.** A small immutable data class — equality compares fields, ToString prints them, nice for events
- **c.** A keyword reserved for database row types only
- **d.** A type of comment used to document classes

## 2. Why is `EventEngine` newing up `Random` directly *bad*?

- **a.** `Random` is slow at runtime; the engine ticks too often to use it
- **b.** The engine is now non-deterministic and untestable — you can't say "given dice X, expect event Y"
- **c.** `Random` is deprecated and the compiler emits warnings about it
- **d.** It uses too much memory across long-running games

## 3. What does the `_` in `_ => null` (the switch) mean?

- **a.** A discard variable that signals you don't care about the value
- **b.** The "anything else" case — matches whatever didn't match above
- **c.** Both of the above; the underscore plays both roles
- **d.** A typo that should have been `default`

## 4. What does `1 when k.Citizens.Count > 0` mean inside a switch?

- **a.** Match the pattern `1`, but only if `k.Citizens.Count > 0`. Otherwise the next pattern is checked.
- **b.** Always match `1`; the `when` clause is a comment for humans
- **c.** A C# 12-only feature that's been removed in newer versions
- **d.** A bug — `when` is not allowed inside switch expressions

## 5. Why are the tests in `EventLogTests.cs` so vague (*"some events happen"*)?

- **a.** The author was rushed and skipped the precise tests
- **b.** The engine is non-deterministic — no way to make precise assertions, only loose ones. Module 1.8 introduces `IRandom` to fix it.
- **c.** xUnit doesn't support assertions tighter than that
- **d.** Shouldly only handles approximate matches; precise checks need a different library

---

When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.

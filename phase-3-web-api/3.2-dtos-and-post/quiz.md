# Quiz — Module 3.2

> Don't write your answers in this file — open `journal/quiz-notes.md` and write them there.

## 1. Why use a request/response DTO instead of returning the engine `Kingdom` directly?

- **a.** JSON serialisation can't always handle classes with constructors that need an `IRandom`; and the wire layout should stay stable, small, and explicit
- **b.** It's purely a performance optimisation for large objects
- **c.** It's an old habit kept around for tradition's sake
- **d.** ASP.NET refuses to serialise non-record types

## 2. What does `Math.Clamp(days ?? 1, 1, 100)` actually do?

- **a.** Throws an exception if `days` is null
- **b.** Defaults `days` to 1 if null, then forces the value into the range 1 to 100 — basic input validation in one line
- **c.** Multiplies `days` by 1 and returns the result
- **d.** Sorts the input value into a list

## 3. Why use `Results.Ok(value)` instead of just returning `value`?

- **a.** They are exactly identical in every way
- **b.** `Results.Ok(value)` lets you control the status code — useful when one handler returns different codes on different conditions (200 here, 404 elsewhere)
- **c.** It's required by .NET 10 for any handler that returns a record
- **d.** It compresses the JSON before sending it back to the client

## 4. The minimal-API parameter `(int? days)` reads from where in the request?

- **a.** From the request body as JSON
- **b.** From the query string — `?days=5` — because a primitive optional parameter binds to the URL
- **c.** From a custom HTTP header named `X-Days`
- **d.** From a cookie set by a previous request

## 5. The lesson says *validate at the boundary*. What's the reasoning?

- **a.** The outer layer is where untrusted input enters. Reject bad input there so the engine never has to defend itself.
- **b.** Boundary validation runs faster than engine validation
- **c.** The HTTP spec mandates validation at the boundary
- **d.** It's a stylistic preference with no real impact

---

When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.

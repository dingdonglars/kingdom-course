# Quiz — Module 3.2

## 1. Why use a request/response DTO instead of returning the engine `Kingdom` directly?

a. JSON serialisation can't handle classes with constructors that require `IRandom` cleanly; and the wire shape should be stable, small, explicit
b. Performance only
c. Tradition
d. Required by ASP.NET

## 2. What does `Math.Clamp(days ?? 1, 1, 100)` do?

a. Throws if `days` is null
b. Defaults `days` to 1 if null, then forces the value into the range [1..100] — basic input validation in one line
c. Multiplies by 1
d. Sorts the input

## 3. Why use `Results.Ok(value)` instead of just returning `value`?

a. They're identical
b. `Results.Ok(value)` lets you control the status code (e.g., return `Results.NotFound()` instead). Useful when one handler returns different status codes on different conditions.
c. Required by .NET
d. To impress reviewers

## 4. The minimal-API parameter `(int? days)` reads from where in the request?

a. The request body
b. The query string (`?days=5`); a primitive optional parameter binds to the URL
c. A header
d. A cookie

## 5. The lesson says "validate at the boundary." Why?

a. The shell is the place untrusted input enters. Reject bad input there so the engine never has to defend itself.
b. Boundary validation runs faster
c. Required by HTTP
d. Style preference

---

When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.

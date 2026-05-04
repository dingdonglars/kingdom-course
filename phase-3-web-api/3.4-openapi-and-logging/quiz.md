# Quiz — Module 3.4

> Don't write your answers in this file — open `journal/quiz-notes.md` and write them there.

## 1. What is an OpenAPI spec?

- **a.** A C# library for parsing JSON
- **b.** A standardised JSON document describing every endpoint of an API — paths, parameters, response shapes, status codes — machine-readable
- **c.** A type of authentication tied to OpenID
- **d.** A logging format used by ASP.NET

## 2. Why prefer `log.LogInformation("Created {Id}", id)` over `log.LogInformation($"Created {id}")`?

- **a.** They behave identically in every way
- **b.** The first uses *structured logging* — `Id` is captured as a named field by destinations that support it. The second loses the structure and ships only a rendered string.
- **c.** The first runs noticeably faster on a hot path
- **d.** It's required by .NET 10 for any logger call

## 3. What does Scalar (or Swagger UI) give you?

- **a.** A C# library that mocks HTTP requests
- **b.** An interactive HTML page generated from the OpenAPI spec — anyone can browse and try the API in a browser
- **c.** An authentication provider for Google Sign-In
- **d.** A test runner for ASP.NET endpoints

## 4. The lesson sets `Microsoft.AspNetCore` log level to `Warning`. What's the reason?

- **a.** To hide errors from the operator
- **b.** The framework emits a lot of `Information`-level chatter on each request. Setting it to `Warning` keeps your own logs visible.
- **c.** It's required by the framework itself
- **d.** Pure performance — `Information` logging is too slow

## 5. Why is OpenAPI especially useful when an AI assistant reads your code?

- **a.** AI assistants can read C# fine without it
- **b.** The spec is a small, complete description of *what your API does* — an AI can call the right endpoints from it without reading every handler. Saves tokens and avoids guesses.
- **c.** AI assistants don't read OpenAPI specs at all
- **d.** It enforces strict typing on incoming JSON

---

When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.

# Quiz — Module 3.3

> Don't write your answers in this file — open `journal/quiz-notes.md` and write them there.

## 1. What's the right status code for a successful POST that creates a new resource?

- **a.** 200 OK with the new resource as the body
- **b.** 201 Created — and include a `Location:` header pointing at the new resource's URL
- **c.** 204 No Content with no body
- **d.** 202 Accepted with no extra information

## 2. What does `MapGroup("/kingdoms")` do?

- **a.** Renames the existing route group on the application
- **b.** Sets a path prefix shared by all endpoints registered on it — so you don't repeat `/kingdoms` in every `Map*` call
- **c.** Creates a database group for kingdom records
- **d.** Loads existing kingdoms from the database

## 3. What does the `:int` in `{id:int}` enforce at routing time?

- **a.** Nothing — it's purely cosmetic
- **b.** A *route constraint* — `/kingdoms/abc` won't match (no int parsing); only routes where `{id}` parses as an `int`
- **c.** That the value must be a 32-bit integer rather than 64-bit
- **d.** That the `id` parameter is required rather than optional

## 4. Why is using `try/catch (InvalidOperationException)` to translate a missing record into a 404 a *smell*?

- **a.** Throwing exceptions is much slower than returning false
- **b.** Exception-as-control-flow — exceptions should signal *exceptional* situations, not normal ones (like "not found"). A `TryLoad` returning `bool` is cleaner.
- **c.** C# does not allow catching `InvalidOperationException`
- **d.** It's not a smell at all — this is the recommended pattern

## 5. Why use `Results.NoContent()` (204) for a successful DELETE instead of 200 OK with a body?

- **a.** There's nothing meaningful to return — the resource is gone. 204 says "success, no body" — saves bytes and matches client expectations.
- **b.** The HTTP framework requires 204 for any DELETE response
- **c.** 200 OK is not a valid response for the DELETE verb
- **d.** It's a tradition with no real reason behind it

---

When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.

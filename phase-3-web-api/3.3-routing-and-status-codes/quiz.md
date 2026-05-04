# Quiz — Module 3.3

## 1. What's the right status code for a successful POST that creates a new resource?

a. 200 OK
b. 201 Created — and include a `Location:` header pointing at the new resource's URL
c. 204 No Content
d. 202 Accepted

## 2. What does `MapGroup("/kingdoms")` do?

a. Renames the group
b. Sets a path prefix shared by all endpoints registered on it — saves repeating `/kingdoms` in every `Map*` call
c. Creates a database group
d. Loads kingdoms

## 3. What does the `:int` in `{id:int}` enforce?

a. Nothing
b. A *route constraint* — `/kingdoms/abc` won't match (no int parsing); only routes where `{id}` parses as an int
c. The value must be 32-bit
d. The id is required

## 4. Why is using `try/catch (InvalidOperationException)` to translate a missing record into a 404 a *smell*?

a. Exceptions are slow
b. Exception-as-control-flow — exceptions should signal *exceptional* situations, not normal ones (like "not found"). A `TryLoad` returning `bool` is cleaner.
c. C# forbids it
d. It's not a smell

## 5. Why `Results.NoContent()` (204) for DELETE instead of 200 OK with a body?

a. There's nothing meaningful to return — the resource is gone. 204 says "success, no body" — saves bytes and matches client expectations.
b. Required by the framework
c. 200 doesn't work for DELETE
d. Tradition
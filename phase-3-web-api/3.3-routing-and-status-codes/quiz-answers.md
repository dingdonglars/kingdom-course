# Quiz answers — Module 3.3

## 1. b
201 Created + `Location:` header — the standard "you POSTed, here's the new thing's URL" answer. Lets the client follow up with `GET <Location>` if it wants the full resource. `Results.Created(uri, value)` does both for you.

## 2. b
`MapGroup` is grouping for routing convenience. Every `Map*` call on the returned group inherits the prefix. You can also chain middleware to a group (auth, validation, etc.). Cleaner code, less repetition.

## 3. b
Route constraints filter matches. `{id:int}` only matches if the path segment parses as an `int`. `/kingdoms/abc` falls through to "no route matched" → 404. Without the constraint, you'd parse manually inside the handler.

## 4. b
Exceptions are for exceptional cases (the database connection died), not normal ones (the requested id doesn't exist). Using exceptions for control flow is harder to read, slower, and easier to silently swallow. `TryLoad(id, out var k)` is cleaner — the next module fixes this.

## 5. a
204 No Content — *"successful, nothing to send."* DELETE conventionally returns 204 because the resource is gone; there's nothing useful in the body. Saves bytes, matches every API client's expectation.
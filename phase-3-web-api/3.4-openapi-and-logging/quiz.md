# Quiz — Module 3.4

## 1. What's an OpenAPI spec?

a. A C# library
b. A standardised JSON document describing every endpoint of an API: paths, parameters, response shapes, status codes — machine-readable
c. A type of authentication
d. A logging format

## 2. Why prefer `log.LogInformation("Created {Id}", id)` over `log.LogInformation($"Created {id}")`?

a. They're identical
b. The first uses *structured logging* — `Id` is captured as a named field by sinks that support it. The second loses the structure (just a rendered string).
c. The first is faster
d. Required by .NET

## 3. What does Scalar (or Swagger UI) give you?

a. A C# library
b. An interactive HTML page generated from the OpenAPI spec — anyone can browse + try the API in a browser
c. An auth provider
d. Logging

## 4. The lesson sets `Microsoft.AspNetCore` log level to `Warning`. Why?

a. To hide errors
b. The framework emits a lot of `Information`-level chatter. Setting it to `Warning` keeps your own logs visible.
c. Required by Microsoft
d. Performance

## 5. Why is OpenAPI especially useful when AI assistants read your code?

a. AI can read C# anyway
b. The spec is a small, complete description of *what your API does* — an AI can call the right endpoints from it without reading every handler. Saves tokens and avoids guesses.
c. AI doesn't need OpenAPI
d. It enforces typing
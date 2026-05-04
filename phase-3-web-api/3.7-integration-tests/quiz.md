# Quiz — Module 3.7

## 1. What's the difference between a unit test and an integration test?

a. They're identical
b. Unit tests exercise one component in isolation; integration tests exercise multiple together — catching seam bugs (routing, serialisation, auth wiring)
c. Integration tests are deprecated
d. Unit tests are slower

## 2. What does `WebApplicationFactory<Program>` do?

a. Compiles the project
b. Boots your whole API in-process (no real port) and gives you an `HttpClient` to make real requests against it
c. Mocks all dependencies
d. Disables auth

## 3. Why does `Program.cs` end with `public partial class Program { }`?

a. Required by the framework
b. So `WebApplicationFactory<Program>` can reference the `Program` type — the top-level Program class needs to be visible to the test project
c. Style preference
d. Documentation

## 4. The lesson recommends ~5-10 integration tests. Why so few?

a. They're slower and more brittle than unit tests. Cover the critical seams — auth-required, OpenAPI spec, key endpoints — and rely on unit tests for breadth.
b. They're hard to write
c. The framework limits them
d. Required by Microsoft

## 5. Why use a per-fixture temp DB?

a. So tests don't interfere with each other or your dev DB. Each fixture gets a fresh DB; gone after the run.
b. Performance
c. Required by EF
d. To save bandwidth
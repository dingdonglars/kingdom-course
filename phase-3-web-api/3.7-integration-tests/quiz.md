# Quiz — Module 3.7

> Don't write your answers in this file — open `journal/quiz-notes.md` and write them there.
>
> Travou no inglês? Abra o `quiz.pt.md` — é este mesmo quiz em português. Tente em inglês primeiro.

## 1. What's the difference between a unit test and an integration test?

- **a.** They are different names for the same kind of test
- **b.** Unit tests exercise one component in isolation; integration tests exercise multiple together — catching seam bugs (routing, serialisation, auth wiring)
- **c.** Integration tests have been deprecated since .NET 9
- **d.** Unit tests are slower and integration tests are faster

## 2. What does `WebApplicationFactory<Program>` do?

- **a.** Compiles the project into a DLL ready for deployment
- **b.** Boots your whole API in-process (no real port) and gives you an `HttpClient` to make real requests against it
- **c.** Mocks every dependency in the application
- **d.** Disables authentication on every endpoint

## 3. Why does `Program.cs` end with `public partial class Program { }`?

- **a.** It's required by every ASP.NET Core project
- **b.** So `WebApplicationFactory<Program>` can reference the `Program` type — top-level Program needs to be visible to the test project
- **c.** It is a stylistic preference with no real effect
- **d.** It enables OpenAPI documentation generation

## 4. The lesson recommends roughly five to ten integration tests. Why so few?

- **a.** They're slower and more brittle than unit tests. Cover the critical seams — auth-required, OpenAPI spec, key endpoints — and rely on unit tests for breadth.
- **b.** They are difficult to write and best avoided
- **c.** The framework limits how many can run in one project
- **d.** Microsoft requires a maximum count for performance reasons

## 5. Why use a per-fixture temp DB?

- **a.** So tests don't interfere with each other or with your dev DB. Each fixture gets a fresh DB; it's gone after the run.
- **b.** Pure performance — temp DBs are faster than disk-based ones
- **c.** EF Core requires a unique DB path on every test invocation
- **d.** To save bandwidth on continuous integration runs

---

When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.

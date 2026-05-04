# Quiz — Bonus B1.2

> Don't write your answers in this file — open `journal/quiz-notes.md` and write them there.

## 1. How many lines of code change to swap SQLite for SQL Server?

- **a.** About three — the package reference, the provider call, the connection string
- **b.** About fifty — provider, store methods, entity configs, test fixtures
- **c.** Several hundred, because every query has to be rewritten in T-SQL
- **d.** The whole engine — store, entities, and queries all need touching

## 2. Why must the migrations be regenerated when you change providers?

- **a.** Migration files contain provider-specific SQL — same C# `Add Column` produces different output per provider
- **b.** Performance — old migrations run too slowly against the new provider
- **c.** EF Core enforces it as a rule; there's no underlying technical reason
- **d.** Style — the new files look better when freshly generated

## 3. The lesson calls the result "boring on purpose." Why is that the point?

- **a.** The engine-vs-shell rule predicted the swap would be small. The boredom is the proof the discipline held.
- **b.** SQL Server is a boring database compared to newer NoSQL options
- **c.** The lesson is short to save time before tomorrow's SSMS install
- **d.** EF Core is a boring framework that hides everything interesting

## 4. The same three-line pattern works for...

- **a.** Only SQL Server, because Microsoft built EF Core for their own database
- **b.** SQL Server, PostgreSQL, MySQL, and any database EF Core has a provider for
- **c.** Only SQLite plus SQL Server, because the others use different APIs
- **d.** Only Microsoft databases — open-source providers need a different approach

## 5. Why does "tests pass unchanged" matter as the moment of proof?

- **a.** Tests describe behaviour, not implementation — passing means the engine still produces the same kingdom against a different store
- **b.** Tests are required by the curriculum; passing them is just compliance
- **c.** Performance — passing tests runs faster than failing tests
- **d.** Style — green ticks look better than red ones

---

When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.

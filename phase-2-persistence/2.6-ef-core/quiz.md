# Quiz — Module 2.6

> Don't write your answers in this file — open `journal/quiz-notes.md` and write them there.
>
> Travou no inglês? Abra o `quiz.pt.md` — é este mesmo quiz em português. Tente em inglês primeiro.

## 1. What does ORM stand for, and what does it do?

- **a.** Object-Relational Mapper — translates between objects in memory and rows in a relational database
- **b.** Online Resource Manager — fetches assets from a remote server
- **c.** Object Reference Model — a way to share object identity across processes
- **d.** Order-Routing Module — a feature of payment systems

## 2. Why don't we map `Kingdom.Engine.Kingdom` directly to a database table?

- **a.** It would work fine; we just chose not to
- **b.** The engine class has interfaces, private fields, and an `IRandom`/`IClock` constructor; EF needs simple property bags
- **c.** EF Core doesn't support classes from other projects in the solution
- **d.** Performance — EF runs faster on entity classes than on engine classes

## 3. What does `ctx.Kingdoms.Add(entity); ctx.SaveChanges();` do?

- **a.** Adds the entity to in-memory state only; no database write
- **b.** Stages the entity for INSERT, then flushes all staged changes to the database in a single transaction
- **c.** Sends a network request to a separate database server process
- **d.** Reads the entity back from the database and returns it

## 4. What does `Include(k => k.Buildings)` do in a query?

- **a.** Filters the query to only kingdoms with at least one building
- **b.** Tells EF to also load the related Buildings list; without it, the navigation property is empty
- **c.** Sorts the results by building count, ascending
- **d.** Adds a new building to every kingdom returned

## 5. The lesson maps `engine model ↔ entity ↔ database`. Why three layers?

- **a.** Each layer has one job, and any one of them can be swapped without rewriting the others
- **b.** Tradition with no clear modern justification
- **c.** .NET requires three layers in any persistence project
- **d.** Performance — the extra layer caches expensive lookups

---

When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.

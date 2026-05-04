# Quiz — Module 2.6

## 1. What does ORM stand for, and what does it do?

a. Object-Relational Mapper — translates between objects in memory and rows in a relational database
b. Online Resource Manager
c. Object Reference Model
d. Order-Routing Module

## 2. Why don't we map `Kingdom.Engine.Kingdom` directly to a database table?

a. It would work fine; we just chose not to
b. The engine class has interfaces, private fields, and a constructor with `IRandom`/`IClock`. EF needs simple property bags to map. So we use entity classes in `Kingdom.Persistence` instead.
c. EF Core doesn't support C# 10
d. Performance

## 3. What does `ctx.Kingdoms.Add(entity); ctx.SaveChanges();` do?

a. Adds the entity to memory only
b. Stages the entity for INSERT, then flushes all staged changes to the database in a single transaction
c. Sends a network request
d. Reads from the database

## 4. What does `Include(k => k.Buildings)` do in a query?

a. Filters the query
b. Tells EF to also load the related Buildings list (eager loading); without `Include`, the navigation property is empty
c. Sorts the results
d. Adds buildings to the entity

## 5. The lesson maps `engine model ↔ entity ↔ database`. Why three layers?

a. Each has one job. The engine doesn't depend on EF; the database doesn't depend on engine quirks. Swapping any layer (e.g., EF Core → Dapper, or SQLite → PostgreSQL) doesn't ripple.
b. Tradition
c. Required by .NET
d. Performance optimization

---

When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.

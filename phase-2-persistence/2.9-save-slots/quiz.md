# Quiz — Module 2.9

> Don't write your answers in this file — open `journal/quiz-notes.md` and write them there.
>
> Travou no inglês? Abra o `quiz.pt.md` — é este mesmo quiz em português. Tente em inglês primeiro.

## 1. CRUD stands for...

- **a.** Code, Refactor, Update, Deploy — the development cycle
- **b.** Create / Read / Update / Delete — the four operations on rows
- **c.** Class, Record, Update, Database — a C# data pattern
- **d.** A LINQ-specific naming convention for queries

## 2. Why use `Find(id)` instead of `Single(k => k.Id == id)` for delete-if-exists?

- **a.** They are functionally identical and either works
- **b.** `Find` returns `null` if the row is missing; `Single` throws — `null` is the right answer for "delete if it exists"
- **c.** `Find` runs faster on every database EF supports
- **d.** Pure style preference with no behaviour difference

## 3. What does `.Select(k => new KingdomSlotInfo(k.Id, k.Name, k.Day))` do in EF?

- **a.** Filters rows where `KingdomSlotInfo` matches
- **b.** Projects each row into a `KingdomSlotInfo`; EF generates SQL that pulls only those three columns
- **c.** Renames the table to `KingdomSlotInfo`
- **d.** Sorts the rows by `Id`, `Name`, `Day` in that order

## 4. Why does `Update` replace the whole `Buildings` list (clear + add) instead of diffing?

- **a.** Simplicity — for a small list this is correct and cheap; large lists would diff to minimise updates
- **b.** EF Core does not support adding to an existing collection
- **c.** `AddRange` is the only EF method that works with navigation properties
- **d.** Performance — clear-and-readd is faster than diffing in every case

## 5. Why is *"list, then load"* the rule?

- **a.** Performance — listing first warms the cache for the load
- **b.** UX — show the player every slot before asking which to load; anything else asks them to remember IDs
- **c.** It is required by the EF Core framework
- **d.** Tradition with no clear underlying reason

---

When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.

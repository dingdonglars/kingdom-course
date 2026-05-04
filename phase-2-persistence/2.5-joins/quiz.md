# Quiz — Module 2.5

> Don't write your answers in this file — open `journal/quiz-notes.md` and write them there.

## 1. What's a foreign key?

- **a.** A key that opens databases written in another language
- **b.** A column whose value matches an `id` in another table — the link between two tables
- **c.** A key from a programming language different to the database's own
- **d.** A SQLite-only feature absent from other databases

## 2. What's the difference between `INNER JOIN` and `LEFT JOIN`?

- **a.** They return identical results in modern databases
- **b.** `INNER JOIN` returns only matched rows; `LEFT JOIN` returns every row from the left table, with `NULL` on the right where no match exists
- **c.** `LEFT JOIN` runs faster because it skips matching
- **d.** `INNER JOIN` does not accept conditions in the `ON` clause

## 3. Why does the lesson keep buildings in their own table instead of stuffing them into a column on `kingdoms`?

- **a.** SQLite refuses to store lists in a single cell
- **b.** Querying inside a stuffed column is awkward and slow; a separate table with a foreign key is queryable, indexable, and joinable
- **c.** Tradition that all databases follow without justification
- **d.** Just to make the schema look bigger

## 4. What does `GROUP BY k.id` do in the COUNT query?

- **a.** Sorts the result rows by kingdom id
- **b.** Collapses rows into one per kingdom, so `COUNT(b.id)` counts buildings *for each kingdom*
- **c.** Filters out rows that don't have an id
- **d.** Joins the table to itself a second time

## 5. Why are the table aliases `k` and `b` used (instead of full names)?

- **a.** SQL requires aliases on every table reference
- **b.** Readability — without aliases you'd repeat `kingdoms.name`, `buildings.kingdom_id` everywhere; the query becomes English-shaped
- **c.** To hide the underlying table names from the database
- **d.** Aliases run faster than full names

---

When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.

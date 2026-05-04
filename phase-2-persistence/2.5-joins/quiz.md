# Quiz — Module 2.5

## 1. What's a foreign key?

a. A key that opens foreign databases
b. A column whose value matches an `id` in another table — the link between two tables
c. A key from a different programming language
d. A SQLite-only feature

## 2. What's the difference between `INNER JOIN` and `LEFT JOIN`?

a. They're identical
b. `INNER JOIN` returns only matched rows; `LEFT JOIN` returns every row from the left table, with `NULL` on the right where no match exists
c. `LEFT JOIN` is faster
d. `INNER JOIN` doesn't accept conditions

## 3. Why does the lesson keep buildings in their own table instead of stuffing them into a column on `kingdoms`?

a. SQLite can't store lists in a cell
b. Querying inside a stuffed column is awkward and slow. A separate table with a foreign key is queryable, indexable, and joinable — the relational model's core idea.
c. Tradition
d. To pad the schema

## 4. What does `GROUP BY k.id` do in the COUNT query?

a. Sorts the results
b. Collapses rows into one per kingdom — so `COUNT(b.id)` counts the buildings *for each kingdom*
c. Filters
d. Joins again

## 5. Why are the table aliases `k` and `b` used (instead of full names)?

a. They're required
b. Brevity — without aliases you'd repeat `kingdoms.name`, `buildings.kingdom_id` everywhere; aliases keep the query readable
c. To hide the table names
d. Performance

---

When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.

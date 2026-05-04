# Quiz answers — Module 2.5

## 1. b
A foreign key is a column whose value points at the `id` (primary key) of a row in another table. `buildings.kingdom_id` is a foreign key to `kingdoms.id` — that's how a building "belongs to" a kingdom. Foreign keys are how relational databases form the relationships between entities.

## 2. b
`INNER JOIN` keeps only rows that match on both sides; if a kingdom has no buildings, it disappears from the result. `LEFT JOIN` keeps every row from the left table; the right side becomes `NULL` where there's no match. Use `LEFT JOIN` when you need "all parents, even if no children."

## 3. b
Stuffing a list into a column means you can't easily query the items. A separate table with a foreign key is the standard relational model: every entity has its own table, related entities are linked by foreign keys. Indexing, joining, filtering all become trivial.

## 4. b
`GROUP BY k.id` collapses the rows so there's exactly one result row per distinct `k.id`. Then aggregates (`COUNT`, `SUM`, etc.) operate within each group. `COUNT(b.id)` counts buildings *per kingdom*, because rows are grouped by kingdom.

## 5. b
Aliases shrink `kingdoms.name` and `buildings.kingdom_id` to `k.name` and `b.kingdom_id`. Once a query has 2+ tables, aliases massively improve readability. The cost is one letter of memory.
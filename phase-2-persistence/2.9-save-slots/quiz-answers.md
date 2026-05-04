# Quiz answers — Module 2.9

## 1. b
CRUD = Create / Read / Update / Delete. The four operations cover ~90% of database interactions. APIs, ORMs, REST endpoints, admin panels — all built on CRUD.

## 2. b
`Find` returns the entity if it exists, `null` if not. That matches "delete if exists, no-op otherwise" perfectly: `if (entity is null) return;`. `Single` throws `InvalidOperationException` on miss — appropriate when missing is genuinely an error (Load), not when missing is fine (Delete).

## 3. b
Projection — instead of materialising the full `KingdomEntity` (with all its columns and the navigation collection), EF generates a SQL `SELECT id, name, day FROM Kingdoms`. Less data over the wire, no entity tracking overhead, you only pay for what you use.

## 4. a
Clear-and-readd is the simplest correct strategy for small lists. EF tracks the deletes and inserts in one transaction. For a list of thousands you'd diff (find which buildings changed, only touch those), but the diff logic is meaningful work — don't pre-optimise.

## 5. b
List-then-load is the contract every save-slot UI follows: show what exists, let the user pick. The opposite (ask for an ID upfront) shifts the cognitive load to the user. Same rule applies to file pickers, contact lists, search-then-click — anything where the user picks from a known set.
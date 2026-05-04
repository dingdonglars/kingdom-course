# Quiz — Module 2.9

## 1. CRUD stands for...

a. Code, Refactor, Update, Deploy
b. Create / Read / Update / Delete — the four operations on rows
c. Class, Record, Update, Database
d. A pattern from C#

## 2. Why use `Find(id)` instead of `Single(k => k.Id == id)` for delete-if-exists?

a. They're identical
b. `Find` returns `null` if missing — perfect for "delete if it exists, no-op otherwise." `Single` throws.
c. `Find` is faster
d. Style preference

## 3. What does `.Select(k => new KingdomSlotInfo(k.Id, k.Name, k.Day))` do in EF?

a. Filters rows
b. Projects each row into a `KingdomSlotInfo`. EF generates SQL that fetches only those three columns (not the whole row) — faster and lighter.
c. Renames the table
d. Sorts the rows

## 4. Why does `Update` replace the whole `Buildings` list (clear + add) instead of diffing?

a. Simplicity. For a small list this is correct and cheap. Real apps with large lists would diff to minimise updates, but the tradeoff is more complex code.
b. Required by EF
c. EF doesn't support adding to existing collections
d. Performance

## 5. Why is "list, then load" the rule?

a. Performance
b. UX — show the player every slot before asking which to load. Anything else is hostile.
c. Required by the framework
d. Tradition
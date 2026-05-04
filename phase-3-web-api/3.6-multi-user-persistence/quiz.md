# Quiz — Module 3.6

## 1. Why does *every* store method take `ownerSub` as a parameter?

a. To make the API verbose
b. So a caller who forgets it gets a compile error, not a security bug. Mandatory parameters are guard rails.
c. EF requires it
d. To make tests easier

## 2. What's the difference between authentication and authorisation?

a. They're identical
b. Authentication = *who are you* (sign-in). Authorisation = *what are you allowed to do* (e.g., access this kingdom)
c. Authorisation runs before authentication
d. Only one is needed

## 3. Why prefer `sub` over `email` to identify the user in the database?

a. `sub` is shorter
b. `email` can change; `sub` (subject id) is permanent and globally unique
c. `email` doesn't exist in claims
d. Tradition

## 4. Why is the *cross-user* test (`Load other user's kingdom throws`) the most important one?

a. It catches the bug class that real-world breaches happen on. The "happy path" test wouldn't notice if you forgot the `WHERE OwnerSub = ?` clause.
b. It runs faster
c. Required by xUnit
d. It's not particularly important

## 5. Why does `HasIndex(k => k.OwnerSub)` matter?

a. EF requires it
b. Every list query is `WHERE OwnerSub = ?`. Without an index, the DB scans the whole table — slow as data grows. With an index, it's a direct lookup.
c. For security
d. Style preference

---

When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.

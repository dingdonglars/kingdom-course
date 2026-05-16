# Quiz — Module 3.6

> Don't write your answers in this file — open `journal/quiz-notes.md` and write them there.
>
> Travou no inglês? Abra o `quiz.pt.md` — é este mesmo quiz em português. Tente em inglês primeiro.

## 1. Why does *every* store method take `ownerSub` as a parameter?

- **a.** To make the API more verbose for documentation purposes
- **b.** So a caller who forgets it gets a compile error rather than a security bug. Required parameters are guard rails.
- **c.** Because EF Core's tracker requires a per-user identifier on every call
- **d.** To make the unit tests simpler to write

## 2. What's the difference between authentication and authorisation?

- **a.** They are different spellings of exactly the same idea
- **b.** Authentication answers *who are you* (sign-in). Authorisation answers *what are you allowed to do* (e.g., access this kingdom).
- **c.** Authorisation always runs before authentication in ASP.NET
- **d.** Only authentication is needed; authorisation is optional padding

## 3. Why prefer `sub` over `email` to identify the user in the database?

- **a.** `sub` is shorter and saves a few bytes per row
- **b.** `email` can change; `sub` (the subject id) is permanent and globally unique
- **c.** `email` doesn't appear in the claims set Google returns
- **d.** It's a tradition with no real reason behind it

## 4. Why is the *cross-user* test (*loading another user's kingdom throws*) the most important one?

- **a.** It catches the bug class real-world breaches actually happen on. The happy-path test wouldn't notice if you forgot the `WHERE OwnerSub = ?` clause.
- **b.** It runs noticeably faster than the other tests
- **c.** It's required by xUnit on multi-user projects
- **d.** It's not particularly important compared to the others

## 5. Why does `HasIndex(k => k.OwnerSub)` matter?

- **a.** EF Core refuses to compile without an index on every queryable column
- **b.** Every list query is `WHERE OwnerSub = ?`. Without an index, the DB scans the whole table — slow as data grows. With an index, it's a direct lookup.
- **c.** It enables encryption on the column at rest
- **d.** It is purely a stylistic preference with no runtime effect

---

When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.

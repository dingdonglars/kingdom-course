# Quiz — Module 2.11 (naming-themed)

## 1. Why is `KingdomManager` a weak name?

a. It has too many letters
b. `Manager` is a noise word — manager of *what*? It tells the reader the thing exists, not what it does.
c. It conflicts with C# keywords
d. Naming conventions forbid it

## 2. The 3-letter local `b` for a building inside a 5-line method is...

a. Always wrong — names should be long
b. Fine — scope is tiny, the type is obvious from the surrounding code, the reader doesn't need more
c. Required by some style guide
d. Bad form

## 3. Why do the rename party in *one focused session* instead of "as I go"?

a. Faster typing
b. Pure-rename PRs are easy to review (no logic mixed in), and a single sitting catches related names that should change together
c. To impress your team
d. No real reason

## 4. What's the right tool for renaming across a codebase?

a. Search-and-replace by hand
b. The IDE's Rename refactoring (F2 in VS / Rider) — updates every call site, comment, doc, test in one shot, safely
c. A regex
d. `git ls-files | xargs sed`

## 5. The lesson says "every name pays a cost and earns a benefit." What's the cost?

a. CPU cycles
b. The reader has to learn it. Every name has weight in the reader's head; weak names earn less than they cost.
c. Compile time
d. Disk space
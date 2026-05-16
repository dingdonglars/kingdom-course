# Quiz — Module 2.11 (naming-themed)

> Don't write your answers in this file — open `journal/quiz-notes.md` and write them there.
>
> Travou no inglês? Abra o `quiz.pt.md` — é este mesmo quiz em português. Tente em inglês primeiro.

## 1. Why is `KingdomManager` a weak name?

- **a.** It uses too many letters and slows down readers
- **b.** *Manager* is a noise word — it tells you the thing exists, not what it does
- **c.** It conflicts with a reserved C# keyword
- **d.** Naming conventions explicitly forbid suffixes longer than five letters

## 2. The 3-letter local `b` for a building inside a 5-line method is...

- **a.** Always wrong; names should always be at least four letters
- **b.** Fine — scope is tiny, the type is obvious from the surrounding code
- **c.** Required by some major C# style guides
- **d.** Bad form because single-letter names are unprofessional

## 3. Why do the rename party in *one focused session* instead of "as I go"?

- **a.** It types faster when you batch the keystrokes
- **b.** Pure-rename PRs are easy to review, and one sitting catches related names that should change together
- **c.** To impress a reviewing teammate
- **d.** No real reason; either approach works equally well

## 4. What's the right tool for renaming across a codebase?

- **a.** Search-and-replace by hand in each file
- **b.** The IDE's Rename refactoring (F2) — updates every reference, comment, and test in one shot, safely
- **c.** A regex applied with `sed` or PowerShell
- **d.** `git ls-files | xargs sed -i 's/old/new/g'`

## 5. The lesson says *"every name pays a cost and earns a benefit."* What's the cost?

- **a.** CPU cycles spent looking up the symbol at runtime
- **b.** The reader has to learn it; weak names earn less than they cost
- **c.** Compile time spent resolving the symbol
- **d.** Disk space taken up by the longer string

---

When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.

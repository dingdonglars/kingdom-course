# Quiz — Module 1.9

> Don't write your answers in this file — open `journal/quiz-notes.md` and write them there.
>
> Travou no inglês? Abra o `quiz.pt.md` — é este mesmo quiz em português. Tente em inglês primeiro.

## 1. The compiler doesn't care about folders. So why use them?

- **a.** The compiler does care; folders affect compilation order
- **b.** Folders are a convention that helps humans scan the codebase. Matching folders to namespaces keeps the layout consistent in two places.
- **c.** Folders speed up `dotnet build` by parallelising compilation
- **d.** Mainly to look organised in screenshots and reviews

## 2. What's a *global using*?

- **a.** A `using` directive in a file like `GlobalUsings.cs` that applies to every file in the project
- **b.** A `using` for `System` only, applied as a default by the SDK
- **c.** A keyword that doesn't exist — it's a community proposal not yet in C#
- **d.** A normal `using` directive; "global" is just a comment-style label

## 3. Why does the *aggregate root* (`Kingdom.cs`) stay at the top level instead of in a subfolder?

- **a.** Convention — the root class lives at the root, not buried in a subfolder named after itself
- **b.** C# requires the aggregate root to live at the project root
- **c.** It compiles faster when it's not in a subfolder
- **d.** It was placed there by accident and got left

## 4. What's the rough threshold for splitting a flat folder into subfolders?

- **a.** Exactly five files; the .NET style guide insists on it
- **b.** Around seven files — a soft rule of thumb. More than you can scan at a glance.
- **c.** A hundred files; below that, flat is fine
- **d.** Never — flat is best, splitting always adds friction

## 5. The lesson cautions against pre-emptively sub-namespacing. Why?

- **a.** Sub-namespaces add a runtime cost on every method call
- **b.** They add `using` lines and "where did that type come from" friction. Worth it for mid/large projects, overkill for tiny ones.
- **c.** The compiler emits warnings for projects with too many sub-namespaces
- **d.** Pre-emptive sub-namespacing is forbidden by the .NET style guide

---

When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.

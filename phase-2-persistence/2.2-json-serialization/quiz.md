# Quiz — Module 2.2

> Don't write your answers in this file — open `journal/quiz-notes.md` and write them there.

## 1. Why introduce a separate `Kingdom.Persistence` project instead of putting JSON code in `Kingdom.Engine`?

- **a.** To make the file count look more impressive in commits
- **b.** Other runtimes (Roblox, database) won't use JSON; keeping it separate keeps the engine free of unwanted dependencies
- **c.** .NET requires JSON code to live in its own project
- **d.** It's the only way to pass the M3 milestone challenge

## 2. What's a DTO?

- **a.** A small data-only record purpose-built for crossing a boundary (disk, network)
- **b.** Direct Type Override, a C# language feature
- **c.** Default Type Output, a serializer mode
- **d.** A keyword for declaring data classes in C#

## 3. Why is `KingdomSummary` a `record` instead of a `class`?

- **a.** Records run faster than classes at the JIT level
- **b.** Records give you free immutability and value-equality, both ideal for DTOs
- **c.** Records are the only type the JSON serializer accepts
- **d.** Pure style preference; either would work identically

## 4. What does `WriteIndented = true` do?

- **a.** Makes the JSON multi-line and readable; flip to `false` for network use
- **b.** Validates the JSON before writing it to disk
- **c.** Adds line numbers as comments in the output
- **d.** Encrypts the output before saving

## 5. Why isn't `EventLog` in the `KingdomSummary` record?

- **a.** Forgot to include it; would otherwise be there
- **b.** The summary is intentionally limited — only the fields we need; later modules build a fuller snapshot
- **c.** The JSON serializer cannot handle lists of objects
- **d.** EventLog is a private field and unreachable from outside the engine

---

When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.

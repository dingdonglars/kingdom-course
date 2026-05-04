# Quiz — Module 5.2

> Don't write your answers in this file — open `journal/quiz-notes.md` and write them there.

## 1. What does `local` do in Luau?

- **a.** Declares a variable in the global namespace
- **b.** Declares a variable scoped to its function or block — without `local`, the variable becomes global
- **c.** Imports a module from `ReplicatedStorage`
- **d.** A reserved word with no effect on the variable

## 2. What is the array index of the first element in Luau?

- **a.** Zero — same as C# and JavaScript
- **b.** One — Lua and Luau are one-indexed; the daily gotcha for C# and JavaScript brains
- **c.** Negative one, counting from the end of the array
- **d.** Either zero or one, depending on the table

## 3. What is the syntax for joining two strings in Luau?

- **a.** `+` — same as C#
- **b.** `..` — two dots; `+` would be a number error
- **c.** `&` — borrowed from BASIC
- **d.** A built-in `concat()` function call

## 4. What is the difference between `ipairs` and `pairs`?

- **a.** `ipairs` walks an array in order and stops at the first nil; `pairs` walks every key in a dictionary in unspecified order
- **b.** Nothing — they are aliases for the same function
- **c.** `ipairs` is the older form and is now deprecated
- **d.** `pairs` runs faster on large tables

## 5. Why does Luau add type annotations on top of plain Lua?

- **a.** They give Studio enough information to underline the wrong type as you type — same value as TypeScript, runtime-cheap
- **b.** They are required for any script that runs on the server
- **c.** They make the Lua interpreter run noticeably faster at runtime
- **d.** They are a style preference with no functional effect

---

When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.

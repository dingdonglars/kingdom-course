# Quiz — Module 5.5

## 1. Why use string keys (`"Gold"`) instead of an enum in Luau?

a. Lua doesn't have enums; strings are the standard idiom for closed sets
b. Performance
c. Required by Roblox
d. Style preference

## 2. What does `task.wait(5)` do?

a. Blocks the entire game for 5 seconds
b. Pauses *this script* for 5 seconds without blocking other scripts (Roblox runs each in a coroutine)
c. Waits for 5 events
d. Performance optimisation

## 3. The lesson says "the model is forever; the runtime is a detail." How many runtimes have you proven this on by end of Block 7?

a. 2
b. 3
c. 5 — console, file/JSON/SQLite, web API, browser, Roblox
d. 10

## 4. The Luau `Kingdom.lua` is shorter than the C# `Kingdom.cs`. Why?

a. Luau is more powerful
b. Luau has less ceremony — no namespaces, no using directives, no public/private modifiers, no XML docs. Same shape, smaller text.
c. The Luau version is incomplete
d. C# is wordy by nature

## 5. Why should the game loop run on the server, not the client?

a. The server is authoritative — running it client-side means each player has a different state, easy to cheat. Server ticks once; clients see the result via replication.
b. Performance
c. Client can't run loops
d. Tradition

---

When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.

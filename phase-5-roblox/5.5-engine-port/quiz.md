# Quiz — Module 5.5

> Don't write your answers in this file — open `journal/quiz-notes.md` and write them there.
>
> Travou no inglês? Abra o `quiz.pt.md` — é este mesmo quiz em português. Tente em inglês primeiro.

## 1. Why use string keys (`"Gold"`) instead of an enum in Luau?

- **a.** Lua doesn't have enums — strings are the standard idiom for closed sets, and Luau's editor still type-checks them
- **b.** Strings are faster than enums on the Lua interpreter
- **c.** Roblox requires string keys for any value stored in a table
- **d.** A style preference with no functional difference

## 2. What does `task.wait(5)` do?

- **a.** Blocks the entire game for five seconds, including every other script
- **b.** Pauses *this script* for five seconds without blocking others — Roblox runs each script in its own coroutine
- **c.** Waits for five events to fire on a RemoteEvent
- **d.** Hints to the runtime that the script is finished

## 3. The lesson says "the engine doesn't care what runtime it sits in." How many runtimes have you proven that on by the end of this module?

- **a.** Two — console and Roblox
- **b.** Three — adding the web API in the middle
- **c.** Five — console, file with JSON and SQLite, web API, browser, Roblox
- **d.** Ten or more, counting every shell variation

## 4. The Luau `Kingdom.lua` is shorter than the C# `Kingdom.cs`. Why?

- **a.** Luau is a more powerful language than C# in every way
- **b.** Luau has less ceremony — no namespaces, no `using` directives, no public/private modifiers, no XML documentation. Same idea, smaller text.
- **c.** The Luau version is incomplete and skips functionality
- **d.** C# is wordy by nature; the shorter Luau text isn't really meaningful

## 5. Why should the game loop run on the server, not the client?

- **a.** The server is authoritative — running the loop on each client means each player has their own version of state, which is trivially exploitable. The server ticks once; clients see the result via replication.
- **b.** Performance — the server's hardware is faster than any player's
- **c.** Clients can't run loops at all in Roblox
- **d.** Tradition borrowed from older Roblox tutorials

---

When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.

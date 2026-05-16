# Quiz — Module 5.7

> Don't write your answers in this file — open `journal/quiz-notes.md` and write them there.
>
> Travou no inglês? Abra o `quiz.pt.md` — é este mesmo quiz em português. Tente em inglês primeiro.

## 1. Where can you call DataStore from?

- **a.** Server scripts only — `DataStoreService` is server-only by design, for security
- **b.** Anywhere — server, client, ModuleScripts called from either
- **c.** LocalScripts only — DataStore is a client-side store
- **d.** From `ReplicatedStorage` scripts only

## 2. What does `pcall(fn)` do?

- **a.** Calls the function asynchronously, returning a promise-like object
- **b.** Lua's protected call — returns `(true, result)` if the function ran cleanly, `(false, error)` if it threw
- **c.** A performance optimisation that compiles the function before running it
- **d.** A required wrapper for any DataStore call to avoid syntax errors

## 3. Why is `BindToClose` important?

- **a.** When the server shuts down (for maintenance, redeploy, or migration), Roblox gives you about thirty seconds. `BindToClose` is your final chance to flush unsaved player data before the process dies.
- **b.** It's optional decoration that some Roblox developers add for style
- **c.** Required on every script in the place; the runtime warns without it
- **d.** A performance hint to the runtime about expected close time

## 4. What is the rule about saving frequency?

- **a.** Save on every tick — losing data is unacceptable in a multiplayer game
- **b.** Don't save on every tick. The DataStore quota and the per-call cost both punish that. Save on player leave plus every five minutes or so for safety.
- **c.** Save once per session, on shutdown only
- **d.** Save whenever convenient; quotas don't apply to small payloads

## 5. The lesson cites Module 2.1, Module 2.2, Module 2.4, Module 2.6, and Module 5.7 as the same pattern. What is it?

- **a.** Snapshot the engine state, write it somewhere, read it back, rehydrate. Same discipline; different medium each time.
- **b.** They are all SQL-based persistence at heart
- **c.** They are all JSON-based persistence at heart
- **d.** They are not really the same — only the lesson framing makes them look alike

---

When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.

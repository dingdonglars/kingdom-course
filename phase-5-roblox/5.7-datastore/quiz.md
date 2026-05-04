# Quiz — Module 5.7

## 1. Where can you call DataStore from?

a. Server scripts only — DataStoreService is server-only by design (security)
b. Anywhere
c. LocalScripts only
d. ReplicatedStorage

## 2. What does `pcall(fn)` do?

a. Calls the function async
b. Lua's protected-call (try/catch); returns `(true, result)` on success or `(false, error)` on throw
c. Performance call
d. Required for DataStore

## 3. Why is `BindToClose` important?

a. When the server shuts down (e.g., for maintenance), Roblox gives you ~30 seconds. `BindToClose` is your final chance to save unsaved player data before the process dies.
b. It's optional decoration
c. Required for any script
d. Performance

## 4. What's the rule about saving frequency?

a. Every tick — losing data is unacceptable
b. Don't save on every tick (DataStore quota; cost). Save on player leave + every ~5 minutes for safety.
c. Once per session
d. Whenever convenient

## 5. The lesson cites M2.1 → M2.2 → M2.4 → M2.6 → M5.7 as the same shape. What is it?

a. Snapshot the engine state, write it somewhere, read it back, rehydrate. Same discipline; different medium.
b. They're all SQL
c. They're all JSON
d. They're not the same

---

When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.

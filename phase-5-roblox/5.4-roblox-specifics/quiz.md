# Quiz — Module 5.4

> Don't write your answers in this file — open `journal/quiz-notes.md` and write them there.

## 1. Where does the authoritative game state live in Roblox?

- **a.** On each client — every player has their own copy
- **b.** On the server — one per place; clients can send messages but the server validates and applies them
- **c.** In `Workspace`, replicated by Roblox between everyone
- **d.** Either side, depending on the developer's preference

## 2. What is a RemoteEvent?

- **a.** An async one-way message between client and server — the wiring that lets the two halves of a multiplayer place talk
- **b.** A C#-style event reused across the Roblox runtime
- **c.** A logging tool that records events to the Output panel
- **d.** Roblox's version of async/await on a single thread

## 3. Why does the server treat client messages as untrusted?

- **a.** Clients can be tampered with — cheats and exploits modify the local code. Validate everything server-side; never trust raw client input.
- **b.** Performance — server validation is faster than client validation
- **c.** Required by Roblox; the runtime throws if you skip the check
- **d.** Tradition borrowed from older multiplayer engines

## 4. What is `ReplicatedStorage` for?

- **a.** Holding server-only data that clients should never see
- **b.** Holding client-only assets each player downloads on join
- **c.** Holding code and objects shared between server and clients — engine ModuleScripts and RemoteEvents live here
- **d.** Holding parts that have been replicated from somewhere else

## 5. Why is "server is authority, client is presentation" the rule?

- **a.** Multiplayer places need exactly one source of truth, and that has to be the server. Clients render the result. Without this rule, players see different states and accuse each other of lag.
- **b.** It's a performance optimisation; the server is always faster
- **c.** Tradition borrowed from older Roblox documentation
- **d.** Required by the .NET runtime that backs Roblox

---

When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.

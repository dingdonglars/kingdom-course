# Quiz — Module 5.4

## 1. Where does authoritative game state live in Roblox?

a. The client
b. The server (one per place; clients can send messages but server validates + applies)
c. Workspace
d. Either

## 2. What's a RemoteEvent?

a. A C# event
b. Async one-way message between client ↔ server; lets the two halves of a multiplayer game talk
c. A logging tool
d. A Roblox-specific async/await

## 3. Why does the server treat client messages as untrusted?

a. Clients can be tampered with (cheats, exploits). Trust the server's own validation; never trust raw client input.
b. Performance
c. Required by Roblox
d. Tradition

## 4. What's `ReplicatedStorage` for?

a. Server-only data
b. Client-only assets
c. Shared between server and clients — engine ModuleScripts and RemoteEvents live here
d. Storage for replicated parts only

## 5. Why is "server is authority, client is presentation" the rule?

a. Multiplayer games need exactly one source of truth (server). Clients render the result. Without this rule, players see different state and accuse each other of lag.
b. Performance only
c. Tradition
d. Required by .NET

---

When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.

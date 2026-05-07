# Module 5.4 — Roblox Specifics: Workspace, Server vs Client, RemoteEvents

Roblox isn't just "Lua plus 3D." It's a multiplayer-first runtime — every place is split between *one server* and *many clients*. Today we name that split, work out which code belongs where, and meet **RemoteEvents** — the messaging system clients and server use to talk.

> **Words to watch**
>
> - **Workspace** — the live 3D scene. Every part, model, and character lives here.
> - **Server** — runs once on Roblox's machines. Authoritative. Every place has exactly one.
> - **Client** — runs on each player's device. Each connected player has their own client.
> - **RemoteEvent** — a one-way async message between server and client.
> - **RemoteFunction** — a request/response message; the caller waits for an answer.
> - **Replication** — Roblox's built-in sync of `Workspace` changes from server to clients.

---

## Why server vs client matters

Every Roblox game is a multiplayer game, even single-player ones — there's still a server, just with one client connected. The line that matters is: **authoritative state lives on the server.** Anything that affects gameplay belongs on the server. The client is for showing what the server says.

Some examples to make the rule concrete:

| Code | Where it runs | Why |
| --- | --- | --- |
| Engine tick (resources change) | Server | The server is the authority; client code can be tampered with. |
| UI rendering | Client | Each player sees a different view. |
| Particle effects | Client | Cosmetic only; running fifty on the server would lag everyone. |
| Save and load (DataStore) | Server | The DataStore API is server-only by design. |
| Detecting a mouse click | Client | The mouse belongs to the player. |
| Reacting to a mouse click | Server (via RemoteEvent) | The server validates, then applies. |

The rule, in plain words: **client shows and asks; server validates and applies.**

## Folders by intent

| Folder | Lives on | Visible to |
| --- | --- | --- |
| `ServerScriptService` | Server | Server only |
| `ServerStorage` | Server | Server only (data, blueprints) |
| `ReplicatedStorage` | Both | Server and clients (engine modules go here) |
| `StarterPlayerScripts` | Client | Each player when they join |
| `StarterPack` | Both | Items each player gets in their backpack on spawn |
| `Workspace` | Both | Replicated to all clients |

`ReplicatedStorage` is the shared library of the place — engine ModuleScripts live there so both server scripts and client scripts can `require` them.

## RemoteEvent — the bridge

When a client needs to ask the server to do something, the pattern is always the same:

1. The server has a `RemoteEvent` somewhere in `ReplicatedStorage`.
2. The client fires it with `event:FireServer(args)`.
3. The server has connected to it with `event.OnServerEvent:Connect(function(player, args) ... end)`.

Going the other way — server to client — uses `event:FireClient(player, args)` or `event:FireAllClients(args)`, and the client listens with `event.OnClientEvent`.

```lua
-- ReplicatedStorage/Events/TickRequest is a RemoteEvent inserted via Studio Explorer.

-- Client (LocalScript in StarterPlayerScripts)
local event = game.ReplicatedStorage.Events.TickRequest
event:FireServer(5)   -- "advance 5 days, please"

-- Server (Script in ServerScriptService)
local event = game.ReplicatedStorage.Events.TickRequest
event.OnServerEvent:Connect(function(player, days)
    -- validate days; apply to that player's kingdom; reply
    print(player.Name, "asked for", days, "days")
end)
```

The single most important rule of RemoteEvent handling: **the server treats every client message as untrusted.** Validate the input — is `days` reasonable? Does this player own the kingdom they're acting on? — before doing anything with it. Same discipline as the API endpoints you wrote in Phase 3.

## Replication

When you change a Part in `Workspace` from server code, Roblox automatically syncs the change to every connected client. Move a brick on the server and every player sees it move. **You don't write the network code — Roblox does.**

The flip side of the rule: a change a *client* makes to `Workspace` doesn't replicate. It's visible on that client only. (`ReplicatedFirst` and `ReplicatedStorage` work similarly for client-side writes.) For shared changes, route through the server via RemoteEvent.

## Tinker

Start a Studio test session with two simulated clients via *Test → Local Server → 2 players*. Run a script that fires a RemoteEvent from one client and watch the server log it for both. The round-trip is visible in the Output of each window.

Try changing a Part in `Workspace` from a LocalScript — move it to a different position, change its colour. Other clients don't see the change. Move the same code to a Script in `ServerScriptService` and the change replicates everywhere.

Try sending a giant payload via `FireServer` — a ten-megabyte string. Roblox throttles you with a clear error. RemoteEvents are designed for small messages; bulk asset transfer goes through the asset system instead.

## What you just did

You met the multiplayer split that the rest of Phase 5 builds on. One server, many clients; the server is authoritative; the client is for presentation. RemoteEvents are the messaging system between the two halves, with one going one way (`FireServer`, `FireClient`) and one taking a request and giving an answer (`RemoteFunction`). You also met replication — Roblox's automatic sync of `Workspace` changes from server to clients, which is the reason most multiplayer games don't have to write any network code themselves. The rule to take into the next four modules: client shows and asks; server validates and applies.

**Key concepts you can now name:**

- *server vs client* — one authoritative server; one client per player
- *RemoteEvent* — async one-way message between the two halves
- *replication* — automatic sync of `Workspace` changes from server out
- *`ReplicatedStorage`* — shared library; both halves can `require` modules here
- *the multiplayer rule* — client shows and asks; server validates and applies

## Words to add to the glossary

- **server** — the authoritative process; one per Roblox place.
- **client** — a single player's process; one per connected player.
- **RemoteEvent** — one-way async message between server and client.
- **RemoteFunction** — request/response message between server and client.
- **replication** — automatic sync of `Workspace` changes from server to clients.

## Quiz

Open `quiz.md`. When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.

## Next

Module 5.5 starts the **engine port** properly — `ResourceLedger`, `Citizen`, `Kingdom` — translated into Luau. Server-side. With the test from Module 5.3 as the smoke check.

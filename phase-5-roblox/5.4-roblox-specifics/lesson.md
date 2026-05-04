# Module 5.4 — Roblox Specifics: Workspace, Server vs Client, RemoteEvents

> **Hook:** Roblox isn't just "Lua + 3D." It's a multiplayer-first runtime — every game is split between *server* (one) and *clients* (each player). Today we name that split, learn why some code runs where, and meet **RemoteEvents** — the way client and server talk.

> **Words to watch**
> - **Workspace** — the live 3D scene; every part / model / character lives here
> - **Server** — runs once on Roblox's machines; authoritative
> - **Client** — runs on each player's device; one per player
> - **RemoteEvent** — async one-way message between server ↔ client
> - **RemoteFunction** — request/response between server ↔ client
> - **Replication** — Roblox's built-in sync of Workspace changes across clients

---

## Why server vs client matters

Every Roblox game is a multiplayer game (even single-player ones — there's still a server, just with one client). **Authoritative state lives on the server.** Cosmetic state can live on the client.

Examples:

| Code | Where | Why |
|---|---|---|
| Engine tick (resource changes) | Server | Authority; can't trust client to honor rules |
| UI rendering | Client | Each player has a different view |
| Particle effects | Client | Cosmetic; running 50 on the server would lag |
| Save/load (DataStore) | Server | DataStore API is server-only |
| Detect mouse click | Client | Mouse is the player's input |
| React to mouse click | Server (via RemoteEvent) | Server validates + applies |

**Anything that affects gameplay belongs on the server.** Client is presentation.

## Folders by intent

| Folder | Lives on | Visible to |
|---|---|---|
| `ServerScriptService` | Server | Server only |
| `ServerStorage` | Server | Server only (data, blueprints) |
| `ReplicatedStorage` | Both | Server + clients (engine modules go here) |
| `StarterPlayerScripts` | Client | Each player on join |
| `StarterPack` | Both | Items each player gets on spawn |
| `Workspace` | Both | Replicated to all clients |

`ReplicatedStorage` is the "shared library" — engine ModuleScripts live there so both server scripts and client scripts can `require` them.

## RemoteEvent — the bridge

When a client needs to *ask the server to do something*, the pattern is:

1. Server has a `RemoteEvent` in `ReplicatedStorage`.
2. Client fires it: `event:FireServer(args)`.
3. Server listens: `event.OnServerEvent:Connect(function(player, args) ... end)`.

Or going server → client:

1. Server fires: `event:FireClient(player, args)` or `event:FireAllClients(args)`.
2. Client listens: `event.OnClientEvent:Connect(function(args) ... end)`.

```lua
-- ReplicatedStorage/Events/TickRequest (RemoteEvent — insert via Studio Explorer)

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

**Server treats every client message as untrusted.** Validate the input (`days <= 100`?), check the player owns what they're asking about, etc. Same discipline as your API endpoints in Block 5.

## Replication

When you change a Part in `Workspace` from server code, Roblox automatically syncs the change to every client. Move a brick on the server → all players see it move. **You don't write the network code — Roblox does.**

The flip side: **changes a client makes to Workspace don't replicate.** They appear locally on that client only. (`ReplicatedFirst` and `ReplicatedStorage` have similar local-only writes.) For shared changes, route through the server via RemoteEvent.

## Delta starter

- `roblox-kingdom/Events/SETUP.md` — instructions to insert RemoteEvents in Studio
- `roblox-kingdom/scripts/server/handle-tick.lua` (Script in ServerScriptService)
- `roblox-kingdom/scripts/client/request-tick.lua` (LocalScript in StarterPlayerScripts)

## Tinker

- Start a Studio test session with 2 simulated clients (Test → Local Server → 2 players). Watch a client→server→client roundtrip in Output.
- Try changing a Part in Workspace from a LocalScript. **Other clients don't see it.** Move the same code to a Script in ServerScriptService — replicates everywhere.
- Try sending a giant payload via `FireServer` (a 10MB string). **Roblox throttles you.** RemoteEvents are best for small messages; bulk data uses the asset system.

## Name it

- **Server (one) / Client (one per player)** — the multiplayer split.
- **`Workspace`** — replicated 3D scene.
- **`ReplicatedStorage`** — shared modules + RemoteEvents.
- **`ServerScriptService` / `ServerStorage`** — server-only.
- **`StarterPlayerScripts`** — runs on each client on join.
- **RemoteEvent** — async one-way client↔server message.
- **RemoteFunction** — request/response client↔server (use for "ask server, get answer").

## The rule of the through-line

> **Server is authoritative. Client is presentation. RemoteEvents are the bridge.** Every multiplayer feature you build will follow this shape: client shows + asks; server validates + applies + tells everyone.

## Quiz / challenge

Open `quiz.md`.

## Connect

Module 5.5 starts the **engine port** in earnest — `ResourceLedger`, `Citizen`, `Kingdom` — to Luau. Server-side. With the test from M5.3 as the smoke check.
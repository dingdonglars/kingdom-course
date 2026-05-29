# Module 5.4 — Roblox Specifics: Workspace, Server vs Client, RemoteEvents

Roblox isn't just "Lua plus 3D." It's built for multiplayer from the start. Every place is split between *one server* and *many clients*. Today we name that split, work out which code belongs where, and meet **RemoteEvents** — the way clients and the server send messages to each other.

> **Words to watch**
>
> - **Workspace** — the live 3D scene. Every part, model, and character lives here.
> - **Server** — runs once on Roblox's machines. Holds the real game state. Every place has exactly one.
> - **Client** — runs on each player's device. Each connected player has their own client.
> - **RemoteEvent** — a one-way async message between server and client.
> - **RemoteFunction** — a request/response message; the caller waits for an answer.
> - **Replication** — Roblox's built-in sync of `Workspace` changes from server to clients.

---

## Why server vs client matters

Every Roblox game is a multiplayer game, even single-player ones. There's still a server, just with one client connected. Here is the rule that matters: **the real state of the game lives on the server.** Anything that affects gameplay belongs on the server. The client is for showing what the server says.

Some examples to make the rule clear:

| Code | Where it runs | Why |
| --- | --- | --- |
| Engine tick (resources change) | Server | The server is the authority; client code can be tampered with. |
| UI rendering | Client | Each player sees a different view. |
| Particle effects | Client | Cosmetic only; running fifty on the server would lag everyone. |
| Save and load (DataStore) | Server | The DataStore API is server-only by design. |
| Detecting a mouse click | Client | The mouse belongs to the player. |
| Reacting to a mouse click | Server (via RemoteEvent) | The server validates, then applies. |

The rule, in plain words: **client shows and asks; server validates and applies.**

## What each folder is for

| Folder | Lives on | Visible to |
| --- | --- | --- |
| `ServerScriptService` | Server | Server only |
| `ServerStorage` | Server | Server only (data, blueprints) |
| `ReplicatedStorage` | Both | Server and clients (engine modules go here) |
| `StarterPlayerScripts` | Client | Each player when they join |
| `StarterPack` | Both | Items each player gets in their backpack on spawn |
| `Workspace` | Both | Replicated to all clients |

`ReplicatedStorage` is the shared library of the place. Engine ModuleScripts live there so both server scripts and client scripts can `require` them.

## RemoteEvent — the link between the two sides

When a client needs to ask the server to do something, the steps are always the same:

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

The most important rule when handling a RemoteEvent: **the server should not trust any message from a client.** Check the input first — is `days` a sensible number? Does this player own the kingdom they're trying to change? — before you do anything with it. This is the same care you used for the API endpoints you wrote in Phase 3.

## Replication

When you change a Part in `Workspace` from server code, Roblox automatically copies the change to every connected client. Move a brick on the server and every player sees it move. **You don't write the network code — Roblox does.**

The other half of the rule: a change a *client* makes to `Workspace` is not copied. It's visible on that client only. So if you want a change everyone can see, send it through the server with a RemoteEvent.

## Tinker

Start a Studio test session with two pretend clients via *Test → Local Server → 2 players*. Run a script that fires a RemoteEvent from one client and see the server log it for both. The message and the reply both show up in the Output of each window.

Try changing a Part in `Workspace` from a LocalScript — move it to a different position, change its colour. Other clients don't see the change. Move the same code to a Script in `ServerScriptService` and the change appears everywhere.

Try sending a huge message with `FireServer` — a ten-megabyte string. Roblox blocks you with a clear error. RemoteEvents are made for small messages. Large files are sent a different way.

## What you just did

You met the multiplayer split that the rest of Phase 5 builds on. One server, many clients. The server holds the real state; the client just shows it. RemoteEvents are how the two sides send messages. A RemoteEvent goes one way (`FireServer`, `FireClient`). A RemoteFunction sends a request and waits for an answer. You also met replication — Roblox copying `Workspace` changes from the server to the clients on its own. This is why most multiplayer games don't have to write any network code themselves. Keep this rule in mind for the next four modules: the client shows and asks; the server checks and applies.

**Key concepts you can now name:**

- *server vs client* — one server holds the real state; one client per player
- *RemoteEvent* — a one-way message between the two sides
- *replication* — Roblox copying `Workspace` changes from server to clients
- *`ReplicatedStorage`* — shared library; both sides can `require` modules here
- *the multiplayer rule* — client shows and asks; server checks and applies

## On your own

Time to put the book away. Don't scroll back up to the code — prove to yourself, from your own head, that the two sides stuck. No one marks this one — it's just for you. It's the easiest way to spot what *hasn't* stuck yet, while it's still simple to fix. Getting stuck here is completely fine — that's exactly what it's for.

In your own words, finish this sentence out loud: "The client ___ and ___; the server ___ and ___." Then write the two lines that send a message from a client to a server: the client fires the event, and the server listens for it.

<details><summary>Stuck? Open this to check yourself.</summary>

The rule: **the client shows and asks; the server checks and applies.** The real state lives on the server.

The two lines, with a RemoteEvent that already sits in `ReplicatedStorage`:

```lua
-- Client
event:FireServer(5)

-- Server
event.OnServerEvent:Connect(function(player, days)
    print(player.Name, "asked for", days, "days")
end)
```

The server line always gets `player` first, then whatever the client sent. And the server should check that value before using it — never trust a message from a client.

</details>

## Words to add to the glossary

- **server** — the side that holds the real game state; one per Roblox place.
- **client** — one player's side; one per connected player.
- **RemoteEvent** — a one-way message between server and client.
- **RemoteFunction** — a request-and-reply message between server and client.
- **replication** — Roblox copying `Workspace` changes from server to clients.

## Wrap up

1. **Quiz** — open `quiz.md`, jot your answers in `journal/quiz-notes.md`.
2. **Progress** — one line in `journal/progress.md`: `Module 5.4 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit and push** — stage both files, commit message `Module 5.4 done`, Sync.
4. **Post in `#wins`** — one line about today, plus the URL of the commit.

Module 0.1 covers the why and the panel/CLI steps if you need a refresher. Bring quiz answers you're least sure about to the next weekly sync.

## Next

Module 5.5 starts the **engine port** properly — `ResourceLedger`, `Citizen`, `Kingdom` — translated into Luau, running on the server. You'll use the test from Module 5.3 to check it still works.

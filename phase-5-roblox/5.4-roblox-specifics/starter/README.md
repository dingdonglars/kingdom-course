# Module 5.4 starter — Roblox specifics

A small client-server demo: a `RemoteEvent` that the client fires and the server logs.

## What's in here

- `roblox-kingdom/Events/SETUP.md` — written setup instructions for the RemoteEvent (Studio's UI is the only way to insert one, so the file walks through the clicks).
- `roblox-kingdom/scripts/server/handle-tick.lua` — the server-side listener. Validates the input and prints the player and value.
- `roblox-kingdom/scripts/client/request-tick.lua` — the client-side fire. Sends a request once on join.

## How to use it

In Studio:

1. In the Explorer, insert a `Folder` under `ReplicatedStorage` and name it `Events`.
2. Inside `Events`, insert a `RemoteEvent` named `TickRequest`.
3. Insert a `Script` under `ServerScriptService` and paste `scripts/server/handle-tick.lua` into it.
4. Insert a `LocalScript` under `StarterPlayerScripts` and paste `scripts/client/request-tick.lua` into it.
5. Run the place via *Test → Local Server → 2 players* and watch the Output of each window.

## Common gotcha

The names of the `Folder` (`Events`) and the `RemoteEvent` (`TickRequest`) need to match what the script paths look up. If the client throws *attempt to index nil with FireServer*, the path is wrong — usually a typo in the folder or event name in the Explorer.

The other gotcha is putting the LocalScript in the wrong folder. LocalScripts only run from `StarterPlayerScripts` (or `StarterPlayer.StarterCharacterScripts`), not from `ServerScriptService`.

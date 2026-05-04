# Module 5.7 starter — DataStore

Save the kingdom on player leave, load it on join. Server-side only; the engine adds two methods (`toSnapshot`, `fromSnapshot`) and the rest is Roblox's `DataStoreService`.

## What's in here

- `roblox-kingdom/scripts/server/save-load.lua` — the `PlayerAdded`, `PlayerRemoving`, and `BindToClose` handlers, plus the `pcall`-wrapped DataStore calls.
- `roblox-kingdom/Engine/Kingdom.lua` (extended) — adds `:toSnapshot()` and `.fromSnapshot()` methods to the existing engine module.

## How to use it

1. In Studio, open *Game Settings → Security* and tick **Enable Studio Access to API Services**. DataStore is disabled in Studio by default; this opt-in lets you test save/load without publishing.
2. Replace the existing `Kingdom` ModuleScript under `ReplicatedStorage/Engine` with the extended `Kingdom.lua` from this folder. The two new methods are at the bottom.
3. Insert a `Script` under `ServerScriptService` named `SaveLoad`. Paste `scripts/server/save-load.lua` into it.
4. Hit Play. The first time a player joins, they get a fresh kingdom. Quit the test session. Hit Play again as the same user. The kingdom returns at the same day count and resource totals.

## Common gotcha

If `GetAsync` is throwing or returning `nil` unexpectedly, check API Services is enabled in Game Settings. Studio without it lets the API exist but silently no-ops every call.

The other gotcha: `BindToClose` only runs when the server actually shuts down. In Studio, that means clicking *Stop*. If you close Studio entirely, `BindToClose` may not finish before the process is killed. For real testing, use *Stop* and watch the Output panel for the save log.

## A note on quotas

The free tier allows about sixty DataStore calls per minute per server, with up to four megabytes per key. The starter saves on player leave plus a five-minute background timer; that's well within the budget for a small place. Don't be tempted to save on every tick — a busy place would blow the quota in seconds.

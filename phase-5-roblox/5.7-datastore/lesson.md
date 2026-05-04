# Module 5.7 — Roblox DataStore: Persistence

> **Hook:** today the kingdom survives across sessions. Roblox provides **DataStoreService** — a key/value store accessible from server code only. We save the kingdom's snapshot on player leave; load it on join. Same instinct as M2.2's JSON store, different API.

> **Words to watch**
> - **DataStore** — Roblox's key/value persistence; ~free; per-place
> - **`DataStoreService`** — the API; only callable from server scripts
> - **`SetAsync(key, value)` / `GetAsync(key)`** — the two basic ops
> - **`UpdateAsync(key, fn)`** — atomic update; safer than get+set
> - **JSON via Lua** — `HttpService:JSONEncode/JSONDecode` for encoding tables

---

## DataStore in 30 seconds

```lua
local DataStoreService = game:GetService("DataStoreService")
local store = DataStoreService:GetDataStore("Kingdoms")

-- Write
store:SetAsync("player_12345", { day = 11, gold = 250 })

-- Read
local data = store:GetAsync("player_12345")
print(data.day, data.gold)
```

That's the entire API for 95% of cases. **Server-only** — DataStore won't load in a LocalScript.

`SetAsync` and `GetAsync` are async (network round-trips to Roblox's servers). They can throw on transient errors; wrap in `pcall` for production code.

## Snapshot ↔ JSON

Roblox tables → JSON for storage:

```lua
local HttpService = game:GetService("HttpService")

-- Encode
local json = HttpService:JSONEncode({ day = 11, name = "Eldoria" })
-- Decode
local table = HttpService:JSONDecode(json)
```

Most of the time you don't need this — DataStore can store tables directly. But when you need a string (e.g., to send via RemoteEvent), use JSON.

## The save/load lifecycle

```lua
local DataStoreService = game:GetService("DataStoreService")
local Players = game:GetService("Players")
local store = DataStoreService:GetDataStore("Kingdoms")

local kingdoms: { [number]: any } = {}   -- userId → kingdom in memory

local function key(player: Player): string
    return "player_" .. player.UserId
end

local function loadKingdom(player: Player)
    local snapshot = nil
    local ok, err = pcall(function()
        snapshot = store:GetAsync(key(player))
    end)
    if not ok then
        warn("Failed to load:", err)
        return Kingdom.new(player.Name .. "'s Kingdom")
    end
    if snapshot then
        return Kingdom.fromSnapshot(snapshot)
    end
    return Kingdom.new(player.Name .. "'s Kingdom")
end

local function saveKingdom(player: Player, kingdom: any)
    local snapshot = kingdom:toSnapshot()
    local ok, err = pcall(function()
        store:SetAsync(key(player), snapshot)
    end)
    if not ok then
        warn("Failed to save:", err)
    end
end

Players.PlayerAdded:Connect(function(player)
    kingdoms[player.UserId] = loadKingdom(player)
end)

Players.PlayerRemoving:Connect(function(player)
    if kingdoms[player.UserId] then
        saveKingdom(player, kingdoms[player.UserId])
        kingdoms[player.UserId] = nil
    end
end)

game:BindToClose(function()
    -- Server is shutting down — save everyone first
    for _, player in ipairs(Players:GetPlayers()) do
        if kingdoms[player.UserId] then
            saveKingdom(player, kingdoms[player.UserId])
        end
    end
end)
```

Five hooks:

- `PlayerAdded` — load when joining
- `PlayerRemoving` — save when leaving
- `BindToClose` — save everyone on server shutdown (Roblox gives you ~30 seconds)
- `pcall` — Roblox's try/catch; DataStore errors are recoverable
- `warn` — logs to Output in yellow; visible in production logs

## Engine snapshot/rehydrate

`Kingdom:toSnapshot()` and `Kingdom.fromSnapshot(data)` need to exist on the Luau engine — same idea as Block 4's `KingdomSnapshot`. Add:

```lua
function Kingdom:toSnapshot()
    return {
        name = self.name,
        day = self.day,
        gold = self.resources:get("Gold"),
        wood = self.resources:get("Wood"),
        stone = self.resources:get("Stone"),
        food = self.resources:get("Food"),
        buildings = (function()
            local out = {}
            for _, b in ipairs(self.buildings) do
                table.insert(out, { kind = getmetatable(b).__name, name = b.name, level = b.level })
            end
            return out
        end)(),
    }
end

function Kingdom.fromSnapshot(snap: any)
    -- (similar to C# Kingdom.LoadFrom — full implementation in starter)
end
```

(Each subclass would set `__name` on its metatable, e.g., `Farm.__name = "Farm"`. Same role as `b.GetType().Name` in C#.)

## DataStore quotas

Free tier: ~60 calls/min/server, 4MB/key. Plenty for one player at a time. **Don't save on every tick** — save on player leave + every ~5 minutes.

For production, layer in:
- **Session locking** — prevent two server instances writing the same key at once
- **Backups** — version history if a save corrupts
- **Migration** — handle old snapshot shapes

Roblox has libraries for these (e.g., `ProfileService`, `Suphi DataStore`) — out of scope for the lesson.

## Delta starter

- `roblox-kingdom/scripts/server/save-load.lua` — Player+/PlayerLeaving handlers
- Engine extension: `Kingdom:toSnapshot()` / `Kingdom.fromSnapshot()` (added to `Kingdom.lua`)

## Tinker

- Test in Studio with "Test → Local Server" — DataStore is **disabled** in Studio by default. Enable: `Game Settings → Security → Enable Studio Access to API Services`.
- Save explicitly via a chat command (`/save`); restart the server; reload as the same player. **Same kingdom returns.**
- Look at the `__EFMigrationsHistory` parallel: in DataStore, you might add a `version` field to your snapshot and handle older versions on load. **Same migration concept; different mechanism.**

## Name it

- **DataStore** — Roblox's k/v store; server-only.
- **`SetAsync` / `GetAsync` / `UpdateAsync`** — basic ops.
- **`pcall(fn)`** — Lua's try/catch; returns success bool + result.
- **`warn(...)`** — yellow log; visible in production.
- **`Players.PlayerAdded` / `PlayerRemoving`** — lifecycle hooks.
- **`BindToClose`** — final flush on server shutdown.

## The rule of the through-line

> **Persistence on every runtime; same shape.** File (M2.1), JSON (M2.2), SQLite (M2.4), EF Core (M2.6), DataStore (M5.7). Always: snapshot the engine state, write it somewhere, read it back, rehydrate. The medium varies; the discipline doesn't.

## Quiz / challenge

Open `quiz.md`.

## Connect

Module 5.8 is the last: **publish + capstone reflection**. Friends play your game. The course closes.
# Module 5.7 — Roblox DataStore: Persistence

Today the kingdom survives across sessions. Roblox provides **DataStoreService** — a key/value store accessible from server code only. We save the kingdom's snapshot when a player leaves and reload it when they come back. Same idea as Phase 2's JSON file, with a different API.

> **Words to watch**
>
> - **DataStore** — Roblox's key/value persistence. Free up to a quota; one set per place.
> - **`DataStoreService`** — the API. Only callable from server scripts.
> - **`SetAsync(key, value)` / `GetAsync(key)`** — the two basic operations.
> - **`UpdateAsync(key, fn)`** — atomic update; safer than read-then-write.
> - **`pcall`** — Lua's protected call, equivalent to a try/catch wrapper.
> - **JSON via Lua** — `HttpService:JSONEncode` and `JSONDecode` for encoding tables to strings.

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

That's the entire API for ninety-five percent of cases. The two functions are server-only — `DataStore` won't load in a `LocalScript`.

`SetAsync` and `GetAsync` are async — they make a network round-trip to Roblox's servers. They can throw on transient errors, so production code wraps them in `pcall`.

## Snapshot to JSON

Most of the time you don't need to convert anything yourself — DataStore can store a Lua table directly. When you do need a string (for example, sending one through a RemoteEvent), Roblox provides JSON via `HttpService`:

```lua
local HttpService = game:GetService("HttpService")

-- Encode
local json = HttpService:JSONEncode({ day = 11, name = "Eldoria" })
-- Decode
local table = HttpService:JSONDecode(json)
```

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
    -- Server is shutting down — save everyone first.
    for _, player in ipairs(Players:GetPlayers()) do
        if kingdoms[player.UserId] then
            saveKingdom(player, kingdoms[player.UserId])
        end
    end
end)
```

Five hooks worth naming:

- `PlayerAdded` — fires when someone joins; load their kingdom here.
- `PlayerRemoving` — fires when someone leaves; save their kingdom here.
- `BindToClose` — fires when the server itself is shutting down; Roblox gives you about thirty seconds to flush.
- `pcall` — Lua's try/catch. DataStore errors are recoverable, so wrapping the call lets the script continue if the network blips.
- `warn` — logs to the Output panel in yellow; visible in production logs as well as in Studio.

## Engine snapshot and rehydrate

`Kingdom:toSnapshot()` and `Kingdom.fromSnapshot(data)` need to exist on the Luau engine — same idea as the `KingdomSnapshot` you wrote in Phase 2. A first sketch:

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
    -- (similar to the Phase 2 Kingdom.LoadFrom; full implementation in the starter)
end
```

Each subclass sets `__name` on its metatable — `Farm.__name = "Farm"`, `Mine.__name = "Mine"` — which plays the same role as `b.GetType().Name` did in C#.

## DataStore quotas

The free tier gives you about sixty calls per minute per server, with up to four megabytes per key. Plenty for one player at a time. Don't save on every tick — save on player leave plus every five minutes or so for safety.

For production, you'd add three more layers:

- **Session locking** — prevent two server instances from writing the same key at once.
- **Backups** — version history in case a save corrupts.
- **Migration** — handle older snapshot shapes when you change the format.

Roblox has community libraries for these — `ProfileService` and `Suphi DataStore` are the two most-named — but they're out of scope for this lesson.

## Tinker

Test in Studio with *Test → Local Server*. DataStore is **disabled** in Studio by default; enable it via *Game Settings → Security → Enable Studio Access to API Services*.

Save explicitly via a chat command (`/save`), restart the server, reload as the same player, and watch the same kingdom return.

Notice the parallel with the EF Core migrations table from Phase 2: in DataStore, you'd add a `version` field to your snapshot and handle older versions on load. Same migration concept; different mechanism.

## What you just did

You taught the kingdom to survive across sessions on Roblox. `Players.PlayerAdded` loads the player's snapshot from DataStore on join; `Players.PlayerRemoving` saves it on leave; `game:BindToClose` flushes everyone when the server itself shuts down. `pcall` wraps the network calls so a transient blip doesn't kill the script. The `Kingdom:toSnapshot` and `Kingdom.fromSnapshot` methods live on the engine itself, mirroring the Phase 2 design — the engine knows how to describe itself, and the persistence layer just moves bytes. **The same pattern five times now: file (Module 2.1), JSON (Module 2.2), SQLite (Module 2.4), EF Core (Module 2.6), DataStore (Module 5.7).** The medium changes; the discipline doesn't.

**Key concepts you can now name:**

- *DataStoreService* — Roblox's built-in key/value store, server-only
- *`SetAsync` / `GetAsync` / `UpdateAsync`* — the three basic operations
- *`pcall`* — Lua's try/catch wrapper; protects against transient errors
- *`PlayerAdded` / `PlayerRemoving` / `BindToClose`* — lifecycle hooks for save and load
- *snapshot and rehydrate* — engine method on one side, persistence on the other

## Words to add to the glossary

- **DataStore** — Roblox's key/value persistence; server-only.
- **`SetAsync` / `GetAsync` / `UpdateAsync`** — the basic DataStore operations.
- **`pcall`** — Lua's protected call; returns success boolean plus result or error.
- **`warn`** — yellow log in Output; visible in production server logs.
- **`BindToClose`** — final-flush hook when the server shuts down.

## Quiz

Open `quiz.md`. When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.

## Next

Module 5.8 is the last one. **Publish, M6, and the year-end reflection.** Friends play your game. The course closes.

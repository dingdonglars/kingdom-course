# Module 5.7 — Roblox DataStore: Persistence

Today the kingdom stays saved between play sessions. Roblox gives you **DataStoreService** — a key/value store you can only use from server code. We save the kingdom's snapshot when a player leaves and load it again when they come back. Same idea as Phase 2's JSON file, with a different API.

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

That's almost the whole API for most cases. The two functions only work on the server — `DataStore` won't load in a `LocalScript`.

`SetAsync` and `GetAsync` are async: they send a request to Roblox's servers and wait for the reply. Sometimes that request fails for a moment, and the call throws an error. So real games wrap these calls in `pcall`.

## Snapshot to JSON

Most of the time you don't need to convert anything yourself — DataStore can store a Lua table as-is. When you do need a string (for example, to send one through a RemoteEvent), Roblox gives you JSON through `HttpService`:

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

Five things worth naming:

- `PlayerAdded` — runs when someone joins; load their kingdom here.
- `PlayerRemoving` — runs when someone leaves; save their kingdom here.
- `BindToClose` — runs when the server itself is shutting down; Roblox gives you about thirty seconds to save everything.
- `pcall` — Lua's try/catch. A DataStore error can be recovered from, so wrapping the call lets the script keep going if the network fails for a moment.
- `warn` — prints to the Output panel in yellow; you can see it in the live server logs as well as in Studio.

## Engine: save and load

`Kingdom:toSnapshot()` and `Kingdom.fromSnapshot(data)` need to exist on the Luau engine — same idea as the `KingdomSnapshot` you wrote in Phase 2. `toSnapshot` turns the kingdom into a plain table you can save; `fromSnapshot` builds a full kingdom back from that table. A first sketch:

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

Each subclass sets `__name` on its metatable — `Farm.__name = "Farm"`, `Mine.__name = "Mine"` — which does the same job as `b.GetType().Name` did in C#.

## DataStore quotas

The free plan gives you about sixty calls per minute per server, with up to four megabytes per key. That's plenty for one player at a time. Don't save on every tick — save when the player leaves, plus every five minutes or so to be safe.

For a real, busy game, you'd add three more things:

- **Session locking** — stop two servers from writing the same key at the same time.
- **Backups** — keep old versions in case a save gets corrupted.
- **Migration** — handle older save formats when you change the layout.

Roblox has community libraries for these — `ProfileService` and `Suphi DataStore` are the two most often named — but they're beyond what this lesson covers.

## Tinker

Test in Studio with *Test → Local Server*. DataStore is **turned off** in Studio by default. Turn it on with *Game Settings → Security → Enable Studio Access to API Services*.

Add a chat command (`/save`) that saves on demand, restart the server, join again as the same player, and see the same kingdom come back.

Notice how this is like the EF Core migrations table from Phase 2: in DataStore, you'd add a `version` field to your snapshot and handle older versions when you load. Same migration idea; different tool.

## What you just did

You taught the kingdom to stay saved between sessions on Roblox. `Players.PlayerAdded` loads the player's snapshot from DataStore when they join. `Players.PlayerRemoving` saves it when they leave. `game:BindToClose` saves everyone when the server itself shuts down. `pcall` wraps the network calls, so a short failure doesn't stop the whole script. The `Kingdom:toSnapshot` and `Kingdom.fromSnapshot` methods live on the engine itself, the same as the Phase 2 design — the engine knows how to describe itself, and the saving code just moves the data. **The same pattern five times now: file (Module 2.1), JSON (Module 2.2), SQLite (Module 2.4), EF Core (Module 2.6), DataStore (Module 5.7).** The way you store it changes; the careful method doesn't.

**Key concepts you can now name:**

- *DataStoreService* — Roblox's built-in key/value store, server-only
- *`SetAsync` / `GetAsync` / `UpdateAsync`* — the three basic operations
- *`pcall`* — Lua's try/catch wrapper; keeps the script going if a call fails for a moment
- *`PlayerAdded` / `PlayerRemoving` / `BindToClose`* — events that run on join, leave, and shutdown
- *save and load* — the engine describes itself; the saving code moves the data

## Words to add to the glossary

- **DataStore** — Roblox's key/value persistence; server-only.
- **`SetAsync` / `GetAsync` / `UpdateAsync`** — the basic DataStore operations.
- **`pcall`** — Lua's protected call; returns success boolean plus result or error.
- **`warn`** — yellow log in Output; visible in production server logs.
- **`BindToClose`** — runs one last save when the server shuts down.

## Wrap up

1. **Quiz** — open `quiz.md`, jot your answers in `journal/quiz-notes.md`.
2. **Progress** — one line in `journal/progress.md`: `Module 5.7 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit and push** — stage both files, commit message `Module 5.7 done`, Sync.
4. **Post in `#wins`** — one line about today, plus the URL of the commit.

Module 0.1 covers the why and the panel/CLI steps if you need a refresher. Bring quiz answers you're least sure about to the next weekly sync.

## Next

Module 5.8 is the last one. **Publish, M6, and the year-end reflection.** Friends play your game. The course closes.

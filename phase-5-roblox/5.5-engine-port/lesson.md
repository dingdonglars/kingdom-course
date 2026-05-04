# Module 5.5 — Engine Port: ResourceLedger, Citizen, Kingdom, Tick

> **Hook:** today the engine reaches feature parity with Block 3. `ResourceLedger`, `Citizen`, the `Kingdom` aggregate, the day-tick loop — all in Luau. Same shape; smaller language. **By the end of this module, the engine works exactly the same in both runtimes.**

> **Words to watch**
> - **port** — translate code into another language while preserving behavior
> - **`task.wait(seconds)`** — Roblox's `setTimeout` / `Thread.Sleep` equivalent; pauses without blocking other code
> - **`game:GetService("RunService")`** — Roblox's per-frame heartbeat, used for game loops

---

## Engine port plan

| Block 3 (C#) | Roblox (Luau) | Notes |
|---|---|---|
| `Resource.cs` (enum) | string keys ("Gold", "Wood", ...) | Lua doesn't have enums; strings are the idiom |
| `ResourceLedger.cs` | `ResourceLedger.lua` (ModuleScript) | Same get/add/spend/snapshot |
| `Citizen.cs` | `Citizen.lua` (ModuleScript) | Same |
| `Building.cs` + subclasses | done in M5.3 | already ported |
| `Kingdom.cs` (aggregate) | `Kingdom.lua` (ModuleScript) | Owns lists; calls `:tick(ledger)` |
| `EventEngine.cs` | `EventEngine.lua` | Uses Lua's `math.random` (or pass an injected fn) |
| Console/file shells | n/a | Server-side game loop replaces them |

## `ResourceLedger.lua`

```lua
local ResourceLedger = {}
ResourceLedger.__index = ResourceLedger

function ResourceLedger.new()
    local self = setmetatable({}, ResourceLedger)
    self.amounts = { Gold = 0, Wood = 0, Stone = 0, Food = 0 }
    return self
end

function ResourceLedger:get(resource: string): number
    return self.amounts[resource] or 0
end

function ResourceLedger:add(resource: string, amount: number)
    if amount < 0 then error("Use spend for negatives") end
    self.amounts[resource] = (self.amounts[resource] or 0) + amount
end

function ResourceLedger:spend(resource: string, amount: number): boolean
    if amount < 0 then error("Spend amount must be non-negative") end
    local have = self.amounts[resource] or 0
    if have < amount then return false end
    self.amounts[resource] = have - amount
    return true
end

function ResourceLedger:snapshot(): { [string]: number }
    local out = {}
    for k, v in pairs(self.amounts) do out[k] = v end
    return out
end

return ResourceLedger
```

Same five methods as the C# version. `error(...)` is Lua's `throw`.

## `Citizen.lua`

```lua
local Citizen = {}
Citizen.__index = Citizen

function Citizen.new(name: string)
    local self = setmetatable({}, Citizen)
    self.name = name
    return self
end

return Citizen
```

Even smaller than C# — just a name (matching Block 1's minimal `Citizen`).

## `Kingdom.lua`

```lua
local Building = require(script.Parent.Building)
local ResourceLedger = require(script.Parent.ResourceLedger)

local Kingdom = {}
Kingdom.__index = Kingdom

function Kingdom.new(name: string)
    local self = setmetatable({}, Kingdom)
    self.name = name
    self.day = 1
    self.buildings = {}
    self.citizens = {}
    self.resources = ResourceLedger.new()
    -- Starting resources
    self.resources:add("Gold", 100)
    self.resources:add("Wood", 50)
    self.resources:add("Stone", 20)
    self.resources:add("Food", 30)
    return self
end

function Kingdom:addBuilding(b)
    table.insert(self.buildings, b)
end

function Kingdom:addCitizen(c)
    table.insert(self.citizens, c)
end

function Kingdom:advanceDay()
    -- Buildings produce
    for _, b in ipairs(self.buildings) do
        b:tick(self.resources)
    end
    -- Citizens eat
    for _ in ipairs(self.citizens) do
        self.resources:spend("Food", 1)
    end
    self.day = self.day + 1
end

return Kingdom
```

The whole engine, in one file. Reads identical to the C# `Kingdom.cs`.

## The game loop (server)

```lua
-- ServerScriptService/Script

local Engine = game.ReplicatedStorage:WaitForChild("Engine")
local Kingdom = require(Engine.Kingdom)
local Farm = require(Engine.Farm)
local Mine = require(Engine.Mine)
local Citizen = require(Engine.Citizen)

local kingdom = Kingdom.new("Eldoria")
kingdom:addBuilding(Farm.new("Main Farm"))
kingdom:addBuilding(Mine.new("Old Mine"))
kingdom:addCitizen(Citizen.new("Lyra"))

-- Tick every 5 seconds (5 days/min — slow but visible)
while true do
    task.wait(5)
    kingdom:advanceDay()
    print(string.format(
        "Day %d — Gold:%d Wood:%d Stone:%d Food:%d",
        kingdom.day,
        kingdom.resources:get("Gold"),
        kingdom.resources:get("Wood"),
        kingdom.resources:get("Stone"),
        kingdom.resources:get("Food")))
end
```

`task.wait(5)` pauses the script for 5 seconds. **Doesn't block other scripts** — Roblox runs each in its own coroutine.

For a real game, the loop would tick faster (every second, every 0.1 second), use `RunService.Heartbeat:Connect(function(dt) ... end)` for frame-rate sync, etc. **For learning, `task.wait` is enough.**

## Delta starter

- `roblox-kingdom/Engine/ResourceLedger.lua`
- `roblox-kingdom/Engine/Citizen.lua`
- `roblox-kingdom/Engine/Kingdom.lua`
- `roblox-kingdom/scripts/server/main.lua` — the game loop

Insert into Studio under `ReplicatedStorage/Engine/` (ModuleScripts) and `ServerScriptService/Script` (the loop). Hit Play; watch the kingdom tick in Output.

## Tinker

- Increase the tick rate to `task.wait(1)`. Watch the resources change every second.
- Add a third `Lumberyard.new("Eastern Lumberyard")`. Wood starts climbing.
- Replace the `while true do` with `RunService.Heartbeat:Connect(function() ... end)` and tick every N frames. **Same engine; different scheduler.**
- Compare the Luau `Kingdom.lua` to the C# `Kingdom.cs`. **Same shape, smaller text.**

## Name it

- **Port** — translate to another language while preserving meaning.
- **String keys instead of enums** — Lua idiom for closed sets.
- **`task.wait(seconds)`** — pause; doesn't block other coroutines.
- **`RunService.Heartbeat`** — frame-rate game loop (covered as you scale).

## The rule of the through-line

> **The engine doesn't care what runtime it's in.** Same buildings, same ledger, same kingdom, same advance-day. C# in Block 3, Luau in Block 7. The model is forever; the runtime is a detail.

> **You have lived this lesson now five times: console (Block 3), file/JSON/SQLite (Block 4), web API (Block 5), browser (Block 6), Roblox (Block 7). Five shells, one engine.** The point of the curriculum lands.

## Quiz / challenge

Open `quiz.md`.

## Connect

Module 5.6 introduces **the visual world** — building a 3D representation of the kingdom in Workspace. Click a tile, build a farm; the engine drives the appearance.
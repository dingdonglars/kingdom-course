# Module 5.5 — Engine Port: ResourceLedger, Citizen, Kingdom, Tick

Today the engine reaches feature parity with Phase 1 — but in Luau, on a Roblox server. `ResourceLedger`, `Citizen`, the `Kingdom` aggregate, the day-tick loop. Same idea, smaller language, a different runtime. By the end of this module the engine works the same way in both worlds.

> **Words to watch**
>
> - **port** — translate code into another language while keeping its meaning.
> - **`task.wait(seconds)`** — Roblox's pause-this-script-for-a-while function. Doesn't block other scripts.
> - **`game:GetService("RunService")`** — the per-frame heartbeat service, used for game loops that need a frame-rate tick.
> - **coroutine** — a function that can pause itself and be resumed later. Roblox runs every script in one of these, which is why `task.wait` doesn't freeze the place.

---

## The port plan

Reading the C# version next to the Luau version is the fastest way to internalise the translation:

| Phase 1 (C#) | Roblox (Luau) | Note |
| --- | --- | --- |
| `Resource.cs` (enum) | string keys (`"Gold"`, `"Wood"`, ...) | Lua doesn't have enums; strings are the standard. |
| `ResourceLedger.cs` | `ResourceLedger.lua` (ModuleScript) | Same get / add / spend / snapshot. |
| `Citizen.cs` | `Citizen.lua` (ModuleScript) | Same. |
| `Building.cs` and subclasses | done in Module 5.3 | Already ported. |
| `Kingdom.cs` (aggregate) | `Kingdom.lua` (ModuleScript) | Owns the lists; calls `:tick(ledger)`. |
| `EventEngine.cs` | `EventEngine.lua` | Uses Lua's `math.random`, or pass an injected function to keep tests deterministic. |
| Console / file shells | not applicable | Server-side game loop replaces them. |

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

The same five methods as the C# version. `error(...)` is Lua's `throw`.

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

Even smaller than the C# version — just a name. Matches the minimal Phase 1 `Citizen`.

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

The whole engine in one file. Read it next to the C# `Kingdom.cs` and you'll see the same lines, just shorter.

## The game loop, server-side

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

-- Tick every 5 seconds (5 days a minute — slow but visible)
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

`task.wait(5)` pauses *this script* for five seconds. It doesn't block other scripts — Roblox runs each script in its own coroutine. A *coroutine* is a function that can pause itself and be resumed later; the runtime quietly switches between them while one is sleeping. Worth knowing the word; you'll see it in the docs.

For a real game, the loop would tick faster (every second, or every 0.1 second), and you'd hook into `RunService.Heartbeat:Connect(function(dt) ... end)` to sync with the frame rate. For learning, `task.wait` is enough.

## Tinker

Increase the tick rate to `task.wait(1)`. The resources change every second now; the place feels noticeably more alive.

Add a third `Lumberyard.new("Eastern Lumberyard")` to the kingdom. Wood starts climbing.

Replace the `while true do` with `RunService.Heartbeat:Connect(function() ... end)` and tick every N frames. Same engine, different scheduler — one of the more satisfying small ports you'll write this year.

Open the C# `Kingdom.cs` and the Luau `Kingdom.lua` side by side in two windows. Read them top to bottom and notice how few lines actually changed.

## What you just did

You took the engine you wrote in Phase 1 and translated it into Luau on a Roblox server. `ResourceLedger`, `Citizen`, and `Kingdom` are now ModuleScripts under `ReplicatedStorage`; a server Script in `ServerScriptService` calls `kingdom:advanceDay()` every five seconds and prints the result. The ports come out shorter than the C# versions because Luau has less ceremony — no namespaces, no `using` directives, no public/private modifiers. The pattern is the proof: the engine doesn't care what runtime it sits in. **Five times now: console, file with JSON and SQLite, web API, browser, Roblox. Five different runtimes; one engine.** That is the point of the whole curriculum, in your hands.

**Key concepts you can now name:**

- *port* — translate to another language while preserving meaning
- *string keys instead of enums* — the Lua idiom for closed sets
- *`task.wait(seconds)`* — pauses one script; doesn't block others
- *coroutine* — a function that can pause and resume; Roblox runs scripts in these
- *`RunService.Heartbeat`* — frame-rate-synced game loop hook

## Words to add to the glossary

- **port** — translate code from one language or runtime into another while keeping its meaning.
- **`task.wait`** — pause the current script for N seconds; other scripts keep running.
- **coroutine** — a function that can pause and be resumed later; the unit Roblox runs scripts in.
- **`RunService.Heartbeat`** — Roblox event that fires every frame; used for frame-rate-synced loops.

## Wrap up

1. **Quiz** — open `quiz.md`, jot your answers in `journal/quiz-notes.md`.
2. **Progress** — one line in `journal/progress.md`: `Module 5.5 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit and push** — stage both files, commit message `Module 5.5 done`, Sync.
4. **Post in `#wins`** — one line about today, plus the URL of the commit.

Module 0.1 covers the why and the panel/CLI steps if you need a refresher. Bring quiz answers you're least sure about to the next weekly sync.

## Next

Module 5.6 builds **the visual world** — a 3D representation of the kingdom in `Workspace`. Click a tile, build a farm; the engine drives the appearance.

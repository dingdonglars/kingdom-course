# Module 5.5 — Engine Port: ResourceLedger, Citizen, Kingdom, Tick

> Travou no inglês? Abra o `lesson.pt.md` — é esta mesma lição em português. Tente em inglês primeiro.

Today the engine does everything it did in Phase 1 — but in Luau, on a Roblox server. `ResourceLedger`, `Citizen`, the `Kingdom` aggregate, the day-tick loop. Same idea, smaller language, a different place to run. By the end of this module the engine works the same way in both versions.

> **Words to watch**
>
> - **port** — translate code into another language while keeping its meaning.
> - **`task.wait(seconds)`** — Roblox's pause-this-script-for-a-while function. Doesn't block other scripts.
> - **`game:GetService("RunService")`** — the service that fires once every frame, used for game loops that need to run at the frame rate.
> - **coroutine** — a function that can pause itself and be resumed later. Roblox runs every script in one of these, which is why `task.wait` doesn't freeze the place.

---

## The port plan

Reading the C# version next to the Luau version is the fastest way to learn the translation:

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

`task.wait(5)` pauses *this script* for five seconds. It doesn't stop other scripts, because Roblox runs each script in its own coroutine. A *coroutine* is a function that can pause itself and start again later. While one script is paused, Roblox quietly switches to the others. It's worth knowing the word, because you'll see it in the docs.

For a real game, the loop would tick faster (every second, or every 0.1 second), and you'd use `RunService.Heartbeat:Connect(function(dt) ... end)` to match the frame rate. For learning, `task.wait` is enough.

## Tinker

Change the tick to `task.wait(1)`. The resources change every second now, and the place feels much more alive.

Add a third building, `Lumberyard.new("Eastern Lumberyard")`, to the kingdom. Wood starts going up.

Replace the `while true do` with `RunService.Heartbeat:Connect(function() ... end)` and tick every N frames. Same engine, different way of timing it — a small, satisfying port.

Open the C# `Kingdom.cs` and the Luau `Kingdom.lua` side by side in two windows. Read them top to bottom and notice how few lines really changed.

## What you just did

You took the engine you wrote in Phase 1 and translated it into Luau on a Roblox server. `ResourceLedger`, `Citizen`, and `Kingdom` are now ModuleScripts under `ReplicatedStorage`. A server Script in `ServerScriptService` calls `kingdom:advanceDay()` every five seconds and prints the result. The Luau version comes out shorter than the C# version because Luau needs less extra code — no namespaces, no `using` lines, no public or private labels. And here is the proof of the whole idea: the engine doesn't care where it runs. **Five times now: console, file with JSON and SQLite, web API, browser, Roblox. Five different places to run; one engine.** That is the point of the whole course, and you just did it.

**Key concepts you can now name:**

- *port* — translate to another language while keeping the meaning
- *string keys instead of enums* — the Lua way to handle a fixed set of values
- *`task.wait(seconds)`* — pauses one script; lets the others keep running
- *coroutine* — a function that can pause and start again; Roblox runs scripts in these
- *`RunService.Heartbeat`* — an event that fires every frame; used for a frame-rate game loop

## On your own

Time to put the book away. Don't scroll back up to the code — prove to yourself, from your own head, that the server loop stuck. No one marks this one — it's just for you. It's the easiest way to spot what *hasn't* stuck yet, while it's still simple to fix. Getting stuck here is completely fine — that's exactly what it's for.

Open a new Script in Studio. Without looking, write the forever loop that:

1. Waits one second.
2. Advances the kingdom one day.
3. Prints the day number.

(You can use a fake kingdom — a plain table with a `day` field — if you don't want to wire up the real engine.) Press Play and watch the day count climb in Output.

<details><summary>Stuck? Open this to check yourself.</summary>

```lua
local kingdom = { day = 1 }

while true do
    task.wait(1)
    kingdom.day = kingdom.day + 1
    print("Day", kingdom.day)
end
```

`task.wait(1)` pauses only this script for one second; the rest of the place keeps running. With the real engine you'd call `kingdom:advanceDay()` in place of the `day + 1` line. Press Stop to end the loop.

</details>

## Words to add to the glossary

- **port** — translate code from one language or runtime into another while keeping its meaning.
- **`task.wait`** — pause the current script for N seconds; other scripts keep running.
- **coroutine** — a function that can pause and start again later; Roblox runs each script in one.
- **`RunService.Heartbeat`** — a Roblox event that fires every frame; used for frame-rate game loops.

## Wrap up

1. **Quiz** — open `quiz.md`, jot your answers in `journal/quiz-notes.md`.
2. **Progress** — one line in `journal/progress.md`: `Module 5.5 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit and push** — stage both files, commit message `Module 5.5 done`, Sync.
4. **Post in `#wins`** — one line about today, plus the URL of the commit.

Module 0.1 covers the why and the panel/CLI steps if you need a refresher. Bring quiz answers you're least sure about to the next weekly sync.

## Next

Module 5.6 builds **the visual world** — a 3D representation of the kingdom in `Workspace`. Click a tile, build a farm; the engine drives the appearance.

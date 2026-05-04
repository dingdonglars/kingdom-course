# Module 5.6 — The Visual World

Today the kingdom appears in the world. A grid of tiles in `Workspace`; click a tile, build a farm; a 3D model spawns on top. The engine drives the appearance — same kingdom that printed to the console in Phase 1, now showing as Parts in Roblox.

> **Words to watch**
>
> - **Part** — the basic 3D building block. Has a shape, a position, a colour, a material.
> - **Model** — a folder of Parts grouped together so they move and select as one.
> - **CFrame** — a Roblox value combining position and orientation. What you set to move and rotate a thing.
> - **`ClickDetector`** — a child you parent to a Part to make it react to mouse clicks.
> - **`Instance.new(class, parent)`** — create a runtime object of the given class and parent it.
> - **`Vector3` / `Color3`** — the 3D vector and RGB colour types.

---

## The pattern

Visual game objects in Roblox are just data — Parts in `Workspace`. Server scripts create them at runtime; Roblox automatically *replicates* them — copies them out to every player's screen — so you don't have to send anything by hand. The recipe for a clickable kingdom is short:

1. Lay out a grid of empty tiles, either on place start or when the player asks for one.
2. Each tile has a `ClickDetector` listening for clicks.
3. On click, the server checks whether the player can afford a Farm (an engine call) and spawns a Part on top of the tile.
4. The kingdom's resources tick on the server every few seconds; the player's UI shows the result.

## Spawning a tile from code

```lua
local function makeTile(position: Vector3): Part
    local p = Instance.new("Part")
    p.Anchored = true
    p.Size = Vector3.new(8, 1, 8)
    p.Position = position
    p.Color = Color3.fromRGB(120, 180, 100)   -- grass green
    p.Material = Enum.Material.Grass
    p.Parent = workspace
    return p
end

-- Make a 5x5 grid
for x = 1, 5 do
    for z = 1, 5 do
        makeTile(Vector3.new(x * 10, 0, z * 10))
    end
end
```

Five lines per tile plus the loop. The script runs once on server start and every connected player sees the grid appear.

## Click handling

```lua
local function onTileClicked(tile: Part, player: Player)
    print(player.Name, "clicked", tile.Position)
    -- engine call: check resources, deduct, spawn farm model
end

-- Attach a ClickDetector to a tile
local detector = Instance.new("ClickDetector")
detector.Parent = tile
detector.MouseClick:Connect(function(player)
    onTileClicked(tile, player)
end)
```

`ClickDetector` works because it's a child of a Part. The connection is made on the server, so the handler runs on the server — no RemoteEvent needed for in-world clicks.

> **One detail in that snippet.** The function passed to `:Connect(...)` is a *closure* — when the click happens later, the function still remembers the `tile` variable from the surrounding `for` loop. Lua captures it automatically. (You'll see this same trick everywhere in event-driven code: define the handler now; the variables it uses are still there when it runs.)

## Spawning a building model

```lua
local function spawnFarm(tile: Part)
    local farm = Instance.new("Part")
    farm.Anchored = true
    farm.Size = Vector3.new(6, 4, 6)
    farm.Color = Color3.fromRGB(150, 100, 50)   -- brown
    farm.Material = Enum.Material.Wood
    farm.Position = tile.Position + Vector3.new(0, 2.5, 0)   -- on top of the tile
    farm.Parent = workspace
end
```

A real farm would be a `Model` made of several Parts, possibly using one of Roblox's free models from the Toolbox (3D models you drag in). For learning, a single brown box reads as "a farm" and gets out of the way.

## Tying engine to visuals

The flow:

```lua
local Kingdom = require(...)
local kingdom = Kingdom.new("Eldoria")
local tileToBuilding = {}    -- map tile Part → engine Building reference

local function tileClicked(tile, player)
    if tileToBuilding[tile] then return end          -- already built
    if not kingdom.resources:spend("Wood", 10) then  -- can the player afford it?
        print(player.Name, "can't afford a farm")
        return
    end
    local farm = Farm.new("Farm @" .. tile.Position.X .. "," .. tile.Position.Z)
    kingdom:addBuilding(farm)
    spawnFarm(tile)
    tileToBuilding[tile] = farm
    print("Built farm; total now:", #kingdom.buildings)
end
```

The engine and the visuals stay in sync via the `tileToBuilding` map. **The engine is the source of truth.** The visual is a projection of the engine's state; if the engine doesn't have a building, the world shouldn't show one.

## Tinker

Make the tiles glow on hover by listening to `ClickDetector.MouseHoverEnter`. Standard UX feedback.

Add a `Sound` instance under `Workspace` and call `sound:Play()` on each click. Audio for free.

Replace the brown box with a free model from the Toolbox (`View → Toolbox`, search "farm"). Drag it in, parent it to `ServerStorage.Templates`, and `:Clone()` it on each spawn instead of using `Instance.new("Part")`.

Add a `BillboardGui` showing the current resource totals floating above the kingdom. UI in 3D space.

## What you just did

You wired the engine to a 3D world. Server code laid out a five-by-five grid of tiles in `Workspace`, gave each tile a `ClickDetector`, and built farms on click — all driven through the engine's `:spend` and `:addBuilding` methods. The `tileToBuilding` map kept the engine and the visible world in sync, with the engine as the authoritative side. The pattern is the lesson: the engine is the source of truth; the visual is the projection. Same engine that printed to the console, that wrote JSON, that served HTTP, that drove the browser DOM — now spawning Parts in Workspace.

**Key concepts you can now name:**

- *Part* — the 3D atom in Roblox; size, position, colour, material
- *Model* — a folder of Parts that select and move as one
- *`Instance.new(class, parent)`* — create a runtime object and place it
- *`ClickDetector`* — child of a Part that fires server-side click events
- *engine as source of truth* — the visual follows; never the other way around

## Words to add to the glossary

- **Part** — the basic 3D building block in Roblox.
- **Model** — a folder of Parts grouped together.
- **CFrame** — a Roblox value combining position and orientation.
- **`Instance.new`** — create a runtime object of a given class.
- **`ClickDetector`** — child of a Part that fires server-side click events.

## Quiz

Open `quiz.md`. When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.

## Next

Module 5.7 introduces **Roblox DataStore** — Roblox's built-in way to persist player data across sessions. Save the kingdom; reload it on next visit.

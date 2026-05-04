# Module 5.6 — The Visual World

> **Hook:** today the kingdom *appears in the world.* A grid of tiles in Workspace; click a tile, build a farm; a 3D model spawns. **The engine drives the appearance** — same kingdom that prints to the console, now showing as Parts.

> **Words to watch**
> - **Part** — the basic 3D building block (`workspace:Part`); shape, position, color
> - **Model** — a folder of Parts grouped together
> - **CFrame** — a Roblox value combining position + orientation; what you set to move things
> - **`ClickDetector`** — child of a Part that fires when clicked
> - **`Instance.new(class, parent)`** — create a runtime object

---

## The pattern

Visual game objects in Roblox are **just data** — Parts in Workspace. Server scripts create them at runtime; Roblox replicates to clients automatically.

For the kingdom:
1. Lay out a grid of empty tiles (or have them appear on a build action).
2. Each tile has a `ClickDetector` listening for clicks.
3. On click → server checks if the player can afford a Farm (engine call) → spawns a Part on top of the tile.
4. The kingdom's resources tick on the server every N seconds; the player's UI shows the result.

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

Five lines per tile + the loop. Run once on server start; every player sees the grid.

## Click handling

```lua
local function onTileClicked(tile: Part, player: Player)
    print(player.Name, "clicked", tile.Position)
    -- Engine call: check resources, deduct, spawn farm model
    -- (RemoteEvent would be the cleaner pattern; for click in Workspace we can stay server-side)
end

-- Attach to every tile
local detector = Instance.new("ClickDetector")
detector.Parent = tile
detector.MouseClick:Connect(function(player)
    onTileClicked(tile, player)
end)
```

`ClickDetector` works because it's a child of a Part. Server-side connection means the handler runs on the server (no RemoteEvent needed for in-world clicks).

## Spawning a building model

```lua
local function spawnFarm(tile: Part)
    local farm = Instance.new("Part")
    farm.Anchored = true
    farm.Size = Vector3.new(6, 4, 6)
    farm.Color = Color3.fromRGB(150, 100, 50)   -- brown
    farm.Material = Enum.Material.Wood
    farm.Position = tile.Position + Vector3.new(0, 2.5, 0)   -- on top of tile
    farm.Parent = workspace
end
```

A real farm would be a `Model` of multiple Parts, possibly using one of Roblox's free assets from Toolbox (3D models you can drag in). For learning, a single brown box reads as "a farm."

## Tying engine to visuals

The flow:

```lua
local Kingdom = require(...)
local kingdom = Kingdom.new("Eldoria")
local tileToBuilding = {}    -- map tile Part → engine Building reference

local function tileClicked(tile, player)
    if tileToBuilding[tile] then return end          -- already built
    if not kingdom.resources:spend("Wood", 10) then  -- can the player afford?
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

The engine and the visual are kept in sync via `tileToBuilding` map. **The engine is the source of truth.** The visual is the projection.

## Delta starter

- `roblox-kingdom/scripts/server/world.lua` — grid + click + spawn
- `roblox-kingdom/scripts/server/main.lua` — extended to integrate

(Roblox 3D scenes are visual-first — there's no .lua-only way to ship the appearance. The starter scripts produce the visuals at runtime.)

## Tinker

- Make tiles glow on hover: `ClickDetector.MouseHoverEnter:Connect(...)`. Standard UX feedback.
- Add a `Sound` instance to the Workspace; play it on each click (`sound:Play()`). Audio for free.
- Replace the brown box with a free model from Toolbox (View → Toolbox → search "farm"). Drag in; insert as child of `ServerStorage.Templates`; `:Clone()` per spawn.
- Add a `BillboardGui` showing current resources floating over the kingdom. UI in 3D space.

## Name it

- **Part** — the 3D atom.
- **Model** — group of Parts.
- **CFrame** — position + orientation.
- **`Instance.new(class, parent)`** — create a runtime object.
- **`ClickDetector`** — fires server-side click events.
- **Vector3 / Color3** — 3D vector + RGB color types.

## The rule of the through-line

> **The engine is the source of truth; the visual is its projection.** Same engine that ran the console prints, the JSON serialiser, the API, the browser cards — now spawns Parts. The shape of "given engine state, show this" never changes.

## Quiz / challenge

Open `quiz.md`.

## Connect

Module 5.7 introduces **Roblox DataStore** — the platform's way to persist player data across sessions. Save the kingdom; reload it on next visit.
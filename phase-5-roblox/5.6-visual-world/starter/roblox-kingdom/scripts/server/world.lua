-- ServerScriptService/World

local Engine = game.ReplicatedStorage:WaitForChild("Engine")
local Kingdom = require(Engine.Kingdom)
local Farm = require(Engine.Farm)

local kingdom = Kingdom.new("Eldoria")
local tileToBuilding: { [Part]: any } = {}

local function makeTile(position: Vector3): Part
    local p = Instance.new("Part")
    p.Anchored = true
    p.Size = Vector3.new(8, 1, 8)
    p.Position = position
    p.Color = Color3.fromRGB(120, 180, 100)
    p.Material = Enum.Material.Grass
    p.Parent = workspace
    return p
end

local function spawnFarm(tile: Part)
    local farm = Instance.new("Part")
    farm.Anchored = true
    farm.Size = Vector3.new(6, 4, 6)
    farm.Color = Color3.fromRGB(150, 100, 50)
    farm.Material = Enum.Material.Wood
    farm.Position = tile.Position + Vector3.new(0, 2.5, 0)
    farm.Parent = workspace
end

local function tileClicked(tile: Part, player: Player)
    if tileToBuilding[tile] then return end
    if not kingdom.resources:spend("Wood", 10) then
        print(player.Name, "can't afford a farm")
        return
    end
    local farm = Farm.new(string.format("Farm@%d,%d", tile.Position.X, tile.Position.Z))
    kingdom:addBuilding(farm)
    spawnFarm(tile)
    tileToBuilding[tile] = farm
    print("Built farm; total now:", #kingdom.buildings)
end

-- Build the grid + wire clicks
for x = 1, 5 do
    for z = 1, 5 do
        local tile = makeTile(Vector3.new(x * 10, 0, z * 10))
        local detector = Instance.new("ClickDetector")
        detector.Parent = tile
        detector.MouseClick:Connect(function(player)
            tileClicked(tile, player)
        end)
    end
end

-- Tick loop (same as Module 5.5)
while true do
    task.wait(5)
    kingdom:advanceDay()
    print(string.format(
        "Day %d — Wood:%d  Buildings:%d",
        kingdom.day, kingdom.resources:get("Wood"), #kingdom.buildings))
end

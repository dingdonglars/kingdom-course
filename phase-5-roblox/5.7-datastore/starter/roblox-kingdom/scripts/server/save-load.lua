-- ServerScriptService/SaveLoad

local DataStoreService = game:GetService("DataStoreService")
local Players = game:GetService("Players")
local store = DataStoreService:GetDataStore("Kingdoms")

local Engine = game.ReplicatedStorage:WaitForChild("Engine")
local Kingdom = require(Engine.Kingdom)

local kingdoms: { [number]: any } = {}

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
    end
    if snapshot and Kingdom.fromSnapshot then
        return Kingdom.fromSnapshot(snapshot)
    end
    return Kingdom.new(player.Name .. "'s Kingdom")
end

local function saveKingdom(player: Player, kingdom: any)
    if not kingdom.toSnapshot then return end
    local snapshot = kingdom:toSnapshot()
    local ok, err = pcall(function()
        store:SetAsync(key(player), snapshot)
    end)
    if not ok then warn("Failed to save:", err) end
end

Players.PlayerAdded:Connect(function(player)
    kingdoms[player.UserId] = loadKingdom(player)
    print(player.Name, "kingdom loaded; day", kingdoms[player.UserId].day)
end)

Players.PlayerRemoving:Connect(function(player)
    if kingdoms[player.UserId] then
        saveKingdom(player, kingdoms[player.UserId])
        kingdoms[player.UserId] = nil
    end
end)

game:BindToClose(function()
    for _, player in ipairs(Players:GetPlayers()) do
        if kingdoms[player.UserId] then
            saveKingdom(player, kingdoms[player.UserId])
        end
    end
end)

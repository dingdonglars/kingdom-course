-- ServerScriptService/MainLoop

local Engine = game.ReplicatedStorage:WaitForChild("Engine")
local Kingdom = require(Engine.Kingdom)
local Farm = require(Engine.Farm)
local Mine = require(Engine.Mine)
local Lumberyard = require(Engine.Lumberyard)
local Citizen = require(Engine.Citizen)

local kingdom = Kingdom.new("Eldoria")
kingdom:addBuilding(Farm.new("Main Farm"))
kingdom:addBuilding(Lumberyard.new("Eastern Lumberyard"))
kingdom:addBuilding(Mine.new("Old Mine"))
kingdom:addCitizen(Citizen.new("Lyra"))

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

-- Smoke test — paste into ServerScriptService/Script

local Engine = game.ReplicatedStorage:WaitForChild("Engine")
local Building = require(Engine.Building)
local Farm = require(Engine.Farm)
local Mine = require(Engine.Mine)

-- A tiny inline ledger
local ledger = {
    amounts = { Food = 0, Wood = 0, Stone = 0, Gold = 0 },
}
function ledger:add(resource: string, amount: number)
    self.amounts[resource] = self.amounts[resource] + amount
end

-- Build some buildings
local farm = Farm.new("Main Farm")
local mine = Mine.new("Old Mine")
mine:upgrade()    -- level 2

farm:tick(ledger)
mine:tick(ledger)

print(string.format("Farm '%s' level %d", farm.name, farm.level))
print(string.format("Mine '%s' level %d", mine.name, mine.level))
print("Ledger:", ledger.amounts.Food, ledger.amounts.Wood, ledger.amounts.Stone)
-- Expected: Food=5, Wood=0, Stone=4

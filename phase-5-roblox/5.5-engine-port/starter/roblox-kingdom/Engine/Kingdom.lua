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
    for _, b in ipairs(self.buildings) do
        b:tick(self.resources)
    end
    for _ in ipairs(self.citizens) do
        self.resources:spend("Food", 1)
    end
    self.day = self.day + 1
end

return Kingdom

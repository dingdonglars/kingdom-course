local Building = require(script.Parent.Building)

local Lumberyard = setmetatable({}, { __index = Building })
Lumberyard.__index = Lumberyard

function Lumberyard.new(name: string)
    local self = Building.new(name)
    setmetatable(self, Lumberyard)
    return self
end

function Lumberyard:tick(ledger: any)
    ledger:add("Wood", 3 * self.level)
end

return Lumberyard

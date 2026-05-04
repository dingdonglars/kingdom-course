local Building = require(script.Parent.Building)

local Mine = setmetatable({}, { __index = Building })
Mine.__index = Mine

function Mine.new(name: string)
    local self = Building.new(name)
    setmetatable(self, Mine)
    return self
end

function Mine:tick(ledger: any)
    ledger:add("Stone", 2 * self.level)
end

return Mine

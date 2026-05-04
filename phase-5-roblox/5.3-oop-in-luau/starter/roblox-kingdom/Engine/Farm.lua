local Building = require(script.Parent.Building)

local Farm = setmetatable({}, { __index = Building })
Farm.__index = Farm

function Farm.new(name: string)
    local self = Building.new(name)
    setmetatable(self, Farm)
    return self
end

function Farm:tick(ledger: any)
    ledger:add("Food", 5 * self.level)
end

return Farm

local ResourceLedger = {}
ResourceLedger.__index = ResourceLedger

function ResourceLedger.new()
    local self = setmetatable({}, ResourceLedger)
    self.amounts = { Gold = 0, Wood = 0, Stone = 0, Food = 0 }
    return self
end

function ResourceLedger:get(resource: string): number
    return self.amounts[resource] or 0
end

function ResourceLedger:add(resource: string, amount: number)
    if amount < 0 then error("Use spend for negatives") end
    self.amounts[resource] = (self.amounts[resource] or 0) + amount
end

function ResourceLedger:spend(resource: string, amount: number): boolean
    if amount < 0 then error("Spend amount must be non-negative") end
    local have = self.amounts[resource] or 0
    if have < amount then return false end
    self.amounts[resource] = have - amount
    return true
end

function ResourceLedger:snapshot(): { [string]: number }
    local out = {}
    for k, v in pairs(self.amounts) do out[k] = v end
    return out
end

return ResourceLedger

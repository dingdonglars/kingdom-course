local Building = {}
Building.__index = Building

function Building.new(name: string)
    local self = setmetatable({}, Building)
    self.name = name
    self.level = 1
    return self
end

function Building:upgrade()
    self.level = self.level + 1
end

function Building:tick(_ledger: any)
    -- default: no production
end

return Building

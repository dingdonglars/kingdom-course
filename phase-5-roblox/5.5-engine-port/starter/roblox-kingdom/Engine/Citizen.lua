local Citizen = {}
Citizen.__index = Citizen

function Citizen.new(name: string)
    local self = setmetatable({}, Citizen)
    self.name = name
    return self
end

return Citizen

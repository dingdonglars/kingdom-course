-- Luau basics — paste into a Script in ServerScriptService

-- variables
local x: number = 10
local name: string = "Athos"
local active: boolean = true
print(x, name, active)

-- function
local function greet(who: string)
    print("Hello, " .. who)
end
greet(name)

-- conditional
if x > 5 then
    print("x is big")
elseif x > 0 then
    print("x is small but positive")
else
    print("x is zero or negative")
end

-- numeric loop (1-indexed!)
for i = 1, 5 do
    print("i =", i)
end

-- table-as-array
local resources = { "Gold", "Wood", "Stone", "Food" }
print("First resource:", resources[1])
print("Length:", #resources)

for index, value in ipairs(resources) do
    print(index, "=", value)
end

-- table-as-dict
local kingdom = { name = "Eldoria", day = 11 }
print(kingdom.name, "is on day", kingdom.day)

for key, value in pairs(kingdom) do
    print(key, "=", value)
end

-- mutate
table.insert(resources, "Marble")
print("After insert:", #resources)

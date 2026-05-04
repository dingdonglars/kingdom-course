-- Server: ServerScriptService/Script

local event = game.ReplicatedStorage:WaitForChild("Events"):WaitForChild("TickRequest")

event.OnServerEvent:Connect(function(player: Player, days: any)
    local n = math.clamp(typeof(days) == "number" and days or 1, 1, 100)
    print(player.Name, "asked for", n, "ticks (validated)")
    -- Here we'd advance the player's kingdom, save, etc.
    -- Reply on the client side:
    event:FireClient(player, "ticked " .. n .. " days")
end)

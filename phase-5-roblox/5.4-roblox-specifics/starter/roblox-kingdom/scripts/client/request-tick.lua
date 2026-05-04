-- Client: StarterPlayerScripts/LocalScript

local event = game.ReplicatedStorage:WaitForChild("Events"):WaitForChild("TickRequest")

-- Listen for replies
event.OnClientEvent:Connect(function(message: string)
    print("Server replied:", message)
end)

-- Send a request after 2 seconds
task.wait(2)
event:FireServer(5)

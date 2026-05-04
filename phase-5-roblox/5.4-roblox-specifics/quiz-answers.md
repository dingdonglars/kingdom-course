# Quiz answers — Module 5.4

## 1. b
The server is the single source of truth. Authority for resources, building levels, day counter, save state — all server. Clients send requests via RemoteEvents; server validates + applies. Otherwise players can cheat by editing their local state.

## 2. b
RemoteEvents are the network bridge. `event:FireServer(args)` from a client; `event.OnServerEvent:Connect(fn)` on the server. Async one-way; for request/response use RemoteFunction.

## 3. a
Anything from the client can be tampered with. A modded Roblox client could fire `TickRequest:FireServer(99999)`. The server must clamp, validate, refuse — same discipline as your Web API in Block 5.

## 4. c
Server scripts can `require` from `ReplicatedStorage`; client scripts can too. RemoteEvents typically live there as well so both sides can reference them by path. Server-only stuff goes in `ServerStorage`.

## 5. a
One source of truth = no contradictions. The server tells every client "the kingdom is at day 11" and they all agree. The split makes networked games coherent. Skipping it = "Player 1 says I have 100 gold but the leaderboard says 50."
# Module 5.4 starter — Roblox specifics

Manual Studio setup:
1. Insert a `Folder` in `ReplicatedStorage` named `Events`.
2. Inside `Events`, insert a `RemoteEvent` named `TickRequest`.
3. In `ServerScriptService`, insert a `Script`; paste `scripts/server/handle-tick.lua`.
4. In `StarterPlayerScripts`, insert a `LocalScript`; paste `scripts/client/request-tick.lua`.
5. Test → Local Server → 2 players. Watch the Output of each.
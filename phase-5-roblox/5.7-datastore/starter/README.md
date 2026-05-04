# Module 5.7 starter — DataStore

Studio:
- Game Settings → Security → enable "Studio Access to API Services" (so DataStore works in test)
- Insert a Script in `ServerScriptService` named `SaveLoad`; paste `scripts/server/save-load.lua`
- Engine: extend `Kingdom.lua` with `:toSnapshot()` and `.fromSnapshot()` (sketch in lesson)

Test: play as one user; build farms; quit; rejoin; the kingdom comes back.
# Quiz answers — Module 5.7

## 1. a
DataStore is server-only. Roblox enforces it: a LocalScript that calls `DataStoreService:GetDataStore(...)` will get a no-op or error. The reason: persistence is authority's job — letting clients write directly would be a free duplication exploit.

## 2. b
`pcall(fn)` calls `fn` and catches errors. Returns `(true, returnValue)` if it ran cleanly, `(false, errorMessage)` if `fn` threw. The Lua equivalent of try/catch. Wrap every DataStore call — they can fail on transient network issues.

## 3. a
When Roblox restarts a server (deploys, crashes, scaling), `BindToClose` runs first. Without it, the last 5 minutes of player progress are lost. Roblox waits ~30 seconds for your handler to finish — plenty of time to save a few snapshots.

## 4. b
DataStore has quotas (~60 calls/min/server, 4MB/key). Saving every tick blows the budget. Save on `PlayerRemoving`, on `BindToClose`, and on a 5-minute timer. Recovery is "load from last save"; minor data loss is acceptable.

## 5. a
Same shape, every block. Snapshot → store → load → rehydrate. The medium changes (file → JSON → SQLite → EF Core → DataStore); the discipline doesn't. By M5.7 you've internalised the pattern.
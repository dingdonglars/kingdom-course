# Module 5.5 starter — engine port (parity with Phase 1)

The engine reaches feature parity with Phase 1, this time on a Roblox server. `ResourceLedger`, `Citizen`, and the `Kingdom` aggregate are now ModuleScripts; a server Script ticks the kingdom every five seconds.

## What's in here

- `roblox-kingdom/Engine/ResourceLedger.lua` — the resource book of record.
- `roblox-kingdom/Engine/Citizen.lua` — minimal citizen, just a name.
- `roblox-kingdom/Engine/Kingdom.lua` — the aggregate; owns buildings, citizens, resources, and the day counter.
- `roblox-kingdom/scripts/server/main.lua` — the game loop. Builds a kingdom, ticks it every five seconds, prints the state.

## How to use it

In Studio, with the Module 5.3 setup already in place (`Building`, `Farm`, `Lumberyard`, `Mine` already as ModuleScripts under `ReplicatedStorage/Engine`):

1. Add three new ModuleScripts under `ReplicatedStorage/Engine`: `ResourceLedger`, `Citizen`, `Kingdom`. Paste each `.lua` into the matching ModuleScript.
2. Insert a `Script` under `ServerScriptService` named `MainLoop`. Paste `scripts/server/main.lua` into it.
3. Hit Play. Output ticks every five seconds with the day counter and the four resource totals.

## Common gotcha

The order in which `require` calls happen matters. `Kingdom.lua` requires `Building` and `ResourceLedger`, so both ModuleScripts have to exist with the right names before the kingdom can load. If the loop log says *attempt to call require with nil*, one of the dependencies is missing or misnamed in the Explorer.

The other thing to watch: the `while true do` loop blocks the script thread (in its own coroutine). If you forget the `task.wait(5)` inside the loop, the script runs the loop body without pausing — Roblox kills the script after a few seconds with a script-timed-out warning.

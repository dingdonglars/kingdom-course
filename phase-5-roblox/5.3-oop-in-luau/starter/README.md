# Module 5.3 starter — OOP in Luau

A mini-port of the Phase 1 engine: `Building`, `Farm`, `Lumberyard`, `Mine`, plus a small smoke-test script.

## What's in here

- `roblox-kingdom/Engine/Building.lua` — the base class, written with the metatable recipe.
- `roblox-kingdom/Engine/Farm.lua`, `Lumberyard.lua`, `Mine.lua` — three subclasses; each overrides `tick`.
- `roblox-kingdom/scripts/test-engine.lua` — a script that imports the modules, builds a few instances, and prints results.

## How to use it

In Studio:

1. In the Explorer, insert a `Folder` under `ReplicatedStorage` and name it `Engine`.
2. Inside that folder, insert four `ModuleScript` objects named `Building`, `Farm`, `Lumberyard`, and `Mine`.
3. Open each `.lua` file from `roblox-kingdom/Engine/`, copy the contents, paste into the matching ModuleScript.
4. Insert a `Script` under `ServerScriptService` and paste `test-engine.lua` into it.
5. Hit Play. The Output panel shows the smoke-test results.

## Common gotcha

Two filenames need to match exactly. The ModuleScript name in the Explorer is the name `require` looks for — if the folder script does `require(game.ReplicatedStorage.Engine.Farm)` and the ModuleScript is called `farm` (lowercase), `require` returns `nil` and the next line dies with an obscure error. Capitalise the ModuleScript names exactly as written.

The other gotcha is colon vs dot. If the test script logs `attempt to index nil` on a method call, you almost certainly wrote `farm.upgrade()` instead of `farm:upgrade()`.

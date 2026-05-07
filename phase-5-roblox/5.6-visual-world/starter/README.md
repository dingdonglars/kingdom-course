# Module 5.6 starter — visual world

A click-to-build five-by-five grid on a Roblox server. Tiles spawn from code; clicking a tile builds a farm if the player can afford it; the engine stays the source of truth.

## What's in here

- `roblox-kingdom/scripts/server/world.lua` — generates the grid, attaches `ClickDetector`s, spawns farms on click. Uses the `tileToBuilding` map to keep engine and visuals in sync.
- The script extends the Module 5.5 `main.lua` setup; both are server-side.

## How to use it

In Studio, with the Module 5.5 setup already in place:

1. Insert a `Script` under `ServerScriptService` named `World`. Paste `scripts/server/world.lua` into it.
2. Hit Play. The Viewport shows a five-by-five grid of green tiles on the baseplate.
3. Click any tile. If the kingdom has at least 10 Wood, a brown farm Part spawns on top.
4. Watch the Output panel for the engine's print logs alongside the visual changes.

## Common gotcha

`ClickDetector` only fires if the script that connected the handler is server-side. If the click does nothing in your tests, check that the script is in `ServerScriptService`, not `StarterPlayerScripts`.

The other gotcha: every Part in the world starts with `Anchored = false` by default, which means physics tries to apply to your tiles and they fall through the floor. Always set `tile.Anchored = true` before parenting it to `workspace`. The starter does this; if you write your own version, that's the line you'll forget once.

## If you want prettier visuals

Open `View → Toolbox` in Studio, search for "farm" or "house", drag a free model into `ServerStorage` under a folder called `Templates`. In your script, replace `Instance.new("Part")` with `template:Clone()` and parent the clone to `workspace`. The model carries its own visuals; the rest of the script stays the same.

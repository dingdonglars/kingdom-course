# Module 5.1 — Roblox Studio Tour

> **Hook:** Roblox is the third-largest game platform on Earth. Your friends are already there. **Roblox Studio** is the editor — free, downloads in 2 minutes. Today we install it, open the default world, click around, and run our first script. **No code yet** — just the tour, so the next 8 modules have a frame.

> **Words to watch**
> - **Roblox Studio** — the editor (Windows/Mac); free
> - **Place** — one Roblox world; what you publish
> - **Workspace** — the 3D scene; everything you can see + touch
> - **Explorer** — Studio's tree view of every object in the place
> - **Properties panel** — inspector for the selected object
> - **Output panel** — Studio's console; print statements + errors land here

---

## Install

1. Go to <https://create.roblox.com/>
2. Click "Start Creating" → it'll prompt to install Roblox Studio (~150MB).
3. Sign in with your Roblox account (create one if needed).
4. Once installed, double-click "Studio" to launch.

## The 4-tab layout

Open the **Baseplate** template. You see:

- **Viewport** (center): the 3D world. Default = a flat green plate.
- **Explorer** (right): the tree of every object — `Workspace`, `Players`, `Lighting`, `ReplicatedStorage`, `ServerScriptService`, etc.
- **Properties** (right, below Explorer): selected object's properties — Name, Size, Material, etc.
- **Output** (bottom, may need to be opened from View menu): the console.

**Open Output before doing anything else.** It's where errors and `print(...)` calls land.

## Your first script (no code, just where it lives)

In Explorer:
1. Right-click `ServerScriptService` → Insert Object → Script.
2. The new `Script` opens in a tab. Default content:
   ```lua
   print("Hello world!")
   ```
3. Click the green **Play** button at the top.
4. The output panel shows: `Hello world!`.

That's the one-line sanity check. Studio plays your code in a simulated session; no publishing needed.

## What lives where

Just memorise this for now:

- **`Workspace`** — 3D parts/models the player sees + touches.
- **`Players`** — runtime: every connected player.
- **`ServerScriptService`** — server-side scripts (run once on the game server; not visible to clients).
- **`ReplicatedStorage`** — shared between server + client; data + modules both can use.
- **`StarterPlayerScripts`** — scripts that run on every player's machine when they join.
- **`StarterPack`** — items every player gets in their backpack on spawn.

Server vs client is huge in Roblox — a deeper module covers it (5.4). For today: scripts in `ServerScriptService` run on the server.

## Studio shortcuts you'll use daily

| Shortcut | Effect |
|---|---|
| Right-click Explorer object → Insert | Add child |
| F (with object selected in Viewport) | Focus camera |
| W/A/S/D + mouse | Fly camera (when right-click held) |
| Ctrl+S | Save place locally |
| Play button | Test in Studio |
| Stop button | End test |

## Publish path (later, M5.9)

`File → Publish to Roblox` creates a place ID. Friends visit `roblox.com/games/<id>/<name>` and play. Save a draft today; we'll publish at M5.9.

## Delta starter

This module ships:

- `roblox-kingdom/README.md` — placeholder for your Roblox project setup notes
- `journal/5.1-studio-tour.md` — your notes from clicking around

No code in the curriculum-side starter; the actual Studio file (`.rbxl`) lives in your local Roblox install.

## Tinker

- Insert a `Part` in Workspace. Drag it around. Change its `Size`, `Color`, `Material`.
- Right-click a Part → Group as Model. **Models are folders for parts.** Useful when you have a "house" of 20 parts.
- Press F5 (Test in player). The character spawns; walk around.
- Use the Output panel — type `print("test")` in the Output's command line and hit Enter.

## Name it

- **Studio** — the editor. Free. Windows/Mac.
- **Place** — one Roblox world.
- **Workspace** — the live 3D scene.
- **Explorer** — the tree of all objects.
- **`ServerScriptService`** — server-side script home.
- **Output panel** — console.

## The rule of the through-line

> **Same instinct, different tool.** Visual Studio Code → Roblox Studio is a new editor; the work is still "find the file, edit it, run it, read the output." Don't get intimidated by the visual interface — it's the same loop.

## Quiz / challenge

Open `quiz.md`.

## Connect

Module 5.2 introduces **Luau** — Lua-with-types. Looks like JavaScript and Python had a child. Your engine port begins here.
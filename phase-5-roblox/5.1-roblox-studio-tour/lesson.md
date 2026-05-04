# Module 5.1 — Roblox Studio Tour

Roblox is one of the largest game platforms in the world, and your friends are already there. **Roblox Studio** is the editor for it — free, downloads in a couple of minutes. Today we install it, open the default world, click around the panels, and run our first script. No real code yet — the goal is to know where things live, so the next seven modules don't waste their first ten minutes on the geography.

> **Words to watch**
>
> - **Roblox Studio** — the editor. Windows or Mac, free.
> - **Place** — one Roblox world. What you publish.
> - **Workspace** — the live 3D scene. Everything visible and touchable.
> - **Explorer** — Studio's tree view of every object in the place.
> - **Properties panel** — the inspector for whatever you've selected.
> - **Output panel** — Studio's console. `print(...)` and runtime errors land here.

---

## Step 1 — install

Go to <https://create.roblox.com/> and click *Start Creating*. The page prompts you to install Roblox Studio (about 150 MB). Sign in with your Roblox account, or create one. Once it's installed, double-click *Studio* to launch.

## Step 2 — open the Baseplate

When Studio starts you pick a template. Choose **Baseplate** — the simplest one, a flat green plate floating in the air. Once it's open, you should see four areas of the screen:

- The **Viewport** in the centre — the 3D world, with the green plate sitting in it.
- The **Explorer** on the right — a tree of every object in the place: `Workspace`, `Players`, `Lighting`, `ReplicatedStorage`, `ServerScriptService`, and a handful more.
- The **Properties panel** below Explorer — when you click an object, its fields show here. Name, Size, Material, Color.
- The **Output panel** at the bottom — Studio's console. If it isn't visible, open it from *View → Output*.

Open the Output panel before you do anything else. Errors and `print` calls land there, and you'll want it visible the moment something runs.

## Step 3 — your first script

In the Explorer, right-click `ServerScriptService` and choose *Insert Object → Script*. A new tab opens with one line:

```lua
print("Hello world!")
```

Click the green **Play** button at the top of Studio. The Output panel shows:

```
Hello world!
```

That's the one-line sanity check. Studio runs your code in a simulated session — no publishing, no upload, just a local test of what would happen if a player joined. Click *Stop* when you've seen the output.

## What lives where (memorise this much)

Roblox places have a lot of folders. The ones that matter for the next seven modules:

- **`Workspace`** — the 3D parts and models the player sees and touches.
- **`Players`** — the runtime list of every connected player.
- **`ServerScriptService`** — server-side scripts. They run once on the game server when the place starts. Not visible to clients.
- **`ReplicatedStorage`** — shared between server and client. Engine modules go here so both halves can use them.
- **`StarterPlayerScripts`** — scripts that run on every player's machine when they join.
- **`StarterPack`** — items every player gets in their backpack on spawn.

The split between server and client is the single biggest idea in Roblox, and it gets its own module (5.4). Today's working knowledge: scripts in `ServerScriptService` run on the server.

## Studio shortcuts you'll use daily

| Shortcut | What it does |
| --- | --- |
| Right-click an Explorer object → Insert | Add a child object |
| F (with a part selected in the Viewport) | Focus the camera on it |
| Right-click held + WASD + mouse | Fly the camera around |
| Ctrl+S | Save the place locally |
| Play button | Test inside Studio |
| Stop button | End the test |

## Tinker

Insert a Part into Workspace from the Explorer. Drag it around the green plate. Open the Properties panel and change its `Size`, `Color`, and `Material` and watch the Part change in the Viewport. Right-click the Part and choose *Group as Model* — a Model is just a folder of Parts grouped together, useful when you have a building made of twenty pieces and want to move them as one.

Press F5 to run a player test. A character spawns and you can walk around with WASD. While the test is running, type `print("test")` directly into the Output panel's command line at the bottom and hit Enter.

The four-panel layout is what every Roblox tutorial assumes you can find. Spend ten minutes clicking around until the words *Viewport, Explorer, Properties, Output* are reflexes.

## Publishing comes later

`File → Publish to Roblox` is how a place becomes a URL your friends can visit. We're not pressing that button today — M5.8 is the publishing module. For now, a saved local draft is enough.

## What you just did

You installed Roblox Studio and ran your first script in it. You met the four panels every Roblox tutorial assumes you can find — Viewport, Explorer, Properties, Output — and saw a one-line script print into the console. You also met the folder geography that the next seven modules build on: `Workspace` for the 3D scene, `ServerScriptService` for server code, `ReplicatedStorage` for code that both sides can read, and a couple of others. Nothing today required typing real Luau; that starts tomorrow. The point of today was to make Studio feel familiar before the lessons start asking you to do real work in it.

**Key concepts you can now name:**

- *Roblox Studio* — the free editor for Roblox places
- *the four panels* — Viewport, Explorer, Properties, Output
- *`ServerScriptService`* — where server-side scripts live
- *`ReplicatedStorage`* — shared code home for server and client
- *Place* — one Roblox world; what gets published

## Words to add to the glossary

- **Roblox Studio** — the free editor for building Roblox places.
- **Place** — a single Roblox game world.
- **Workspace** — the live 3D scene every player sees.
- **Explorer** — Studio's tree view of every object in the place.
- **Output panel** — Studio's console; `print` and errors land here.

## Quiz

Open `quiz.md`. When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.

## Next

Module 5.2 introduces **Luau** — Lua with optional types. It looks like JavaScript and Python had a child. Your engine port begins there.

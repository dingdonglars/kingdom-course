# Module 5.1 — Roblox Studio Tour

> Travou no inglês? Abra o `lesson.pt.md` — é esta mesma lição em português. Tente em inglês primeiro.

Roblox is one of the biggest game platforms in the world, and your friends are already there. **Roblox Studio** is the editor for it. It is free and downloads in a couple of minutes. Today we install it, open the default world, click around the panels, and run our first script. No real code yet. The goal is to learn where everything is, so the next seven modules don't waste their first ten minutes on it.

> **Words to watch**
>
> - **Roblox Studio** — the editor. Windows or Mac, free.
> - **Place** — one Roblox world. What you publish.
> - **Workspace** — the live 3D scene. Everything visible and touchable.
> - **Explorer** — Studio's tree view of every object in the place.
> - **Properties panel** — the inspector for whatever you've selected.
> - **Output panel** — Studio's console. `print(...)` and runtime errors show up here.

---

## Phase opener — `phase-5` branch

Before any code (Module 1.1 explains why):

```powershell
cd C:\code\kingdom
git switch -c phase-5
```

Every commit this phase goes on `phase-5`. At Module 5.8 (the M6 close, the last one), you'll open a pull request to bring it back to `main`.

---

## Step 1 — install

Go to <https://create.roblox.com/> and click *Start Creating*. The page prompts you to install Roblox Studio (about 150 MB). Sign in with your Roblox account, or create one. Once it's installed, double-click *Studio* to launch.

## Step 2 — open the Baseplate

When Studio starts you pick a template. Choose **Baseplate** — the simplest one, a flat green plate floating in the air. Once it's open, you should see four areas of the screen:

- The **Viewport** in the centre — the 3D world, with the green plate in it.
- The **Explorer** on the right — a tree of every object in the place: `Workspace`, `Players`, `Lighting`, `ReplicatedStorage`, `ServerScriptService`, and a few more.
- The **Properties panel** below Explorer — when you click an object, its fields show here. Name, Size, Material, Color.
- The **Output panel** at the bottom — Studio's console. If it isn't visible, open it from *View → Output*.

Open the Output panel before you do anything else. Errors and `print` calls show up there, and you'll want it visible the moment something runs.

## Step 3 — your first script

In the Explorer, right-click `ServerScriptService` and choose *Insert Object → Script*. A new tab opens with one line:

```lua
print("Hello world!")
```

Click the green **Play** button at the top of Studio. The Output panel shows:

```
Hello world!
```

That's the one-line check that everything works. Studio runs your code in a pretend session. Nothing is published and nothing is uploaded. It's just a local test of what would happen if a player joined. Click *Stop* when you've seen the output.

## What lives where (learn this much)

Roblox places have a lot of folders. These are the ones that matter for the next seven modules:

- **`Workspace`** — the 3D parts and models the player sees and touches.
- **`Players`** — the live list of every connected player.
- **`ServerScriptService`** — server-side scripts. They run once on the game server when the place starts. Players can't see them.
- **`ReplicatedStorage`** — shared between server and client. Engine modules go here so both sides can use them.
- **`StarterPlayerScripts`** — scripts that run on every player's machine when they join.
- **`StarterPack`** — items every player gets in their backpack when they spawn.

The split between server and client is the biggest idea in Roblox, and it gets its own module (5.4). For today, you only need to know one thing: scripts in `ServerScriptService` run on the server.

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

Insert a Part into Workspace from the Explorer. Drag it around the green plate. Open the Properties panel and change its `Size`, `Color`, and `Material`, then watch the Part change in the Viewport. Right-click the Part and choose *Group as Model*. A Model is just a folder of Parts grouped together. It's useful when you have a building made of twenty pieces and want to move them all at once.

Press F5 to run a player test. A character spawns and you can walk around with WASD. While the test is running, type `print("test")` directly into the Output panel's command line at the bottom and press Enter.

Most Roblox tutorials expect you to know where these four panels are. Spend ten minutes clicking around until the words *Viewport, Explorer, Properties, Output* feel familiar.

## Publishing comes later

`File → Publish to Roblox` is how a place becomes a URL your friends can visit. We're not pressing that button today. Module 5.8 is the publishing module. For now, a saved local draft is enough.

## What you just did

You installed Roblox Studio and ran your first script in it. You met the four panels most Roblox tutorials expect you to know — Viewport, Explorer, Properties, Output — and saw a one-line script print into the console. You also met the folders that the next seven modules build on: `Workspace` for the 3D scene, `ServerScriptService` for server code, `ReplicatedStorage` for code that both sides can read, and a couple of others. Nothing today needed real Luau. That starts tomorrow. The point of today was to make Studio feel familiar before the lessons start asking you to do real work in it.

**Key concepts you can now name:**

- *Roblox Studio* — the free editor for Roblox places
- *the four panels* — Viewport, Explorer, Properties, Output
- *`ServerScriptService`* — where server-side scripts live
- *`ReplicatedStorage`* — shared code home for server and client
- *Place* — one Roblox world; what gets published

## On your own

Time to put the book away. Don't scroll back up to the steps — prove to yourself, from your own head, that the big idea stuck. No one marks this one — it's just for you. It's the easiest way to spot what *hasn't* stuck yet, while it's still simple to fix. Getting stuck here is completely fine — that's exactly what it's for.

Open Studio with the Baseplate. Without looking:

1. Add a Script to `ServerScriptService`.
2. Make it print one word, and press Play.
3. Then point at each of the four panels and say its name out loud.

<details><summary>Stuck? Open this to check yourself.</summary>

You should have done all of this:

- Right-clicked `ServerScriptService` in the Explorer and chose *Insert Object → Script*.
- Typed something like `print("Kingdom")` in the new Script.
- Pressed the green **Play** button and saw `Kingdom` show up in the Output panel.
- Named the four panels: **Viewport** (the 3D world in the centre), **Explorer** (the object tree on the right), **Properties** (the fields below Explorer), **Output** (the console at the bottom).

If the Output panel was hidden, you open it from *View → Output*.

</details>

## Words to add to the glossary

- **Roblox Studio** — the free editor for building Roblox places.
- **Place** — a single Roblox game world.
- **Workspace** — the live 3D scene every player sees.
- **Explorer** — Studio's tree view of every object in the place.
- **Output panel** — Studio's console; `print` and errors show up here.

## Wrap up

1. **Quiz** — open `quiz.md`, jot your answers in `journal/quiz-notes.md`.
2. **Progress** — one line in `journal/progress.md`: `Module 5.1 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit and push** — stage both files, commit message `Module 5.1 done`, Sync.
4. **Post in `#wins`** — one line about today, plus the URL of the commit.

Module 0.1 covers the why and the panel/CLI steps if you need a refresher. Bring quiz answers you're least sure about to the next weekly sync.

## Next

Module 5.2 introduces **Luau** — Lua with optional types. It looks a bit like JavaScript and a bit like Python. Your engine port begins there.

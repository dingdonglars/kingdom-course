# Quiz — Module 5.6

> Don't write your answers in this file — open `journal/quiz-notes.md` and write them there.

## 1. What is a Part in Roblox?

- **a.** A code file that contains scripts and assets
- **b.** The basic 3D building block — has shape, size, position, colour, material; lives in `Workspace`
- **c.** A Lua function that builds something visible
- **d.** A type of script used for visual logic only

## 2. What does `Instance.new("Part", workspace)` do?

- **a.** Creates a new Part and parents it to `workspace` — the Part appears in the live 3D scene immediately
- **b.** Loads a Part that was previously saved to disk
- **c.** Imports a Part from the Roblox Toolbox
- **d.** Compiles a new Part class for use later

## 3. Why does the lesson keep a `tileToBuilding` map?

- **a.** The engine and the visual representation need to stay in sync. Without the map, you can't tell which tile already has a building when re-clicked.
- **b.** It improves runtime performance of click handling
- **c.** Roblox requires the map for `ClickDetector` to function
- **d.** Style preference — any data structure would work

## 4. What is a `ClickDetector` for?

- **a.** Detecting mouse clicks on a Part; fires a server-side event with the clicking player as the argument
- **b.** Detecting bugs and runtime errors
- **c.** A required child for any Part to render in `Workspace`
- **d.** Audio detection on a Part's surface

## 5. The lesson's rule "the engine is the source of truth, the visual is its projection." Apply it.

- **a.** The engine decides what's true (the kingdom's building list); the visual follows from that. Don't update the visual without going through the engine.
- **b.** Engine and visual are independent and may disagree
- **c.** The visual is authoritative; the engine reflects what the player sees
- **d.** The two are always equal because they share the same data

---

When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.

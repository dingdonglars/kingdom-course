# Quiz — Module 5.6

## 1. What's a Part in Roblox?

a. A code file
b. The basic 3D building block — has shape, size, position, color; visible in Workspace
c. A function
d. A type of script

## 2. What does `Instance.new("Part", workspace)` do?

a. Creates a new Part and parents it to `workspace` — the Part appears in the live 3D scene immediately
b. Loads a saved Part
c. Imports a Part
d. Compiles a Part

## 3. Why does the lesson keep a `tileToBuilding` map?

a. The engine and the visual representation are kept in sync. Without the map, you couldn't know which tile already has a building when re-clicked.
b. Performance
c. Required by Roblox
d. Style

## 4. What's a `ClickDetector` for?

a. Detect mouse clicks on a Part; fires a server-side event with the clicking player
b. Detect bugs
c. Required by parts
d. Audio

## 5. The lesson's rule "the engine is the source of truth, the visual is its projection" — apply it.

a. The engine decides what's true (the kingdom's building list); the visual follows from that. Don't update the visual without going through the engine.
b. They're independent
c. The visual is authoritative
d. They're always equal
# Quiz answers — Module 5.6

## 1. b
A Part is the visual atom — a 3D shape with size, position, color, material, physics. Everything visible in Roblox is a Part or composed of Parts (Models = grouped Parts).

## 2. a
`Instance.new(class, parent)` creates and parents in one call. Setting a parent makes the object appear in the world (or wherever appropriate); without a parent, the object exists in memory but isn't live.

## 3. a
The `tileToBuilding` map keeps the visual ↔ engine link explicit. When a tile is re-clicked, you check the map: already has a building → skip. The engine doesn't know about Parts; the visual layer maintains its own mapping.

## 4. a
`ClickDetector` is a Roblox-specific helper: child of a Part, listens for cursor clicks, fires a server-side event with the player who clicked. No need for raycasting or raw mouse handling.

## 5. a
The engine owns the model. The visual is a *view* of that model. Always change the model first, then update the visual. Skipping the engine = the kingdom's state diverges from what's drawn = bug.
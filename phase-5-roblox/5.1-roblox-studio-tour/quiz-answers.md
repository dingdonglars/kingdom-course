# Quiz answers — Module 5.1

## 1. b
`ServerScriptService` — runs once on the game server when the place starts. Not visible to clients. Use for game logic that needs the authoritative server (e.g., resource validation).

## 2. b
The Output panel is Roblox's console. `print(x)` appears here. Runtime errors appear here in red. Always-open while developing.

## 3. b
Viewport (the 3D scene), Explorer (the object tree), Properties (the inspector), Output (the console). Roblox's interface borrows from Unity/Unreal's vocabulary.

## 4. b
A "place" in Roblox vocabulary is one game world. Your published place has a place ID (`roblox.com/games/<id>/<name>`); friends visit and play.

## 5. b
The visual editor adds *one* dimension (the 3D viewport) on top of a familiar code-edit-run loop. Everything else maps to what you've used in VS Code: a file tree, an inspector, a console. Recognise the shape; the rest is detail.
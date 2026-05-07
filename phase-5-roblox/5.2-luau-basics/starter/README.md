# Module 5.2 starter — Luau basics

A small Luau playground that exercises every syntax point from the lesson — variables, functions, conditionals, loops, tables, type annotations.

## What's in here

- `roblox-kingdom/scripts/luau-basics.lua` — the playground file. Each section in the file matches a section in the lesson and has a `print(...)` so you can see it run.
- `roblox-kingdom/` — the placeholder folder for your Roblox project setup notes.

## How to use it

1. Open Studio and load the place you started in Module 5.1.
2. In the Explorer, right-click `ServerScriptService` and choose *Insert Object → Script*.
3. Open `luau-basics.lua` from this folder, copy the whole contents, and paste it into the new Script.
4. Hit Play. Read the Output panel from top to bottom and match each `print` to the section of the lesson it came from.

## Common gotcha

Luau arrays are one-indexed. If the loop section prints nothing, look for `for i = 0, n` somewhere — change the zero to a one. This will trip you up roughly daily until your fingers re-train; mark the file the first time it bites and read the comment back to yourself the second time.

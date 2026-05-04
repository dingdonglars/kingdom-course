# Challenge — M6 — *Kingdom v5 (Roblox-Published)*

Wraps **Block 7 (Roblox Kingdom)**.

> **No automated tests.** Roblox publishes outside the .NET tooling chain; verifying a published place programmatically would require Roblox's API + auth tokens — out of scope. The artefact *is* the public Roblox URL.

## What this checks (manual)

- A `roblox-kingdom/` (or similarly named) folder exists with at least the engine ModuleScripts ported to Luau (`Building.lua`, `Farm.lua`, `Lumberyard.lua`, `Mine.lua`, `Kingdom.lua`, `ResourceLedger.lua`, `Citizen.lua`)
- A live Roblox place URL exists and 3 friends have visited it (provide screenshots)
- `journal/wins.md` has an M6 entry mentioning M6 / Block 7 / Roblox + the place URL
- `journal/m6-looking-back.md` exists and is filled in (~the four prompts from M5.8)

## What this does NOT check

- That the place is *good* (no automated quality bar — your friends are the bar)
- That the Lua code passes lint (no automated Lua lint in this repo; do it manually)

## How to run

```powershell
dotnet test path\to\challenges\M6\M6.Tests.csproj
```

The .NET test verifies the *artefacts*: folder structure, wins entry, capstone entry. The actual *play* is the human bar.

Green = M6 met. Mentor signs off the year.

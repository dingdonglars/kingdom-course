# Challenge — M6 — *Roblox-Published Kingdom*

Wraps **Phase 5 — Roblox Kingdom**. This is the year-end milestone — the moment you send your friends a Roblox link and they actually play.

> **No automated runtime tests for the Roblox place itself.** Roblox publishes outside the .NET tooling chain, and verifying a published place programmatically would need Roblox's API and auth tokens — out of scope for this repo. What you keep *is* the public Roblox URL, and the proof is your friends playing it.

## What this verifies

| Check | Looks for |
| --- | --- |
| Roblox folder | `roblox-kingdom/` (or similar) with engine ModuleScripts ported to Luau — at minimum `Building.lua`, `Farm.lua`, `Lumberyard.lua`, `Mine.lua`, `Kingdom.lua`, `ResourceLedger.lua`, `Citizen.lua` |
| Public URL | A live Roblox place URL exists; three friends have visited it (screenshots) |
| Wins entry | `journal/wins.md` with an M6 entry mentioning M6 / Phase 5 / Roblox plus the place URL |
| Looking-back entry | `journal/m6-looking-back.md` exists and is filled in (the four prompts from Module 5.8) |

## What this skips

- Whether the place is *good* — there's no automated quality check. Your friends are the standard.
- Whether the Lua passes lint — no automated Lua lint in this repo. Run it yourself.

## How to run

From your repo root:

```powershell
dotnet test path\to\challenges\M6\M6.Tests.csproj
```

The .NET test verifies the files: folder structure, wins entry, looking-back entry. The actual *play* is the human check.

Green = M6 met. Mentor signs off the year.

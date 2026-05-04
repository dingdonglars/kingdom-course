# Challenge — M1 — *Inventory Tool*

Wraps **Phase 0 — Foundations** (modules 0.5–0.8).

By M1 you've built a small CLI inventory tool that takes commands, holds items in memory, and finds them on demand. This challenge confirms the tool is in your repo, builds, and responds to the four commands the lesson taught.

## What this verifies

| Check | Looks for |
| --- | --- |
| Tool folder | `InventoryTool/` at the repo root with a `.csproj` that builds |
| Repo README | `README.md` at the repo root |
| Wins entry | `journal/wins.md` with an M1 entry of at least 100 characters |
| Commands work | `add`, `list`, `find`, and `quit` all respond — adding `apple` and finding it returns a line containing `Found: apple` |

## What this skips

- The exact wording around your output — the test looks for `apple` and `Found: apple` but doesn't care what's printed alongside.
- Save and load — those are visual checks you ran during the lesson.
- Error handling and rough edges — the mentor walks through those at milestone review.

## How to run

From your repo root:

```powershell
dotnet test path\to\challenges\M1\M1.Tests.csproj
```

Green = M1 met.

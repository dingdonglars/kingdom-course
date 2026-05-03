# Challenge — M1 — *Inventory Tool*

Wraps **Block 2 (Foundations)**.

## What this checks

That your repo by end of M1 has:

- `InventoryTool/` folder at the repo root with a `.csproj` that builds.
- A `README.md` at the repo root.
- A `journal/wins.md` entry of at least 100 characters.
- The `InventoryTool` actually runs and responds to `add`, `list`, `find`, `quit`.

## What this does NOT check

- The exact wording of your output (we look for `apple` and `Found: apple`, but not the surrounding prose).
- Save/load (those are checked by you running them in the lesson).
- Error handling (visual check).

## How to run

In your repo root:

```powershell
dotnet test path\to\challenges\M1\M1.Tests.csproj
```

Green = M1 met.
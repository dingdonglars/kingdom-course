# Challenge — M3 — *Kingdom v2 (Persisted)*

Wraps **Block 4 (Persistence)**.

## What this checks

- `Kingdom.Persistence/` project exists and builds
- A `KingdomEfStore` (or equivalently named class) exists with `Save`, `Load`, `ListSlots`, `Delete` methods
- An `IRandom` abstraction is wired through (no `new Random()` inside the engine — same rule as M2)
- A round-trip works: save a kingdom, load it back, name and resources match
- `journal/wins.md` has an M3 entry (≥100 chars, mentions M3 / Block 4 / Persistence)

## What this does NOT check

- Specific entity / table / column names (yours can differ)
- Whether you used JSON, SQLite, EF Core, or all three (the lesson covers all; the choice is yours)
- The exact menu UI layout
- Migrations vs. `EnsureCreated` — either approach passes

## How to run

```powershell
dotnet test path\to\challenges\M3\M3.Tests.csproj
```

Green = M3 met. Run the per-milestone ritual: wins post + before/after + Discord screenshot.

## What it does in practice

1. Compiles your `Kingdom.Persistence/` project.
2. Reflects on it: does a class with `Save(Kingdom k) → int` and `Load(int id, ...)` exist?
3. Builds a sample kingdom in-memory, saves it, lists slots, loads it back, asserts name + resources survive.
4. Verifies `journal/wins.md` exists and mentions M3.

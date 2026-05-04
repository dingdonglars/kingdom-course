# Challenge — M3 — *Persisted Kingdom*

Wraps **Phase 2 — Persistence**.

M3 confirms your kingdom can save itself and come back. The test does a real round-trip: build a kingdom in memory, save it, list slots, load it back, and verify the name and resources survived the trip.

## What this verifies

| Check | Looks for |
| --- | --- |
| Persistence project | `Kingdom.Persistence/` exists and builds |
| Store class | A class with `Save`, `Load`, `ListSlots`, `Delete` methods (name can vary — `KingdomEfStore` or whatever you chose) |
| `IRandom` still wired | No `new Random()` inside the engine — same rule as M2 |
| Round-trip | Save a kingdom, load it back, name and resources match |
| Wins entry | `journal/wins.md` with an M3 entry of at least 100 characters mentioning M3 / Phase 2 / Persistence |

## What this skips

- Specific entity, table, or column names — yours can differ.
- Whether you ended on JSON, SQLite, or EF Core — the lesson covers all three; the choice is yours.
- The menu UI layout.
- Migrations vs. `EnsureCreated` — either approach passes.

## How to run

From your repo root:

```powershell
dotnet test path\to\challenges\M3\M3.Tests.csproj
```

Green = M3 met. Then the milestone ritual.

## What the suite actually does

1. Compiles your `Kingdom.Persistence/` project.
2. Reflects on it — does a class with `Save(Kingdom k) → int` and `Load(int id, ...)` exist?
3. Builds a sample kingdom in memory, saves it, lists slots, loads it back, asserts name and resources survive.
4. Verifies `journal/wins.md` exists and mentions M3.

# Challenge — M2 — *Console Kingdom*

Wraps **Phase 1 — Console Kingdom**.

M2 is the first milestone where the test suite has real teeth. The engine has to be deterministic — that is, two runs with the same seed produce the same events in the same order. If you new'd up `Random` directly anywhere in the engine, this challenge fails. That's the moment Module 1.8's lesson on `IRandom` finally pays for itself.

## What this verifies

| Check | Looks for |
| --- | --- |
| Project layout | `Kingdom.Engine/`, `Kingdom.Console/`, and `tests/Kingdom.Engine.Tests/` build |
| Engine vs shell | No `Console.WriteLine` inside `Kingdom.Engine/` |
| Building family | `Farm`, `Lumberyard`, `Mine`, plus the abstract `Building` |
| `IRandom` abstraction | An interface in the engine that the events system depends on |
| EventLog | A property on `Kingdom` that grows when the engine ticks |
| Sub-namespaces | At least `Kingdom.Engine.Buildings` and one other |
| Test count | 30+ passing tests across the engine |
| Wins entry | `journal/wins.md` with an M2 entry of at least 100 characters |

## What this skips

- Exact resource starting values, tick rates, or chance percentages — the kingdom is yours to balance.
- The names of your event subclasses — only that the log fills up.
- The exact sub-namespace names beyond `Buildings/`.
- README polish — that's a mentor read at milestone review.

## How to run

From your repo root:

```powershell
dotnet test path\to\challenges\M2\M2.Tests.csproj
```

Green = M2 met. Then the milestone ritual: wins entry, `#wins` post, before/after one-liner.

## What the suite actually does

1. Compiles your engine.
2. Reflects on it — does `Kingdom.Engine.Buildings.Building` exist? Does `Kingdom.Engine.Infrastructure.IRandom` exist? Does `Kingdom.Engine.Kingdom` have an `EventLog` property?
3. Runs the engine for 50 days with a seeded `SystemRandom`. Asserts `EventLog.Count > 0`.
4. Runs the same scenario twice with the same seed; asserts both runs produce the same events in the same order.

Step 4 is the determinism check. If the engine reaches for `new Random()` anywhere, the two runs diverge and the test fails — closing the loop on Module 1.8.

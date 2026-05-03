# Challenge — M2 — *Kingdom v1 (Console)*

Wraps **Block 3 (Console Kingdom)**.

## What this checks

That your repo by end of M2 has:

- `Kingdom.Engine/`, `Kingdom.Console/`, and `tests/Kingdom.Engine.Tests/` projects that build.
- The engine separated from the shell (no `Console.WriteLine` inside `Kingdom.Engine/`).
- An `IRandom`-style abstraction so the engine is deterministic when seeded.
- The four building subclasses (`Farm`, `Lumberyard`, `Mine`, plus the abstract/base `Building`).
- An `EventLog` populated by an event engine that takes `IRandom`.
- Sub-namespace organisation — at least `Kingdom.Engine.Buildings` and one other.
- 30+ passing tests across the engine.
- A `journal/wins.md` entry for M2 of at least 100 characters.

## What this does NOT check

- Exact resource starting values, tick rates, or chance %s — pick what makes sense for your kingdom.
- The names of your event subclasses (only that `EventLog` exists and grows).
- The exact subfolder names (just that `Buildings/` exists).
- README polish (visual check by mentor).

## How to run

In your repo root:

```powershell
dotnet test path\to\challenges\M2\M2.Tests.csproj
```

Green = M2 met. Run the per-milestone ritual: wins post + before/after + Discord.

## What this looks like in practice

The challenge runs a structural smoke test:

1. Compiles your engine.
2. Reflects on it: does `Kingdom.Engine.Buildings.Building` exist? Does `Kingdom.Engine.Infrastructure.IRandom` exist? Does `Kingdom.Engine.Kingdom` have an `EventLog` property?
3. Runs your engine for 50 days with a seeded `SystemRandom`. Asserts the `EventLog.Count > 0`.
4. Runs *the same scenario twice with the same seed*; asserts both runs produce the same number of events with identical descriptions in order.

The third point is the hard one — if your `EventEngine` newed up `Random` directly, it'll fail. That's the fail-the-test moment that closes the loop on Module 1.8.
## M3 — Phase 2 — Persistence

- 71 tests, deterministic, across engine + persistence
- Same engine now savable in 4 ways: text file, JSON, raw SQLite, EF Core
- Real save-slot UI; you can play across sessions
- Did the M3 rename party — codebase reads more cleanly

**Before:** `Console.WriteLine($"Day {kingdom.Day}");` and the kingdom died on close
**After:**  Save, quit, reopen days later — your kingdom is exactly where you left it

Posted to `#wins` on YYYY-MM-DD.

# Block 4 — Persistence

> **Your kingdom survives a restart.**

**What you'll have at the end:** Your console Kingdom now saves and loads. Multiple saved kingdoms via SQLite + Entity Framework Core. Plus a deliberate naming pass over everything you've written so far.

**Estimated effort:** 7 weeks at 4–6 hrs/week  ·  **Wraps:** M3 *Kingdom v2 — Persisted*

---

## Why this block?

A game that forgets you when you close it isn't really a game. By the end of this block, your Kingdom *survives*. You'll touch three layers of persistence in sequence — files, then JSON, then a real relational database — and learn why each came to exist.

This block also introduces the database, which is the Kingdom's *third shell* (after the engine and console). The same engine that didn't care about being in a console doesn't care about being persisted to disk either. **The model is forever; the runtime is a detail.**

## What you'll learn (named)

- File I/O — paths, encoding, reading and writing
- JSON serialisation — `System.Text.Json`, save/load
- Round-trip tests — your first taste of property-style thinking
- SQL — what a relational database is, and how to query one
- EF Core — the .NET ORM, code-first
- Migrations — evolving the schema without losing data
- Multiple kingdoms — DB-backed save slots
- **Semantic naming** — the deeper craft (Module 2.11 "Names That Earn Their Keep")

## What you'll build (the climb)

- **Week 16:** files and JSON.
- **Week 17:** round-trip tests + understanding why files break.
- **Weeks 18–19:** SQL primer, JOINs, EF Core code-first, migrations.
- **Weeks 20–21:** SQLTools, multiple kingdoms, then the Rename Party.

## Brag-worthy outcome

You can demo the Kingdom *across two sessions*. Close the app, reopen it, your kingdom is still there. With a leaderboard of saved kingdoms.

---

[**→ Start Module 2.1 — File I/O**](./2.1-file-io/lesson.md)

# Phase 2 — Persistence

> **Your kingdom survives a restart.**

**What you'll have at the end:** A console kingdom that saves and loads. Multiple saved kingdoms in a real database via SQLite + Entity Framework Core. Plus a deliberate naming pass over everything you've written so far.

**Estimated effort:** 7 weeks at 4–6 hrs/week  ·  **Wraps:** **M3** *Kingdom v2 — Persisted*

---

## Why this phase?

A game that forgets you when you close it isn't really a game. By the end of these seven weeks, your kingdom *survives* — close the program, reopen it tomorrow, the kingdom is exactly where you left it. That's the milestone. Everything else this phase teaches you serves it.

You touch three layers of saving in sequence — files, then JSON, then a real relational database — and learn why each one came to exist. Files are the simplest thing; you can open them in Notepad. JSON is files plus a clean format that records and APIs love. A database is JSON plus *queryability* — once you have one, "give me the kingdom with the most gold" is a single line.

This is also where the **database** joins the family of things your engine doesn't care about. Console, JSON, SQLite — each is a different runtime over the same engine. Nothing in `Kingdom.Engine/` changes when you swap one for another. That separation has paid off twice already; it pays off three more times in this phase, then again in Phase 3 (HTTP), Phase 4 (browser), Phase 5 (Roblox). The engine is forever; the runtime is a detail.

## What you'll learn (named)

- File I/O — paths, encoding, reading and writing text on disk
- JSON serialisation — `System.Text.Json`, the save-and-load round trip
- Round-trip tests — your first taste of property-style thinking
- SQL — what a relational database is, and how to query one
- EF Core — the .NET ORM, code-first, two-way mapping between classes and rows
- *Migrations* — versioned schema changes that don't lose data
- Multiple kingdoms — DB-backed save slots, full CRUD
- Naming for readers — the deeper craft (Module 2.11 *Names That Earn Their Keep*)

## What you'll build (the climb)

- **Week 1:** files and JSON. Your kingdom writes itself to disk for the first time.
- **Week 2:** round-trip tests + the redesigns persistence forces on the engine.
- **Weeks 3–4:** SQL primer, JOINs, EF Core code-first, then *migrations* — the proper way to evolve a schema after you've shipped.
- **Weeks 5–6:** database tooling, multiple kingdoms with full save-slot CRUD, an interactive console UI.
- **Week 7:** the rename party — a deliberate sweep over every name in the codebase. M3 lands at the end.

## Brag-worthy outcome

You can demo the kingdom *across two sessions*. Close the app on Monday, open it on Friday — kingdom still there, with all its history. Plus a list of every saved kingdom you've ever made.

---

[**→ Start Module 2.1 — File I/O**](./2.1-file-io/lesson.md)

# Module 4.0 — Context Engineering Unlock

You've been programming for about seven months. You have an engine, a database, an API on the internet that anyone with the URL can call. Today the AI joins as a tool you're allowed to use for writing code. The question stops being *can the AI write something* — yes, obviously — and becomes *how do I get the AI to write something that fits my project*?

This module names the discipline. It's called **context engineering** — choosing what the AI sees before it answers. The default failure mode of AI-assisted code is generic-tutorial code: it works in a vacuum, ignores your conventions, names things differently than your existing code, and reaches for libraries you don't have. Context is the cure. The same question, asked with the right context, gets a useful answer in three seconds. Asked without context, it gets a thirty-minute back-and-forth. The investment is small; the difference is large.

> **Words to watch**
>
> - **context window** — the slice of text the AI can read at once. There's a limit.
> - **context engineering** — the practice of choosing what goes into that window.
> - **prompt** — what you say to the AI; the frame around your question.
> - **reference set** — the small folder of project-specific docs the AI reads first.
> - **explanation rule** — the post-Unlock rule: you must be able to explain every line you ship.

---

## The four-step frame

When you ask the AI to write something non-trivial, give it four things in order. Goal first, then where it goes, then your conventions, then the traps. The full version adds a fifth — one similar example from your own code — but the four-step version is the one to learn first.

1. **The goal**, in one sentence. *"I need a method that returns the user's richest kingdom."*
2. **Where it goes** — file path, the class it lives on, what calls it.
3. **The conventions** — relevant excerpts from `STANDARDS.md`, your naming style, your error pattern.
4. **What it should not do** — the traps. *"Don't `new Random()` — we always inject `IRandom`."*

The fifth, when you reach for it: one similar method from elsewhere in your project. *"Here's how we did `Save`; do `LoadRichest` the same way."* That single example is often worth more than two paragraphs of explanation.

## Scoping — the discipline of small asks

Tutorials show big asks. *"Write me a checkout flow."* In your own work, those answers are useless — too much code, too many guesses, hard to verify line by line. The discipline is the opposite. **One method at a time. One file at a time.** A small ask is easy to scope, easy to read, easy to check.

If the work is bigger, scaffold it yourself first. Sketch the empty methods. Write the test names. Then ask the AI to fill in one method whose signature you've already decided. You stay the architect; the AI handles the typing.

## Eval — checking the answer

Every answer the AI gives gets read line by line before it goes near your code. The post-Unlock rule is one sentence: *you must be able to explain every line you ship*. That rule is honor-system. It also drives every PR review you'll do for the rest of the year. The AI-assistance section in your PR description names the AI-touched files; mentor reads those at the weekly sync.

A failure mode worth a name: **invented APIs.** When the AI is over-confident, it confidently calls methods that don't exist. `db.Kingdoms.GetRichest()` looks plausible — it's not in your codebase. Always run the code; always read the diff.

## The reference set

Curate a small folder the AI reads before it answers. The course already gives you most of it:

- `STANDARDS.md` — your conventions
- `ai-context/CLAUDE.md` — the AI's machine-side rules
- `ai-context/prompts/` — pre-written templates for common asks
- `GLOSSARY.md` — terms used in your project
- A short `ARCHITECTURE.md` — what's in each project, how data flows (you'll write this in Step 3 below)

When using Claude Code or a similar agent, point it at the folder; it reads automatically. When using a chat UI, paste the most relevant two or three files at the start of the conversation.

## What changes in this module

- **NEW:** `ARCHITECTURE.md` (you write yours in Step 3)
- **MODIFIED:** `ai-context/prompts/README.md` — add an "Implementation requests" section
- **NEW:** `ai-context/prompts/implementation-help.md` — a fill-in template for code requests

No code changes today. This module is about the tools around the AI, not the AI's output.

## Step 1 — read the post-Unlock `CLAUDE.md`

Open `ai-context/CLAUDE.md`. Notice:

- The mode flag now reads `post-unlock`.
- The post-Unlock behaviour section applies.
- The PR template's AI-assistance section is in force.

Read it end-to-end. You'll come back to it many times.

## Step 2 — try the four-step frame on a real task

Pick a tiny task. *"Write me a method that returns the user's kingdom with the most gold."*

Without context, the prompt looks like this:

> Me: write me a method that returns the kingdom with the most gold

The AI invents an unrelated `Kingdom` class, uses a different LINQ style, doesn't know about your `KingdomEfStore.ListSlots`. The answer is useless.

With context:

> Me: I'm working in `Kingdom.Persistence/EfCore/KingdomEfStore.cs`. The class already has `Save`, `Load`, `Delete`, `ListSlots(string ownerSub)`. I need a new method `LoadRichest(string ownerSub)` that returns the user's `KingdomSummary` with the highest gold (or null if they have no kingdoms). Match the style of the other methods (using `using var ctx = new KingdomDbContext(_dbPath); ... AsNoTracking()`). Don't load full Kingdom entities — project to summary inline.

The AI writes the right method, in the right style, in three lines. Same question; very different answer.

## Step 3 — write your `ARCHITECTURE.md`

A one-pager at your repo root. Aim for thirty to fifty lines.

```markdown
# Architecture

## Projects
- `Kingdom.Engine/` — domain logic. No IO. No frameworks. Subnamespaces: Buildings, Citizens, Resources, Events, Infrastructure (interfaces), Snapshots.
- `Kingdom.Persistence/` — JSON store + SQLite (via raw SqliteConnection) + EF Core. `KingdomEfStore` is the canonical persistence.
- `Kingdom.Console/` — interactive console shell (`SaveSlotUI` menu loop).
- `Kingdom.Api/` — minimal API. Auth via Google OAuth + cookies. Multi-user via `OwnerSub` claim. OpenAPI at `/openapi/v1.json`.
- `tests/Kingdom.*.Tests/` — xUnit + Shouldly + FakeItEasy. Integration tests use `WebApplicationFactory<Program>`.

## Data flow
HTTP request → cookie auth → `OwnerSub` extracted → `KingdomEfStore` (scoped query) → engine via `Kingdom.LoadFrom(snapshot, rng, clock)`.

## Conventions
- See `STANDARDS.md` for naming, commits, branches, tests.
- DTOs at every boundary (JSON store, EF entities, API request/response).
- Engine takes `IRandom` + `IClock` via constructor — never `new Random()`.
- All store methods take `ownerSub` first; never optional.

## Not here yet
- Citizens are minimal — just a Name. Plans to add a `Mood` field and a `CitizenHappy` event.
- Events: 3 kinds (TraderArrived, CitizenIll, BuildingBurned). The browser shell will add CitizenStarved.
```

Commit it. The AI now reads this on session start, and so does future-you.

## Step 4 — install the implementation prompt

`ai-context/prompts/implementation-help.md`:

```markdown
# Implementation help (post-Unlock)

Use when asking Claude to write non-trivial code.

## Fill in:

**Goal (one sentence):**

**File path + surrounding context:**

**Relevant existing code (paste 1-3 small snippets):**

**Conventions to follow:**
- (link to STANDARDS.md sections)
- (any project-specific quirks)

**What you should NOT do:**
- (traps: e.g., "Don't use `new Random()` — use `IRandom`.")

**My understanding:** I'll be asked to explain each line you write. End your response with the explanation prompt per `ai-context/CLAUDE.md`.
```

## Tinker

Use the four-step frame on a real task today. Notice the difference in answer quality. Then compare two prompts side by side: one with just the goal, one with full context. Save both responses; come back in a month and read them again. The improvement gets clearer over time.

Your `ARCHITECTURE.md` will drift as the project grows. Updating it is a small piece of work in itself — every milestone touches it, even if the only change is *"still accurate."*

Audit the AI's output for invented APIs. When it's over-confident, it makes things up. Catch them early; don't merge a method that calls something that doesn't exist.

## What you just did

You named the discipline you'll be using for the rest of the course. **Context engineering** is choosing what the AI sees before it answers — and the four-step frame (goal, where, conventions, traps) is the move you'll repeat. You wrote a short `ARCHITECTURE.md` so the AI starts from your project, not from a generic tutorial. You installed an `implementation-help.md` prompt template so the next code request has a layout to fill in. You also met the post-Unlock rule that drives everything from here: you can ship AI-assisted code, *and* you can explain every line you ship. Two artefacts on disk; one rule in your head.

**Key concepts you can now name:**

- **context engineering** — choosing what the AI sees so its answer fits
- **the four-step frame** — goal, where, conventions, traps
- **scoping** — one method, one file at a time; small asks beat big asks
- **invented APIs** — the failure mode where the AI calls methods that don't exist
- **the explanation rule** — you must be able to explain every line you ship

## Quiz

Open `quiz.md`. When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.

## Next

Module 4.1 starts the actual browser work — HTML and CSS. The smallest useful page that shows your kingdom, opened straight from a `.html` file with no build step yet.

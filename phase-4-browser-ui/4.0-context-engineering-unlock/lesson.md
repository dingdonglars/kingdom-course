# Module 4.0 — Context Engineering Unlock

> **Hook:** the AI Unlock fired at M4. Today we use it intentionally for the first time. **Context engineering** is the meta-skill: how you set up the AI's view of your project so it can write code that actually fits — instead of generic tutorial code that breaks against your style. The discipline you build today carries through Phases 4, 5, and the bonus arc.

> **Words to watch**
> - **context window** — the slice of text the AI can read at once. Limited.
> - **context engineering** — the practice of choosing what goes into the window
> - **prompt** — what you say to the AI; the *frame* around your question
> - **reference set** — the files and notes the AI can lean on (yours; project-specific)
> - **explanation gate** — the post-unlock rule: you must be able to explain every line you merge

---

## Why a separate module

You've been programming for ~7 months. You have an engine, a database, an API on the internet. The AI is now allowed to help write code. **The question is no longer "can I get the AI to write something" — it's "how do I get the AI to write something *good for my project*?"**

The default failure mode of AI-assisted code is *generic-tutorial-shaped code* — works in a vacuum, ignores your conventions, names things differently than your existing code, uses libraries you don't have. **Context is the cure.**

## The context recipe

When you ask the AI to write something non-trivial, give it five things:

1. **The goal**, in one sentence.
2. **Where it goes** — file path, surrounding code, the function it'll be called from.
3. **The conventions** — relevant excerpts from `STANDARDS.md`, your naming style, your error pattern.
4. **One similar example** — "here's how we did X; do Y the same way."
5. **What it should *not* do** — pointers to traps. ("Don't `new Random()` — use the `IRandom` we always inject.")

That's the recipe. Try it without; the output is mediocre. Try it with; the output sometimes ships unchanged. **Context is leverage.**

## The reference set

Curate a small folder the AI can read first. The course already gives you most of it:

- `STANDARDS.md` — your conventions
- `ai-context/CLAUDE.md` — the AI's machine-side rules
- `ai-context/prompts/` — pre-written templates for common asks
- `GLOSSARY.md` — terms used in your project
- A 1-paragraph `ARCHITECTURE.md` — what's in each project, how data flows (write this in Step 3 below)

When using Claude Code or similar, point it at the folder; it reads automatically. When using a chat UI, paste the most relevant 2-3 files into the conversation start.

## Delta starter

- **NEW:** `ARCHITECTURE.md` (write yours in Step 3)
- **MODIFIED:** `ai-context/prompts/README.md` — add an "Implementation requests (post-unlock)" section
- **NEW:** `ai-context/prompts/implementation-help.md` — a fill-in template for code requests

No code changes; this module is about your tooling around the AI.

## Step 1 — read the post-unlock `CLAUDE.md`

Open `ai-context/CLAUDE.md`. Notice:

- Mode flag: `post-unlock`
- The "post-unlock behavior" section now applies
- The PR template's AI-assistance section is in force

Read it end-to-end. **You'll be back to it many times.**

## Step 2 — try the recipe

Pick a tiny task. *"Write me a method that returns the kingdom with the most gold."*

**Without context:**

> Me: write me a method that returns the kingdom with the most gold

The AI invents an unrelated `Kingdom` class, uses a different LINQ style, doesn't know your `KingdomEfStore.ListAll`. Output: useless.

**With context:**

> Me: I'm working in `Kingdom.Persistence/EfCore/KingdomEfStore.cs`. The class already has `Save`, `Load`, `Delete`, `ListSlots(string ownerSub)`. I need a new method `LoadRichest(string ownerSub)` that returns the user's `KingdomSummary` with the highest gold (or null if they have no kingdoms). Should match the style of the other methods (using `using var ctx = new KingdomDbContext(_dbPath); ... AsNoTracking()`). Don't load full Kingdom entities — project to summary inline.

The AI writes the right method, in the right style, in 3 lines. **Context turned a 30-minute back-and-forth into 30 seconds.**

## Step 3 — write your `ARCHITECTURE.md`

A one-pager at your repo root. Aim for 30-50 lines.

Template:

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

## What's deliberately not here yet
- Citizens are minimal — just a Name. Plans to add `Mood` enum + `CitizenHappy` event.
- Events: 3 kinds (TraderArrived, CitizenIll, BuildingBurned). Browser shell will add CitizenStarved.
```

Commit it. **The AI now reads this on session start; so does future-you.**

## Step 4 — install the implementation prompt

`ai-context/prompts/implementation-help.md`:

```markdown
# Implementation help (post-unlock)

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

- Use the recipe for a real task today. Notice the difference in output quality.
- Compare two prompts: one with just the goal, one with full context. **Save both responses.** Later in the year, look back. The improvement compounds.
- Your `ARCHITECTURE.md` will drift as the project grows. Updating it is a deliverable in itself — *every* milestone touches it, even just to confirm "still accurate."
- Audit your AI assistant's output for **invented APIs** (calls to methods that don't exist). When the AI is over-confident, it makes things up. Catch them early.

## Name it

- **Context engineering** — choosing what the AI sees so its output fits.
- **Reference set** — the curated folder of project-specific docs.
- **The five-thing recipe** — goal, where, conventions, similar example, traps.
- **Invented APIs** — failure mode where the AI calls methods that don't exist.
- **Explanation gate** — must explain every line before merging.

## The rule of the through-line

> **Context is leverage.** Five sentences of project-specific context turn the AI from a generic tutorial parrot into a senior pair. The AI's quality is bounded by the context you give it; the discipline of giving it good context is yours to learn.

## Quiz / challenge

Open `quiz.md`.

## Connect

Module 4.1 starts the actual browser work: HTML + CSS fundamentals + serving a static page from your existing API.
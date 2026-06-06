# Module 4.0 — Context Engineering Unlock

You've been programming for about seven months. You have an engine, a database, and an API on the internet that anyone with the URL can call. Today the AI joins in. You're now allowed to use it as a tool for writing code. The question is no longer *can the AI write something* — of course it can — it's *how do I get the AI to write something that fits my project*?

This module gives that skill a name. It's called **context engineering** — choosing what the AI sees before it answers. AI-assisted code usually fails the same way: it comes out as generic-tutorial code. It works on its own, but it ignores your rules, it names things differently from your existing code, and it uses libraries you don't have. Context fixes this. Ask the same question with the right context and you get a useful answer in three seconds. Ask it without context and you get thirty minutes of back-and-forth. You spend a little time on context, and it saves you a lot.

> **Words to watch**
>
> - **context window** — the slice of text the AI can read at once. There's a limit.
> - **context engineering** — the practice of choosing what goes into that window.
> - **prompt** — what you say to the AI; the frame around your question.
> - **reference set** — the small folder of project-specific docs the AI reads first.
> - **explanation rule** — the post-Unlock rule: you must be able to explain every line you ship.

---

## Phase opener — `phase-4` branch

Before any code (the why is in Module 1.1):

```powershell
cd C:\code\kingdom
git switch -c phase-4
```

Every commit this phase goes on `phase-4`. At Module 4.7 (M5 close), you'll open a PR to merge it back into `main`.

---

## The four-step frame

When you ask the AI to write something that isn't tiny, give it four things in order. Goal first, then where the code goes, then your rules, then the traps to avoid. There's a fifth thing you can add — one similar example from your own code — but learn the four-step version first.

1. **The goal**, in one sentence. *"I need a method that returns the user's richest kingdom."*
2. **Where it goes** — file path, the class it belongs to, what calls it.
3. **The rules** — the parts of `STANDARDS.md` that matter here, your naming style, how you handle errors.
4. **What it should not do** — the traps. *"Don't `new Random()` — we always inject `IRandom`."*

The fifth thing, when you want it: one similar method from somewhere else in your project. *"Here's how we did `Save`; do `LoadRichest` the same way."* That one example is often worth more than two paragraphs of explanation.

## Scoping — the discipline of small asks

Tutorials show big requests. *"Write me a checkout flow."* In your own work, those answers are useless — too much code, too many guesses, too hard to check line by line. Do the opposite. **One method at a time. One file at a time.** A small request is easy to plan, easy to read, and easy to check.

If the work is bigger, build the outline yourself first. Sketch the empty methods. Write the test names. Then ask the AI to fill in one method whose signature you've already decided. You stay in charge of the design; the AI does the typing.

## Eval — checking the answer

Read every answer the AI gives, line by line, before it goes anywhere near your code. The post-Unlock rule is one sentence: *you must be able to explain every line you ship*. Nobody checks this for you — it's on your honour. It also guides every PR review you'll do for the rest of the year. The AI-assistance part of your PR description lists the files the AI touched; your mentor reads those at the weekly sync.

There's one mistake worth a name: **invented APIs.** When the AI is too sure of itself, it calls methods that don't exist. `db.Kingdoms.GetRichest()` looks real, but it's not in your code. Always run the code. Always read the diff.

## The reference set

Keep a small folder of files the AI reads before it answers. The course already gives you most of them:

- `STANDARDS.md` — your conventions
- `CLAUDE.md` — the AI's machine-side rules (auto-loaded via the root `CLAUDE.md` import)
- `.claude/commands/` — your slash commands (`/explain-this-concept`, `/code-review`, `/stuck-on-error`, `/walk-through-code`, plus `/lesson-review` and `/milestone-review`)
- `GLOSSARY.md` — terms used in your project
- A short `ARCHITECTURE.md` — what's in each project, how data flows (you'll write this in Step 3 below)

When you use Claude Code, the root `CLAUDE.md` and the slash commands load on their own. When you use a chat window instead, paste the two or three most useful files at the start of the conversation.

## What changes in this module

- **NEW:** `ARCHITECTURE.md` (you write yours in Step 3)
- **NEW:** `.claude/commands/implementation-help.md` — a slash command for code requests (you install yours in Step 4)

No code changes today. This module is about the tools you set up around the AI, not the code the AI writes.

## Step 1 — read the post-Unlock `CLAUDE.md`

Open `CLAUDE.md`. Notice:

- The mode flag now reads `post-unlock`.
- The post-Unlock behaviour section applies.
- The PR template's AI-assistance section is in force.

Read it end-to-end. You'll come back to it many times.

## Step 2 — try the four-step frame on a real task

Pick a tiny task. *"Write me a method that returns the user's kingdom with the most gold."*

Without context, the prompt looks like this:

> Me: write me a method that returns the kingdom with the most gold

The AI makes up a `Kingdom` class that has nothing to do with yours, uses a different LINQ style, and doesn't know about your `KingdomEfStore.ListSlots`. The answer is useless.

With context:

> Me: I'm working in `Kingdom.Persistence/EfCore/KingdomEfStore.cs`. The class already has `Save`, `Load`, `Delete`, `ListSlots(string ownerSub)`. I need a new method `LoadRichest(string ownerSub)` that returns the user's `KingdomSummary` with the highest gold (or null if they have no kingdoms). Match the style of the other methods (using `using var ctx = new KingdomDbContext(_dbPath); ... AsNoTracking()`). Don't load full Kingdom entities — project to summary inline.

The AI writes the right method, in the right style, in three lines. Same question, very different answer.

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

Commit it. The AI now reads this when a session starts, and so will you, later on.

## Step 4 — install the implementation slash command

Drop this into `.claude/commands/implementation-help.md` in your repo:

```markdown
---
description: Post-Unlock implementation help. Asks for goal/where/conventions/traps before writing code.
---

You are being invoked via `/implementation-help`. The learner is past the AI Unlock and is asking you to write non-trivial code. Read `CLAUDE.md` and `STANDARDS.md` first.

If the learner already pasted the goal in `$ARGUMENTS`, use it. Otherwise ask for these in one combined message:

1. **Goal** — one sentence on what the code needs to do.
2. **Where** — file path + a snippet of the surrounding code.
3. **Existing patterns to match** — one or two small snippets from nearby methods.
4. **Conventions to follow** — relevant `STANDARDS.md` sections plus any project-specific quirks (link to them).
5. **Traps** — what *not* to do (e.g. *"don't use `new Random()` — use `IRandom`"*).

Once you have all five, write the implementation. Match the style. Stay inside the named conventions. Don't invent APIs that aren't visible in the code the learner showed you.

End your response with: *"Before you keep this, walk me through what each line does. If you can't explain a line, ask me about it instead of keeping it."*
```

Restart Claude Code (or run *"Reload Window"* in VS Code). Now type `/` and your new command should be in the list.

## Tinker

Use the four-step frame on a real task today. Notice how much better the answer is. Then try two prompts side by side: one with just the goal, one with full context. Save both answers. Read them again in a month. You'll see the difference more clearly over time.

Your `ARCHITECTURE.md` will fall out of date as the project grows. Keeping it current is a small job on its own — check it at every milestone, even if the only change is *"still correct."*

Check the AI's output for invented APIs. When it's too sure of itself, it makes things up. Catch them early. Don't merge a method that calls something that doesn't exist.

## What you just did

You learned the skill you'll use for the rest of the course. **Context engineering** is choosing what the AI sees before it answers — and the four-step frame (goal, where, rules, traps) is the move you'll repeat. You wrote a short `ARCHITECTURE.md` so the AI starts from your project, not from a generic tutorial. You installed an `/implementation-help` slash command so your next code request follows a clear set of steps instead of being a free-form question. You also met the post-Unlock rule that everything from here builds on: you can ship AI-assisted code, *and* you can explain every line you ship. Two files on disk; one rule in your head.

**Key concepts you can now name:**

- **context engineering** — choosing what the AI sees so its answer fits
- **the four-step frame** — goal, where, rules, traps
- **scoping** — one method, one file at a time; small requests beat big ones
- **invented APIs** — the mistake where the AI calls methods that don't exist
- **the explanation rule** — you must be able to explain every line you ship

## On your own

Time to put the book away. Don't scroll back up to the steps. No one marks this — it's just for you. It's the easiest way to spot what hasn't stuck yet, while it's still simple to fix. Getting stuck here is completely fine — that's exactly what it's for.

From your own head:

1. List the four things you give the AI before it writes code for you, in order.
2. Say each one out loud, or write it down.

<details><summary>Stuck? Open this to check yourself.</summary>

The four-step frame, in order:

1. **The goal** — one sentence on what the code should do.
2. **Where it goes** — the file, the class, what calls it.
3. **The rules** — your naming style, your `STANDARDS.md`, how you handle errors.
4. **What it should not do** — the traps. (For example: don't `new Random()` — use `IRandom`.)

If you got the four in the right order, you have the move. The fifth thing — one similar example from your own code — is the bonus you add once the four feel easy.

</details>

## Wrap up

1. **Quiz** — open `quiz.md`, jot your answers in `journal/quiz-notes.md`.
2. **Progress** — one line in `journal/progress.md`: `Module 4.0 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit and push** — stage both files, commit message `Module 4.0 done`, Sync.
4. **Post in `#wins`** — one line about today, plus the URL of the commit.

Module 0.1 covers the why and the panel/CLI steps if you need a refresher. Bring quiz answers you're least sure about to the next weekly sync.

## Next

Module 4.1 starts the actual browser work — HTML and CSS. The smallest useful page that shows your kingdom, opened straight from a `.html` file with no build step yet.

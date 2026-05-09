# Module 3.0 — Reading Code Before Writing It

Every module so far has been *write code*. Today is *read* code — somebody else's. Before we write our first ASP.NET endpoint, you'll spend about thirty minutes reading two real ones and saying out loud what they do. What you produce is a journal entry, not a program. That's the whole module: read, take notes, commit your notes.

Reading is the skill nobody teaches and everyone needs. The senior on a team reads roughly five times more code than they write. They know which file to open first, they scan rather than parse, they notice the *missing* things. None of that is innate — it's a habit you build by doing it on purpose, weekly, even when you're not chasing a bug.

> **Words to watch**
>
> - **read-don't-write hour** — a deliberate window where you only read code; no editing
> - **call graph** — who calls what; how a method's behaviour ripples through a project
> - **smell** — a not-yet-bug; something that makes you suspicious without being clearly broken

---

## Phase opener — `phase-3` branch

Before any code (the why is in Module 1.1):

```powershell
cd C:\code\kingdom
git switch -c phase-3
```

Every commit this phase lands on `phase-3`. At Module 3.9 (M4 close + AI Unlock), you'll PR it back to `main`.

---

## Why read first

The default learning loop most people use is *read tutorial → write thing → break → fix*. It works for syntax. It does not work for *judgement* — knowing why one way of structuring code is better than another. Judgement comes from reading lots of code and asking *"what does this person know that I don't?"*

The standard you can aim for is one hour of reading per week, deliberately, not just when you're hunting a bug. That's the habit this module starts.

## What you're doing today

Two files to read. Don't run them. Don't edit them. Just read.

The first one is small — a one-screen minimal API sample by the ASP.NET Core team:

> [`https://github.com/dotnet/aspnetcore/blob/main/src/Mvc/samples/MvcSandbox/Program.cs`](https://github.com/dotnet/aspnetcore/blob/main/src/Mvc/samples/MvcSandbox/Program.cs)

The second one is bigger — a real TODO API by David Fowler, one of the people who designed ASP.NET Core:

> [`https://github.com/davidfowl/TodoApi/blob/main/TodoApi/Program.cs`](https://github.com/davidfowl/TodoApi/blob/main/TodoApi/Program.cs)

For each file, answer these five questions in your `journal/3.0-reading.md` (template at the bottom of this lesson). One or two sentences each — handwritten reasoning, not Wikipedia summaries.

1. **What does this file do, in one English sentence?**
2. **What's the very first thing it sets up?** (`builder = WebApplication.CreateBuilder(args);` — what does that line actually do?)
3. **List the endpoints in order.** For each one: HTTP verb, path, what it returns.
4. **Find one thing that surprises you.** Some syntax you've never seen, a pattern you don't recognise. Write down what you don't yet understand.
5. **Find one thing you think you understand. Write a one-sentence summary.** When we cover it formally next module, you'll check whether you were right.

> Don't search for answers as you read. The point is to *try* understanding, then *check yourself* later. Wrong guesses are part of how you build judgement — they're where the new knowledge actually fits.

## Smell test

While you're reading, also note things that look a bit off. Real codebases are full of these. Examples:

- Anything that looks copy-pasted that could have been a helper
- Variable names that say nothing (`var thing = ...`)
- Long methods doing many different things
- Magic numbers — `if (x > 100)` — why a hundred? Where does that number come from?

Most of the time, the answer for code in the wild is *"yes, it's not perfect, but the cost of fixing is higher than the cost of leaving."* Knowing what to fix and what to live with is also a senior judgement. Reading like this is how you build the eye for it.

## What ships in the starter

This module is reading plus a journal entry. The starter folder ships:

- `journal/3.0-reading.md` — a template with the five questions to fill in for each file

No code. No tests. What you keep is your written notes.

The template:

```markdown
# Module 3.0 — Reading code

## File 1: aspnetcore/MvcSandbox/Program.cs

1. One-sentence summary:
2. First setup line + what it does:
3. Endpoints (verb, path, returns):
4. Surprise:
5. What I think I understand:

## File 2: davidfowl/TodoApi/Program.cs

1. One-sentence summary:
2. First setup line + what it does:
3. Endpoints (verb, path, returns):
4. Surprise:
5. What I think I understand:

## Smells noticed (across both files)

-
-

## Total reading time
```

## Tinker

Pick a third file from the `dotnet/aspnetcore` repo — anything that catches your eye. Time-box it to fifteen minutes. Whatever you read, write one sentence about it in your journal.

Compare the two files you read. How do their styles differ? `MvcSandbox` is a one-screen demo; `TodoApi` is a real application. Both are written by people who know the framework deeply. Neither is wrong. Style varies by purpose, and noticing that is itself a step forward.

**Commit** your `journal/3.0-reading.md` to your repo. *"Module 3.0 reading notes"* is a fine message. (Source Control panel → stage → commit → Sync. Or CLI: `git add . && git commit -m "Module 3.0 reading notes" && git push`.) Reading-and-noting is real work, and a real commit makes it visible in your git log. Future-you scrolling through commits in three months will see *"yep, that was the day I started reading other people's code on purpose."*

## Reading as a deliverable

The notes you write today are what you keep for this module. Coming sessions will refer back to them — *"compare the surprise you wrote down to what we just learned."* The journal is the through-line; treat it like any other commit.

## What you just did

You spent thirty minutes reading two real ASP.NET Core files written by the people who built the framework — and you wrote down what you saw, including what surprised you and what you guessed at. No code, no tests, just notes. That seems small, but it starts the habit that separates beginners from intermediates: senior developers read far more code than they write, and they read on purpose. By committing your `journal/3.0-reading.md` to your repo, you also treated reading as a real deliverable instead of an unpaid prerequisite. Next module you'll meet the same patterns from inside, in your own code.

**Key concepts you can now name:**

- **read-don't-write hour** — reading time set aside on purpose
- **call graph** — following who calls what through a project
- **smell** — a pattern that makes you uneasy, not always a bug
- **the five questions** — a reusable mini-protocol for any unfamiliar file

## Wrap up

1. **Quiz** — open `quiz.md`, jot your answers in `journal/quiz-notes.md`.
2. **Progress** — one line in `journal/progress.md`: `Module 3.0 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit and push** — stage both files, commit message `Module 3.0 done`, Sync.
4. **Post in `#wins`** — one line about today, plus the URL of the commit.

Module 0.1 covers the why and the panel/CLI steps if you need a refresher. Bring quiz answers you're least sure about to the next weekly sync.

## Next

Module 3.1 starts the actual API: HTTP basics, the minimal API setup you just read about, and your first endpoint serving your kingdom over the internet.

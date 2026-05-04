# Module 3.0 — Reading Code Before Writing It

> **Hook:** every other module so far has been *write code*. Today is *read* code — somebody else's. **Reading is the senior skill that nobody teaches and everyone needs.** Before we write our first ASP.NET endpoint, you'll spend 30 minutes reading two real ones and saying out loud what they do.

> **Words to watch**
> - **read-don't-write hour** — a deliberate window where you only read; no editing
> - **call graph** — who calls what; the way a method's behaviour ripples
> - **smell** — a not-yet-bug that makes you suspicious

---

## Why read first

The default learning loop is `read tutorial → write thing → break → fix`. It works for syntax. It doesn't work for *judgement* — knowing why one structure is better than another. Judgement comes from reading lots of code and asking *"what does this person know that I don't?"*

The senior on your team reads 5x more code than they write. They know which file to open first. They scan rather than parse. They notice missing things. **All of that is a learnable habit.** The bar is reading 1 hour a week — deliberately, not just when you're hunting a bug.

## Today's exercise

Open this file in your browser:

> [`https://github.com/dotnet/aspnetcore/blob/main/src/Mvc/samples/MvcSandbox/Program.cs`](https://github.com/dotnet/aspnetcore/blob/main/src/Mvc/samples/MvcSandbox/Program.cs)

That's a small minimal-API sample by the ASP.NET Core team. **Don't edit. Don't run.** Just read.

Open this companion:

> [`https://github.com/davidfowl/TodoApi/blob/main/TodoApi/Program.cs`](https://github.com/davidfowl/TodoApi/blob/main/TodoApi/Program.cs)

A larger real-world example: a complete TODO API by David Fowler (one of the ASP.NET Core architects).

## The five questions

For each file, answer in 1-2 sentences each, in your `journal/3.0-reading.md`:

1. **What does this file do, in one English sentence?**
2. **What's the very first thing it sets up?** (`builder = WebApplication.CreateBuilder(args);` — what does that do?)
3. **List the endpoints in order. For each: HTTP verb, path, what it returns.**
4. **Find one thing that surprises you.** It might be a syntax (`builder.Services.AddSingleton<X>()`) or a pattern (`MapGroup("/todos")`). Write down what you don't yet understand.
5. **Find one thing you think you understand. Write a one-sentence summary.** When we cover it formally next module, check whether you were right.

> Don't Google answers as you read. The point is to *try* understanding, then *check yourself* later. Wrong guesses are part of how you build judgement.

## Smell test

While reading, also note:

- Anything that looks copy-pasted (might want to extract a helper)
- Variable names that say nothing (`var thing = ...`)
- Long methods doing many things
- Magic numbers (`if (x > 100)` — why 100?)

You'll see all of these in real codebases. **Most of the time the answer is "yes, it's not great, but the cost of fixing is higher than the cost of leaving."** That's also a senior judgement: knowing what to fix and what to live with.

## Delta starter

This module is reading + a journal entry. The starter ships:

- `journal/3.0-reading.md` template — five questions to fill in for each file

No code. No tests. The artefact is your written notes.

`journal/3.0-reading.md` template:

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

- Read a third file: pick *anything* in the `dotnet/aspnetcore` repo that interests you. Time-box: 15 minutes. Whatever you read, write one sentence about it in your journal.
- Compare the two files: how do their styles differ? `MvcSandbox` is a one-screen demo; `TodoApi` is a real app. Both are written by experts; neither is wrong. **Style varies by purpose.**
- Commit your `journal/3.0-reading.md` to your repo. **Reading-and-noting is real work.** Make it visible in your git log.

## Name it

- **Read-don't-write hour.** Time deliberately reserved for reading code, not editing it.
- **Call graph (informal).** Mapping who calls what; a way to read a codebase by following the threads.
- **Smell.** A pattern that makes you uneasy. Not always a bug. Worth noticing.
- **Five questions.** A reusable mini-protocol for any unfamiliar file: what / first setup / endpoints / surprise / claimed understanding.

## The rule of the through-line

> **Treat reading as a deliverable. The notes in `journal/3.0-reading.md` are your artefact for this module.**

Coming sessions will refer back to those notes — *"compare the surprise you wrote down to what we just learned."*

## Quiz / challenge

Open `quiz.md`.

## Connect

Module 3.1 starts the actual API: HTTP basics, the minimal API setup you just read about, and your first endpoint.
# Module 3.0 — Reading Code Before Writing It

Every module so far has been about *writing* code. Today is about *reading* code that someone else wrote. Before we write our first ASP.NET endpoint, you'll spend about thirty minutes reading two real ones and saying out loud what they do. What you make today is a journal entry, not a program. That's the whole module: read, take notes, commit your notes.

Reading is a skill nobody teaches but everyone needs. An experienced developer on a team reads about five times more code than they write. They know which file to open first. They scan instead of reading every word. They notice when something is *missing*. Nobody is born knowing how to do this. It's a habit you build by doing it on purpose, every week, even when you're not chasing a bug.

> **Words to watch**
>
> - **read-don't-write hour** — a deliberate window where you only read code; no editing
> - **call graph** — who calls what; how a change in one method affects the rest of a project
> - **smell** — not yet a bug; something that makes you suspicious without being clearly broken

---

## Phase opener — `phase-3` branch

Before any code (the why is in Module 1.1):

```powershell
cd C:\code\kingdom
git switch -c phase-3
```

Every commit this phase goes on `phase-3`. At Module 3.9 (M4 close + AI Unlock), you'll open a pull request to merge it back into `main`.

---

## Why read first

Most people learn like this: read a tutorial, write something, break it, fix it. That works for learning syntax. It does not teach *judgement* — knowing why one way of laying out code is better than another. Judgement comes from reading a lot of code and asking *"what does this person know that I don't?"*

A good target is one hour of reading per week, on purpose, not just when you're hunting a bug. That's the habit this module starts.

## What you're doing today

Two files to read. Don't run them. Don't edit them. Just read.

The first one is small — a one-screen minimal API sample by the ASP.NET Core team:

> [`https://github.com/dotnet/aspnetcore/blob/main/src/Mvc/samples/MvcSandbox/Program.cs`](https://github.com/dotnet/aspnetcore/blob/main/src/Mvc/samples/MvcSandbox/Program.cs)

The second one is bigger — a real TODO API by David Fowler, one of the people who designed ASP.NET Core:

> [`https://github.com/davidfowl/TodoApi/blob/main/TodoApi/Program.cs`](https://github.com/davidfowl/TodoApi/blob/main/TodoApi/Program.cs)

For each file, answer these five questions in your `journal/3.0-reading.md` (there's a template at the bottom of this lesson). One or two sentences each. Write your own reasoning in your own words — don't copy a summary from somewhere else.

1. **What does this file do, in one English sentence?**
2. **What's the very first thing it sets up?** (`builder = WebApplication.CreateBuilder(args);` — what does that line actually do?)
3. **List the endpoints in order.** For each one: HTTP verb, path, what it returns.
4. **Find one thing that surprises you.** Some syntax you've never seen, or a pattern you don't recognise. Write down what you don't understand yet.
5. **Find one thing you think you understand. Write a one-sentence summary.** When we cover it properly next module, you'll check whether you were right.

> Don't search for answers as you read. The point is to *try* to understand first, then *check yourself* later. Wrong guesses are part of how you build judgement. A wrong guess is where the right answer fits in once you learn it.

## Smell test

While you're reading, also note things that look a little off. Real codebases are full of these. Examples:

- Code that looks copy-pasted, where a helper method would have done the job
- Variable names that tell you nothing (`var thing = ...`)
- Long methods doing many different things at once
- Magic numbers — `if (x > 100)` — why a hundred? Where does that number come from?

Most of the time, with real code, the answer is *"yes, it's not perfect, but fixing it would cost more than leaving it alone."* Knowing what to fix and what to leave alone is also part of an experienced developer's judgement. Reading like this is how you learn to spot it.

## What ships in the starter

This module is reading plus a journal entry. The starter folder gives you:

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

Pick a third file from the `dotnet/aspnetcore` repo — anything that catches your eye. Give it fifteen minutes, no more. Whatever you read, write one sentence about it in your journal.

Compare the two files you read. How are their styles different? `MvcSandbox` is a one-screen demo. `TodoApi` is a real application. Both are written by people who know the framework very well. Neither one is wrong. The style changes with the job, and noticing that is itself a step forward.

**Commit** your `journal/3.0-reading.md` to your repo. *"Module 3.0 reading notes"* is a fine message. (Source Control panel → stage → commit → Sync. Or in the terminal: `git add . && git commit -m "Module 3.0 reading notes" && git push`.) Reading and taking notes is real work, and a real commit makes it show up in your git log. In three months you'll scroll past this commit and think *"that was the day I started reading other people's code on purpose."*

## Your notes are the result

The notes you write today are what you keep for this module. Later sessions will point back to them — *"compare the surprise you wrote down to what we just learned."* The journal runs through the whole course. Treat it like any other commit.

## What you just did

You spent thirty minutes reading two real ASP.NET Core files written by the people who built the framework. You wrote down what you saw, including what surprised you and what you guessed at. No code, no tests, just notes. That seems small, but it starts a habit that sets stronger developers apart: they read far more code than they write, and they read on purpose. By committing your `journal/3.0-reading.md` to your repo, you treated reading as real work that counts, not just a step on the way to writing. Next module you'll meet the same patterns from the inside, in your own code.

**Key concepts you can now name:**

- **read-don't-write hour** — reading time set aside on purpose
- **call graph** — following who calls what through a project
- **smell** — a pattern that makes you uneasy, not always a bug
- **the five questions** — a set of questions you can reuse on any new file

## On your own

Time to put the book away. Don't scroll back up to the steps — from your own head, name the five questions you asked of each file you read today. No one marks this one — it's just for you. It's the easiest way to spot what *hasn't* stuck yet, while it's still simple to fix. Getting stuck here is completely fine — that's exactly what it's for.

Take a blank piece of paper. Without looking, write down all five questions you answer for every new file. Then say why each one helps you understand code you didn't write.

<details><summary>Stuck? Open this to check yourself.</summary>

The five questions:

1. **What does this file do, in one English sentence?**
2. **What's the very first thing it sets up?**
3. **List the endpoints in order** (verb, path, what it returns).
4. **Find one thing that surprises you** — syntax or a pattern you don't know yet.
5. **Find one thing you think you understand** — write a one-sentence summary.

The point of the set: question 1 forces the big picture, question 2 finds the starting line, question 3 maps the real work, question 4 marks what to learn next, and question 5 gives you something to check later. You can use this same list on any new file for the rest of your life.

</details>

## Wrap up

1. **Quiz** — open `quiz.md`, jot your answers in `journal/quiz-notes.md`.
2. **Progress** — one line in `journal/progress.md`: `Module 3.0 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit and push** — stage both files, commit message `Module 3.0 done`, Sync.
4. **Post in `#wins`** — one line about today, plus the URL of the commit.

Module 0.1 covers the why and the panel/CLI steps if you need a refresher. Bring quiz answers you're least sure about to the next weekly sync.

## Next

Module 3.1 starts the real API: HTTP basics, the minimal API setup you just read about, and your first endpoint serving your kingdom over the internet.

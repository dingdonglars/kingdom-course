# AI tools — for you

This file is the plain-English version of how AI fits into the course. The machine-side version (the one Claude itself reads as a system prompt) is at `ai-context/CLAUDE.md` — same rules, written for the AI to enforce. Your repo's root `CLAUDE.md` imports it automatically when you run Claude Code, so the rules apply without you having to ask.

## What you'll be using

You'll have your own personal Claude subscription. Lars sets it up with you when you start; the tier is whatever Lars picks (he'll tell you). **Claude is your default AI assistant for the year.** Most weeks you'll talk to it through Claude Code in your terminal, occasionally through claude.ai in the browser.

You don't have to use Claude exclusively. If you end up working somewhere later that uses **Cursor** (an AI-aware code editor), or **GitHub Copilot** (AI completions inside VS Code), the patterns transfer — same way of asking, same discipline about reading the output. The course teaches Claude because that's the one tool I (Lars) can guide you on directly. Pick up the others when the moment comes.

What the course does *not* teach you to use: AI as an autopilot. You will write code in this course. AI helps; AI doesn't replace you.

## The three buckets

There are three categories of AI use in the course. The middle one expands once at a specific moment — the **AI Unlock**, which fires at the end of Phase 3 (after M4). Before the unlock, AI is mostly for friction. After, it can also help you write code, with one hard rule.

### 🟥 Don't ask AI for — ever

These are the things where doing them yourself *is* the learning.

- Write the code for an exercise
- Solve a quiz question
- Design the next feature of your Kingdom
- Refactor your code into something you didn't write
- Explain a concept you haven't tried to read about yourself yet

If you ask Claude for these, Claude will push back: *"this looks like a course exercise — let's walk through it together instead of me writing it."*

### 🟨 OK to ask AI for — when you ask on purpose

This is the bucket that **expands at the AI Unlock.**

**Before the unlock** (you're here until you finish Phase 3):

- Get out of git messes — yes, ask Claude to fix your detached HEAD
- Environment setup, install errors, *"why won't this build?"*
- Explain an error message — *after* you've already tried to read it yourself
- Code review — Claude points out issues; Claude does **not** write the fix
- Routine boilerplate (a `Program.cs` skeleton, an empty xUnit test class)
- Explain a concept — *after* you've genuinely tried to read about it yourself

**After the unlock** (Phase 4 onward): all of the above, **plus** you can ask Claude to help **write implementation code** — under the hard rule below.

### 🟩 Always fine — use freely

- Look up syntax or docs (*"how do I write a switch expression in C# 12?"*)
- Formatting and linting
- Translate a phrase
- Suggest names for variables, methods, classes
- Sanity-check a tiny snippet (*"is this good practice?"*)
- Ask what an English word means — this never counts against the 20-minute rule

## The hard rule (turns on at the AI Unlock)

> **If you cannot explain every line Claude wrote, the AI's contribution gets removed and you write it yourself.**

That's the whole rule. It exists so you don't end up shipping code you don't understand — which is the one failure mode this course is built to prevent. Code you can't explain is code you can't fix later, and code you can't fix later is technical debt with your name on it.

The rule is **honor system**. Lars cannot read your mind. But the rule lives inside the milestone reviews — when you open a post-unlock milestone PR, you fill in an "AI assistance" section (which files were AI-helped). At the milestone review, Lars walks you through some of those lines and asks *"explain this one to me."* Honest disclosure that turns out you don't understand a line is a learning moment, not a strike. Hiding AI use to avoid the conversation is the failure.

## Why the unlock exists

By the time you graduate from this course, your peers in any junior developer job will be using AI assistants every day. If the course taught you without AI, you'd graduate into a museum piece. If the course let you use AI for everything from day one, you'd graduate as someone who types prompts but can't read his own code.

The unlock splits the difference: get good at the basics first, *then* learn to drive the AI from a position of strength. The named skill on the other side of the unlock is **context engineering** — making the AI useful by handing it the right files, constraints, and example code. That's a real skill, and it depends on understanding what's in your codebase. Phase 3 ends; you've earned it; the unlock fires.

## The 20-minute rule

This is a non-AI rule, but it lives next to AI use, so name it now: before pinging Lars in `#help`, try yourself for 20 minutes. *Then* ask Claude. If Claude can't help, *then* ping Lars — and show what you tried.

This is universal developer culture. The full thing is in the Mentor Protocol Lars gives you separately. The short version: Lars's time is precious; Claude's is not; your time spent struggling at the keyboard is the time when learning happens fastest.

## Slash commands you have on day 1

Your repo ships with six Claude Code slash commands. Type `/` in the Claude Code prompt to list them; the four most useful in daily work:

- **`/explain-this-concept`** — beginner-level walkthrough of a concept, calibrated to what you already know.
- **`/code-review`** — Claude points out issues in code you wrote. Yellow bucket: **does not write the fix.**
- **`/stuck-on-error`** — diagnose an error after the 20-minute rule. Hint, not a complete fix.
- **`/walk-through-code`** — line-by-line explanation in plain English; stops at the bits that are likely to surprise a beginner.

The other two — `/lesson-review` and `/milestone-review` — you'll meet in the lessons that introduce them.

These are the same patterns the previous version of this course used to deliver as paste-the-prompt files. The slash command form is just easier — type `/<name>`, fill in the gap when Claude asks, done.

## When in doubt

Ask Lars. The protocol exists to be questioned.

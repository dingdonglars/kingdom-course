# AI Tools — For You

> This is the plain-English version of how to use AI in this course. The machine-side version is at `ai-context/CLAUDE.md` — same rules, written for AI to enforce.

## The big idea

AI is allowed in this course. **Encouraged**, even. But there are three buckets, and the boundary between them moves once at a specific moment in the course (the **AI Unlock Gate** at end of Block 5 / after M4).

Before the gate, AI is for **friction**. After the gate, AI can also help with **implementation** — under one hard rule.

## The three buckets

### 🟥 Don't ask AI for — *ever*

These are the things where doing them yourself **is the learning.**

- Write the code for an exercise.
- Solve a quiz question.
- Design the next feature of your Kingdom.
- Refactor your code into something you didn't write.
- Explain a concept you haven't tried to read about yourself yet.

If you ask Claude for these, Claude will push back: *"this looks like a course exercise — let's walk through it together instead of me writing it."*

### 🟨 OK to ask AI for — *when you ask explicitly*

This is the bucket that **expands at the gate**.

**Before the gate** (you're in this phase now if Module 4.0 hasn't run yet):
- **Get out of git messes** (yes, ask Claude to fix your detached HEAD)
- Environment setup, install errors, "why won't this build"
- Explain an error message *after* you've already tried to read it
- Code review — Claude points out issues; *Claude does not write the fix*
- Generate routine boilerplate (a `Program.cs` with `Main`, an `xUnit` test class skeleton)
- Explain a concept *after* you've genuinely tried

**After the gate** (Module 4.0 onward): all of the above, **plus** you can ask Claude to help **write implementation code** — under the hard rule below.

### 🟩 Always fine — use freely

- Look up syntax / docs (*"how do I write a switch expression in C# 12?"*)
- Format / lint
- Translate a phrase
- Suggest names
- Sanity-check a tiny snippet (*"is this good practice?"*)
- Ask what an English word means (this never counts against the 20-minute rule)

## The hard rule (turns on at the gate)

> **If you cannot explain every line that Claude wrote, the AI's contribution gets rolled back and you write it yourself.**

That's it. That's the whole rule. It exists so you don't end up shipping code you don't understand — which is the *one* failure mode this course is built to prevent.

## How `/milestone-review` works (post-gate)

When you open a milestone PR after the gate, you fill in an "AI assistance" section (the PR template prompts you):

- AI-assisted in this PR? yes / no
- If yes, which files / chunks?

This is **honor system**. Lars will use it to walk you through the AI-assisted code line-by-line at milestone review. Honest disclosure that turns out you don't understand a line is a **learning moment**, not a strike. Hiding AI use to avoid the conversation is the failure.

## Why bother with all this?

By 2027, your peers will be shipping with Cursor and Claude. If we taught you the curriculum without AI, you'd graduate into a museum piece. If we let you use AI for everything from Day 1, you'd end up a prompt-typist who can't read his own code. The gate splits the difference: master the substrate first, then learn to drive the AI from a position of competence.

The skill being taught after the gate is **context engineering** — making the AI useful by handing it the right files, constraints, and tests. It's a real engineering skill, and it requires understanding the codebase first. That's why the gate exists.

## When in doubt

Ask Lars. Or post in `#help`. The protocol exists to be questioned.

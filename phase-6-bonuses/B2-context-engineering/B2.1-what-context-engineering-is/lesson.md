# Bonus B2.1 — What Context Engineering Is

You have been doing context engineering since the AI Unlock at Module 4.0. You probably haven't called it that. Today we give it a name. By the end of B2, every time you use an AI tool you'll see four parts you can change — instead of a black box you ask things and hope.

The skill works a lot like editing. Most of the value is in two places: the choices you make *before* you write the first prompt, and the way you read the answer *after* the AI writes it. The middle part — the AI actually writing — is the part you don't control. The whole point of context engineering is to spend your effort on the parts you do control.

> **Words to watch**
>
> - **context engineering** — the deliberate practice of choosing what the AI sees before it answers
> - **prompt** — the instruction itself; one part of the context
> - **scaffolding** — persistent background context (project files the AI can read)
> - **scoping** — per-task framing — what's in and out for *this* request
> - **eval** — your judgement step; does the output fit your project?

---

## Why give it its own bonus

You unlocked AI implementation help at Module 4.0. By the time you reach here at the end of Phase 5, that has been months of practice — across code, content, debugging, even writing wins entries. B2 gathers what you have learned so far and sharpens it. Giving the skill a name makes you better at it.

The AI Unlock at Module 4.0 set the rule: you write the first version yourself, then bring the AI in for the second. B2 is about *how* you bring the AI in — what you hand it, and how you read what comes back.

## The four-step frame

Every time you use an AI tool, there are four steps:

1. **Prompt** — what you ask, in a narrow way. Your message.
2. **Context** — everything else the AI sees: the system prompt, the project files it can read, earlier turns in the chat, and its tools.
3. **Output** — what comes back from the model.
4. **Eval** — your judgement on whether the output is right *for your project*.

You can change three of these yourself. The fourth — the actual writing — you can't. So the rule is to spend your effort on the three you control: the prompt, the context, and the eval.

## Where each B2 module sits

The five B2 modules each sharpen one part of the frame:

- **B2.1** (this one) — the frame itself, and what counts as context
- **B2.2** — scaffolding (the background that stays around — `ARCHITECTURE.md`, `STANDARDS.md`, examples)
- **B2.3** — scoping (per-task: goal, non-goals, traps, success criteria)
- **B2.4** — reading output carefully (the eval step — invented APIs, edge cases missed, style drift)
- **B2.5** — comparing AI tools, then the closing reflection

## Three failure modes worth naming

When AI output is bad, it is usually bad in one of three ways. Knowing the three names is half the cure — once you can spot them in seconds, you stop accepting them.

1. **Generic-tutorial output.** The AI didn't see your project, so it gave you what works in textbooks instead of what fits your code. The fix is more scaffolding (B2.2 covers this).
2. **Invented APIs.** The AI confidently calls methods that don't exist. The fix is to read every line and grep the ones that look wrong (B2.4 covers this).
3. **Drift from style.** Your codebase uses one pattern; the AI gives you a different one. The fix is to point at an existing example in your scoping (*"match the style of `KingdomEfStore.cs`"* — B2.3 covers this).

These three cover almost every bad AI output you will get. The rest of B2 turns each one into a habit.

## Tinker

Look back at three AI conversations from the last week or two. For each one, rate the output as good, okay, or bad. For the bad ones, ask: *what context was missing?* Was it scaffolding (the AI didn't know your project), scoping (you didn't say what was in or out), or eval (you accepted something you shouldn't have)?

Pick a tiny task — something five lines long. Try it twice. Once with no context (just the goal in one sentence), and once with the full Module 4.0 recipe. Compare the two outputs. The difference between them is the value of context engineering.

Read your `CLAUDE.md` from start to finish. Notice how often the AI has been guided by it without you doing anything per-prompt. The file is doing context engineering for you in the background.

## What you just did

You met the four-step frame — prompt, context, output, eval — and the rule that goes with it: three of the four are yours to change, one isn't, so spend your effort on the three you control. You also met the three most common ways AI output goes wrong (generic-tutorial output, invented APIs, drift from style) that cover almost every bad AI output. The rest of B2 turns this frame into specific habits.

**Key concepts you can now name:**

- **context engineering** — choosing what the AI sees before it answers
- **the four-step frame** — prompt, context, output, eval
- **scaffolding vs scoping** — persistent background versus per-task framing
- **eval** — your judgement on whether the output fits your project
- **the three failure modes** — generic-tutorial, invented APIs, style drift

## On your own

Time to put the book away. Don't scroll back up to the steps — prove to yourself, from your own head, that the big idea stuck. No one marks this one — it's just for you. It's the easiest way to spot what hasn't stuck yet, while it's still simple to fix. Getting stuck here is completely fine — that's exactly what it's for.

Without scrolling back up, write the four-step frame from memory: name all four steps, then mark which three you can change and which one you can't. Then say the rule that follows from that.

<details><summary>Stuck? Open this to check yourself.</summary>

The four steps:

1. **Prompt** — what you ask. (You control this.)
2. **Context** — everything else the AI sees: system prompt, project files, earlier turns, tools. (You control this.)
3. **Output** — what comes back from the model. (You do *not* control this.)
4. **Eval** — your judgement on whether the output fits your project. (You control this.)

The rule: spend your effort on the three you control — prompt, context, and eval — not on the one you can't.

</details>

## Wrap up

1. **Quiz** — open `quiz.md`, jot your answers in `journal/quiz-notes.md`.
2. **Progress** — one line in `journal/progress.md`: `Module B2.1 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit and push** — stage both files, commit message `Module B2.1 done`, Sync.
4. **Post in `#wins`** — one line about today, plus the URL of the commit.

Module 0.1 covers the why and the panel/CLI steps if you need a refresher. Bring quiz answers you're least sure about to the next weekly sync.

## Next

B2.2 goes deeper on **scaffolding** — the background files that keep every AI conversation in your style without you re-explaining each time.

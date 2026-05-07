# Bonus B2.1 — What Context Engineering Is

You've been doing context engineering since the AI Unlock at Module 4.0. You probably haven't called it that. Today we name it. By the end of B2, every interaction with an AI tool will look like four design surfaces you can change — instead of a black box you ask things and hope.

The skill itself is shaped a lot like editing. Most of the value sits in two places: the decisions you make *before* you write the first prompt, and the way you read the response *after* the AI generates. The middle bit — the AI's actual generation — is the part you don't control. The whole point of context engineering is to put your energy on the parts you do.

> **Words to watch**
>
> - **context engineering** — the deliberate practice of choosing what the AI sees before it answers
> - **prompt** — the instruction itself; one part of the context
> - **scaffolding** — persistent background context (project files the AI can read)
> - **scoping** — per-task framing — what's in and out for *this* request
> - **eval** — your judgement step; does the output fit your project?

---

## Why give it its own bonus

You unlocked AI implementation help at Module 4.0. By the time you get here at the end of Phase 5, that's been months of accumulated practice — across code, content, debugging, even drafting wins entries. B2 collects what you've learned so far and sharpens it. Naming the skill makes you better at it.

The AI Unlock at Module 4.0 set the rule (you write the first version yourself, then bring AI in for the second). B2 is about *how* the bringing-in happens — what you hand to the AI and how you read what comes back.

## The four-step frame

Every interaction with an AI tool is four steps:

1. **Prompt** — what you ask, narrowly. The user message.
2. **Context** — everything else the AI sees: system prompt, project files it has access to, prior turns, tools.
3. **Output** — what comes back from the model.
4. **Eval** — your judgement on whether the output is right *for your project*.

You can change three of these directly. The fourth — the actual generation — you can't. So the discipline is to spend your effort on the three you control: the prompt, the context, and the eval.

## Where each B2 module sits

The five B2 modules each sharpen one piece of the frame:

- **B2.1** (this one) — the frame itself; what counts as context
- **B2.2** — scaffolding (the persistent background — `ARCHITECTURE.md`, `STANDARDS.md`, examples)
- **B2.3** — scoping (per-task: goal, non-goals, traps, success criteria)
- **B2.4** — reading output critically (the eval step — invented APIs, edge cases missed, style drift)
- **B2.5** — comparing AI tools, then the closing reflection

## Three failure modes worth naming

When AI output is bad, it's usually bad in one of three ways. Naming the failure modes is half the cure — once you can spot them in seconds, you stop accepting them.

1. **Generic-tutorial output.** The AI didn't see your project, so it gave you what works in textbooks rather than what fits your code. The fix is more scaffolding (B2.2 covers this).
2. **Invented APIs.** The AI confidently calls methods that don't exist. The fix is to read every line and grep the suspicious ones (B2.4 covers this).
3. **Drift from style.** Your codebase uses one pattern; the AI gives you a different one. The fix is to point at an existing example in your scoping (*"match the style of `KingdomEfStore.cs`"* — B2.3 covers this).

These three account for almost every bad AI output you'll get. The rest of B2 turns each into a habit.

## Tinker

Look back at three AI conversations from the last week or two. For each, classify the output as good, mid, or bad. For the bad ones, ask: *what context was missing?* Was it scaffolding (the AI didn't know your project), scoping (you didn't say what was in or out), or eval (you accepted something you shouldn't have)?

Pick a tiny task — something five lines long. Try it twice. Once with no context (just the goal in one sentence), once with the full Module 4.0 recipe. Compare the outputs. The gap between them is the value of context engineering.

Read your `CLAUDE.md` end to end. Notice how often the AI has been steered by it without you doing anything per-prompt. The file is doing context engineering for you in the background.

## What you just did

You met the four-step frame — prompt, context, output, eval — and the rule that goes with it: three of the four are yours to change, one isn't, so spend your energy on the three you control. You also met the three high-frequency failure modes (generic-tutorial output, invented APIs, drift from style) that account for almost every bad AI output. The rest of B2 turns this frame into specific habits.

**Key concepts you can now name:**

- **context engineering** — choosing what the AI sees before it answers
- **the four-step frame** — prompt, context, output, eval
- **scaffolding vs scoping** — persistent background versus per-task framing
- **eval** — your judgement on whether the output fits your project
- **the three failure modes** — generic-tutorial, invented APIs, style drift

## Quiz

Open `quiz.md`. When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.

## Next

B2.2 deepens **scaffolding** — the persistent background that keeps every AI interaction on-style without you re-explaining each time.

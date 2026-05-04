# Bonus B2.1 — What Context Engineering Is

> **Hook:** you've been doing context engineering since M4.0. **B2 names it explicitly + sharpens it.** This module sets the frame for the next four. By the end of B2, you'll think about every AI interaction as *"what context did I give it, and what did it produce given that context."*

> **Words to watch**
> - **context engineering** — the deliberate practice of choosing what the AI sees before answering
> - **prompt** — the instruction (one part of the context)
> - **scaffolding** — the *background* context — files, snippets, references — that shapes interpretation
> - **scoping** — defining what's in/out of the AI's task
> - **eval** — judging the output against your standards (not the AI's)

---

## Why give it its own bonus

You unlocked AI implementation help at M4.0. By M6, you've been using it across code, content, debugging, even writing wins entries. That's six months of accumulated practice. **B2 collects the lessons.**

The skill is shaped like editing: most of the value is decisions you make *before* writing prompt #1, and *while reading* response #1. The middle (the AI's actual generation) is the part you don't control. Focus your energy on the edges.

## The frame: prompt → context → output → eval

Every interaction is four steps:

1. **Prompt** — what you ask, narrowly.
2. **Context** — what the AI sees in addition to your prompt (system prompt, files, prior turns, tools).
3. **Output** — what comes back.
4. **Eval** — your judgement of whether it's right *for your project*.

Each step is engineerable. You don't directly control the AI's generation, but you control everything around it. **Treat all four as design surfaces.**

## Where each B2 module sits

- **B2.1** (this one) — frame; what counts as context
- **B2.2** — context scaffolding (the persistent background — `ARCHITECTURE.md`, `STANDARDS.md`, examples)
- **B2.3** — prompt scoping (per-task: goal + non-goals + traps)
- **B2.4** — reading output critically (invented APIs, drift from style, edge cases missed)
- **B2.5** — comparing tools + the reflection close

## Three failure modes to watch

1. **Generic-tutorial output.** AI didn't see your project; gave you what works in textbooks. Cure: give it more scaffolding (M4.0 + B2.2).
2. **Invented APIs.** AI confidently calls methods that don't exist. Cure: read every line; grep the suspicious ones.
3. **Drift from style.** Your codebase uses one pattern; AI gives you a different one. Cure: include "match the style of `KingdomEfStore.cs`" in the scoping.

These three account for ~all the bad AI outputs you'll get. **Naming them is half the cure.**

## Tinker

- Look back at three AI conversations from the last week. For each: classify the output as good / mid / bad. Ask: *what context was missing in the bad one?*
- Pick a tiny task. Try it twice — once with no context (just goal), once with full M4.0 recipe. Compare outputs.
- Read your `ai-context/CLAUDE.md` end to end. Notice you've been steered by it without thinking. **The file is doing context engineering for you.**

## Name it

- **Context** — everything the AI sees other than the user message of the moment.
- **Prompt** — the user message itself.
- **Scaffolding** — persistent background context (config, files).
- **Scoping** — per-task context (this specific ask).
- **Eval** — your judgement step.

## The rule of the through-line

> **You don't control the AI. You control its context. Optimise the part you control.**

## Quiz / challenge

Open `quiz.md`.

## Connect

B2.2 deepens **scaffolding** — the persistent background that keeps every interaction on-style without you re-explaining each time.
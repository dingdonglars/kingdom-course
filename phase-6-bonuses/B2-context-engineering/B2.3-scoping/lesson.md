# Bonus B2.3 — Scoping (Per-Task Framing)

Scaffolding sets the stage. Scoping is what you say *for this specific task.* Yesterday's lesson was about the persistent files the AI reads on every session; today's is about the per-prompt framing that turns a vague request into a clean ask. Scoping is the difference between *"write me a method that finds the kingdom with the most gold"* and *"in `KingdomEfStore.cs`, add `LoadRichest(string ownerSub)` returning a `KingdomSlotInfo?`, matching the projection style of `ListSlots`, with a test in `SlotCrudTests.cs`."*

The first prompt makes the AI guess your boundaries. The second one tells it. Three minutes typing the second saves ten minutes of cleanup on what comes back.

> **Words to watch**
>
> - **scoping** — defining what's in and out of the AI's task for this specific request
> - **non-goal** — something the AI should *not* do
> - **trap** — a known pitfall to call out explicitly
> - **success criterion** — how you'll judge "done"
> - **single example** — one concrete reference snippet, in the message itself

---

## Why scoping matters

Without scoping, the AI guesses your boundaries. When it guesses, it tends to guess *more* — which usually means:

- Refactoring surrounding code you didn't ask about (often badly).
- Adding features you didn't request (*"I also added validation!"*).
- Explaining at length when you wanted code.
- Coding at length when you wanted explanation.

A scoped prompt prevents all four by stating goal, non-goals, format expectation, and success criterion up front.

## The scoping template

You already have this from Module 4.0 — it lives as the `/implementation-help` slash command at `.claude/commands/implementation-help.md`. The five slots it asks for are:

```markdown
**Goal (one sentence):**

**File path and surrounding context:**

**Relevant existing code (paste 1-3 small snippets):**

**Conventions to follow:**

**What you should NOT do:**

**My understanding:** I'll be asked to explain each line you write.
```

B2.3 makes one upgrade — add explicit success criteria:

```markdown
**Done when:**
- The new method appears in `KingdomEfStore.cs` between `Delete` and `ListSlots`
- Existing tests still pass
- One new test exists in `tests/.../SlotCrudTests.cs`
- Output ends with the explanation prompt
```

Now you can read the response and tick the list. *Concrete done.*

## Worked example

**Bad prompt:**

> me: write me a method that finds the kingdom with the most gold for a given user

The AI invents a `Kingdom` type, uses LINQ on a `List<Kingdom>`, ignores your store entirely. Output is unrelated to your project.

**Better prompt:**

> me: in `KingdomEfStore.cs`, add `LoadRichest(string ownerSub): KingdomSlotInfo?` that returns the user's slot with the highest gold (or null if no kingdoms). Match the projection style of `ListSlots`. Add one test in `SlotCrudTests.cs` that creates three kingdoms with different gold values and asserts the right id comes back. Don't load full Kingdom entities — project to summary inline.

The AI writes the right method, in the right style, in the right file, with a real test. *Three minutes of typing the prompt saves ten of cleanup.*

## The three-sentence fallback

If the full template feels heavy for a small task, fall back to three sentences:

1. **In `<file>`, add `<methodSignature>` that does `<one sentence>`.**
2. **Match the style of `<existing method or example>`.**
3. **Don't `<known trap>`.**

Three sentences land 80% of small asks. Use the full template when the request is bigger than around 30 lines.

## Step — write your next AI prompt this way

Pick the next task you were planning to ask the AI about. Before you send the prompt, write it out in the three-sentence form. *Then* send it. Compare what comes back to what you would have got from the vague version.

You'll feel the difference quickly. That's the whole lesson.

## Tinker

Take a recent vague prompt you used. Rewrite it as a scoped prompt. Send both — same task, same model, different framings. Compare the outputs side by side.

Pick the worst output you've gotten in the last month. Diagnose: was it scaffolding, scoping, or eval that failed? Often it's scoping — the prompt didn't say what was in or out, and the AI filled the silence with assumptions.

Add two more example files to `.claude/examples/`. Reference them in your next five scoped prompts. Watch how often the AI snaps to the example.

## What you just did

You met the scoping template (goal, non-goals, traps, success criteria) and the three-sentence fallback for smaller asks. You saw a worked example where a vague prompt produced unrelated output and a scoped prompt produced exactly the method asked for. The point of scoping is simple: a prompt is a contract, and the AI delivers against the contract you wrote — vague contract, vague delivery.

**Key concepts you can now name:**

- **scoping** — per-task framing for this specific request
- **non-goal** — the explicit "don't do this" line
- **trap** — a known pitfall called out up front
- **success criterion** — how you judge "done"
- **the three-sentence prompt** — fallback for small asks

## Quiz

Open `quiz.md`. When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.

## Next

B2.4 covers **reading the output critically** — the eval step. The other half of doing this well: catching what slips through.

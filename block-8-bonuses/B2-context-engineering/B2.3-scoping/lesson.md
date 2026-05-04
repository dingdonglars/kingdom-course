# Bonus B2.3 — Scoping (Per-Task Framing)

> **Hook:** scaffolding sets the stage. **Scoping is what you say *for this specific task*.** Today's lesson: how to write a prompt that lands the right answer the first time, with explicit non-goals + traps + a single example.

> **Words to watch**
> - **scoping** — defining what's in/out of the AI's task
> - **non-goal** — something the AI explicitly should NOT do
> - **trap** — a known pitfall to call out
> - **single example** — one concrete reference snippet, in the message
> - **success criterion** — how you'll judge "done"

---

## Why scoping matters

Without scoping, the AI guesses your boundaries. It might:
- Refactor surrounding code you didn't ask about (often badly).
- Add features you didn't request ("I also added validation!").
- Explain at length when you wanted code.
- Code at length when you wanted explanation.

**A scoped prompt prevents all four** by stating goal, non-goals, format expectation, success criterion.

## The scoping template (pre-built; in `ai-context/prompts/implementation-help.md`)

You already have this from M4.0:

```markdown
**Goal (one sentence):**

**File path + surrounding context:**

**Relevant existing code (paste 1-3 small snippets):**

**Conventions to follow:**

**What you should NOT do:**

**My understanding:** I'll be asked to explain each line you write.
```

B2.3 makes one upgrade: **add success criteria.**

```markdown
**Done when:**
- The new method appears in `KingdomEfStore.cs` between `Delete` and `ListSlots`
- Existing tests still pass
- One new test exists in `tests/.../SlotCrudTests.cs`
- Output ends with the explanation prompt
```

Now you can read the response and check off the list. **Concrete done.**

## Worked example

**Bad prompt:**

> me: write me a method that finds the kingdom with the most gold for a given user

The AI invents `Kingdom`, uses LINQ on a `List<Kingdom>`, ignores your store. Output is unrelated.

**Better:**

> me: in `KingdomEfStore.cs`, add `LoadRichest(string ownerSub): KingdomSlotInfo?` that returns the user's slot with the highest gold (or null if no kingdoms). Match the projection style of `ListSlots`. Add one test in `SlotCrudTests.cs` that creates 3 kingdoms with different gold and asserts the right id comes back. Don't load full Kingdom entities — project to summary inline.

The AI writes the right method, in the right style, in the right file. **Three minutes of typing the prompt; saves ten of cleanup.**

## Three sentences that fit most prompts

If the template feels heavy, fall back to three sentences:

1. **In `<file>`, add `<methodSignature>` that does `<one sentence>`.**
2. **Match the style of `<existing method or example>`.**
3. **Don't `<known trap>`.**

Three sentences land 80% of small asks. Use the full template for anything bigger than ~30 lines.

## Tinker

- Take a recent vague prompt you used. Rewrite it as a scoped prompt. Send both. Compare outputs.
- Pick the worst output you've gotten in the last month. Diagnose: was it scaffolding, scoping, or eval that failed?
- Add 2 more example files to `ai-context/examples/`. Reference them in your next 5 scoped prompts.

## Name it

- **Scoping** — per-task framing.
- **Non-goal** — explicit "don't do this."
- **Trap** — known pitfall.
- **Success criterion** — how to judge "done."
- **The 3-sentence prompt** — fallback for small asks.

## The rule of the through-line

> **The prompt is a contract.** State the goal, the limits, the proof of done. The AI delivers against the contract you wrote — vague contract, vague delivery.

## Quiz / challenge

Open `quiz.md`.

## Connect

B2.4 covers **reading the output critically** — the eval step. The other half of doing this well.
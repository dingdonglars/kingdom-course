# STYLE.md — Voice & Lesson Template

> The voice and structure every lesson follows. Author against this. Reviewer (Lars or `/lesson-review`) checks against this.

## Voice

- **Conversational, witty, never academic.** Think Fireship video over textbook.
- **Address the reader as "you."** Never by name.
- **Code samples have personality.** `string heroName = "Sir Bravo the Bold";` — not `string s = "x";`. Generic but evocative placeholder names.
- **Use the learner's world.** Roblox, gaming, tycoon vocabulary. The kingdom is theirs to name and shape.
- **Funny but not patronising.** Self-aware humour, never condescension.
- **Celebrate explicitly** at each milestone. Even an ASCII trophy + one-line *"you just did X. Six weeks ago you'd have called this magic."*
- **Honesty over perfection.** When something is hard, say it's hard. When a tool sucks at something, say so.

## Lesson template (every lesson, every time)

Six beats:

1. **Hook** — one line + screenshot/output preview of what you'll make.
2. **Do it** — paste, run, see it work.
3. **Tinker** — modify it, break it on purpose, fix it.
4. **Name it** — *now* we name the concept you just used.
5. **Quiz / challenge** — markdown MCQs (in `quiz.md`) or `dotnet test` green check-marks.
6. **Connect** — one line on how this serves the Kingdom.

## Words to watch — the per-lesson sidebar

When a lesson introduces new terms (technical or just unusual English), include a short sidebar at the top of the lesson:

```markdown
> **Words to watch**
> - **encapsulation** — keeping a class's internal data hidden behind methods
> - **constructor** — the special method that runs when a new object is created
> - **`new`** — the C# keyword that calls a constructor
```

Three to eight items. One line each. Every entry sources into `GLOSSARY.md`.

## Idiom callouts — when an idiom carries weight

Cultural idioms that the learner will encounter again get a callout:

```markdown
> 💬 **Idiom — "rubber duck debugging":** explain your code aloud to an inanimate object. The act of explaining surfaces the bug.
```

Don't strip useful idioms; don't smuggle in disposable ones.

## Pronunciation footnotes — first time only

For unguessable terms, give the pronunciation on first appearance:

```markdown
*OAuth* (oh-auth) is a way for one application to delegate identity to another.
```

Once per term, never again.

## Plain-language target

Course prose targets approximately **CEFR B1–B2** — short sentences, present tense, second person, no academic register, no needless idioms. The standard exists so difficult *language* doesn't block difficult *concepts*. (Full plain-language convention in `STANDARDS.md`.)

## Diff between phases

- **Phase 0** voice is highest-energy, most casual. Spark Week is meant to be fun.
- **Phases 1–4** maintain warmth and humour but introduce more vocabulary and rigour as the learner can handle it.
- **Phase 5** — capstone tone. Reflective, *"look how far you've come."*
- **Block 8 bonuses** — short, punchy. *"This should feel anticlimactic — that IS the lesson."*

## Per-milestone ritual

Every milestone (M0–M6) ends with three things, called out at the end of the milestone module:

1. **`journal/wins.md` entry** in the learner's repo — one paragraph in their own words.
2. **`#wins` Slack post** — link to the PR + screenshot + one-line caption.
3. **Before/after one-liner** — *"Six weeks ago I didn't know what a class was. Today I shipped a multi-user API."*

The lesson markdown ends with: *"You just shipped Mn. Time for the ritual: `wins.md` entry, `#wins` post, before/after one-liner. Then take the rest of the day off."*

## What every lesson file contains

```
phase-N-name/N.M-module-slug/
├─ lesson.md          # The six-beat lesson per template above
├─ starter/           # (optional) Code to paste / start from
├─ quiz.md            # 3–5 MCQs
├─ quiz-answers.md    # Separate file — no spoilers
└─ screenshots/       # (optional) Per type rules in spec §20
```

## What block-opener READMEs contain

Per spec §5.2 — see template at `STYLE.md` Appendix A below.

---

## Appendix A — Block-Opener Template

```markdown
# Block N — <Block Name>

> <One-line pitch — the brag, in your vocabulary>

**What you'll have at the end:** <The artefact — concrete, demonstrable, friend-shareable>

**Estimated effort:** <N weeks at 4–6 hrs/week>  ·  **Wraps:** <Milestone, e.g. M4>

---

## Why this block?

<2–3 sentences. Honest about what's hard and what's worth it. No hype.>

## What you'll learn (named)

- <Concept 1 — in your vocabulary>
- <Concept 2>
- <Concept 3 — three to five bullets max>

## What you'll build (the climb)

<3–5 bullets describing the journey, not the modules. Modules are the implementation; this is the story.>

## Brag-worthy outcome

<One sentence describing what you can show someone at the end. Specific.>

---

[**→ Start the first module**](./first-module-link)
```
</content>
</invoke>
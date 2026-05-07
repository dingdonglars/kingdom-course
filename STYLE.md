# STYLE.md — Voice & Lesson Template

> The voice and structure every lesson follows. Author against this. Reviewer (Lars or `/lesson-review`) checks against this.
> The two pilot lessons are the working examples: `phase-0-spark/0.0.8-roast-o-matic/lesson.md` and `phase-1-console-kingdom/1.4-game-loop/lesson.md`.

## Voice

- **Conversational, never academic.** A friend who happens to know things, talking you through them. Never a textbook, never a hype reel.
- **Address the reader as "you."** Never by name.
- **Code samples have personality.** `string heroName = "Sir Bravo the Bold";` — not `string s = "x";`. Generic but evocative placeholder names.
- **Use the learner's world.** Roblox, gaming, tycoon vocabulary. The kingdom is theirs to name and shape.
- **Honesty over perfection.** When something is hard, say it's hard. When a tool sucks at something, say so. When a rule has a trap, name the trap.
- **No `Hook:` openers.** Lessons open with a paragraph that does the work without announcing itself. If a lesson opens well, the reader is pulled in. If it has to be labelled "Hook," it isn't pulling.
- **No celebration theatre.** Quiet competence over confetti. Milestones get their own celebration ritual; modules don't need one.

## What every lesson file contains

```
phase-N-name/N.M-module-slug/
├── lesson.md            # The lesson — see structure below
├── starter/             # Code to paste or build from (optional)
└── quiz.md              # 3-7 MCQs in bullet format
```

**Quiz answers do NOT live in the course repo.** They go in `kingdom-curriculum/quiz-answers/<mirrored-path>/quiz-answers.md`. Mentor-side only.

## Lesson structure (every lesson, every time)

Each lesson runs through these sections, in this order:

1. **`# Module N.M — Title`** — heading, no preamble before it.
2. **Conversational opening paragraph** — one or two paragraphs that pull the reader in by setting up *what we'll do today and why it matters.* No `Hook:` callout. The opener IS the hook; naming it would be a tell.
3. **`> **Words to watch**`** — block-quoted sidebar listing 3-8 new terms with one-line definitions. Every entry sources into `GLOSSARY.md`. Skip when a lesson introduces no new vocabulary.
4. **The body** — typically `## Step 1`, `## Step 2`, etc. Each step is *a thing the reader does*: paste this, run that, observe the result. Code blocks live here. Prose around the code explains *why*, not just *what*.
5. **`## Tinker`** — short prose paragraphs (not bullet lists of prose) inviting the reader to play with what they just did. Ideas to try; what to break on purpose; what to notice. Optional but common.
6. **`## What you just did`** — recap section. **Required, every lesson.** Format:
   - One narrative paragraph (~5 sentences) tying the lesson's promise to the concepts it landed; include a numeric proof point if one fits naturally (test count, line count, etc.).
   - Then a `**Key concepts you can now name:**` block with 3-5 bullet phrases (no full sentences; bolded terms; pair concept with 5-word essence). Bullets do *not* repeat the glossary section's definitions verbatim — they pair the concept with a short reminder of *what it does in this lesson*.
7. **`## Quiz`** — points to `quiz.md` plus the closing line:
   > Open `quiz.md`. When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.
   For lessons without a quiz, use `## Wrap up` instead of `## Quiz` and no closing line.
8. **`## Next`** — one or two sentences pointing at the next module, naming what it builds on.

For milestone-closing lessons (Module 0.4, Module 0.8, Module 1.10, Module 2.11, Module 3.9, Module 4.7, Module 5.8, B1.4, B2.5), step 7 also includes the per-milestone ritual block (see "Per-milestone ritual" below).

## Quiz file structure

Every `quiz.md` follows this pattern:

```markdown
# Quiz — Module N.M

> Don't write your answers in this file — open `journal/quiz-notes.md` and write them there.

(Optional: a one-line setup sentence if the quiz needs context.)

## 1. Question?

- **a.** First option
- **b.** Second option
- **c.** Third option
- **d.** Fourth option

## 2. Question?

- **a.** ...
- **b.** ...
(etc.)

---

When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.
```

3-7 questions. Distractors are roughly the same length as the right answer — never give the answer away by length. The full *explanation* of why the right answer is right lives in the mentor answer file, not in the quiz.

## Mentor answer file structure

Lives at `kingdom-curriculum/quiz-answers/<mirrored-path>/quiz-answers.md`. Three pieces per question:

```markdown
## 1. Right answer: b

**Key point:** One sentence that names what the question is testing.

**If Athos picked a, c, or d:** Concrete reinforcement move for the weekly sync — what to demo, what to revisit, what question to ask back.
```

This file is mentor-facing throughout. Lars reads it before the weekly sync and walks through whichever questions Athos flagged in `journal/quiz-notes.md`.

## Words to watch — the per-lesson sidebar

When a lesson introduces new terms (technical or just unusual English), include a short sidebar at the top of the lesson:

```markdown
> **Words to watch**
>
> - **encapsulation** — keeping a class's internal data hidden behind methods
> - **constructor** — the special method that runs when a new object is created
> - **`new`** — the C# keyword that calls a constructor
```

Three to eight items. One line each. Every entry sources into `GLOSSARY.md`.

## Idiom callouts — when an idiom carries weight

Cultural idioms the learner will encounter again get a callout the first time they appear:

```markdown
> 💬 **"rubber duck debugging":** explain your code aloud to an inanimate object. The act of explaining surfaces the bug.
```

Don't strip useful idioms; don't smuggle in disposable ones.

## Pronunciation footnotes — first time only

For unguessable terms, give the pronunciation on first appearance:

```markdown
*OAuth* (oh-auth) is a way for one application to let another verify the user.
```

Once per term, never again.

## Vocabulary discipline (what to avoid in Athos-facing prose)

The full list — three tiers — lives in `STANDARDS.md` under *Vocabulary in course prose*. Headlines:

- **Avoid in Athos-facing prose** — words like *substrate, brevity, capstone, gate, hook, anti-climax, AI-rot, leverage, shape, bar, ...* These are AI-product or jargon vocabulary that costs more than it earns. Full list in STANDARDS, grows as words surface.
- **Explain on first use** — developer-profession terms (*shell, DTO, ORM, migration, idempotent, ...*). Always introduce in one sentence the first time the term appears in lesson prose, not just in the sidebar. After that, use freely.
- **Mentor-side say-out-loud rephrase** — words that are fine *in writing* in mentor-side files (answer keys, journal) but land badly *spoken* during the weekly sync. Mentor mentally rephrases. List in STANDARDS.

## Plain-language target

Course prose targets approximately **CEFR B1–B2** — short sentences, present tense, second person, no academic register, no needless idioms. The standard exists so difficult *language* doesn't block difficult *concepts*. The full plain-language convention is in `STANDARDS.md`.

## Voice across phases

- **Phase 0** is highest-energy, most casual. Spark Week is meant to be fun.
- **Phases 1–4** maintain warmth but introduce more vocabulary and rigour as the learner can handle it.
- **Phase 5** is reflective. *"Look how far you've come."*
- **Phase 6 bonuses** are short and punchy. The B1 lesson explicitly notes its own brevity is the lesson.

## Per-milestone ritual

Every milestone (M0–M6) ends with three things, called out at the end of the milestone module:

1. **`journal/wins.md` entry** in the learner's repo — one paragraph in his own words.
2. **`#wins` Slack post** — link to the PR + screenshot + one-line caption.
3. **Before/after one-liner** — *"Six weeks ago I didn't know what a class was. Today I shipped a multi-user API."*

The lesson markdown ends with a clear pointer to the ritual:
> *You just shipped Mn. Time for the ritual: `wins.md` entry, `#wins` post, before/after one-liner. Then take the rest of the day off.*

## Per-quiz ritual

Every lesson with a quiz feeds into a smaller, lighter ritual: Athos jots his answers + a sentence of reasoning in `journal/quiz-notes.md`. Lars consults the answer key in `kingdom-curriculum/quiz-answers/` before the weekly sync. They walk through any flagged questions at sync. See `MENTOR-PROTOCOL.md` *"Per-quiz ritual"* section for the full flow.

## Code style in lessons

Code samples follow `STANDARDS.md`. The two rules that affect lesson layout most:

- **Fluent / LINQ chains break before the dot at 3+ methods or >80 chars.** One method per line, dot leading. Reads like a story; easy to scan; easy to comment out one step.
- **Code samples have personality.** `string heroName = "Sir Bravo the Bold";` over `string s = "x";`. Concrete names land better than abstract ones.

## What phase-opener READMEs contain

Phase openers (`phase-N-name/README.md`) describe what's coming for the phase. Template at the bottom of this file (Appendix A).

---

## Appendix A — Phase-Opener Template

```markdown
# Phase N — <Phase Name>

> <One-line pitch — the brag, in plain language>

**What you'll have at the end:** <The thing — concrete, demonstrable, friend-shareable>

**Estimated effort:** <N weeks at 4–6 hrs/week>  ·  **Wraps:** <Milestone, e.g. M4>

---

## Why this phase?

<Two or three sentences of plain prose. Honest about what's hard and what's worth it. No hype, no `Hook:`.>

## What you'll learn (named)

- <Concept 1 — in plain words>
- <Concept 2>
- <Concept 3 — three to five bullets max>

## What you'll build (the climb)

<3–5 bullets describing the journey week by week. Modules are the implementation; the bullets are the story.>

## Brag-worthy outcome

<One or two sentences describing what you can show someone at the end. Specific.>

---

[**→ Start the first module**](./first-module-link)
```

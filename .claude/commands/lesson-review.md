---
description: Light end-of-lesson sanity check. Flags issues, doesn't write fixes.
---

You are running the `/lesson-review` slash command. The learner has just finished a non-trivial course lesson. Your job is a **light sanity check** — surface issues, do not write fixes.

## What to check

1. **Code quality** — naming follows `STANDARDS.md`; no obvious dead code; reasonable structure.
2. **Engine vs shell separation** — engine code (domain) doesn't depend on shell code (console, API, frontend). Same direction; never the reverse.
3. **Naming concerns** — methods are verbs, classes are nouns, booleans are questions. Specific beats general.
4. **Missed concepts** — concepts the lesson introduced but the learner didn't actually use in the implementation.
5. **Glossary currency** — any new term in the lesson body that isn't yet in `course/GLOSSARY.md`. Any glossary entry from this lesson with no body reference.
6. **Test coverage of the lesson's headline behavior** — if the lesson taught a new feature, is there at least one test for it?

## What NOT to do

- Do **not** write fixes. If you find an issue, describe it and *let the learner fix it*.
- Do **not** introduce concepts the learner hasn't met yet. Calibrate to the current phase per `ai-context/CLAUDE.md` curriculum context.
- Do **not** rewrite naming. Suggest renames; let the learner do them with `F2`.

## Output format

A short markdown checklist:

```markdown
# Lesson review — `<lesson identifier>`

## Findings

- [ ] **<short title>** (severity: low / medium / high)
  - Where: `path/to/file.cs:line`
  - What: <description>
  - Suggested next step: <action the learner takes>

## Glossary check

- New terms found in lesson body, not yet in GLOSSARY.md: `<list, or "none">`
- Glossary entries from this lesson with no body reference: `<list, or "none">`

## Overall

<one sentence — "looks good", "needs naming pass", "engine-shell leak — fix before milestone", etc.>
```

## When in doubt

If you're unsure whether something is a real issue, surface it as low-severity and let the learner decide.

---
description: Explain code line by line in plain English. Stops at confusing bits.
---

You are being invoked via `/walk-through-code`. The learner wants to understand a piece of code line-by-line — their own, the reference repo's, or something they found online.

Read `CLAUDE.md` for the bucket policy and the current curriculum phase. Walking through code is in the green bucket — pure learning, always fine.

## What to do

If the learner pasted code (or a file path) in `$ARGUMENTS`, use that. Otherwise ask: *"Paste the code (or give me a file path) you'd like me to walk through."*

Then explain it **line by line** in plain English. Specifically:

- **One line at a time.** Quote the line, then explain what it does in plain language.
- **No high-level summary.** If they wanted a summary, they'd have asked for `/explain-this-concept`. They asked for *walkthrough*.
- **Stop at any line that's likely to surprise a beginner** and explain *why it's written that way* — not just what it does. The surprises are where the learning is.
- **Stay calibrated.** Use vocabulary at or below the learner's current curriculum phase (`CLAUDE.md`). If a line uses a feature they haven't met yet, name the feature and say *"this is X — you'll meet it formally in module Y; for now treat it as Z"*.

## Style

- Plain English. Gaming and everyday analogies welcome.
- Where the same idiom appears in multiple lines, you can refer back: *"line 7 does the same thing as line 3 but for a different field."*
- Avoid the phrase "as you can see" — they couldn't, that's why they asked.

## End with

After the walkthrough, ask: *"Which line is the most surprising or confusing? I'll go deeper on that one."* The follow-up is where the lesson lands.

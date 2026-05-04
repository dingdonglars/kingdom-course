---
description: Beginner-level walkthrough of a concept. Calibrates to what you already know.
---

You are being invoked via `/explain-this-concept`. The learner wants a beginner-level walkthrough of a concept — calibrated to what they already understand.

Read `ai-context/CLAUDE.md` for the bucket policy and the current curriculum phase. Stay inside the green/yellow buckets — explanation is fine.

## What to do

If the learner gave you a topic in the command (`$ARGUMENTS`), use it as the concept. Otherwise, ask: *"What concept would you like me to walk through?"*

Then, before answering, ask the learner these in one combined message:

1. **What have you already read or tried about this?** (so I don't repeat what you've seen)
2. **What part do you think you understand?** (so I can skip it)
3. **What part is confusing?** (so I focus there)
4. **What related concepts have you already met?** (so I can lean on them)

Once you have those answers, give a beginner-level explanation that:

- Skips what they already understand.
- Focuses on the gap they named.
- Uses **one small concrete example** in the language of their current phase (C#, TypeScript, or Luau — check `ai-context/CLAUDE.md` curriculum context).
- Stays at or below the level of concepts they've met. If you need a concept they haven't met yet, name it as *"we'll meet this later"* rather than diving in.

Don't pontificate. One small example beats a paragraph of abstraction.

## When the concept is *itself* a course exercise

If the concept they're asking about is the *exact thing the current lesson teaches them to build*, push back gently: *"Walk me through what you've tried first — explaining the concept now would replace the lesson. I'll help you reason about what you've got."* This stays inside the red bucket per `ai-context/CLAUDE.md`.

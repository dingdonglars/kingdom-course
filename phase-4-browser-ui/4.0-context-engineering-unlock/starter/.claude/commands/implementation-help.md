---
description: Post-Unlock implementation help. Asks for goal/where/conventions/traps before writing code.
---

You are being invoked via `/implementation-help`. The learner is past the AI Unlock and is asking you to write non-trivial code. Read `ai-context/CLAUDE.md` and `STANDARDS.md` first.

If the learner already pasted the goal in `$ARGUMENTS`, use it. Otherwise ask for these in one combined message:

1. **Goal** — one sentence on what the code needs to do.
2. **Where** — file path + a snippet of the surrounding code.
3. **Existing patterns to match** — one or two small snippets from nearby methods.
4. **Conventions to follow** — relevant `STANDARDS.md` sections plus any project-specific quirks (link to them).
5. **Traps** — what *not* to do (e.g. *"don't use `new Random()` — use `IRandom`"*).

Once you have all five, write the implementation. Match the style. Stay inside the named conventions. Don't invent APIs that aren't visible in the code the learner showed you.

End your response with: *"Before you keep this, walk me through what each line does. If you can't explain a line, ask me about it instead of keeping it."*

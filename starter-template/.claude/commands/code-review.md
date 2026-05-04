---
description: Find issues in your code, but don't write the fix. Yellow bucket.
---

You are being invoked via `/code-review`. The learner wants you to review their code — point out issues, but **do not write the fix**. The reviewing IS the lesson.

Read `CLAUDE.md` for the bucket policy and the project's `STANDARDS.md`. This command stays inside the yellow bucket: friction help, not learning replacement.

## What to do

If the learner pasted code in `$ARGUMENTS` or pointed at a file path, use that. Otherwise ask: *"Paste the code (or give me a file path) you'd like me to review."*

Review for:

1. **Bugs** — anything that wouldn't behave the way the variable names imply.
2. **Naming** — methods are verbs, classes are nouns, booleans are questions; specific beats general; `STANDARDS.md` rules.
3. **Style** — formatting, redundant code, dead code.
4. **Structure** — is the file doing one thing or three? Is the engine vs shell line clean?
5. **Edge cases** — null inputs, empty collections, off-by-one.

For each issue:

- **Where** — file and line if you have it.
- **What's wrong** — one sentence.
- **Why it matters** — one sentence.

Group findings by severity (high / medium / low / nit).

## What NOT to do

- **Do not write the corrected code.** Even partial corrected snippets count. Describe what the fix should change, not what it should look like.
- **Do not refactor.** A code-review is a list of findings, not a rewrite.
- **Do not invent issues.** If the code is fine, say so — "no findings" is a valid output.

## End with

After the findings, ask: *"Want me to explain any of these in more depth (still without writing the fix)? Pick one and I'll go deeper."*
